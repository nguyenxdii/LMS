//// LMS.GUI/DriverAdmin/ucDriverEditor_Admin.cs
//using Guna.UI2.WinForms;
//using LMS.BUS.Dtos;
//using LMS.BUS.Services;
//using System;
//using System.Drawing;
//using System.Linq;
//using System.Text.RegularExpressions; // Cần cho Regex
//using System.Windows.Forms;

//namespace LMS.GUI.DriverAdmin // Đổi namespace
//{
//    public partial class ucDriverEditor_Admin : UserControl
//    {
//        public enum EditorMode { Add, Edit }

//        private EditorMode _mode = EditorMode.Add;
//        private int _driverId = 0;
//        private readonly DriverService _driverSvc = new DriverService();
//        private ErrorProvider errProvider;
//        private DriverEditorDto _originalData;

//        // --- Biến cho Tooltip ---
//        private readonly Guna2HtmlToolTip htip = new Guna2HtmlToolTip();

//        // --- Biến cho kéo thả Form Cha ---
//        private bool isDragging = false;
//        private Point dragStartPoint = Point.Empty;
//        private Point parentFormStartPoint = Point.Empty;

//        public ucDriverEditor_Admin()
//        {
//            InitializeComponent();

//            this.errProvider = new System.Windows.Forms.ErrorProvider();
//            this.errProvider.ContainerControl = this;

//            ConfigureTooltip();
//            SetupFieldTips();
//            ConfigureLicenseTypeCombo(); // Cấu hình ComboBox Bằng Lái
//            WireEvents();

//            this.Load += (s, e) => { txtFullName.Focus(); }; // Giả sử tên là txtFullName
//        }

//        #region Tooltip Configuration
//        private void ConfigureTooltip()
//        {
//            htip.TitleFont = new Font("Segoe UI", 9, FontStyle.Bold);
//            htip.TitleForeColor = Color.FromArgb(31, 113, 185);
//            htip.ForeColor = Color.Black; htip.BackColor = Color.White;
//            htip.BorderColor = Color.FromArgb(210, 210, 210); htip.MaximumSize = new Size(300, 0);
//            htip.InitialDelay = 150; htip.AutoPopDelay = 30000; htip.ReshowDelay = 100;
//            htip.ShowAlways = true; htip.UseAnimation = false; htip.UseFading = false;
//        }

//        // Cấu hình ComboBox Bằng Lái
//        private void ConfigureLicenseTypeCombo()
//        {
//            if (cmbLicenseType != null) // Giả sử tên là cmbLicenseType
//            {
//                cmbLicenseType.DropDownStyle = ComboBoxStyle.DropDownList;
//                cmbLicenseType.Items.Clear();
//                cmbLicenseType.Items.AddRange(new object[] { "B2", "C", "CE", "D", "DE", "FC" });
//            }
//        }

//        private void SetupFieldTips()
//        {
//            // Đảm bảo các control (txtFullName, txtPhone,...) tồn tại trong Designer
//            AttachHtmlTip(txtFullName, "<b>Họ và tên tài xế</b><br/><i>Ví dụ: Nguyễn Văn A</i>");
//            AttachHtmlTip(txtPhone, "<b>Số điện thoại</b><br/><i>(9-15 chữ số). Ví dụ: 0912345678</i>");
//            AttachHtmlTip(txtCitizenId, "<b>Số CCCD (Citizen ID)</b><br/><i>(Yêu cầu 12 chữ số).</i>");
//            AttachHtmlTip(cmbLicenseType, "<b>Hạng Bằng Lái</b><br/><i>Chọn hạng GPLX phù hợp (B2, C, CE...).</i>");
//            AttachHtmlTip(txtUsername, "<b>Tên đăng nhập</b><br/><i>(8-16 ký tự, chỉ gồm chữ cái, số, dấu gạch dưới '_').</i>");
//            AttachHtmlTip(txtPassword, "<b>Mật khẩu</b><br/><i>(Ít nhất 6 ký tự).<br/>Khi sửa: Để trống nếu không muốn thay đổi.</i>");
//            AttachHtmlTip(txtConfirmPassword, "<b>Xác nhận mật khẩu</b><br/><i>Nhập lại mật khẩu giống ô trên.</i>");
//        }

//        // (Các hàm AttachHtmlTip, ParentForm_FormClosing giữ nguyên y hệt ucCustomerEditor_Admin)
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

