//using LMS.BUS.Helpers;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace LMS.GUI.Main
//{
//    public partial class frmMain_Admin : Form
//    {
//        bool menuExpant = false;

//        private List<Button> navigationButtons;
//        private Color normalColor = Color.FromArgb(32, 33, 36);
//        private Color selectedColor = Color.FromArgb(0, 4, 53);

//        private bool dragging = false;
//        private Point dragCursorPoint;
//        private Point dragFormPoint;

//        public frmMain_Admin()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen;
//            this.FormBorderStyle = FormBorderStyle.None;

//            foreach (Control control in sidebar.Controls)
//            {
//                control.Width = sidebar.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
//            }

//            InitializeNavigationButtons();
//            btnMenu.Click += btnMenu_Click;
//            this.Load += FrmMain_Admin_Load;
//            this.FormClosing += new FormClosingEventHandler(this.Admin_FormClosing);
//            lblTitle.Click += lblTitle_Click;
//            btnHam.Click += btnHam_Click;

//            if (this.pnlTop != null) // Kiểm tra panel tồn tại
//            {
//                this.pnlTop.MouseDown += PnlTop_MouseDown;
//                this.pnlTop.MouseMove += PnlTop_MouseMove;
//                this.pnlTop.MouseUp += PnlTop_MouseUp;
//            }

//            LoadUc(new LMS.GUI.Main.ucDashboard_Ad());
//            lblPageTitle.Text = "Trang Chủ";
//        }

//        private void FrmMain_Admin_Load(object sender, EventArgs e)
//        {
//            if (!AppSession.IsLoggedIn || AppSession.Role != DAL.Models.UserRole.Admin)
//            {
//                MessageBox.Show("Bạn chưa đăng nhập bằng tài khoản Quản trị viên.", "Lỗi Truy Cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                this.Close();
//                return;
//            }

//            // THÊM LẠI ĐOẠN NÀY ĐỂ HIỂN THỊ NGAY LẬP TỨC
//            if (AppSession.IsLoggedIn)
//            {
//                string userName = AppSession.DisplayName;
//                string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
//                tsslblUserInfo.Text = $"User: {userName} - {dateTimeNow}";
//            }
//            else
//            {
//                tsslblUserInfo.Text = "Chưa đăng nhập";
//            }
//            if (tsslblUserInfo != null) // Giả sử bạn có label chào mừng
//            {
//                tsslblUserInfo.Text = $"Xin chào, {AppSession.DisplayName}";
//            }

//        }

//        private void InitializeNavigationButtons()
//        {
//            navigationButtons = new List<Button>
//            {
//                btnOrder,
//                btnCustomer, // Đảm bảo nút này tồn tại trong Designer
//                btnDriver,
//                btnShipment,
//                btnWarehouse,
//                btnRouteTemplate,
//                btnVehicle,
//                btnReport,
//            };

//            foreach (Button btn in navigationButtons)
//            {
//                btn.Click += NavigationButton_Click;
//            }
//            btnHome.Click += btnHome_Click;
//            btnLogOut.Click += btnLogOut_Click;
//            btnAccount.Click += btnAccount_Click;
//            btnHam.Click += btnHam_Click; // Giữ lại các sự kiện gốc
//            lblTitle.Click += lblTitle_Click; // Giữ lại các sự kiện gốc
//        }

//        private void ResetButtonStyles()
//        {
//            foreach (Button btn in navigationButtons)
//            {
//                btn.BackColor = normalColor;
//            }
//        }

//        private void NavigationButton_Click(object sender, EventArgs e)
//        {
//            ResetButtonStyles();
//            Button clickedButton = sender as Button;
//            if (clickedButton != null)
//            {
//                clickedButton.BackColor = selectedColor;
//            }

