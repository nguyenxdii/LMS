//using Guna.UI2.WinForms;
//using LMS.BUS.Helpers; // Thêm using này nếu chưa có
//using LMS.BUS.Services; // Đảm bảo using này đúng
//using LMS.DAL.Models;   // Đảm bảo using này đúng
//using System;
//using System.Collections.Generic; // Cần cho List
//using System.ComponentModel;
//using System.Drawing;
//using System.Linq;
//using System.Reflection; // Cần cho Sorting
//using System.Windows.Forms;

//namespace LMS.GUI.AccountAdmin // Đảm bảo namespace này đúng
//{
//    public partial class ucAccountSearch_Admin : UserControl
//    {
//        private readonly UserAccountService_Admin _svc = new UserAccountService_Admin();
//        public event EventHandler<int> AccountSelected; // Sự kiện báo ID đã chọn

//        private readonly Timer _debounce = new Timer { Interval = 300 };

//        // --- Biến cho Sorting ---
//        private BindingList<object> _bindingList;
//        private DataGridViewColumn _sortedColumn = null;
//        private SortOrder _sortOrder = SortOrder.None;
//        // --- Kết thúc biến ---

//        public ucAccountSearch_Admin()
//        {
//            InitializeComponent();
//            this.Load += UcAccountSearch_Admin_Load;
//        }

//        private void UcAccountSearch_Admin_Load(object sender, EventArgs e)
//        {
//            ConfigGrid();
//            LoadFilters(); // Tải filter trước
//            LoadAll();     // Tải dữ liệu ban đầu
//            Wire();
//        }

//        #region Grid Configuration and Formatting
//        private void ConfigGrid()
//        {
//            dgvResults.Columns.Clear();
//            dgvResults.ApplyBaseStyle(); // Dùng helper style của bạn

//            // --- Định nghĩa cột (Thêm SortMode, Sửa cột Status) ---
//            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
//            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "Username", DataPropertyName = "Username", HeaderText = "Tài khoản", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "Role", DataPropertyName = "Role", HeaderText = "Vai trò", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "PersonName", DataPropertyName = "PersonName", HeaderText = "Họ tên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "LinkedTo", DataPropertyName = "LinkedTo", HeaderText = "Liên kết", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "StatusText", DataPropertyName = "IsActive", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }, SortMode = DataGridViewColumnSortMode.Programmatic });

//            // Gán sự kiện
//            dgvResults.CellDoubleClick += DgvResults_CellDoubleClick;
//            dgvResults.CellFormatting += DgvResults_CellFormatting;
//            dgvResults.RowPrePaint += DgvResults_RowPrePaint;
//            dgvResults.ColumnHeaderMouseClick += DgvResults_ColumnHeaderMouseClick; // Gán sự kiện sort
//        }

//        // Format chữ cho cột Role và StatusText
//        private void DgvResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            if (dgvResults.Columns[e.ColumnIndex].Name == "StatusText" && e.Value is bool isActive)
//            {
//                e.Value = isActive ? "Hoạt động" : "Đã khóa"; e.FormattingApplied = true;
//            }
//            else if (dgvResults.Columns[e.ColumnIndex].Name == "Role" && e.Value is UserRole role)
//            {
//                switch (role)
//                {
//                    case UserRole.Customer: e.Value = "Khách hàng"; break;
//                    case UserRole.Driver: e.Value = "Tài xế"; break;
//                    default: e.Value = role.ToString(); break;
//                }
//                e.FormattingApplied = true;
//            }
//        }
//        // Đổi màu chữ cho dòng bị khóa
//        private void DgvResults_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
//        {
//            if (e.RowIndex < 0) return; var row = dgvResults.Rows[e.RowIndex]; dynamic it = row.DataBoundItem; if (it == null) return;
//            try { bool active = (bool)it.IsActive; row.DefaultCellStyle.ForeColor = active ? Color.Black : Color.Gray; }
//            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Error in RowPrePaint: {ex.Message}"); } // Sử dụng ex
//        }
//        #endregion

