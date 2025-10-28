// LMS.GUI.ProfileDriver.ucDriverProfile.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos; // Cần cho DriverProfileDto
using LMS.BUS.Services; // Cần cho DriverService
using LMS.DAL.Models; // Cần cho UserAccount, Driver
using System;
using System.Drawing;
using System.IO; // Cần cho MemoryStream
using System.Windows.Forms;
using LMS.BUS.Helpers; // Cần cho AppSession

namespace LMS.GUI.ProfileUser
{
    public partial class ucDriverProfile : UserControl
    {
        // === Service và Dữ liệu ===
        private readonly DriverService _driverSvc; // Sử dụng DriverService
        private UserAccount _currentUserAccount;
        private Driver _currentDriver; // Thay Customer bằng Driver
        private EditMode _currentMode = EditMode.Viewing;

        // === Các hằng số vị trí & kích thước (Giữ nguyên) ===
        private readonly Size _groupboxSmallSize = new Size(347, 223);
        private readonly Size _groupboxLargeSize = new Size(347, 417);
        private readonly Point _saveButtonInfoLocation = new Point(439, 525);
        private readonly Point _cancelButtonInfoLocation = new Point(439, 576);
        private readonly Point _saveButtonPassLocation = new Point(827, 525);
        private readonly Point _cancelButtonPassLocation = new Point(827, 576);

        private enum EditMode { Viewing, EditingInfo, EditingPassword }

        // === Hàm khởi tạo ===
        public ucDriverProfile()
        {
            InitializeComponent();
            _driverSvc = new DriverService(); // Khởi tạo DriverService
            this.Load += ucDriverProfile_Load;
        }