//            this.HandleCreated += (s, e) =>
//            {
//                var parentForm = this.FindForm();
//                if (parentForm != null) { parentForm.FormClosing -= ParentForm_FormClosing; parentForm.FormClosing += ParentForm_FormClosing; }
//            };
//        }
//        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e) { htip.Hide(this); }

//        #endregion

//        public void LoadData(EditorMode mode, int? driverId)
//        {
//            _mode = mode; errProvider.Clear(); ResetConfirmPasswordVisibility();
//            if (mode == EditorMode.Edit)
//            {
//                if (!driverId.HasValue) throw new ArgumentNullException(nameof(driverId));
//                _driverId = driverId.Value;
//                try
//                {
//                    var dto = _driverSvc.GetDriverForEdit(_driverId);
//                    _originalData = dto;

//                    txtFullName.Text = dto.FullName;
//                    txtPhone.Text = dto.Phone;
//                    txtCitizenId.Text = dto.CitizenId; // Thêm
//                    cmbLicenseType.SelectedItem = dto.LicenseType; // Thêm
//                    txtUsername.Text = dto.Username;

//                    txtPassword.Clear(); txtConfirmPassword.Clear();
//                    var parentForm = this.FindForm(); if (parentForm != null) parentForm.Text = "Chỉnh Sửa Tài Xế";
//                    lblConfirmPassLabel.Visible = false; txtConfirmPassword.Visible = false;
//                }
//                catch (Exception ex) { MessageBox.Show($"Lỗi tải thông tin: {ex.Message}", "Lỗi"); }
//            }
//            else
//            {
//                _originalData = new DriverEditorDto();

//                txtFullName.Clear(); txtPhone.Clear(); txtCitizenId.Clear(); // Sửa
//                cmbLicenseType.SelectedIndex = -1; // Sửa
//                txtUsername.Clear(); txtPassword.Clear(); txtConfirmPassword.Clear();

//                var parentForm = this.FindForm(); if (parentForm != null) parentForm.Text = "Thêm Tài Xế Mới";
//                lblConfirmPassLabel.Visible = true; txtConfirmPassword.Visible = true;
//            }
//        }

//        private void WireEvents()
//        {
//            btnSave.Click += (s, e) => Save();
//            btnCancel.Click += (s, e) => Cancel();
//            txtPassword.TextChanged += TxtPassword_TextChanged;

//            // ==========================================================
//            // === (QUAN TRỌNG) ĐIỀU CHỈNH GÁN SỰ KIỆN KÉO THẢ TỪ grpInfo SANG pnlTop ===
//            Control dragHandle = pnlTop; // Thay đổi từ grpInfo sang pnlTop
//            if (dragHandle != null)
//            {
//                dragHandle.MouseDown += DragHandle_MouseDown;
//                dragHandle.MouseMove += DragHandle_MouseMove;
//                dragHandle.MouseUp += DragHandle_MouseUp;
//            }
//            // ==========================================================
//        }

//        // (Các hàm TxtPassword_TextChanged, ResetConfirmPasswordVisibility giữ nguyên)
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
//            else if (!Regex.IsMatch(txtUsername.Text.Trim(), @"^[a-zA-Z0-9_]{8,16}$")) { errProvider.SetError(txtUsername, "Tên đăng nhập 8-16 ký tự (a-z, 0-9, _)."); isValid = false; }

//            // Validate trường mới
//            if (!string.IsNullOrWhiteSpace(txtCitizenId.Text) && !Regex.IsMatch(txtCitizenId.Text.Trim(), @"^\d{12}$")) { errProvider.SetError(txtCitizenId, "CCCD phải là 12 chữ số."); isValid = false; }
//            if (cmbLicenseType.SelectedItem == null) { errProvider.SetError(cmbLicenseType, "Vui lòng chọn hạng bằng lái."); isValid = false; }

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
//            }
//            return isValid;
//        }