//            if (clickedButton == btnOrder)
//            {
//                LoadUc(new LMS.GUI.OrderAdmin.ucOrder_Admin());
//                lblPageTitle.Text = "Quản Lý / Đơn Hàng";
//            }
//            else if (clickedButton == btnCustomer)
//            {
//                LoadUc(new LMS.GUI.CustomerAdmin.ucCustomer_Admin()); // Load UC Customer
//                lblPageTitle.Text = "Quản Lý / Khách Hàng";
//            }
//            else if (clickedButton == btnDriver)
//            {
//                LoadUc(new LMS.GUI.DriverAdmin.ucDriver_Admin());
//                lblPageTitle.Text = "Quản lý / Tài Xế";
//            }
//            else if (clickedButton == btnShipment)
//            {
//                LoadUc(new LMS.GUI.ShipmentAdmin.ucShipment_Admin()); // Load UC Shipment
//                lblPageTitle.Text = "Quản Lý / Chuyến Hàng";
//            }
//            else if (clickedButton == btnWarehouse)
//            {
//                LoadUc(new LMS.GUI.WarehouseAdmin.ucWarehouse_Admin()); // Load UC Shipment
//                lblPageTitle.Text = "Quản Lý / Kho";
//            }
//            else if (clickedButton == btnRouteTemplate)
//            {
//                LoadUc(new LMS.GUI.RouteTemplateAdmin.ucRouteTemplate_Admin()); // Load UC Shipment
//                lblPageTitle.Text = "Quản Lý / Kho";
//            }
//            else if (clickedButton == btnVehicle)
//            {
//                LoadUc(new LMS.GUI.VehicleAdmin.ucVehicle_Admin()); // Load UC Shipment
//                lblPageTitle.Text = "Quản Lý / Phương Tiện";
//            }
//            else if (clickedButton == btnReport)
//            {
//                LoadUc(new LMS.GUI.ReportAdmin.ucStatistics()); // Load UC Shipment
//                lblPageTitle.Text = "Quản Lý / Phương Tiện";
//            }
//        }
//        private void btnHome_Click(object sender, EventArgs e)
//        {
//            ResetButtonStyles();
//            LoadUc(new LMS.GUI.Main.ucDashboard_Ad());
//            lblPageTitle.Text = "Trang Chủ";
//        }

//        private void btnAccount_Click(object sender, EventArgs e)
//        {
//            ResetButtonStyles();
//            LoadUc(new LMS.GUI.AccountAdmin.ucAccount_Admin());
//            lblPageTitle.Text = "Quản Lý / Tài Khoản";
//        }

//        private void btnLogOut_Click(object sender, EventArgs e)
//        {
//            if (HandleLogout())
//            {
//                this.Close();
//            }
//        }

//        private void timerClock_Tick(object sender, EventArgs e)
//        {
//            if (AppSession.IsLoggedIn)
//            {
//                string userName = AppSession.DisplayName; // Lấy tên từ AppSession
//                string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"); // Định dạng ngày giờ

//                tsslblUserInfo.Text = $"User: {userName} - {dateTimeNow}";
//            }
//            //else
//            //{
//            //    tsslblUserInfo.Text = "Chưa đăng nhập";
//            //}
//        }

//        private void Admin_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            // Chỉ kiểm tra xác nhận nếu form đang đóng do người dùng nhấn nút Close
//            if (e.CloseReason == CloseReason.UserClosing)
//            {
//                if (!HandleLogout())
//                {
//                    e.Cancel = true; // Hủy đóng form nếu người dùng chọn "No"
//                }
//            }
//        }

//        // Hàm chung xử lý logic đăng xuất
//        private bool HandleLogout()
//        {
//            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//            {
//                return true; // Cho phép đóng form
//            }
//            return false; // Hủy đóng form
//        }


//        public void LoadUc(UserControl uc)
//        {
//            pnlContent.Controls.Clear();   // xóa UC cũ
//            uc.Dock = DockStyle.Fill;       // cho UC mới chiếm toàn panel
//            pnlContent.Controls.Add(uc);    // thêm UC mới vào
//        }

//        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left)
//            {
//                dragging = true;
//                dragCursorPoint = Cursor.Position;
//                dragFormPoint = this.Location;
//            }
//        }

//        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (dragging)
//            {
//                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
//                this.Location = Point.Add(dragFormPoint, new Size(dif));
//            }
//        }

//        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left)
//            {
//                dragging = false;
//            }
//        }

//        private void menuTransition_Tick(object sender, EventArgs e)
//        {
//            if (menuExpant == false)
//            {
//                menuContainer.Height += 10;
//                if (menuContainer.Height >= 525)
//                {
//                    menuTransition.Stop();
//                    menuExpant = true;
//                }
//            }
//            else
//            {
//                menuContainer.Height -= 10;
//                if (menuContainer.Height <= 58)
//                {
//                    menuTransition.Stop();
//                    menuExpant = false;
//                }
//            }
//        }

