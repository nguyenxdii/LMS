using Guna.UI2.WinForms;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

//============ Namespace ============
namespace LMS.GUI.Auth
{
    public partial class frmRegister : Form
    {
        private readonly AuthService _auth = new AuthService();
        private readonly Guna2HtmlToolTip htip = new Guna2HtmlToolTip();
        private bool _suppressRoleChanged = false;

        // Gợi ý email dạng abc@... (menu nhỏ)
        private readonly ContextMenuStrip emailMenu = new ContextMenuStrip();
        private readonly string[] emailDomains = new[] { "gmail.com", "yahoo.com", "outlook.com", "hotmail.com", "icloud.com", "student.edu.vn", "company.com" };

        //============ Constructor ============
        public frmRegister()
        {
            InitializeComponent();

            // khóa kích thước form
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Ẩn cả 2 uc ban đầu
            //_ucCustomer.Visible = false;
            //_ucDriver.Visible = false;

            // AcceptButton sẽ set theo role
            this.AcceptButton = null;

            // ===== CHỌN LOẠI TÀI KHOẢN =====
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.DataSource = new[] { UserRole.Customer, UserRole.Driver };
            SetRoleIndex(null);
            cmbRole.SelectedIndexChanged += cmbRole_SelectedIndexChanged;

            // Ẩn panel ban đầu
            pnlDriverFields.Visible = false;
            pnlCustomerFields.Visible = false;

            // ====== Nút ======
            btnRegisterC.Click += btnRegisterC_Click;
            btnExitC.Click += btnExitC_Click;
            btnRegisterD.Click += btnRegisterD_Click;
            btnExitD.Click += btnExitD_Click;

            // Layout
            pnlDriverFields.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pnlCustomerFields.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // AcceptButton sẽ set theo role
            this.AcceptButton = null;

            // ====== CẤU HÌNH GUNA2 HTML TOOLTIP ======
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

            // ====== SHOW/HIDE PASSWORD ======
            chkShowPassC1.Checked = chkShowPassC2.Checked = false;
            chkShowPassD1.Checked = chkShowPassD2.Checked = false;

            InitPasswordMask(txtPasswordC);
            InitPasswordMask(txtConfirmC);
            InitPasswordMask(txtPasswordD);
            InitPasswordMask(txtConfirmD);

            chkShowPassC1.CheckedChanged += (s, e) => TogglePasswordMask(txtPasswordC, chkShowPassC1.Checked);
            chkShowPassC2.CheckedChanged += (s, e) => TogglePasswordMask(txtConfirmC, chkShowPassC2.Checked);
            chkShowPassD1.CheckedChanged += (s, e) => TogglePasswordMask(txtPasswordD, chkShowPassD1.Checked);
            chkShowPassD2.CheckedChanged += (s, e) => TogglePasswordMask(txtConfirmD, chkShowPassD2.Checked);

            // === GPLX ComboBox ===
            cmbLicenseType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLicenseType.Items.AddRange(new object[] { "B2", "C", "CE", "D", "DE", "FC" });
            cmbLicenseType.StartIndex = -1;

            // Cố định kích thước form
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            // đặt vị trí form ở giữa màn hình
            this.StartPosition = FormStartPosition.CenterScreen;
            // bỏ viền form
            this.FormBorderStyle = FormBorderStyle.None;

            //
            guna2GroupBox1.FillColor = Color.White;
            guna2GroupBox1.BackColor = Color.Transparent;

            guna2GroupBox1.BringToFront();

            // AutoComplete email (gõ @ đầu vẫn hoạt động)
            ConfigureEmailAutoComplete();
            // Menu gợi ý khi gõ abc@...
            ConfigureEmailSuggestMenu();

            // Đăng ký tooltip cho các ô
            RegisterFieldTips();
        }

        //============ Password Management ============
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

