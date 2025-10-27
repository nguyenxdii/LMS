//// LMS.GUI/ShipmentAdmin/ucShipmentDetail_Admin.cs
//using Guna.UI2.WinForms;
//using LMS.BUS.Dtos; // Cần cho ShipmentDetailDto, RouteStopLiteDto, ShipmentDriverLogDto
//using LMS.BUS.Helpers;
//using LMS.BUS.Services;
//using LMS.DAL.Models; // Cần cho Enums
//using System;
//using System.Collections.Generic;
//using System.ComponentModel; // Cần cho BindingList
//using System.Drawing;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;

//namespace LMS.GUI.ShipmentAdmin
//{
//    public partial class ucShipmentDetail_Admin : UserControl
//    {
//        private readonly int _shipmentId;
//        private readonly ShipmentService_Admin _shipmentSvc = new ShipmentService_Admin(); // Service Admin
//        private ShipmentDetailDto _currentDto; // Lưu trữ DTO để truy cập

//        // Biến sort cho grid RouteStop
//        private DataGridViewColumn _stopSortedColumn = null;
//        private SortOrder _stopSortOrder = SortOrder.None;

//        // Biến sort cho grid Lịch sử Tài xế
//        private DataGridViewColumn _historySortedColumn = null;
//        private SortOrder _historySortOrder = SortOrder.None;

//        public ucShipmentDetail_Admin(int shipmentId)
//        {
//            InitializeComponent();
//            _shipmentId = shipmentId;
//            this.Load += UcShipmentDetail_Admin_Load;
//        }

//        private void UcShipmentDetail_Admin_Load(object sender, EventArgs e)
//        {
//            // Cập nhật Label tiêu đề nếu có
//            if (this.lblTitle != null)
//            {
//                this.lblTitle.Text = "Chi Tiết Chuyến Hàng";
//            }
//            ConfigureRouteStopGrid();
//            //ConfigureDriverHistoryGrid(); // Gọi hàm cấu hình mới
//            LoadDetails();
//            WireEvents();
//        }

//        #region Setup Controls
//        private void ConfigureRouteStopGrid()
//        {
//            var g = dgvRouteStops; // Giả sử tên grid là dgvRouteStops
//            g.Columns.Clear();
//            g.ApplyBaseStyle();

//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Seq", DataPropertyName = "Seq", HeaderText = "#", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "StopName", DataPropertyName = "StopName", HeaderText = "Điểm dừng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "StopStatus", DataPropertyName = "StopStatus", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "ArrivedAt", DataPropertyName = "ArrivedAt", HeaderText = "Đến lúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm" }, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DepartedAt", DataPropertyName = "DepartedAt", HeaderText = "Rời lúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm" }, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "PlannedETA", DataPropertyName = "PlannedETA", HeaderText = "Dự kiến", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm" }, SortMode = DataGridViewColumnSortMode.Programmatic });

//            g.CellFormatting += DgvRouteStops_CellFormatting; // Format Status chặng
//        }

//        // Hàm cấu hình grid lịch sử
//        private void ConfigureDriverHistoryGrid()
//        {
//            // *** QUAN TRỌNG: Đảm bảo dgvDriverHistory tồn tại trong Designer ***
//            var g = dgvDriverHistory;
//            if (g == null)
//            {
//                System.Diagnostics.Debug.WriteLine("Warning: dgvDriverHistory control not found in ucShipmentDetail_Admin Designer.");
//                return; // Thoát nếu chưa thêm grid vào design
//            }


//            g.Columns.Clear();
//            g.ApplyBaseStyle(); // Áp dụng style chung
//            // Giảm chiều cao header và row cho grid phụ (tùy chỉnh nếu muốn)
//            g.ColumnHeadersHeight = 30;
//            g.RowTemplate.Height = 35;


