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

// dùng livecharts wpf (series) và host trong winforms controls
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

                // chỉ cho phép double-click mở chi tiết trên grid đơn hàng chờ duyệt
                dgvPendingOrders.CellDoubleClick += DgvPendingOrders_CellDoubleClick;
            }
        }

        // bọc toàn bộ nội dung usercontrol vào 1 panel có scrollbar (không cần sửa designer)
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
                // load dữ liệu cho dashboard
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

        // ===================== kpi (có hover so sánh tháng trước) =====================

        private void LoadKpis()
        {
            using (var db = new LogisticsDbContext())
            {
                // số liệu hiện tại
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
                    .Where(o => o.Status == OrderStatus.Completed && o.CreatedAt >= monthStart)
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

                // gán label hiển thị
                lblKpiPendingOrders.Text = pendingOrders.ToString("N0");
                lblKpiActiveShipments.Text = activeShipments.ToString("N0");
                lblKpiTotalCustomers.Text = totalCustomers.ToString("N0");
                lblKpiTotalShipments.Text = totalShipments.ToString("N0");
                lblKpiRevenue.Text = revenueThisMonth.ToString("N0");

                // tooltip so sánh với tháng trước
                SetHoverDelta(pnlKpiPendingOrders, pendingOrders,
                    CountOrders(prevMonthStart, prevMonthEnd, OrderStatus.Pending), "Đơn chờ duyệt");

                SetHoverDelta(pnlKpiActiveShipments, activeShipments,
                    CountActiveShipments(prevMonthStart, prevMonthEnd), "Chuyến đang chạy");

                SetHoverDelta(pnlKpiTotalCustomers, totalCustomers,
                    CountCustomers(prevMonthEnd), "Tổng khách hàng");

                SetHoverDelta(pnlKpiTotalShipments, totalShipments,
                    CountShipments(prevMonthStart, prevMonthEnd), "Tổng số chuyến");

                SetHoverDelta(pnlKpiRevenue, revenueThisMonth, revenuePrevMonth, "Doanh thu");
            }
        }

        // đặt tooltip delta cho kpi số nguyên
        private void SetHoverDelta(Control pnl, int current, int previous, string label)
        {
            string delta = BuildDeltaText(current, previous);
            _tip.SetToolTip(pnl, $"{label}: {delta}");
        }

        // đặt tooltip delta cho kpi tiền
        private void SetHoverDelta(Control pnl, decimal current, decimal previous, string label)
        {
            string delta = BuildDeltaText(current, previous);
            _tip.SetToolTip(pnl, $"{label}: {delta}");
        }

        // tạo chuỗi so sánh dạng +x (y%)
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

        // các helper đọc db để tính delta kpi
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
                // hiện không có CreatedAt cho customer nên đếm tổng
                return db.Customers.AsNoTracking().Count();
            }
        }

        private int CountShipments(DateTime from, DateTime to)
        {
            using (var db = new LogisticsDbContext())
            {
                // shipment không có CreatedAt nên dùng StartedAt làm mốc
                return db.Shipments.AsNoTracking()
                    .Count(s => s.StartedAt != null && s.StartedAt >= from && s.StartedAt <= to);
            }
        }

        // ======================= charts =======================

        // doanh thu 30 ngày gần nhất
        private void BindRevenue30Days()
        {
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
                    .Select(g => new { Day = g.Key.Value, Sum = g.Sum(x => x.TotalFee) })
                    .ToList();

                foreach (var x in data)
                {
                    int idx = (x.Day - start).Days;
                    if (idx >= 0 && idx < 30) values[idx] = x.Sum;
                }
            }

            // cấu hình series và trục cho chart doanh thu
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

        // biểu đồ tròn trạng thái đơn hàng
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

        // ======================= grids =======================

        // cấu hình grid đơn hàng chờ duyệt
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
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy HH:mm",
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Giá trị",
                DataPropertyName = nameof(PendingOrderRow.Amount),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });
            dgvPendingOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Trạng thái",
                DataPropertyName = nameof(PendingOrderRow.StatusText),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
        }

        // load dữ liệu đơn hàng chờ duyệt (top 30 mới nhất)
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

        // mở dialog chi tiết đơn khi double-click dòng
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

        // cấu hình grid chuyến đang chạy
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
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy HH:mm",
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            dgvActiveShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Trạng thái",
                DataPropertyName = nameof(ActiveShipmentRow.StatusText),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
        }

        // load dữ liệu chuyến đang chạy (top 30 cập nhật gần nhất)
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
                    RouteText =
                        $"{(whDict.TryGetValue(s.FromWarehouseId, out var fromN) ? fromN : "Kho ?")} → " +
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

        // mở dialog chứa usercontrol chi tiết đơn hàng
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

        // public refresh cho dashboard
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
