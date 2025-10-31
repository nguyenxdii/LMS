//using System;
//using System.Linq;
//using System.Windows.Forms;
//using System.Drawing; // => dùng Color bình thường

//// LiveCharts v1
//using LiveCharts;
//using LiveCharts.Wpf;

//// Alias tránh trùng & nhầm Brush
//using WpfBrushes = System.Windows.Media.Brushes;
//using WfCartesianChart = LiveCharts.WinForms.CartesianChart;   // dùng cho chữ ký hàm init nếu cần

//// App
//using LMS.BUS.Services;
//using LMS.BUS.Dtos;
//using LMS.BUS.Helpers;  // nếu bạn có ApplyBaseStyle()

//namespace LMS.GUI.ReportAdmin
//{
//    public partial class ucStatistics : UserControl
//    {
//        private readonly StatisticsService _statsSvc = new StatisticsService();
//        private Control _activeFilterButton = null;

//        // Màu nút filter (WinForms Color)
//        private readonly Color _activeFilterColor = Color.FromArgb(32, 33, 36);
//        private readonly Color _inactiveFilterColor = Color.Black;

//        public ucStatistics()
//        {
//            InitializeComponent();
//            this.AutoScroll = true;
//        }

//        protected override void OnLoad(EventArgs e)
//        {
//            base.OnLoad(e);
//            if (DesignMode) return;

//            InitializeAllCharts();
//            WireFilterEvents();

//            tabControlMain.SelectedIndexChanged += tabControlMain_SelectedIndexChanged;

//            // mặc định: Hôm nay
//            if (btnToday != null) btnToday.PerformClick();
//            else LoadAllData(DateTime.Today, DateTime.Today);
//        }

//        // ====================== Init charts ======================
//        private void InitializeAllCharts()
//        {
//            // Tab 1: Tổng quan (Pie LiveCharts)
//            if (pieOrderStatus != null)
//            {
//                pieOrderStatus.LegendLocation = LegendLocation.Right;
//                pieOrderStatus.BackColor = Color.White; // WinForms color
//                pieOrderStatus.HoverPushOut = 8;
//                pieOrderStatus.InnerRadius = 60; // donut
//            }

//            // Tab 2: Doanh Thu (Cartesian LiveCharts)
//            InitCartesian(chartRevenueOverTime);

//            // Tab 3: Vận Hành
//            if (pieShipmentStatus != null)
//            {
//                pieShipmentStatus.LegendLocation = LegendLocation.Right;
//                pieShipmentStatus.BackColor = Color.White;
//                pieShipmentStatus.HoverPushOut = 8;
//                pieShipmentStatus.InnerRadius = 60;
//            }
//            InitCartesian(chartTopRoutes);

//            // Tab 4: Khách hàng
//            InitCartesian(chartTopCustomers);
//            dgvCustomerDetails?.ApplyBaseStyle();

//            // Tab 5: Tài xế
//            InitCartesian(chartTopDrivers);
//            dgvDriverDetails?.ApplyBaseStyle();
//        }

//        // Init cơ bản cho CartesianChart (LiveCharts)
//        private void InitCartesian(WfCartesianChart cart)
//        {
//            if (cart == null) return;
//            cart.Series = new SeriesCollection();

//            cart.AxisX.Clear();
//            cart.AxisY.Clear();

//            cart.AxisX.Add(new Axis { Title = string.Empty, LabelsRotation = 0 });
//            cart.AxisY.Add(new Axis { Title = string.Empty, LabelFormatter = v => v.ToString("N0") });

//            cart.DisableAnimations = false;
//        }

//        // ====================== Wire filters ======================
//        private void WireFilterEvents()
//        {
//            btnToday.Click += FilterButton_Click;
//            btnWeek.Click += FilterButton_Click;
//            btnMonth.Click += FilterButton_Click;
//            btnQuarter.Click += FilterButton_Click;
//            btnYear.Click += FilterButton_Click;
//            btnCustom.Click += FilterButton_Click;

//            dtpFrom.ValueChanged += DatePicker_ValueChanged;
//            dtpTo.ValueChanged += DatePicker_ValueChanged;
//        }

//        private void FilterButton_Click(object sender, EventArgs e)
//        {
//            var btn = sender as Guna.UI2.WinForms.Guna2Button;
//            if (btn == null) return;

//            bool isCustom = (btn == btnCustom);
//            dtpFrom.Enabled = isCustom;
//            dtpTo.Enabled = isCustom;

//            SetActiveFilterButton(btn);

//            DateTime today = DateTime.Today;
//            DateTime from, to = today;

