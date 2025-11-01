// LiveCharts v1
using LiveCharts;
using LiveCharts.Wpf;
using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
// App
using LMS.BUS.Services;
// DAL cho dữ liệu chi tiết RDLC
using LMS.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static LMS.BUS.Dtos.ChartDataPoint;
using WfCartesianChart = LiveCharts.WinForms.CartesianChart;
// Alias cho các DTO chi tiết dùng trong RDLC
using ShipmentDetailDto = LMS.BUS.Dtos.ShipmentDetailDto1;
using CustomerOrderDetailDto = LMS.BUS.Dtos.CustomerOrderDetailDto;
using DriverShipmentDetailDto = LMS.BUS.Dtos.DriverShipmentDetailDto;
using TimeSeriesDataPoint = LMS.BUS.Dtos.TimeSeriesDataPoint;
using TopCustomerDto = LMS.BUS.Dtos.ChartDataPoint.TopCustomerDto;
using TopDriverDto = LMS.BUS.Dtos.ChartDataPoint.TopDriverDto;

namespace LMS.GUI.ReportAdmin
{
    public partial class ucStatistics : UserControl
    {
        private readonly StatisticsService _statsSvc = new StatisticsService();
        private Control _activeFilterButton = null;

        private readonly Color _activeFilterColor = Color.FromArgb(32, 33, 36);
        private readonly Color _inactiveFilterColor = Color.Black;

        public ucStatistics()
        {
            InitializeComponent();
            this.AutoScroll = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            InitializeAllCharts();
            WireFilterEvents();
            tabControlMain.SelectedIndexChanged += tabControlMain_SelectedIndexChanged;

            // mặc định: Hôm nay
            if (btnToday != null) btnToday.PerformClick();
            else LoadAllData(DateTime.Today, DateTime.Today);
        }

        // -------------------- Init --------------------
        private void InitializeAllCharts()
        {
            // Tab 1: Tổng quan (Pie)
            if (pieOrderStatus != null)
            {
                pieOrderStatus.LegendLocation = LegendLocation.Right;
                pieOrderStatus.BackColor = Color.White;
                pieOrderStatus.HoverPushOut = 8;
                pieOrderStatus.InnerRadius = 60;
            }

            // Tab 2: Doanh thu (Line)
            InitCartesian(chartRevenueOverTime);

            // Tab 3: Vận hành (Pie + Bar)
            if (pieShipmentStatus != null)
            {
                pieShipmentStatus.LegendLocation = LegendLocation.Right;
                pieShipmentStatus.BackColor = Color.White;
                pieShipmentStatus.HoverPushOut = 8;
                pieShipmentStatus.InnerRadius = 60;
            }
            InitCartesian(chartTopRoutes);

            // Tab 4/5: Bar + Grid
            InitCartesian(chartTopCustomers);
            InitCartesian(chartTopDrivers);

            ConfigureGrids();
        }

