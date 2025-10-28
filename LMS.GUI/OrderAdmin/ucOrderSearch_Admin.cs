//using Guna.UI2.WinForms;
//using LMS.BUS.Dtos;
//using LMS.BUS.Services;
//using LMS.DAL;
//using LMS.DAL.Models;
//using System;
//using System.Data.Entity;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Reflection;
//using LMS.BUS.Helpers;

//namespace LMS.GUI.OrderAdmin
//{
//    public partial class ucOrderSearch_Admin : UserControl
//    {
//        public event EventHandler<int> OrderPicked;
//        private readonly OrderService_Admin _orderSvc = new OrderService_Admin();
//        private DataGridViewColumn _sortedColumn = null;
//        private SortOrder _sortOrder = SortOrder.None;

//        public ucOrderSearch_Admin()
//        {
//            InitializeComponent();
//            this.Load += UcOrderSearch_Admin_Load;
//        }

//        private void UcOrderSearch_Admin_Load(object sender, EventArgs e)
//        {
//            if (this.lblTitle != null)
//            {
//                this.lblTitle.Text = "Tìm kiếm Đơn Hàng";
//            }

//            BindFilters();
//            ConfigureGrid();
//            WireEvents();
//        }

//        private void BindFilters()
//        {
//            using (var db = new LogisticsDbContext())
//            {
//                var customers = db.Customers.OrderBy(c => c.Name)
//                    .Select(c => new { c.Id, c.Name }).ToList();
//                customers.Insert(0, new { Id = 0, Name = "— Tất cả —" });
//                cmbCustomer.DataSource = customers;
//                cmbCustomer.ValueMember = "Id";
//                cmbCustomer.DisplayMember = "Name";

//                var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
//                    .Select(s => new { Id = (int)s, Name = s.ToString() }).ToList();
//                statuses.Insert(0, new { Id = -1, Name = "— Tất cả —" });
//                cmbStatus.DataSource = statuses;
//                cmbStatus.ValueMember = "Id";
//                cmbStatus.DisplayMember = "Name";

//                var wh = db.Warehouses.OrderBy(w => w.Name)
//                    .Select(w => new { w.Id, w.Name }).ToList();
//                var whAll = wh.ToList();
//                whAll.Insert(0, new { Id = 0, Name = "— Tất cả —" });
//                cmbOrigin.DataSource = whAll.ToList();
//                cmbOrigin.ValueMember = "Id";
//                cmbOrigin.DisplayMember = "Name";
//                cmbDest.DataSource = whAll.ToList();
//                cmbDest.ValueMember = "Id";
//                cmbDest.DisplayMember = "Name";
//            }
//            dtFrom.Checked = false;
//            dtTo.Checked = false;
//        }

//        private void ConfigureGrid()
//        {
//            var g = dgvSearchResults;
//            g.Columns.Clear();
//            g.ApplyBaseStyle();
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CustomerName", DataPropertyName = "CustomerName", HeaderText = "Khách hàng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OriginWarehouseName", DataPropertyName = "OriginWarehouseName", HeaderText = "Kho gửi", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DestWarehouseName", DataPropertyName = "DestWarehouseName", HeaderText = "Kho nhận", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalFee", DataPropertyName = "TotalFee", HeaderText = "Tổng phí", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DepositAmount", DataPropertyName = "DepositAmount", HeaderText = "Đặt cọc", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", DataPropertyName = "CreatedAt", HeaderText = "Ngày tạo", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }, SortMode = DataGridViewColumnSortMode.Programmatic });
//            TryEnableDoubleBuffer(g);
//        }

//        private static void TryEnableDoubleBuffer(DataGridView grid)
//        {
//            try
//            {
//                var property = grid.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
//                if (property != null)
//                {
//                    property.SetValue(grid, true, null);
//                }
//            }
//            catch { }
//        }

//        private void WireEvents()
//        {
//            btnDoSearch.Click += (s, e) => DoSearch();
//            btnReset.Click += (s, e) =>
//            {
//                cmbCustomer.SelectedIndex = 0;
//                cmbStatus.SelectedIndex = 0;
//                cmbOrigin.SelectedIndex = 0;
//                cmbDest.SelectedIndex = 0;
//                txtCode.Clear();
//                dtFrom.Checked = false;
//                dtTo.Checked = false;
//                dgvSearchResults.DataSource = null;
//                ResetSortGlyphs();
//            };
//            btnClose.Click += (s, e) =>
//            {
//                var form = this.FindForm();
//                if (form != null)
//                {
//                    form.Close();
//                }
//            };
//            dgvSearchResults.ColumnHeaderMouseClick += dgvSearchResults_ColumnHeaderMouseClick;
//            dgvSearchResults.CellDoubleClick += (s, e) =>
//            {
//                if (e.RowIndex >= 0)
//                {
//                    var row = dgvSearchResults.Rows[e.RowIndex];
//                    var id = Convert.ToInt32(row.Cells["Id"].Value);
//                    if (OrderPicked != null)
//                    {
//                        OrderPicked(this, id);
//                    }
//                }
//            };
//        }

