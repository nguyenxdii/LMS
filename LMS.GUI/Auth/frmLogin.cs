using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.GUI.Main;
using System;
using System.Drawing; // Cần cho Point
using System.Windows.Forms;

namespace LMS.GUI.Auth
{
    public partial class frmLogin : Form
    {
        private readonly AuthService _auth = new AuthService();

        // BIẾN CHO KÉO THẢ FORM
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public frmLogin()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;

            // GÁN SỰ KIỆN KÉO THẢ CHO PANEL TIÊU ĐỀ
            // Giả định tên Panel là pnlTop
            if (pnlTop != null)
            {
                pnlTop.MouseDown += PnlTop_MouseDown;
                pnlTop.MouseMove += PnlTop_MouseMove;
                pnlTop.MouseUp += PnlTop_MouseUp;
            }

            txtUsername.Focus();
            btnLogin.Click += btnLogin_Click;
            this.AcceptButton = btnLogin;

            txtPassword.UseSystemPasswordChar = true;
            chkShowPassword.CheckedChanged += (s, e) =>
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
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
                            MessageBox.Show("Tài khoản hoặc mật khẩu sai.", "Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtUsername.Focus();
                            txtUsername.SelectAll();
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
                AppSession.SignIn(acc);

                Form next;
                switch (acc.Role)
                {
                    case UserRole.Admin: next = new frmMain_Admin(); break; // Giả định frmMain_Admin tồn tại
                    case UserRole.Customer: next = new frmMain_Customer(); break; // Giả định frmMain_Customer tồn tại
                    default: next = new frmMain_Driver(); break;
                }

                this.Hide();
                next.ShowDialog();
                this.Show();
                txtPassword.Clear();
                txtUsername.Focus();
                this.AcceptButton = btnLogin;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            using (var f = new frmRegister()) // Giả định frmRegister tồn tại
            {
                f.ShowDialog();
            }
            this.Show();
            txtPassword.Clear();
            txtUsername.Focus();
        }
    }
}