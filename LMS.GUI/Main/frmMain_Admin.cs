using LMS.BUS.Helpers;
using LMS.GUI.CustomerAdmin; // Ví dụ: Cần using này để LoadUc
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing; // <-- Cần using này
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.GUI.Main
{
    public partial class frmMain_Admin : Form
    {
        bool menuExpant = false;

        private List<Button> navigationButtons;
        private Color normalColor = Color.FromArgb(32, 33, 36);
        private Color selectedColor = Color.FromArgb(0, 4, 53);

        // --- (1) THÊM CÁC BIẾN ĐỂ KÉO THẢ ---
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        // --- KẾT THÚC THÊM BIẾN ---

        public frmMain_Admin()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            // *** (2) THÊM DÒNG NÀY ĐỂ BỎ VIỀN FORM ***
            this.FormBorderStyle = FormBorderStyle.None;

            foreach (Control control in sidebar.Controls)
            {
                control.Width = sidebar.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            }

            InitializeNavigationButtons();
            btnMenu.Click += btnMenu_Click;
            this.Load += FrmMain_Admin_Load;
            this.FormClosing += new FormClosingEventHandler(this.Admin_FormClosing);
            lblTitle.Click += lblTitle_Click;
            btnHam.Click += btnHam_Click;

            // --- (3) GÁN SỰ KIỆN KÉO THẢ CHO PANEL TOP ---
            // !!! QUAN TRỌNG: Đảm bảo Panel trên cùng của bạn tên là 'pnlTop' !!!
            if (this.pnlTop != null) // Kiểm tra panel tồn tại
            {
                this.pnlTop.MouseDown += PnlTop_MouseDown;
                this.pnlTop.MouseMove += PnlTop_MouseMove;
                this.pnlTop.MouseUp += PnlTop_MouseUp;
            }
            // --- KẾT THÚC GÁN SỰ KIỆN ---

            LoadUc(new LMS.GUI.Main.ucDashboard_Ad());
            lblPageTitle.Text = "Trang Chủ";
        }

        private void FrmMain_Admin_Load(object sender, EventArgs e)
        {
            // Kiểm tra xem đã đăng nhập chưa và đúng vai trò Admin chưa
            if (!AppSession.IsLoggedIn || AppSession.Role != DAL.Models.UserRole.Admin)
            {
                MessageBox.Show("Bạn chưa đăng nhập bằng tài khoản Quản trị viên.", "Lỗi Truy Cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            if (stsWelcome != null)
            {
                stsWelcome.Text = $"Xin chào, {AppSession.DisplayName}";
            }
        }

        private void InitializeNavigationButtons()
        {
            navigationButtons = new List<Button>
            {
                //btnMenu,
                btnOrder,
                //btnAccount,
                btnCustomer, // Đảm bảo nút này tồn tại trong Designer
                btnDriver,
                btnShipment,
            };

            foreach (Button btn in navigationButtons)
            {
                btn.Click += NavigationButton_Click;
            }
            // Gán thêm sự kiện cho nút Home, Logout nếu cần
            btnHome.Click += btnHome_Click;
            btnLogOut.Click += btnLogOut_Click;
            btnAccount.Click += btnAccount_Click;
            btnHam.Click += btnHam_Click; // Giữ lại các sự kiện gốc
            lblTitle.Click += lblTitle_Click; // Giữ lại các sự kiện gốc
        }

        private void ResetButtonStyles()
        {
            foreach (Button btn in navigationButtons)
            {
                btn.BackColor = normalColor;
            }
        }

        private void NavigationButton_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                clickedButton.BackColor = selectedColor;
            }

            if (clickedButton == btnOrder)
            {
                LoadUc(new LMS.GUI.OrderAdmin.ucOrder_Admin());
                lblPageTitle.Text = "Quản Lý / Đơn Hàng";
            }
            //else if (clickedButton == btnAccount)
            //{
            //    LoadUc(new LMS.GUI.AccountAdmin.ucAccount_Admin());
            //    lblPageTitle.Text = "Quản Lý / Tài Khoản";
            //}
            else if (clickedButton == btnCustomer)
            {
                LoadUc(new LMS.GUI.CustomerAdmin.ucCustomer_Admin()); // Load UC Customer
                lblPageTitle.Text = "Quản Lý / Khách Hàng";
            }
            else if (clickedButton == btnDriver)
            {
                LoadUc(new LMS.GUI.DriverAdmin.ucDriver_Admin());
                lblPageTitle.Text = "Quản lý / Tài Xế";
            }
            else if (clickedButton == btnShipment)
            {
                LoadUc(new LMS.GUI.ShipmentAdmin.ucShipment_Admin()); // Load UC Shipment
                lblPageTitle.Text = "Quản Lý / Chuyến Hàng";
            }
        }
        // Thêm lại các hàm sự kiện Home, Logout,... nếu bạn có
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
            if (HandleLogout())
            {
                this.Close();
            }
        }

        private void Admin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Chỉ kiểm tra xác nhận nếu form đang đóng do người dùng nhấn nút Close
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!HandleLogout())
                {
                    e.Cancel = true; // Hủy đóng form nếu người dùng chọn "No"
                }
            }
        }

        // Hàm chung xử lý logic đăng xuất
        private bool HandleLogout()
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                return true; // Cho phép đóng form
            }
            return false; // Hủy đóng form
        }


        public void LoadUc(UserControl uc)
        {
            pnlContent.Controls.Clear();   // xóa UC cũ
            uc.Dock = DockStyle.Fill;       // cho UC mới chiếm toàn panel
            pnlContent.Controls.Add(uc);    // thêm UC mới vào
        }

        // --- (4) THÊM 3 HÀM XỬ LÝ KÉO THẢ ---
        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            }
        }

        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = false;
            }
        }
        // --- KẾT THÚC THÊM HÀM ---


        // --- (Các hàm animation giữ nguyên code gốc của bạn) ---
        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (menuExpant == false)
            {
                menuContainer.Height += 10;
                // Giữ nguyên giá trị Height gốc của bạn
                if (menuContainer.Height >= 524)
                {
                    menuTransition.Stop();
                    menuExpant = true;
                }
            }
            else
            {
                menuContainer.Height -= 10;
                // Giữ nguyên giá trị Height gốc của bạn
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
            ResetButtonStyles();
            menuTransition.Start();
        }

        bool sidebarExpant = true;

        private void sidebarTransition_Tick(object sender, EventArgs e)
        {
            if (sidebarExpant)
            {
                sidebar.Width -= 10;
                // Giữ nguyên giá trị Width gốc của bạn
                if (sidebar.Width <= 78)
                {
                    sidebarExpant = false;
                    sidebarTransition.Stop();
                }
            }
            else
            {
                sidebar.Width += 5;
                // Giữ nguyên giá trị Width gốc của bạn
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
    }
}