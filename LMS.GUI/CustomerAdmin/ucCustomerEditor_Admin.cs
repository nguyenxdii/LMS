////using Guna.UI2.WinForms;
////using LMS.BUS.Dtos; // Ensure DTOs are in this namespace or add using
////using LMS.BUS.Services;
////using System;
////using System.Drawing;
////using System.Linq; // Needed for Linq operations in email suggest
////using System.Text.RegularExpressions;
////using System.Windows.Forms;

////namespace LMS.GUI.CustomerAdmin
////{
////    public partial class ucCustomerEditor_Admin : UserControl
////    {
////        public enum EditorMode { Add, Edit }

////        private EditorMode _mode = EditorMode.Add;
////        private int _customerId = 0;
////        private readonly CustomerService _customerSvc = new CustomerService();
////        private ErrorProvider errProvider;

////        // --- (1) BIẾN CHO TOOLTIP VÀ GỢI Ý EMAIL ---
////        private readonly Guna2HtmlToolTip htip = new Guna2HtmlToolTip();
////        private readonly ContextMenuStrip emailMenu = new ContextMenuStrip();
////        private readonly string[] emailDomains = { // Danh sách domain gợi ý
////            "gmail.com", "yahoo.com", "outlook.com", "hotmail.com",
////            "icloud.com", "student.edu.vn", "company.com" // Thêm domain khác nếu cần
////        };
////        // --- KẾT THÚC BIẾN ---

////        public ucCustomerEditor_Admin()
////        {
////            InitializeComponent();

////            // Khởi tạo ErrorProvider
////            if (this.components == null) { this.components = new System.ComponentModel.Container(); }
////            this.errProvider = new System.Windows.Forms.ErrorProvider(this.components);
////            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).BeginInit();
////            // (Code designer khác)
////            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).EndInit();

////            // --- (2) GỌI CÁC HÀM CẤU HÌNH ---
////            ConfigureTooltip();         // Cấu hình giao diện tooltip
////            SetupFieldTips();           // Gán nội dung tooltip cho các ô
////            ConfigureEmailSuggestMenu();// Cấu hình menu gợi ý email
////            // --- KẾT THÚC GỌI HÀM ---

////            txtFullName.Focus(); // Đặt focus ban đầu
////            WireEvents();
////        }

////        #region Tooltip and Email Suggest Configuration
////        // --- (3) HÀM CẤU HÌNH TOOLTIP (Giống ucRegister_Cus) ---
////        private void ConfigureTooltip()
////        {
////            htip.TitleFont = new Font("Segoe UI", 9, FontStyle.Bold);
////            htip.TitleForeColor = Color.FromArgb(31, 113, 185);
////            htip.ForeColor = Color.Black;
////            htip.BackColor = Color.White;
////            htip.BorderColor = Color.FromArgb(210, 210, 210);
////            htip.MaximumSize = new Size(300, 0);
////            htip.InitialDelay = 100;
////            htip.AutoPopDelay = 30000;
////            htip.ReshowDelay = 80;
////            htip.ShowAlways = true;
////            htip.UseAnimation = false;
////            htip.UseFading = false;
////        }

////        // --- (4) HÀM GÁN NỘI DUNG TOOLTIP ---
////        private void SetupFieldTips()
////        {
////            // Gán tooltip cho từng control nhập liệu
////            AttachHtmlTip(txtFullName, "<b>Họ và tên khách hàng</b><br/><i>Ví dụ: Công ty TNHH ABC</i>");
////            AttachHtmlTip(txtPhone, "<b>Số điện thoại</b><br/><i>(9-15 chữ số). Ví dụ: 0283123456</i>");
////            AttachHtmlTip(txtEmail, "<b>Địa chỉ Email</b><br/><i>(Tùy chọn). Gõ phần sau '@' để xem gợi ý.</i>");
////            AttachHtmlTip(txtAddress, "<b>Địa chỉ chi tiết</b><br/><i>Số nhà, đường, phường/xã, quận/huyện, tỉnh/thành phố.</i>");
////            AttachHtmlTip(txtUsername, "<b>Tên đăng nhập</b><br/><i>(8-16 ký tự, chỉ gồm chữ cái, số, dấu gạch dưới).</i>");
////            AttachHtmlTip(txtPassword, "<b>Mật khẩu</b><br/><i>(Ít nhất 6 ký tự).<br/>Ở chế độ sửa: Để trống nếu không muốn thay đổi.</i>");
////            AttachHtmlTip(txtConfirmPassword, "<b>Xác nhận mật khẩu</b><br/><i>Nhập lại mật khẩu giống ô trên (chỉ hiện khi nhập mật khẩu mới).</i>");
////        }

////        // --- (5) HÀM GẮN TOOLTIP VÀO CONTROL (Giống ucRegister_Cus) ---
////        private void AttachHtmlTip(Control ctl, string html)
////        {
////            ctl.Enter += (sender, e) =>
////            {
////                try
////                {
////                    var ttSize = TextRenderer.MeasureText(Regex.Replace(html, "<.*?>", string.Empty), SystemFonts.DefaultFont);
////                    int estWidth = Math.Min(htip.MaximumSize.Width, ttSize.Width + 24);
////                    int estHeight = Math.Max(26, ttSize.Height + 14);
////                    int x = ctl.Width + 6;
////                    int y = (ctl.Height - estHeight) / 2 - 2;
////                    Point scr = ctl.PointToScreen(new Point(x, y));
////                    Screen screen = Screen.FromControl(this);
////                    int screenRight = screen.WorkingArea.Right; int screenTop = screen.WorkingArea.Top; int screenBottom = screen.WorkingArea.Bottom;
////                    if (scr.X + estWidth > screenRight) x = -estWidth - 8;
////                    scr = ctl.PointToScreen(new Point(x, y));
////                    if (scr.Y < screenTop) y += (screenTop - scr.Y);
////                    else if (scr.Y + estHeight > screenBottom) y -= (scr.Y + estHeight - screenBottom);
////                    htip.Show(html, ctl, x, y, int.MaxValue);
////                }
////                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Error showing tooltip for {ctl.Name}: {ex.Message}"); }
////            };
////            ctl.Leave += (sender, e) => htip.Hide(ctl);
////            ctl.EnabledChanged += (sender, e) => { if (!ctl.Enabled) htip.Hide(ctl); };
////            ctl.VisibleChanged += (sender, e) => { if (!ctl.Visible) htip.Hide(ctl); };
////        }

