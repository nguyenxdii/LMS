using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.GUI.Main;
using System;
using System.Windows.Forms;

namespace LMS.GUI.Auth
{
    public partial class frmLogin : Form
    {
        private readonly AuthService _auth = new AuthService();

        public frmLogin()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;

            this.AcceptButton = btnLogin;

            txtPassword.UseSystemPasswordChar = true;
            chkShowPassword.CheckedChanged += (s, e) =>
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            // optional: focus username box
            txtUsername.Focus();
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
                            MessageBox.Show("Sai tài khoản hoặc mật khẩu.", "Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtUsername.Focus();
                            txtUsername.SelectAll();
                            return;

                        case LoginFailReason.WrongPassword:
                            MessageBox.Show("Sai tài khoản hoặc mật khẩu.", "Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtPassword.Focus();
                            txtPassword.SelectAll();
                            return;

                        case LoginFailReason.Locked:
                            MessageBox.Show("Tài khoản đã bị khóa. Vui lòng liên hệ quản trị.", "Đăng nhập",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;

                        default:
                            MessageBox.Show("Đăng nhập không thành công.", "Đăng nhập");
                            return;
                    }
                }

                // === Login OK: set session + mở form theo role ===
                var acc = res.Account;
                //AppSession.CurrentAccount = acc;
                //AppSession.Role = acc.Role;
                //AppSession.DisplayName = acc.Customer?.Name ?? acc.Driver?.FullName ?? acc.Username;

                AppSession.SignIn(acc);

                Form next;
                switch (acc.Role)
                {
                    case UserRole.Admin: next = new frmMain_Admin(); break;
                    case UserRole.Customer: next = new frmMain_Customer(); break;
                    default: next = new frmMain_Driver(); break;
                }

                this.Hide();
                next.ShowDialog();     // đóng main thì quay lại login
                this.Show();
                txtPassword.Clear();
                txtUsername.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            using (var f = new frmRegister())
            {
                f.ShowDialog();
            }
            this.Show();
            txtPassword.Clear();
            txtUsername.Focus();
        }
    }
}
