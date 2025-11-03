using Guna.UI2.WinForms;
using LMS.BUS.Services;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LMS.GUI.Auth
{
    public partial class ucRegister_Cus : UserControl
    {
        private readonly AuthService _auth;
        private readonly Guna2HtmlToolTip htip = new Guna2HtmlToolTip();
        private readonly ContextMenuStrip emailMenu = new ContextMenuStrip();
        private readonly string[] emailDomains = new[] { "gmail.com", "yahoo.com", "outlook.com", "hotmail.com", "icloud.com", "student.edu.vn", "company.com" };

        public ucRegister_Cus(AuthService auth)
        {
            InitializeComponent();
            _auth = auth;

            // cấu hình tooltip và mẹo điền trường
            ConfigureTooltip();
            RegisterFieldTips();

            // gợi ý, autocomplete email
            ConfigureEmailAutoComplete();
            ConfigureEmailSuggestMenu();

            // cấu hình hiển thị mật khẩu
            chkShowPassC1.Checked = chkShowPassC2.Checked = false;
            InitPasswordMask(txtPasswordC);
            InitPasswordMask(txtConfirmC);
            chkShowPassC1.CheckedChanged += (s, e) => TogglePasswordMask(txtPasswordC, chkShowPassC1.Checked);
            chkShowPassC2.CheckedChanged += (s, e) => TogglePasswordMask(txtConfirmC, chkShowPassC2.Checked);

            // hiển thị ảnh đại diện
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;

            // gán sự kiện nút
            btnChooseImage.Click += btnChooseImage_Click;
            btnRegisterC.Click += btnRegisterC_Click;
            btnExitC.Click += btnExitC_Click;
        }

        // cấu hình style cho tooltip html
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

        // gắn các mẹo nhập liệu cho từng trường
        private void RegisterFieldTips()
        {
            AttachHtmlTip(txtFullnameC, "<b>Họ tên</b>: ghi đầy đủ, có dấu.");
            AttachHtmlTip(txtUsernameC, "<b>Tài khoản</b>: 8–16 ký tự (chữ/số/_).");
            AttachHtmlTip(txtPasswordC, "<b>Mật khẩu</b> ≥ 6 ký tự.<br/>Có thể bật <i>Hiện mật khẩu</i> để xem.");
            AttachHtmlTip(txtConfirmC, "Nhập lại <b>mật khẩu</b> giống ô trên.");
            AttachHtmlTip(txtCusAddress, "Địa chỉ: Số nhà, đường, phường/xã, quận/huyện, tỉnh/thành.");
            AttachHtmlTip(txtCusPhone, "<b>SĐT</b>: 9–15 chữ số (0–9).");
            AttachHtmlTip(txtCusEmail, "Ví dụ: <i>ten@domain.com</i>.<br/>Gõ phần sau <b>@</b> để hiện gợi ý.");
        }

        // hiển thị tooltip html cạnh control, có xử lý tránh tràn màn hình
        private void AttachHtmlTip(Control ctl, string html)
        {
            ctl.Enter += (s, e) =>
            {
                var ttSize = TextRenderer.MeasureText(Regex.Replace(html, "<.*?>", string.Empty), SystemFonts.DefaultFont);
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

        // cấu hình autocomplete cho phần domain sau @
        private void ConfigureEmailAutoComplete()
        {
            var src = new AutoCompleteStringCollection();
            src.AddRange(new[] { "@gmail.com", "@yahoo.com", "@outlook.com", "@hotmail.com", "@icloud.com", "@student.edu.vn", "@company.com" });
            txtCusEmail.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtCusEmail.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCusEmail.AutoCompleteCustomSource = src;
        }

        // menu gợi ý email dạng context menu ngay dưới textbox
        private void ConfigureEmailSuggestMenu()
        {
            emailMenu.ShowImageMargin = false;
            emailMenu.Font = new Font("Segoe UI", 9);
            emailMenu.MaximumSize = new Size(txtCusEmail.Width, 220);

            txtCusEmail.TextChanged += TxtCusEmail_TextChanged;
            txtCusEmail.LostFocus += (s, e) => { if (!emailMenu.Focused) emailMenu.Hide(); };
            txtCusEmail.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape && emailMenu.Visible)
                {
                    emailMenu.Hide();
                    e.SuppressKeyPress = true;
                }
            };
        }

        // tạo danh sách gợi ý theo phần sau @ và hiển thị menu
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

        // bật/tắt ký tự mask cho mật khẩu guna2
        private void TogglePasswordMask(Guna2TextBox tb, bool showPlain)
        {
            tb.UseSystemPasswordChar = false;
            tb.PasswordChar = showPlain ? '\0' : '●';
        }

        // khởi tạo ký tự mask
        private void InitPasswordMask(Guna2TextBox tb)
        {
            tb.UseSystemPasswordChar = false;
            tb.PasswordChar = '●';
        }

        // chuyển ảnh sang byte[] để lưu db
        public byte[] ImageToByteArray(Image imageIn)
        {
            if (imageIn == null) return null;
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        // chọn ảnh đại diện từ file
        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif",
                Title = "Chọn Ảnh Đại Diện"
            };

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    picAvatar.Image = Image.FromFile(openFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể tải ảnh: " + ex.Message, "Lỗi Ảnh");
                }
            }
        }

        // xử lý đăng ký khách hàng
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
                    MessageBox.Show("Vui Lòng Nhập Họ Tên.");
                    txtFullnameC.Focus();
                    return;
                }
                if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]{8,16}$"))
                {
                    MessageBox.Show("Tên Tài Khoản Phải 8–16 Ký Tự (Chữ/Số/_).");
                    txtUsernameC.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6)
                {
                    MessageBox.Show("Mật Khẩu Phải ≥ 6 Ký Tự.");
                    txtPasswordC.Focus();
                    return;
                }
                if (pass != confirm)
                {
                    MessageBox.Show("Xác Nhận Mật Khẩu Không Khớp.");
                    txtConfirmC.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(addr))
                {
                    MessageBox.Show("Vui Lòng Nhập Địa Chỉ.");
                    txtCusAddress.Focus();
                    return;
                }
                if (!Regex.IsMatch(phone, @"^\d{9,15}$"))
                {
                    MessageBox.Show("SĐT Không Hợp Lệ.");
                    txtCusPhone.Focus();
                    return;
                }
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Email Không Hợp Lệ.");
                    txtCusEmail.Focus();
                    return;
                }

                byte[] avatarData = null;
                // chỉ lưu ảnh nếu khác ảnh mặc định
                if (picAvatar.Image != null &&
                    !picAvatar.Image.Equals(LMS.GUI.Properties.Resources.default_avatar_2))
                {
                    avatarData = ImageToByteArray(picAvatar.Image);
                }

                _auth.RegisterCustomer(fullName, username, pass, addr, phone, email, avatarData);
                MessageBox.Show("Đăng Ký Khách Hàng Thành Công!", "Thành Công");
                this.FindForm()?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Đăng Ký");
            }
        }

        // thoát về màn hình đăng nhập
        private void btnExitC_Click(object sender, EventArgs e)
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
