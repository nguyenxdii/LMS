using Guna.UI2.WinForms;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.ComponentModel;
using System.Drawing; // Cần cho Point
using System.Linq;
using System.Windows.Forms;

namespace LMS.GUI.Auth
{
    public partial class frmRegister : Form
    {
        private readonly AuthService _auth = new AuthService();
        private ucRegister_Cus _ucCustomer;
        private ucRegister_Drv _ucDriver;
        private bool _suppressRoleChanged = false;

        // BIẾN CHO KÉO THẢ FORM
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;


        public frmRegister()
        {
            InitializeComponent();

            // Cấu hình form
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None; // Thường đi kèm với kéo thả

            // GÁN SỰ KIỆN KÉO THẢ CHO PANEL TIÊU ĐỀ
            // Giả định tên Panel là pnlTop
            if (pnlTop != null)
            {
                pnlTop.MouseDown += PnlTop_MouseDown;
                pnlTop.MouseMove += PnlTop_MouseMove;
                pnlTop.MouseUp += PnlTop_MouseUp;
            }

            // Cấu hình cmbRole
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.DataSource = new[] { UserRole.Customer, UserRole.Driver };
            SetRoleIndex(null); // Mặc định không chọn vai trò
            cmbRole.SelectedIndexChanged += cmbRole_SelectedIndexChanged;

            // Cấu hình pnlContent
            pnlContent.Controls.Clear(); // Đảm bảo pnlContent trống ban đầu

            // Khởi tạo user controls
            _ucCustomer = new ucRegister_Cus(_auth); // Giả định ucRegister_Cus tồn tại
            _ucDriver = new ucRegister_Drv(_auth); // Giả định ucRegister_Drv tồn tại
        }

        // ====== LOGIC KÉO THẢ FORM (3 HÀM) ======

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

        private void SetRoleIndex(int? idxOrNull)
        {
            try
            {
                _suppressRoleChanged = true;
                if (idxOrNull.HasValue)
                    cmbRole.SelectedIndex = idxOrNull.Value;
                else
                {
                    cmbRole.SelectedIndex = -1;
                    cmbRole.SelectedItem = null;
                    cmbRole.Text = "Chọn vai trò"; // Placeholder khi chưa chọn
                }
            }
            finally { _suppressRoleChanged = false; }
        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressRoleChanged) return;

            pnlContent.Controls.Clear(); // Xóa nội dung cũ trong pnlContent
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
                this.AcceptButton = null; // Không có vai trò được chọn
            }
        }
    }
}