        private void ConfigureGrids()
        {
            // Grid Top Customers
            if (dgvCustomerDetails != null)
            {
                dgvCustomerDetails.Columns.Clear();
                dgvCustomerDetails.ApplyBaseStyle();
                dgvCustomerDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgvCustomerDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "CustomerName", DataPropertyName = "CustomerName", HeaderText = "Tên Khách Hàng", FillWeight = 30 });
                dgvCustomerDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", DataPropertyName = "Phone", HeaderText = "SĐT", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                dgvCustomerDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", DataPropertyName = "Email", HeaderText = "Email", FillWeight = 30 });
                dgvCustomerDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalOrders", DataPropertyName = "TotalOrders", HeaderText = "Tổng Đơn", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
                dgvCustomerDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalRevenue", DataPropertyName = "TotalRevenue", HeaderText = "Tổng Doanh Thu", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
            }

            // Grid Top Drivers
            if (dgvDriverDetails != null)
            {
                dgvDriverDetails.Columns.Clear();
                dgvDriverDetails.ApplyBaseStyle();
                dgvDriverDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgvDriverDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "DriverName", DataPropertyName = "DriverName", HeaderText = "Tên Tài Xế", FillWeight = 30 });
                dgvDriverDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", DataPropertyName = "Phone", HeaderText = "SĐT", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                dgvDriverDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "LicenseType", DataPropertyName = "LicenseType", HeaderText = "GPLX", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                dgvDriverDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "VehiclePlate", DataPropertyName = "VehiclePlate", HeaderText = "Biển Số Xe", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                dgvDriverDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalShipments", DataPropertyName = "TotalShipments", HeaderText = "Tổng Chuyến", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            }
        }

        private void InitCartesian(WfCartesianChart cart)
        {
            if (cart == null) return;
            cart.Series = new SeriesCollection();
            cart.AxisX.Clear();
            cart.AxisY.Clear();
            cart.AxisX.Add(new Axis { Title = string.Empty, LabelsRotation = 0 });
            cart.AxisY.Add(new Axis { Title = string.Empty, LabelFormatter = v => v.ToString("N0") });
            cart.DisableAnimations = false;
        }

        private void WireFilterEvents()
        {
            btnToday.Click += FilterButton_Click;
            btnWeek.Click += FilterButton_Click;
            btnMonth.Click += FilterButton_Click;
            btnQuarter.Click += FilterButton_Click;
            btnYear.Click += FilterButton_Click;
            btnCustom.Click += FilterButton_Click;

            dtpFrom.ValueChanged += DatePicker_ValueChanged;
            dtpTo.ValueChanged += DatePicker_ValueChanged;

            // chỉ đăng ký 1 chỗ để tránh click mở 2 form
            btnExportOverview.Click += btnExportOverview_Click;
        }

        // -------------------- Filters --------------------
        private void FilterButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn == null) return;

            bool isCustom = (btn == btnCustom);
            dtpFrom.Enabled = isCustom;
            dtpTo.Enabled = isCustom;

            SetActiveFilterButton(btn);

            DateTime today = DateTime.Today;
            DateTime from, to = today;

            if (isCustom)
            {
                from = dtpFrom.Value.Date;
                to = dtpTo.Value.Date;
            }
            else if (btn == btnToday)
            {
                from = today;
            }
            else if (btn == btnWeek)
            {
                int diff = (7 + (int)today.DayOfWeek - (int)DayOfWeek.Monday) % 7;
                from = today.AddDays(-diff).Date;
            }
            else if (btn == btnMonth)
            {
                from = new DateTime(today.Year, today.Month, 1);
            }
            else if (btn == btnQuarter)
            {
                int q = ((today.Month - 1) / 3) + 1;
                int startMonth = (q - 1) * 3 + 1;
                from = new DateTime(today.Year, startMonth, 1);
            }
            else // Year
            {
                from = new DateTime(today.Year, 1, 1);
            }

            dtpFrom.ValueChanged -= DatePicker_ValueChanged;
            dtpTo.ValueChanged -= DatePicker_ValueChanged;
            dtpFrom.Value = from;
            dtpTo.Value = to;
            dtpFrom.ValueChanged += DatePicker_ValueChanged;
            dtpTo.ValueChanged += DatePicker_ValueChanged;

            LoadAllData(from, to);
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (_activeFilterButton == btnCustom)
                LoadAllData(dtpFrom.Value, dtpTo.Value);
        }

        private void SetActiveFilterButton(Control active)
        {
            if (_activeFilterButton is Guna.UI2.WinForms.Guna2Button oldBtn)
            {
                oldBtn.FillColor = _inactiveFilterColor;
                oldBtn.ForeColor = Color.White;
            }
            if (active is Guna.UI2.WinForms.Guna2Button newBtn)
            {
                newBtn.FillColor = _activeFilterColor;
                newBtn.ForeColor = Color.White;
                _activeFilterButton = newBtn;
            }
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAllData(dtpFrom.Value, dtpTo.Value);
        }

        // -------------------- Load tổng --------------------
        private void LoadAllData(DateTime from, DateTime to)
        {
            try
            {
                LoadKpis(from, to);

                if (tabControlMain.SelectedTab == tabOverview)
                {
                    LoadOrderStatusPie(from, to);
                }
                else if (tabControlMain.SelectedTab == tabRevenue)
                {
                    LoadRevenueChart(from, to);
                }
                else if (tabControlMain.SelectedTab == tabOperations)
                {
                    LoadShipmentStatusPie(from, to);
                    LoadTopRoutesBar(from, to);
                }
                else if (tabControlMain.SelectedTab == tabCustomers)
                {
                    LoadTopCustomersBar(from, to);
                }
                else if (tabControlMain.SelectedTab == tabDrivers)
                {
                    LoadTopDriversBar(from, to);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thống kê: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -------------------- KPI --------------------
        private void LoadKpis(DateTime from, DateTime to)
        {
            var kpi = _statsSvc.GetKpis(from, to);
            lblTotalOrdersValue.Text = kpi.TotalOrders.ToString("N0");
            lblInProgressValue.Text = kpi.ShipmentsInProgress.ToString("N0");
            lblCompletedValue.Text = kpi.ShipmentsCompleted.ToString("N0");
            lblRevenueValue.Text = kpi.TotalRevenue.ToString("N0") + " đ";
        }

        // -------------------- Pie: Orders --------------------
        private void LoadOrderStatusPie(DateTime from, DateTime to)
        {
            var data = _statsSvc.GetOrderStatusCounts(from, to);

            var series = new SeriesCollection();
            if (data == null || data.Count == 0 || data.All(d => d.Value == 0))
            {
                series.Add(new PieSeries { Title = "Không có dữ liệu", Values = new ChartValues<int> { 1 }, DataLabels = false, Fill = WpfBrushes.LightGray });
            }
            else
            {
                foreach (var item in data.Where(d => d.Value > 0))
                {
                    series.Add(new PieSeries
                    {
                        Title = item.Label,
                        Values = new ChartValues<double> { Convert.ToDouble(item.Value) },
                        DataLabels = true,
                        LabelPoint = cp => $"{cp.Participation:P0}",
                        Foreground = WpfBrushes.White,
                        Stroke = WpfBrushes.White,
                        StrokeThickness = 2
                    });
                }
            }

            pieOrderStatus.Series = series;
            pieOrderStatus.LegendLocation = LegendLocation.Right;
        }

        // -------------------- Pie: Shipments --------------------
        private void LoadShipmentStatusPie(DateTime from, DateTime to)
        {
            var data = _statsSvc.GetShipmentStatusCounts(from, to);

            var series = new SeriesCollection();
            if (data == null || data.Count == 0 || data.All(d => d.Value == 0))
            {
                series.Add(new PieSeries { Title = "Không có dữ liệu", Values = new ChartValues<int> { 1 }, DataLabels = false, Fill = WpfBrushes.LightGray });
            }
            else
            {
                foreach (var item in data.Where(d => d.Value > 0))
                {
                    series.Add(new PieSeries
                    {
                        Title = item.Label,
                        Values = new ChartValues<double> { Convert.ToDouble(item.Value) },
                        DataLabels = true,
                        LabelPoint = cp => $"{cp.Participation:P0}",
                        Foreground = WpfBrushes.White,
                        Stroke = WpfBrushes.White,
                        StrokeThickness = 2
                    });
                }
            }

            pieShipmentStatus.Series = series;
            pieShipmentStatus.LegendLocation = LegendLocation.Right;
        }

        // -------------------- Line: Revenue --------------------
        private void LoadRevenueChart(DateTime from, DateTime to)
        {
            var rows = _statsSvc.GetRevenueOverTime(from, to);

            if (rows == null || rows.Count == 0)
            {
                chartRevenueOverTime.Series.Clear();
                chartRevenueOverTime.AxisX.Clear();
                chartRevenueOverTime.AxisY.Clear();
                return;
            }

            chartRevenueOverTime.Series = new SeriesCollection {
                new LineSeries {
                    Title = "Doanh thu",
                    Values = new ChartValues<double>(rows.Select(r => Convert.ToDouble(r.Value))),
                    PointGeometry = DefaultGeometries.Circle,
                    StrokeThickness = 3
                }
            };

            chartRevenueOverTime.AxisX.Clear();
            chartRevenueOverTime.AxisX.Add(new Axis { Labels = rows.Select(r => r.Date.ToString("dd/MM")).ToArray() });

            chartRevenueOverTime.AxisY.Clear();
            chartRevenueOverTime.AxisY.Add(new Axis { LabelFormatter = v => v.ToString("N0") });
        }

        // -------------------- Bar: Top Routes --------------------
        private void LoadTopRoutesBar(DateTime from, DateTime to)
        {
            var rows = _statsSvc.GetTopRoutes(from, to, 5);

            if (rows == null || rows.Count == 0)
            {
                chartTopRoutes.Series.Clear();
                chartTopRoutes.AxisX.Clear();
                chartTopRoutes.AxisY.Clear();
                return;
            }

            chartTopRoutes.Series = new SeriesCollection {
                new RowSeries {
                    Title = "Số chuyến",
                    Values = new ChartValues<double>(rows.Select(x => Convert.ToDouble(x.Value)))
                }
            };

            chartTopRoutes.AxisY.Clear();
            chartTopRoutes.AxisY.Add(new Axis { Labels = rows.Select(x => x.Label).ToArray() });
            chartTopRoutes.AxisX.Clear();
            chartTopRoutes.AxisX.Add(new Axis { LabelFormatter = v => v.ToString("N0") });
        }

        // -------------------- Bar + Grid: Top Customers --------------------
        private void LoadTopCustomersBar(DateTime from, DateTime to)
        {
            var rows = _statsSvc.GetTopCustomers(from, to, 5).ToList();

            if (rows.Count == 0)
            {
                chartTopCustomers.Series.Clear();
                chartTopCustomers.AxisX.Clear();
                chartTopCustomers.AxisY.Clear();
            }
            else
            {
                chartTopCustomers.Series = new SeriesCollection {
                    new RowSeries {
                        Title = "Doanh thu",
                        Values = new ChartValues<double>(rows.Select(x => Convert.ToDouble(x.TotalRevenue)))
                    }
                };

                chartTopCustomers.AxisY.Clear();
                chartTopCustomers.AxisY.Add(new Axis { Labels = rows.Select(x => x.CustomerName).ToArray() });
                chartTopCustomers.AxisX.Clear();
                chartTopCustomers.AxisX.Add(new Axis { LabelFormatter = v => v.ToString("N0") });
            }

            if (dgvCustomerDetails != null)
            {
                var gridData = _statsSvc.GetTopCustomers(from, to, 10);
                dgvCustomerDetails.DataSource = gridData;
            }
        }

        // -------------------- Bar + Grid: Top Drivers --------------------
        private void LoadTopDriversBar(DateTime from, DateTime to)
        {
            var rows = _statsSvc.GetTopDrivers(from, to, 5).ToList();

            if (rows.Count == 0)
            {
                chartTopDrivers.Series.Clear();
                chartTopDrivers.AxisX.Clear();
                chartTopDrivers.AxisY.Clear();
            }
            else
            {
                chartTopDrivers.Series = new SeriesCollection {
                    new RowSeries {
                        Title = "Số chuyến",
                        Values = new ChartValues<double>(rows.Select(x => Convert.ToDouble(x.TotalShipments)))
                    }
                };

                chartTopDrivers.AxisY.Clear();
                chartTopDrivers.AxisY.Add(new Axis { Labels = rows.Select(x => x.DriverName).ToArray() });
                chartTopDrivers.AxisX.Clear();
                chartTopDrivers.AxisX.Add(new Axis { LabelFormatter = v => v.ToString("N0") });
            }

            if (dgvDriverDetails != null)
            {
                var gridData = _statsSvc.GetTopDrivers(from, to, 10);
                dgvDriverDetails.DataSource = gridData;
            }
        }

        // -------------------- Helper: map trạng thái đơn hàng sang tiếng Việt --------------------
        private string MapOrderStatusVi(string statusEnum)
        {
            if (string.IsNullOrWhiteSpace(statusEnum)) return string.Empty;
            switch (statusEnum.Trim())
            {
                case "Pending": return "Chờ duyệt";
                case "Approved": return "Đã duyệt";
                case "Completed": return "Hoàn thành";
                case "Cancelled": return "Đã hủy";
                default: return statusEnum;
            }
        }

        // -------------------- Helper: chi tiết đơn hàng cho RDLC --------------------
        private System.Collections.Generic.List<ChartDataPoint.OrderStatusDetailDto> GetOrderDetails(DateTime from, DateTime to)
        {
            var list = new System.Collections.Generic.List<ChartDataPoint.OrderStatusDetailDto>();

            using (var db = new LogisticsDbContext())
            {
                var orders = db.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.CreatedAt >= from && o.CreatedAt <= to)
                    .OrderByDescending(o => o.CreatedAt)
                    .Select(o => new
                    {
                        o.OrderNo,
                        CustomerName = o.Customer != null ? o.Customer.Name : "",
                        StatusText = o.Status.ToString(),
                        o.TotalFee,
                        o.CreatedAt
                    })
                    .ToList();

                foreach (var o in orders)
                {
                    list.Add(new ChartDataPoint.OrderStatusDetailDto
                    {
                        OrderNo = o.OrderNo,
                        CustomerName = o.CustomerName,
                        Status = MapOrderStatusVi(o.StatusText),
                        TotalFee = o.TotalFee,
                        CreatedAt = o.CreatedAt
                    });
                }
            }

            return list;
        }

        // -------------------- Helper: chi tiết doanh thu cho RDLC --------------------
        private System.Collections.Generic.List<ChartDataPoint.RevenueDetailDto> GetRevenueDetails(DateTime from, DateTime to)
        {
            var list = new System.Collections.Generic.List<ChartDataPoint.RevenueDetailDto>();
            using (var db = new LogisticsDbContext())
            {
                var orders = db.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.CreatedAt >= from && o.CreatedAt <= to)
                    .OrderByDescending(o => o.CreatedAt)
                    .Select(o => new
                    {
                        o.OrderNo,
                        CustomerName = o.Customer != null ? o.Customer.Name : "",
                        o.TotalFee,
                        o.CreatedAt
                    })
                    .ToList();

                foreach (var o in orders)
                {
                    list.Add(new ChartDataPoint.RevenueDetailDto
                    {
                        OrderNo = o.OrderNo,
                        CustomerName = o.CustomerName,
                        TotalFee = o.TotalFee,
                        DeliveredAt = o.CreatedAt // nếu bạn muốn ngày giao, đổi sang DeliveredAt từ Shipment
                    });
                }
            }
            return list;
        }

        // -------------------- Nút xuất báo cáo dùng chung --------------------
        private void btnExportOverview_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date;

                if (tabControlMain.SelectedTab == tabOverview)
                {
                    HandleExportOrderStatus(from, to);
                }
                else if (tabControlMain.SelectedTab == tabRevenue)
                {
                    HandleExportRevenue(from, to);
                }
                else if (tabControlMain.SelectedTab == tabOperations)
                {
                    HandleExportOperations(from, to);
                }
                else if (tabControlMain.SelectedTab == tabCustomers)
                {
                    HandleExportCustomers(from, to);
                }
                else if (tabControlMain.SelectedTab == tabDrivers)
                {
                    HandleExportDrivers(from, to);
                }
                else
                {
                    MessageBox.Show("Tab này chưa hỗ trợ xuất báo cáo.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tạo báo cáo: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Xử lý: xuất báo cáo Tình trạng đơn hàng ---
        private void HandleExportOrderStatus(DateTime from, DateTime to)
        {
            var summary = _statsSvc.GetOrderStatusCounts(from, to)
                .Select(x => new ChartDataPoint { Label = x.Label, Value = x.Value })
                .ToList();

            if (summary == null || summary.Count == 0 || summary.All(d => d.Value == 0))
            {
                MessageBox.Show("Không có dữ liệu để lập báo cáo.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var details = GetOrderDetails(from, to);

            var viewer = new ucReportViewer { Dock = DockStyle.Fill };
            string dateRange = $"Từ {from:dd/MM/yyyy} đến {to:dd/MM/yyyy}";
            viewer.ReportTitle = "Tình Trạng Đơn Hàng";
            viewer.LoadOrderStatusReport(summary, details, dateRange);

            var frm = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                MinimizeBox = false,
                MaximizeBox = false,
                BackColor = Color.White,
                Size = new Size(1300, 820)
            };
            frm.Controls.Add(viewer);
            frm.ShowDialog(this.FindForm());
        }

        // --- Xử lý: xuất báo cáo Doanh thu ---
        private void HandleExportRevenue(DateTime from, DateTime to)
        {
            var summaryData = _statsSvc.GetRevenueOverTime(from, to);
            var detailData = GetRevenueDetails(from, to);

            if ((summaryData == null || summaryData.Count == 0) && (detailData == null || detailData.Count == 0))
            {
                MessageBox.Show("Không có dữ liệu (Doanh thu) để lập báo cáo.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var viewer = new ucReportViewer { Dock = DockStyle.Fill };
            string dateRange = $"Từ {from:dd/MM/yyyy} đến {to:dd/MM/yyyy}";
            viewer.ReportTitle = "Báo Cáo Doanh Thu";
            viewer.LoadRevenueReport(summaryData, detailData, dateRange);

            var frm = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                MinimizeBox = false,
                MaximizeBox = false,
                BackColor = Color.White,
                Size = new Size(1300, 820)
            };
            frm.Controls.Add(viewer);
            frm.ShowDialog(this.FindForm());
        }

        // THÊM MỚI trong ucStatistics.cs
        // --- Chi tiết vận hành cho RDLC ---
        private List<ShipmentDetailDto> GetOperationDetails(DateTime from, DateTime to)
        {
            using (var db = new LogisticsDbContext())
            {
                var q = db.Shipments
                    .Include(s => s.Driver)
                    .Include(s => s.Vehicle)
                    .Where(s =>
                        (s.StartedAt.HasValue && s.StartedAt >= from && s.StartedAt <= to) ||
                        (s.DeliveredAt.HasValue && s.DeliveredAt >= from && s.DeliveredAt <= to))
                    .OrderByDescending(s => s.DeliveredAt ?? s.StartedAt)
                    .Select(s => new
                    {
                        // Nếu Shipment có trường ShipmentNo thì dùng, 
                        // còn nếu không thì sinh tạm bằng Id
                        ShipmentNo = s.Id.ToString(),

                        DriverName = s.Driver != null ? s.Driver.FullName : "",
                        VehiclePlate = s.Vehicle != null ? s.Vehicle.PlateNo : "",
                        StatusText = s.Status.ToString(),
                        StartedAt = s.StartedAt,
                        CompletedAt = s.DeliveredAt
                    })
                    .ToList();

                var list = new List<ShipmentDetailDto>();
                foreach (var x in q)
                {
                    list.Add(new ShipmentDetailDto
                    {
                        ShipmentNo = x.ShipmentNo,
                        DriverName = x.DriverName,
                        VehiclePlate = x.VehiclePlate,
                        Status = MapOrderStatusVi(x.StatusText),
                        // Dùng GetValueOrDefault để tránh lỗi nullable
                        StartedAt = x.StartedAt.GetValueOrDefault(),
                        CompletedAt = x.CompletedAt.GetValueOrDefault()
                    });
                }
                return list;
            }
        }



        // THÊM MỚI
        private List<CustomerOrderDetailDto> GetCustomerOrderDetails(DateTime from, DateTime to)
        {
            using (var db = new LogisticsDbContext())
            {
                var q = db.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.CreatedAt >= from && o.CreatedAt <= to)
                    .OrderByDescending(o => o.CreatedAt)
                    .Select(o => new
                    {
                        o.OrderNo,
                        CustomerName = o.Customer != null ? o.Customer.Name : "",
                        StatusText = o.Status.ToString(),
                        o.TotalFee,
                        o.CreatedAt
                    })
                    .ToList();

                var list = new List<CustomerOrderDetailDto>();
                foreach (var o in q)
                {
                    list.Add(new CustomerOrderDetailDto
                    {
                        OrderNo = o.OrderNo,
                        CustomerName = o.CustomerName,
                        Status = MapOrderStatusVi(o.StatusText),
                        TotalFee = o.TotalFee,
                        CreatedAt = o.CreatedAt
                    });
                }
                return list;
            }
        }

        // THÊM MỚI
        // Chi tiết theo tài xế – RDLC Drivers
        private List<DriverShipmentDetailDto> GetDriverShipmentDetails(DateTime from, DateTime to)
        {
            using (var db = new LogisticsDbContext())
            {
                var q = db.Shipments
                    .Include(s => s.Driver)
                    .Include(s => s.Vehicle)
                    .Where(s =>
                        (s.StartedAt.HasValue && s.StartedAt >= from && s.StartedAt <= to) ||
                        (s.DeliveredAt.HasValue && s.DeliveredAt >= from && s.DeliveredAt <= to))  // ✅ đổi DeliveredAt
                    .OrderByDescending(s => s.DeliveredAt ?? s.StartedAt)                         // ✅ đổi DeliveredAt
                    .Select(s => new
                    {
                        s.Id,
                        DriverName = s.Driver != null ? s.Driver.FullName : "",
                        VehiclePlate = s.Vehicle != null ? s.Vehicle.PlateNo : "",
                        StatusText = s.Status.ToString(),
                        CompletedAt = s.DeliveredAt,                                             // ✅ alias sang CompletedAt
                        Fee = s.Order != null ? s.Order.TotalFee : (decimal?)null        // nếu muốn hiển thị phí
                    })
                    .ToList();

                var list = new List<DriverShipmentDetailDto>();
                foreach (var s in q)
                {
                    list.Add(new DriverShipmentDetailDto
                    {
                        ShipmentNo = "SHP" + s.Id,  // ShipmentNo thực tế tạo từ Id
                        DriverName = s.DriverName,
                        VehiclePlate = s.VehiclePlate,
                        Status = MapOrderStatusVi(s.StatusText),
                        CompletedAt = s.CompletedAt,
                        //Fee = s.Fee
                    });
                }
                return list;
            }
        }

        private void HandleExportOperations(DateTime from, DateTime to)
        {
            var shipmentStatus = _statsSvc.GetShipmentStatusCounts(from, to);
            var topRoutes = _statsSvc.GetTopRoutes(from, to, 5);
            var opDetails = GetOperationDetails(from, to);

            if ((shipmentStatus == null || shipmentStatus.Count == 0) &&
                (topRoutes == null || topRoutes.Count == 0) &&
                (opDetails == null || opDetails.Count == 0))
            {
                MessageBox.Show("Không có dữ liệu (Vận hành) để lập báo cáo.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var viewer = new ucReportViewer { Dock = DockStyle.Fill };
            string dateRange = $"Từ {from:dd/MM/yyyy} đến {to:dd/MM/yyyy}";
            viewer.ReportTitle = "Báo Cáo Vận Hành";
            viewer.LoadOperationsReport(shipmentStatus, topRoutes, opDetails, dateRange);

            ShowViewerInForm(viewer);
        }

        private void HandleExportCustomers(DateTime from, DateTime to)
        {
            var rows = _statsSvc.GetTopCustomers(from, to, 50).Cast<TopCustomerDto>().ToList();
            var details = GetCustomerOrderDetails(from, to);

            if ((rows == null || rows.Count == 0) && (details == null || details.Count == 0))
            {
                MessageBox.Show("Không có dữ liệu (Khách hàng) để lập báo cáo.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var viewer = new ucReportViewer { Dock = DockStyle.Fill };
            string dateRange = $"Từ {from:dd/MM/yyyy} đến {to:dd/MM/yyyy}";
            viewer.ReportTitle = "Báo Cáo Khách Hàng";
            viewer.LoadCustomersReport(rows, details, dateRange);
            ShowViewerInForm(viewer);
        }

        private void HandleExportDrivers(DateTime from, DateTime to)
        {
            var rows = _statsSvc.GetTopDrivers(from, to, 50).Cast<TopDriverDto>().ToList();
            var details = GetDriverShipmentDetails(from, to); // tên hàm đúng là GetDriverShipmentDetails

            if ((rows == null || rows.Count == 0) && (details == null || details.Count == 0))
            {
                MessageBox.Show("Không có dữ liệu (Tài xế) để lập báo cáo.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var viewer = new ucReportViewer { Dock = DockStyle.Fill };
            string dateRange = $"Từ {from:dd/MM/yyyy} đến {to:dd/MM/yyyy}";
            viewer.ReportTitle = "Báo Cáo Tài Xế";
            viewer.LoadDriversReport(rows, details, dateRange);
            ShowViewerInForm(viewer);
        }

        // helper hiển thị form
        private void ShowViewerInForm(ucReportViewer viewer)
        {
            var frm = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                MinimizeBox = false,
                MaximizeBox = false,
                BackColor = Color.White,
                Size = new Size(1300, 820)
            };
            viewer.Dock = DockStyle.Fill;
            frm.Controls.Add(viewer);
            frm.ShowDialog(this.FindForm());
        }

    }
}