//        private void DoSearch()
//        {
//            int? cusId = null, originId = null, destId = null;
//            OrderStatus? status = null;
//            DateTime? from = null, to = null;
//            if (cmbCustomer != null && (int)cmbCustomer.SelectedValue != 0) cusId = (int)cmbCustomer.SelectedValue;
//            if (cmbOrigin != null && (int)cmbOrigin.SelectedValue != 0) originId = (int)cmbOrigin.SelectedValue;
//            if (cmbDest != null && (int)cmbDest.SelectedValue != 0) destId = (int)cmbDest.SelectedValue;
//            var st = (int)(cmbStatus != null ? cmbStatus.SelectedValue : -1);
//            if (st != -1) status = (OrderStatus)st;
//            if (dtFrom != null && dtFrom.Checked) from = dtFrom.Value.Date;
//            if (dtTo != null && dtTo.Checked) to = dtTo.Value.Date;
//            var list = _orderSvc.SearchForAdmin(cusId, status, originId, destId, from, to, txtCode != null ? txtCode.Text?.Trim() : null);
//            if (dgvSearchResults != null)
//            {
//                dgvSearchResults.DataSource = list;
//            }
//            ResetSortGlyphs();
//        }

//        private void dgvSearchResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            var list = dgvSearchResults != null ? dgvSearchResults.DataSource as List<OrderListItemDto> : null;
//            if (list == null || list.Count == 0) return;

//            var column = dgvSearchResults != null ? dgvSearchResults.Columns[e.ColumnIndex] : null;
//            if (column == null || column.SortMode == DataGridViewColumnSortMode.NotSortable) return;

//            // Xác định hướng sắp xếp
//            if (_sortedColumn == column)
//            {
//                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
//            }
//            else
//            {
//                if (_sortedColumn != null)
//                {
//                    _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//                }
//                _sortOrder = SortOrder.Ascending;
//                _sortedColumn = column;
//            }

//            // Sắp xếp danh sách bằng reflection
//            var property = typeof(OrderListItemDto).GetProperty(column.DataPropertyName);
//            if (property != null)
//            {
//                IOrderedEnumerable<OrderListItemDto> sortedList;
//                if (_sortOrder == SortOrder.Ascending)
//                    sortedList = list.OrderBy(x => property.GetValue(x));
//                else
//                    sortedList = list.OrderByDescending(x => property.GetValue(x));

//                if (dgvSearchResults != null)
//                {
//                    dgvSearchResults.DataSource = sortedList.ToList();
//                    column.HeaderCell.SortGlyphDirection = _sortOrder;
//                }
//            }
//        }

//        private void ResetSortGlyphs()
//        {
//            if (_sortedColumn != null)
//            {
//                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//            }
//            _sortedColumn = null;
//            _sortOrder = SortOrder.None;
//        }
//    }
//}

// LMS.GUI.OrderAdmin.ucOrderSearch_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Services;
using LMS.DAL;
using LMS.DAL.Models; // Cần cho OrderStatus
using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using LMS.BUS.Helpers;

namespace LMS.GUI.OrderAdmin
{
    public partial class ucOrderSearch_Admin : UserControl
    {
        public event EventHandler<int> OrderPicked;
        private readonly OrderService_Admin _orderSvc = new OrderService_Admin();
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        public ucOrderSearch_Admin()
        {
            InitializeComponent();
            this.Load += UcOrderSearch_Admin_Load;
        }

        private void UcOrderSearch_Admin_Load(object sender, EventArgs e)
        {
            if (this.lblTitle != null)
            {
                this.lblTitle.Text = "Tìm kiếm Đơn Hàng";
            }

            BindFilters();
            ConfigureGrid();
            WireEvents();
        }

