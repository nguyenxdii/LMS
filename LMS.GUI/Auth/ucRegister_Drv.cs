using Guna.UI2.WinForms;
using LMS.BUS.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LMS.GUI.Auth
{
    public partial class ucRegister_Drv : UserControl
    {
        private readonly AuthService _auth;
        private readonly Guna2HtmlToolTip htip = new Guna2HtmlToolTip();

        //public Button RegisterButton => btnRegisterD;

        public ucRegister_Drv(AuthService auth)
        {
            InitializeComponent();
            _auth = auth;

            // Cấu hình Guna2 HTML Tooltip
            ConfigureTooltip();
            RegisterFieldTips();

            // Cấu hình hiển thị mật khẩu
            chkShowPassD1.Checked = chkShowPassD2.Checked = false;
            InitPasswordMask(txtPasswordD);
            InitPasswordMask(txtConfirmD);
            chkShowPassD1.CheckedChanged += (s, e) => TogglePasswordMask(txtPasswordD, chkShowPassD1.Checked);
            chkShowPassD2.CheckedChanged += (s, e) => TogglePasswordMask(txtConfirmD, chkShowPassD2.Checked);

            // Cấu hình ComboBox GPLX
            cmbLicenseType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLicenseType.Items.AddRange(new object[] { "B2", "C", "CE", "D", "DE", "FC" });
            cmbLicenseType.SelectedIndex = -1;

            // Gán sự kiện nút
            btnRegisterD.Click += btnRegisterD_Click;
            btnExitD.Click += btnExitD_Click;
        }

        private void ConfigureTooltip()
        {
            htip.TitleFont = new Font("Segoe UI", 9, FontStyle.Bold);
            htip.TitleForeColor = Color.FromArgb(31, 113, 185);
            htip.ForeColor = Color.Black;
            htip.BackColor = Color.White;
            htip.BorderColor = Color.FromArgb(210, 210, 210);
            htip.MaximumSize = new Size(300, 0);
            htip.InitialDelay = 100;
            htip.AutoPopDelay = 30000;
            htip.ReshowDelay = 80;
            htip.ShowAlways = true;
            htip.UseAnimation = false;
            htip.UseFading = false;
        }

        private void RegisterFieldTips()
        {
            AttachHtmlTip(txtFullnameD, "Họ tên <b>tài xế</b>.");
            AttachHtmlTip(txtUsernameD, "Tên đăng nhập: 8–16 ký tự (chữ/số/_).");
            AttachHtmlTip(txtPasswordD, "<b>Mật khẩu</b> ≥ 6 ký tự.");
            AttachHtmlTip(txtConfirmD, "Nhập lại mật khẩu.");
            AttachHtmlTip(txtDriverPhone, "SĐT: 9–15 chữ số.");
            AttachHtmlTip(cmbLicenseType, "<b>B2</b>: xe tải ≤3.5t, bán tải/van<br/><b>C</b>: xe tải >3.5t<br/><b>CE/FC</b>: đầu kéo + rơ-moóc");
        }

        private void AttachHtmlTip(Control ctl, string html)
        {
            ctl.Enter += (s, e) =>
            {
                var ttSize = TextRenderer.MeasureText(
                    Regex.Replace(html, "<.*?>", string.Empty),
                    SystemFonts.DefaultFont);
                int estWidth = Math.Min(htip.MaximumSize.Width, ttSize.Width + 24);
                int estHeight = Math.Max(26, ttSize.Height + 14);
                int x = ctl.Width + 6;
                int y = (ctl.Height - estHeight) / 2 - 2;
                Point scr = ctl.PointToScreen(new Point(x, y));
                int screenRight = Screen.FromControl(this).WorkingArea.Right;
                int screenTop = Screen.FromControl(this).WorkingArea.Top;
                int screenBottom = Screen.FromControl(this).WorkingArea.Bottom;
                if (scr.X + estWidth > screenRight) x = -estWidth - 8;
                scr = ctl.PointToScreen(new Point(x, y));
                if (scr.Y < screenTop) y += (screenTop - scr.Y);
                else if (scr.Y + estHeight > screenBottom) y -= (scr.Y + estHeight - screenBottom);
                htip.Show(html, ctl, x, y, int.MaxValue);
            };
            ctl.Leave += (s, e) => htip.Hide(ctl);
            ctl.EnabledChanged += (s, e) => { if (!ctl.Enabled) htip.Hide(ctl); };
            ctl.VisibleChanged += (s, e) => { if (!ctl.Visible) htip.Hide(ctl); };
        }

        private void TogglePasswordMask(Guna2TextBox tb, bool showPlain)
        {
            tb.UseSystemPasswordChar = false;
            tb.PasswordChar = showPlain ? '\0' : '●';
        }

        private void InitPasswordMask(Guna2TextBox tb)
        {
            tb.UseSystemPasswordChar = false;
            tb.PasswordChar = '●';
        }

        private void btnRegisterD_Click(object sender, EventArgs e)
        {
            try
            {
                var fullName = txtFullnameD.Text.Trim();
                var username = txtUsernameD.Text.Trim();
                var pass = txtPasswordD.Text;
                var confirm = txtConfirmD.Text;
                var phone = txtDriverPhone.Text.Trim();
                var licenseType = cmbLicenseType.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(fullName))
                {
                    MessageBox.Show("Vui lòng nhập họ tên.");
                    txtFullnameD.Focus();
                    return;
                }
                if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]{8,16}$"))
                {
                    MessageBox.Show("Tên tài khoản phải 8–16 ký tự (chữ/số/_)");
                    txtUsernameD.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải ≥ 6 ký tự.");
                    txtPasswordD.Focus();
                    return;
                }
                if (pass != confirm)
                {
                    MessageBox.Show("Xác nhận mật khẩu không khớp.");
                    txtConfirmD.Focus();
                    return;
                }
                if (!Regex.IsMatch(phone, @"^\d{9,15}$"))
                {
                    MessageBox.Show("SĐT không hợp lệ.");
                    txtDriverPhone.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(licenseType))
                {
                    MessageBox.Show("Vui lòng chọn loại GPLX.");
                    cmbLicenseType.Focus();
                    return;
                }
                _auth.RegisterDriver(fullName, username, pass, phone, licenseType);
                MessageBox.Show("Đăng ký tài xế thành công!", "Thành công");
                this.FindForm()?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi đăng ký");
            }
        }

        private void btnExitD_Click(object sender, EventArgs e)
        {
            if (this.FindForm()?.Owner is frmLogin ownerLogin)
            {
                ownerLogin.Show();
                ownerLogin.Activate();
            }
            else
            {
                var login = Application.OpenForms.OfType<frmLogin>().FirstOrDefault() ?? new frmLogin();
                if (!login.Visible) login.Show();
                login.Activate();
            }
            this.FindForm()?.Close();
        }
    }
}