//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Timestamp", DataPropertyName = "Timestamp", HeaderText = "Thời điểm đổi", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm:ss" }, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OldDriverName", DataPropertyName = "OldDriverName", HeaderText = "Tài xế cũ", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "NewDriverName", DataPropertyName = "NewDriverName", HeaderText = "Tài xế mới", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "StopSequenceNumber", DataPropertyName = "StopSequenceNumber", HeaderText = "Tại chặng #", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }, SortMode = DataGridViewColumnSortMode.NotSortable }); // Có thể sort nếu muốn
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Reason", DataPropertyName = "Reason", HeaderText = "Lý do", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.NotSortable });

//            // Thêm sự kiện sort cho grid lịch sử
//            g.ColumnHeaderMouseClick += dgvDriverHistory_ColumnHeaderMouseClick;
//        }

//        private void WireEvents()
//        {
//            // *** QUAN TRỌNG: Đảm bảo btnClose tồn tại trong Designer ***
//            if (btnClose != null)
//            {
//                btnClose.Click += (s, e) => this.FindForm()?.Close();
//            }
//            else
//            {
//                System.Diagnostics.Debug.WriteLine("Warning: btnClose control not found in ucShipmentDetail_Admin Designer.");
//            }

//            // Gán sự kiện sort cho grid RouteStop
//            if (dgvRouteStops != null)
//            {
//                dgvRouteStops.ColumnHeaderMouseClick += dgvRouteStops_ColumnHeaderMouseClick;
//            }

//            //// Thêm sự kiện sort cho grid lịch sử
//            //if (dgvDriverHistory != null)
//            //{
//            //    dgvDriverHistory.ColumnHeaderMouseClick += dgvDriverHistory_ColumnHeaderMouseClick;
//            //}
//        }
//        #endregion

//        #region Load Data & Binding
//        private void LoadDetails()
//        {
//            try
//            {
//                _currentDto = _shipmentSvc.GetShipmentDetailForAdmin(_shipmentId); // Gọi hàm service Admin
//                if (_currentDto == null || _currentDto.Header == null)
//                {
//                    throw new Exception("Không tải được dữ liệu chi tiết chuyến hàng.");
//                }

//                BindHeader(_currentDto.Header);
//                BindMisc(_currentDto);
//                BindStops(_currentDto.Stops);
//                //BindDriverHistory(_currentDto.DriverHistory); // Gọi hàm bind mới
//                ResetStopSortGlyphs();
//                ResetHistorySortGlyphs(); // Reset sort grid mới
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tải chi tiết: {ex.Message}", "Lỗi");
//                //this.FindForm()?.Close(); // Đóng popup nếu lỗi
//            }
//        }

//        private void BindHeader(ShipmentRunHeaderDto header)
//        {
//            // *** Đảm bảo các Label này tồn tại trong Designer ***
//            if (lblShipmentNo != null) lblShipmentNo.Text = $"Mã Chuyến: {header.ShipmentNo}";
//            if (lblOrderNo != null) lblOrderNo.Text = $"Mã Đơn: {header.OrderNo}";
//            if (lblCustomerName != null) lblCustomerName.Text = $"Khách hàng: {header.CustomerName ?? "N/A"}";
//            if (lblRoute != null) lblRoute.Text = $"Tuyến: {header.Route ?? "N/A"}";
//            if (lblStatus != null) lblStatus.Text = $"Trạng thái: {FormatShipmentStatusString(header.Status)}"; // Format Status
//            if (lblStartedAt != null) lblStartedAt.Text = $"Bắt đầu: {header.StartedAt?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa bắt đầu"}";
//            if (lblDeliveredAt != null) lblDeliveredAt.Text = $"Kết thúc: {header.DeliveredAt?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa kết thúc"}";
//        }

//        private void BindMisc(ShipmentDetailDto detail)
//        {
//            // *** Đảm bảo các Label/TextBox này tồn tại trong Designer ***
//            if (lblDriverName != null) lblDriverName.Text = $"Tài xế hiện tại: {detail.DriverName ?? "N/A"}"; // Cập nhật text
//            if (lblVehicleNo != null) lblVehicleNo.Text = $"Biển số xe: {detail.VehicleNo ?? "N/A"}";
//            if (lblDuration != null) lblDuration.Text = $"Thời gian: {detail.Duration?.ToString(@"hh\:mm\:ss") ?? "N/A"}";
//            if (txtNotes != null)
//            {
//                txtNotes.Text = detail.Notes ?? ""; // Hiển thị ghi chú
//                txtNotes.ReadOnly = true; // Chỉ cho xem
//            }
//        }

