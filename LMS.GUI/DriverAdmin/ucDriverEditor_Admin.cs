using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Services;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LMS.GUI.DriverAdmin
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

        // kéo thả form cha qua pnlTop
        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;
        private Point parentFormStartPoint = Point.Empty;

        public ucDriverEditor_Admin()
        {
            InitializeComponent();
            errProvider = new ErrorProvider { ContainerControl = this };
            ConfigureTooltip();
            SetupFieldTips();
            ConfigureLicenseTypeCombo();
            WireEvents();
            this.Load += (s, e) => { txtFullName.Focus(); };
        }

        // cấu hình tooltip
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

        // danh sách hạng gplx
        private void ConfigureLicenseTypeCombo()
        {
            if (cmbLicenseType == null) return;
            cmbLicenseType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLicenseType.Items.Clear();
            cmbLicenseType.Items.AddRange(new object[] { "B2", "C", "CE", "D", "DE", "FC" });
        }

        // tooltip cho từng trường
        private void SetupFieldTips()
        {
            AttachHtmlTip(txtFullName, "<b>Họ và tên tài xế</b><br/><i>Ví dụ: Nguyễn Văn A</i>");
            AttachHtmlTip(txtPhone, "<b>Số điện thoại</b><br/><i>(9-15 chữ số). Ví dụ: 0912345678</i>");
            AttachHtmlTip(txtCitizenId, "<b>Số CCCD</b><br/><i>yêu cầu 12 chữ số</i>");
            AttachHtmlTip(cmbLicenseType, "<b>Hạng bằng lái</b><br/><i>chọn B2, C, CE...</i>");
            AttachHtmlTip(txtUsername, "<b>Tên đăng nhập</b><br/><i>8-16 ký tự: a-z, 0-9, _</i>");
            AttachHtmlTip(txtPassword, "<b>Mật khẩu</b><br/><i>ít nhất 6 ký tự; khi sửa có thể để trống</i>");
            AttachHtmlTip(txtConfirmPassword, "<b>Xác nhận mật khẩu</b><br/><i>nhập trùng mật khẩu</i>");
        }

        // gắn tooltip html
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

                    var screen = Screen.FromControl(this).WorkingArea;
                    var scr = ctl.PointToScreen(new Point(x, y));
                    if (scr.X + estWidth > screen.Right) x = -estWidth - 8;
                    scr = ctl.PointToScreen(new Point(x, y));
                    if (scr.Y < screen.Top) y += (screen.Top - scr.Y);
                    else if (scr.Y + estHeight > screen.Bottom) y -= (scr.Y + estHeight - screen.Bottom);

                    htip.Show(html, ctl, x, y, htip.AutoPopDelay);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"tooltip error {ctl.Name}: {ex.Message}");
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
        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e) => htip.Hide(this);

        // nạp dữ liệu cho chế độ thêm/sửa
        public void LoadData(EditorMode mode, int? driverId)
        {
            _mode = mode;
            errProvider.Clear();
            ResetConfirmPasswordVisibility();

            var titleFont = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            if (lblTitle != null)
            {
                lblTitle.Font = titleFont;

                if (mode == EditorMode.Edit)
                {
                    lblTitle.Text = "Sửa Thông Tin Tài Xế";
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

                        txtPassword.Clear();
                        txtConfirmPassword.Clear();
                        lblConfirmPassLabel.Visible = false;
                        txtConfirmPassword.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi tải thông tin: {ex.Message}", "Lỗi");
                    }
                }
                else
                {
                    lblTitle.Text = "Thêm Thông Tin Tài Xế";
                    _originalData = new DriverEditorDto();

                    txtFullName.Clear();
                    txtPhone.Clear();
                    txtCitizenId.Clear();
                    cmbLicenseType.SelectedIndex = -1;
                    txtUsername.Clear();
                    txtPassword.Clear();
                    txtConfirmPassword.Clear();
                    lblConfirmPassLabel.Visible = true;
                    txtConfirmPassword.Visible = true;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("không tìm thấy lblTitle trong ucDriverEditor_Admin");
            }
        }

        // gắn sự kiện nút và kéo thả
        private void WireEvents()
        {
            btnSave.Click += (s, e) => Save();
            btnCancel.Click += (s, e) => Cancel();
            txtPassword.TextChanged += TxtPassword_TextChanged;

            var dragHandle = pnlTop; // panel tiêu đề
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
            if (_mode != EditorMode.Edit) return;

            bool showConfirm = !string.IsNullOrWhiteSpace(txtPassword.Text);
            lblConfirmPassLabel.Visible = showConfirm;
            txtConfirmPassword.Visible = showConfirm;
            if (!showConfirm)
            {
                txtConfirmPassword.Clear();
                errProvider.SetError(txtConfirmPassword, "");
            }
        }

        // đặt lại trạng thái xác nhận mật khẩu theo mode
        private void ResetConfirmPasswordVisibility()
        {
            bool isAddMode = (_mode == EditorMode.Add);
            lblConfirmPassLabel.Visible = isAddMode;
            txtConfirmPassword.Visible = isAddMode;
        }

        // validate dữ liệu đầu vào
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

            if (!string.IsNullOrWhiteSpace(txtCitizenId.Text) && !Regex.IsMatch(txtCitizenId.Text.Trim(), @"^\d{12}$"))
            {
                errProvider.SetError(txtCitizenId, "CCCD phải là 12 chữ số.");
                isValid = false;
            }

            if (cmbLicenseType.SelectedItem == null)
            {
                errProvider.SetError(cmbLicenseType, "Vui lòng chọn hạng bằng lái.");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !Regex.IsMatch(txtPhone.Text.Trim(), @"^\d{9,15}$"))
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

        // phát hiện thay đổi để confirm khi hủy
        private bool HasChanges()
        {
            if (_mode == EditorMode.Add)
            {
                return !string.IsNullOrWhiteSpace(txtFullName.Text)
                    || !string.IsNullOrWhiteSpace(txtPhone.Text)
                    || !string.IsNullOrWhiteSpace(txtCitizenId.Text)
                    || (cmbLicenseType.SelectedIndex > -1)
                    || !string.IsNullOrWhiteSpace(txtUsername.Text)
                    || !string.IsNullOrWhiteSpace(txtPassword.Text);
            }
            else
            {
                if (_originalData == null) return false;

                return (_originalData.FullName ?? "") != txtFullName.Text.Trim()
                    || (_originalData.Phone ?? "") != txtPhone.Text.Trim()
                    || (_originalData.CitizenId ?? "") != txtCitizenId.Text.Trim()
                    || (_originalData.LicenseType ?? "") != (cmbLicenseType.SelectedItem?.ToString() ?? "")
                    || (_originalData.Username ?? "") != txtUsername.Text.Trim()
                    || !string.IsNullOrWhiteSpace(txtPassword.Text);
            }
        }

        // lưu dữ liệu
        private void Save()
        {
            if (!ValidateInput())
            {
                MessageBox.Show("Vui lòng kiểm tra lại thông tin.", "Lỗi nhập liệu");
                return;
            }

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
                if (parentForm != null)
                {
                    parentForm.DialogResult = DialogResult.OK;
                    parentForm.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi");
            }
        }

        // hủy và đóng popup
        private void Cancel()
        {
            if (HasChanges())
            {
                var confirm = MessageBox.Show("Có thay đổi chưa lưu. Hủy bỏ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;
            }

            var parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.DialogResult = DialogResult.Cancel;
                parentForm.Close();
            }
        }

        // xử lý kéo thả form cha qua pnlTop
        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var parentForm = this.FindForm();
            if (parentForm == null) return;

            isDragging = true;
            dragStartPoint = Cursor.Position;
            parentFormStartPoint = parentForm.Location;
        }

        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;
            var parentForm = this.FindForm();
            if (parentForm == null) return;

            Point diff = Point.Subtract(Cursor.Position, new Size(dragStartPoint));
            parentForm.Location = Point.Add(parentFormStartPoint, new Size(diff));
        }

        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            isDragging = false;
            dragStartPoint = Point.Empty;
            parentFormStartPoint = Point.Empty;
        }
    }
}
