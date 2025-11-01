using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

// LiveCharts v1
using LiveCharts;
using LiveCharts.Wpf;

// Alias tránh trùng Brush/Control
using WpfBrushes = System.Windows.Media.Brushes;
using WfCartesianChart = LiveCharts.WinForms.CartesianChart;

// App
using LMS.BUS.Services;
using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
using static LMS.BUS.Dtos.ChartDataPoint;

// DAL cho dữ liệu chi tiết RDLC
using LMS.DAL;
using System.Data.Entity;

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
                        //Status = o.StatusText,
                        Status = StatusMapper.ToVietnamese(o.StatusText),
                        TotalFee = o.TotalFee,
                        CreatedAt = o.CreatedAt
                    });
                }
            }

            return list;
        }

        //// -------------------- Nút xem/Export Overview --------------------
        //private void btnExportOverview_Click(object sender, EventArgs e)
        //{
        //    if (tabControlMain.SelectedTab != tabOverview)
        //    {
        //        MessageBox.Show("Chức năng này chỉ dành cho tab 'Tổng Quan'.", "Thông báo",
        //            MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }

        //    try
        //    {
        //        DateTime from = dtpFrom.Value.Date;
        //        DateTime to = dtpTo.Value.Date;

        //        var summary = _statsSvc.GetOrderStatusCounts(from, to);
        //        if (summary == null || summary.Count == 0 || summary.All(d => d.Value == 0))
        //        {
        //            MessageBox.Show("Không có dữ liệu để lập báo cáo.", "Thông báo",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return;
        //        }

        //        // dữ liệu chi tiết cho RDLC (bảng)
        //        var details = GetOrderDetails(from, to);

        //        var viewer = new ucReportViewer { Dock = DockStyle.Fill };
        //        string dateRange = $"Từ {from:dd/MM/yyyy} đến {to:dd/MM/yyyy}";
        //        viewer.LoadOrderStatusReport(summary, details, dateRange);

        //        var frm = new Form
        //        {
        //            //Text = "Xem Trước Báo Cáo - Tình Trạng Đơn Hàng",
        //            StartPosition = FormStartPosition.CenterParent,
        //            MinimizeBox = false,
        //            MaximizeBox = true,
        //            FormBorderStyle = FormBorderStyle.Sizable,
        //            BackColor = Color.White,
        //            Size = new Size(1100, 800)
        //        };
        //        frm.Controls.Add(viewer);
        //        frm.ShowDialog(this.FindForm());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Đã xảy ra lỗi khi tạo báo cáo: " + ex.Message,
        //            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        // -------------------- Nút xem/Export Overview --------------------
        private void btnExportOverview_Click(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedTab != tabOverview)
            {
                MessageBox.Show("Chức năng này chỉ dành cho tab 'Tổng Quan'.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date;

                //var summary = _statsSvc.GetOrderStatusCounts(from, to);
                var summary = _statsSvc.GetOrderStatusCounts(from, to)
                    .Select(x => new ChartDataPoint
                    {
                        Label = StatusMapper.ToVietnamese(x.Label), // dùng helper đã tách
                        Value = x.Value
                    })
                    .ToList();

                if (summary == null || summary.Count == 0 || summary.All(d => d.Value == 0))
                {
                    MessageBox.Show("Không có dữ liệu để lập báo cáo.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // dữ liệu chi tiết cho RDLC (bảng)
                var details = GetOrderDetails(from, to);

                var viewer = new ucReportViewer { Dock = DockStyle.Fill };
                string dateRange = $"Từ {from:dd/MM/yyyy} đến {to:dd/MM/yyyy}";
                viewer.ReportTitle = "Tình Trạng Đơn Hàng"; // <== hiển thị lên lblReportTitle
                viewer.LoadOrderStatusReport(summary, details, dateRange);

                // === Tạo form borderless, center, size 1300x820 ===
                var frm = new Form
                {
                    StartPosition = FormStartPosition.CenterScreen,   // center
                    FormBorderStyle = FormBorderStyle.None,           // tắt border
                    MinimizeBox = false,                              // tắt
                    MaximizeBox = false,                              // tắt
                    BackColor = Color.White,
                    Size = new Size(1300, 820)                        // kích thước yêu cầu
                };

                // đặt UC ở giữa form nếu cần (nhưng đã DockFill)
                viewer.Dock = DockStyle.Fill;
                frm.Controls.Add(viewer);

                // show modal
                frm.ShowDialog(this.FindForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tạo báo cáo: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
