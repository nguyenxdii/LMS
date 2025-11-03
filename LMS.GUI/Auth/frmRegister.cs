using Guna.UI2.WinForms;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LMS.GUI.Auth
{
    public partial class frmRegister : Form
    {
        private readonly AuthService _auth = new AuthService();
        private ucRegister_Cus _ucCustomer;
        private ucRegister_Drv _ucDriver;
        private bool _suppressRoleChanged = false;

        // kéo thả form borderless
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public frmRegister()
        {
            InitializeComponent();

            // cấu hình form
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;

            // gán sự kiện kéo thả cho panel tiêu đề (pnlTop)
            if (pnlTop != null)
            {
                pnlTop.MouseDown += PnlTop_MouseDown;
                pnlTop.MouseMove += PnlTop_MouseMove;
                pnlTop.MouseUp += PnlTop_MouseUp;
            }

            // cấu hình combobox vai trò
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.DataSource = new[] { UserRole.Customer, UserRole.Driver };
            SetRoleIndex(null);
            cmbRole.SelectedIndexChanged += cmbRole_SelectedIndexChanged;

            // panel nội dung
            pnlContent.Controls.Clear();

            // khởi tạo user controls
            _ucCustomer = new ucRegister_Cus(_auth);
            _ucDriver = new ucRegister_Drv(_auth);
        }

        // logic kéo thả form (3 hàm)
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

        // đặt trạng thái chọn vai trò (null = chưa chọn)
        private void SetRoleIndex(int? idxOrNull)
        {
            try
            {
                _suppressRoleChanged = true;
                if (idxOrNull.HasValue)
                {
                    cmbRole.SelectedIndex = idxOrNull.Value;
                }
                else
                {
                    cmbRole.SelectedIndex = -1;
                    cmbRole.SelectedItem = null;
                    cmbRole.Text = "Chọn Vai Trò"; // text hiển thị cho người dùng phải viết Hoa
                }
            }
            finally
            {
                _suppressRoleChanged = false;
            }
        }

        // thay đổi vai trò -> nạp đúng usercontrol
        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressRoleChanged) return;

            pnlContent.Controls.Clear();

            if (cmbRole.SelectedItem is UserRole role)
            {
                if (role == UserRole.Customer)
                {
                    pnlContent.Controls.Add(_ucCustomer);
                    _ucCustomer.Dock = DockStyle.Fill;
                }
                else if (role == UserRole.Driver)
                {
                    pnlContent.Controls.Add(_ucDriver);
                    _ucDriver.Dock = DockStyle.Fill;
                }
            }
            else
            {
                this.AcceptButton = null;
            }
        }
    }
}
