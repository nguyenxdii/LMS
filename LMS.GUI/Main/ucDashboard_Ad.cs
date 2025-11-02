//using LMS.BUS.Dtos;
//using LMS.BUS.Helpers;
//using LMS.BUS.Services;
//using LMS.DAL.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace LMS.GUI.Main
//{
//    public partial class ucDashboard_Ad : UserControl
//    {
//        // ====== Services ======
//        private readonly ShipmentService_Admin _shipmentSvc = new ShipmentService_Admin();
//        private readonly OrderService_Admin _orderSvc = new OrderService_Admin();
//        // Nếu bạn có OrderService_Admin thì bật dòng dưới, còn không cứ để null và lưới sẽ rỗng (không lỗi)
//        // private readonly OrderService_Admin _orderSvc = new OrderService_Admin();
//        // private readonly StatisticsService _statsSvc = new StatisticsService();

//        // ====== Helpers ======
//        private readonly ToolTip _tip = new ToolTip { IsBalloon = true };

//        public ucDashboard_Ad()
//        {
//            InitializeComponent();
//            AutoScroll = true;            // cho phép cuộn toàn UC
//            DoubleBuffered = true;        // mượt hơn khi vẽ

//            this.Load += UcDashboard_Ad_Load;
//            WireGridEvents();
//            ConfigureGrids();
//        }

//        private void UcDashboard_Ad_Load(object sender, EventArgs e)
//        {
//            TryLoadKpisAndCharts();
//            LoadPendingOrders();
//            LoadActiveShipments();
//        }

//        // =========================================================
//        // KPI + CHARTS (bạn gắn service thật vào 3 hàm Get* là xong)
//        // =========================================================
//        private void TryLoadKpisAndCharts()
//        {
//            try
//            {
//                var monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
//                var prevMonthStart = monthStart.AddMonths(-1);
//                var prevMonthEnd = monthStart.AddTicks(-1);

//                // Doanh thu tháng & chênh lệch
//                decimal revenueThis = GetMonthRevenue(monthStart, DateTime.Today);
//                decimal revenuePrev = GetMonthRevenue(prevMonthStart, prevMonthEnd);
//                lblKpiRevenue.Text = $"{FormatVnCurrency(revenueThis)} / tháng";
//                SetHoverDelta(pnlKpiRevenue, revenueThis, revenuePrev, "Doanh thu");

//                // Đơn chờ duyệt trong tháng
//                int pendingOrdersThis = GetMonthOrderCount(monthStart, DateTime.Today, OrderStatus.Pending);
//                int pendingOrdersPrev = GetMonthOrderCount(prevMonthStart, prevMonthEnd, OrderStatus.Pending);
//                lblKpiPendingOrders.Text = $"{pendingOrdersThis:N0} / tháng";
//                SetHoverDelta(pnlKpiPendingOrders, pendingOrdersThis, pendingOrdersPrev, "Đơn chờ duyệt");

//                // Chuyến đang chạy trong tháng (Assigned + OnRoute + AtWarehouse)
//                int activeThis = GetMonthActiveShipments(monthStart, DateTime.Today);
//                int activePrev = GetMonthActiveShipments(prevMonthStart, prevMonthEnd);
//                lblKpiActiveShipments.Text = $"{activeThis:N0} / tháng";
//                SetHoverDelta(pnlKpiActiveShipments, activeThis, activePrev, "Chuyến đang chạy");

//                // Tổng khách hàng (tăng so với tháng trước)
//                int cusThis = GetTotalCustomers(to: DateTime.Today);
//                int cusPrev = GetTotalCustomers(to: prevMonthEnd);
//                lblKpiTotalCustomers.Text = $"{cusThis:N0} / tháng";
//                SetHoverDelta(pnlKpiTotalCustomers, cusThis, cusPrev, "Tổng khách hàng");

//                // Tổng chuyến hàng trong tháng
//                int shpThis = GetMonthTotalShipments(monthStart, DateTime.Today);
//                int shpPrev = GetMonthTotalShipments(prevMonthStart, prevMonthEnd);
//                lblKpiTotalShipments.Text = $"{shpThis:N0} / tháng";
//                SetHoverDelta(pnlKpiTotalShipments, shpThis, shpPrev, "Tổng số chuyến hàng");

//                // Chart Doanh thu 14 ngày (line)
//                BindRevenue14Days();

