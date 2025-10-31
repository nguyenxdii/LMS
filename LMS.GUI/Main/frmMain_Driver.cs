//// LMS.GUI/Main/frmMain_Driver.cs
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using LMS.BUS.Helpers;
//using LMS.DAL.Models;
//using LMS.GUI.OrderDriver;

//namespace LMS.GUI.Main
//{
//    public partial class frmMain_Driver : Form
//    {
//        bool menuExpant = false;
//        bool sidebarExpant = true;

//        // KHAI BÁO CÁC BIẾN KÉO THẢ MỚI
//        private bool dragging = false;
//        private Point dragCursorPoint;
//        private Point dragFormPoint;

//        private List<Button> navigationButtons;
//        private Color normalColor = Color.FromArgb(32, 33, 36);
//        private Color selectedColor = Color.FromArgb(0, 4, 53);

//        private int _driverId;

//        public frmMain_Driver(int driverId)
//        {
//            InitializeComponent();
//            _driverId = driverId;
//            this.StartPosition = FormStartPosition.CenterParent;
//            this.FormBorderStyle = FormBorderStyle.None;
//            this.MinimumSize = new Size(1000, 650);

//            // GÁN CÁC SỰ KIỆN KÉO THẢ CHO PANEL TIÊU ĐỀ
//            // Giả sử tên Panel là pnlTopBar (hoặc pnlHeader)
//            pnlTop.MouseDown += PnlTopBar_MouseDown;
//            pnlTop.MouseMove += PnlTopBar_MouseMove;
//            pnlTop.MouseUp += PnlTopBar_MouseUp;


//            foreach (Control control in sidebar.Controls)
//            {
//                control.Width = sidebar.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
//            }

//            InitializeNavigationButtons();
//            this.Load += FrmMain_Driver_Load;
//            btnMenu.Click += btnMenu_Click;
//            this.FormClosing += new FormClosingEventHandler(this.Driver_FormClosing);

//            LoadUc(new LMS.GUI.Main.ucDashboard_Drv());
//            lblPageTitle.Text = "Trang Chủ";
//        }

//        // Constructor phụ (dùng AppSession)
//        public frmMain_Driver() : this(0) { }

//        private void FrmMain_Driver_Load(object sender, EventArgs e)
//        {
//            if (!AppSession.IsLoggedIn || AppSession.Role != DAL.Models.UserRole.Driver)
//            {
//                MessageBox.Show("Bạn chưa đăng nhập bằng tài khoản Tài xế.", "Lỗi Truy Cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                this.Close();
//                return;
//            }

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
//                btnMyShipments,
//                btnShipmentDetail,
//                //btnShipmentRun,
//            };

//            foreach (Button btn in navigationButtons)
//            {
//                btn.Click += NavigationButton_Click;
//            }

//            btnHome.Click += btnHome_Click;
//            btnLogOut.Click += btnLogOut_Click;
//            btnHam.Click += btnHam_Click;
//            btnMenu.Click += btnMenu_Click;
//            lblTitle.Click += lblTitle_Click;
//        }

//        private void ResetButtonStyles()
//        {
//            foreach (Button btn in navigationButtons)
//            {
//                btn.BackColor = normalColor;
//            }
//        }

//        internal void NavigationButton_Click(object sender, EventArgs e)
//        {
//            ResetButtonStyles();

//            Button clickedButton = sender as Button;
//            if (clickedButton != null)
//            {
//                clickedButton.BackColor = selectedColor;
//            }

//            if (clickedButton == btnMyShipments)
//            {
//                LoadUc(new LMS.GUI.OrderDriver.ucMyShipments_Drv());
//                lblPageTitle.Text = "Công Cụ / Đơn Hàng Của Tôi";
//            }
//            else if (clickedButton == btnShipmentDetail)
//            {
//                LoadUc(new LMS.GUI.OrderDriver.ucShipmentDetail_Drv());
//                lblPageTitle.Text = "Công Cụ / Chi Tiết Chuyến Hàng";
//            }
//            else if (clickedButton == btnShipmentRun)
//            {
//                LoadUc(new LMS.GUI.OrderDriver.ucShipmentRun_Drv());
//                lblPageTitle.Text = "Công Cụ / Thực Hiện Chuyến Hàng";
//            }
//        }