        // === HÀM LOAD CHÍNH ===
        private void ucDriverProfile_Load(object sender, EventArgs e)
        {
            UserAccount loggedInAccount = AppSession.CurrentAccount;
            // Kiểm tra xem có phải Driver không
            if (loggedInAccount == null || loggedInAccount.DriverId == null)
            {
                MessageBox.Show("Lỗi: Không thể tải hồ sơ tài xế. (Session không hợp lệ)", "Lỗi Hồ Sơ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _currentUserAccount = loggedInAccount;
            try
            {
                // Gọi hàm mới của DriverService
                var details = _driverSvc.GetDriverDetailsByAccountId(_currentUserAccount.Id);
                _currentDriver = details.Driver; // Gán cho _currentDriver
                _currentUserAccount = details.Account;

                // Load dữ liệu GPLX vào ComboBox (Làm 1 lần)
                LoadLicenseTypes();

                PopulateData();
                SetEditMode(EditMode.Viewing);
                WireEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải hồ sơ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load danh sách GPLX vào ComboBox
        private void LoadLicenseTypes()
        {
            // Chỉ load nếu chưa có item nào
            if (cmbLicenseType.Items.Count == 0)
            {
                cmbLicenseType.DropDownStyle = ComboBoxStyle.DropDownList; // Không cho nhập tự do
                                                                           // Thêm các loại GPLX phổ biến
                cmbLicenseType.Items.AddRange(new object[] { "B2", "C", "D", "E", "FC", "FD", "FE" });
            }
        }


        // Gán sự kiện
        private void WireEvents()
        {
            btnChangeAvatar.Click -= btnChangeAvatar_Click;
            btnEditInfo.Click -= btnEditInfo_Click;
            btnChangePassword.Click -= btnChangePassword_Click;
            btnSave.Click -= btnSave_Click;
            btnCancel.Click -= btnCancel_Click;
            chkShowPassword.CheckedChanged -= chkShowPassword_CheckedChanged;

            btnChangeAvatar.Click += btnChangeAvatar_Click;
            btnEditInfo.Click += btnEditInfo_Click;
            btnChangePassword.Click += btnChangePassword_Click;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;
            chkShowPassword.CheckedChanged += chkShowPassword_CheckedChanged;
        }

        // Đổ dữ liệu vào control (Đã cập nhật cho Driver)
        private void PopulateData()
        {
            lblUsername.Text = _currentUserAccount.Username;
            txtUsername.Text = _currentUserAccount.Username;
            // Dùng _currentDriver
            txtFullName.Text = _currentDriver.FullName;
            txtPhone.Text = _currentDriver.Phone;
            txtCitizenId.Text = _currentDriver.CitizenId;
            cmbLicenseType.SelectedItem = _currentDriver.LicenseType; // Chọn đúng GPLX

            txtOldPassword.Text = _currentUserAccount.PasswordHash; // (Giả định chưa hash)
            txtOldPassword.PasswordChar = '●';

            txtNewPassword.Clear();
            txtConfirmPassword.Clear();

            // Tải ảnh đại diện Driver
            if (_currentDriver.AvatarData == null || _currentDriver.AvatarData.Length == 0)
            {
                picAvatar.Image = Properties.Resources.default_avatar_2; // Dùng ảnh default
            }
            else
            {
                picAvatar.Image = ByteArrayToImage(_currentDriver.AvatarData);
            }
        }

        // === HÀM ĐIỀU KHIỂN GIAO DIỆN CHÍNH (Giữ nguyên logic, chỉ đổi tên biến nếu cần) ===
        private void SetEditMode(EditMode mode)
        {
            _currentMode = mode;
            bool isViewing = (mode == EditMode.Viewing);
            bool isEditingInfo = (mode == EditMode.EditingInfo);
            bool isEditingPass = (mode == EditMode.EditingPassword);

            // 1. Thông tin cá nhân Driver
            SetTextboxReadOnly(txtFullName, !isEditingInfo);
            SetTextboxReadOnly(txtPhone, !isEditingInfo);
            SetTextboxReadOnly(txtCitizenId, !isEditingInfo);
            cmbLicenseType.Enabled = isEditingInfo; // Bật/tắt ComboBox

            // 2. Thông tin tài khoản
            SetTextboxReadOnly(txtUsername, true);
            txtOldPassword.Enabled = isViewing || isEditingPass;
            txtOldPassword.ReadOnly = !isEditingPass;

            // 3. Khu vực Mật Khẩu Mới
            txtNewPassword.Visible = isEditingPass;
            txtConfirmPassword.Visible = isEditingPass;
            // lblNewPassword?.Visible = isEditingPass;
            // lblConfirmPassword?.Visible = isEditingPass;

            // 4. Checkbox hiện mật khẩu
            chkShowPassword.Enabled = isViewing || isEditingPass;
            if (isViewing)
            {
                chkShowPassword.Checked = false;
            }
            ApplyPasswordMask(chkShowPassword.Checked);

            // 5. Kích thước GroupBox
            grpAccountInfo.Size = isEditingPass ? _groupboxLargeSize : _groupboxSmallSize;

            // 6. Các nút "Đổi"
            btnEditInfo.Enabled = isViewing;
            btnChangePassword.Enabled = isViewing;
            btnChangeAvatar.Enabled = isViewing;

            // 7. Các nút "Lưu" / "Hủy"
            btnSave.Visible = !isViewing;
            btnCancel.Visible = !isViewing;

            // 8. Di chuyển nút "Lưu" / "Hủy"
            if (isEditingInfo)
            {
                btnSave.Location = _saveButtonInfoLocation;
                btnCancel.Location = _cancelButtonInfoLocation;
            }
            else if (isEditingPass)
            {
                btnSave.Location = _saveButtonPassLocation;
                btnCancel.Location = _cancelButtonPassLocation;
            }
        }

        // Hàm hỗ trợ để Bật/Tắt ReadOnly
        private void SetTextboxReadOnly(Guna2TextBox txt, bool isReadOnly)
        {
            txt.ReadOnly = isReadOnly;
        }

        // === LOGIC CHECKBOX HIỆN MẬT KHẨU (Giữ nguyên) ===
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkShowPassword.Checked;
            if (_currentMode == EditMode.Viewing && isChecked)
            {
                var confirmResult = MessageBox.Show("Cảnh báo: Hiển thị mật khẩu có thể gây lộ thông tin!\nBạn có muốn tiếp tục?",
                                                     "Cảnh báo bảo mật", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.No)
                {
                    chkShowPassword.Checked = false;
                    ApplyPasswordMask(false);
                    return;
                }
            }
            ApplyPasswordMask(isChecked);
        }
        private void ApplyPasswordMask(bool showPassword)
        {
            char passChar = showPassword ? '\0' : '●';
            if (txtOldPassword.Enabled)
            {
                txtOldPassword.PasswordChar = passChar;
            }
            if (_currentMode == EditMode.EditingPassword)
            {
                txtNewPassword.PasswordChar = passChar;
                txtConfirmPassword.PasswordChar = passChar;
            }
        }

        // === CÁC SỰ KIỆN KHI NHẤN NÚT (Logic giữ nguyên) ===
        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            SetEditMode(EditMode.EditingInfo);
        }
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            SetEditMode(EditMode.EditingPassword);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn lưu các thay đổi này?", "Xác nhận lưu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.No) return;

            if (txtNewPassword.Visible) SaveChanges_Password();
            else SaveChanges_Info();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (HasChanges())
            {
                var confirmResult = MessageBox.Show("Hủy bỏ những thay đổi chưa lưu?", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.No) return;
            }
            PopulateData();
            SetEditMode(EditMode.Viewing);
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();
        }