//                // Pie trạng thái đơn hàng
//                BindOrderStatusPie();
//            }
//            catch
//            {
//                // Nếu chưa có StatisticsService/DB đầy đủ, vẫn chạy UI
//                lblKpiRevenue.Text = "-";
//                lblKpiPendingOrders.Text = "-";
//                lblKpiActiveShipments.Text = "-";
//                lblKpiTotalCustomers.Text = "-";
//                lblKpiTotalShipments.Text = "-";
//            }
//        }

//        private void SetHoverDelta(Control pnl, decimal current, decimal previous, string label)
//        {
//            string delta = BuildDeltaText(current, previous);
//            _tip.SetToolTip(pnl, $"{label}: {delta}");
//        }
//        private void SetHoverDelta(Control pnl, int current, int previous, string label)
//        {
//            string delta = BuildDeltaText(current, previous);
//            _tip.SetToolTip(pnl, $"{label}: {delta}");
//        }
//        private string BuildDeltaText(decimal current, decimal previous)
//        {
//            if (previous == 0) return "So với tháng trước: —";
//            var diff = current - previous;
//            var pct = (double)diff / (double)Math.Abs(previous) * 100.0;
//            return $"{(diff >= 0 ? "+" : "")}{FormatVnCurrency(diff)} ({pct:0.#}%)";
//        }
//        private string BuildDeltaText(int current, int previous)
//        {
//            if (previous == 0) return "So với tháng trước: —";
//            var diff = current - previous;
//            var pct = (double)diff / (double)Math.Abs(previous) * 100.0;
//            return $"{(diff >= 0 ? "+" : "")}{diff:N0} ({pct:0.#}%)";
//        }
//        private string FormatVnCurrency(decimal amount)
//            => string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:c0}", amount);

//        // ====== STATS data providers (bạn map sang StatisticsService nếu đã có) ======
//        private decimal GetMonthRevenue(DateTime from, DateTime to)
//        {
//            // Gợi ý: _statsSvc.GetRevenueTotal(from, to)
//            // Tạm thời trả 0 để không lỗi nếu chưa có service
//            return 0m;
//        }
//        private int GetMonthOrderCount(DateTime from, DateTime to, OrderStatus status)
//        {
//            // Gợi ý: _statsSvc.CountOrders(from, to, status)
//            return 0;
//        }
//        private int GetMonthActiveShipments(DateTime from, DateTime to)
//        {
//            // Gợi ý: Count Shipment status in (Assigned, OnRoute, AtWarehouse) trong khoảng tháng
//            return 0;
//        }
//        private int GetTotalCustomers(DateTime to)
//        {
//            // Gợi ý: _statsSvc.CountCustomers(to)
//            return 0;
//        }
//        private int GetMonthTotalShipments(DateTime from, DateTime to)
//        {
//            // Gợi ý: _statsSvc.CountShipments(from, to)
//            return 0;
//        }

//        // =======================
//        // 1️⃣ Line Chart – Doanh thu 14 ngày
//        // =======================
//        private void BindRevenue14Days()
//        {
//            // Clear series cũ
//            chartRevenue.Series.Clear();

//            // Giả lập dữ liệu 14 ngày gần nhất
//            var rnd = new Random();
//            var values = new LiveCharts.ChartValues<decimal>();
//            var labels = new List<string>();
//            for (int i = 13; i >= 0; i--)
//            {
//                var day = DateTime.Today.AddDays(-i);
//                values.Add(rnd.Next(10, 100)); // giá trị ngẫu nhiên
//                labels.Add(day.ToString("dd/MM"));
//            }

//            var series = new LiveCharts.Wpf.LineSeries
//            {
//                Title = "Doanh thu (triệu đồng)",
//                Values = new LiveCharts.ChartValues<decimal>(values),
//                PointGeometrySize = 10,
//                StrokeThickness = 2,
//                Fill = System.Windows.Media.Brushes.Transparent,
//                Stroke = System.Windows.Media.Brushes.DodgerBlue,
//                DataLabels = false
//            };

//            chartRevenue.Series.Add(series);
//            chartRevenue.AxisX.Clear();
//            chartRevenue.AxisY.Clear();

//            chartRevenue.AxisX.Add(new LiveCharts.Wpf.Axis
//            {
//                Title = "Ngày",
//                Labels = labels,
//                Separator = new LiveCharts.Wpf.Separator { Step = 1, IsEnabled = false }
//            });

//            chartRevenue.AxisY.Add(new LiveCharts.Wpf.Axis
//            {
//                Title = "Doanh thu (triệu)",
//                LabelFormatter = value => $"{value:N0}"
//            });
//        }