//        #region Event Wiring and Filtering
//        private void Wire()
//        {
//            // Nút bấm
//            // btnSearch.Click += (s, e) => DoSearch(); // Bỏ nút Search riêng nếu dùng debounce
//            btnReload.Click += BtnReload_Click;
//            //btnClose.Click += (s, e) => this.FindForm()?.Close(); // Nút đóng form popup tìm kiếm

//            // Lọc tự động khi thay đổi text hoặc lựa chọn
//            txtUser.TextChanged += Filter_InputChanged;
//            txtName.TextChanged += Filter_InputChanged;
//            cmbRole.SelectedIndexChanged += Filter_InputChanged;
//            cmbStatus.SelectedIndexChanged += Filter_InputChanged;

//            // Timer Tick
//            _debounce.Tick += Debounce_Tick;
//        }

//        // Tải dữ liệu cho ComboBox filters
//        private void LoadFilters()
//        {
//            // ComboBox Vai trò (chỉ Customer, Driver)
//            var roleItems = new BindingList<object> { new { Value = (UserRole?)null, Text = "— Tất cả vai trò —" } };
//            roleItems.Add(new { Value = (UserRole?)UserRole.Customer, Text = "Khách hàng" });
//            roleItems.Add(new { Value = (UserRole?)UserRole.Driver, Text = "Tài xế" });
//            cmbRole.DataSource = roleItems; cmbRole.DisplayMember = "Text"; cmbRole.ValueMember = "Value"; cmbRole.SelectedIndex = 0;

//            // ComboBox Trạng thái
//            var statusItems = new BindingList<object> {
//                 new { Value = (bool?)null, Text = "— Tất cả trạng thái —" }, new { Value = (bool?)true, Text = "Hoạt động" }, new { Value = (bool?)false, Text = "Đã khóa" }
//            };
//            cmbStatus.DataSource = statusItems; cmbStatus.DisplayMember = "Text"; cmbStatus.ValueMember = "Value"; cmbStatus.SelectedIndex = 0;
//        }

//        // Hàm xử lý chung khi filter thay đổi (TextBox hoặc ComboBox)
//        private void Filter_InputChanged(object sender, EventArgs e)
//        {
//            _debounce.Stop(); // Dừng timer cũ
//            _debounce.Start(); // Bắt đầu đếm lại
//        }

//        // Hàm gọi khi timer debounce kết thúc
//        private void Debounce_Tick(object sender, EventArgs e)
//        {
//            _debounce.Stop(); // Dừng timer hiện tại
//            DoSearch(); // Thực hiện tìm kiếm/lọc
//        }

//        // Nút Tải lại / Reset Filter
//        private void BtnReload_Click(object sender, EventArgs e)
//        {
//            // Tạm ngưng xử lý sự kiện SelectedIndexChanged để tránh gọi DoSearch nhiều lần
//            cmbRole.SelectedIndexChanged -= Filter_InputChanged;
//            cmbStatus.SelectedIndexChanged -= Filter_InputChanged;

//            txtUser.Clear();
//            txtName.Clear();
//            cmbRole.SelectedIndex = 0; // Đặt lại về "Tất cả"
//            cmbStatus.SelectedIndex = 0; // Đặt lại về "Tất cả"

//            // Gán lại sự kiện
//            cmbRole.SelectedIndexChanged += Filter_InputChanged;
//            cmbStatus.SelectedIndexChanged += Filter_InputChanged;


//            LoadAll(); // Tải lại toàn bộ (đã lọc Admin và có sort)
//        }
//        #endregion