//        private bool HasChanges()
//        {
//            if (_mode == EditorMode.Add)
//            {
//                return !string.IsNullOrWhiteSpace(txtFullName.Text) ||
//                        !string.IsNullOrWhiteSpace(txtPhone.Text) ||
//                        !string.IsNullOrWhiteSpace(txtCitizenId.Text) ||
//                        (cmbLicenseType.SelectedIndex > -1) ||
//                        !string.IsNullOrWhiteSpace(txtUsername.Text) ||
//                        !string.IsNullOrWhiteSpace(txtPassword.Text);
//            }
//            else
//            {
//                if (_originalData == null) return false;
//                return (_originalData.FullName ?? "") != txtFullName.Text.Trim() ||
//                        (_originalData.Phone ?? "") != txtPhone.Text.Trim() ||
//                        (_originalData.CitizenId ?? "") != txtCitizenId.Text.Trim() ||
//                        (_originalData.LicenseType ?? "") != (cmbLicenseType.SelectedItem?.ToString() ?? "") ||
//                        (_originalData.Username ?? "") != txtUsername.Text.Trim() ||
//                        !string.IsNullOrWhiteSpace(txtPassword.Text);
//            }
//        }

//        private void Save()
//        {
//            if (!ValidateInput())
//            {
//                MessageBox.Show("Vui lòng kiểm tra lại thông tin đã nhập.", "Lỗi nhập liệu");
//                return;
//            }

//            var confirm = MessageBox.Show("Bạn có chắc muốn lưu thông tin này?", "Xác nhận lưu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//            if (confirm != DialogResult.Yes)
//                return;

//            var dto = new DriverEditorDto
//            {
//                Id = _driverId,
//                FullName = txtFullName.Text.Trim(),
//                Phone = txtPhone.Text.Trim(),
//                CitizenId = txtCitizenId.Text.Trim(),
//                LicenseType = cmbLicenseType.SelectedItem?.ToString(),
//                Username = txtUsername.Text.Trim(),
//                Password = txtPassword.Text
//            };

//            try
//            {
//                if (_mode == EditorMode.Add)
//                    _driverSvc.CreateDriverAndAccount(dto);
//                else
//                    _driverSvc.UpdateDriverAndAccount(dto);

//                MessageBox.Show("Lưu thành công!", "Thông báo");

//                var parentForm = this.FindForm();
//                if (parentForm != null)
//                {
//                    parentForm.DialogResult = DialogResult.OK;
//                    parentForm.Close();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi");
//            }
//        }

//        private void Cancel()
//        {
//            if (HasChanges())
//            {
//                var confirm = MessageBox.Show("Bạn có thay đổi chưa lưu. Bạn có chắc muốn hủy bỏ và đóng?", "Xác nhận hủy",
//                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

//                if (confirm != DialogResult.Yes)
//                    return;
//            }

//            var parentForm = this.FindForm();
//            if (parentForm != null)
//            {
//                parentForm.DialogResult = DialogResult.Cancel;
//                parentForm.Close();
//            }
//        }

//        #region --- 3 HÀM XỬ LÝ KÉO THẢ FORM CHA (Giữ nguyên) ---
//        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left)
//            {
//                Form parentForm = this.FindForm();
//                if (parentForm != null)
//                {
//                    isDragging = true;
//                    dragStartPoint = Cursor.Position;
//                    parentFormStartPoint = parentForm.Location;
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
//                    Point diff = Point.Subtract(Cursor.Position, new Size(dragStartPoint));
//                    parentForm.Location = Point.Add(parentFormStartPoint, new Size(diff));
//                }
//            }
//        }

//        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left)
//            {
//                isDragging = false;
//                dragStartPoint = Point.Empty;
//                parentFormStartPoint = Point.Empty;
//            }
//        }
//        #endregion
//    }
//}


// LMS.GUI/DriverAdmin/ucDriverEditor_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions; // Cần cho Regex
using System.Windows.Forms;

namespace LMS.GUI.DriverAdmin // Đổi namespace
{
    public partial class ucDriverEditor_Admin : UserControl
    {
        public enum EditorMode { Add, Edit }

        private EditorMode _mode = EditorMode.Add;
        private int _driverId = 0;
        private readonly DriverService _driverSvc = new DriverService();
        private ErrorProvider errProvider;
        private DriverEditorDto _originalData;

        private readonly Guna2HtmlToolTip htip = new Guna2HtmlToolTip();

        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;
        private Point parentFormStartPoint = Point.Empty;

        public ucDriverEditor_Admin()
        {
            InitializeComponent();
            this.errProvider = new System.Windows.Forms.ErrorProvider { ContainerControl = this };
            ConfigureTooltip();
            SetupFieldTips();
            ConfigureLicenseTypeCombo();
            WireEvents();
            this.Load += (s, e) => { txtFullName.Focus(); };
        }