//        // =======================
//        // 2️⃣ Pie Chart – Trạng thái đơn hàng
//        // =======================
//        private void BindOrderStatusPie()
//        {
//            chartOrderStatus.Series.Clear();

//            // Các series PieSeries trực tiếp (không cần SeriesCollection)
//            chartOrderStatus.Series = new LiveCharts.SeriesCollection
//            {
//                new LiveCharts.Wpf.PieSeries
//                {
//                    Title = "Chờ duyệt",
//                    Values = new LiveCharts.ChartValues<int> { 10 },
//                    Fill = System.Windows.Media.Brushes.Gold
//                },
//                new LiveCharts.Wpf.PieSeries
//                {
//                    Title = "Đã duyệt",
//                    Values = new LiveCharts.ChartValues<int> { 25 },
//                    Fill = System.Windows.Media.Brushes.DeepSkyBlue
//                },
//                new LiveCharts.Wpf.PieSeries
//                {
//                    Title = "Hoàn thành",
//                    Values = new LiveCharts.ChartValues<int> { 40 },
//                    Fill = System.Windows.Media.Brushes.MediumSeaGreen
//                },
//                new LiveCharts.Wpf.PieSeries
//                {
//                    Title = "Đã hủy",
//                    Values = new LiveCharts.ChartValues<int> { 5 },
//                    Fill = System.Windows.Media.Brushes.IndianRed
//                }
//            };
//        }



//        // =========================================================
//        //  GRIDS
//        // =========================================================
//        private void ConfigureGrids()
//        {
//            // ---- Pending Orders ----
//            dgvPendingOrders.ApplyBaseStyle();
//            dgvPendingOrders.AutoGenerateColumns = false;
//            dgvPendingOrders.Columns.Clear();
//            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PendingOrderRow.OrderNo), HeaderText = "Mã đơn", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PendingOrderRow.CustomerName), HeaderText = "Khách hàng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
//            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PendingOrderRow.CreatedAt), HeaderText = "Ngày tạo", DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PendingOrderRow.Amount), HeaderText = "Giá trị", DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PendingOrderRow.StatusText), HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

//            // ---- Active Shipments ----
//            dgvActiveShipments.ApplyBaseStyle();
//            dgvActiveShipments.AutoGenerateColumns = false;
//            dgvActiveShipments.Columns.Clear();
//            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ShipmentListItemAdminDto.ShipmentNo), HeaderText = "Mã CH", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ShipmentListItemAdminDto.Route), HeaderText = "Tuyến đường", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
//            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ShipmentListItemAdminDto.DriverName), HeaderText = "Tài xế", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ShipmentListItemAdminDto.UpdatedAt), HeaderText = "Cập nhật", DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ShipmentListItemAdminDto.Status), HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//        }

//        private void WireGridEvents()
//        {
//            dgvPendingOrders.CellDoubleClick += DgvPendingOrders_CellDoubleClick;
//            dgvActiveShipments.CellDoubleClick += DgvActiveShipments_CellDoubleClick;
//            dgvActiveShipments.CellFormatting += DgvActiveShipments_CellFormatting;
//        }

//        // ---- Load data cho Pending Orders ----
//        //private void LoadPendingOrders()
//        //{
//        //    try
//        //    {
//        //        // Nếu bạn có OrderService_Admin: gọi service thật ở đây
//        //        // var data = _orderSvc.GetPendingOrdersLite(); // trả về List<PendingOrderRow>

//        //        // Tạm thời để danh sách rỗng (không lỗi, UI chạy):
//        //        var data = new List<PendingOrderRow>();
//        //        dgvPendingOrders.DataSource = new BindingList<PendingOrderRow>(data);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        MessageBox.Show("Lỗi tải 'Đơn hàng cần duyệt': " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//        //        dgvPendingOrders.DataSource = null;
//        //    }
//        //}

//        private void LoadPendingOrders()
//        {
//            try
//            {
//                // Lấy các đơn chờ duyệt – bạn có thể đổi hàm cho khớp service hiện có
//                var orders = _orderSvc.GetOrdersByStatus(OrderStatus.Pending)
//                                      .OrderByDescending(o => o.CreatedAt)
//                                      .Take(20) // muốn bao nhiêu tùy bạn
//                                      .ToList();

