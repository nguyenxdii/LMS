// LMS.GUI/OrderCustomer/ucTracking_Cus.cs
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.GUI.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.OrderCustomer
{
    public partial class ucTracking_Cus : UserControl
    {
        private readonly OrderService_Customer _orderSvc = new OrderService_Customer();
        private readonly int _orderId;
        private readonly int _customerId;

        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        public ucTracking_Cus(int customerId, int orderId)
        {
            InitializeComponent();
            _customerId = customerId;
            _orderId = orderId;
            this.Load += UcTracking_Cus_Load;
        }

        // ===== INIT =====
        private void UcTracking_Cus_Load(object sender, EventArgs e)
        {
            ConfigureStopsGrid();
            LoadOrderDetails();

            btnBack.Click += BtnBack_Click;
            dgvRouteStops.ColumnHeaderMouseClick += dgvRouteStops_ColumnHeaderMouseClick;
        }

        // ===== GRID CONFIG =====
        private void ConfigureStopsGrid()
        {
            var g = dgvRouteStops;
            g.Columns.Clear();
            g.ApplyBaseStyle();

            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Seq", DataPropertyName = "Seq", HeaderText = "#", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Warehouse", DataPropertyName = "Warehouse", HeaderText = "Kho", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "StatusVi", DataPropertyName = "StatusVi", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "ArrivedAt", DataPropertyName = "ArrivedAt", HeaderText = "Đến lúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DepartedAt", DataPropertyName = "DepartedAt", HeaderText = "Rời lúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Activity", DataPropertyName = "Activity", HeaderText = "Diễn giải", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 60, SortMode = DataGridViewColumnSortMode.NotSortable });

            TryEnableDoubleBuffer(g);
        }

        private static void TryEnableDoubleBuffer(DataGridView grid)
        {
            try
            {
                var prop = grid.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                prop?.SetValue(grid, true, null);
            }
            catch { }
        }

        // ===== LOAD DATA =====
        private void LoadOrderDetails()
        {
            var order = _orderSvc.GetOrderWithStops(_customerId, _orderId);
            if (order == null)
            {
                MessageBox.Show($"Không tìm thấy đơn hàng #{_orderId} hoặc bạn không có quyền xem.", "LMS");
                lblOrderStatus.Text = "— Không tìm thấy đơn hàng —";
                dgvRouteStops.DataSource = null;
                return;
            }

            BindOrder(order);
            ResetSortGlyphs();
        }

        private void BindOrder(Order order)
        {
            // Hiển thị trạng thái đơn + chuyến
            if (order.Status == OrderStatus.Cancelled)
            {
                lblOrderStatus.Text =
                    $"Đơn #{OrderCode.ToCode(order.Id)} – {ToVi(order.Status)}\n" +
                    $"Lý do: {order.CancelReason ?? "Không có lý do cụ thể."}";
                lblOrderStatus.ForeColor = Color.Red;
            }
            else if (order.Shipment != null)
            {
                lblOrderStatus.Text =
                    $"Đơn #{OrderCode.ToCode(order.Id)} – {ToVi(order.Status)}\n" +
                    $"Chuyến #{order.Shipment.Id}: {ToVi(order.Shipment.Status)}";
                lblOrderStatus.ForeColor = Color.Black;
            }
            else
            {
                lblOrderStatus.Text =
                    $"Đơn #{OrderCode.ToCode(order.Id)} – {ToVi(order.Status)} (Chưa có chuyến)";
                lblOrderStatus.ForeColor = Color.Black;
            }

            // Route stops
            var stops = (order.Shipment?.RouteStops ?? new List<RouteStop>())
                .OrderBy(rs => rs.Seq)
                .ToList();

            var current = stops.FirstOrDefault(s => s.Status != RouteStopStatus.Departed);

            var rows = stops.Select(s => new
            {
                s.Seq,
                Warehouse = s.Warehouse?.Name ?? $"#{s.WarehouseId}",
                StatusVi = ToVi(s.Status),
                s.ArrivedAt,
                s.DepartedAt,
                Activity = BuildActivityVi(s, current) // diễn giải hành trình
            }).ToList();

            dgvRouteStops.DataSource = rows;
        }

        // ===== UI EVENTS =====
        private void BtnBack_Click(object sender, EventArgs e)
        {
            var host = this.FindForm() as frmMain_Customer;
            host?.LoadUc(new ucOrderList_Cus(_customerId));
        }

        private void dgvRouteStops_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var list = dgvRouteStops.DataSource as IEnumerable<object>;
            if (list == null || !list.Any()) return;

            var newColumn = dgvRouteStops.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            // Toggle sort
            if (_sortedColumn == null) _sortOrder = SortOrder.Ascending;
            else if (_sortedColumn == newColumn)
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            else
            {
                _sortOrder = SortOrder.Ascending;
                if (_sortedColumn?.HeaderCell != null)
                    _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            _sortedColumn = newColumn;

            // Sort reflection
            try
            {
                string prop = newColumn.DataPropertyName;
                var info = list.First().GetType().GetProperty(prop);
                if (info == null) return;

                IEnumerable<object> sorted =
                    (_sortOrder == SortOrder.Ascending)
                    ? list.OrderBy(x => info.GetValue(x, null))
                    : list.OrderByDescending(x => info.GetValue(x, null));

                dgvRouteStops.DataSource = sorted.ToList();
                newColumn.HeaderCell.SortGlyphDirection = _sortOrder;
            }
            catch
            {
                ResetSortGlyphs();
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn?.HeaderCell != null)
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;

            _sortedColumn = null;
            _sortOrder = SortOrder.None;
        }

        // ===== TEXT MAPPING =====
        private string BuildActivityVi(RouteStop s, RouteStop current)
        {
            if (current == null) return "Đã hoàn tất lộ trình";
            if (s.Seq == current.Seq)
            {
                if (s.Status == RouteStopStatus.Arrived)
                    return $"Đang ở {s.Warehouse?.Name ?? $"kho #{s.WarehouseId}"}";
                if (s.Status == RouteStopStatus.Waiting)
                    return $"Đang di chuyển đến {s.Warehouse?.Name ?? $"kho #{s.WarehouseId}"}";
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
                case OrderStatus.Completed: return "Hoàn thành";
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
