using LMS.BUS.Helpers;
using LMS.DAL.Models; // cần để dùng UserRole
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.Main
{
    public partial class frmMain_Admin : Form
    {
        bool menuExpant = false;
        bool sidebarExpant = true;

        private List<Button> navigationButtons;
        private readonly Color normalColor = Color.FromArgb(32, 33, 36);
        private readonly Color selectedColor = Color.FromArgb(0, 4, 53);

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public frmMain_Admin()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MinimumSize = new Size(1000, 650);
            this.DoubleBuffered = true;

            EnableDoubleBuffer(pnlContent);

            if (sidebar != null)
            {
                foreach (Control control in sidebar.Controls)
                    control.Width = sidebar.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            }

            InitializeNavigationButtons();

            this.Load += FrmMain_Admin_Load;
            this.FormClosing += Admin_FormClosing;

            if (pnlTop != null)
            {
                pnlTop.MouseDown += PnlTop_MouseDown;
                pnlTop.MouseMove += PnlTop_MouseMove;
                pnlTop.MouseUp += PnlTop_MouseUp;
            }
        }

        private void FrmMain_Admin_Load(object sender, EventArgs e)
        {
            if (!AppSession.IsLoggedIn || AppSession.Role != UserRole.Admin)
            {
                MessageBox.Show("Bạn chưa đăng nhập bằng tài khoản Quản trị viên.",
                    "Lỗi Truy Cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            var userName = AppSession.DisplayName ?? "Admin";
            if (tsslblUserInfo != null)
                tsslblUserInfo.Text = $"User: {userName} - {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

            SetTitle("Trang Chủ");
            LoadUc(new ucDashboard_Ad());
        }

        private void InitializeNavigationButtons()
        {
            navigationButtons = new List<Button>
            {
                btnOrder, btnCustomer, btnDriver, btnShipment,
                btnWarehouse, btnRouteTemplate, btnVehicle, btnReport
            }.Where(b => b != null).ToList();

            foreach (var btn in navigationButtons)
                btn.Click += NavigationButton_Click;

            if (btnHome != null) btnHome.Click += btnHome_Click;
            if (btnLogOut != null) btnLogOut.Click += btnLogOut_Click;
            if (btnAccount != null) btnAccount.Click += btnAccount_Click;
            if (btnHam != null) btnHam.Click += btnHam_Click;
            if (btnMenu != null) btnMenu.Click += btnMenu_Click;
            if (lblTitle != null) lblTitle.Click += lblTitle_Click;
        }

        private void ResetButtonStyles()
        {
            foreach (var btn in navigationButtons)
                btn.BackColor = normalColor;
        }

        private void NavigationButton_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();
            if (sender is Button clickedButton) clickedButton.BackColor = selectedColor;

            if (sender == btnOrder)
            {
                LoadUc(new OrderAdmin.ucOrder_Admin());
                SetTitle("Quản Lý / Đơn Hàng");
            }
            else if (sender == btnCustomer)
            {
                LoadUc(new CustomerAdmin.ucCustomer_Admin());
                SetTitle("Quản Lý / Khách Hàng");
            }
            else if (sender == btnDriver)
            {
                LoadUc(new DriverAdmin.ucDriver_Admin());
                SetTitle("Quản Lý / Tài Xế");
            }
            else if (sender == btnShipment)
            {
                LoadUc(new ShipmentAdmin.ucShipment_Admin());
                SetTitle("Quản Lý / Chuyến Hàng");
            }
            else if (sender == btnWarehouse)
            {
                LoadUc(new WarehouseAdmin.ucWarehouse_Admin());
                SetTitle("Quản Lý / Kho");
            }
            else if (sender == btnRouteTemplate)
            {
                LoadUc(new RouteTemplateAdmin.ucRouteTemplate_Admin());
                SetTitle("Quản Lý / Tuyến Đường Mẫu");
            }
            else if (sender == btnVehicle)
            {
                LoadUc(new VehicleAdmin.ucVehicle_Admin());
                SetTitle("Quản Lý / Phương Tiện");
            }
            else if (sender == btnReport)
            {
                LoadUc(new ReportAdmin.ucStatistics());
                SetTitle("Quản Lý / Báo Cáo");
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();
            LoadUc(new ucDashboard_Ad());
            SetTitle("Trang Chủ");
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();
            LoadUc(new AccountAdmin.ucAccount_Admin());
            SetTitle("Quản Lý / Tài Khoản");
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Close(); // xác nhận sẽ chạy trong FormClosing
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            if (!AppSession.IsLoggedIn || tsslblUserInfo == null) return;
            var userName = AppSession.DisplayName ?? "Admin";
            tsslblUserInfo.Text = $"User: {userName} - {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
        }

        private void Admin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !HandleLogout())
                e.Cancel = true;
        }

        private bool HandleLogout()
        {
            return MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public void LoadUc(UserControl uc)
        {
            if (uc == null || pnlContent == null) return;

            var old = pnlContent.Controls.OfType<UserControl>().FirstOrDefault();

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
            pnlContent.ResumeLayout();

            old?.Dispose(); // tránh rò rỉ handle/ram
        }

        public void NavigateToOrderAdmin(string orderNoToSelect = null)
        {
            var uc = new OrderAdmin.ucOrder_Admin();
            LoadUc(uc);
            SetTitle("Quản Lý / Đơn Hàng");

            if (!string.IsNullOrWhiteSpace(orderNoToSelect))
                BeginInvoke(new Action(() => uc.SelectOrderByNo(orderNoToSelect)));
        }

        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = Location;
        }

        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            var dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            Location = Point.Add(dragFormPoint, new Size(dif));
        }

        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) dragging = false;
        }

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
            sidebarTransition?.Start();
        }

        private void SetTitle(string text)
        {
            if (lblPageTitle != null) lblPageTitle.Text = text;
            else if (lblTitle != null) lblTitle.Text = text;
        }

        private static void EnableDoubleBuffer(Control c)
        {
            if (c == null) return;
            var prop = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            prop?.SetValue(c, true, null);
        }
    }
}