//                var rows = orders.Select(o => new PendingOrderRow
//                {
//                    OrderId = o.Id,
//                    OrderNo = string.IsNullOrWhiteSpace(o.OrderNo) ? OrderCode.ToCode(o.Id) : o.OrderNo,
//                    CustomerName = o.Customer != null ? o.Customer.Name : $"#{o.CustomerId}",
//                    CreatedAt = o.CreatedAt,
//                    Amount = o.TotalFee.ToString("N0"),
//                    StatusText = "Chờ duyệt"
//                }).ToList();

//                // nếu muốn giữ OrderId ẩn trong lưới, thêm 1 cột ẩn hoặc truy cập từ DataBoundItem
//                dgvPendingOrders.DataSource = new BindingList<PendingOrderRow>(rows);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi tải 'Đơn hàng cần duyệt': " + ex.Message, "Lỗi",
//                                MessageBoxButtons.OK, MessageBoxIcon.Error);
//                dgvPendingOrders.DataSource = null;
//            }
//        }


//        // ---- Load data cho Active Shipments ----
//        private void LoadActiveShipments()
//        {
//            try
//            {
//                var all = _shipmentSvc.GetAllShipmentsForAdmin();
//                // Lọc các trạng thái được xem là đang chạy
//                var active = all.Where(s =>
//                        s.Status == ShipmentStatus.Assigned ||
//                        s.Status == ShipmentStatus.OnRoute ||
//                        s.Status == ShipmentStatus.AtWarehouse)
//                    .OrderByDescending(x => x.UpdatedAt ?? x.StartedAt ?? DateTime.MinValue)
//                    .ToList();

//                dgvActiveShipments.DataSource = new BindingList<ShipmentListItemAdminDto>(active);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi tải 'Chuyến đang chạy': " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                dgvActiveShipments.DataSource = null;
//            }
//        }

//        // =========================================================
//        //  INTERACTION
//        // =========================================================

//        // Double-click Pending Orders -> chuyển ucOrder_Admin & chọn đúng đơn
//        //private void DgvPendingOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
//        //{
//        //    if (e.RowIndex < 0) return;
//        //    var row = dgvPendingOrders.Rows[e.RowIndex].DataBoundItem as PendingOrderRow;
//        //    if (row == null) return;

//        //    var frm = this.FindForm() as LMS.GUI.Main.frmMain_Admin;
//        //    if (frm == null) return;

//        //    // Hàm bên frmMain_Admin: Load UC Order_Admin và chọn đơn
//        //    frm.NavigateToOrderAdmin(row.OrderNo);
//        //}
//        private void DgvPendingOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex < 0) return;
//            var row = dgvPendingOrders.Rows[e.RowIndex].DataBoundItem as PendingOrderRow;
//            if (row == null) return;

//            ShowOrderDetailDialog(row.OrderId);
//        }


//        // Double-click Active Shipments -> popup xem thông tin
//        private void DgvActiveShipments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex < 0) return;
//            var dto = dgvActiveShipments.Rows[e.RowIndex].DataBoundItem as ShipmentListItemAdminDto;
//            if (dto == null) return;

//            try
//            {
//                var detail = _shipmentSvc.GetShipmentDetailForAdmin(dto.Id);

//                using (var f = new Form
//                {
//                    StartPosition = FormStartPosition.CenterScreen,
//                    FormBorderStyle = FormBorderStyle.FixedDialog,
//                    MinimizeBox = false,
//                    MaximizeBox = false,
//                    Text = $"Thông tin chuyến {detail?.Header?.ShipmentNo}",
//                    Width = 720,
//                    Height = 520
//                })
//                {
//                    var txt = new TextBox
//                    {
//                        Multiline = true,
//                        ReadOnly = true,
//                        Dock = DockStyle.Fill,
//                        ScrollBars = ScrollBars.Vertical
//                    };

//                    txt.Text =
//                        $"Mã CH: {detail.Header.ShipmentNo}\r\n" +
//                        $"Mã đơn: {detail.Header.OrderNo}\r\n" +
//                        $"Khách hàng: {detail.Header.CustomerName}\r\n" +
//                        $"Tuyến: {detail.Header.Route}\r\n" +
//                        $"Trạng thái: {detail.Header.Status}\r\n" +
//                        $"Bắt đầu: {detail.Header.StartedAt:dd/MM/yyyy HH:mm}\r\n" +
//                        $"Kết thúc: {detail.Header.DeliveredAt:dd/MM/yyyy HH:mm}\r\n" +
//                        $"Tài xế: {detail.DriverName}\r\n" +
//                        $"Xe: {detail.VehicleNo}\r\n" +
//                        $"Ghi chú: {detail.Notes}\r\n" +
//                        $"\r\n--- Các chặng ---\r\n" +
//                        string.Join("\r\n", detail.Stops.Select(s =>
//                            $"#{s.Seq} - {s.StopName} | ETA: {ToText(s.PlannedETA)} | Arrive: {ToText(s.ArrivedAt)} | Depart: {ToText(s.DepartedAt)} | {s.StopStatus}"));

