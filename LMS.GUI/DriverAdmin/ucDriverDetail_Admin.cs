// LMS.GUI/DriverAdmin/ucDriverDetail_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Helpers;
using LMS.BUS.Services; // Cần cho DriverService
using LMS.DAL.Models;  // Cần cho Shipment, ShipmentStatus...
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LMS.GUI.DriverAdmin // Đổi namespace
{
    public partial class ucDriverDetail_Admin : UserControl
    {
        private readonly int _driverId;
        private readonly DriverService _driverSvc = new DriverService();

        // ==========================================================
        // === (1) THÊM CÁC BIẾN ĐỂ KÉO THẢ FORM CHỨA UC NÀY ===
        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;
        private Point parentFormStartPoint = Point.Empty;
        // ==========================================================

        public ucDriverDetail_Admin(int driverId)
        {
            InitializeComponent();
            _driverId = driverId;
            this.Load += UcDriverDetail_Admin_Load;
            // Đảm bảo các Label (lblFullName, lblPhone,...) 
            // và dgvDriverShipments tồn tại trong Designer.cs
        }

        private void UcDriverDetail_Admin_Load(object sender, EventArgs e)
        {
            if (this.lblTitle != null)
            {
                this.lblTitle.Text = "Chi Tiết Khách Hàng";
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureShipmentGrid(); // Đổi tên hàm
            LoadDetails();
            WireDragEvents(); // Gọi hàm gán sự kiện kéo thả

            // Gán sự kiện nút đóng (Giả sử tên là btnClose)
            var closeButton = this.Controls.Find("btnClose", true).FirstOrDefault();
            if (closeButton != null)
            {
                closeButton.Click += (s, ev) => this.FindForm()?.Close();
            }
        }

        // ==========================================================
        // === (2) HÀM GÁN SỰ KIỆN KÉO THẢ CHO pnlTop ===
        private void WireDragEvents()
        {
            // !!! QUAN TRỌNG: Giả sử Panel trên cùng có tên là 'pnlTop' !!!
            // Bạn cần đảm bảo control này tồn tại trong Designer
            Control dragHandle = this.Controls.Find("pnlTop", true).FirstOrDefault() as Control;

            // Nếu không tìm thấy pnlTop, có thể dùng chính User Control này hoặc control khác
            if (dragHandle == null) return;

            dragHandle.MouseDown += DragHandle_MouseDown;
            dragHandle.MouseMove += DragHandle_MouseMove;
            dragHandle.MouseUp += DragHandle_MouseUp;
        }
        // ==========================================================


        private void ConfigureShipmentGrid()
        {
            dgvDriverShipments.Columns.Clear(); // Giả sử tên là dgvDriverShipments
            dgvDriverShipments.ApplyBaseStyle();

            // Định nghĩa cột cho grid lịch sử chuyến hàng
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
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvDriverShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StartedAt",
                DataPropertyName = "StartedAt",
                HeaderText = "Bắt đầu",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
            });
            dgvDriverShipments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DeliveredAt",
                DataPropertyName = "DeliveredAt",
                HeaderText = "Kết thúc",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
            });

            // Gán sự kiện format cho cột Status
            dgvDriverShipments.CellFormatting += dgvDriverShipments_CellFormatting;
        }

        private void LoadDetails()
        {
            try
            {
                var dto = _driverSvc.GetDriverDetails(_driverId);

                // Gán thông tin Tài xế
                lblFullName.Text = $"Họ tên: {dto.Driver?.FullName ?? "(trống)"}";
                lblPhone.Text = $"SĐT: {dto.Driver?.Phone ?? "(trống)"}";
                lblCitizenId.Text = $"CCCD: {dto.Driver?.CitizenId ?? "(trống)"}"; // Thêm
                lblLicenseType.Text = $"Bằng lái: {dto.Driver?.LicenseType ?? "(trống)"}"; // Thêm

                // Gán thông tin Tài khoản
                if (dto.Account != null)
                {
                    lblUsername.Text = $"Tài khoản: {dto.Account.Username}";
                    // Đây là Hash, không nên hiển thị, chỉ hiển thị thông báo
                    lblPassword.Text = $"Mật khẩu: {(string.IsNullOrWhiteSpace(dto.Account.PasswordHash) ? "(chưa đặt)" : "(Đã thiết lập)")}";
                    lblAccountStatus.Text = $"Trạng thái TK: {(dto.Account.IsActive ? "Đang hoạt động" : "Bị khóa")}";
                }
                else
                {
                    lblUsername.Text = "Tài khoản: (Chưa có)";
                    lblPassword.Text = "Mật khẩu: (N/A)";
                    lblAccountStatus.Text = "Trạng thái TK: (N/A)";
                }
                if (dto.Driver?.Vehicle != null) // Kiểm tra xem Driver có được gán xe không
                {
                    // Nếu có xe, hiển thị biển số và loại xe
                    lblVehiclePlate.Text = $"Xe đang chạy: {dto.Driver.Vehicle.PlateNo} ({dto.Driver.Vehicle.Type})";
                }
                else
                {
                    // Nếu không có xe, hiển thị "(Chưa gán)"
                    lblVehiclePlate.Text = "Xe đang chạy: (Chưa gán)";
                }

                // Gán dữ liệu Lịch sử chuyến hàng
                var shipmentData = dto.Shipments.Select(s => new
                {
                    ShipmentNo = "SHP" + s.Id, // Tạo mã chuyến
                    OrderNo = s.Order?.OrderNo ?? OrderCode.ToCode(s.OrderId), // Lấy mã đơn
                    Route = $"{s.FromWarehouse?.Name} → {s.ToWarehouse?.Name}", // Tạo tuyến
                    s.StartedAt,
                    s.DeliveredAt,
                    s.Status // Giữ Enum để format
                }).ToList();

                dgvDriverShipments.DataSource = shipmentData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải chi tiết tài xế: {ex.Message}", "Lỗi");
                this.FindForm()?.Close();
            }
        }

        // Format ShipmentStatus Enum sang tiếng Việt
        private void dgvDriverShipments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvDriverShipments.Columns[e.ColumnIndex].Name == "Status" && e.Value is ShipmentStatus status)
            {
                switch (status)
                {
                    case ShipmentStatus.Pending: e.Value = "Chờ nhận"; break;
                    case ShipmentStatus.Assigned: e.Value = "Đã nhận"; break;
                    case ShipmentStatus.OnRoute: e.Value = "Đang đi đường"; break;
                    case ShipmentStatus.AtWarehouse: e.Value = "Đang ở kho"; break;
                    case ShipmentStatus.ArrivedDestination: e.Value = "Đã tới đích"; break;
                    case ShipmentStatus.Delivered: e.Value = "Đã giao xong"; break;
                    case ShipmentStatus.Failed: e.Value = "Gặp sự cố"; break;
                    default: e.Value = status.ToString(); break;
                }
                e.FormattingApplied = true;
            }
        }

        // ==========================================================
        // === (3) THÊM 3 HÀM XỬ LÝ KÉO THẢ CHO FORM CHỨA UC NÀY ===
        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            // Lấy Form cha (Form popup)
            Form parentForm = this.FindForm();
            if (parentForm == null) return;

            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragStartPoint = Cursor.Position;
                parentFormStartPoint = parentForm.Location; // Lấy vị trí của Form cha
            }
        }

        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
        {
            // Lấy Form cha
            Form parentForm = this.FindForm();
            if (parentForm == null) return;

            if (isDragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragStartPoint));
                parentForm.Location = Point.Add(parentFormStartPoint, new Size(diff)); // Di chuyển Form cha
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
        // ==========================================================
    }
}