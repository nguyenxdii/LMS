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
using LMS.BUS.Services;

namespace LMS.GUI.Main
{
    public partial class frmMain_Customer : Form
    {
        private readonly OrderService_Customer _orderSvc = new OrderService_Customer();
        bool menuExpant = false;
        bool sidebarExpant = true;
        private bool dragging = false;
        private Point dragCursorPoint; // Vị trí con trỏ chuột khi bắt đầu kéo
        private Point dragFormPoint;   // Vị trí form khi bắt đầu kéo

        private List<Button> navigationButtons;
        private Color normalColor = Color.FromArgb(32, 33, 36);
        private Color selectedColor = Color.FromArgb(0, 4, 53);

        private int _customerId;

        public frmMain_Customer(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;

            foreach (Control control in sidebar.Controls)
            {
                control.Width = sidebar.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            }

            InitializeNavigationButtons();
            this.Load += frmMain_Customer_Load;
            btnMenu.Click += btnMenu_Click;

            LoadUc(new LMS.GUI.Main.ucDashboard_Cus());
            lblPageTitle.Text = "Trang Chủ";
        }

        // Constructor phụ (dùng AppSession)
        public frmMain_Customer() : this(0) { }

        private void frmMain_Customer_Load(object sender, EventArgs e)
        {
            // Lấy id từ AppSession nếu không được truyền vào
            if (_customerId <= 0)
                _customerId = AppSession.CustomerId ?? 0;

            //if (_customerId <= 0)
            //{
            //    MessageBox.Show("Bạn chưa đăng nhập bằng tài khoản Customer.", "LMS",
            //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    Close();
            //    return;
            //}

            // Trong hàm khởi tạo frmMain_Customer() hoặc sự kiện Load
            pnlTopBar.MouseDown += PnlTopBar_MouseDown;
            pnlTopBar.MouseMove += PnlTopBar_MouseMove;
            pnlTopBar.MouseUp += PnlTopBar_MouseUp;

            tsllblWelcome.Text = $"Xin chào, {AppSession.CustomerId}";

        }

        private void InitializeNavigationButtons()
        {
            // Thêm các nút điều hướng của Customer
            navigationButtons = new List<Button>
            {
                btnNewOrder,
                btnOrderList,
                //btnViewTracking
            };

            foreach (Button btn in navigationButtons)
            {
                btn.Click += NavigationButton_Click;
            }

            btnHome.Click += btnHome_Click;
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

            if (clickedButton == btnNewOrder)
            {
                LoadUc(new LMS.GUI.OrderCustomer.ucOrderCreate_Cus(_customerId));
                lblPageTitle.Text = "Công Cụ / Tạo Đơn Hàng";
            }
            else if (clickedButton == btnOrderList)
            {
                LoadUc(new LMS.GUI.OrderCustomer.ucOrderList_Cus(_customerId));
                lblPageTitle.Text = "Công Cụ / Danh Sách Đơn Hàng";
            }
            else if (clickedButton == btnViewTracking)
            {
                //LoadUc(new LMS.GUI.OrderCustomer.ucTracking_Cus(_customerId));
                lblPageTitle.Text = "Công Cụ / Theo Dõi Vận Đơn";
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

        // Hàm này được gọi khi bạn nhấn chuột xuống trên Panel
        private void PnlTopBar_MouseDown(object sender, MouseEventArgs e)
        {
            // Chỉ bắt đầu kéo khi nhấn chuột trái
            if (e.Button == MouseButtons.Left)
            {
                dragging = true; // Bắt đầu trạng thái kéo
                dragCursorPoint = Cursor.Position; // Lấy vị trí con trỏ chuột (tọa độ màn hình)
                dragFormPoint = this.Location;     // Lấy vị trí hiện tại của Form (góc trên bên trái)
            }
        }

        // Hàm này được gọi khi bạn di chuyển chuột *trong khi đang nhấn giữ*
        private void PnlTopBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging) // Chỉ thực hiện khi đang kéo
            {
                // Tính toán khoảng cách con trỏ chuột đã di chuyển
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));

                // Cập nhật vị trí mới của Form bằng cách cộng khoảng cách di chuyển vào vị trí ban đầu
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        // Hàm này được gọi khi bạn nhả chuột ra
        private void PnlTopBar_MouseUp(object sender, MouseEventArgs e)
        {
            // Chỉ dừng kéo nếu đang nhấn chuột trái
            if (e.Button == MouseButtons.Left)
            {
                dragging = false; // Kết thúc trạng thái kéo
            }
        }

        // =============== (Giữ nguyên toàn bộ code animation) ===============

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

            LoadUc(new LMS.GUI.Main.ucDashboard_Cus());
            lblPageTitle.Text = "Trang Chủ";
        }
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}