//                    f.Controls.Add(txt);
//                    f.ShowDialog(this);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Không tải được thông tin chuyến.\r\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private string ToText(DateTime? dt) => dt.HasValue ? dt.Value.ToString("dd/MM/yyyy HH:mm") : "—";

//        // Format cột trạng thái cho Active Shipments
//        private void DgvActiveShipments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            if (dgvActiveShipments.Columns[e.ColumnIndex].DataPropertyName == nameof(ShipmentListItemAdminDto.Status)
//                && e.Value is ShipmentStatus st)
//            {
//                e.Value = StatusMapper.ToShipmentVN(st); // nếu bạn có StatusMapper; nếu không, comment dòng này
//                e.FormattingApplied = true;
//            }
//        }

//        // =========================================================
//        //  ViewModels cho lưới Pending Orders
//        // =========================================================
//        private class PendingOrderRow
//        {
//            public int OrderId { get; set; }   // <-- thêm
//            public string OrderNo { get; set; }
//            public string CustomerName { get; set; }
//            public DateTime? CreatedAt { get; set; }
//            public string Amount { get; set; }     // hiển thị tiền đã format để khỏi lệ thuộc property DAL
//            public string StatusText { get; set; }
//        }
//    }
//}
using LMS.BUS.Helpers;           // OrderCode, GridHelper.ApplyBaseStyle()
using LMS.GUI.OrderAdmin;        // ucOrderDetail_Admin
using LMS.DAL;
using LMS.DAL.Models;            // OrderStatus, ShipmentStatus, entities
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

// LiveCharts (dùng WPF series, host bởi WinForms controls)
using LiveCharts;
using LiveCharts.Wpf;

namespace LMS.GUI.Main
{
    public partial class ucDashboard_Ad : UserControl
    {
        private Panel _scrollHost;
        private readonly ToolTip _tip = new ToolTip { IsBalloon = true };

        public ucDashboard_Ad()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                this.DoubleBuffered = true;
                this.Load += ucDashboard_Ad_Load;

                ConfigurePendingOrdersGrid();
                ConfigureActiveShipmentsGrid();