        // Bind dữ liệu vào các ComboBox filter
        private void BindFilters()
        {
            using (var db = new LogisticsDbContext())
            {
                // Load Customers
                var customers = db.Customers.OrderBy(c => c.Name)
                    .Select(c => new { c.Id, c.Name }).ToList();
                customers.Insert(0, new { Id = 0, Name = "— Tất cả —" });
                cmbCustomer.DataSource = customers;
                cmbCustomer.ValueMember = "Id";
                cmbCustomer.DisplayMember = "Name";

                // Load Statuses (Chuyển sang tiếng Việt ở đây nếu muốn hiển thị trong ComboBox)
                var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                    .Select(s => new { Id = (int)s, Name = GetVietnameseOrderStatus(s) }) // Gọi hàm chuyển đổi
                    .ToList();
                statuses.Insert(0, new { Id = -1, Name = "— Tất cả —" });
                cmbStatus.DataSource = statuses;
                cmbStatus.ValueMember = "Id";
                cmbStatus.DisplayMember = "Name";

                // Load Warehouses
                var wh = db.Warehouses.OrderBy(w => w.Name)
                    .Select(w => new { w.Id, w.Name }).ToList();
                var whAll = wh.ToList(); // Tạo bản sao
                whAll.Insert(0, new { Id = 0, Name = "— Tất cả —" });
                cmbOrigin.DataSource = whAll.ToList(); // Dùng bản sao
                cmbOrigin.ValueMember = "Id";
                cmbOrigin.DisplayMember = "Name";
                cmbDest.DataSource = whAll.ToList(); // Dùng bản sao
                cmbDest.ValueMember = "Id";
                cmbDest.DisplayMember = "Name";
            }
            dtFrom.Checked = false; // Bỏ check mặc định
            dtTo.Checked = false;   // Bỏ check mặc định
        }

        // Cấu hình các cột cho DataGridView
        private void ConfigureGrid()
        {
            var g = dgvSearchResults;
            g.Columns.Clear();
            g.ApplyBaseStyle(); // Áp dụng style cơ bản (nếu có)

            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            // Các cột khác giữ nguyên AutoSizeMode Fill hoặc đổi thành AllCells nếu muốn
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo", DataPropertyName = "OrderNo", HeaderText = "Mã ĐH", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CustomerName", DataPropertyName = "CustomerName", HeaderText = "Khách hàng", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OriginWarehouseName", DataPropertyName = "OriginWarehouseName", HeaderText = "Kho gửi", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DestWarehouseName", DataPropertyName = "DestWarehouseName", HeaderText = "Kho nhận", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalFee", DataPropertyName = "TotalFee", HeaderText = "Tổng phí", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DepositAmount", DataPropertyName = "DepositAmount", HeaderText = "Đặt cọc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic }); // Giữ AllCells
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", DataPropertyName = "CreatedAt", HeaderText = "Ngày tạo", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 120, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }, SortMode = DataGridViewColumnSortMode.Programmatic }); // Cột cuối Fill

            TryEnableDoubleBuffer(g); // Bật double buffer
        }

        // Hàm bật Double Buffer (giữ nguyên)
        private static void TryEnableDoubleBuffer(DataGridView grid)
        {
            try
            {
                var property = grid.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (property != null)
                {
                    property.SetValue(grid, true, null);
                }
            }
            catch { /* Bỏ qua lỗi nếu không set được */ }
        }

        // Gán sự kiện cho các control
        private void WireEvents()
        {
            btnDoSearch.Click += (s, e) => DoSearch();
            btnReset.Click += (s, e) =>
            {
                // Reset các filter về giá trị mặc định
                cmbCustomer.SelectedIndex = 0;
                cmbStatus.SelectedIndex = 0;
                cmbOrigin.SelectedIndex = 0;
                cmbDest.SelectedIndex = 0;
                txtCode.Clear();
                dtFrom.Checked = false;
                dtTo.Checked = false;
                dgvSearchResults.DataSource = null; // Xóa kết quả cũ
                ResetSortGlyphs(); // Reset mũi tên sort
            };
            btnClose.Click += (s, e) =>
            {
                // Đóng Form cha chứa UserControl này
                var form = this.FindForm();
                form?.Close(); // Dùng ?. để tránh lỗi nếu form là null
            };
            dgvSearchResults.ColumnHeaderMouseClick += dgvSearchResults_ColumnHeaderMouseClick; // Sự kiện sort
            dgvSearchResults.CellDoubleClick += (s, e) =>
            {
                // Khi double click vào 1 dòng
                if (e.RowIndex >= 0) // Đảm bảo không phải header
                {
                    var row = dgvSearchResults.Rows[e.RowIndex];
                    var id = Convert.ToInt32(row.Cells["Id"].Value);
                    // Phát sự kiện OrderPicked với ID đã chọn
                    OrderPicked?.Invoke(this, id); // Dùng ?. Invoke cho an toàn
                }
            };
            // ===== GÁN SỰ KIỆN CELL FORMATTING =====
            dgvSearchResults.CellFormatting += dgvSearchResults_CellFormatting;
            // ======================================
        }