        #region Tooltip Configuration
        // (Các hàm ConfigureTooltip, ConfigureLicenseTypeCombo, SetupFieldTips, AttachHtmlTip, ParentForm_FormClosing giữ nguyên)
        private void ConfigureTooltip()
        {
            htip.TitleFont = new Font("Segoe UI", 9, FontStyle.Bold);
            htip.TitleForeColor = Color.FromArgb(31, 113, 185);
            htip.ForeColor = Color.Black; htip.BackColor = Color.White;
            htip.BorderColor = Color.FromArgb(210, 210, 210); htip.MaximumSize = new Size(300, 0);
            htip.InitialDelay = 150; htip.AutoPopDelay = 30000; htip.ReshowDelay = 100;
            htip.ShowAlways = true; htip.UseAnimation = false; htip.UseFading = false;
        }
        private void ConfigureLicenseTypeCombo()
        {
            if (cmbLicenseType != null)
            {
                cmbLicenseType.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbLicenseType.Items.Clear();
                cmbLicenseType.Items.AddRange(new object[] { "B2", "C", "CE", "D", "DE", "FC" });
            }
        }
        private void SetupFieldTips()
        {
            AttachHtmlTip(txtFullName, "<b>Họ và tên tài xế</b><br/><i>Ví dụ: Nguyễn Văn A</i>");
            AttachHtmlTip(txtPhone, "<b>Số điện thoại</b><br/><i>(9-15 chữ số). Ví dụ: 0912345678</i>");
            AttachHtmlTip(txtCitizenId, "<b>Số CCCD (Citizen ID)</b><br/><i>(Yêu cầu 12 chữ số).</i>");
            AttachHtmlTip(cmbLicenseType, "<b>Hạng Bằng Lái</b><br/><i>Chọn hạng GPLX phù hợp (B2, C, CE...).</i>");
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
            this.HandleCreated += (s, e) =>
            {
                var parentForm = this.FindForm();
                if (parentForm != null) { parentForm.FormClosing -= ParentForm_FormClosing; parentForm.FormClosing += ParentForm_FormClosing; }
            };
        }
        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e) { htip.Hide(this); }
        #endregion

