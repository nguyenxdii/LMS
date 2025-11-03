// LMS.GUI/CustomerAdmin/ucCustomerSearch_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.CustomerAdmin
{
    public partial class ucCustomerSearch_Admin : UserControl
    {
        // báo id khách hàng được chọn
        public event EventHandler<int> CustomerPicked;

        private readonly CustomerService _customerSvc = new CustomerService();
        private BindingList<Customer> _bindingList;
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        // debounce tìm kiếm
        private readonly Timer _debounceTimer = new Timer { Interval = 350 };

        // kéo thả form cha qua panel top
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public ucCustomerSearch_Admin()
        {
            InitializeComponent();
            this.Load += UcCustomerSearch_Admin_Load;
            _debounceTimer.Tick += DebounceTimer_Tick;
        }

        private void UcCustomerSearch_Admin_Load(object sender, EventArgs e)
        {
            if (this.lblTitle != null) this.lblTitle.Text = "Tìm Kiếm Khách Hàng";

            ConfigureGrid();
            WireEvents();
            DoSearch();
            txtFullName.Focus();
        }

        // cấu hình lưới kết quả
        private void ConfigureGrid()
        {
            var g = dgvSearchResults;
            g.Columns.Clear();
            g.ApplyBaseStyle();

            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "Tên Khách Hàng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 30,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Phone",
                DataPropertyName = "Phone",
                HeaderText = "Số Điện Thoại",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                DataPropertyName = "Email",
                HeaderText = "Email",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Address",
                DataPropertyName = "Address",
                HeaderText = "Địa Chỉ",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 50,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
        }

        // gắn sự kiện điều khiển
        private void WireEvents()
        {
            btnReset.Click += BtnReset_Click;

            // lọc theo text thay đổi (debounce để tránh giật)
            txtFullName.TextChanged += FilterControl_Changed;
            txtPhone.TextChanged += FilterControl_Changed;
            txtEmail.TextChanged += FilterControl_Changed;
            txtAddress.TextChanged += FilterControl_Changed;

            // lưới
            dgvSearchResults.ColumnHeaderMouseClick += dgvSearchResults_ColumnHeaderMouseClick;
            dgvSearchResults.CellDoubleClick += dgvSearchResults_CellDoubleClick;

            // kéo thả panel top
            if (this.pnlTop != null)
            {
                this.pnlTop.MouseDown += PnlTop_MouseDown;
                this.pnlTop.MouseMove += PnlTop_MouseMove;
                this.pnlTop.MouseUp += PnlTop_MouseUp;
            }
        }

        // khi thay đổi filter -> khởi động debounce
        private void FilterControl_Changed(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        // hết debounce -> thực hiện tìm
        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            DoSearch();
        }

        // thực hiện tìm kiếm theo các ô lọc
        private void DoSearch()
        {
            try
            {
                string nameFilter = txtFullName.Text.Trim();
                string phoneFilter = txtPhone.Text.Trim();
                string emailFilter = txtEmail.Text.Trim();
                string addressFilter = txtAddress.Text.Trim();

                var results = _customerSvc.SearchCustomersForAdmin(
                    string.IsNullOrWhiteSpace(nameFilter) ? null : nameFilter,
                    string.IsNullOrWhiteSpace(phoneFilter) ? null : phoneFilter,
                    string.IsNullOrWhiteSpace(emailFilter) ? null : emailFilter,
                    string.IsNullOrWhiteSpace(addressFilter) ? null : addressFilter
                );

                _bindingList = new BindingList<Customer>(results);
                dgvSearchResults.DataSource = _bindingList;

                ApplySort();
                UpdateSortGlyphs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi Khi Tìm Kiếm Khách Hàng: {ex.Message}", "Lỗi");
                dgvSearchResults.DataSource = null;
            }
        }

        // reset ô lọc và nạp lại danh sách
        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtFullName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();

            ResetSortGlyphs();
            DoSearch();
            txtFullName.Focus();
        }

        // double-click để chọn khách hàng
        private void dgvSearchResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                if (dgvSearchResults.Rows[e.RowIndex].DataBoundItem is Customer selectedCustomer)
                {
                    CustomerPicked?.Invoke(this, selectedCustomer.Id);
                    this.FindForm()?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi Khi Chọn Khách Hàng: {ex.Message}", "Lỗi");
            }
        }

        // sắp xếp theo cột khi click tiêu đề
        private void dgvSearchResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;

            var newColumn = dgvSearchResults.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn)
            {
                _sortOrder = (_sortOrder == SortOrder.Ascending)
                    ? SortOrder.Descending
                    : SortOrder.Ascending;
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

        // áp dụng sắp xếp trên danh sách đang bind
        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;

            string propertyName = _sortedColumn.DataPropertyName;
            var propInfo = typeof(Customer).GetProperty(propertyName);
            if (propInfo == null) return;

            var items = _bindingList.ToList();
            var sortedList = (_sortOrder == SortOrder.Ascending)
                ? items.OrderBy(x => propInfo.GetValue(x, null)).ToList()
                : items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

            _bindingList = new BindingList<Customer>(sortedList);
            dgvSearchResults.DataSource = _bindingList;
        }

        // cập nhật icon mũi tên sắp xếp
        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvSearchResults.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;

                if (_sortedColumn != null && col == _sortedColumn)
                    col.HeaderCell.SortGlyphDirection = _sortOrder;
            }
        }

        // reset trạng thái sắp xếp
        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null)
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;

            _sortedColumn = null;
            _sortOrder = SortOrder.None;
            UpdateSortGlyphs();
        }

        // kéo thả form cha qua pnlTop
        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            var parentForm = this.FindForm(); if (parentForm == null) return;
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = parentForm.Location;
            }
        }

        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            var parentForm = this.FindForm(); if (parentForm == null) return;
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                parentForm.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) dragging = false;
        }
    }
}