//            if (isCustom)
//            {
//                from = dtpFrom.Value.Date;
//                to = dtpTo.Value.Date;
//            }
//            else if (btn == btnToday)
//            {
//                from = today;
//            }
//            else if (btn == btnWeek)
//            {
//                int diff = (7 + (int)today.DayOfWeek - (int)DayOfWeek.Monday) % 7;
//                from = today.AddDays(-diff).Date;
//            }
//            else if (btn == btnMonth)
//            {
//                from = new DateTime(today.Year, today.Month, 1);
//            }
//            else if (btn == btnQuarter)
//            {
//                int q = ((today.Month - 1) / 3) + 1;
//                int startMonth = (q - 1) * 3 + 1;
//                from = new DateTime(today.Year, startMonth, 1);
//            }
//            else // Year
//            {
//                from = new DateTime(today.Year, 1, 1);
//            }

//            // đồng bộ lại datepicker (tránh vòng lặp sự kiện)
//            dtpFrom.ValueChanged -= DatePicker_ValueChanged;
//            dtpTo.ValueChanged -= DatePicker_ValueChanged;
//            dtpFrom.Value = from;
//            dtpTo.Value = to;
//            dtpFrom.ValueChanged += DatePicker_ValueChanged;
//            dtpTo.ValueChanged += DatePicker_ValueChanged;

//            LoadAllData(from, to);
//        }

//        private void DatePicker_ValueChanged(object sender, EventArgs e)
//        {
//            if (_activeFilterButton == btnCustom)
//                LoadAllData(dtpFrom.Value, dtpTo.Value);
//        }

//        private void SetActiveFilterButton(Control active)
//        {
//            if (_activeFilterButton is Guna.UI2.WinForms.Guna2Button oldBtn)
//            {
//                oldBtn.FillColor = _inactiveFilterColor;
//                oldBtn.ForeColor = Color.White;
//            }
//            if (active is Guna.UI2.WinForms.Guna2Button newBtn)
//            {
//                newBtn.FillColor = _activeFilterColor;
//                newBtn.ForeColor = Color.White;
//                _activeFilterButton = newBtn;
//            }
//        }

//        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            LoadAllData(dtpFrom.Value, dtpTo.Value);
//        }

//        // ====================== Load tổng ======================
//        private void LoadAllData(DateTime from, DateTime to)
//        {
//            try
//            {
//                LoadKpis(from, to);

//                if (tabControlMain.SelectedTab == tabOverview)
//                {
//                    LoadOrderStatusPie(from, to);
//                }
//                else if (tabControlMain.SelectedTab == tabRevenue)
//                {
//                    LoadRevenueChart(from, to);
//                }
//                else if (tabControlMain.SelectedTab == tabOperations)
//                {
//                    LoadShipmentStatusPie(from, to);
//                    LoadTopRoutesBar(from, to);
//                }
//                else if (tabControlMain.SelectedTab == tabCustomers)
//                {
//                    LoadTopCustomersBar(from, to);
//                }
//                else if (tabControlMain.SelectedTab == tabDrivers)
//                {
//                    LoadTopDriversBar(from, to);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi tải thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        // ====================== KPI ======================
//        private void LoadKpis(DateTime from, DateTime to)
//        {
//            var kpi = _statsSvc.GetKpis(from, to);
//            lblTotalOrdersValue.Text = kpi.TotalOrders.ToString("N0");
//            lblInProgressValue.Text = kpi.ShipmentsInProgress.ToString("N0");
//            lblCompletedValue.Text = kpi.ShipmentsCompleted.ToString("N0");
//            lblRevenueValue.Text = kpi.TotalRevenue.ToString("N0") + " đ";
//        }

//        // ====================== Pie: Đơn hàng ======================
//        private void LoadOrderStatusPie(DateTime from, DateTime to)
//        {
//            var data = _statsSvc.GetOrderStatusCounts(from, to);

//            var series = new SeriesCollection();
//            if (data.Count == 0)
//            {
//                series.Add(new PieSeries
//                {
//                    Title = "Không có dữ liệu",
//                    Values = new ChartValues<int> { 1 },
//                    DataLabels = false,
//                    Fill = WpfBrushes.LightGray
//                });
//            }
//            else
//            {
//                foreach (var item in data.Where(d => d.Value > 0))
//                {
//                    series.Add(new PieSeries
//                    {
//                        Title = item.Label,
//                        Values = new ChartValues<double> { Convert.ToDouble(item.Value) },
//                        DataLabels = true,
//                        LabelPoint = cp => $"{cp.Participation:P0}",
//                        Foreground = WpfBrushes.White,
//                        Stroke = WpfBrushes.White,
//                        StrokeThickness = 2
//                    });
//                }
//            }

