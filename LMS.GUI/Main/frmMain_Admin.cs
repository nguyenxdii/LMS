using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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


        public frmMain_Admin()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            foreach (Control control in sidebar.Controls)
            {
                control.Width = sidebar.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            }

            InitializeNavigationButtons();
            btnMenu.Click += btnMenu_Click;

            LoadUc(new LMS.GUI.Main.ucDashboard_Ad());
            lblPageTitle.Text = "Trang Chủ";
        }

        private void InitializeNavigationButtons()
        {
            navigationButtons = new List<Button>
            {
                btnOrder,
                btnAccount
            };

            foreach (Button btn in navigationButtons)
            {
                btn.Click += NavigationButton_Click;
            }
        }

        // hàm reset trạng thái của tất cả các nút
        private void ResetButtonStyles()
        {
            foreach (Button btn in navigationButtons)
            {
                btn.BackColor = normalColor;
            }
        }

        // hàm xử lý sự kiện click chung
        private void NavigationButton_Click(object sender, EventArgs e)
        {
            // reset tất cả các nút về trạng thái bình thường
            ResetButtonStyles();

            // đặt nút vừa bấm sang trạng thái "selected"
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                clickedButton.BackColor = selectedColor;
            }

            if (clickedButton == btnOrder)
            {
                LoadUc(new LMS.GUI.OrderAdmin.ucOrder_Admin());
            }
            else if (clickedButton == btnAccount)
            {
                LoadUc(new LMS.GUI.AccountAdmin.ucAccount_Admin());
            }
        }

        public void LoadUc(UserControl uc)
        {
            pnlContent.Controls.Clear();   // xóa UC cũ
            uc.Dock = DockStyle.Fill;       // cho UC mới chiếm toàn panel
            pnlContent.Controls.Add(uc);    // thêm UC mới vào
        }

        private void Event()
        {

        }

        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (menuExpant == false)
            {
                menuContainer.Height += 10;
                if (menuContainer.Height >= 524)
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

        bool sidebarExpant = true;

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