// LMS.GUI/OrderCustomer/ucTracking_Cus.cs
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.GUI.Main; // Cần để cast FindForm()
using System;
using System.Collections.Generic; // Cần cho List/IEnumerable
using System.ComponentModel; // Cần cho SortOrder
using System.Drawing;
using System.Linq;
using System.Reflection; // Cần cho Reflection (Sort)
using System.Windows.Forms;

namespace LMS.GUI.OrderCustomer
{
    public partial class ucTracking_Cus : UserControl
    {
        private readonly OrderService_Customer _orderSvc = new OrderService_Customer();
        private readonly int _orderId; // ID đơn hàng cần hiển thị (bắt buộc)
        private readonly int _customerId;

        // Biến cho chức năng Sort
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        // Hàm khởi tạo - BẮT BUỘC phải có orderId
        public ucTracking_Cus(int customerId, int orderId)
        {
            InitializeComponent();
            _customerId = customerId;
            _orderId = orderId;
            this.Load += UcTracking_Cus_Load;
        }

        private void UcTracking_Cus_Load(object sender, EventArgs e)
        {
            ConfigureStopsGrid(); // Cấu hình Grid trước
            LoadOrderDetails();   // Tải chi tiết đơn hàng ngay lập tức

            // Gán sự kiện cho nút Back và Grid
            btnBack.Click += BtnBack_Click; // Gán sự kiện cho nút Quay lại mới
            dgvRouteStops.ColumnHeaderMouseClick += dgvRouteStops_ColumnHeaderMouseClick;
            // Không cần CellFormatting vì đã xử lý tiếng Việt trong BindOrder
            // dgvRouteStops.CellFormatting += dgvRouteStops_CellFormatting;
        }

        // Cấu hình giao diện và cột cho Grid hiển thị các chặng
        private void ConfigureStopsGrid()
        {
            var g = dgvRouteStops;
            g.Columns.Clear();
            g.ApplyBaseStyle();

            // --- Định nghĩa Cột ---
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Seq", DataPropertyName = "Seq", HeaderText = "#", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Warehouse", DataPropertyName = "Warehouse", HeaderText = "Kho", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic }); // Tăng FillWeight cho Kho
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "StatusVi", DataPropertyName = "StatusVi", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "ArrivedAt", DataPropertyName = "ArrivedAt", HeaderText = "Đến lúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DepartedAt", DataPropertyName = "DepartedAt", HeaderText = "Rời lúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Activity", DataPropertyName = "Activity", HeaderText = "Diễn giải", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 60, SortMode = DataGridViewColumnSortMode.NotSortable }); // Tăng FillWeight cho Diễn giải