        // === HÀM KIỂM TRA THAY ĐỔI (Đã cập nhật cho Driver) ===
        private bool HasChanges()
        {
            if (_currentMode == EditMode.EditingInfo)
            {
                // So sánh thông tin cá nhân Driver
                return txtFullName.Text.Trim() != (_currentDriver.FullName ?? "") ||
                       txtPhone.Text.Trim() != (_currentDriver.Phone ?? "") ||
                       txtCitizenId.Text.Trim() != (_currentDriver.CitizenId ?? "") ||
                       cmbLicenseType.SelectedItem?.ToString() != (_currentDriver.LicenseType ?? "");
            }
            else if (_currentMode == EditMode.EditingPassword)
            {
                // Kiểm tra các ô mật khẩu
                return txtOldPassword.Text != _currentUserAccount.PasswordHash || // (Giả định chưa hash)
                       !string.IsNullOrEmpty(txtNewPassword.Text) ||
                       !string.IsNullOrEmpty(txtConfirmPassword.Text);
            }
            return false;
        }

        // === LOGIC LƯU TRỮ (Đã cập nhật cho Driver) ===

        // Lưu thông tin cá nhân Driver
        private void SaveChanges_Info()
        {
            try
            {
                // 1. Tạo DTO Driver
                var dto = new DriverProfileDto
                {
                    DriverId = _currentDriver.Id,
                    FullName = txtFullName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    CitizenId = txtCitizenId.Text.Trim(),
                    LicenseType = cmbLicenseType.SelectedItem?.ToString() // Lấy từ ComboBox
                };

                // 2. Gọi DriverService
                _driverSvc.UpdateDriverProfile(dto);

                // 3. Cập nhật biến local
                _currentDriver.FullName = dto.FullName;
                _currentDriver.Phone = dto.Phone;
                _currentDriver.CitizenId = dto.CitizenId;
                _currentDriver.LicenseType = dto.LicenseType;

                MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetEditMode(EditMode.Viewing);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Cập nhật thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Lưu mật khẩu Driver (Logic giữ nguyên, chỉ đổi Service)
        private void SaveChanges_Password()
        {
            string oldPass = txtOldPassword.Text;
            string newPass = txtNewPassword.Text;
            string confirmPass = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(oldPass) || string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(confirmPass))
            {
                MessageBox.Show("Vui lòng nhập đủ Mật khẩu cũ, Mật khẩu mới, và Xác nhận.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (newPass != confirmPass)
            {
                MessageBox.Show("Mật khẩu mới và Xác nhận mật khẩu không khớp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (newPass == oldPass)
            {
                MessageBox.Show("Mật khẩu mới phải khác mật khẩu cũ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (newPass.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải từ 6 ký tự trở lên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Gọi hàm của DriverService
                _driverSvc.ChangeDriverPassword(_currentUserAccount.Id, oldPass, newPass);

                MessageBox.Show("Đổi mật khẩu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _currentUserAccount.PasswordHash = newPass; // Cập nhật local
                SetEditMode(EditMode.Viewing);
                txtOldPassword.Text = newPass; // Hiển thị MK mới vào ô cũ
                ApplyPasswordMask(false);
                txtNewPassword.Clear();
                txtConfirmPassword.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Đổi mật khẩu thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Đổi ảnh đại diện Driver (Logic giữ nguyên, chỉ đổi Service)
        private void btnChangeAvatar_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Tệp ảnh (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif";
            openFile.Title = "Chọn ảnh đại diện";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image newImage = Image.FromFile(openFile.FileName);
                    byte[] avatarData = ImageToByteArray(newImage);
                    // Gọi hàm của DriverService
                    _driverSvc.UpdateDriverAvatar(_currentDriver.Id, avatarData);
                    picAvatar.Image = newImage;
                    _currentDriver.AvatarData = avatarData; // Cập nhật local
                    MessageBox.Show("Cập nhật ảnh đại diện thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi đổi ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // === CÁC HÀM HỖ TRỢ CHUYỂN ĐỔI ẢNH (Giữ nguyên) ===
        private byte[] ImageToByteArray(Image imageIn)
        {
            if (imageIn == null) return null;
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(ms);
            }
        }

    } // Kết thúc lớp ucDriverProfile
} // Kết thúc namespace