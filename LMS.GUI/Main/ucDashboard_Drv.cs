// LMS.GUI/Main/ucDashboard_Drv.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LMS.BUS.Dtos;
using LMS.BUS.Helpers;          // GridHelper
using LMS.BUS.Services;         // DriverShipmentService
using LMS.DAL.Models;           // ShipmentStatus
using LMS.BUS;                  // nếu có AppSession ở namespace khác thì đổi lại

namespace LMS.GUI.Main
{
    public partial class ucDashboard_Drv : UserControl
    {
        private readonly DriverShipmentService _svc = new DriverShipmentService();

        public ucDashboard_Drv()
        {
            InitializeComponent();

            // Wire sự kiện (nếu chưa gán ở Designer)
            this.Load += UcDashboard_Drv_Load;
            if (btnStartRun != null) btnStartRun.Click += BtnStartRun_Click;

            // Cấu hình lưới
            ConfigureGrid();
        }

        // ====== EVENTS ======
        private void UcDashboard_Drv_Load(object sender, EventArgs e)
        {
            // Lời chào
            var name = AppSession.DisplayName ?? "Tài xế";
            lblWelcome.Text = $"Chào mừng trở lại, {name}!";

            // Nạp dữ liệu dashboard
            try
            {
                LoadDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải Dashboard Tài xế.\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnStartRun_Click(object sender, EventArgs e)
        {
            // Nếu có chuyến đang hoạt động thì điều hướng sang Chi tiết chuyến
            try
            {
                int driverId = AppSession.DriverId ?? 0; // đảm bảo bạn có AppSession.DriverId
                var active = _svc.GetAssignedAndRunning(driverId);

                if (active != null && active.Any())
                {
                    // Điều hướng sang UC Chi tiết chuyến
                    var parent = this.FindForm() as frmMain_Driver;
                    if (parent != null)
                    {
                        parent.NavigateToShipmentDetail();
                        return;
                    }
                }

                // Không có chuyến đang chạy => chuyển sang “Đơn hàng của tôi”
                var frm = this.FindForm() as frmMain_Driver;
                if (frm != null)
                {
                    // giả sử nút MyShipments có handler sẵn trong frmMain_Driver
                    frm.NavigateToShipmentDetail(); // hoặc điều hướng sang ucMyShipments_Drv nếu bạn muốn
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể điều hướng.\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ====== LOAD DATA ======
        private void LoadDashboard()
        {
            int driverId = AppSession.DriverId ?? 0; // hoặc lấy từ ctor/form nếu bạn dùng cách khác

            // Lấy 30 ngày gần nhất để hiển thị “5 chuyến gần đây”
            DateTime from = DateTime.Now.Date.AddDays(-30);
            DateTime to = DateTime.Now.Date.AddDays(1);

            var all = _svc.GetAllMine(driverId, from, to, null); // List<ShipmentRowDto>

            // --- KPI ---
            int pending = all.Count(x => IsPendingOrAssigned(x.Status));
            int completedToday = all.Count(x =>
                ToEnumStatus(x.Status) == ShipmentStatus.Delivered &&
                x.DeliveredAt.HasValue &&
                x.DeliveredAt.Value.Date == DateTime.Today);
            int completedTotal = all.Count(x => ToEnumStatus(x.Status) == ShipmentStatus.Delivered);

            lblKpiPending.Text = pending.ToString();
            lblKpiCompletedToday.Text = completedToday.ToString();
            lblKpiCompletedTotal.Text = completedTotal.ToString();

            // --- 5 chuyến gần đây ---
            var recent = all
                .OrderByDescending(x => x.UpdatedAt ?? x.DeliveredAt ?? x.StartedAt)
                .Take(5)
                .Select(x => new GridRow
                {
                    ShipmentNo = x.ShipmentNo,
                    Route = x.Route,
                    StartedAt = x.StartedAt?.ToString("dd/MM HH:mm") ?? "-",
                    DeliveredAt = x.DeliveredAt?.ToString("dd/MM HH:mm") ?? "-",
                    Status = ToVN(x.Status)
                })
                .ToList();

            dgvLatestShipments.DataSource = recent;
        }

        // ====== GRID ======
        private void ConfigureGrid()
        {
            dgvLatestShipments.ApplyBaseStyle();
            dgvLatestShipments.AutoGenerateColumns = false;
            dgvLatestShipments.Columns.Clear();

            dgvLatestShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GridRow.ShipmentNo),
                HeaderText = "Mã Chuyến",
                Width = 110
            });
            dgvLatestShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GridRow.Route),
                HeaderText = "Tuyến đường",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvLatestShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GridRow.StartedAt),
                HeaderText = "Bắt đầu",
                Width = 95,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvLatestShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GridRow.DeliveredAt),
                HeaderText = "Kết thúc",
                Width = 95,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvLatestShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GridRow.Status),
                HeaderText = "Trạng thái",
                Width = 140
            });

            dgvLatestShipments.CellDoubleClick += DgvLatestShipments_CellDoubleClick;
        }

        private void DgvLatestShipments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvLatestShipments.Rows[e.RowIndex].DataBoundItem as GridRow;
            if (row == null) return;

            // Tìm form cha
            if (this.FindForm() is frmMain_Driver parent)
            {
                // Điều hướng sang UC chi tiết
                parent.NavigateToShipmentDetail();

                // Delay nhẹ để UC được load xong
                parent.BeginInvoke(new Action(() =>
                {
                    foreach (Control ctrl in parent.Controls)
                    {
                        if (ctrl is Panel pnlContent)
                        {
                            var uc = pnlContent.Controls.OfType<LMS.GUI.OrderDriver.ucShipmentDetail_Drv>().FirstOrDefault();
                            if (uc != null)
                            {
                                // Gọi hàm chọn dòng
                                uc.SelectShipmentByNo(row.ShipmentNo);
                                break;
                            }
                        }
                    }
                }));
            }
        }



        // ====== HELPERS ======
        private static ShipmentStatus? ToEnumStatus(string statusStr)
        {
            // statusStr đến từ enum .ToString() ở Service
            if (Enum.TryParse(statusStr, out ShipmentStatus s)) return s;
            return null;
        }

        private static bool IsPendingOrAssigned(string statusStr)
        {
            var s = ToEnumStatus(statusStr);
            return s == ShipmentStatus.Pending || s == ShipmentStatus.Assigned;
        }

        private static string ToVN(string statusStr)
        {
            // Việt hoá nhanh cho hiển thị lưới/kpi
            var s = ToEnumStatus(statusStr);
            switch (s)
            {
                case ShipmentStatus.Pending: return "Chờ nhận";
                case ShipmentStatus.Assigned: return "Đã nhận chuyến";
                case ShipmentStatus.OnRoute: return "Đang vận chuyển";
                case ShipmentStatus.AtWarehouse: return "Đang ở kho";
                case ShipmentStatus.ArrivedDestination: return "Đã tới đích";
                case ShipmentStatus.Delivered: return "Hoàn thành";
                case ShipmentStatus.Failed: return "Đã hủy / Sự cố";
                //case ShipmentStatus.Canceled: return "Đã hủy";
                default: return statusStr ?? "-";
            }
        }

        // ViewModel nhỏ cho lưới
        private class GridRow
        {
            public int Id { get; set; }        // nếu ShipmentRowDto có Id
            public string ShipmentNo { get; set; }
            public string Route { get; set; }
            public string StartedAt { get; set; }
            public string DeliveredAt { get; set; }
            public string Status { get; set; }
        }
    }
}