////        // --- (6) THÊM HÀM CẤU HÌNH GỢI Ý EMAIL (Giống ucRegister_Cus) ---
////        private void ConfigureEmailSuggestMenu()
////        {
////            emailMenu.ShowImageMargin = false; // Không hiển thị icon
////            emailMenu.Font = new Font("Segoe UI", 9); // Font chữ menu
////            emailMenu.MaximumSize = new Size(txtEmail.Width, 220); // Giới hạn kích thước menu

////            // Gán sự kiện cho TextBox Email
////            txtEmail.TextChanged += TxtEmail_TextChanged; // Khi nội dung thay đổi -> cập nhật gợi ý
////            txtEmail.LostFocus += (s, e) => { if (!emailMenu.Focused) emailMenu.Hide(); }; // Ẩn khi mất focus
////            txtEmail.KeyDown += (s, e) =>
////            { // Ẩn khi nhấn Escape
////                if (e.KeyCode == Keys.Escape && emailMenu.Visible) { emailMenu.Hide(); e.SuppressKeyPress = true; }
////            };
////        }

////        // --- (7) THÊM HÀM XỬ LÝ GỢI Ý EMAIL (Giống ucRegister_Cus, dùng txtEmail) ---
////        private void TxtEmail_TextChanged(object sender, EventArgs e)
////        {
////            var tb = txtEmail; // TextBox email trong UC này
////            var text = tb.Text;
////            int atIndex = text.IndexOf('@');

////            // Nếu không có @ hoặc @ ở đầu -> ẩn menu
////            if (atIndex <= 0) { emailMenu.Hide(); return; }

////            string localPart = text.Substring(0, atIndex); // Phần trước @
////            string domainFragment = text.Substring(atIndex + 1); // Phần sau @ (có thể rỗng)

////            // Lọc danh sách domain dựa trên phần đã gõ sau @
////            var suggestions = emailDomains
////                .Where(domain => domain.StartsWith(domainFragment, StringComparison.OrdinalIgnoreCase))
////                .Select(domain => $"{localPart}@{domain}") // Tạo lại email đầy đủ
////                .Take(8) // Giới hạn số lượng gợi ý
////                .ToList();

////            // Nếu không có gợi ý -> ẩn menu
////            if (suggestions.Count == 0) { emailMenu.Hide(); return; }

////            // Xóa item cũ và thêm item mới vào menu
////            emailMenu.Items.Clear();
////            foreach (var suggestion in suggestions)
////            {
////                var menuItem = new ToolStripMenuItem(suggestion);
////                menuItem.Click += (s, _) => // Gán sự kiện click cho từng item
////                {
////                    tb.Text = suggestion; // Điền email đã chọn vào TextBox
////                    tb.SelectionStart = tb.Text.Length; // Di chuyển con trỏ về cuối
////                    tb.SelectionLength = 0;
////                    emailMenu.Hide(); // Ẩn menu
////                    tb.Focus(); // Focus lại vào TextBox (quan trọng)
////                };
////                emailMenu.Items.Add(menuItem);
////            }

////            // Hiển thị menu gợi ý bên dưới TextBox Email
////            var location = tb.PointToScreen(new Point(0, tb.Height + 2));
////            emailMenu.Show(location);
////            // Cần set lại chiều rộng menu nếu chiều rộng TextBox thay đổi
////            emailMenu.Width = tb.Width;
////        }
////        #endregion

////        // --- Các hàm LoadData, WireEvents, TxtPassword_TextChanged, ResetConfirmPasswordVisibility, ValidateInput, Save, Cancel giữ nguyên ---
////        public void LoadData(EditorMode mode, int? customerId)
////        {
////            _mode = mode; errProvider.Clear(); ResetConfirmPasswordVisibility();
////            if (mode == EditorMode.Edit) { /* ... code load dữ liệu Edit ... */ }
////            else { /* ... code clear dữ liệu Add ... */ }
////        }
////        private void WireEvents()
////        {
////            btnSave.Click += (s, e) => Save();
////            btnCancel.Click += (s, e) => Cancel();
////            txtPassword.TextChanged += TxtPassword_TextChanged;
////        }
////        private void TxtPassword_TextChanged(object sender, EventArgs e)
////        {
////            if (_mode == EditorMode.Edit) { /* ... code ẩn/hiện confirm password ... */ }
////        }
////        //Reset visibility for LoadData
////        private void ResetConfirmPasswordVisibility()
////        {
////            bool isAddMode = (_mode == EditorMode.Add);
////            lblConfirmPassLabel.Visible = isAddMode;
////            txtConfirmPassword.Visible = isAddMode;
////        }

////        private bool ValidateInput()
////        {
////            errProvider.Clear();
////            bool isValid = true;

////            if (string.IsNullOrWhiteSpace(txtFullName.Text))
////            {
////                errProvider.SetError(txtFullName, "Họ tên không được để trống.");
////                isValid = false;
////            }
////            if (string.IsNullOrWhiteSpace(txtUsername.Text))
////            {
////                errProvider.SetError(txtUsername, "Tên đăng nhập không được để trống.");
////                isValid = false;
////            }
////            // Email: optional but must be valid if entered
////            if (!string.IsNullOrWhiteSpace(txtEmail.Text) &&
////                !Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
////            {
////                errProvider.SetError(txtEmail, "Email không hợp lệ.");
////                isValid = false;
////            }
////            // Phone: optional but must be valid if entered
////            if (!string.IsNullOrWhiteSpace(txtPhone.Text) &&
////                !Regex.IsMatch(txtPhone.Text.Trim(), @"^\d{9,15}$"))
////            {
////                errProvider.SetError(txtPhone, "Số điện thoại không hợp lệ (9-15 chữ số).");
////                isValid = false;
////            }

////            // Password Validation Logic
////            string pass = txtPassword.Text;
////            string confirmPass = txtConfirmPassword.Text;