//            pieOrderStatus.Series = series;
//            pieOrderStatus.LegendLocation = LegendLocation.Right;
//        }

//        // ====================== Pie: Shipment ======================
//        private void LoadShipmentStatusPie(DateTime from, DateTime to)
//        {
//            var data = _statsSvc.GetShipmentStatusCounts(from, to);

//            var series = new SeriesCollection();
//            if (data.Count == 0)
//            {
//                series.Add(new PieSeries
//                {
//                    Title = "Không có dữ liệu",
//                    Values = new ChartValues<int> { 1 },
//                    DataLabels = false,
//                    Fill = WpfBrushes.LightGray
//                });
//            }
//            else
//            {
//                foreach (var item in data.Where(d => d.Value > 0))
//                {
//                    series.Add(new PieSeries
//                    {
//                        Title = item.Label,
//                        Values = new ChartValues<double> { Convert.ToDouble(item.Value) },
//                        DataLabels = true,
//                        LabelPoint = cp => $"{cp.Participation:P0}",
//                        Foreground = WpfBrushes.White,
//                        Stroke = WpfBrushes.White,
//                        StrokeThickness = 2
//                    });
//                }
//            }

//            pieShipmentStatus.Series = series;
//            pieShipmentStatus.LegendLocation = LegendLocation.Right;
//        }

//        // ====================== Revenue: Line ======================
//        private void LoadRevenueChart(DateTime from, DateTime to)
//        {
//            var rows = _statsSvc.GetRevenueOverTime(from, to);

//            chartRevenueOverTime.Series = new SeriesCollection {
//                new LineSeries {
//                    Title = "Doanh thu",
//                    Values = new ChartValues<double>(rows.Select(r => Convert.ToDouble(r.Value))),
//                    PointGeometry = DefaultGeometries.Circle,
//                    StrokeThickness = 3
//                }
//            };

//            chartRevenueOverTime.AxisX.Clear();
//            chartRevenueOverTime.AxisX.Add(new Axis
//            {
//                Labels = rows.Select(r => r.Date.ToString("dd/MM")).ToArray()
//            });

//            chartRevenueOverTime.AxisY.Clear();
//            chartRevenueOverTime.AxisY.Add(new Axis
//            {
//                LabelFormatter = v => v.ToString("N0")
//            });
//        }

//        // ====================== Bar ngang: Top Routes ======================
//        private void LoadTopRoutesBar(DateTime from, DateTime to)
//        {
//            var rows = _statsSvc.GetTopRoutes(from, to, 5);

//            chartTopRoutes.Series = new SeriesCollection {
//                new RowSeries {
//                    Title = "Số chuyến",
//                    Values = new ChartValues<double>(rows.Select(x => Convert.ToDouble(x.Value)))
//                }
//            };

//            chartTopRoutes.AxisY.Clear();
//            chartTopRoutes.AxisY.Add(new Axis
//            {
//                Labels = rows.Select(x => x.Label).ToArray()
//            });
//            chartTopRoutes.AxisX.Clear();
//            chartTopRoutes.AxisX.Add(new Axis
//            {
//                LabelFormatter = v => v.ToString("N0")
//            });
//        }

//        // ====================== Bar ngang + Grid: Top Customers ======================
//        private void LoadTopCustomersBar(DateTime from, DateTime to)
//        {
//            var rows = _statsSvc.GetTopCustomers(from, to, 5).ToList();

//            chartTopCustomers.Series = new SeriesCollection {
//                new RowSeries {
//                    Title = "Doanh thu",
//                    Values = new ChartValues<double>(rows.Select(x => Convert.ToDouble(x.TotalRevenue)))
//                }
//            };

//            chartTopCustomers.AxisY.Clear();
//            chartTopCustomers.AxisY.Add(new Axis
//            {
//                Labels = rows.Select(x => x.CustomerName).ToArray()
//            });
//            chartTopCustomers.AxisX.Clear();
//            chartTopCustomers.AxisX.Add(new Axis
//            {
//                LabelFormatter = v => v.ToString("N0")
//            });

//            if (dgvCustomerDetails != null)
//            {
//                dgvCustomerDetails.DataSource = _statsSvc.GetTopCustomers(from, to, 10);
//                dgvCustomerDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//            }
//        }

//        // ====================== Bar ngang + Grid: Top Drivers ======================
//        private void LoadTopDriversBar(DateTime from, DateTime to)
//        {
//            var rows = _statsSvc.GetTopDrivers(from, to, 5).ToList();