                // chỉ gắn double-click cho PendingOrders
                dgvPendingOrders.CellDoubleClick += DgvPendingOrders_CellDoubleClick;
                // KHÔNG gắn cho dgvActiveShipments nữa
            }
        }

        // ==== Wrap toàn bộ nội dung vào panel cuộn (không cần sửa Designer) ====
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (DesignMode) return;

            if (_scrollHost == null)
            {
                _scrollHost = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    AutoScrollMinSize = new Size(0, 50)
                };

                var children = this.Controls.Cast<Control>().ToArray();
                this.Controls.Clear();
                _scrollHost.SuspendLayout();
                foreach (var c in children) _scrollHost.Controls.Add(c);
                _scrollHost.ResumeLayout();

                this.Controls.Add(_scrollHost);
            }
        }

        private void ucDashboard_Ad_Load(object sender, EventArgs e)
        {
            try
            {
                // Bố cục gợi ý để cuộn mượt (đổi cho khớp tên panel Designer nếu có):
                // Ví dụ bạn có các panel: pnlKpis, pnlCharts, pnlGrids.
                // Nếu không có, bỏ phần này.
                //try
                //{
                //    if (pnlKpis != null) { pnlKpis.Dock = DockStyle.Top; pnlKpis.Height = 120; }
                //    if (pnlCharts != null) { pnlCharts.Dock = DockStyle.Top; pnlCharts.Height = 360; }
                //    if (pnlGrids != null) { pnlGrids.Dock = DockStyle.Top; pnlGrids.Height = 440; }
                //}
                //catch { /* ignore if panels not exist */ }

                LoadKpis();
                BindRevenue30Days();
                BindOrderStatusPie();
                LoadPendingOrders();
                LoadActiveShipments();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dashboard admin: " + ex.Message,
                    "LMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===================== KPI (có hover so sánh tháng trước) =====================

        private void LoadKpis()
        {
            using (var db = new LogisticsDbContext())
            {
                // base numbers
                int pendingOrders = db.Orders.AsNoTracking()
                                             .Count(o => o.Status == OrderStatus.Pending);

                int activeShipments = db.Shipments.AsNoTracking()
                                                  .Count(s => s.StartedAt != null && s.DeliveredAt == null);

                int totalCustomers = db.Customers.AsNoTracking().Count();
                int totalShipments = db.Shipments.AsNoTracking().Count();

                var monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var prevMonthStart = monthStart.AddMonths(-1);
                var prevMonthEnd = monthStart.AddTicks(-1);

                decimal revenueThisMonth = db.Orders.AsNoTracking()
                                                    .Where(o => o.Status == OrderStatus.Completed &&
                                                                o.CreatedAt >= monthStart)
                                                    .Select(o => o.TotalFee)
                                                    .DefaultIfEmpty(0m)
                                                    .Sum();

                decimal revenuePrevMonth = db.Orders.AsNoTracking()
                                                    .Where(o => o.Status == OrderStatus.Completed &&
                                                                o.CreatedAt >= prevMonthStart &&
                                                                o.CreatedAt <= prevMonthEnd)
                                                    .Select(o => o.TotalFee)
                                                    .DefaultIfEmpty(0m)
                                                    .Sum();

                // gán label
                lblKpiPendingOrders.Text = pendingOrders.ToString("N0");
                lblKpiActiveShipments.Text = activeShipments.ToString("N0");
                lblKpiTotalCustomers.Text = totalCustomers.ToString("N0");
                lblKpiTotalShipments.Text = totalShipments.ToString("N0");
                lblKpiRevenue.Text = revenueThisMonth.ToString("N0");

                // hover (so với tháng trước)
                SetHoverDelta(pnlKpiPendingOrders, pendingOrders, CountOrders(prevMonthStart, prevMonthEnd, OrderStatus.Pending), "Đơn chờ duyệt");
                SetHoverDelta(pnlKpiActiveShipments, activeShipments, CountActiveShipments(prevMonthStart, prevMonthEnd), "Chuyến đang chạy");
                SetHoverDelta(pnlKpiTotalCustomers, totalCustomers, CountCustomers(prevMonthEnd), "Tổng khách hàng");
                SetHoverDelta(pnlKpiTotalShipments, totalShipments, CountShipments(prevMonthStart, prevMonthEnd), "Tổng số chuyến");
                SetHoverDelta(pnlKpiRevenue, revenueThisMonth, revenuePrevMonth, "Doanh thu");
            }
        }

        private void SetHoverDelta(Control pnl, int current, int previous, string label)
        {
            string delta = BuildDeltaText(current, previous);
            _tip.SetToolTip(pnl, $"{label}: {delta}");
        }
        private void SetHoverDelta(Control pnl, decimal current, decimal previous, string label)
        {
            string delta = BuildDeltaText(current, previous);
            _tip.SetToolTip(pnl, $"{label}: {delta}");
        }
        private string BuildDeltaText(int current, int previous)
        {
            if (previous == 0) return "So với tháng trước: —";
            var diff = current - previous;
            var pct = (double)diff / Math.Abs(previous) * 100.0;
            return $"{(diff >= 0 ? "+" : "")}{diff:N0} ({pct:0.#}%)";
        }
        private string BuildDeltaText(decimal current, decimal previous)
        {
            if (previous == 0) return "So với tháng trước: —";
            var diff = current - previous;
            var pct = (double)diff / (double)Math.Abs(previous) * 100.0;
            return $"{(diff >= 0 ? "+" : "")}{diff:N0} ({pct:0.#}%)";
        }

        // quick helpers cho KPI hover (đọc DB)
        private int CountOrders(DateTime from, DateTime to, OrderStatus st)
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Orders.AsNoTracking()
                        .Count(o => o.Status == st && o.CreatedAt >= from && o.CreatedAt <= to);
            }
        }
        private int CountActiveShipments(DateTime from, DateTime to)
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Shipments.AsNoTracking()
                        .Count(s => s.StartedAt != null && s.DeliveredAt == null &&
                                    (s.UpdatedAt ?? s.StartedAt) >= from &&
                                    (s.UpdatedAt ?? s.StartedAt) <= to);
            }
        }
        private int CountCustomers(DateTime to)
        {
            using (var db = new LogisticsDbContext())
            {
                // Customer không có CreatedAt => đếm toàn bộ (vì không có thời gian tạo)
                return db.Customers.AsNoTracking().Count();
            }
        }

        private int CountShipments(DateTime from, DateTime to)
        {
            using (var db = new LogisticsDbContext())
            {
                // Shipment không có CreatedAt => dùng StartedAt làm mốc
                return db.Shipments.AsNoTracking()
                        .Count(s => s.StartedAt != null &&
                                    s.StartedAt >= from && s.StartedAt <= to);
            }
        }


        // ======================= CHARTS =======================

        // 1) Doanh thu 30 ngày gần đây
        private void BindRevenue30Days()
        {
            // lấy 30 ngày
            DateTime today = DateTime.Today;
            DateTime start = today.AddDays(-29);

            List<string> labels = Enumerable.Range(0, 30)
                                            .Select(i => start.AddDays(i).ToString("dd/MM"))
                                            .ToList();

            decimal[] values = new decimal[30];

            using (var db = new LogisticsDbContext())
            {
                var data = db.Orders.AsNoTracking()
                              .Where(o => o.Status == OrderStatus.Completed &&
                                          DbFunctions.TruncateTime(o.CreatedAt) >= start &&
                                          DbFunctions.TruncateTime(o.CreatedAt) <= today)
                              .GroupBy(o => DbFunctions.TruncateTime(o.CreatedAt))
                              .Select(g => new
                              {
                                  Day = g.Key.Value,
                                  Sum = g.Sum(x => x.TotalFee)
                              })
                              .ToList();

                foreach (var x in data)
                {
                    int idx = (x.Day - start).Days;
                    if (idx >= 0 && idx < 30) values[idx] = x.Sum;
                }
            }

            // clear & add series
            chartRevenue.Series = new SeriesCollection();
            chartRevenue.AxisX.Clear();
            chartRevenue.AxisY.Clear();

            var line = new LineSeries
            {
                Title = "Doanh thu (₫)",
                Values = new ChartValues<decimal>(values),
                PointGeometrySize = 8,
                StrokeThickness = 2,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = System.Windows.Media.Brushes.DodgerBlue,
                DataLabels = false
            };

            chartRevenue.Series.Add(line);
            chartRevenue.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Ngày",
                Labels = labels,
                Separator = new LiveCharts.Wpf.Separator { Step = 1, IsEnabled = false }
            });
            chartRevenue.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "₫",
                LabelFormatter = v => v.ToString("N0")
            });
        }

        // 2) Pie trạng thái đơn hàng
        private void BindOrderStatusPie()
        {
            int pending = 0, approved = 0, completed = 0, cancelled = 0;

            using (var db = new LogisticsDbContext())
            {
                var groups = db.Orders.AsNoTracking()
                                .GroupBy(o => o.Status)
                                .Select(g => new { Status = g.Key, Count = g.Count() })
                                .ToList();

                foreach (var g in groups)
                {
                    switch (g.Status)
                    {
                        case OrderStatus.Pending: pending = g.Count; break;
                        case OrderStatus.Approved: approved = g.Count; break;
                        case OrderStatus.Completed: completed = g.Count; break;
                        case OrderStatus.Cancelled: cancelled = g.Count; break;
                    }
                }
            }

            chartOrderStatus.Series = new SeriesCollection
            {
                new PieSeries { Title = "Chờ duyệt",  Values = new ChartValues<int> { pending   } },
                new PieSeries { Title = "Đã duyệt",   Values = new ChartValues<int> { approved  } },
                new PieSeries { Title = "Hoàn thành", Values = new ChartValues<int> { completed } },
                new PieSeries { Title = "Đã huỷ",     Values = new ChartValues<int> { cancelled } }
            };
        }

        // ======================= GRIDS =======================

        // PENDING
        private void ConfigurePendingOrdersGrid()
        {
            dgvPendingOrders.ApplyBaseStyle();
            dgvPendingOrders.AutoGenerateColumns = false;
            dgvPendingOrders.Columns.Clear();

            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderId",
                DataPropertyName = nameof(PendingOrderRow.OrderId),
                Visible = false
            });
            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Mã đơn",
                DataPropertyName = nameof(PendingOrderRow.OrderNo),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Khách hàng",
                DataPropertyName = nameof(PendingOrderRow.CustomerName),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Ngày tạo",
                DataPropertyName = nameof(PendingOrderRow.CreatedAt),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Giá trị",
                DataPropertyName = nameof(PendingOrderRow.Amount),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Trạng thái",
                DataPropertyName = nameof(PendingOrderRow.StatusText),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
        }

        private void LoadPendingOrders()
        {
            using (var db = new LogisticsDbContext())
            {
                var orders = db.Orders.AsNoTracking()
                               .Include(o => o.Customer)
                               .Where(o => o.Status == OrderStatus.Pending)
                               .OrderByDescending(o => o.CreatedAt)
                               .Take(30)
                               .ToList();

                var rows = orders.Select(o => new PendingOrderRow
                {
                    OrderId = o.Id,
                    OrderNo = string.IsNullOrWhiteSpace(o.OrderNo) ? OrderCode.ToCode(o.Id) : o.OrderNo,
                    CustomerName = o.Customer != null ? o.Customer.Name : $"#{o.CustomerId}",
                    CreatedAt = o.CreatedAt,
                    Amount = o.TotalFee.ToString("N0"),
                    StatusText = "Chờ duyệt"
                }).ToList();

                dgvPendingOrders.DataSource = new BindingList<PendingOrderRow>(rows);
            }
        }

        private void DgvPendingOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvPendingOrders.Rows[e.RowIndex].DataBoundItem as PendingOrderRow;
            if (row == null) return;

            ShowOrderDetailDialog(row.OrderId);
        }

        private class PendingOrderRow
        {
            public int OrderId { get; set; }
            public string OrderNo { get; set; }
            public string CustomerName { get; set; }
            public DateTime? CreatedAt { get; set; }
            public string Amount { get; set; }
            public string StatusText { get; set; }
        }

        // ACTIVE SHIPMENTS
        private void ConfigureActiveShipmentsGrid()
        {
            dgvActiveShipments.ApplyBaseStyle();
            dgvActiveShipments.AutoGenerateColumns = false;
            dgvActiveShipments.Columns.Clear();

            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ShipmentId",
                DataPropertyName = nameof(ActiveShipmentRow.ShipmentId),
                Visible = false
            });
            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Mã CH",
                DataPropertyName = nameof(ActiveShipmentRow.ShipmentNo),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tuyến đường",
                DataPropertyName = nameof(ActiveShipmentRow.RouteText),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tài xế",
                DataPropertyName = nameof(ActiveShipmentRow.DriverName),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Cập nhật",
                DataPropertyName = nameof(ActiveShipmentRow.UpdatedAt),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Trạng thái",
                DataPropertyName = nameof(ActiveShipmentRow.StatusText),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
        }

        private void LoadActiveShipments()
        {
            using (var db = new LogisticsDbContext())
            {
                var list = db.Shipments.AsNoTracking()
                             .Include(s => s.Driver)
                             .Where(s => s.StartedAt != null && s.DeliveredAt == null)
                             .OrderByDescending(s => s.UpdatedAt ?? s.StartedAt)
                             .Take(30)
                             .ToList();

                var whIds = list.Select(s => s.FromWarehouseId)
                                .Concat(list.Select(s => s.ToWarehouseId))
                                .Distinct()
                                .ToList();

                var whDict = db.Warehouses.AsNoTracking()
                              .Where(w => whIds.Contains(w.Id))
                              .ToDictionary(w => w.Id, w => w.Name);

                var rows = list.Select(s => new ActiveShipmentRow
                {
                    ShipmentId = s.Id,
                    ShipmentNo = $"SHP{s.Id}",
                    RouteText = $"{(whDict.TryGetValue(s.FromWarehouseId, out var fromN) ? fromN : "Kho ?")} → " +
                                 $"{(whDict.TryGetValue(s.ToWarehouseId, out var toN) ? toN : "Kho ?")}",
                    DriverName = s.Driver != null ? s.Driver.FullName : "(Chưa gán)",
                    UpdatedAt = s.UpdatedAt ?? s.StartedAt,
                    StatusText = "Đang chạy"
                }).ToList();

                dgvActiveShipments.DataSource = new BindingList<ActiveShipmentRow>(rows);
            }
        }

        private class ActiveShipmentRow
        {
            public int ShipmentId { get; set; }
            public string ShipmentNo { get; set; }
            public string RouteText { get; set; }
            public string DriverName { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public string StatusText { get; set; }
        }

        // ======================= COMMON DIALOG =======================

        private void ShowOrderDetailDialog(int orderId)
        {
            using (var f = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Width = 496,
                Height = 496,
            })
            {
                var uc = new ucOrderDetail_Admin(orderId) { Dock = DockStyle.Fill };
                f.Controls.Add(uc);
                f.ShowDialog(this);
            }
        }


        // Public refresh
        public void RefreshDashboard()
        {
            LoadKpis();
            BindRevenue30Days();
            BindOrderStatusPie();
            LoadPendingOrders();
            LoadActiveShipments();
        }
    }
}