        //============ COMBOBOX ROLE ============
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
                    cmbRole.Text = string.Empty;
                }
            }
            finally { _suppressRoleChanged = false; }
        }

        //============ EMAIL SUGGEST (abc@...) ============
        private void ConfigureEmailSuggestMenu()
        {
            emailMenu.ShowImageMargin = false;
            emailMenu.Font = new Font("Segoe UI", 9);
            emailMenu.MaximumSize = new Size(txtCusEmail.Width, 220);

            txtCusEmail.TextChanged += TxtCusEmail_TextChanged;
            txtCusEmail.LostFocus += (s, e) => { if (!emailMenu.Focused) emailMenu.Hide(); };
            txtCusEmail.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape && emailMenu.Visible) { emailMenu.Hide(); e.SuppressKeyPress = true; }
            };
        }

        private void TxtCusEmail_TextChanged(object sender, EventArgs e)
        {
            var tb = txtCusEmail;
            var text = tb.Text;
            int at = text.IndexOf('@');
            if (at < 0) { emailMenu.Hide(); return; }

            string local = text.Substring(0, at);
            string frag = text.Substring(at + 1);

            if (string.IsNullOrWhiteSpace(local)) { emailMenu.Hide(); return; }

            var items = emailDomains
                .Where(d => d.StartsWith(frag, StringComparison.OrdinalIgnoreCase))
                .Select(d => $"{local}@{d}")
                .Take(8)
                .ToList();

            if (items.Count == 0) { emailMenu.Hide(); return; }

            emailMenu.Items.Clear();
            foreach (var it in items)
            {
                var mi = new ToolStripMenuItem(it);
                mi.Click += (s, _) =>
                {
                    tb.Text = it;
                    tb.SelectionStart = tb.Text.Length;
                    tb.SelectionLength = 0;
                    emailMenu.Hide();
                };
                emailMenu.Items.Add(mi);
            }

            var ptClient = new Point(0, tb.Height + 2);
            var ptScreen = tb.PointToScreen(ptClient);
            emailMenu.Show(ptScreen);
        }

        //============ EMAIL AUTOCOMPLETE (@ ở đầu) ============
        private void ConfigureEmailAutoComplete()
        {
            var src = new AutoCompleteStringCollection();
            src.AddRange(new[] { "@gmail.com", "@yahoo.com", "@outlook.com", "@hotmail.com", "@icloud.com", "@student.edu.vn", "@company.com" });
            txtCusEmail.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtCusEmail.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCusEmail.AutoCompleteCustomSource = src;
        }

        //============ HTML TOOLTIP (focus vào ô) ============
        private void RegisterFieldTips()
        {
            // CUSTOMER
            AttachHtmlTip(txtFullnameC, "<b>Họ tên</b>: ghi đầy đủ, có dấu.");
            AttachHtmlTip(txtUsernameC, "<b>Tài khoản</b>: 8–16 ký tự (chữ/số/_).");
            AttachHtmlTip(txtPasswordC, "<b>Mật khẩu</b> ≥ 6 ký tự.<br/>Có thể bật <i>Hiện mật khẩu</i> để xem.");
            AttachHtmlTip(txtConfirmC, "Nhập lại <b>mật khẩu</b> giống ô trên.");
            AttachHtmlTip(txtCusAddress, "Địa chỉ: Số nhà, đường, phường/xã, quận/huyện, tỉnh/thành.");
            AttachHtmlTip(txtCusPhone, "<b>SĐT</b>: 9–15 chữ số (0–9).");
            AttachHtmlTip(txtCusEmail, "Ví dụ: <i>ten@domain.com</i>.<br/>Gõ phần sau <b>@</b> để hiện gợi ý.");

            // DRIVER
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

        //============ CUSTOMER ============
        private void btnRegisterC_Click(object sender, EventArgs e)
        {
            try
            {
                var fullName = txtFullnameC.Text.Trim();
                var username = txtUsernameC.Text.Trim();
                var pass = txtPasswordC.Text;
                var confirm = txtConfirmC.Text;
                var addr = txtCusAddress.Text.Trim();
                var phone = txtCusPhone.Text.Trim();
                var email = txtCusEmail.Text.Trim();

                if (string.IsNullOrWhiteSpace(fullName))
                {
                    MessageBox.Show("Vui lòng nhập họ tên.");
                    txtFullnameC.Focus();
                    return;
                }
                if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]{8,16}$"))
                {
                    MessageBox.Show("Tên tài khoản phải 8–16 ký tự (chữ/số/_)");
                    txtUsernameC.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải ≥ 6 ký tự.");
                    txtPasswordC.Focus();
                    return;
                }
                if (pass != confirm)
                {
                    MessageBox.Show("Xác nhận mật khẩu không khớp.");
                    txtConfirmC.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(addr))
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ.");
                    txtCusAddress.Focus();
                    return;
                }
                if (!Regex.IsMatch(phone, @"^\d{9,15}$"))
                {
                    MessageBox.Show("SĐT không hợp lệ.");
                    txtCusPhone.Focus();
                    return;
                }
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Email không hợp lệ.");
                    txtCusEmail.Focus();
                    return;
                }

                _auth.RegisterCustomer(fullName, username, pass, addr, phone, email);
                MessageBox.Show("Đăng ký khách hàng thành công!", "Thành công");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi đăng ký");
            }
        }

        //============ DRIVER ============
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
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi đăng ký");
            }
        }

        //============ EXIT ============
        private void btnExitC_Click(object sender, EventArgs e)
        {
            if (this.Owner is frmLogin ownerLogin)
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

            this.Close();
        }

        private void btnExitD_Click(object sender, EventArgs e)
        {
            if (this.Owner is frmLogin ownerLogin)
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

            this.Close();
        }

        //============ ROLE SWITCH ============
        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressRoleChanged) return;

            pnlCustomerFields.Visible = false;
            pnlDriverFields.Visible = false;

            if (cmbRole.SelectedItem is UserRole role)
            {
                if (role == UserRole.Customer)
                {
                    pnlCustomerFields.Visible = true;
                    pnlCustomerFields.BringToFront();
                    this.AcceptButton = btnRegisterC;
                }
                else
                {
                    pnlDriverFields.Visible = true;
                    pnlDriverFields.BringToFront();
                    this.AcceptButton = btnRegisterD;
                }
            }
        }
    }
}