//            chartTopDrivers.Series = new SeriesCollection {
//                new RowSeries {
//                    Title = "Số chuyến",
//                    Values = new ChartValues<double>(rows.Select(x => Convert.ToDouble(x.TotalShipments)))
//                }
//            };

//            chartTopDrivers.AxisY.Clear();
//            chartTopDrivers.AxisY.Add(new Axis
//            {
//                Labels = rows.Select(x => x.DriverName).ToArray()
//            });
//            chartTopDrivers.AxisX.Clear();
//            chartTopDrivers.AxisX.Add(new Axis
//            {
//                LabelFormatter = v => v.ToString("N0")
//            });

//            if (dgvDriverDetails != null)
//            {
//                dgvDriverDetails.DataSource = _statsSvc.GetTopDrivers(from, to, 10);
//                dgvDriverDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//            }
//        }
//    }
//}



using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing; // => dùng Color bình thường

// LiveCharts v1
using LiveCharts;
using LiveCharts.Wpf;

// Alias tránh trùng & nhầm Brush
using WpfBrushes = System.Windows.Media.Brushes;
using WfCartesianChart = LiveCharts.WinForms.CartesianChart;   // dùng cho chữ ký hàm init nếu cần

// App
using LMS.BUS.Services;
using LMS.BUS.Dtos;
using LMS.BUS.Helpers;  // Cần cho ApplyBaseStyle()
using System.Globalization; // Cần cho Format
using static LMS.BUS.Dtos.ChartDataPoint; // Cần để truy cập TopCustomerDto, TopDriverDto

namespace LMS.GUI.ReportAdmin
{
    public partial class ucStatistics : UserControl
    {
        private readonly StatisticsService _statsSvc = new StatisticsService();
        private Control _activeFilterButton = null;

        // Màu nút filter (WinForms Color)
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

        // ====================== Init charts ======================
        private void InitializeAllCharts()
        {
            // Tab 1: Tổng quan (Pie LiveCharts)
            if (pieOrderStatus != null)
            {
                pieOrderStatus.LegendLocation = LegendLocation.Right;
                pieOrderStatus.BackColor = Color.White; // WinForms color
                pieOrderStatus.HoverPushOut = 8;
                pieOrderStatus.InnerRadius = 60; // donut
            }

            // Tab 2: Doanh Thu (Cartesian LiveCharts)
            InitCartesian(chartRevenueOverTime);

            // Tab 3: Vận Hành
            if (pieShipmentStatus != null)
            {
                pieShipmentStatus.LegendLocation = LegendLocation.Right;
                pieShipmentStatus.BackColor = Color.White;
                pieShipmentStatus.HoverPushOut = 8;
                pieShipmentStatus.InnerRadius = 60;
            }
            InitCartesian(chartTopRoutes);

            // <--- FIX: GỌI HÀM CẤU HÌNH GRIDS TẠI ĐÂY ---
            ConfigureGrids();
            // <--- KẾT THÚC FIX ---
        }

