using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.AccountAdmin
{
    public partial class ucAccountSearch_Admin : UserControl
    {
        private readonly UserAccountService_Admin _svc = new UserAccountService_Admin();
        public event EventHandler<int> AccountSelected; // báo id tài khoản được chọn

        private readonly Timer _debounce = new Timer { Interval = 300 };

        // dữ liệu + trạng thái sắp xếp
        private BindingList<object> _bindingList;
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        // kéo thả form chứa usercontrol (khi form borderless)
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public ucAccountSearch_Admin()
        {
            InitializeComponent();
            this.Load += UcAccountSearch_Admin_Load;
        }

        private void UcAccountSearch_Admin_Load(object sender, EventArgs e)
        {
            // lưu ý: form cha nên để FormBorderStyle = None nếu muốn kéo thả
            if (this.lblTitle != null)
            {
                this.lblTitle.Text = "Tìm Kiếm Tài Khoản";
            }

            ConfigGrid();
            LoadFilters();
            LoadAll();
            Wire();
        }

        // cấu hình lưới kết quả
        private void ConfigGrid()
        {
            dgvResults.Columns.Clear();
            dgvResults.ApplyBaseStyle(); // style helper riêng

            dgvResults.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                Visible = false
            });

            dgvResults.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Username",
                DataPropertyName = "Username",
                HeaderText = "Tài khoản",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            dgvResults.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Role",
                DataPropertyName = "Role",
                HeaderText = "Vai trò",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            dgvResults.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PersonName",
                DataPropertyName = "PersonName",
                HeaderText = "Họ tên",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            dgvResults.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LinkedTo",
                DataPropertyName = "LinkedTo",
                HeaderText = "Liên kết",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            dgvResults.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StatusText",
                DataPropertyName = "IsActive",
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter },
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            // sự kiện lưới
            dgvResults.CellDoubleClick += DgvResults_CellDoubleClick;
            dgvResults.CellFormatting += DgvResults_CellFormatting;
            dgvResults.RowPrePaint += DgvResults_RowPrePaint;
            dgvResults.ColumnHeaderMouseClick += DgvResults_ColumnHeaderMouseClick;
        }

        // format hiển thị vai trò + trạng thái
        private void DgvResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var colName = dgvResults.Columns[e.ColumnIndex].Name;

            if (colName == "StatusText" && e.Value is bool isActive)
            {
                e.Value = isActive ? "Hoạt động" : "Đã khóa";
                e.FormattingApplied = true;
            }
            else if (colName == "Role" && e.Value is UserRole role)
            {
                switch (role)
                {
                    case UserRole.Customer: e.Value = "Khách hàng"; break;
                    case UserRole.Driver: e.Value = "Tài xế"; break;
                    default: e.Value = role.ToString(); break;
                }
                e.FormattingApplied = true;
            }
        }

        // đổi màu dòng nếu bị khóa
        private void DgvResults_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvResults.Rows[e.RowIndex];
            dynamic it = row.DataBoundItem;
            if (it == null) return;

            try
            {
                bool active = (bool)it.IsActive;
                row.DefaultCellStyle.ForeColor = active ? Color.Black : Color.Gray;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in RowPrePaint: {ex.Message}");
            }
        }

        // gắn sự kiện vào control
        private void Wire()
        {
            btnReload.Click += BtnReload_Click;

            txtUser.TextChanged += Filter_InputChanged;
            txtName.TextChanged += Filter_InputChanged;
            cmbRole.SelectedIndexChanged += Filter_InputChanged;
            cmbStatus.SelectedIndexChanged += Filter_InputChanged;

            _debounce.Tick += Debounce_Tick;

            // cho phép kéo thả bằng panel top nếu tồn tại
            if (this.pnlTop != null)
            {
                this.pnlTop.MouseDown += PnlTop_MouseDown;
                this.pnlTop.MouseMove += PnlTop_MouseMove;
                this.pnlTop.MouseUp += PnlTop_MouseUp;
            }
        }

        // nạp giá trị lọc
        private void LoadFilters()
        {
            // vai trò (ẩn admin)
            var roleItems = new BindingList<object>
            {
                new { Value = (UserRole?)null,           Text = "— Tất Cả Vai Trò —" },
                new { Value = (UserRole?)UserRole.Customer, Text = "Khách Hàng" },
                new { Value = (UserRole?)UserRole.Driver,   Text = "Tài Xế" }
            };
            cmbRole.DataSource = roleItems;
            cmbRole.DisplayMember = "Text";
            cmbRole.ValueMember = "Value";
            cmbRole.SelectedIndex = 0;

            // trạng thái
            var statusItems = new BindingList<object>
            {
                new { Value = (bool?)null,  Text = "— Tất Cả Trạng Thái —" },
                new { Value = (bool?)true,  Text = "Hoạt Động" },
                new { Value = (bool?)false, Text = "Đã Khóa" }
            };
            cmbStatus.DataSource = statusItems;
            cmbStatus.DisplayMember = "Text";
            cmbStatus.ValueMember = "Value";
            cmbStatus.SelectedIndex = 0;
        }

        // debounce khi nhập/lọc
        private void Filter_InputChanged(object sender, EventArgs e)
        {
            _debounce.Stop();
            _debounce.Start();
        }

        // tick debounce -> thực hiện tìm
        private void Debounce_Tick(object sender, EventArgs e)
        {
            _debounce.Stop();
            DoSearch();
        }

        // nút tải lại / reset filter
        private void BtnReload_Click(object sender, EventArgs e)
        {
            cmbRole.SelectedIndexChanged -= Filter_InputChanged;
            cmbStatus.SelectedIndexChanged -= Filter_InputChanged;

            txtUser.Clear();
            txtName.Clear();
            cmbRole.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;

            cmbRole.SelectedIndexChanged += Filter_InputChanged;
            cmbStatus.SelectedIndexChanged += Filter_InputChanged;

            LoadAll();
        }

        // tải toàn bộ (ẩn admin)
        private void LoadAll()
        {
            try
            {
                var data = _svc.GetAll()
                    .Where(a => a.Role != UserRole.Admin)
                    .Select(a => new
                    {
                        a.Id,
                        a.Username,
                        a.Role,
                        PersonName = a.Customer != null
                            ? a.Customer.Name
                            : (a.Driver != null ? a.Driver.FullName : ""),
                        LinkedTo = a.CustomerId != null
                            ? $"KH: {(a.Customer?.Name ?? $"ID {a.CustomerId}")}"
                            : (a.DriverId != null
                                ? $"TX: {(a.Driver?.FullName ?? $"ID {a.DriverId}")}"
                                : "(N/A)"),
                        a.IsActive
                    })
                    .ToList();

                _bindingList = new BindingList<object>(data.Cast<object>().ToList());
                dgvResults.DataSource = _bindingList;

                ApplySort();
                UpdateSortGlyphs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvResults.DataSource = null;
            }
        }

        // thực hiện tìm kiếm theo bộ lọc
        private void DoSearch()
        {
            try
            {
                var usernameFilter = string.IsNullOrWhiteSpace(txtUser.Text) ? null : txtUser.Text.Trim();
                var nameFilter = string.IsNullOrWhiteSpace(txtName.Text) ? null : txtName.Text.Trim();

                // ép kiểu từ SelectedValue sang nullable
                UserRole? roleFilter = cmbRole.SelectedValue as UserRole?;
                bool? isActiveFilter = cmbStatus.SelectedValue as bool?;

                var data = _svc.GetAll(usernameFilter, nameFilter, roleFilter, isActiveFilter)
                    .Where(a => a.Role != UserRole.Admin)
                    .Select(a => new
                    {
                        a.Id,
                        a.Username,
                        a.Role,
                        PersonName = a.Customer != null
                            ? a.Customer.Name
                            : (a.Driver != null ? a.Driver.FullName : ""),
                        LinkedTo = a.CustomerId != null
                            ? $"KH: {(a.Customer?.Name ?? $"ID {a.CustomerId}")}"
                            : (a.DriverId != null
                                ? $"TX: {(a.Driver?.FullName ?? $"ID {a.DriverId}")}"
                                : "(N/A)"),
                        a.IsActive
                    })
                    .ToList();

                _bindingList = new BindingList<object>(data.Cast<object>().ToList());
                dgvResults.DataSource = _bindingList;

                ApplySort();
                UpdateSortGlyphs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // double click chọn tài khoản
        private void DgvResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                var id = (int)((dynamic)dgvResults.Rows[e.RowIndex].DataBoundItem).Id;
                AccountSelected?.Invoke(this, id);
                this.FindForm()?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // click header để sắp xếp
        private void DgvResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;

            var newColumn = dgvResults.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn)
            {
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                if (_sortedColumn != null)
                    _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;

                _sortOrder = SortOrder.Ascending;
                _sortedColumn = newColumn;
            }

            ApplySort();
            UpdateSortGlyphs();
        }

        // áp dụng sắp xếp trên bindinglist
        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;

            string propertyName = _sortedColumn.DataPropertyName;
            PropertyInfo propInfo = null;

            var firstItem = _bindingList.FirstOrDefault(item => item != null);
            if (firstItem != null)
                propInfo = firstItem.GetType().GetProperty(propertyName);

            if (propInfo == null) return;

            var items = _bindingList.ToList();
            try
            {
                var sortedList = (_sortOrder == SortOrder.Ascending)
                    ? items.OrderBy(x => propInfo.GetValue(x, null)).ToList()
                    : items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

                _bindingList = new BindingList<object>(sortedList);
                dgvResults.DataSource = _bindingList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi sắp xếp cột '{_sortedColumn.HeaderText}': {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetSortGlyphs();
            }
        }

        // cập nhật biểu tượng mũi tên sort
        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvResults.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;

                if (_sortedColumn != null && col == _sortedColumn)
                    col.HeaderCell.SortGlyphDirection = _sortOrder;
            }
        }

        // reset trạng thái sort
        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null)
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;

            _sortedColumn = null;
            _sortOrder = SortOrder.None;
            UpdateSortGlyphs();
        }

        // bắt đầu kéo form cha tại khu vực pnlTop
        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            var parentForm = this.FindForm();
            if (parentForm == null) return;

            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = parentForm.Location;
            }
        }

        // di chuyển form cha khi kéo
        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            var parentForm = this.FindForm();
            if (parentForm == null) return;

            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                parentForm.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        // kết thúc kéo
        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = false;
            }
        }
    }
}
