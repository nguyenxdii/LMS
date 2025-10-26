// LMS.GUI/Main/frmMain_Driver.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LMS.BUS.Helpers;
using LMS.DAL.Models;
using LMS.GUI.OrderDriver;

namespace LMS.GUI.Main
{
    public partial class frmMain_Driver : Form
    {
        bool menuExpant = false;
        bool sidebarExpant = true;

        // KHAI BÁO CÁC BIẾN KÉO THẢ MỚI
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private List<Button> navigationButtons;
        private Color normalColor = Color.FromArgb(32, 33, 36);
        private Color selectedColor = Color.FromArgb(0, 4, 53);

        private int _driverId;

        public frmMain_Driver(int driverId)
        {
            InitializeComponent();
            _driverId = driverId;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MinimumSize = new Size(1000, 650);

            // GÁN CÁC SỰ KIỆN KÉO THẢ CHO PANEL TIÊU ĐỀ
            // Giả sử tên Panel là pnlTopBar (hoặc pnlHeader)
            pnlTop.MouseDown += PnlTopBar_MouseDown;
            pnlTop.MouseMove += PnlTopBar_MouseMove;
            pnlTop.MouseUp += PnlTopBar_MouseUp;


            foreach (Control control in sidebar.Controls)
            {
                control.Width = sidebar.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            }

            InitializeNavigationButtons();
            this.Load += FrmMain_Driver_Load;
            btnMenu.Click += btnMenu_Click;

            LoadUc(new LMS.GUI.Main.ucDashboard_Drv());
            lblPageTitle.Text = "Trang Chủ";
        }

        // Constructor phụ (dùng AppSession)
        public frmMain_Driver() : this(0) { }

        private void FrmMain_Driver_Load(object sender, EventArgs e)
        {
            if (_driverId <= 0)
                _driverId = AppSession.DriverId ?? 0;

            if (_driverId <= 0 || AppSession.Role != UserRole.Driver)
            {
                MessageBox.Show("Bạn chưa đăng nhập bằng tài khoản Tài xế.", "LMS",
                     MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }
            // lblWelcome.Text = $"Xin chào, {AppSession.DisplayName}";
        }

        private void InitializeNavigationButtons()
        {
            navigationButtons = new List<Button>
            {
                btnMyShipments,
                btnShipmentDetail,
                //btnShipmentRun,
            };

            foreach (Button btn in navigationButtons)
            {
                btn.Click += NavigationButton_Click;
            }

            btnHome.Click += btnHome_Click;
            btnLogOut.Click += btnLogOut_Click;
            btnHam.Click += btnHam_Click;
            btnMenu.Click += btnMenu_Click;
            lblTitle.Click += lblTitle_Click;
        }

        private void ResetButtonStyles()
        {
            foreach (Button btn in navigationButtons)
            {
                btn.BackColor = normalColor;
            }
        }

        internal void NavigationButton_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();

            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                clickedButton.BackColor = selectedColor;
            }

            if (clickedButton == btnMyShipments)
            {
                LoadUc(new LMS.GUI.OrderDriver.ucMyShipments_Drv());
                lblPageTitle.Text = "Công Cụ / Đơn Hàng Của Tôi";
            }
            else if (clickedButton == btnShipmentDetail)
            {
                LoadUc(new LMS.GUI.OrderDriver.ucShipmentDetail_Drv());
                lblPageTitle.Text = "Công Cụ / Chi Tiết Chuyến Hàng";
            }
            else if (clickedButton == btnShipmentRun)
            {
                LoadUc(new LMS.GUI.OrderDriver.ucShipmentRun_Drv());
                lblPageTitle.Text = "Công Cụ / Thực Hiện Chuyến Hàng";
            }
        }

        public void LoadUc(UserControl uc)
        {
            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
            pnlContent.ResumeLayout();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ====== LOGIC KÉO THẢ FORM (3 HÀM) ======

        private void PnlTopBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            }
        }

        private void PnlTopBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void PnlTopBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = false;
            }
        }

        // ====== LOGIC ANIMATION (Giữ nguyên) ======

        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (menuExpant == false)
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

        private void btnHam_Click(object sender, EventArgs e)
        {
            sidebarTransition.Start();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            menuTransition.Start();
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();
            LoadUc(new LMS.GUI.Main.ucDashboard_Drv());
            lblPageTitle.Text = "Trang Chủ";
        }

        private void sidebarTransition_Tick(object sender, EventArgs e)
        {
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
                sidebar.Width += 5;
                if (sidebar.Width >= 301)
                {
                    sidebarExpant = true;
                    sidebarTransition.Stop();
                }
            }
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {
            sidebarTransition.Start();
        }

        // ===== HÀM ĐIỀU HƯỚNG TỪ UC CON =====
        public void NavigateToShipmentDetail()
        {
            if (btnShipmentDetail != null)
            {
                NavigationButton_Click(btnShipmentDetail, EventArgs.Empty);
            }
        }
    }
}