//        private void BindStops(List<RouteStopLiteDto> stops)
//        {
//            if (dgvRouteStops != null)
//            {
//                dgvRouteStops.DataSource = new BindingList<RouteStopLiteDto>(stops ?? new List<RouteStopLiteDto>());
//            }
//        }

//        // Hàm bind lịch sử
//        private void BindDriverHistory(List<ShipmentDriverLogDto> history)
//        {
//            if (dgvDriverHistory == null) return; // Bỏ qua nếu grid không tồn tại

//            // Hiển thị hoặc ẩn GroupBox/Panel chứa grid lịch sử dựa vào việc có log hay không
//            // *** Giả sử container tên là grpDriverHistory (cần tạo trong Designer) ***
//            var historyContainer = dgvDriverHistory; // Hoặc dgvDriverHistory.Parent;
//            if (historyContainer != null)
//            {
//                historyContainer.Visible = history != null && history.Any();
//            }

//            dgvDriverHistory.DataSource = new BindingList<ShipmentDriverLogDto>(history ?? new List<ShipmentDriverLogDto>());
//        }
//        #endregion

//        #region Stop Grid Sorting
//        private void dgvRouteStops_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            var list = dgvRouteStops?.DataSource as BindingList<RouteStopLiteDto>;
//            if (list == null || list.Count == 0 || dgvRouteStops == null) return;
//            var newColumn = dgvRouteStops.Columns[e.ColumnIndex];
//            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

//            if (_stopSortedColumn == newColumn) { _stopSortOrder = (_stopSortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
//            else { if (_stopSortedColumn != null) _stopSortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _stopSortOrder = SortOrder.Ascending; _stopSortedColumn = newColumn; }

//            ApplyStopSort(list);
//            UpdateStopSortGlyphs();
//        }

//        private void ApplyStopSort(BindingList<RouteStopLiteDto> data)
//        {
//            if (_stopSortedColumn == null || data == null || data.Count == 0 || dgvRouteStops == null) return;
//            string propertyName = _stopSortedColumn.DataPropertyName;
//            PropertyInfo propInfo = typeof(RouteStopLiteDto).GetProperty(propertyName);
//            if (propInfo == null) return;

//            List<RouteStopLiteDto> items = data.ToList();
//            List<RouteStopLiteDto> sortedList;
//            try
//            {
//                if (_stopSortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
//                else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }
//                // Cập nhật lại DataSource
//                dgvRouteStops.DataSource = new BindingList<RouteStopLiteDto>(sortedList);
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp chặng: {ex.Message}"); ResetStopSortGlyphs(); }
//        }

//        private void UpdateStopSortGlyphs()
//        {
//            if (dgvRouteStops == null) return;
//            foreach (DataGridViewColumn col in dgvRouteStops.Columns)
//            {
//                if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None;
//                if (_stopSortedColumn != null && col == _stopSortedColumn) col.HeaderCell.SortGlyphDirection = _stopSortOrder;
//            }
//        }
//        private void ResetStopSortGlyphs()
//        {
//            if (_stopSortedColumn != null) _stopSortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//            _stopSortedColumn = null; _stopSortOrder = SortOrder.None;
//            UpdateStopSortGlyphs();
//        }
//        #endregion

//        #region History Grid Sorting
//        private void dgvDriverHistory_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            var list = dgvDriverHistory?.DataSource as BindingList<ShipmentDriverLogDto>;
//            if (list == null || list.Count == 0 || dgvDriverHistory == null) return;
//            var newColumn = dgvDriverHistory.Columns[e.ColumnIndex];
//            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

//            if (_historySortedColumn == newColumn) { _historySortOrder = (_historySortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
//            else { if (_historySortedColumn != null) _historySortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _historySortOrder = SortOrder.Ascending; _historySortedColumn = newColumn; }

//            ApplyHistorySort(list);
//            UpdateHistorySortGlyphs();
//        }