////            if (_mode == EditorMode.Add)
////            {
////                // ADD Mode: Password required, must match confirm
////                if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6)
////                {
////                    errProvider.SetError(txtPassword, "Mật khẩu phải có ít nhất 6 ký tự.");
////                    isValid = false;
////                }
////                else if (pass != confirmPass)
////                {
////                    errProvider.SetError(txtConfirmPassword, "Xác nhận mật khẩu không khớp.");
////                    isValid = false;
////                }
////            }
////            else // EDIT Mode
////            {
////                // EDIT Mode: Password optional (only for reset)
////                // If password IS entered, validate it and confirm
////                if (!string.IsNullOrWhiteSpace(pass))
////                {
////                    if (pass.Length < 6)
////                    {
////                        errProvider.SetError(txtPassword, "Mật khẩu mới phải có ít nhất 6 ký tự.");
////                        isValid = false;
////                    }
////                    // Only check confirm password if the controls are visible
////                    else if (lblConfirmPassLabel.Visible && pass != confirmPass)
////                    {
////                        errProvider.SetError(txtConfirmPassword, "Xác nhận mật khẩu không khớp.");
////                        isValid = false;
////                    }
////                }
////                // Check if only confirm password was entered (while controls are visible)
////                else if (lblConfirmPassLabel.Visible && !string.IsNullOrWhiteSpace(confirmPass))
////                {
////                    errProvider.SetError(txtPassword, "Vui lòng nhập mật khẩu mới trước khi xác nhận.");
////                    isValid = false;
////                }
////                // If no new password entered, no password validation needed for Edit
////            }

////            return isValid;
////        }

////        private void Save()
////        {
////            if (!ValidateInput())
////            {
////                MessageBox.Show("Vui lòng kiểm tra lại thông tin đã nhập.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
////                return;
////            }

////            var confirmResult = MessageBox.Show("Bạn có chắc muốn lưu thông tin này?", "Xác nhận lưu",
////                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
////            if (confirmResult != DialogResult.Yes)
////                return;

////            var dto = new CustomerEditorDto
////            {
////                Id = _customerId, // Will be 0 in Add mode
////                FullName = txtFullName.Text.Trim(),
////                Phone = txtPhone.Text.Trim(),
////                Email = txtEmail.Text.Trim(),
////                Address = txtAddress.Text.Trim(),
////                Username = txtUsername.Text.Trim(),
////                // Send password, service will handle if it's empty during edit
////                Password = txtPassword.Text
////            };

////            try
////            {
////                if (_mode == EditorMode.Add)
////                {
////                    _customerSvc.CreateCustomerAndAccount(dto);
////                }
////                else // Mode.Edit
////                {
////                    _customerSvc.UpdateCustomerAndAccount(dto);
////                }

////                MessageBox.Show("Đã lưu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

////                // Close the parent form with OK result
////                var parentForm = this.FindForm();
////                if (parentForm != null)
////                {
////                    parentForm.DialogResult = DialogResult.OK;
////                    parentForm.Close();
////                }
////            }
////            catch (Exception ex)
////            {
////                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
////                // Don't close form on error
////            }
////        }

////        private void Cancel()
////        {
////            var confirmResult = MessageBox.Show("Bạn có chắc muốn hủy bỏ các thay đổi và đóng cửa sổ?", "Xác nhận hủy",
////                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
////            if (confirmResult == DialogResult.Yes)
////            {
////                // Close the parent form with Cancel result
////                var parentForm = this.FindForm();
////                if (parentForm != null)
////                {
////                    parentForm.DialogResult = DialogResult.Cancel;
////                    parentForm.Close();
////                }
////            }
////        }
////    }
////}


//using Guna.UI2.WinForms;
//using LMS.BUS.Dtos; // Đảm bảo DTOs nằm trong namespace này hoặc thêm using
//using LMS.BUS.Services;
//using System;
//using System.Drawing; // Cần cho Point, Size, Font, Color
//using System.Linq; // Cần cho Linq trong gợi ý email
//using System.Text.RegularExpressions; // Cần cho Regex trong AttachHtmlTip
//using System.Windows.Forms;

//namespace LMS.GUI.CustomerAdmin // Đảm bảo namespace này đúng
//{
//    public partial class ucCustomerEditor_Admin : UserControl
//    {
//        public enum EditorMode { Add, Edit }

//        private EditorMode _mode = EditorMode.Add;
//        private int _customerId = 0;
//        private readonly CustomerService _customerSvc = new CustomerService();
//        private ErrorProvider errProvider;

//        // --- Biến cho Tooltip và Gợi ý Email ---
//        private readonly Guna2HtmlToolTip htip = new Guna2HtmlToolTip();
//        private readonly ContextMenuStrip emailMenu = new ContextMenuStrip();
//        private readonly string[] emailDomains = {
//            "gmail.com", "yahoo.com", "outlook.com", "hotmail.com",
//            "icloud.com", "student.edu.vn", "company.com" // Thêm domain khác nếu cần
//        };
//        // --- Kết thúc biến ---

//        // --- (1) BIẾN ĐỂ KÉO THẢ FORM CHA ---
//        private bool isDragging = false;
//        private Point dragStartPoint = Point.Empty; // Vị trí chuột khi bắt đầu kéo
//        private Point parentFormStartPoint = Point.Empty; // Vị trí form cha khi bắt đầu kéo
//        // --- KẾT THÚC BIẾN KÉO THẢ ---

//        public ucCustomerEditor_Admin()
//        {
//            InitializeComponent();

//            // Khởi tạo ErrorProvider
//            // Cách này an toàn hơn, không phụ thuộc vào `this.components`
//            this.errProvider = new System.Windows.Forms.ErrorProvider();
//            this.errProvider.ContainerControl = this; // Gán container là chính UserControl này

//            // Cấu hình Tooltip, Gợi ý Email
//            ConfigureTooltip();
//            SetupFieldTips();
//            ConfigureEmailSuggestMenu();

//            // Gán sự kiện (bao gồm cả kéo thả)
//            WireEvents();

//            // Đặt focus ban đầu khi UC load xong
//            this.Load += (s, e) => { txtFullName.Focus(); };
//        }

