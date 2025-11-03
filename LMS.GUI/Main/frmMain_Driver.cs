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
        // trạng thái menu và sidebar
        private bool menuExpant = false;
        private bool sidebarExpant = true;

        // kéo thả form qua panel top
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        // các nút điều hướng
        private List<Button> navigationButtons;
        private readonly Color normalColor = Color.FromArgb(32, 33, 36);
        private readonly Color selectedColor = Color.FromArgb(0, 4, 53);

        private readonly int _driverId;

        public frmMain_Driver(int driverId)
        {
            InitializeComponent();

            _driverId = driverId;

            // thiết lập giao diện cơ bản
            this.StartPosition = FormStartPosition.CenterScreen; // CenterParent không có Owner sẽ không ăn
            this.FormBorderStyle = FormBorderStyle.None;
            this.MinimumSize = new Size(1000, 650);
            this.DoubleBuffered = true; // giảm nhấp nháy khi animate

            // kéo form bằng panel top (đảm bảo pnlTop tồn tại)
            if (pnlTop != null)
            {
                pnlTop.MouseDown += PnlTopBar_MouseDown;
                pnlTop.MouseMove += PnlTopBar_MouseMove;
                pnlTop.MouseUp += PnlTopBar_MouseUp;
            }

            // fit chiều rộng item trong sidebar (nếu cần)
            if (sidebar != null)
            {
                foreach (Control control in sidebar.Controls)
                    control.Width = sidebar.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            }

            InitializeNavigationButtons();

            // lifecycle
            this.Load += FrmMain_Driver_Load;
            this.FormClosing += Driver_FormClosing;
        }

        // constructor phụ (dùng AppSession)
        public frmMain_Driver() : this(0) { }

        private void FrmMain_Driver_Load(object sender, EventArgs e)
        {
            if (!AppSession.IsLoggedIn || AppSession.Role != UserRole.Driver)
            {
                MessageBox.Show("Bạn chưa đăng nhập bằng tài khoản Tài xế.", "Lỗi Truy Cập",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            tsslblUserInfo.Text = $"User: {AppSession.DisplayName} - {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

            LoadUc(new ucDashboard_Drv());
            lblPageTitle.Text = "Trang Chủ";
        }

        // khởi tạo các nút điều hướng
        private void InitializeNavigationButtons()
        {
            navigationButtons = new List<Button>();

            if (btnMyShipments != null) navigationButtons.Add(btnMyShipments);
            if (btnShipmentDetail != null) navigationButtons.Add(btnShipmentDetail);
            // if (btnShipmentRun != null) navigationButtons.Add(btnShipmentRun);

            foreach (var btn in navigationButtons)
                btn.Click += NavigationButton_Click;

            // gán sự kiện 1 nơi duy nhất, tránh trùng
            if (btnHome != null) btnHome.Click += btnHome_Click;
            if (btnLogOut != null) btnLogOut.Click += btnLogOut_Click;
            if (btnHam != null) btnHam.Click += btnHam_Click;
            if (btnMenu != null) btnMenu.Click += btnMenu_Click;
            if (lblTitle != null) lblTitle.Click += lblTitle_Click;
        }

        // reset style các nút
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
        }

        // cập nhật đồng hồ (tuỳ chọn)
        private void timerClock_Tick(object sender, EventArgs e)
        {
            if (AppSession.IsLoggedIn)
            {
                string userName = AppSession.DisplayName;
                string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                tsslblUserInfo.Text = $"User: {userName} - {dateTimeNow}";
            }
        }

        // load user control vào panel
        public void LoadUc(UserControl uc)
        {
            if (uc == null || pnlContent == null) return;

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
            pnlContent.ResumeLayout();
        }

        // xử lý đóng form
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

        // xử lý đăng xuất
        private bool HandleLogout()
        {
            return MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Close();
        }

        // xử lý animation sidebar và menu
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

        // xử lý kéo thả form
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

        // animation cho menu
        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (menuContainer == null || menuTransition == null) return;

            if (!menuExpant)
            {
                menuContainer.Height += 10;
                if (menuContainer.Height >= 175)
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

        // animation cho sidebar
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

        // điều hướng từ user control con
        public void NavigateToShipmentDetail()
        {
            if (btnShipmentDetail != null)
            {
                NavigationButton_Click(btnShipmentDetail, EventArgs.Empty);
            }
        }
    }
}