//        private void ApplyHistorySort(BindingList<ShipmentDriverLogDto> data)
//        {
//            if (_historySortedColumn == null || data == null || data.Count == 0 || dgvDriverHistory == null) return;
//            string propertyName = _historySortedColumn.DataPropertyName;
//            PropertyInfo propInfo = typeof(ShipmentDriverLogDto).GetProperty(propertyName);
//            if (propInfo == null) return;

//            List<ShipmentDriverLogDto> items = data.ToList();
//            List<ShipmentDriverLogDto> sortedList;
//            try
//            {
//                if (_historySortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
//                else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }
//                dgvDriverHistory.DataSource = new BindingList<ShipmentDriverLogDto>(sortedList);
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp lịch sử: {ex.Message}"); ResetHistorySortGlyphs(); }
//        }

//        private void UpdateHistorySortGlyphs()
//        {
//            if (dgvDriverHistory == null) return;
//            foreach (DataGridViewColumn col in dgvDriverHistory.Columns)
//            {
//                if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None;
//                if (_historySortedColumn != null && col == _historySortedColumn) col.HeaderCell.SortGlyphDirection = _historySortOrder;
//            }
//        }
//        private void ResetHistorySortGlyphs()
//        {
//            if (_historySortedColumn != null) _historySortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//            _historySortedColumn = null; _historySortOrder = SortOrder.None;
//            UpdateHistorySortGlyphs();
//        }
//        #endregion

//        #region Formatting Helpers
//        // Format Status Chặng (RouteStopStatus)
//        private void DgvRouteStops_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            if (dgvRouteStops == null || e.RowIndex < 0 || e.ColumnIndex < 0 || e.ColumnIndex >= dgvRouteStops.Columns.Count) return;

//            if (dgvRouteStops.Columns[e.ColumnIndex].Name == "StopStatus" && e.Value != null)
//            {
//                if (Enum.TryParse<RouteStopStatus>(e.Value.ToString(), out var status))
//                {
//                    e.Value = FormatRouteStopStatus(status);
//                    e.FormattingApplied = true;
//                }
//            }

//            // Highlight dòng hiện tại (nếu có)
//            if (_currentDto?.Header?.CurrentStopSeq.HasValue == true)
//            {
//                var rowData = dgvRouteStops.Rows[e.RowIndex].DataBoundItem as RouteStopLiteDto;
//                // Highlight nếu Seq khớp VÀ trạng thái không phải là Departed (vì CurrentStopSeq chỉ đến chặng chưa rời)
//                if (rowData?.Seq == _currentDto.Header.CurrentStopSeq.Value && rowData.StopStatus != RouteStopStatus.Departed.ToString())
//                {
//                    e.CellStyle.BackColor = Color.LightYellow; // Màu highlight
//                    e.CellStyle.ForeColor = Color.Black; // Đảm bảo chữ đen dễ đọc
//                }
//                else
//                {
//                    // Reset màu nền cho các dòng khác để tránh lỗi tô màu sai khi cuộn
//                    // Lấy màu nền mặc định của dòng chẵn/lẻ
//                    Color defaultBackColor = (e.RowIndex % 2 == 0) ? dgvRouteStops.DefaultCellStyle.BackColor : dgvRouteStops.AlternatingRowsDefaultCellStyle.BackColor;
//                    // Lấy màu chữ mặc định
//                    Color defaultForeColor = dgvRouteStops.DefaultCellStyle.ForeColor;

//                    // Chỉ reset nếu màu hiện tại khác màu mặc định
//                    if (e.CellStyle.BackColor != defaultBackColor) e.CellStyle.BackColor = defaultBackColor;
//                    if (e.CellStyle.ForeColor != defaultForeColor) e.CellStyle.ForeColor = defaultForeColor;
//                }
//            }
//        }

//        // Format Enum ShipmentStatus sang tiếng Việt (dùng cho Label header)
//        private string FormatShipmentStatusString(string statusString)
//        {
//            if (Enum.TryParse<ShipmentStatus>(statusString, out var status))
//            {
//                return FormatShipmentStatus(status); // Gọi hàm helper chung
//            }
//            return statusString ?? "N/A";
//        }