//        private void timerClock_Tick(object sender, EventArgs e)
//        {
//            if (AppSession.IsLoggedIn)
//            {
//                string userName = AppSession.DisplayName;
//                string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
//                tsslblUserInfo.Text = $"User: {userName} - {dateTimeNow}";
//            }
//        }
//        public void LoadUc(UserControl uc)
//        {
//            pnlContent.SuspendLayout();
//            pnlContent.Controls.Clear();
//            uc.Dock = DockStyle.Fill;
//            pnlContent.Controls.Add(uc);
//            pnlContent.ResumeLayout();
//        }
//        private void Driver_FormClosing(object sender, FormClosingEventArgs e)
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

//        private bool HandleLogout()
//        {
//            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//            {
//                return true; // Cho phép đóng form
//            }
//            return false; // Hủy đóng form
//        }

//        private void btnLogOut_Click(object sender, EventArgs e)
//        {
//            this.Close();
//        }
//        private void btnHam_Click(object sender, EventArgs e)
//        {
//            sidebarTransition.Start();
//        }

//        private void btnMenu_Click(object sender, EventArgs e)
//        {
//            menuTransition.Start();
//        }
//        private void btnHome_Click(object sender, EventArgs e)
//        {
//            ResetButtonStyles();
//            LoadUc(new LMS.GUI.Main.ucDashboard_Drv());
//            lblPageTitle.Text = "Trang Chủ";
//        }
//        private void btnAccount_Click(object sender, EventArgs e)
//        {
//            ResetButtonStyles();
//            LoadUc(new LMS.GUI.ProfileUser.ucDriverProfile());
//            lblPageTitle.Text = "Thông Tin Tài Khoản";
//        }

//        private void PnlTopBar_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left)
//            {
//                dragging = true;
//                dragCursorPoint = Cursor.Position;
//                dragFormPoint = this.Location;
//            }
//        }

//        private void PnlTopBar_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (dragging)
//            {
//                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
//                this.Location = Point.Add(dragFormPoint, new Size(dif));
//            }
//        }

//        private void PnlTopBar_MouseUp(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left)
//            {
//                dragging = false;
//            }
//        }

//        // ====== LOGIC ANIMATION (Giữ nguyên) ======

//        private void menuTransition_Tick(object sender, EventArgs e)
//        {
//            if (menuExpant == false)
//            {
//                menuContainer.Height += 10;
//                if (menuContainer.Height >= 235)
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

//        // ===== HÀM ĐIỀU HƯỚNG TỪ UC CON =====
//        public void NavigateToShipmentDetail()
//        {
//            if (btnShipmentDetail != null)
//            {
//                NavigationButton_Click(btnShipmentDetail, EventArgs.Empty);
//            }
//        }
//    }
//}
// LMS.GUI/Main/frmMain_Driver.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LMS.BUS.Helpers;
using LMS.DAL.Models;
using LMS.GUI.OrderDriver;

namespace LMS.GUI.Main
{
    public partial class frmMain_Driver : Form
    {
        // ====== STATE ======
        private bool menuExpant = false;
        private bool sidebarExpant = true;

        // Kéo thả form qua panel top
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        // Nav buttons
        private List<Button> navigationButtons;
        private readonly Color normalColor = Color.FromArgb(32, 33, 36);
        private readonly Color selectedColor = Color.FromArgb(0, 4, 53);

        private readonly int _driverId;

        public frmMain_Driver(int driverId)
        {
            InitializeComponent();

            _driverId = driverId;

            // UI base
            this.StartPosition = FormStartPosition.CenterScreen; // CenterParent không có Owner sẽ không ăn
            this.FormBorderStyle = FormBorderStyle.None;
            this.MinimumSize = new Size(1000, 650);
            this.DoubleBuffered = true; // giảm nhấp nháy khi animate

            // Kéo form bằng panel top (đảm bảo pnlTop tồn tại)
            if (pnlTop != null)
            {
                pnlTop.MouseDown += PnlTopBar_MouseDown;
                pnlTop.MouseMove += PnlTopBar_MouseMove;
                pnlTop.MouseUp += PnlTopBar_MouseUp;
            }

            // Fit chiều rộng item trong sidebar (nếu cần)
            if (sidebar != null)
            {
                foreach (Control control in sidebar.Controls)
                    control.Width = sidebar.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            }

            InitializeNavigationButtons();

            // Lifecycle
            this.Load += FrmMain_Driver_Load;
            this.FormClosing += Driver_FormClosing;
        }

        // Constructor phụ (dùng AppSession)
        public frmMain_Driver() : this(0) { }