//        #region Data Loading and Searching
//        // Tải tất cả tài khoản (trừ Admin)
//        private void LoadAll()
//        {
//            try
//            {
//                var data = _svc.GetAll()
//                    .Where(a => a.Role != UserRole.Admin) // Lọc Admin
//                    .Select(a => new
//                    {
//                        a.Id,
//                        a.Username,
//                        a.Role,
//                        PersonName = a.Customer != null ? a.Customer.Name : (a.Driver != null ? a.Driver.FullName : ""),
//                        LinkedTo = a.CustomerId != null ? $"KH: {(a.Customer?.Name ?? $"ID {a.CustomerId}")}" : (a.DriverId != null ? $"TX: {(a.Driver?.FullName ?? $"ID {a.DriverId}")}" : "(N/A)"),
//                        a.IsActive
//                    })
//                    .ToList(); // Lấy dữ liệu

//                _bindingList = new BindingList<object>(data.Cast<object>().ToList());
//                dgvResults.DataSource = _bindingList; // Gán DataSource

//                ApplySort(); // Áp dụng sort (nếu có)
//                UpdateSortGlyphs(); // Cập nhật mũi tên
//            }
//            catch (Exception ex) // Sử dụng biến ex
//            {
//                MessageBox.Show($"Lỗi tải danh sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hiển thị lỗi
//                dgvResults.DataSource = null;
//            }
//            // Không cần finally ResetSortGlyphs() ở đây nữa
//        }

//        // Thực hiện tìm kiếm/lọc
//        private void DoSearch()
//        {
//            try
//            {
//                // Lấy giá trị filter từ controls
//                var usernameFilter = string.IsNullOrWhiteSpace(txtUser.Text) ? null : txtUser.Text.Trim();
//                var nameFilter = string.IsNullOrWhiteSpace(txtName.Text) ? null : txtName.Text.Trim();
//                // Lấy SelectedValue và ép kiểu sang nullable
//                UserRole? roleFilter = cmbRole.SelectedValue as UserRole?;
//                bool? isActiveFilter = cmbStatus.SelectedValue as bool?;

//                // Gọi service với filter, lọc Admin
//                var data = _svc.GetAll(usernameFilter, nameFilter, roleFilter, isActiveFilter)
//                    .Where(a => a.Role != UserRole.Admin) // Lọc Admin
//                    .Select(a => new
//                    {
//                        a.Id,
//                        a.Username,
//                        a.Role,
//                        PersonName = a.Customer != null ? a.Customer.Name : (a.Driver != null ? a.Driver.FullName : ""),
//                        LinkedTo = a.CustomerId != null ? $"KH: {(a.Customer?.Name ?? $"ID {a.CustomerId}")}" : (a.DriverId != null ? $"TX: {(a.Driver?.FullName ?? $"ID {a.DriverId}")}" : "(N/A)"),
//                        a.IsActive
//                    })
//                    .ToList(); // Lấy dữ liệu

//                _bindingList = new BindingList<object>(data.Cast<object>().ToList());
//                dgvResults.DataSource = _bindingList; // Gán DataSource

//                ApplySort(); // Áp dụng sort (nếu có)
//                UpdateSortGlyphs(); // Cập nhật mũi tên
//            }
//            catch (Exception ex) // Sử dụng biến ex
//            {
//                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hiển thị lỗi
//                // dgvResults.DataSource = null; // Có thể giữ kết quả cũ hoặc xóa đi tùy ý
//            }
//        }
//        #endregion

//        #region Grid Interaction (Double Click)
//        // Xử lý double click -> chọn và đóng form
//        private void DgvResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex >= 0) // Click vào dòng dữ liệu
//            {
//                try
//                {
//                    // Lấy ID từ đối tượng dữ liệu của dòng
//                    var id = (int)((dynamic)dgvResults.Rows[e.RowIndex].DataBoundItem).Id;
//                    // Kích hoạt sự kiện AccountSelected, gửi ID ra ngoài
//                    AccountSelected?.Invoke(this, id);
//                    // Đóng form tìm kiếm này lại
//                    this.FindForm()?.Close();
//                }
//                catch (Exception ex) // Sử dụng biến ex
//                {
//                    MessageBox.Show($"Lỗi khi chọn tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Hiển thị lỗi
//                }
//            }
//        }
//        #endregion

