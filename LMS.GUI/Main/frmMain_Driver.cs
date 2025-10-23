using LMS.BUS.Helpers;
using LMS.GUI.OrderDriver;
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
    public partial class frmMain_Driver : Form
    {
        public frmMain_Driver()
        {
            InitializeComponent();

            btnMyShipments.Click += btnMyShipments_Click;

        }

        public void ShowUc(UserControl uc)
        {
            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
        }

        private void btnMyShipments_Click(object sender, EventArgs e)
        {
            //ShowUc(new ucMyShipments_Drv());
            if (!AppSession.DriverId.HasValue)
            {
                MessageBox.Show("Bạn chưa đăng nhập với tài khoản Driver.", "Thông báo");
                return;
            }

            ShowUc(new ucMyShipments_Drv(AppSession.DriverId.Value));
        }
        // Để các UC khác gọi lại (Back → danh sách)
        public void OpenMyShipments()
        {
            btnMyShipments_Click(null, EventArgs.Empty);
        }
    }
}
