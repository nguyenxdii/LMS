// LMS.GUI.ProfileCustomer.ucCustomerProfile.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos; // Cần cho CustomerProfileDto
using LMS.BUS.Services; // Cần cho CustomerService
using LMS.DAL.Models; // Cần cho UserAccount, Customer
using System;
using System.Drawing;
using System.IO; // Cần cho MemoryStream
using System.Windows.Forms;
using LMS.BUS.Helpers; // Cần cho AppSession

namespace LMS.GUI.ProfileCustomer
{
    public partial class ucCustomerProfile : UserControl
    {
        // === Service và Dữ liệu ===
        private readonly CustomerService _customerSvc;
        private UserAccount _currentUserAccount; // Tài khoản đang đăng nhập
        private Customer _currentCustomer; // Hồ sơ khách hàng của tài khoản đó
        private EditMode _currentMode = EditMode.Viewing; // Biến lưu trạng thái hiện tại

        // === Các hằng số vị trí & kích thước ===
        private readonly Size _groupboxSmallSize = new Size(347, 223);
        private readonly Size _groupboxLargeSize = new Size(347, 417);
        private readonly Point _saveButtonInfoLocation = new Point(439, 525);
        private readonly Point _cancelButtonInfoLocation = new Point(439, 576);
        private readonly Point _saveButtonPassLocation = new Point(827, 525);
        private readonly Point _cancelButtonPassLocation = new Point(827, 576);

        // Enum để theo dõi trạng thái của form
        private enum EditMode { Viewing, EditingInfo, EditingPassword }

        // === Hàm khởi tạo ===
        public ucCustomerProfile()
        {
            InitializeComponent();
            _customerSvc = new CustomerService();
            this.Load += ucCustomerProfile_Load; // Gán sự kiện Load
        }