//        #region Sorting Logic (Đã hoàn chỉnh)
//        private void DgvResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            if (_bindingList == null || _bindingList.Count == 0) return;
//            var newColumn = dgvResults.Columns[e.ColumnIndex];
//            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

//            if (_sortedColumn == newColumn)
//            {
//                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
//            }
//            else
//            {
//                if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//                _sortOrder = SortOrder.Ascending;
//                _sortedColumn = newColumn;
//            }
//            ApplySort();
//            UpdateSortGlyphs();
//        }

//        private void ApplySort()
//        {
//            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;
//            string propertyName = _sortedColumn.DataPropertyName; PropertyInfo propInfo = null;
//            var firstItem = _bindingList.FirstOrDefault(item => item != null);
//            if (firstItem != null) propInfo = firstItem.GetType().GetProperty(propertyName);
//            if (propInfo == null) return;

//            List<object> items = _bindingList.ToList(); List<object> sortedList;
//            try
//            {
//                if (_sortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
//                else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }
//                _bindingList = new BindingList<object>(sortedList); dgvResults.DataSource = _bindingList;
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp cột '{_sortedColumn.HeaderText}': {ex.Message}"); ResetSortGlyphs(); }
//        }

//        private void UpdateSortGlyphs()
//        {
//            foreach (DataGridViewColumn col in dgvResults.Columns)
//            {
//                if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None;
//                if (_sortedColumn != null && col == _sortedColumn) { col.HeaderCell.SortGlyphDirection = _sortOrder; }
//            }
//        }
//        private void ResetSortGlyphs()
//        {
//            if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//            _sortedColumn = null; _sortOrder = SortOrder.None; UpdateSortGlyphs();
//        }
//        #endregion
//    }
//}

using Guna.UI2.WinForms;
using LMS.BUS.Helpers; // Thêm using này nếu chưa có
using LMS.BUS.Services; // Đảm bảo using này đúng
using LMS.DAL.Models;  // Đảm bảo using này đúng
using System;
using System.Collections.Generic; // Cần cho List
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection; // Cần cho Sorting
using System.Windows.Forms;

namespace LMS.GUI.AccountAdmin // Đảm bảo namespace này đúng
{
    public partial class ucAccountSearch_Admin : UserControl
    {
        private readonly UserAccountService_Admin _svc = new UserAccountService_Admin();
        public event EventHandler<int> AccountSelected; // Sự kiện báo ID đã chọn

        private readonly Timer _debounce = new Timer { Interval = 300 };

        // --- Biến cho Sorting ---
        private BindingList<object> _bindingList;
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;
        // --- Kết thúc biến ---

        // ==========================================================
        // === (1) THÊM CÁC BIẾN ĐỂ KÉO THẢ FORM CHỨA UC NÀY (GIỐNG ADMIN) ===
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        // ==========================================================

        public ucAccountSearch_Admin()
        {
            InitializeComponent();
            this.Load += UcAccountSearch_Admin_Load;
        }

        private void UcAccountSearch_Admin_Load(object sender, EventArgs e)
        {
            // !!! QUAN TRỌNG: Form chứa UC này cần có FormBorderStyle = None !!!
            // (Thường Form popup sẽ được thiết lập như vậy)

            if (this.lblTitle != null)
            {
                this.lblTitle.Text = "Tìm Kiếm Tài khoản";
            }

            ConfigGrid();
            LoadFilters(); // Tải filter trước
            LoadAll();     // Tải dữ liệu ban đầu
            Wire();
        }

