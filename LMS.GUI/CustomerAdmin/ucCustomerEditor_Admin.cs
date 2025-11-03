using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LMS.GUI.CustomerAdmin
{
    public partial class ucCustomerEditor_Admin : UserControl
    {
        public enum EditorMode { Add, Edit }

        private EditorMode _mode = EditorMode.Add;
        private int _customerId = 0;
        private readonly CustomerService _customerSvc = new CustomerService();
        private readonly ErrorProvider errProvider;
        private CustomerEditorDto _originalData;

        private readonly Guna2HtmlToolTip htip = new Guna2HtmlToolTip();
        private readonly ContextMenuStrip emailMenu = new ContextMenuStrip();
        private readonly string[] emailDomains =
        {
            "gmail.com", "yahoo.com", "outlook.com", "hotmail.com",
            "icloud.com", "student.edu.vn", "company.com"
        };

        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;
        private Point parentFormStartPoint = Point.Empty;

        public ucCustomerEditor_Admin()
        {
            InitializeComponent();
            errProvider = new ErrorProvider { ContainerControl = this };

            // cấu hình tooltip + gợi ý
            ConfigureTooltip();
            SetupFieldTips();
            ConfigureEmailSuggestMenu();

            // sự kiện điều khiển
            WireEvents();

            // focus mặc định
            this.Load += (s, e) => { txtFullName.Focus(); };
        }

        // cấu hình tooltip html
        private void ConfigureTooltip()
        {
            htip.TitleFont = new Font("Segoe UI", 9, FontStyle.Bold);
            htip.TitleForeColor = Color.FromArgb(31, 113, 185);
            htip.ForeColor = Color.Black;
            htip.BackColor = Color.White;
            htip.BorderColor = Color.FromArgb(210, 210, 210);
            htip.MaximumSize = new Size(300, 0);
            htip.InitialDelay = 150;
            htip.AutoPopDelay = 30000;
            htip.ReshowDelay = 100;
            htip.ShowAlways = true;
            htip.UseAnimation = false;
            htip.UseFading = false;
        }

        // gắn mẹo cho các trường nhập
        private void SetupFieldTips()
        {
            AttachHtmlTip(txtFullName, "<b>Họ và tên khách hàng</b><br/><i>Ví dụ: Công ty TNHH ABC</i>");
            AttachHtmlTip(txtPhone, "<b>Số điện thoại</b><br/><i>(9-15 chữ số). Ví dụ: 0912345678</i>");
            AttachHtmlTip(txtEmail, "<b>Địa chỉ Email</b><br/><i>(Tùy chọn). Gõ phần sau '@' để xem gợi ý.</i>");
            AttachHtmlTip(txtAddress, "<b>Địa chỉ chi tiết</b><br/><i>Số nhà, đường, phường/xã, quận/huyện, tỉnh/thành phố.</i>");
            AttachHtmlTip(txtUsername, "<b>Tên đăng nhập</b><br/><i>(8-16 ký tự, chỉ gồm chữ cái, số, dấu gạch dưới '_').</i>");
            AttachHtmlTip(txtPassword, "<b>Mật khẩu</b><br/><i>(Ít nhất 6 ký tự).<br/>Khi sửa: Để trống nếu không muốn thay đổi.</i>");
            AttachHtmlTip(txtConfirmPassword, "<b>Xác nhận mật khẩu</b><br/><i>Nhập lại mật khẩu giống ô trên.</i>");
        }

        // hiển thị tooltip cạnh control, tránh tràn màn hình
        private void AttachHtmlTip(Control ctl, string html)
        {
            ctl.Enter += (sender, e) =>
            {
                try
                {
                    var ttSize = TextRenderer.MeasureText(Regex.Replace(html, "<.*?>", string.Empty), SystemFonts.DefaultFont);
                    int estWidth = Math.Min(htip.MaximumSize.Width, ttSize.Width + 24);
                    int estHeight = Math.Max(26, ttSize.Height + 14);

                    int x = ctl.Width + 6;
                    int y = (ctl.Height - estHeight) / 2 - 2;

                    Point scr = ctl.PointToScreen(new Point(x, y));
                    Screen screen = Screen.FromControl(this);

                    if (scr.X + estWidth > screen.WorkingArea.Right) x = -estWidth - 8;

                    scr = ctl.PointToScreen(new Point(x, y));
                    if (scr.Y < screen.WorkingArea.Top) y += (screen.WorkingArea.Top - scr.Y);
                    else if (scr.Y + estHeight > screen.WorkingArea.Bottom) y -= (scr.Y + estHeight - screen.WorkingArea.Bottom);

                    htip.Show(html, ctl, x, y, htip.AutoPopDelay);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error showing tooltip for {ctl.Name}: {ex.Message}");
                }
            };

            ctl.Leave += (sender, e) => htip.Hide(ctl);
            ctl.EnabledChanged += (sender, e) => { if (!ctl.Enabled) htip.Hide(ctl); };
            ctl.VisibleChanged += (sender, e) => { if (!ctl.Visible) htip.Hide(ctl); };

            this.HandleCreated += (s, e) =>
            {
                var parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.FormClosing -= ParentForm_FormClosing;
                    parentForm.FormClosing += ParentForm_FormClosing;
                }
            };
        }

        // ẩn tooltip khi đóng form
        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            htip.Hide(this);
        }

        // gợi ý domain email bằng context menu
        private void ConfigureEmailSuggestMenu()
        {
            emailMenu.ShowImageMargin = false;
            emailMenu.Font = new Font("Segoe UI", 9);

            if (txtEmail != null)
            {
                emailMenu.MaximumSize = new Size(txtEmail.Width, 220);
                txtEmail.TextChanged += TxtEmail_TextChanged;
                txtEmail.LostFocus += (s, e) =>
                {
                    if (!emailMenu.Focused && !txtEmail.Focused) emailMenu.Hide();
                };
                txtEmail.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Escape && emailMenu.Visible)
                    {
                        emailMenu.Hide();
                        e.SuppressKeyPress = true;
                    }
                };
                txtEmail.SizeChanged += (s, e) => { emailMenu.MaximumSize = new Size(txtEmail.Width, 220); };
            }
        }

        // xử lý hiển thị menu gợi ý email theo phần sau '@'
        private void TxtEmail_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as Guna2TextBox;
            if (tb == null) return;

            var text = tb.Text;
            int atIndex = text.IndexOf('@');
            if (atIndex <= 0) { emailMenu.Hide(); return; }

            string localPart = text.Substring(0, atIndex);
            string domainFragment = text.Substring(atIndex + 1);

            var suggestions = emailDomains
                .Where(d => d.StartsWith(domainFragment, StringComparison.OrdinalIgnoreCase) && d.Length > domainFragment.Length)
                .Select(d => $"{localPart}@{d}")
                .Take(8)
                .ToList();

            if (suggestions.Count == 0) { emailMenu.Hide(); return; }

            emailMenu.Items.Clear();
            foreach (var suggestion in suggestions)
            {
                var menuItem = new ToolStripMenuItem(suggestion);
                menuItem.Click += (sItem, _) =>
                {
                    tb.Text = suggestion;
                    tb.SelectionStart = tb.Text.Length;
                    tb.SelectionLength = 0;
                    emailMenu.Hide();
                };
                emailMenu.Items.Add(menuItem);
            }

            var location = tb.PointToScreen(new Point(0, tb.Height));
            Screen screen = Screen.FromControl(tb);

            if (location.Y + emailMenu.PreferredSize.Height > screen.WorkingArea.Bottom)
            {
                location = tb.PointToScreen(new Point(0, -emailMenu.PreferredSize.Height));
            }
            if (location.X + emailMenu.Width > screen.WorkingArea.Right)
            {
                location.X = screen.WorkingArea.Right - emailMenu.Width;
            }

            emailMenu.Width = tb.Width;
            emailMenu.Show(location);
        }

        // nạp dữ liệu cho chế độ thêm/sửa
        public void LoadData(EditorMode mode, int? customerId)
        {
            _mode = mode;
            errProvider.Clear();
            ResetConfirmPasswordVisibility();

            // đặt tiêu đề bằng label (nếu có)
            var titleFont = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            if (lblTitle != null)
            {
                lblTitle.Font = titleFont;

                if (mode == EditorMode.Edit)
                {
                    lblTitle.Text = "Sửa Thông Tin Khách Hàng";

                    if (!customerId.HasValue) throw new ArgumentNullException(nameof(customerId));
                    _customerId = customerId.Value;

                    try
                    {
                        var dto = _customerSvc.GetCustomerForEdit(_customerId);
                        _originalData = dto;

                        txtFullName.Text = dto.FullName;
                        txtPhone.Text = dto.Phone;
                        txtEmail.Text = dto.Email;
                        txtAddress.Text = dto.Address;
                        txtUsername.Text = dto.Username;

                        txtPassword.Clear();
                        txtConfirmPassword.Clear();

                        lblConfirmPassLabel.Visible = false;
                        txtConfirmPassword.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi Tải Thông Tin: {ex.Message}", "Lỗi");
                    }
                }
                else
                {
                    lblTitle.Text = "Thêm Thông Tin Khách Hàng";

                    _originalData = new CustomerEditorDto();

                    txtFullName.Clear();
                    txtPhone.Clear();
                    txtEmail.Clear();
                    txtAddress.Clear();
                    txtUsername.Clear();
                    txtPassword.Clear();
                    txtConfirmPassword.Clear();

                    lblConfirmPassLabel.Visible = true;
                    txtConfirmPassword.Visible = true;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Cảnh báo: Không tìm thấy lblTitle trong ucCustomerEditor_Admin.");
            }
        }

        // gắn sự kiện điều khiển
        private void WireEvents()
        {
            btnSave.Click += (s, e) => Save();
            btnCancel.Click += (s, e) => Cancel();
            txtPassword.TextChanged += TxtPassword_TextChanged;

            // cho phép kéo form cha bằng grpInfo (nếu có)
            Control dragHandle = grpInfo;
            if (dragHandle != null)
            {
                dragHandle.MouseDown += DragHandle_MouseDown;
                dragHandle.MouseMove += DragHandle_MouseMove;
                dragHandle.MouseUp += DragHandle_MouseUp;
            }
        }

        // hiển thị/ẩn xác nhận mật khẩu khi sửa
        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            if (_mode == EditorMode.Edit)
            {
                bool showConfirm = !string.IsNullOrWhiteSpace(txtPassword.Text);
                lblConfirmPassLabel.Visible = showConfirm;
                txtConfirmPassword.Visible = showConfirm;
                if (!showConfirm)
                {
                    txtConfirmPassword.Clear();
                    errProvider.SetError(txtConfirmPassword, "");
                }
            }
        }

        // reset hiển thị ô xác nhận theo mode
        private void ResetConfirmPasswordVisibility()
        {
            bool isAddMode = (_mode == EditorMode.Add);
            lblConfirmPassLabel.Visible = isAddMode;
            txtConfirmPassword.Visible = isAddMode;
        }

        // validate dữ liệu nhập
        private bool ValidateInput()
        {
            errProvider.Clear();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                errProvider.SetError(txtFullName, "Họ tên trống.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errProvider.SetError(txtUsername, "Tên đăng nhập trống.");
                isValid = false;
            }
            else if (!Regex.IsMatch(txtUsername.Text.Trim(), @"^[a-zA-Z0-9_]{8,16}$"))
            {
                errProvider.SetError(txtUsername, "Tên đăng nhập 8-16 ký tự (a-z, 0-9, _).");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text) &&
                !Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errProvider.SetError(txtEmail, "Email không hợp lệ.");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtPhone.Text) &&
                !Regex.IsMatch(txtPhone.Text.Trim(), @"^\d{9,15}$"))
            {
                errProvider.SetError(txtPhone, "SĐT không hợp lệ (9-15 số).");
                isValid = false;
            }

            string pass = txtPassword.Text;
            string confirmPass = txtConfirmPassword.Text;

            if (_mode == EditorMode.Add)
            {
                if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6)
                {
                    errProvider.SetError(txtPassword, "Mật khẩu >= 6 ký tự.");
                    isValid = false;
                }
                else if (pass != confirmPass)
                {
                    errProvider.SetError(txtConfirmPassword, "Xác nhận mật khẩu sai.");
                    isValid = false;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(pass))
                {
                    if (pass.Length < 6)
                    {
                        errProvider.SetError(txtPassword, "Mật khẩu mới >= 6 ký tự.");
                        isValid = false;
                    }
                    else if (lblConfirmPassLabel.Visible && pass != confirmPass)
                    {
                        errProvider.SetError(txtConfirmPassword, "Xác nhận mật khẩu sai.");
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        // kiểm tra có thay đổi dữ liệu hay chưa
        private bool HasChanges()
        {
            if (_mode == EditorMode.Add)
            {
                return !string.IsNullOrWhiteSpace(txtFullName.Text)
                       || !string.IsNullOrWhiteSpace(txtPhone.Text)
                       || !string.IsNullOrWhiteSpace(txtEmail.Text)
                       || !string.IsNullOrWhiteSpace(txtAddress.Text)
                       || !string.IsNullOrWhiteSpace(txtUsername.Text)
                       || !string.IsNullOrWhiteSpace(txtPassword.Text);
            }
            else
            {
                if (_originalData == null) return false;

                return (_originalData.FullName ?? "") != txtFullName.Text.Trim()
                       || (_originalData.Phone ?? "") != txtPhone.Text.Trim()
                       || (_originalData.Email ?? "") != txtEmail.Text.Trim()
                       || (_originalData.Address ?? "") != txtAddress.Text.Trim()
                       || (_originalData.Username ?? "") != txtUsername.Text.Trim()
                       || !string.IsNullOrWhiteSpace(txtPassword.Text);
            }
        }

        // lưu thông tin
        private void Save()
        {
            if (!ValidateInput())
            {
                MessageBox.Show("Vui Lòng Kiểm Tra Lại Thông Tin.", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Lưu Thông Tin Này?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            var dto = new CustomerEditorDto
            {
                Id = _customerId,
                FullName = txtFullName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text
            };

            try
            {
                if (_mode == EditorMode.Add)
                    _customerSvc.CreateCustomerAndAccount(dto);
                else
                    _customerSvc.UpdateCustomerAndAccount(dto);

                MessageBox.Show("Lưu Thành Công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.DialogResult = DialogResult.OK;
                    parentForm.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi Khi Lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // hủy và đóng form
        private void Cancel()
        {
            if (HasChanges())
            {
                var confirm = MessageBox.Show("Có Thay Đổi Chưa Lưu. Hủy Bỏ?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;
            }

            var parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.DialogResult = DialogResult.Cancel;
                parentForm.Close();
            }
        }

        // kéo thả form cha bằng grpInfo
        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var parentForm = this.FindForm();
                if (parentForm != null)
                {
                    isDragging = true;
                    dragStartPoint = Cursor.Position;
                    parentFormStartPoint = parentForm.Location;
                }
            }
        }

        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var parentForm = this.FindForm();
                if (parentForm != null)
                {
                    Point diff = Point.Subtract(Cursor.Position, new Size(dragStartPoint));
                    parentForm.Location = Point.Add(parentFormStartPoint, new Size(diff));
                }
            }
        }

        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
                dragStartPoint = Point.Empty;
                parentFormStartPoint = Point.Empty;
            }
        }
    }
}