        // === HÀM LOAD CHÍNH (Sự kiện Load) ===
        private void ucCustomerProfile_Load(object sender, EventArgs e)
        {
            UserAccount loggedInAccount = AppSession.CurrentAccount;
            if (loggedInAccount == null || loggedInAccount.CustomerId == null)
            {
                MessageBox.Show("Lỗi: Không thể tải hồ sơ khách hàng. (Session không hợp lệ)", "Lỗi Hồ Sơ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _currentUserAccount = loggedInAccount;
            try
            {
                var details = _customerSvc.GetCustomerDetailsByAccountId(_currentUserAccount.Id);
                _currentCustomer = details.Customer;
                _currentUserAccount = details.Account;
                PopulateData();
                SetEditMode(EditMode.Viewing); // Bắt đầu ở chế độ xem
                WireEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải hồ sơ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Gán sự kiện cho các control
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

        // Đổ dữ liệu vào các control
        private void PopulateData()
        {
            lblUsername.Text = _currentUserAccount.Username;
            txtUsername.Text = _currentUserAccount.Username;
            txtFullName.Text = _currentCustomer.Name;
            txtPhone.Text = _currentCustomer.Phone;
            txtEmail.Text = _currentCustomer.Email;
            txtAddress.Text = _currentCustomer.Address;

            // Hiển thị mật khẩu cũ (giả định chưa hash)
            txtOldPassword.Text = _currentUserAccount.PasswordHash;
            txtOldPassword.PasswordChar = '●'; // Luôn bắt đầu ẩn

            txtNewPassword.Clear();
            txtConfirmPassword.Clear();

            // Tải ảnh đại diện
            if (_currentCustomer.AvatarData == null || _currentCustomer.AvatarData.Length == 0)
            {
                picAvatar.Image = Properties.Resources.default_avatar_2;
            }
            else
            {
                picAvatar.Image = ByteArrayToImage(_currentCustomer.AvatarData);
            }
        }

        // === HÀM ĐIỀU KHIỂN GIAO DIỆN CHÍNH ===
        private void SetEditMode(EditMode mode)
        {
            _currentMode = mode;
            bool isViewing = (mode == EditMode.Viewing);
            bool isEditingInfo = (mode == EditMode.EditingInfo);
            bool isEditingPass = (mode == EditMode.EditingPassword);

            SetTextboxReadOnly(txtFullName, !isEditingInfo);
            SetTextboxReadOnly(txtPhone, !isEditingInfo);
            SetTextboxReadOnly(txtEmail, !isEditingInfo);
            SetTextboxReadOnly(txtAddress, !isEditingInfo);

            SetTextboxReadOnly(txtUsername, true);
            txtOldPassword.Enabled = isViewing || isEditingPass;
            txtOldPassword.ReadOnly = !isEditingPass; // Cho sửa MK cũ CHỈ KHI đang Sửa MK

            txtNewPassword.Visible = isEditingPass;
            txtConfirmPassword.Visible = isEditingPass;
            // lblNewPassword?.Visible = isEditingPass;
            // lblConfirmPassword?.Visible = isEditingPass;

            chkShowPassword.Enabled = isViewing || isEditingPass;
            if (isViewing)
            {
                chkShowPassword.Checked = false;
            }
            ApplyPasswordMask(chkShowPassword.Checked);

            grpAccountInfo.Size = isEditingPass ? _groupboxLargeSize : _groupboxSmallSize;

            // Các nút "Đổi" (Bật/Tắt thay vì ẩn/hiện)
            btnEditInfo.Enabled = isViewing;
            btnChangePassword.Enabled = isViewing;
            btnChangeAvatar.Enabled = isViewing;

            btnSave.Visible = !isViewing;
            btnCancel.Visible = !isViewing;

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

        // === LOGIC CHECKBOX HIỆN MẬT KHẨU ===
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkShowPassword.Checked;

            if (_currentMode == EditMode.Viewing && isChecked)
            {
                var confirmResult = MessageBox.Show("Cảnh báo: Hiển thị mật khẩu có thể gây lộ thông tin!\nBạn có muốn tiếp tục?",
                                                     "Cảnh báo bảo mật",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.No)
                {
                    chkShowPassword.Checked = false;
                    ApplyPasswordMask(false);
                    return;
                }
            }
            ApplyPasswordMask(isChecked);
        }

        // Hàm áp dụng ẩn/hiện mật khẩu
        private void ApplyPasswordMask(bool showPassword)
        {
            char passChar = showPassword ? '\0' : '●';
            if (txtOldPassword.Enabled) // Chỉ đổi nếu ô MK cũ đang bật
            {
                txtOldPassword.PasswordChar = passChar;
            }
            if (_currentMode == EditMode.EditingPassword)
            {
                txtNewPassword.PasswordChar = passChar;
                txtConfirmPassword.PasswordChar = passChar;
            }
        }

        // === CÁC SỰ KIỆN KHI NHẤN NÚT ===

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
            // txtOldPassword sẽ được PopulateData xử lý
        }

        // === HÀM KIỂM TRA THAY ĐỔI ===
        private bool HasChanges()
        {
            if (_currentMode == EditMode.EditingInfo)
            {
                return txtFullName.Text.Trim() != (_currentCustomer.Name ?? "") ||
                       txtPhone.Text.Trim() != (_currentCustomer.Phone ?? "") ||
                       txtEmail.Text.Trim() != (_currentCustomer.Email ?? "") ||
                       txtAddress.Text.Trim() != (_currentCustomer.Address ?? "");
            }
            else if (_currentMode == EditMode.EditingPassword)
            {
                // Chỉ cần 1 trong 3 ô MK có thay đổi là true
                return txtOldPassword.Text != _currentUserAccount.PasswordHash || // (Giả định chưa hash)
                       !string.IsNullOrEmpty(txtNewPassword.Text) ||
                       !string.IsNullOrEmpty(txtConfirmPassword.Text);
            }
            return false;
        }

        // === LOGIC LƯU TRỮ (GỌI SERVICE) ===

        private void SaveChanges_Info()
        {
            try
            {
                var dto = new CustomerProfileDto
                {
                    CustomerId = _currentCustomer.Id,
                    FullName = txtFullName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Address = txtAddress.Text.Trim()
                };
                _customerSvc.UpdateCustomerProfile(dto);

                _currentCustomer.Name = dto.FullName;
                _currentCustomer.Phone = dto.Phone;
                _currentCustomer.Email = dto.Email;
                _currentCustomer.Address = dto.Address;

                MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetEditMode(EditMode.Viewing);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Cập nhật thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Lưu mật khẩu (Đã cập nhật kiểm tra trùng MK cũ)
        private void SaveChanges_Password()
        {
            string oldPass = txtOldPassword.Text; // MK cũ user nhập lại
            string newPass = txtNewPassword.Text;
            string confirmPass = txtConfirmPassword.Text;

            // --- Validation ---
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
            // ===== KIỂM TRA TRÙNG MẬT KHẨU CŨ =====
            if (newPass == oldPass) // So sánh MK mới với MK cũ user vừa nhập
            {
                MessageBox.Show("Mật khẩu mới phải khác mật khẩu cũ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng lại không lưu
            }
            // =====================================
            // (Thêm kiểm tra độ dài newPass nếu cần, vd: >= 6 ký tự)
            if (newPass.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải từ 6 ký tự trở lên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // --- Gọi Service ---
                _customerSvc.ChangeCustomerPassword(_currentUserAccount.Id, oldPass, newPass);

                MessageBox.Show("Đổi mật khẩu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // --- Cập nhật UI ---
                _currentUserAccount.PasswordHash = newPass; // Cập nhật biến local (giả định chưa hash)
                SetEditMode(EditMode.Viewing); // Quay về chế độ Xem
                txtOldPassword.Text = newPass; // Hiển thị MK mới (đã cập nhật) vào ô MK cũ
                ApplyPasswordMask(false);      // Đảm bảo nó ẩn đi
                txtNewPassword.Clear();        // Xóa các ô không cần nữa
                txtConfirmPassword.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Đổi mật khẩu thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Không xóa MK nếu lỗi để user sửa lại
            }
        }

        // Đổi ảnh đại diện
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
                    _customerSvc.UpdateCustomerAvatar(_currentCustomer.Id, avatarData);
                    picAvatar.Image = newImage;
                    _currentCustomer.AvatarData = avatarData;
                    MessageBox.Show("Cập nhật ảnh đại diện thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi đổi ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // === CÁC HÀM HỖ TRỢ CHUYỂN ĐỔI ẢNH ===

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
    } // Kết thúc lớp ucCustomerProfile
} // Kết thúc namespace