//        #region Tooltip and Email Suggest Configuration
//        // --- Hàm ConfigureTooltip(), SetupFieldTips(), AttachHtmlTip() ---
//        private void ConfigureTooltip()
//        {
//            htip.TitleFont = new Font("Segoe UI", 9, FontStyle.Bold);
//            htip.TitleForeColor = Color.FromArgb(31, 113, 185);
//            htip.ForeColor = Color.Black; htip.BackColor = Color.White;
//            htip.BorderColor = Color.FromArgb(210, 210, 210); htip.MaximumSize = new Size(300, 0);
//            htip.InitialDelay = 150; htip.AutoPopDelay = 30000; htip.ReshowDelay = 100;
//            htip.ShowAlways = true; htip.UseAnimation = false; htip.UseFading = false;
//        }
//        private void SetupFieldTips()
//        {
//            AttachHtmlTip(txtFullName, "<b>Họ và tên khách hàng</b><br/><i>Ví dụ: Công ty TNHH ABC</i>");
//            AttachHtmlTip(txtPhone, "<b>Số điện thoại</b><br/><i>(9-15 chữ số). Ví dụ: 0912345678</i>");
//            AttachHtmlTip(txtEmail, "<b>Địa chỉ Email</b><br/><i>(Tùy chọn). Gõ phần sau '@' để xem gợi ý.</i>");
//            AttachHtmlTip(txtAddress, "<b>Địa chỉ chi tiết</b><br/><i>Số nhà, đường, phường/xã, quận/huyện, tỉnh/thành phố.</i>");
//            AttachHtmlTip(txtUsername, "<b>Tên đăng nhập</b><br/><i>(8-16 ký tự, chỉ gồm chữ cái, số, dấu gạch dưới '_').</i>");
//            AttachHtmlTip(txtPassword, "<b>Mật khẩu</b><br/><i>(Ít nhất 6 ký tự).<br/>Khi sửa: Để trống nếu không muốn thay đổi.</i>");
//            AttachHtmlTip(txtConfirmPassword, "<b>Xác nhận mật khẩu</b><br/><i>Nhập lại mật khẩu giống ô trên.</i>");
//        }
//        private void AttachHtmlTip(Control ctl, string html)
//        {
//            ctl.Enter += (sender, e) =>
//            {
//                try
//                {
//                    var ttSize = TextRenderer.MeasureText(Regex.Replace(html, "<.*?>", string.Empty), SystemFonts.DefaultFont);
//                    int estWidth = Math.Min(htip.MaximumSize.Width, ttSize.Width + 24); int estHeight = Math.Max(26, ttSize.Height + 14);
//                    int x = ctl.Width + 6; int y = (ctl.Height - estHeight) / 2 - 2;
//                    Point scr = ctl.PointToScreen(new Point(x, y)); Screen screen = Screen.FromControl(this);
//                    if (scr.X + estWidth > screen.WorkingArea.Right) x = -estWidth - 8;
//                    scr = ctl.PointToScreen(new Point(x, y));
//                    if (scr.Y < screen.WorkingArea.Top) y += (screen.WorkingArea.Top - scr.Y);
//                    else if (scr.Y + estHeight > screen.WorkingArea.Bottom) y -= (scr.Y + estHeight - screen.WorkingArea.Bottom);
//                    htip.Show(html, ctl, x, y, htip.AutoPopDelay);
//                }
//                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Error showing tooltip for {ctl.Name}: {ex.Message}"); }
//            };
//            ctl.Leave += (sender, e) => htip.Hide(ctl);
//            ctl.EnabledChanged += (sender, e) => { if (!ctl.Enabled) htip.Hide(ctl); };
//            ctl.VisibleChanged += (sender, e) => { if (!ctl.Visible) htip.Hide(ctl); };
//            // Gắn sự kiện FormClosing để ẩn tooltip khi form đóng
//            var parentForm = this.FindForm();
//            if (parentForm != null) { parentForm.FormClosing -= ParentForm_FormClosing; parentForm.FormClosing += ParentForm_FormClosing; }
//        }
//        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e) { htip.Hide(this); } // Ẩn tooltip khi form đóng

//        // --- Hàm ConfigureEmailSuggestMenu(), TxtEmail_TextChanged() ---
//        private void ConfigureEmailSuggestMenu()
//        {
//            emailMenu.ShowImageMargin = false; emailMenu.Font = new Font("Segoe UI", 9);
//            if (txtEmail != null)
//            {
//                emailMenu.MaximumSize = new Size(txtEmail.Width, 220);
//                txtEmail.TextChanged += TxtEmail_TextChanged;
//                txtEmail.LostFocus += (s, e) => { if (!emailMenu.Focused && !txtEmail.Focused) emailMenu.Hide(); };
//                txtEmail.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape && emailMenu.Visible) { emailMenu.Hide(); e.SuppressKeyPress = true; } };
//                txtEmail.SizeChanged += (s, e) => { emailMenu.MaximumSize = new Size(txtEmail.Width, 220); };
//            }
//        }
//        private void TxtEmail_TextChanged(object sender, EventArgs e)
//        {
//            var tb = sender as Guna2TextBox; if (tb == null) return;
//            var text = tb.Text; int atIndex = text.IndexOf('@');
//            if (atIndex <= 0) { emailMenu.Hide(); return; }
//            string localPart = text.Substring(0, atIndex); string domainFragment = text.Substring(atIndex + 1);
//            var suggestions = emailDomains
//                .Where(d => d.StartsWith(domainFragment, StringComparison.OrdinalIgnoreCase) && d.Length > domainFragment.Length)
//                .Select(d => $"{localPart}@{d}").Take(8).ToList();
//            if (suggestions.Count == 0) { emailMenu.Hide(); return; }
//            emailMenu.Items.Clear();
//            foreach (var suggestion in suggestions)
//            {
//                var menuItem = new ToolStripMenuItem(suggestion);
//                menuItem.Click += (sItem, _) =>
//                {
//                    tb.Text = suggestion; tb.SelectionStart = tb.Text.Length; tb.SelectionLength = 0; emailMenu.Hide();
//                };
//                emailMenu.Items.Add(menuItem);
//            }
//            var location = tb.PointToScreen(new Point(0, tb.Height)); Screen screen = Screen.FromControl(tb);
//            if (location.Y + emailMenu.PreferredSize.Height > screen.WorkingArea.Bottom) { location = tb.PointToScreen(new Point(0, -emailMenu.PreferredSize.Height)); }
//            if (location.X + emailMenu.Width > screen.WorkingArea.Right) { location.X = screen.WorkingArea.Right - emailMenu.Width; }
//            emailMenu.Width = tb.Width; emailMenu.Show(location);
//        }
//        #endregion