        // <--- FIX: THÊM HÀM MỚI ĐỂ CẤU HÌNH DATAGRIDVIEWS ---
        /// <summary>
        /// Định nghĩa các cột cho DataGridViews.
        /// </summary>
        private void ConfigureGrids()
        {
            // Cấu hình Grid Top Khách Hàng
            if (dgvCustomerDetails != null)
            {
                dgvCustomerDetails.Columns.Clear();
                dgvCustomerDetails.ApplyBaseStyle(); // Dùng helper
                dgvCustomerDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgvCustomerDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "CustomerName",
                    DataPropertyName = "CustomerName",
                    HeaderText = "Tên Khách Hàng",
                    FillWeight = 30
                });
                dgvCustomerDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Phone",
                    DataPropertyName = "Phone",
                    HeaderText = "SĐT",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                dgvCustomerDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Email",
                    DataPropertyName = "Email",
                    HeaderText = "Email",
                    FillWeight = 30
                });
                dgvCustomerDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "TotalOrders",
                    DataPropertyName = "TotalOrders",
                    HeaderText = "Tổng Đơn",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });
                dgvCustomerDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "TotalRevenue",
                    DataPropertyName = "TotalRevenue",
                    HeaderText = "Tổng Doanh Thu",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
                });
            }

            // Cấu hình Grid Top Tài Xế
            if (dgvDriverDetails != null)
            {
                dgvDriverDetails.Columns.Clear();
                dgvDriverDetails.ApplyBaseStyle();
                dgvDriverDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgvDriverDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "DriverName",
                    DataPropertyName = "DriverName",
                    HeaderText = "Tên Tài Xế",
                    FillWeight = 30
                });
                dgvDriverDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Phone",
                    DataPropertyName = "Phone",
                    HeaderText = "SĐT",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                dgvDriverDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "LicenseType",
                    DataPropertyName = "LicenseType",
                    HeaderText = "GPLX",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                dgvDriverDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "VehiclePlate",
                    DataPropertyName = "VehiclePlate",
                    HeaderText = "Biển Số Xe",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
                dgvDriverDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "TotalShipments",
                    DataPropertyName = "TotalShipments",
                    HeaderText = "Tổng Chuyến",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });
            }
        }
        // <--- KẾT THÚC THÊM HÀM MỚI ---

        // Init cơ bản cho CartesianChart (LiveCharts)
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

        // ====================== Wire filters ======================
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
        }

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

            // đồng bộ lại datepicker (tránh vòng lặp sự kiện)
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

        // ====================== Load tổng ======================
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
                MessageBox.Show("Lỗi tải thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================== KPI ======================
        private void LoadKpis(DateTime from, DateTime to)
        {
            var kpi = _statsSvc.GetKpis(from, to);
            lblTotalOrdersValue.Text = kpi.TotalOrders.ToString("N0");
            lblInProgressValue.Text = kpi.ShipmentsInProgress.ToString("N0");
            lblCompletedValue.Text = kpi.ShipmentsCompleted.ToString("N0");
            lblRevenueValue.Text = kpi.TotalRevenue.ToString("N0") + " đ";
        }

        // ====================== Pie: Đơn hàng ======================
        private void LoadOrderStatusPie(DateTime from, DateTime to)
        {
            var data = _statsSvc.GetOrderStatusCounts(from, to);

            var series = new SeriesCollection();
            if (data == null || data.Count == 0 || data.All(d => d.Value == 0))
            {
                series.Add(new PieSeries
                {
                    Title = "Không có dữ liệu",
                    Values = new ChartValues<int> { 1 },
                    DataLabels = false,
                    Fill = WpfBrushes.LightGray
                });
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

        // ====================== Pie: Shipment ======================
        private void LoadShipmentStatusPie(DateTime from, DateTime to)
        {
            var data = _statsSvc.GetShipmentStatusCounts(from, to);

            var series = new SeriesCollection();
            if (data == null || data.Count == 0 || data.All(d => d.Value == 0))
            {
                series.Add(new PieSeries
                {
                    Title = "Không có dữ liệu",
                    Values = new ChartValues<int> { 1 },
                    DataLabels = false,
                    Fill = WpfBrushes.LightGray
                });
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

        // ====================== Revenue: Line ======================
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
            chartRevenueOverTime.AxisX.Add(new Axis
            {
                Labels = rows.Select(r => r.Date.ToString("dd/MM")).ToArray()
            });

            chartRevenueOverTime.AxisY.Clear();
            chartRevenueOverTime.AxisY.Add(new Axis
            {
                LabelFormatter = v => v.ToString("N0")
            });
        }

        // ====================== Bar ngang: Top Routes ======================
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
            chartTopRoutes.AxisY.Add(new Axis
            {
                Labels = rows.Select(x => x.Label).ToArray()
            });
            chartTopRoutes.AxisX.Clear();
            chartTopRoutes.AxisX.Add(new Axis
            {
                LabelFormatter = v => v.ToString("N0")
            });
        }

        // ====================== Bar ngang + Grid: Top Customers ======================
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
                chartTopCustomers.AxisY.Add(new Axis
                {
                    Labels = rows.Select(x => x.CustomerName).ToArray()
                });
                chartTopCustomers.AxisX.Clear();
                chartTopCustomers.AxisX.Add(new Axis
                {
                    LabelFormatter = v => v.ToString("N0")
                });
            }

            // Luôn tải grid, ngay cả khi biểu đồ rỗng
            if (dgvCustomerDetails != null)
            {
                var gridData = _statsSvc.GetTopCustomers(from, to, 10);
                dgvCustomerDetails.DataSource = gridData;
            }
        }

        // ====================== Bar ngang + Grid: Top Drivers ======================
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
                chartTopDrivers.AxisY.Add(new Axis
                {
                    Labels = rows.Select(x => x.DriverName).ToArray()
                });
                chartTopDrivers.AxisX.Clear();
                chartTopDrivers.AxisX.Add(new Axis
                {
                    LabelFormatter = v => v.ToString("N0")
                });
            }

            // Luôn tải grid, ngay cả khi biểu đồ rỗng
            if (dgvDriverDetails != null)
            {
                var gridData = _statsSvc.GetTopDrivers(from, to, 10);
                dgvDriverDetails.DataSource = gridData;
            }
        }
    }
}