            TryEnableDoubleBuffer(g);
        }

        // Helper bật DoubleBuffer
        private static void TryEnableDoubleBuffer(DataGridView grid)
        {
            try
            {
                var prop = grid.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                prop?.SetValue(grid, true, null);
            }
            catch { /* Bỏ qua lỗi */ }
        }

        // Tải chi tiết đơn hàng và các chặng
        private void LoadOrderDetails()
        {
            // Lấy thông tin đơn hàng dựa vào _orderId và _customerId (để bảo mật)
            var order = _orderSvc.GetOrderWithStops(_customerId, _orderId);

            if (order == null)
            {
                MessageBox.Show($"Không tìm thấy đơn hàng #{_orderId} hoặc bạn không có quyền xem.", "LMS");
                lblOrderStatus.Text = "— Không tìm thấy đơn hàng —";
                dgvRouteStops.DataSource = null;
                // Có thể tự động quay lại nếu muốn:
                // BtnBack_Click(this, EventArgs.Empty);
                return;
            }

            BindOrder(order); // Hiển thị thông tin lên giao diện
            ResetSortGlyphs(); // Reset trạng thái sort
        }

        // Hiển thị thông tin đơn hàng và các chặng lên giao diện
        private void BindOrder(Order order)
        {
            // Hiển thị trạng thái tổng quát của đơn hàng và shipment (nếu có)
            //lblOrderStatus.Text = order.Shipment != null
            //    ? $"Đơn #{OrderCode.ToCode(order.Id)} – {ToVi(order.Status)} – Chuyến #{order.Shipment.Id}: {ToVi(order.Shipment.Status)}"
            //    : $"Đơn #{OrderCode.ToCode(order.Id)} – {ToVi(order.Status)} (Chưa có chuyến)";
            // Trong hàm BindOrder(Order order) của ucTracking_Cus.cs

            lblOrderStatus.Text = order.Shipment != null
                ? $"Đơn #{OrderCode.ToCode(order.Id)} – {ToVi(order.Status)}\n" + // Xuống dòng
                  $"Chuyến #{order.Shipment.Id}: {ToVi(order.Shipment.Status)}"
                : $"Đơn #{OrderCode.ToCode(order.Id)} – {ToVi(order.Status)} (Chưa có chuyến)";

            var stops = (order.Shipment?.RouteStops ?? new List<RouteStop>())
                            .OrderBy(rs => rs.Seq)
                            .ToList();

            // Xác định chặng hiện tại (chặng đầu tiên chưa Departed)
            var current = stops.FirstOrDefault(s => s.Status != RouteStopStatus.Departed);

            // Tạo danh sách dữ liệu cho Grid (bao gồm diễn giải tiếng Việt)
            var rows = stops.Select(s => new
            {
                s.Seq,
                Warehouse = s.Warehouse?.Name ?? $"#{s.WarehouseId}",
                StatusVi = ToVi(s.Status), // Trạng thái chặng tiếng Việt
                s.ArrivedAt,
                s.DepartedAt,
                Activity = BuildActivityVi(s, current) // Diễn giải hoạt động tiếng Việt
            }).ToList();

            dgvRouteStops.DataSource = null; // Xóa nguồn cũ
            dgvRouteStops.DataSource = rows; // Gán nguồn mới
        }

        // --- Các hàm xử lý sự kiện ---

        // Sự kiện click nút Quay lại
        private void BtnBack_Click(object sender, EventArgs e)
        {
            var host = this.FindForm() as frmMain_Customer;
            // Quay về màn hình danh sách đơn hàng
            host?.LoadUc(new ucOrderList_Cus(_customerId));
            // Cập nhật lại tiêu đề trang nếu cần
            //if (host != null) host.lblPageTitle.Text = "Công Cụ / Danh Sách Đơn Hàng";
        }

        // Xử lý click header cột để sort
        private void dgvRouteStops_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var list = dgvRouteStops.DataSource as IEnumerable<object>;
            if (list == null || !list.Any()) return;

            var newColumn = dgvRouteStops.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            // Xác định hướng sort
            if (_sortedColumn == null) _sortOrder = SortOrder.Ascending;
            else if (_sortedColumn == newColumn) _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            else { _sortOrder = SortOrder.Ascending; if (_sortedColumn?.HeaderCell != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; }

            _sortedColumn = newColumn;

            // Sort bằng Reflection
            try
            {
                string propertyName = newColumn.DataPropertyName;
                var propInfo = list.First().GetType().GetProperty(propertyName);
                if (propInfo == null) return;

                IEnumerable<object> sortedList;
                if (_sortOrder == SortOrder.Ascending)
                    sortedList = list.OrderBy(x => propInfo.GetValue(x, null));
                else
                    sortedList = list.OrderByDescending(x => propInfo.GetValue(x, null));

                dgvRouteStops.DataSource = null;
                dgvRouteStops.DataSource = sortedList.ToList();
                newColumn.HeaderCell.SortGlyphDirection = _sortOrder;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sort Error on {newColumn.Name}: {ex.Message}");
                ResetSortGlyphs();
            }
        }

        // Reset mũi tên sort
        private void ResetSortGlyphs()
        {
            if (_sortedColumn?.HeaderCell != null)
            {
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            _sortedColumn = null;
            _sortOrder = SortOrder.None;
        }

        // --- Các hàm Helper chuyển Enum sang tiếng Việt ---

        private string BuildActivityVi(RouteStop s, RouteStop current)
        {
            if (current == null) return "Đã hoàn tất lộ trình";
            if (s.Seq == current.Seq)
            {
                if (s.Status == RouteStopStatus.Arrived) return $"Đang ở {s.Warehouse?.Name ?? $"kho #{s.WarehouseId}"}";
                if (s.Status == RouteStopStatus.Waiting) return $"Đang di chuyển đến {s.Warehouse?.Name ?? $"kho #{s.WarehouseId}"}";
            }
            if (s.Seq < current.Seq) return "Đã rời";
            return "Chưa đến";
        }

        private string ToVi(OrderStatus s)
        {
            switch (s)
            {
                case OrderStatus.Pending: return "Chờ duyệt";
                case OrderStatus.Approved: return "Đã duyệt";
                case OrderStatus.Completed: return "Hoàn thành"; // Sửa lại thành "Hoàn thành"
                case OrderStatus.Cancelled: return "Đã hủy";
                default: return s.ToString();
            }
        }

        private string ToVi(ShipmentStatus s)
        {
            switch (s)
            {
                case ShipmentStatus.Pending: return "Chờ tài xế nhận";
                case ShipmentStatus.Assigned: return "Đã nhận chuyến";
                case ShipmentStatus.OnRoute: return "Đang di chuyển";
                case ShipmentStatus.AtWarehouse: return "Đang ở kho";
                case ShipmentStatus.ArrivedDestination: return "Đã tới đích";
                case ShipmentStatus.Delivered: return "Đã giao xong";
                case ShipmentStatus.Failed: return "Gặp sự cố";
                default: return s.ToString();
            }
        }

        private string ToVi(RouteStopStatus s)
        {
            switch (s)
            {
                case RouteStopStatus.Waiting: return "Chờ";
                case RouteStopStatus.Arrived: return "Đã đến";
                case RouteStopStatus.Departed: return "Đã rời";
                default: return s.ToString();
            }
        }
    }
}