//        // Called from ucCustomer_Admin to load data
//        public void LoadData(EditorMode mode, int? customerId)
//        {
//            _mode = mode; errProvider.Clear(); ResetConfirmPasswordVisibility();
//            if (mode == EditorMode.Edit)
//            {
//                if (!customerId.HasValue) throw new ArgumentNullException(nameof(customerId));
//                _customerId = customerId.Value;
//                try
//                {
//                    var dto = _customerSvc.GetCustomerForEdit(_customerId);
//                    txtFullName.Text = dto.FullName; txtPhone.Text = dto.Phone; txtEmail.Text = dto.Email;
//                    txtAddress.Text = dto.Address; txtUsername.Text = dto.Username;
//                    txtPassword.Clear(); txtConfirmPassword.Clear();
//                    var parentForm = this.FindForm(); if (parentForm != null) parentForm.Text = "Chỉnh Sửa Khách Hàng";
//                    lblConfirmPassLabel.Visible = false; txtConfirmPassword.Visible = false;
//                }
//                catch (Exception ex) { MessageBox.Show($"Lỗi tải thông tin: {ex.Message}", "Lỗi"); }
//            }
//            else
//            {
//                txtFullName.Clear(); txtPhone.Clear(); txtEmail.Clear(); txtAddress.Clear();
//                txtUsername.Clear(); txtPassword.Clear(); txtConfirmPassword.Clear();
//                var parentForm = this.FindForm(); if (parentForm != null) parentForm.Text = "Thêm Khách Hàng Mới";
//                lblConfirmPassLabel.Visible = true; txtConfirmPassword.Visible = true;
//            }
//        }

//        private void WireEvents()
//        {

//            btnSave.Click += (s, e) => Save();
//            btnCancel.Click += (s, e) => Cancel();
//            txtPassword.TextChanged += TxtPassword_TextChanged;

//            // --- (8) GÁN SỰ KIỆN KÉO THẢ ---
//            // Chọn control làm tay nắm kéo, ví dụ: grpInfo hoặc 1 Panel riêng tên pnlTopDrag
//            Control dragHandle = grpInfo; // Hoặc this.pnlTopDrag nếu bạn tạo nó
//            if (dragHandle != null)
//            {
//                dragHandle.MouseDown += DragHandle_MouseDown;
//                dragHandle.MouseMove += DragHandle_MouseMove;
//                dragHandle.MouseUp += DragHandle_MouseUp;
//                // Quan trọng: Gắn cả vào các control con trên title bar của GroupBox nếu dùng GroupBox
//                // Ví dụ nếu grpInfo có Label tiêu đề:
//                // grpInfo.Controls.OfType<Label>().FirstOrDefault()?.MouseDown += DragHandle_MouseDown;
//                // grpInfo.Controls.OfType<Label>().FirstOrDefault()?.MouseMove += DragHandle_MouseMove;
//                // grpInfo.Controls.OfType<Label>().FirstOrDefault()?.MouseUp += DragHandle_MouseUp;
//            }
//            // --- KẾT THÚC GÁN SỰ KIỆN KÉO THẢ ---
//        }

//        // Show/Hide Confirm Password controls based on Password input in Edit mode
//        private void TxtPassword_TextChanged(object sender, EventArgs e)
//        {
//            if (_mode == EditorMode.Edit)
//            {
//                bool showConfirm = !string.IsNullOrWhiteSpace(txtPassword.Text);
//                lblConfirmPassLabel.Visible = showConfirm;
//                txtConfirmPassword.Visible = showConfirm;
//                if (!showConfirm) { txtConfirmPassword.Clear(); errProvider.SetError(txtConfirmPassword, ""); }
//            }
//        }

//        // Reset visibility for LoadData
//        private void ResetConfirmPasswordVisibility()
//        {
//            bool isAddMode = (_mode == EditorMode.Add);
//            lblConfirmPassLabel.Visible = isAddMode;
//            txtConfirmPassword.Visible = isAddMode;
//        }

//        private bool ValidateInput()
//        {
//            errProvider.Clear(); bool isValid = true;
//            if (string.IsNullOrWhiteSpace(txtFullName.Text)) { errProvider.SetError(txtFullName, "Họ tên trống."); isValid = false; }
//            if (string.IsNullOrWhiteSpace(txtUsername.Text)) { errProvider.SetError(txtUsername, "Tên đăng nhập trống."); isValid = false; }
//            else if (!Regex.IsMatch(txtUsername.Text.Trim(), @"^[a-zA-Z0-9_]{8,16}$")) { errProvider.SetError(txtUsername, "Tên đăng nhập 8-16 ký tự (a-z, 0-9, _)."); isValid = false; } // Thêm regex username
//            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) { errProvider.SetError(txtEmail, "Email không hợp lệ."); isValid = false; }
//            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !Regex.IsMatch(txtPhone.Text.Trim(), @"^\d{9,15}$")) { errProvider.SetError(txtPhone, "SĐT không hợp lệ (9-15 số)."); isValid = false; }

//            string pass = txtPassword.Text; string confirmPass = txtConfirmPassword.Text;
//            if (_mode == EditorMode.Add)
//            {
//                if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6) { errProvider.SetError(txtPassword, "Mật khẩu >= 6 ký tự."); isValid = false; }
//                else if (pass != confirmPass) { errProvider.SetError(txtConfirmPassword, "Xác nhận mật khẩu sai."); isValid = false; }
//            }
//            else
//            {
//                if (!string.IsNullOrWhiteSpace(pass))
//                {
//                    if (pass.Length < 6) { errProvider.SetError(txtPassword, "Mật khẩu mới >= 6 ký tự."); isValid = false; }
//                    else if (lblConfirmPassLabel.Visible && pass != confirmPass) { errProvider.SetError(txtConfirmPassword, "Xác nhận mật khẩu sai."); isValid = false; }
//                }
//                else if (lblConfirmPassLabel.Visible && !string.IsNullOrWhiteSpace(confirmPass)) { errProvider.SetError(txtPassword, "Nhập mật khẩu mới trước."); isValid = false; }
//            }
//            return isValid;
//        }

