using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.GUI.ProfileCustomer
{
    public partial class ucCustomerProfile : UserControl
    {
        public ucCustomerProfile()
        {
            InitializeComponent();
        }

        private void MakeFlat(Guna.UI2.WinForms.Guna2TextBox txt)
        {
            txt.BorderThickness = 0;
            txt.BorderColor = Color.Transparent;
            txt.FillColor = this.BackColor;
            txt.FocusedState.BorderColor = Color.Transparent;
            txt.HoverState.BorderColor = Color.Transparent;
            txt.DisabledState.BorderColor = Color.Transparent;
            txt.DisabledState.FillColor = this.BackColor;
            txt.FocusedState.FillColor = this.BackColor;
            txt.HoverState.FillColor = this.BackColor;
            txt.BackColor = this.BackColor;
            txt.Cursor = Cursors.IBeam;
        }

        private void ucCustomerProfile_Load(object sender, EventArgs e)
        {
            //MakeFlat(txtTest);
        }
    }

}
