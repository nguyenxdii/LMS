using System;
using System.Security.Principal;
using System.Windows.Forms;

namespace LMS.GUI.Main
{
    public partial class frmMain_Admin : Form
    {
        public frmMain_Admin()
        {
            InitializeComponent();

            btnOrder.Click += BtnOrder_Click;
            btnAccount.Click += btnAccount_Click;
        }

        private void BtnOrder_Click(object sender, EventArgs e)
        {
            LoadUc(new LMS.GUI.OrderAdmin.ucOrder_Admin());
        }

        // === Hàm này dùng để load UC vào pnlContent ===
        public void LoadUc(UserControl uc)
        {
            pnlContent.Controls.Clear();     // xóa UC cũ
            uc.Dock = DockStyle.Fill;        // cho UC mới chiếm toàn panel
            pnlContent.Controls.Add(uc);     // thêm UC mới vào
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            LoadUc(new LMS.GUI.AccountAdmin.ucAccount_Admin());
        }
    }
}