//        private void Save()
//        {
//            if (!ValidateInput()) { MessageBox.Show("Kiểm tra lại thông tin.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
//            var confirm = MessageBox.Show("Lưu thông tin này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//            if (confirm != DialogResult.Yes) return;

//            var dto = new CustomerEditorDto { /* ... tạo dto ... */ };
//            try
//            {
//                if (_mode == EditorMode.Add) _customerSvc.CreateCustomerAndAccount(dto);
//                else _customerSvc.UpdateCustomerAndAccount(dto);
//                MessageBox.Show("Lưu thành công!", "Thông báo");
//                var parentForm = this.FindForm(); if (parentForm != null) { parentForm.DialogResult = DialogResult.OK; parentForm.Close(); }
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi"); }
//        }

//        private void Cancel()
//        {
//            // Kiểm tra xem có thay đổi chưa lưu không (đơn giản)
//            bool hasChanges = false;
//            if (_mode == EditorMode.Add)
//            {
//                hasChanges = Controls.OfType<Guna2TextBox>().Any(tb => !string.IsNullOrWhiteSpace(tb.Text));
//            }
//            else
//            {
//                // Khi Edit, cần so sánh với dữ liệu gốc (phức tạp hơn, tạm bỏ qua check này khi Edit)
//                // hasChanges = ...;
//            }

//            DialogResult confirm = DialogResult.Yes; // Mặc định là Yes nếu không có thay đổi
//            if (hasChanges || _mode == EditorMode.Edit)
//            { // Luôn hỏi khi Edit hoặc khi Add có thay đổi
//                confirm = MessageBox.Show("Hủy bỏ thay đổi và đóng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//            }

//            if (confirm == DialogResult.Yes)
//            {
//                var parentForm = this.FindForm();
//                if (parentForm != null) { parentForm.DialogResult = DialogResult.Cancel; parentForm.Close(); }
//            }
//        }

//        #region --- (9) THÊM 3 HÀM XỬ LÝ KÉO THẢ FORM CHA ---
//        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left)
//            {
//                Form parentForm = this.FindForm(); // Tìm form cha
//                if (parentForm != null)
//                {
//                    isDragging = true;
//                    dragStartPoint = Cursor.Position; // Tọa độ chuột tuyệt đối
//                    parentFormStartPoint = parentForm.Location; // Tọa độ form cha
//                }
//            }
//        }

//        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (isDragging)
//            {
//                Form parentForm = this.FindForm();
//                if (parentForm != null)
//                {
//                    // Tính khoảng cách chuột đã di chuyển
//                    Point diff = Point.Subtract(Cursor.Position, new Size(dragStartPoint));
//                    // Cập nhật vị trí form cha
//                    parentForm.Location = Point.Add(parentFormStartPoint, new Size(diff));
//                }
//            }
//        }

//        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left)
//            {
//                isDragging = false; // Dừng kéo
//                dragStartPoint = Point.Empty;
//                parentFormStartPoint = Point.Empty;
//            }
//        }
//        #endregion --- KẾT THÚC HÀM KÉO THẢ ---
//    }
//}

using Guna.UI2.WinForms;
using LMS.BUS.Dtos; // Đảm bảo DTOs nằm trong namespace này hoặc thêm using
using LMS.BUS.Services;
using System;
using System.Drawing; // Cần cho Point, Size, Font, Color
using System.Linq; // Cần cho Linq trong gợi ý email
using System.Text.RegularExpressions; // Cần cho Regex
using System.Windows.Forms;

namespace LMS.GUI.CustomerAdmin // Đảm bảo namespace này đúng
{
    public partial class ucCustomerEditor_Admin : UserControl
    {
        public enum EditorMode { Add, Edit }

        private EditorMode _mode = EditorMode.Add;
        private int _customerId = 0;
        private readonly CustomerService _customerSvc = new CustomerService();
        private ErrorProvider errProvider;
        private CustomerEditorDto _originalData; // <-- BIẾN LƯU DỮ LIỆU GỐC

        // --- Biến cho Tooltip và Gợi ý Email ---
        private readonly Guna2HtmlToolTip htip = new Guna2HtmlToolTip();
        private readonly ContextMenuStrip emailMenu = new ContextMenuStrip();
        private readonly string[] emailDomains = {
      "gmail.com", "yahoo.com", "outlook.com", "hotmail.com",
      "icloud.com", "student.edu.vn", "company.com"
    };
        // --- Kết thúc biến ---

        // --- (1) BIẾN ĐỂ KÉO THẢ FORM CHA ---
        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;
        private Point parentFormStartPoint = Point.Empty;
        // --- KẾT THÚC BIẾN KÉO THẢ ---

        public ucCustomerEditor_Admin()
        {
            InitializeComponent();

            this.errProvider = new System.Windows.Forms.ErrorProvider();
            this.errProvider.ContainerControl = this;

            ConfigureTooltip();
            SetupFieldTips();
            ConfigureEmailSuggestMenu();
            WireEvents();

            this.Load += (s, e) => { txtFullName.Focus(); };
        }