        public void LoadData(EditorMode mode, int? driverId)
        {
            _mode = mode;
            errProvider.Clear();
            ResetConfirmPasswordVisibility();

            // === THAY ĐỔI Ở ĐÂY ===
            // 1. Tạo Font chữ
            Font titleFont = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));

            // 2. Gán Font và Text cho lblTitle (Đảm bảo Label tên là lblTitle trong Designer)
            if (lblTitle != null)
            {
                lblTitle.Font = titleFont; // Gán Font

                if (mode == EditorMode.Edit)
                {
                    lblTitle.Text = "Sửa Thông Tin Tài Xế"; // Text cho Sửa
                    if (!driverId.HasValue) throw new ArgumentNullException(nameof(driverId));
                    _driverId = driverId.Value;
                    try
                    {
                        var dto = _driverSvc.GetDriverForEdit(_driverId);
                        _originalData = dto;

                        txtFullName.Text = dto.FullName;
                        txtPhone.Text = dto.Phone;
                        txtCitizenId.Text = dto.CitizenId;
                        cmbLicenseType.SelectedItem = dto.LicenseType;
                        txtUsername.Text = dto.Username;

                        txtPassword.Clear(); txtConfirmPassword.Clear();
                        // Bỏ dòng parentForm.Text
                        // var parentForm = this.FindForm(); if (parentForm != null) parentForm.Text = "Chỉnh Sửa Tài Xế";
                        lblConfirmPassLabel.Visible = false; txtConfirmPassword.Visible = false;
                    }
                    catch (Exception ex) { MessageBox.Show($"Lỗi tải thông tin: {ex.Message}", "Lỗi"); }
                }
                else // Chế độ Add
                {
                    lblTitle.Text = "Thêm Thông Tin Tài Xế"; // Text cho Thêm
                    _originalData = new DriverEditorDto();

                    txtFullName.Clear(); txtPhone.Clear(); txtCitizenId.Clear();
                    cmbLicenseType.SelectedIndex = -1;
                    txtUsername.Clear(); txtPassword.Clear(); txtConfirmPassword.Clear();
                    // Bỏ dòng parentForm.Text
                    // var parentForm = this.FindForm(); if (parentForm != null) parentForm.Text = "Thêm Tài Xế Mới";
                    lblConfirmPassLabel.Visible = true; txtConfirmPassword.Visible = true;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Cảnh báo: Không tìm thấy control lblTitle trong ucDriverEditor_Admin.");
            }
            // === KẾT THÚC THAY ĐỔI ===
        }

        private void WireEvents()
        {
            btnSave.Click += (s, e) => Save();
            btnCancel.Click += (s, e) => Cancel();
            txtPassword.TextChanged += TxtPassword_TextChanged;

            // Gán sự kiện kéo thả cho Panel Top (Giả sử tên là pnlTop)
            Control dragHandle = pnlTop; // Đổi từ grpInfo (nếu trước đó dùng grpInfo)
            if (dragHandle != null)
            {
                dragHandle.MouseDown += DragHandle_MouseDown;
                dragHandle.MouseMove += DragHandle_MouseMove;
                dragHandle.MouseUp += DragHandle_MouseUp;
            }
        }

        // (Các hàm TxtPassword_TextChanged, ResetConfirmPasswordVisibility, ValidateInput, HasChanges, Save, Cancel giữ nguyên)
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
            if (!string.IsNullOrWhiteSpace(txtCitizenId.Text) && !Regex.IsMatch(txtCitizenId.Text.Trim(), @"^\d{12}$")) { errProvider.SetError(txtCitizenId, "CCCD phải là 12 chữ số."); isValid = false; }
            if (cmbLicenseType.SelectedItem == null) { errProvider.SetError(cmbLicenseType, "Vui lòng chọn hạng bằng lái."); isValid = false; }
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
            }
            return isValid;
        }
        private bool HasChanges()
        {
            if (_mode == EditorMode.Add)
            {
                return !string.IsNullOrWhiteSpace(txtFullName.Text) || !string.IsNullOrWhiteSpace(txtPhone.Text) ||
                       !string.IsNullOrWhiteSpace(txtCitizenId.Text) || (cmbLicenseType.SelectedIndex > -1) ||
                       !string.IsNullOrWhiteSpace(txtUsername.Text) || !string.IsNullOrWhiteSpace(txtPassword.Text);
            }
            else
            {
                if (_originalData == null) return false;
                return (_originalData.FullName ?? "") != txtFullName.Text.Trim() || (_originalData.Phone ?? "") != txtPhone.Text.Trim() ||
                       (_originalData.CitizenId ?? "") != txtCitizenId.Text.Trim() || (_originalData.LicenseType ?? "") != (cmbLicenseType.SelectedItem?.ToString() ?? "") ||
                       (_originalData.Username ?? "") != txtUsername.Text.Trim() || !string.IsNullOrWhiteSpace(txtPassword.Text);
            }
        }
        private void Save()
        {
            if (!ValidateInput()) { MessageBox.Show("Vui lòng kiểm tra lại thông tin.", "Lỗi nhập liệu"); return; }
            var confirm = MessageBox.Show("Lưu thông tin này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;
            var dto = new DriverEditorDto
            {
                Id = _driverId,
                FullName = txtFullName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                CitizenId = txtCitizenId.Text.Trim(),
                LicenseType = cmbLicenseType.SelectedItem?.ToString(),
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text
            };
            try
            {
                if (_mode == EditorMode.Add) _driverSvc.CreateDriverAndAccount(dto);
                else _driverSvc.UpdateDriverAndAccount(dto);
                MessageBox.Show("Lưu thành công!", "Thông báo");
                var parentForm = this.FindForm();
                if (parentForm != null) { parentForm.DialogResult = DialogResult.OK; parentForm.Close(); }
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi"); }
        }
        private void Cancel()
        {
            if (HasChanges())
            {
                var confirm = MessageBox.Show("Có thay đổi chưa lưu. Hủy bỏ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;
            }
            var parentForm = this.FindForm();
            if (parentForm != null) { parentForm.DialogResult = DialogResult.Cancel; parentForm.Close(); }
        }

        #region Kéo thả Form Cha (Giữ nguyên)
        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Form parentForm = this.FindForm();
                if (parentForm != null) { isDragging = true; dragStartPoint = Cursor.Position; parentFormStartPoint = parentForm.Location; }
            }
        }
        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Form parentForm = this.FindForm();
                if (parentForm != null) { Point diff = Point.Subtract(Cursor.Position, new Size(dragStartPoint)); parentForm.Location = Point.Add(parentFormStartPoint, new Size(diff)); }
            }
        }
        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { isDragging = false; dragStartPoint = Point.Empty; parentFormStartPoint = Point.Empty; }
        }
        #endregion
    }
}