using Guna.UI2.WinForms;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LMS.GUI.DriverAdmin
{
    public partial class ucDriverDetail_Admin : UserControl
    {
        private readonly int _driverId;
        private readonly DriverService _driverSvc = new DriverService();

        // biến kéo thả form cha
        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;
        private Point parentFormStartPoint = Point.Empty;

        public ucDriverDetail_Admin(int driverId)
        {
            InitializeComponent();
            _driverId = driverId;
            this.Load += UcDriverDetail_Admin_Load;
        }

        private void UcDriverDetail_Admin_Load(object sender, EventArgs e)
        {
            if (this.lblTitle != null)
            {
                this.lblTitle.Text = "Chi Tiết Tài Xế";
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureShipmentGrid();
            LoadDetails();
            WireDragEvents();

            // gán nút đóng nếu có (btnClose)
            var closeButton = this.Controls.Find("btnClose", true).FirstOrDefault();
            if (closeButton != null)
            {
                closeButton.Click += (s, ev) => this.FindForm()?.Close();
            }
        }

        // gán sự kiện kéo thả cho pnlTop
        private void WireDragEvents()
        {
            var dragHandle = this.Controls.Find("pnlTop", true).FirstOrDefault();
            if (dragHandle == null) return;

            dragHandle.MouseDown += DragHandle_MouseDown;
            dragHandle.MouseMove += DragHandle_MouseMove;
            dragHandle.MouseUp += DragHandle_MouseUp;
        }

        // cấu hình lưới lịch sử chuyến hàng
        private void ConfigureShipmentGrid()
        {
            dgvDriverShipments.Columns.Clear();
            dgvDriverShipments.ApplyBaseStyle();

            dgvDriverShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ShipmentNo",
                DataPropertyName = "ShipmentNo",
                HeaderText = "Mã Chuyến",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvDriverShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderNo",
                DataPropertyName = "OrderNo",
                HeaderText = "Mã Đơn",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvDriverShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Route",
                DataPropertyName = "Route",
                HeaderText = "Tuyến",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvDriverShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                DataPropertyName = "Status",
                HeaderText = "Trạng Thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvDriverShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StartedAt",
                DataPropertyName = "StartedAt",
                HeaderText = "Bắt Đầu",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
            });
            dgvDriverShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DeliveredAt",
                DataPropertyName = "DeliveredAt",
                HeaderText = "Kết Thúc",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
            });

            dgvDriverShipments.CellFormatting += dgvDriverShipments_CellFormatting;
        }

        // nạp chi tiết tài xế + lịch sử chuyến
        private void LoadDetails()
        {
            try
            {
                var dto = _driverSvc.GetDriverDetails(_driverId);

                lblFullName.Text = $"Họ Tên: {dto.Driver?.FullName ?? "(Trống)"}";
                lblPhone.Text = $"SĐT: {dto.Driver?.Phone ?? "(Trống)"}";
                lblCitizenId.Text = $"CCCD: {dto.Driver?.CitizenId ?? "(Trống)"}";
                lblLicenseType.Text = $"Bằng Lái: {dto.Driver?.LicenseType ?? "(Trống)"}";

                if (dto.Account != null)
                {
                    lblUsername.Text = $"Tài Khoản: {dto.Account.Username}";
                    lblPassword.Text = $"Mật Khẩu: {(string.IsNullOrWhiteSpace(dto.Account.PasswordHash) ? "(Chưa Đặt)" : "(Đã Thiết Lập)")}";
                    lblAccountStatus.Text = $"Trạng Thái TK: {(dto.Account.IsActive ? "Đang Hoạt Động" : "Bị Khóa")}";
                }
                else
                {
                    lblUsername.Text = "Tài Khoản: (Chưa Có)";
                    lblPassword.Text = "Mật Khẩu: (N/A)";
                    lblAccountStatus.Text = "Trạng Thái TK: (N/A)";
                }

                if (dto.Driver?.Vehicle != null)
                    lblVehiclePlate.Text = $"Xe Đang Chạy: {dto.Driver.Vehicle.PlateNo} ({dto.Driver.Vehicle.Type})";
                else
                    lblVehiclePlate.Text = "Xe Đang Chạy: (Chưa Gán)";

                var shipmentData = dto.Shipments.Select(s => new
                {
                    ShipmentNo = "SHP" + s.Id,
                    OrderNo = s.Order?.OrderNo ?? OrderCode.ToCode(s.OrderId),
                    Route = $"{s.FromWarehouse?.Name} → {s.ToWarehouse?.Name}",
                    s.StartedAt,
                    s.DeliveredAt,
                    s.Status
                }).ToList();

                dgvDriverShipments.DataSource = shipmentData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi Tải Chi Tiết Tài Xế: {ex.Message}", "Lỗi");
                this.FindForm()?.Close();
            }
        }

        // hiển thị trạng thái chuyến bằng tiếng việt
        private void dgvDriverShipments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvDriverShipments.Columns[e.ColumnIndex].Name == "Status" && e.Value is ShipmentStatus status)
            {
                switch (status)
                {
                    case ShipmentStatus.Pending: e.Value = "Chờ Nhận"; break;
                    case ShipmentStatus.Assigned: e.Value = "Đã Nhận"; break;
                    case ShipmentStatus.OnRoute: e.Value = "Đang Đi Đường"; break;
                    case ShipmentStatus.AtWarehouse: e.Value = "Đang Ở Kho"; break;
                    case ShipmentStatus.ArrivedDestination: e.Value = "Đã Tới Đích"; break;
                    case ShipmentStatus.Delivered: e.Value = "Đã Giao Xong"; break;
                    case ShipmentStatus.Failed: e.Value = "Gặp Sự Cố"; break;
                    default: e.Value = status.ToString(); break;
                }
                e.FormattingApplied = true;
            }
        }

        // xử lý kéo thả form cha
        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            var parentForm = this.FindForm();
            if (parentForm == null) return;

            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragStartPoint = Cursor.Position;
                parentFormStartPoint = parentForm.Location;
            }
        }

        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
        {
            var parentForm = this.FindForm();
            if (parentForm == null) return;

            if (isDragging)
            {
                var diff = Point.Subtract(Cursor.Position, new Size(dragStartPoint));
                parentForm.Location = Point.Add(parentFormStartPoint, new Size(diff));
            }
        }

        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
                dragStartPoint = Point.Empty;
                parentFormStartPoint = Point.Empty;
            }
        }
    }
}
