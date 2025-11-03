using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.GUI.OrderCustomer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LMS.GUI.Main
{
    public partial class frmMain_Customer : Form
    {
        private readonly OrderService_Customer _orderSvc = new OrderService_Customer();

        // ===== STATE =====
        bool menuExpant = false;
        bool sidebarExpant = true;

        // Drag form
        private bool dragging = false;
        private Point dragCursorPoint; // vị trí chuột lúc bắt đầu kéo
        private Point dragFormPoint;   // vị trí form lúc bắt đầu kéo

        private List<Button> navigationButtons;
        private readonly Color normalColor = Color.FromArgb(32, 33, 36);
        private readonly Color selectedColor = Color.FromArgb(0, 4, 53);

        private readonly int _customerId;

        public frmMain_Customer(int customerId)
        {
            InitializeComponent();

            _customerId = customerId;

            // Form base
            this.StartPosition = FormStartPosition.CenterScreen; // CenterParent không có owner sẽ không ăn
            this.FormBorderStyle = FormBorderStyle.None;
            //this.MinimumSize = new Size(1000, 650);
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

            // Lifecycle
            this.Load += frmMain_Customer_Load;
            this.FormClosing += Customer_FormClosing;

            // Gắn kéo form bằng pnlTopBar (nếu tồn tại)
            if (pnlTopBar != null)
            {
                pnlTopBar.MouseDown += PnlTopBar_MouseDown;
                pnlTopBar.MouseMove += PnlTopBar_MouseMove;
                pnlTopBar.MouseUp += PnlTopBar_MouseUp;
            }
        }

        // Constructor phụ (dùng AppSession)
        public frmMain_Customer() : this(0) { }

        private void frmMain_Customer_Load(object sender, EventArgs e)
        {
            // Check đăng nhập + vai trò khách hàng
            if (!AppSession.IsLoggedIn || AppSession.Role != DAL.Models.UserRole.Customer)
            {
                MessageBox.Show("Bạn chưa đăng nhập bằng tài khoản Khách Hàng.",
                    "Lỗi Truy Cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Hiển thị user info 1 lần (không ghi đè)
            var userName = AppSession.DisplayName ?? "Customer";
            if (tsslblUserInfo != null)
                tsslblUserInfo.Text = $"User: {userName} - {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

            LoadDashboard();
            lblPageTitle.Text = "Trang Chủ";
        }

        private void InitializeNavigationButtons()
        {
            // Thêm các nút điều hướng của Customer (nếu nút tồn tại)
            navigationButtons = new List<Button>();
            if (btnNewOrder != null) navigationButtons.Add(btnNewOrder);
            if (btnOrderList != null) navigationButtons.Add(btnOrderList);
            // if (btnViewTracking != null) navigationButtons.Add(btnViewTracking);

            foreach (Button btn in navigationButtons)
            {
                btn.Click += NavigationButton_Click;
            }

            // Gắn các sự kiện chung TẠI MỘT NƠI — tránh trùng
            if (btnHome != null) btnHome.Click += btnHome_Click;
            if (btnAccount != null) btnAccount.Click += BtnAccount_Click;
            if (btnHam != null) btnHam.Click += btnHam_Click;
            if (btnMenu != null) btnMenu.Click += btnMenu_Click;
            if (btnLogOut != null) btnLogOut.Click += btnLogOut_Click;
            if (lblTitle != null) lblTitle.Click += lblTitle_Click;
        }

        // Lấy CustomerId an toàn từ phiên/constructor
        private int RequireCustomerId()
        {
            var id = AppSession.CustomerId ?? _customerId;
            if (id <= 0)
            {
                MessageBox.Show("Không xác định được khách hàng. Vui lòng đăng nhập lại.",
                                "Lỗi phiên đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw new InvalidOperationException("CustomerId is not available.");
            }
            return id;
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

            if (sender == btnNewOrder)
            {
                LoadUc(new LMS.GUI.OrderCustomer.ucOrderCreate_Cus(RequireCustomerId()));
                lblPageTitle.Text = "Công Cụ / Tạo Đơn Hàng";
            }
            else if (sender == btnOrderList)
            {
                // Sửa chỗ này để không bao giờ truyền customerId = 0
                LoadUc(new LMS.GUI.OrderCustomer.ucOrderList_Cus(RequireCustomerId()));
                lblPageTitle.Text = "Công Cụ / Danh Sách Đơn Hàng";
            }
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            if (AppSession.IsLoggedIn && tsslblUserInfo != null)
            {
                string userName = AppSession.DisplayName ?? "Customer";
                string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                tsslblUserInfo.Text = $"User: {userName} - {dateTimeNow}";
            }
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

        private void LoadDashboard()
        {
            var dash = new LMS.GUI.Main.ucDashboard_Cus();

            // Nghe sự kiện từ UC con
            dash.CreateOrderClick += (s, e) =>
            {
                LoadUc(new ucOrderCreate_Cus(RequireCustomerId()));
                lblPageTitle.Text = "Công Cụ / Tạo Đơn Hàng";
            };

            dash.OrderDoubleClick += (s, orderId) =>
            {
                LoadUc(new ucTracking_Cus(RequireCustomerId(), orderId));
                lblPageTitle.Text = "Công Cụ / Theo Dõi Vận Đơn";
            };

            LoadUc(dash);
            lblPageTitle.Text = "Trang Chủ";
        }

        private void Customer_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Chỉ confirm khi user thật sự chủ động đóng
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!HandleLogout())
                {
                    e.Cancel = true;
                }
            }
        }

        private bool HandleLogout()
        {
            return MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        // ====== BUTTONS / MENU ======
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            // Chỉ Close(); confirm sẽ nằm ở FormClosing -> tránh hỏi 2 lần
            this.Close();
        }

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
            LoadDashboard();
            lblPageTitle.Text = "Trang Chủ";
        }

        private void BtnAccount_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();
            LoadUc(new LMS.GUI.ProfileCustomer.ucCustomerProfile());
            lblPageTitle.Text = "Tài Khoản Khách Hàng";
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {
            sidebarTransition?.Start();
        }

        // ====== DRAG FORM ======
        private void PnlTopBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void PnlTopBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            this.Location = Point.Add(dragFormPoint, new Size(dif));
        }

        private void PnlTopBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) dragging = false;
        }

        // ====== ANIMATIONS ======
        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (menuContainer == null || menuTransition == null) return;

            if (!menuExpant)
            {
                menuContainer.Height += 10;
                if (menuContainer.Height >= 187)
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
                sidebar.Width += 5;
                if (sidebar.Width >= 301)
                {
                    sidebarExpant = true;
                    sidebarTransition.Stop();
                }
            }
        }
    }
}