        // Thực hiện tìm kiếm
        private void DoSearch()
        {
            // Lấy giá trị từ các filter
            int? cusId = null, originId = null, destId = null;
            OrderStatus? status = null;
            DateTime? from = null, to = null;

            // Kiểm tra null trước khi truy cập SelectedValue
            if (cmbCustomer?.SelectedValue != null && (int)cmbCustomer.SelectedValue != 0) cusId = (int)cmbCustomer.SelectedValue;
            if (cmbOrigin?.SelectedValue != null && (int)cmbOrigin.SelectedValue != 0) originId = (int)cmbOrigin.SelectedValue;
            if (cmbDest?.SelectedValue != null && (int)cmbDest.SelectedValue != 0) destId = (int)cmbDest.SelectedValue;
            if (cmbStatus?.SelectedValue != null)
            {
                var st = (int)cmbStatus.SelectedValue;
                if (st != -1) status = (OrderStatus)st; // Ép kiểu nếu không phải "Tất cả"
            }
            if (dtFrom != null && dtFrom.Checked) from = dtFrom.Value.Date; // Lấy ngày (bỏ giờ)
            if (dtTo != null && dtTo.Checked) to = dtTo.Value.Date.AddDays(1).AddTicks(-1); // Lấy hết ngày

            // Gọi service để tìm kiếm
            var list = _orderSvc.SearchForAdmin(cusId, status, originId, destId, from, to, txtCode?.Text?.Trim()); // Dùng ?. cho txtCode

            // Hiển thị kết quả
            if (dgvSearchResults != null)
            {
                dgvSearchResults.DataSource = list;
            }
            ResetSortGlyphs(); // Reset sort sau khi tìm kiếm
        }

        // === HÀM FORMAT STATUS SANG TIẾNG VIỆT ===
        private void dgvSearchResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Kiểm tra cột "Status" và giá trị không null
            if (dgvSearchResults.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                // Kiểm tra kiểu dữ liệu là OrderStatus
                if (e.Value is OrderStatus statusEnum)
                {
                    // Chuyển đổi và gán lại giá trị hiển thị
                    e.Value = GetVietnameseOrderStatus(statusEnum);
                    e.FormattingApplied = true; // Báo đã xử lý
                }
            }
        }

        // Hàm helper để chuyển OrderStatus sang tiếng Việt
        private string GetVietnameseOrderStatus(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Pending: return "Chờ duyệt";
                case OrderStatus.Approved: return "Đã duyệt";
                case OrderStatus.Completed: return "Hoàn thành";
                case OrderStatus.Cancelled: return "Đã hủy";
                default: return status.ToString(); // Trả về tên gốc nếu không khớp
            }
        }
        // ========================================


        // Xử lý sort khi click vào header (Giữ nguyên)
        private void dgvSearchResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var list = dgvSearchResults?.DataSource as List<OrderListItemDto>; // Dùng ?.
            if (list == null || list.Count == 0) return;

            var column = dgvSearchResults?.Columns[e.ColumnIndex]; // Dùng ?.
            if (column == null || column.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            // Xác định hướng sort mới
            if (_sortedColumn == column)
            {
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                if (_sortedColumn?.HeaderCell != null) // Dùng ?.
                {
                    _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
                _sortOrder = SortOrder.Ascending;
                _sortedColumn = column;
            }

            // Sắp xếp dữ liệu bằng reflection
            var property = typeof(OrderListItemDto).GetProperty(column.DataPropertyName);
            if (property != null)
            {
                // Sử dụng IEnumerable để tránh lỗi ép kiểu
                IEnumerable<OrderListItemDto> sortedList;
                if (_sortOrder == SortOrder.Ascending)
                    sortedList = list.OrderBy(x => property.GetValue(x));
                else
                    sortedList = list.OrderByDescending(x => property.GetValue(x));

                // Cập nhật lại DataSource
                if (dgvSearchResults != null)
                {
                    dgvSearchResults.DataSource = sortedList.ToList(); // Gán lại List đã sort
                    if (column.HeaderCell != null) // Kiểm tra HeaderCell
                    {
                        column.HeaderCell.SortGlyphDirection = _sortOrder; // Cập nhật mũi tên
                    }
                }
            }
        }

        // Reset mũi tên sort (Giữ nguyên)
        private void ResetSortGlyphs()
        {
            if (_sortedColumn?.HeaderCell != null) // Dùng ?.
            {
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            _sortedColumn = null;
            _sortOrder = SortOrder.None;
        }
    }
}