        #region Tooltip and Email Suggest Configuration
        // --- Hàm ConfigureTooltip(), SetupFieldTips(), AttachHtmlTip() ---
        private void ConfigureTooltip()
        {
            htip.TitleFont = new Font("Segoe UI", 9, FontStyle.Bold);
            htip.TitleForeColor = Color.FromArgb(31, 113, 185);
            htip.ForeColor = Color.Black; htip.BackColor = Color.White;
            htip.BorderColor = Color.FromArgb(210, 210, 210); htip.MaximumSize = new Size(300, 0);
            htip.InitialDelay = 150; htip.AutoPopDelay = 30000; htip.ReshowDelay = 100;
            htip.ShowAlways = true; htip.UseAnimation = false; htip.UseFading = false;
        }
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
        private void AttachHtmlTip(Control ctl, string html)
        {
            ctl.Enter += (sender, e) =>
            {
                try
                {
                    var ttSize = TextRenderer.MeasureText(Regex.Replace(html, "<.*?>", string.Empty), SystemFonts.DefaultFont);
                    int estWidth = Math.Min(htip.MaximumSize.Width, ttSize.Width + 24); int estHeight = Math.Max(26, ttSize.Height + 14);
                    int x = ctl.Width + 6; int y = (ctl.Height - estHeight) / 2 - 2;
                    Point scr = ctl.PointToScreen(new Point(x, y)); Screen screen = Screen.FromControl(this);
                    if (scr.X + estWidth > screen.WorkingArea.Right) x = -estWidth - 8;
                    scr = ctl.PointToScreen(new Point(x, y));
                    if (scr.Y < screen.WorkingArea.Top) y += (screen.WorkingArea.Top - scr.Y);
                    else if (scr.Y + estHeight > screen.WorkingArea.Bottom) y -= (scr.Y + estHeight - screen.WorkingArea.Bottom);
                    htip.Show(html, ctl, x, y, htip.AutoPopDelay);
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Error showing tooltip for {ctl.Name}: {ex.Message}"); }
            };
            ctl.Leave += (sender, e) => htip.Hide(ctl);
            ctl.EnabledChanged += (sender, e) => { if (!ctl.Enabled) htip.Hide(ctl); };
            ctl.VisibleChanged += (sender, e) => { if (!ctl.Visible) htip.Hide(ctl); };

            // Sửa lỗi: Cần kiểm tra this.IsHandleCreated trước khi gọi FindForm
            this.HandleCreated += (s, e) =>
            {
                var parentForm = this.FindForm();
                if (parentForm != null) { parentForm.FormClosing -= ParentForm_FormClosing; parentForm.FormClosing += ParentForm_FormClosing; }
            };
        }
        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e) { htip.Hide(this); }