//        private void btnHam_Click(object sender, EventArgs e)
//        {
//            sidebarTransition.Start();
//        }

//        private void btnMenu_Click(object sender, EventArgs e)
//        {
//            ResetButtonStyles();
//            menuTransition.Start();
//        }

//        bool sidebarExpant = true;

//        private void sidebarTransition_Tick(object sender, EventArgs e)
//        {
//            if (sidebarExpant)
//            {
//                sidebar.Width -= 10;
//                if (sidebar.Width <= 78)
//                {
//                    sidebarExpant = false;
//                    sidebarTransition.Stop();
//                }
//            }
//            else
//            {
//                sidebar.Width += 5;
//                if (sidebar.Width >= 301)
//                {
//                    sidebarExpant = true;
//                    sidebarTransition.Stop();
//                }
//            }
//        }

//        private void lblTitle_Click(object sender, EventArgs e)
//        {
//            sidebarTransition.Start();
//        }


//    }
//}

using LMS.BUS.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LMS.GUI.Main
{
    public partial class frmMain_Admin : Form
    {
        // ===== STATE =====
        bool menuExpant = false;
        bool sidebarExpant = true;

        private List<Button> navigationButtons;
        private readonly Color normalColor = Color.FromArgb(32, 33, 36);
        private readonly Color selectedColor = Color.FromArgb(0, 4, 53);

        // Drag to move form via pnlTop
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public frmMain_Admin()
        {
            InitializeComponent();

            // Cấu hình Form
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MinimumSize = new Size(1000, 650);
            this.DoubleBuffered = true; // giảm nhấp nháy

            // Fit chiều rộng item trong sidebar (nếu cần)
            if (sidebar != null)
            {
                foreach (Control control in sidebar.Controls)
                {
                    control.Width = sidebar.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
                }
            }

            InitializeNavigationButtons();

            // Chỉ gán sự kiện 1 lần (không gán trùng ở 2 nơi)
            this.Load += FrmMain_Admin_Load;
            this.FormClosing += Admin_FormClosing;

            // Kéo thả form bằng pnlTop (nếu tồn tại)
            if (this.pnlTop != null)
            {
                this.pnlTop.MouseDown += PnlTop_MouseDown;
                this.pnlTop.MouseMove += PnlTop_MouseMove;
                this.pnlTop.MouseUp += PnlTop_MouseUp;
            }

            // KHÔNG load dashboard ở đây để tránh nháy trước khi check quyền
            // LoadUc(new LMS.GUI.Main.ucDashboard_Ad());
            // lblPageTitle.Text = "Trang Chủ";
        }

        private void FrmMain_Admin_Load(object sender, EventArgs e)
        {
            // Check đăng nhập + vai trò Admin
            if (!AppSession.IsLoggedIn || AppSession.Role != DAL.Models.UserRole.Admin)
            {
                MessageBox.Show("Bạn chưa đăng nhập bằng tài khoản Quản trị viên.",
                    "Lỗi Truy Cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Hiển thị user info 1 lần (không ghi đè)
            var userName = AppSession.DisplayName ?? "Admin";
            tsslblUserInfo.Text = $"User: {userName} - {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            // Nếu muốn cập nhật theo thời gian thực, bật timer trong Designer:
            // timerClock.Enabled = true;

            // Sau khi xác thực -> nạp Dashboard
            LoadUc(new LMS.GUI.Main.ucDashboard_Ad());
            lblPageTitle.Text = "Trang Chủ";
        }

        private void InitializeNavigationButtons()
        {
            navigationButtons = new List<Button>
            {
                btnOrder,
                btnCustomer,
                btnDriver,
                btnShipment,
                btnWarehouse,
                btnRouteTemplate,
                btnVehicle,
                btnReport
            };

            foreach (Button btn in navigationButtons)
                btn.Click += NavigationButton_Click;

            // Gán sự kiện điều hướng/chung TẠI MỘT NƠI
            btnHome.Click += btnHome_Click;
            btnLogOut.Click += btnLogOut_Click;
            btnAccount.Click += btnAccount_Click;
            btnHam.Click += btnHam_Click;
            btnMenu.Click += btnMenu_Click;
            lblTitle.Click += lblTitle_Click;
        }

        private void ResetButtonStyles()
        {
            foreach (Button btn in navigationButtons)
                btn.BackColor = normalColor;
        }

        private void NavigationButton_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();

            if (sender is Button clickedButton)
                clickedButton.BackColor = selectedColor;

            if (sender == btnOrder)
            {
                LoadUc(new LMS.GUI.OrderAdmin.ucOrder_Admin());
                lblPageTitle.Text = "Quản Lý / Đơn Hàng";
            }
            else if (sender == btnCustomer)
            {
                LoadUc(new LMS.GUI.CustomerAdmin.ucCustomer_Admin());
                lblPageTitle.Text = "Quản Lý / Khách Hàng";
            }
            else if (sender == btnDriver)
            {
                LoadUc(new LMS.GUI.DriverAdmin.ucDriver_Admin());
                lblPageTitle.Text = "Quản Lý / Tài Xế";
            }
            else if (sender == btnShipment)
            {
                LoadUc(new LMS.GUI.ShipmentAdmin.ucShipment_Admin());
                lblPageTitle.Text = "Quản Lý / Chuyến Hàng";
            }
            else if (sender == btnWarehouse)
            {
                LoadUc(new LMS.GUI.WarehouseAdmin.ucWarehouse_Admin());
                lblPageTitle.Text = "Quản Lý / Kho";
            }
            else if (sender == btnRouteTemplate)
            {
                LoadUc(new LMS.GUI.RouteTemplateAdmin.ucRouteTemplate_Admin());
                lblPageTitle.Text = "Quản Lý / Tuyến Đường Mẫu";
            }
            else if (sender == btnVehicle)
            {
                LoadUc(new LMS.GUI.VehicleAdmin.ucVehicle_Admin());
                lblPageTitle.Text = "Quản Lý / Phương Tiện";
            }
            else if (sender == btnReport)
            {
                LoadUc(new LMS.GUI.ReportAdmin.ucStatistics());
                lblPageTitle.Text = "Quản Lý / Báo Cáo";
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();
            LoadUc(new LMS.GUI.Main.ucDashboard_Ad());
            lblPageTitle.Text = "Trang Chủ";
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();
            LoadUc(new LMS.GUI.AccountAdmin.ucAccount_Admin());
            lblPageTitle.Text = "Quản Lý / Tài Khoản";
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            //if (HandleLogout())
            this.Close();
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            if (AppSession.IsLoggedIn)
            {
                string userName = AppSession.DisplayName ?? "Admin";
                string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                tsslblUserInfo.Text = $"User: {userName} - {dateTimeNow}";
            }
        }

        private void Admin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!HandleLogout())
                    e.Cancel = true;
            }
        }

        private bool HandleLogout()
        {
            return MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public void LoadUc(UserControl uc)
        {
            if (uc == null || pnlContent == null) return;

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
            pnlContent.ResumeLayout();
        }

        // ===== Drag form via pnlTop =====
        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            this.Location = Point.Add(dragFormPoint, new Size(dif));
        }

        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) dragging = false;
        }

        // ===== Animations =====
        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (menuContainer == null || menuTransition == null) return;

            if (!menuExpant)
            {
                menuContainer.Height += 10;
                if (menuContainer.Height >= 525)
                {
                    menuTransition.Stop();
                    menuExpant = true;
                }
            }
            else
            {
                menuContainer.Height -= 10;
                if (menuContainer.Height <= 58)
                {
                    menuTransition.Stop();
                    menuExpant = false;
                }
            }
        }

        private void btnHam_Click(object sender, EventArgs e)
        {
            sidebarTransition?.Start();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();
            menuTransition?.Start();
        }

        private void sidebarTransition_Tick(object sender, EventArgs e)
        {
            if (sidebar == null || sidebarTransition == null) return;

            if (sidebarExpant)
            {
                sidebar.Width -= 10;
                if (sidebar.Width <= 78)
                {
                    sidebarExpant = false;
                    sidebarTransition.Stop();
                }
            }
            else
            {
                sidebar.Width += 5; // mở chậm hơn đóng; nếu muốn cân, đổi thành 10
                if (sidebar.Width >= 301)
                {
                    sidebarExpant = true;
                    sidebarTransition.Stop();
                }
            }
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {
            sidebarTransition?.Start();
        }
    }
}
