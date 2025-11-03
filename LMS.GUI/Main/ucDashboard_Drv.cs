// LMS.GUI/Main/ucDashboard_Drv.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.BUS;

namespace LMS.GUI.Main
{
    public partial class ucDashboard_Drv : UserControl
    {
        private readonly DriverShipmentService _svc = new DriverShipmentService();

        public ucDashboard_Drv()
        {
            InitializeComponent();

            // gắn sự kiện cần thiết (nếu chưa gán ở designer)
            this.Load += UcDashboard_Drv_Load;
            if (btnStartRun != null) btnStartRun.Click += BtnStartRun_Click;

            // cấu hình lưới
            ConfigureGrid();
        }

        private void UcDashboard_Drv_Load(object sender, EventArgs e)
        {
            // lời chào
            var name = AppSession.DisplayName ?? "Tài xế";
            lblWelcome.Text = $"Chào mừng trở lại, {name}!";
            lblWelcome.Left = (this.Width - lblWelcome.Width) / 2;

            // nạp dữ liệu dashboard
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
            // nếu có chuyến đang hoạt động thì điều hướng sang chi tiết chuyến
            try
            {
                int driverId = AppSession.DriverId ?? 0;
                var active = _svc.GetAssignedAndRunning(driverId);

                if (active != null && active.Any())
                {
                    var parent = this.FindForm() as frmMain_Driver;
                    if (parent != null)
                    {
                        parent.NavigateToShipmentDetail();
                        return;
                    }
                }

                // không có chuyến đang chạy => chuyển sang “đơn hàng của tôi” / chi tiết
                var frm = this.FindForm() as frmMain_Driver;
                if (frm != null)
                {
                    frm.NavigateToShipmentDetail();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể điều hướng.\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadDashboard()
        {
            int driverId = AppSession.DriverId ?? 0;

            // lấy 30 ngày gần nhất để hiển thị “5 chuyến gần đây”
            DateTime from = DateTime.Now.Date.AddDays(-30);
            DateTime to = DateTime.Now.Date.AddDays(1);

            var all = _svc.GetAllMine(driverId, from, to, null); // List<ShipmentRowDto>

            // kpi
            int pending = all.Count(x => IsPendingOrAssigned(x.Status));
            int completedToday = all.Count(x =>
                ToEnumStatus(x.Status) == ShipmentStatus.Delivered &&
                x.DeliveredAt.HasValue &&
                x.DeliveredAt.Value.Date == DateTime.Today);
            int completedTotal = all.Count(x => ToEnumStatus(x.Status) == ShipmentStatus.Delivered);

            lblKpiPending.Text = pending.ToString();
            lblKpiCompletedToday.Text = completedToday.ToString();
            lblKpiCompletedTotal.Text = completedTotal.ToString();

            // 5 chuyến gần đây
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

            // điều hướng sang uc chi tiết và chọn đúng chuyến
            if (this.FindForm() is frmMain_Driver parent)
            {
                parent.NavigateToShipmentDetail();
                parent.BeginInvoke(new Action(() =>
                {
                    foreach (Control ctrl in parent.Controls)
                    {
                        if (ctrl is Panel pnlContent)
                        {
                            var uc = pnlContent.Controls.OfType<LMS.GUI.OrderDriver.ucShipmentDetail_Drv>().FirstOrDefault();
                            if (uc != null)
                            {
                                uc.SelectShipmentByNo(row.ShipmentNo);
                                break;
                            }
                        }
                    }
                }));
            }
        }

        // helpers cho status
        private static ShipmentStatus? ToEnumStatus(string statusStr)
        {
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
                default: return statusStr ?? "-";
            }
        }

        private class GridRow
        {
            public int Id { get; set; }
            public string ShipmentNo { get; set; }
            public string Route { get; set; }
            public string StartedAt { get; set; }
            public string DeliveredAt { get; set; }
            public string Status { get; set; }
        }
    }
}