        // ====== LOAD ======
        private void FrmMain_Driver_Load(object sender, EventArgs e)
        {
            // Kiểm tra đăng nhập & vai trò Tài xế
            if (!AppSession.IsLoggedIn || AppSession.Role != UserRole.Driver)
            {
                MessageBox.Show("Bạn chưa đăng nhập bằng tài khoản Tài xế.", "Lỗi Truy Cập",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            // Hiển thị user info 1 lần (không ghi đè)
            tsslblUserInfo.Text = $"User: {AppSession.DisplayName} - {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            // Nếu muốn cập nhật từng giây, bật timer trong Designer hoặc:
            // timerClock.Enabled = true;

            // Nạp UC mặc định SAU khi xác thực
            LoadUc(new ucDashboard_Drv());
            lblPageTitle.Text = "Trang Chủ";
        }

        // ====== NAV ======
        private void InitializeNavigationButtons()
        {
            navigationButtons = new List<Button>();

            if (btnMyShipments != null) navigationButtons.Add(btnMyShipments);
            if (btnShipmentDetail != null) navigationButtons.Add(btnShipmentDetail);
            // if (btnShipmentRun != null) navigationButtons.Add(btnShipmentRun);

            foreach (var btn in navigationButtons)
                btn.Click += NavigationButton_Click;

            // Gán sự kiện 1 nơi duy nhất, tránh trùng
            if (btnHome != null) btnHome.Click += btnHome_Click;
            if (btnLogOut != null) btnLogOut.Click += btnLogOut_Click;
            if (btnHam != null) btnHam.Click += btnHam_Click;
            if (btnMenu != null) btnMenu.Click += btnMenu_Click;
            if (lblTitle != null) lblTitle.Click += lblTitle_Click;
        }

        private void ResetButtonStyles()
        {
            foreach (var btn in navigationButtons)
                btn.BackColor = normalColor;
        }

        internal void NavigationButton_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();

            if (sender is Button clickedButton)
                clickedButton.BackColor = selectedColor;

            if (sender == btnMyShipments)
            {
                LoadUc(new ucMyShipments_Drv());
                lblPageTitle.Text = "Công Cụ / Đơn Hàng Của Tôi";
            }
            else if (sender == btnShipmentDetail)
            {
                LoadUc(new ucShipmentDetail_Drv());
                lblPageTitle.Text = "Công Cụ / Chi Tiết Chuyến Hàng";
            }
            else if (sender == btnShipmentRun)
            {
                LoadUc(new ucShipmentRun_Drv());
                lblPageTitle.Text = "Công Cụ / Thực Hiện Chuyến Hàng";
            }
        }

        // ====== CLOCK (tuỳ chọn) ======
        private void timerClock_Tick(object sender, EventArgs e)
        {
            if (AppSession.IsLoggedIn)
            {
                string userName = AppSession.DisplayName;
                string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                tsslblUserInfo.Text = $"User: {userName} - {dateTimeNow}";
            }
        }

        // ====== LOAD UC ======
        public void LoadUc(UserControl uc)
        {
            if (uc == null || pnlContent == null) return;

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
            pnlContent.ResumeLayout();
        }

        // ====== ĐÓNG FORM / LOGOUT ======
        private void Driver_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!HandleLogout())
                {
                    e.Cancel = true; // hủy đóng nếu chọn No
                }
            }
        }

        private bool HandleLogout()
        {
            return MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ====== SIDEBAR / MENU ANIMATION ======
        private void btnHam_Click(object sender, EventArgs e)
        {
            sidebarTransition?.Start();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            menuTransition?.Start();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();
            LoadUc(new ucDashboard_Drv());
            lblPageTitle.Text = "Trang Chủ";
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();
            LoadUc(new LMS.GUI.ProfileUser.ucDriverProfile());
            lblPageTitle.Text = "Thông Tin Tài Khoản";
        }

        // ====== DRAG FORM ======
        private void PnlTopBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = Location;
        }

        private void PnlTopBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            Location = Point.Add(dragFormPoint, new Size(dif));
        }

        private void PnlTopBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) dragging = false;
        }

        // ====== TIMER TICK ANIMS ======
        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (menuContainer == null || menuTransition == null) return;

            if (!menuExpant)
            {
                menuContainer.Height += 10;
                if (menuContainer.Height >= 235)
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
                sidebar.Width += 5; // cố tình mở chậm hơn đóng; nếu muốn cân: đổi thành 10
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

        // ====== ĐIỀU HƯỚNG TỪ UC CON ======
        public void NavigateToShipmentDetail()
        {
            if (btnShipmentDetail != null)
            {
                NavigationButton_Click(btnShipmentDetail, EventArgs.Empty);
            }
        }
    }
}
