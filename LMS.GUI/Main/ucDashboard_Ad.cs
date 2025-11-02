using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.GUI.Main
{
    public partial class ucDashboard_Ad : UserControl
    {
        // ====== Services ======
        private readonly ShipmentService_Admin _shipmentSvc = new ShipmentService_Admin();
        // Nếu bạn có OrderService_Admin thì bật dòng dưới, còn không cứ để null và lưới sẽ rỗng (không lỗi)
        // private readonly OrderService_Admin _orderSvc = new OrderService_Admin();
        // private readonly StatisticsService _statsSvc = new StatisticsService();

        // ====== Helpers ======
        private readonly ToolTip _tip = new ToolTip { IsBalloon = true };

        public ucDashboard_Ad()
        {
            InitializeComponent();
            AutoScroll = true;            // cho phép cuộn toàn UC
            DoubleBuffered = true;        // mượt hơn khi vẽ

            this.Load += UcDashboard_Ad_Load;
            WireGridEvents();
            ConfigureGrids();
        }

        private void UcDashboard_Ad_Load(object sender, EventArgs e)
        {
            TryLoadKpisAndCharts();
            LoadPendingOrders();
            LoadActiveShipments();
        }

        // =========================================================
        // KPI + CHARTS (bạn gắn service thật vào 3 hàm Get* là xong)
        // =========================================================
        private void TryLoadKpisAndCharts()
        {
            try
            {
                var monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var prevMonthStart = monthStart.AddMonths(-1);
                var prevMonthEnd = monthStart.AddTicks(-1);

                // Doanh thu tháng & chênh lệch
                decimal revenueThis = GetMonthRevenue(monthStart, DateTime.Today);
                decimal revenuePrev = GetMonthRevenue(prevMonthStart, prevMonthEnd);
                lblKpiRevenue.Text = $"{FormatVnCurrency(revenueThis)} / tháng";
                SetHoverDelta(pnlKpiRevenue, revenueThis, revenuePrev, "Doanh thu");

                // Đơn chờ duyệt trong tháng
                int pendingOrdersThis = GetMonthOrderCount(monthStart, DateTime.Today, OrderStatus.Pending);
                int pendingOrdersPrev = GetMonthOrderCount(prevMonthStart, prevMonthEnd, OrderStatus.Pending);
                lblKpiPendingOrders.Text = $"{pendingOrdersThis:N0} / tháng";
                SetHoverDelta(pnlKpiPendingOrders, pendingOrdersThis, pendingOrdersPrev, "Đơn chờ duyệt");

                // Chuyến đang chạy trong tháng (Assigned + OnRoute + AtWarehouse)
                int activeThis = GetMonthActiveShipments(monthStart, DateTime.Today);
                int activePrev = GetMonthActiveShipments(prevMonthStart, prevMonthEnd);
                lblKpiActiveShipments.Text = $"{activeThis:N0} / tháng";
                SetHoverDelta(pnlKpiActiveShipments, activeThis, activePrev, "Chuyến đang chạy");

                // Tổng khách hàng (tăng so với tháng trước)
                int cusThis = GetTotalCustomers(to: DateTime.Today);
                int cusPrev = GetTotalCustomers(to: prevMonthEnd);
                lblKpiTotalCustomers.Text = $"{cusThis:N0} / tháng";
                SetHoverDelta(pnlKpiTotalCustomers, cusThis, cusPrev, "Tổng khách hàng");

                // Tổng chuyến hàng trong tháng
                int shpThis = GetMonthTotalShipments(monthStart, DateTime.Today);
                int shpPrev = GetMonthTotalShipments(prevMonthStart, prevMonthEnd);
                lblKpiTotalShipments.Text = $"{shpThis:N0} / tháng";
                SetHoverDelta(pnlKpiTotalShipments, shpThis, shpPrev, "Tổng số chuyến hàng");

                // Chart Doanh thu 14 ngày (line)
                BindRevenue14Days();

                // Pie trạng thái đơn hàng
                BindOrderStatusPie();
            }
            catch
            {
                // Nếu chưa có StatisticsService/DB đầy đủ, vẫn chạy UI
                lblKpiRevenue.Text = "-";
                lblKpiPendingOrders.Text = "-";
                lblKpiActiveShipments.Text = "-";
                lblKpiTotalCustomers.Text = "-";
                lblKpiTotalShipments.Text = "-";
            }
        }

        private void SetHoverDelta(Control pnl, decimal current, decimal previous, string label)
        {
            string delta = BuildDeltaText(current, previous);
            _tip.SetToolTip(pnl, $"{label}: {delta}");
        }
        private void SetHoverDelta(Control pnl, int current, int previous, string label)
        {
            string delta = BuildDeltaText(current, previous);
            _tip.SetToolTip(pnl, $"{label}: {delta}");
        }
        private string BuildDeltaText(decimal current, decimal previous)
        {
            if (previous == 0) return "So với tháng trước: —";
            var diff = current - previous;
            var pct = (double)diff / (double)Math.Abs(previous) * 100.0;
            return $"{(diff >= 0 ? "+" : "")}{FormatVnCurrency(diff)} ({pct:0.#}%)";
        }
        private string BuildDeltaText(int current, int previous)
        {
            if (previous == 0) return "So với tháng trước: —";
            var diff = current - previous;
            var pct = (double)diff / (double)Math.Abs(previous) * 100.0;
            return $"{(diff >= 0 ? "+" : "")}{diff:N0} ({pct:0.#}%)";
        }
        private string FormatVnCurrency(decimal amount)
            => string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:c0}", amount);

        // ====== STATS data providers (bạn map sang StatisticsService nếu đã có) ======
        private decimal GetMonthRevenue(DateTime from, DateTime to)
        {
            // Gợi ý: _statsSvc.GetRevenueTotal(from, to)
            // Tạm thời trả 0 để không lỗi nếu chưa có service
            return 0m;
        }
        private int GetMonthOrderCount(DateTime from, DateTime to, OrderStatus status)
        {
            // Gợi ý: _statsSvc.CountOrders(from, to, status)
            return 0;
        }
        private int GetMonthActiveShipments(DateTime from, DateTime to)
        {
            // Gợi ý: Count Shipment status in (Assigned, OnRoute, AtWarehouse) trong khoảng tháng
            return 0;
        }
        private int GetTotalCustomers(DateTime to)
        {
            // Gợi ý: _statsSvc.CountCustomers(to)
            return 0;
        }
        private int GetMonthTotalShipments(DateTime from, DateTime to)
        {
            // Gợi ý: _statsSvc.CountShipments(from, to)
            return 0;
        }

        // =======================
        // 1️⃣ Line Chart – Doanh thu 14 ngày
        // =======================
        private void BindRevenue14Days()
        {
            // Clear series cũ
            chartRevenue.Series.Clear();

            // Giả lập dữ liệu 14 ngày gần nhất
            var rnd = new Random();
            var values = new LiveCharts.ChartValues<decimal>();
            var labels = new List<string>();
            for (int i = 13; i >= 0; i--)
            {
                var day = DateTime.Today.AddDays(-i);
                values.Add(rnd.Next(10, 100)); // giá trị ngẫu nhiên
                labels.Add(day.ToString("dd/MM"));
            }

            var series = new LiveCharts.Wpf.LineSeries
            {
                Title = "Doanh thu (triệu đồng)",
                Values = new LiveCharts.ChartValues<decimal>(values),
                PointGeometrySize = 10,
                StrokeThickness = 2,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = System.Windows.Media.Brushes.DodgerBlue,
                DataLabels = false
            };

            chartRevenue.Series.Add(series);
            chartRevenue.AxisX.Clear();
            chartRevenue.AxisY.Clear();

            chartRevenue.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Ngày",
                Labels = labels,
                Separator = new LiveCharts.Wpf.Separator { Step = 1, IsEnabled = false }
            });

            chartRevenue.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Doanh thu (triệu)",
                LabelFormatter = value => $"{value:N0}"
            });
        }

        // =======================
        // 2️⃣ Pie Chart – Trạng thái đơn hàng
        // =======================
        private void BindOrderStatusPie()
        {
            chartOrderStatus.Series.Clear();

            // Các series PieSeries trực tiếp (không cần SeriesCollection)
            chartOrderStatus.Series = new LiveCharts.SeriesCollection
            {
                new LiveCharts.Wpf.PieSeries
                {
                    Title = "Chờ duyệt",
                    Values = new LiveCharts.ChartValues<int> { 10 },
                    Fill = System.Windows.Media.Brushes.Gold
                },
                new LiveCharts.Wpf.PieSeries
                {
                    Title = "Đã duyệt",
                    Values = new LiveCharts.ChartValues<int> { 25 },
                    Fill = System.Windows.Media.Brushes.DeepSkyBlue
                },
                new LiveCharts.Wpf.PieSeries
                {
                    Title = "Hoàn thành",
                    Values = new LiveCharts.ChartValues<int> { 40 },
                    Fill = System.Windows.Media.Brushes.MediumSeaGreen
                },
                new LiveCharts.Wpf.PieSeries
                {
                    Title = "Đã hủy",
                    Values = new LiveCharts.ChartValues<int> { 5 },
                    Fill = System.Windows.Media.Brushes.IndianRed
                }
            };
        }



        // =========================================================
        //  GRIDS
        // =========================================================
        private void ConfigureGrids()
        {
            // ---- Pending Orders ----
            dgvPendingOrders.ApplyBaseStyle();
            dgvPendingOrders.AutoGenerateColumns = false;
            dgvPendingOrders.Columns.Clear();
            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PendingOrderRow.OrderNo), HeaderText = "Mã đơn", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PendingOrderRow.CustomerName), HeaderText = "Khách hàng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PendingOrderRow.CreatedAt), HeaderText = "Ngày tạo", DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PendingOrderRow.Amount), HeaderText = "Giá trị", DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PendingOrderRow.StatusText), HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

            // ---- Active Shipments ----
            dgvActiveShipments.ApplyBaseStyle();
            dgvActiveShipments.AutoGenerateColumns = false;
            dgvActiveShipments.Columns.Clear();
            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ShipmentListItemAdminDto.ShipmentNo), HeaderText = "Mã CH", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ShipmentListItemAdminDto.Route), HeaderText = "Tuyến đường", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ShipmentListItemAdminDto.DriverName), HeaderText = "Tài xế", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ShipmentListItemAdminDto.UpdatedAt), HeaderText = "Cập nhật", DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter }, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ShipmentListItemAdminDto.Status), HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
        }

        private void WireGridEvents()
        {
            dgvPendingOrders.CellDoubleClick += DgvPendingOrders_CellDoubleClick;
            dgvActiveShipments.CellDoubleClick += DgvActiveShipments_CellDoubleClick;
            dgvActiveShipments.CellFormatting += DgvActiveShipments_CellFormatting;
        }

        // ---- Load data cho Pending Orders ----
        private void LoadPendingOrders()
        {
            try
            {
                // Nếu bạn có OrderService_Admin: gọi service thật ở đây
                // var data = _orderSvc.GetPendingOrdersLite(); // trả về List<PendingOrderRow>

                // Tạm thời để danh sách rỗng (không lỗi, UI chạy):
                var data = new List<PendingOrderRow>();
                dgvPendingOrders.DataSource = new BindingList<PendingOrderRow>(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải 'Đơn hàng cần duyệt': " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvPendingOrders.DataSource = null;
            }
        }

        // ---- Load data cho Active Shipments ----
        private void LoadActiveShipments()
        {
            try
            {
                var all = _shipmentSvc.GetAllShipmentsForAdmin();
                // Lọc các trạng thái được xem là đang chạy
                var active = all.Where(s =>
                        s.Status == ShipmentStatus.Assigned ||
                        s.Status == ShipmentStatus.OnRoute ||
                        s.Status == ShipmentStatus.AtWarehouse)
                    .OrderByDescending(x => x.UpdatedAt ?? x.StartedAt ?? DateTime.MinValue)
                    .ToList();

                dgvActiveShipments.DataSource = new BindingList<ShipmentListItemAdminDto>(active);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải 'Chuyến đang chạy': " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvActiveShipments.DataSource = null;
            }
        }

        // =========================================================
        //  INTERACTION
        // =========================================================

        // Double-click Pending Orders -> chuyển ucOrder_Admin & chọn đúng đơn
        private void DgvPendingOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvPendingOrders.Rows[e.RowIndex].DataBoundItem as PendingOrderRow;
            if (row == null) return;

            var frm = this.FindForm() as LMS.GUI.Main.frmMain_Admin;
            if (frm == null) return;

            // Hàm bên frmMain_Admin: Load UC Order_Admin và chọn đơn
            frm.NavigateToOrderAdmin(row.OrderNo);
        }

        // Double-click Active Shipments -> popup xem thông tin
        private void DgvActiveShipments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dto = dgvActiveShipments.Rows[e.RowIndex].DataBoundItem as ShipmentListItemAdminDto;
            if (dto == null) return;

            try
            {
                var detail = _shipmentSvc.GetShipmentDetailForAdmin(dto.Id);

                using (var f = new Form
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MinimizeBox = false,
                    MaximizeBox = false,
                    Text = $"Thông tin chuyến {detail?.Header?.ShipmentNo}",
                    Width = 720,
                    Height = 520
                })
                {
                    var txt = new TextBox
                    {
                        Multiline = true,
                        ReadOnly = true,
                        Dock = DockStyle.Fill,
                        ScrollBars = ScrollBars.Vertical
                    };

                    txt.Text =
                        $"Mã CH: {detail.Header.ShipmentNo}\r\n" +
                        $"Mã đơn: {detail.Header.OrderNo}\r\n" +
                        $"Khách hàng: {detail.Header.CustomerName}\r\n" +
                        $"Tuyến: {detail.Header.Route}\r\n" +
                        $"Trạng thái: {detail.Header.Status}\r\n" +
                        $"Bắt đầu: {detail.Header.StartedAt:dd/MM/yyyy HH:mm}\r\n" +
                        $"Kết thúc: {detail.Header.DeliveredAt:dd/MM/yyyy HH:mm}\r\n" +
                        $"Tài xế: {detail.DriverName}\r\n" +
                        $"Xe: {detail.VehicleNo}\r\n" +
                        $"Ghi chú: {detail.Notes}\r\n" +
                        $"\r\n--- Các chặng ---\r\n" +
                        string.Join("\r\n", detail.Stops.Select(s =>
                            $"#{s.Seq} - {s.StopName} | ETA: {ToText(s.PlannedETA)} | Arrive: {ToText(s.ArrivedAt)} | Depart: {ToText(s.DepartedAt)} | {s.StopStatus}"));

                    f.Controls.Add(txt);
                    f.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được thông tin chuyến.\r\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ToText(DateTime? dt) => dt.HasValue ? dt.Value.ToString("dd/MM/yyyy HH:mm") : "—";

        // Format cột trạng thái cho Active Shipments
        private void DgvActiveShipments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvActiveShipments.Columns[e.ColumnIndex].DataPropertyName == nameof(ShipmentListItemAdminDto.Status)
                && e.Value is ShipmentStatus st)
            {
                e.Value = StatusMapper.ToShipmentVN(st); // nếu bạn có StatusMapper; nếu không, comment dòng này
                e.FormattingApplied = true;
            }
        }

        // =========================================================
        //  ViewModels cho lưới Pending Orders
        // =========================================================
        private class PendingOrderRow
        {
            public string OrderNo { get; set; }
            public string CustomerName { get; set; }
            public DateTime? CreatedAt { get; set; }
            public string Amount { get; set; }     // hiển thị tiền đã format để khỏi lệ thuộc property DAL
            public string StatusText { get; set; }
        }
    }
}