        #region Grid Configuration and Formatting
        private void ConfigGrid()
        {
            dgvResults.Columns.Clear();
            dgvResults.ApplyBaseStyle(); // Dùng helper style của bạn

            // --- Định nghĩa cột (Thêm SortMode, Sửa cột Status) ---
            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "Username", DataPropertyName = "Username", HeaderText = "Tài khoản", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "Role", DataPropertyName = "Role", HeaderText = "Vai trò", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "PersonName", DataPropertyName = "PersonName", HeaderText = "Họ tên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "LinkedTo", DataPropertyName = "LinkedTo", HeaderText = "Liên kết", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "StatusText", DataPropertyName = "IsActive", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }, SortMode = DataGridViewColumnSortMode.Programmatic });

            // Gán sự kiện
            dgvResults.CellDoubleClick += DgvResults_CellDoubleClick;
            dgvResults.CellFormatting += DgvResults_CellFormatting;
            dgvResults.RowPrePaint += DgvResults_RowPrePaint;
            dgvResults.ColumnHeaderMouseClick += DgvResults_ColumnHeaderMouseClick; // Gán sự kiện sort
        }

        // Format chữ cho cột Role và StatusText
        private void DgvResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvResults.Columns[e.ColumnIndex].Name == "StatusText" && e.Value is bool isActive)
            {
                e.Value = isActive ? "Hoạt động" : "Đã khóa"; e.FormattingApplied = true;
            }
            else if (dgvResults.Columns[e.ColumnIndex].Name == "Role" && e.Value is UserRole role)
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
        // Đổi màu chữ cho dòng bị khóa
        private void DgvResults_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0) return; var row = dgvResults.Rows[e.RowIndex]; dynamic it = row.DataBoundItem; if (it == null) return;
            try { bool active = (bool)it.IsActive; row.DefaultCellStyle.ForeColor = active ? Color.Black : Color.Gray; }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Error in RowPrePaint: {ex.Message}"); } // Sử dụng ex
        }
        #endregion

        #region Event Wiring and Filtering
        private void Wire()
        {
            // Nút bấm
            // btnSearch.Click += (s, e) => DoSearch(); // Bỏ nút Search riêng nếu dùng debounce
            btnReload.Click += BtnReload_Click;
            //btnClose.Click += (s, e) => this.FindForm()?.Close(); // Nút đóng form popup tìm kiếm

            // Lọc tự động khi thay đổi text hoặc lựa chọn
            txtUser.TextChanged += Filter_InputChanged;
            txtName.TextChanged += Filter_InputChanged;
            cmbRole.SelectedIndexChanged += Filter_InputChanged;
            cmbStatus.SelectedIndexChanged += Filter_InputChanged;

            // Timer Tick
            _debounce.Tick += Debounce_Tick;

            // ==========================================================
            // === (2) GÁN SỰ KIỆN KÉO THẢ CHO PANEL TOP CỦA UC NÀY ===
            // !!! QUAN TRỌNG: Đảm bảo Panel trên cùng của bạn tên là 'pnlTop' !!!
            if (this.pnlTop != null) // Kiểm tra panel tồn tại
            {
                this.pnlTop.MouseDown += PnlTop_MouseDown;
                this.pnlTop.MouseMove += PnlTop_MouseMove;
                this.pnlTop.MouseUp += PnlTop_MouseUp;
            }
            // ==========================================================
        }

        // Tải dữ liệu cho ComboBox filters
        private void LoadFilters()
        {
            // ComboBox Vai trò (chỉ Customer, Driver)
            var roleItems = new BindingList<object> { new { Value = (UserRole?)null, Text = "— Tất cả vai trò —" } };
            roleItems.Add(new { Value = (UserRole?)UserRole.Customer, Text = "Khách hàng" });
            roleItems.Add(new { Value = (UserRole?)UserRole.Driver, Text = "Tài xế" });
            cmbRole.DataSource = roleItems; cmbRole.DisplayMember = "Text"; cmbRole.ValueMember = "Value"; cmbRole.SelectedIndex = 0;

            // ComboBox Trạng thái
            var statusItems = new BindingList<object> {
                     new { Value = (bool?)null, Text = "— Tất cả trạng thái —" }, new { Value = (bool?)true, Text = "Hoạt động" }, new { Value = (bool?)false, Text = "Đã khóa" }
            };
            cmbStatus.DataSource = statusItems; cmbStatus.DisplayMember = "Text"; cmbStatus.ValueMember = "Value"; cmbStatus.SelectedIndex = 0;
        }

        // Hàm xử lý chung khi filter thay đổi (TextBox hoặc ComboBox)
        private void Filter_InputChanged(object sender, EventArgs e)
        {
            _debounce.Stop(); // Dừng timer cũ
            _debounce.Start(); // Bắt đầu đếm lại
        }

        // Hàm gọi khi timer debounce kết thúc
        private void Debounce_Tick(object sender, EventArgs e)
        {
            _debounce.Stop(); // Dừng timer hiện tại
            DoSearch(); // Thực hiện tìm kiếm/lọc
        }

        // Nút Tải lại / Reset Filter
        private void BtnReload_Click(object sender, EventArgs e)
        {
            // Tạm ngưng xử lý sự kiện SelectedIndexChanged để tránh gọi DoSearch nhiều lần
            cmbRole.SelectedIndexChanged -= Filter_InputChanged;
            cmbStatus.SelectedIndexChanged -= Filter_InputChanged;

            txtUser.Clear();
            txtName.Clear();
            cmbRole.SelectedIndex = 0; // Đặt lại về "Tất cả"
            cmbStatus.SelectedIndex = 0; // Đặt lại về "Tất cả"

            // Gán lại sự kiện
            cmbRole.SelectedIndexChanged += Filter_InputChanged;
            cmbStatus.SelectedIndexChanged += Filter_InputChanged;


            LoadAll(); // Tải lại toàn bộ (đã lọc Admin và có sort)
        }
        #endregion

        #region Data Loading and Searching
        // Tải tất cả tài khoản (trừ Admin)
        private void LoadAll()
        {
            try
            {
                var data = _svc.GetAll()
                    .Where(a => a.Role != UserRole.Admin) // Lọc Admin
                    .Select(a => new
                    {
                        a.Id,
                        a.Username,
                        a.Role,
                        PersonName = a.Customer != null ? a.Customer.Name : (a.Driver != null ? a.Driver.FullName : ""),
                        LinkedTo = a.CustomerId != null ? $"KH: {(a.Customer?.Name ?? $"ID {a.CustomerId}")}" : (a.DriverId != null ? $"TX: {(a.Driver?.FullName ?? $"ID {a.DriverId}")}" : "(N/A)"),
                        a.IsActive
                    })
                    .ToList(); // Lấy dữ liệu

                _bindingList = new BindingList<object>(data.Cast<object>().ToList());
                dgvResults.DataSource = _bindingList; // Gán DataSource

                ApplySort(); // Áp dụng sort (nếu có)
                UpdateSortGlyphs(); // Cập nhật mũi tên
            }
            catch (Exception ex) // Sử dụng biến ex
            {
                MessageBox.Show($"Lỗi tải danh sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hiển thị lỗi
                dgvResults.DataSource = null;
            }
            // Không cần finally ResetSortGlyphs() ở đây nữa
        }

        // Thực hiện tìm kiếm/lọc
        private void DoSearch()
        {
            try
            {
                // Lấy giá trị filter từ controls
                var usernameFilter = string.IsNullOrWhiteSpace(txtUser.Text) ? null : txtUser.Text.Trim();
                var nameFilter = string.IsNullOrWhiteSpace(txtName.Text) ? null : txtName.Text.Trim();
                // Lấy SelectedValue và ép kiểu sang nullable
                UserRole? roleFilter = cmbRole.SelectedValue as UserRole?;
                bool? isActiveFilter = cmbStatus.SelectedValue as bool?;

                // Gọi service với filter, lọc Admin
                var data = _svc.GetAll(usernameFilter, nameFilter, roleFilter, isActiveFilter)
                    .Where(a => a.Role != UserRole.Admin) // Lọc Admin
                    .Select(a => new
                    {
                        a.Id,
                        a.Username,
                        a.Role,
                        PersonName = a.Customer != null ? a.Customer.Name : (a.Driver != null ? a.Driver.FullName : ""),
                        LinkedTo = a.CustomerId != null ? $"KH: {(a.Customer?.Name ?? $"ID {a.CustomerId}")}" : (a.DriverId != null ? $"TX: {(a.Driver?.FullName ?? $"ID {a.DriverId}")}" : "(N/A)"),
                        a.IsActive
                    })
                    .ToList(); // Lấy dữ liệu

                _bindingList = new BindingList<object>(data.Cast<object>().ToList());
                dgvResults.DataSource = _bindingList; // Gán DataSource

                ApplySort(); // Áp dụng sort (nếu có)
                UpdateSortGlyphs(); // Cập nhật mũi tên
            }
            catch (Exception ex) // Sử dụng biến ex
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hiển thị lỗi
                // dgvResults.DataSource = null; // Có thể giữ kết quả cũ hoặc xóa đi tùy ý
            }
        }
        #endregion

        #region Grid Interaction (Double Click)
        // Xử lý double click -> chọn và đóng form
        private void DgvResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Click vào dòng dữ liệu
            {
                try
                {
                    // Lấy ID từ đối tượng dữ liệu của dòng
                    var id = (int)((dynamic)dgvResults.Rows[e.RowIndex].DataBoundItem).Id;
                    // Kích hoạt sự kiện AccountSelected, gửi ID ra ngoài
                    AccountSelected?.Invoke(this, id);
                    // Đóng form tìm kiếm này lại
                    this.FindForm()?.Close();
                }
                catch (Exception ex) // Sử dụng biến ex
                {
                    MessageBox.Show($"Lỗi khi chọn tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Hiển thị lỗi
                }
            }
        }
        #endregion

        #region Sorting Logic (Đã hoàn chỉnh)
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
                if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                _sortOrder = SortOrder.Ascending;
                _sortedColumn = newColumn;
            }
            ApplySort();
            UpdateSortGlyphs();
        }

        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;
            string propertyName = _sortedColumn.DataPropertyName; PropertyInfo propInfo = null;
            var firstItem = _bindingList.FirstOrDefault(item => item != null);
            if (firstItem != null) propInfo = firstItem.GetType().GetProperty(propertyName);
            if (propInfo == null) return;

            List<object> items = _bindingList.ToList(); List<object> sortedList;
            try
            {
                if (_sortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
                else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }
                _bindingList = new BindingList<object>(sortedList); dgvResults.DataSource = _bindingList;
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp cột '{_sortedColumn.HeaderText}': {ex.Message}"); ResetSortGlyphs(); }
        }

        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvResults.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None;
                if (_sortedColumn != null && col == _sortedColumn) { col.HeaderCell.SortGlyphDirection = _sortOrder; }
            }
        }
        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            _sortedColumn = null; _sortOrder = SortOrder.None; UpdateSortGlyphs();
        }
        #endregion

        // ==========================================================
        // === (3) THÊM 3 HÀM XỬ LÝ KÉO THẢ CHO FORM CHỨA UC NÀY ===
        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            // Lấy Form cha (Form popup)
            Form parentForm = this.FindForm();
            if (parentForm == null) return;

            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = parentForm.Location; // Lấy vị trí của Form cha
            }
        }

        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            // Lấy Form cha
            Form parentForm = this.FindForm();
            if (parentForm == null) return;

            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                parentForm.Location = Point.Add(dragFormPoint, new Size(dif)); // Di chuyển Form cha
            }
        }

        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = false;
            }
        }
        // ==========================================================
    }
}