        // --- Hàm ConfigureEmailSuggestMenu(), TxtEmail_TextChanged() ---
        private void ConfigureEmailSuggestMenu()
        {
            emailMenu.ShowImageMargin = false; emailMenu.Font = new Font("Segoe UI", 9);
            if (txtEmail != null)
            {
                emailMenu.MaximumSize = new Size(txtEmail.Width, 220);
                txtEmail.TextChanged += TxtEmail_TextChanged;
                txtEmail.LostFocus += (s, e) => { if (!emailMenu.Focused && !txtEmail.Focused) emailMenu.Hide(); };
                txtEmail.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape && emailMenu.Visible) { emailMenu.Hide(); e.SuppressKeyPress = true; } };
                txtEmail.SizeChanged += (s, e) => { emailMenu.MaximumSize = new Size(txtEmail.Width, 220); };
            }
        }
        private void TxtEmail_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as Guna2TextBox; if (tb == null) return;
            var text = tb.Text; int atIndex = text.IndexOf('@');
            if (atIndex <= 0) { emailMenu.Hide(); return; }
            string localPart = text.Substring(0, atIndex); string domainFragment = text.Substring(atIndex + 1);
            var suggestions = emailDomains
              .Where(d => d.StartsWith(domainFragment, StringComparison.OrdinalIgnoreCase) && d.Length > domainFragment.Length)
              .Select(d => $"{localPart}@{d}").Take(8).ToList();
            if (suggestions.Count == 0) { emailMenu.Hide(); return; }
            emailMenu.Items.Clear();
            foreach (var suggestion in suggestions)
            {
                var menuItem = new ToolStripMenuItem(suggestion);
                menuItem.Click += (sItem, _) =>
                {
                    tb.Text = suggestion; tb.SelectionStart = tb.Text.Length; tb.SelectionLength = 0; emailMenu.Hide();
                };
                emailMenu.Items.Add(menuItem);
            }
            var location = tb.PointToScreen(new Point(0, tb.Height)); Screen screen = Screen.FromControl(tb);
            if (location.Y + emailMenu.PreferredSize.Height > screen.WorkingArea.Bottom) { location = tb.PointToScreen(new Point(0, -emailMenu.PreferredSize.Height)); }
            if (location.X + emailMenu.Width > screen.WorkingArea.Right) { location.X = screen.WorkingArea.Right - emailMenu.Width; }
            emailMenu.Width = tb.Width; emailMenu.Show(location);
        }
        #endregion

        // --- (CẬP NHẬT) ---
        public void LoadData(EditorMode mode, int? customerId)
        {
            _mode = mode; errProvider.Clear(); ResetConfirmPasswordVisibility();
            if (mode == EditorMode.Edit)
            {
                if (!customerId.HasValue) throw new ArgumentNullException(nameof(customerId));
                _customerId = customerId.Value;
                try
                {
                    var dto = _customerSvc.GetCustomerForEdit(_customerId);
                    _originalData = dto; // <-- LƯU DỮ LIỆU GỐC

                    txtFullName.Text = dto.FullName; txtPhone.Text = dto.Phone; txtEmail.Text = dto.Email;
                    txtAddress.Text = dto.Address; txtUsername.Text = dto.Username;
                    txtPassword.Clear(); txtConfirmPassword.Clear();
                    var parentForm = this.FindForm(); if (parentForm != null) parentForm.Text = "Chỉnh Sửa Khách Hàng";
                    lblConfirmPassLabel.Visible = false; txtConfirmPassword.Visible = false;
                }
                catch (Exception ex) { MessageBox.Show($"Lỗi tải thông tin: {ex.Message}", "Lỗi"); }
            }
            else
            {
                _originalData = new CustomerEditorDto(); // <-- KHỞI TẠO DỮ LIỆU GỐC RỖNG

                txtFullName.Clear(); txtPhone.Clear(); txtEmail.Clear(); txtAddress.Clear();
                txtUsername.Clear(); txtPassword.Clear(); txtConfirmPassword.Clear();
                var parentForm = this.FindForm(); if (parentForm != null) parentForm.Text = "Thêm Khách Hàng Mới";
                lblConfirmPassLabel.Visible = true; txtConfirmPassword.Visible = true;
            }
        }

        private void WireEvents()
        {
            btnSave.Click += (s, e) => Save();
            btnCancel.Click += (s, e) => Cancel();
            txtPassword.TextChanged += TxtPassword_TextChanged;

            // --- (8) GÁN SỰ KIỆN KÉO THẢ ---
            Control dragHandle = grpInfo;
            if (dragHandle != null)
            {
                dragHandle.MouseDown += DragHandle_MouseDown;
                dragHandle.MouseMove += DragHandle_MouseMove;
                dragHandle.MouseUp += DragHandle_MouseUp;
            }
            // --- KẾT THÚC GÁN SỰ KIỆN KÉO THẢ ---
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            if (_mode == EditorMode.Edit)
            {
                bool showConfirm = !string.IsNullOrWhiteSpace(txtPassword.Text);
                lblConfirmPassLabel.Visible = showConfirm;
                txtConfirmPassword.Visible = showConfirm;
                if (!showConfirm) { txtConfirmPassword.Clear(); errProvider.SetError(txtConfirmPassword, ""); }
            }
        }

        private void ResetConfirmPasswordVisibility()
        {
            bool isAddMode = (_mode == EditorMode.Add);
            lblConfirmPassLabel.Visible = isAddMode;
            txtConfirmPassword.Visible = isAddMode;
        }

        private bool ValidateInput()
        {
            errProvider.Clear(); bool isValid = true;
            if (string.IsNullOrWhiteSpace(txtFullName.Text)) { errProvider.SetError(txtFullName, "Họ tên trống."); isValid = false; }
            if (string.IsNullOrWhiteSpace(txtUsername.Text)) { errProvider.SetError(txtUsername, "Tên đăng nhập trống."); isValid = false; }
            else if (!Regex.IsMatch(txtUsername.Text.Trim(), @"^[a-zA-Z0-9_]{8,16}$")) { errProvider.SetError(txtUsername, "Tên đăng nhập 8-16 ký tự (a-z, 0-9, _)."); isValid = false; }
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) { errProvider.SetError(txtEmail, "Email không hợp lệ."); isValid = false; }
            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !Regex.IsMatch(txtPhone.Text.Trim(), @"^\d{9,15}$")) { errProvider.SetError(txtPhone, "SĐT không hợp lệ (9-15 số)."); isValid = false; }

            string pass = txtPassword.Text; string confirmPass = txtConfirmPassword.Text;
            if (_mode == EditorMode.Add)
            {
                if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6) { errProvider.SetError(txtPassword, "Mật khẩu >= 6 ký tự."); isValid = false; }
                else if (pass != confirmPass) { errProvider.SetError(txtConfirmPassword, "Xác nhận mật khẩu sai."); isValid = false; }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(pass))
                {
                    if (pass.Length < 6) { errProvider.SetError(txtPassword, "Mật khẩu mới >= 6 ký tự."); isValid = false; }
                    else if (lblConfirmPassLabel.Visible && pass != confirmPass) { errProvider.SetError(txtConfirmPassword, "Xác nhận mật khẩu sai."); isValid = false; }
                }
                else if (lblConfirmPassLabel.Visible && !string.IsNullOrWhiteSpace(confirmPass)) { errProvider.SetError(txtPassword, "Nhập mật khẩu mới trước."); isValid = false; }
            }
            return isValid;
        }

        // --- (THÊM HÀM NÀY) ---
        /// <summary>
        /// Kiểm tra xem dữ liệu trên form có thay đổi so với bản gốc không.
        /// </summary>
        private bool HasChanges()
        {
            if (_mode == EditorMode.Add)
            {
                // Ở chế độ Add, chỉ cần 1 ô có dữ liệu là tính đã thay đổi
                return !string.IsNullOrWhiteSpace(txtFullName.Text) ||
           !string.IsNullOrWhiteSpace(txtPhone.Text) ||
           !string.IsNullOrWhiteSpace(txtEmail.Text) ||
           !string.IsNullOrWhiteSpace(txtAddress.Text) ||
           !string.IsNullOrWhiteSpace(txtUsername.Text) ||
           !string.IsNullOrWhiteSpace(txtPassword.Text);
            }
            else // Chế độ Edit
            {
                if (_originalData == null) return false; // Không có dữ liệu gốc để so sánh

                // So sánh từng trường (dùng ?? "" để xử lý null từ DB)
                return (_originalData.FullName ?? "") != txtFullName.Text.Trim() ||
           (_originalData.Phone ?? "") != txtPhone.Text.Trim() ||
           (_originalData.Email ?? "") != txtEmail.Text.Trim() ||
           (_originalData.Address ?? "") != txtAddress.Text.Trim() ||
           (_originalData.Username ?? "") != txtUsername.Text.Trim() ||
           !string.IsNullOrWhiteSpace(txtPassword.Text); // Nếu có nhập mật khẩu mới -> là thay đổi
            }
        }

        // --- (CẬP NHẬT) ---
        private void Save()
        {
            // 1. Kiểm tra hợp lệ
            if (!ValidateInput())
            {
                MessageBox.Show("Vui lòng kiểm tra lại thông tin đã nhập.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Hỏi xác nhận lưu
            var confirm = MessageBox.Show("Bạn có chắc muốn lưu thông tin này?", "Xác nhận lưu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            // 3. Tạo DTO (Sửa lại code của bạn cho đúng)
            var dto = new CustomerEditorDto
            {
                Id = _customerId, // Sẽ là 0 ở chế độ Add
                FullName = txtFullName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text // Service sẽ xử lý nếu mật khẩu rỗng khi Edit
            };

            // 4. Thực thi lưu
            try
            {
                if (_mode == EditorMode.Add)
                    _customerSvc.CreateCustomerAndAccount(dto);
                else
                    _customerSvc.UpdateCustomerAndAccount(dto);

                MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.DialogResult = DialogResult.OK;
                    parentForm.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- (CẬP NHẬT) ---
        private void Cancel()
        {
            // 1. Kiểm tra xem có thay đổi không
            if (HasChanges())
            {
                // 2. Nếu có thay đổi -> Hỏi xác nhận
                var confirm = MessageBox.Show("Bạn có thay đổi chưa lưu. Bạn có chắc muốn hủy bỏ và đóng?", "Xác nhận hủy",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Nếu người dùng không đồng ý (nhấn No), thì không làm gì cả
                if (confirm != DialogResult.Yes)
                    return;
            }

            // 3. Nếu không có thay đổi (hoặc người dùng đã đồng ý hủy) -> Đóng form
            var parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.DialogResult = DialogResult.Cancel;
                parentForm.Close();
            }
        }


        #region --- (9) 3 HÀM XỬ LÝ KÉO THẢ FORM CHA ---
        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Form parentForm = this.FindForm();
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
                Form parentForm = this.FindForm();
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
        #endregion --- KẾT THÚC HÀM KÉO THẢ ---
    }
}