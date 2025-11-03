using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.GUI.Main;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LMS.GUI.Auth
{
    public partial class frmLogin : Form
    {
        private readonly AuthService _auth = new AuthService();

        // kéo thả form borderless
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public frmLogin()
        {
            InitializeComponent();

            // cấu hình form đăng nhập
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;

            // gán sự kiện kéo thả cho panel tiêu đề (pnlTop)
            if (pnlTop != null)
            {
                pnlTop.MouseDown += PnlTop_MouseDown;
                pnlTop.MouseMove += PnlTop_MouseMove;
                pnlTop.MouseUp += PnlTop_MouseUp;
            }

            // gán sự kiện và hành vi nhập liệu
            txtUsername.Focus();
            btnLogin.Click += btnLogin_Click;
            this.AcceptButton = btnLogin;

            // ẩn/hiện mật khẩu
            txtPassword.UseSystemPasswordChar = true;
            chkShowPassword.CheckedChanged += (s, e) =>
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            // đặt focus khi mở form
            txtUsername.Focus();
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var res = _auth.Login(txtUsername.Text.Trim(), txtPassword.Text);

                if (!res.Ok)
                {
                    switch (res.Reason)
                    {
                        case LoginFailReason.UserNotFound:
                        case LoginFailReason.WrongPassword:
                            MessageBox.Show("Tài khoản hoặc mật khẩu sai.", "Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtUsername.Focus();
                            txtUsername.SelectAll();
                            return;

                        case LoginFailReason.Locked:
                            MessageBox.Show("Tài khoản đã bị khóa. Vui lòng liên hệ quản trị.", "Đăng Nhập",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;

                        default:
                            MessageBox.Show("Đăng nhập không thành công.", "Đăng Nhập");
                            return;
                    }
                }

                // login ok: set session và mở form theo vai trò
                var acc = res.Account;
                AppSession.SignIn(acc);

                Form next;
                switch (acc.Role)
                {
                    case UserRole.Admin:
                        next = new frmMain_Admin();
                        break;
                    case UserRole.Customer:
                        next = new frmMain_Customer();
                        break;
                    default:
                        next = new frmMain_Driver();
                        break;
                }

                this.Hide();
                next.ShowDialog();
                this.Show();

                // reset input sau khi đóng form con
                txtPassword.Clear();
                txtUsername.Focus();
                this.AcceptButton = btnLogin;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // điều hướng sang form đăng ký
            this.Hide();
            using (var f = new frmRegister())
            {
                f.ShowDialog();
            }
            this.Show();

            // dọn dẹp input khi quay lại
            txtPassword.Clear();
            txtUsername.Focus();
        }
    }
}