//        // Hàm helper format ShipmentStatus
//        private string FormatShipmentStatus(ShipmentStatus status)
//        {
//            switch (status)
//            {
//                case ShipmentStatus.Pending: return "Chờ nhận";
//                case ShipmentStatus.Assigned: return "Đã nhận";
//                case ShipmentStatus.OnRoute: return "Đang đi đường";
//                case ShipmentStatus.AtWarehouse: return "Đang ở kho";
//                case ShipmentStatus.ArrivedDestination: return "Đã tới đích";
//                case ShipmentStatus.Delivered: return "Đã giao xong";
//                case ShipmentStatus.Failed: return "Gặp sự cố";
//                default: return status.ToString();
//            }
//        }

//        // Hàm helper format RouteStopStatus
//        private string FormatRouteStopStatus(RouteStopStatus status)
//        {
//            switch (status)
//            {
//                case RouteStopStatus.Waiting: return "Chờ";
//                case RouteStopStatus.Arrived: return "Đã đến";
//                case RouteStopStatus.Departed: return "Đã rời";
//                // Thêm các trạng thái khác nếu có (Skipped...)
//                default: return status.ToString();
//            }
//        }
//        #endregion
//    }
//}

// LMS.GUI/ShipmentAdmin/ucShipmentDetail_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos; // Cần cho ShipmentDetailDto, RouteStopLiteDto, ShipmentDriverLogDto
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models; // Cần cho Enums
using System;
using System.Collections.Generic;
using System.ComponentModel; // Cần cho BindingList
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.ShipmentAdmin
{
    public partial class ucShipmentDetail_Admin : UserControl
    {
        private readonly int _shipmentId;
        private readonly ShipmentService_Admin _shipmentSvc = new ShipmentService_Admin(); // Service Admin
        private ShipmentDetailDto _currentDto; // Lưu trữ DTO để truy cập

        // Biến sort cho grid RouteStop
        private DataGridViewColumn _stopSortedColumn = null;
        private SortOrder _stopSortOrder = SortOrder.None;

        // Biến sort cho grid Lịch sử Tài xế (Tạm thời không dùng)
        // private DataGridViewColumn _historySortedColumn = null;
        // private SortOrder _historySortOrder = SortOrder.None;

        public ucShipmentDetail_Admin(int shipmentId)
        {
            InitializeComponent();
            _shipmentId = shipmentId;
            this.Load += UcShipmentDetail_Admin_Load;
        }

        private void UcShipmentDetail_Admin_Load(object sender, EventArgs e)
        {
            // Cập nhật Label tiêu đề nếu có
            if (this.lblTitle != null)
            {
                this.lblTitle.Text = "Chi Tiết Chuyến Hàng";
            }
            ConfigureRouteStopGrid();
            // ConfigureDriverHistoryGrid(); // <-- TẠM THỜI VÔ HIỆU HÓA
            LoadDetails();
            WireEvents();
        }

        #region Setup Controls
        private void ConfigureRouteStopGrid()
        {
            var g = dgvRouteStops; // Giả sử tên grid là dgvRouteStops
            if (g == null) return; // Kiểm tra null
            g.Columns.Clear();
            g.ApplyBaseStyle();

            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Seq", DataPropertyName = "Seq", HeaderText = "#", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "StopName", DataPropertyName = "StopName", HeaderText = "Điểm dừng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "StopStatus", DataPropertyName = "StopStatus", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "ArrivedAt", DataPropertyName = "ArrivedAt", HeaderText = "Đến lúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm" }, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DepartedAt", DataPropertyName = "DepartedAt", HeaderText = "Rời lúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm" }, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "PlannedETA", DataPropertyName = "PlannedETA", HeaderText = "Dự kiến", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm" }, SortMode = DataGridViewColumnSortMode.Programmatic });

            g.CellFormatting += DgvRouteStops_CellFormatting; // Format Status chặng
        }

        // Tạm thời vô hiệu hóa hàm cấu hình grid lịch sử
        private void ConfigureDriverHistoryGrid()
        {
            // var g = dgvDriverHistory;
            // if (g == null) { System.Diagnostics.Debug.WriteLine("Warning: dgvDriverHistory control not found..."); return; }
            // g.Columns.Clear();
            // g.ApplyBaseStyle();
            // g.ColumnHeadersHeight = 30;
            // g.RowTemplate.Height = 35;
            // g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Timestamp", DataPropertyName = "Timestamp", HeaderText = "Thời điểm đổi", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm:ss" }, SortMode = DataGridViewColumnSortMode.Programmatic });
            // g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OldDriverName", DataPropertyName = "OldDriverName", HeaderText = "Tài xế cũ", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            // g.Columns.Add(new DataGridViewTextBoxColumn { Name = "NewDriverName", DataPropertyName = "NewDriverName", HeaderText = "Tài xế mới", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            // g.Columns.Add(new DataGridViewTextBoxColumn { Name = "StopSequenceNumber", DataPropertyName = "StopSequenceNumber", HeaderText = "Tại chặng #", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }, SortMode = DataGridViewColumnSortMode.NotSortable });
            // g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Reason", DataPropertyName = "Reason", HeaderText = "Lý do", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.NotSortable });
            // g.ColumnHeaderMouseClick += dgvDriverHistory_ColumnHeaderMouseClick;
        }


        private void WireEvents()
        {
            if (btnClose != null) { btnClose.Click += (s, e) => this.FindForm()?.Close(); }
            else { System.Diagnostics.Debug.WriteLine("Warning: btnClose control not found..."); }

            if (dgvRouteStops != null) { dgvRouteStops.ColumnHeaderMouseClick += dgvRouteStops_ColumnHeaderMouseClick; }

        }
        #endregion

        #region Load Data & Binding
        private void LoadDetails()
        {
            try
            {
                _currentDto = _shipmentSvc.GetShipmentDetailForAdmin(_shipmentId);
                if (_currentDto == null || _currentDto.Header == null)
                {
                    throw new Exception("Không tải được dữ liệu chi tiết chuyến hàng.");
                }

                BindHeader(_currentDto.Header);
                BindMisc(_currentDto);
                BindStops(_currentDto.Stops);
                ResetStopSortGlyphs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải chi tiết:\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Lỗi nghiêm trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindHeader(ShipmentRunHeaderDto header)
        {
            if (lblShipmentNo != null) lblShipmentNo.Text = $"Mã Chuyến: {header.ShipmentNo}";
            if (lblOrderNo != null) lblOrderNo.Text = $"Mã Đơn: {header.OrderNo}";
            if (lblCustomerName != null) lblCustomerName.Text = $"Khách hàng: {header.CustomerName ?? "N/A"}";
            if (lblRoute != null) lblRoute.Text = $"Tuyến: {header.Route ?? "N/A"}";
            if (lblStatus != null) lblStatus.Text = $"Trạng thái: {FormatShipmentStatusString(header.Status)}";
            if (lblStartedAt != null) lblStartedAt.Text = $"Bắt đầu: {header.StartedAt?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa bắt đầu"}";
            if (lblDeliveredAt != null) lblDeliveredAt.Text = $"Kết thúc: {header.DeliveredAt?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa kết thúc"}";
        }

        private void BindMisc(ShipmentDetailDto detail)
        {
            if (lblDriverName != null) lblDriverName.Text = $"Tài xế hiện tại: {detail.DriverName ?? "N/A"}";
            if (lblVehicleNo != null) lblVehicleNo.Text = $"Biển số xe: {detail.VehicleNo ?? "N/A"}";
            if (lblDuration != null) lblDuration.Text = $"Thời gian: {detail.Duration?.ToString(@"hh\:mm\:ss") ?? "N/A"}";
            if (txtNotes != null) { txtNotes.Text = detail.Notes ?? ""; txtNotes.ReadOnly = true; }
        }

        private void BindStops(List<RouteStopLiteDto> stops)
        {
            if (dgvRouteStops != null)
            {
                dgvRouteStops.DataSource = new BindingList<RouteStopLiteDto>(stops ?? new List<RouteStopLiteDto>());
                System.Diagnostics.Debug.WriteLine($"BindStops: Assigned {stops?.Count ?? 0} items to dgvRouteStops DataSource."); // Debug line
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Warning: dgvRouteStops is null in BindStops."); // Debug line
            }
        }


        private void BindDriverHistory(List<ShipmentDriverLogDto> history)
        {
            // if (dgvDriverHistory == null) return;
            // var historyContainer = dgvDriverHistory; // Or Parent
            // if (historyContainer != null) { historyContainer.Visible = history != null && history.Any(); }
            // dgvDriverHistory.DataSource = new BindingList<ShipmentDriverLogDto>(history ?? new List<ShipmentDriverLogDto>());
        }
        #endregion

        #region Stop Grid Sorting
        private void dgvRouteStops_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var list = dgvRouteStops?.DataSource as BindingList<RouteStopLiteDto>;
            if (list == null || list.Count == 0 || dgvRouteStops == null) return;
            var newColumn = dgvRouteStops.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_stopSortedColumn == newColumn) { _stopSortOrder = (_stopSortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
            else { if (_stopSortedColumn != null) _stopSortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _stopSortOrder = SortOrder.Ascending; _stopSortedColumn = newColumn; }

            ApplyStopSort(list);
            UpdateStopSortGlyphs();
        }

        private void ApplyStopSort(BindingList<RouteStopLiteDto> data)
        {
            if (_stopSortedColumn == null || data == null || data.Count == 0 || dgvRouteStops == null) return;
            string propertyName = _stopSortedColumn.DataPropertyName;
            PropertyInfo propInfo = typeof(RouteStopLiteDto).GetProperty(propertyName);
            if (propInfo == null) return;

            List<RouteStopLiteDto> items = data.ToList();
            List<RouteStopLiteDto> sortedList;
            try
            {
                if (_stopSortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
                else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }
                dgvRouteStops.DataSource = new BindingList<RouteStopLiteDto>(sortedList);
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp chặng: {ex.Message}"); ResetStopSortGlyphs(); }
        }

        private void UpdateStopSortGlyphs()
        {
            if (dgvRouteStops == null) return;
            foreach (DataGridViewColumn col in dgvRouteStops.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None;
                if (_stopSortedColumn != null && col == _stopSortedColumn) col.HeaderCell.SortGlyphDirection = _stopSortOrder;
            }
        }
        private void ResetStopSortGlyphs()
        {
            if (_stopSortedColumn != null && _stopSortedColumn.HeaderCell != null) // Check HeaderCell null
                _stopSortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            _stopSortedColumn = null; _stopSortOrder = SortOrder.None;
            UpdateStopSortGlyphs();
        }
        #endregion

        // Tạm thời vô hiệu hóa History Grid Sorting
        #region History Grid Sorting
        private void dgvDriverHistory_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //   var list = dgvDriverHistory?.DataSource as BindingList<ShipmentDriverLogDto>;
            //   if (list == null || list.Count == 0 || dgvDriverHistory == null) return;
            //   var newColumn = dgvDriverHistory.Columns[e.ColumnIndex];
            //   if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;
            //   if (_historySortedColumn == newColumn) { _historySortOrder = (_historySortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
            //   else { if (_historySortedColumn != null) _historySortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _historySortOrder = SortOrder.Ascending; _historySortedColumn = newColumn; }
            //   ApplyHistorySort(list);
            //   UpdateHistorySortGlyphs();
        }

        private void ApplyHistorySort(BindingList<ShipmentDriverLogDto> data)
        {
            //  if (_historySortedColumn == null || data == null || data.Count == 0 || dgvDriverHistory == null) return;
            //  string propertyName = _historySortedColumn.DataPropertyName;
            //  PropertyInfo propInfo = typeof(ShipmentDriverLogDto).GetProperty(propertyName);
            //  if (propInfo == null) return;
            //  List<ShipmentDriverLogDto> items = data.ToList();
            //  List<ShipmentDriverLogDto> sortedList;
            //  try
            //  {
            //      if (_historySortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
            //      else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }
            //      dgvDriverHistory.DataSource = new BindingList<ShipmentDriverLogDto>(sortedList);
            //  }
            //  catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp lịch sử: {ex.Message}"); ResetHistorySortGlyphs(); }
        }

        private void UpdateHistorySortGlyphs()
        {
            //  if (dgvDriverHistory == null) return;
            //  foreach (DataGridViewColumn col in dgvDriverHistory.Columns)
            //  {
            //      if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None;
            //      if (_historySortedColumn != null && col == _historySortedColumn) col.HeaderCell.SortGlyphDirection = _historySortOrder;
            //  }
        }
        private void ResetHistorySortGlyphs()
        {
            // if (_historySortedColumn != null && _historySortedColumn.HeaderCell != null) // Check HeaderCell null
            //      _historySortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            // _historySortedColumn = null; _historySortOrder = SortOrder.None;
            // UpdateHistorySortGlyphs();
        }
        #endregion

        #region Formatting Helpers
        private void DgvRouteStops_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvRouteStops == null || e.RowIndex < 0 || e.ColumnIndex < 0 || e.ColumnIndex >= dgvRouteStops.Columns.Count) return;

            if (dgvRouteStops.Columns[e.ColumnIndex].Name == "StopStatus" && e.Value != null)
            {
                if (Enum.TryParse<RouteStopStatus>(e.Value.ToString(), out var status)) { e.Value = FormatRouteStopStatus(status); e.FormattingApplied = true; }
            }

            if (_currentDto?.Header?.CurrentStopSeq.HasValue == true)
            {
                var rowData = dgvRouteStops.Rows[e.RowIndex].DataBoundItem as RouteStopLiteDto;
                if (rowData?.Seq == _currentDto.Header.CurrentStopSeq.Value && rowData.StopStatus != RouteStopStatus.Departed.ToString())
                {
                    e.CellStyle.BackColor = Color.LightYellow; e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    // Reset màu nền/chữ
                    Color defaultBackColor = (e.RowIndex % 2 == 0)
                       ? (dgvRouteStops.DefaultCellStyle?.BackColor ?? SystemColors.Window) // Lấy màu mặc định hoặc màu cửa sổ
                       : (dgvRouteStops.AlternatingRowsDefaultCellStyle?.BackColor ?? SystemColors.Control); // Lấy màu xen kẽ hoặc màu control
                    Color defaultForeColor = dgvRouteStops.DefaultCellStyle?.ForeColor ?? SystemColors.ControlText; // Lấy màu chữ mặc định

                    // Chỉ reset nếu màu hiện tại khác màu mặc định
                    if (e.CellStyle.BackColor != defaultBackColor) e.CellStyle.BackColor = defaultBackColor;
                    if (e.CellStyle.ForeColor != defaultForeColor) e.CellStyle.ForeColor = defaultForeColor;
                }
            }
        }

        private string FormatShipmentStatusString(string statusString)
        {
            if (Enum.TryParse<ShipmentStatus>(statusString, out var status)) { return FormatShipmentStatus(status); }
            return statusString ?? "N/A";
        }

        private string FormatShipmentStatus(ShipmentStatus status)
        {
            switch (status)
            {
                case ShipmentStatus.Pending: return "Chờ nhận";
                case ShipmentStatus.Assigned: return "Đã nhận";
                case ShipmentStatus.OnRoute: return "Đang đi đường";
                case ShipmentStatus.AtWarehouse: return "Đang ở kho";
                case ShipmentStatus.ArrivedDestination: return "Đã tới đích";
                case ShipmentStatus.Delivered: return "Đã giao xong";
                case ShipmentStatus.Failed: return "Gặp sự cố";
                default: return status.ToString();
            }
        }

        private string FormatRouteStopStatus(RouteStopStatus status)
        {
            switch (status)
            {
                case RouteStopStatus.Waiting: return "Chờ";
                case RouteStopStatus.Arrived: return "Đã đến";
                case RouteStopStatus.Departed: return "Đã rời";
                default: return status.ToString();
            }
        }
        #endregion
    }
}