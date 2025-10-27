//// LMS.GUI/VehicleAdmin/ucVehicleEditor_Admin.cs
//using Guna.UI2.WinForms;
//using LMS.BUS.Services; // Giả sử bạn sẽ tạo service này
//using LMS.DAL.Models;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Windows.Forms;

//namespace LMS.GUI.VehicleAdmin
//{
//    public partial class ucVehicleEditor_Admin : UserControl
//    {
//        public enum EditorMode { Add, Edit }

//        // State & Dependencies
//        private EditorMode _mode = EditorMode.Add;
//        private int _vehicleId = 0;
//        private readonly VehicleService_Admin _vehicleSvc = new VehicleService_Admin(); // Service mới
//        private Vehicle _originalData; // Để theo dõi thay đổi

//        // UI Helpers
//        private ErrorProvider errProvider;

//        // Dragging State
//        private bool isDragging = false;
//        private Point dragStartPoint = Point.Empty;
//        private Point parentFormStartPoint = Point.Empty;

//        public ucVehicleEditor_Admin()
//        {
//            InitializeComponent();
//            // Đảm bảo bạn đã kéo ErrorProvider vào form trong Designer
//            this.errProvider = new ErrorProvider(this.components); // Hoặc new ErrorProvider { ContainerControl = this };
//            WireEvents();
//            LoadComboBoxes();
//            this.Load += (s, e) => { txtPlateNo.Focus(); }; // Focus biển số khi load
//        }

//        #region Data Loading & Mode Handling

//        public void LoadData(EditorMode mode, int? vehicleId)
//        {
//            _mode = mode;
//            errProvider.Clear(); // Xóa lỗi cũ

//            Font titleFont = new Font("Segoe UI", 14F, FontStyle.Bold);
//            if (lblTitle != null) lblTitle.Font = titleFont;

//            if (mode == EditorMode.Edit)
//            {
//                if (!vehicleId.HasValue) throw new ArgumentNullException(nameof(vehicleId), "Vehicle ID is required for Edit mode.");
//                _vehicleId = vehicleId.Value;
//                if (lblTitle != null) lblTitle.Text = "Sửa Thông Tin Phương Tiện";

//                try
//                {
//                    // TODO: Implement GetVehicleForEdit in VehicleService_Admin
//                    _originalData = _vehicleSvc.GetVehicleForEdit(_vehicleId);
//                    if (_originalData == null) throw new Exception($"Phương tiện ID {_vehicleId} không tìm thấy.");

//                    // Populate controls
//                    txtPlateNo.Text = _originalData.PlateNo;
//                    cmbType.Text = _originalData.Type; // Giả sử cmbType cho nhập tự do hoặc chọn
//                    txtCapacity.Text = _originalData.CapacityKg?.ToString() ?? "";
//                    cmbStatus.SelectedValue = _originalData.Status;
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Lỗi tải thông tin phương tiện: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    this.FindForm()?.CloseDialog(DialogResult.Cancel); // Đóng nếu lỗi
//                }
//            }
//            else // Add Mode
//            {
//                if (lblTitle != null) lblTitle.Text = "Thêm Phương Tiện Mới";
//                _vehicleId = 0;
//                _originalData = new Vehicle(); // Dữ liệu gốc rỗng

//                // Clear controls
//                txtPlateNo.Clear();
//                cmbType.SelectedIndex = -1;
//                cmbType.Text = ""; // Xóa text nếu cho nhập tự do
//                txtCapacity.Clear();
//                cmbStatus.SelectedIndex = 0; // Mặc định là Active
//            }
//        }

//        private void LoadComboBoxes()
//        {
//            // Load Status ComboBox
//            // Sử dụng Dictionary để binding dễ dàng hơn
//            var statusOptions = new Dictionary<VehicleStatus, string>
//            {
//                { VehicleStatus.Active, "Hoạt động" },
//                { VehicleStatus.Maintenance, "Đang bảo trì" },
//                { VehicleStatus.Inactive, "Ngừng hoạt động" }
//            };
//            cmbStatus.DataSource = new BindingSource(statusOptions, null);
//            cmbStatus.DisplayMember = "Value";
//            cmbStatus.ValueMember = "Key";
//            cmbStatus.SelectedIndex = 0; // Mặc định là Active

//            // Load Type ComboBox (Ví dụ - bạn có thể lấy từ DB hoặc cấu hình)
//            cmbType.Items.Clear();
//            cmbType.Items.AddRange(new string[] { "Xe tải 1.5T", "Xe tải 5T", "Xe bán tải", "Xe máy", "Container" });
//            // Nếu muốn cho nhập tự do, không cần dòng DropDownStyle
//            cmbType.DropDownStyle = ComboBoxStyle.DropDownList; // Chỉ cho chọn từ list
//        }

//        #endregion

//        #region Event Wiring & Handlers

//        private void WireEvents()
//        {
//            btnSave.Click += BtnSave_Click;
//            btnCancel.Click += BtnCancel_Click;

//            // Drag and Drop
//            Control dragHandle = pnlTop; // Dùng panel top
//            if (dragHandle != null)
//            {
//                dragHandle.MouseDown += DragHandle_MouseDown;
//                dragHandle.MouseMove += DragHandle_MouseMove;
//                dragHandle.MouseUp += DragHandle_MouseUp;
//            }
//            //if (btnClose != null) btnClose.Click += (s, e) => BtnCancel_Click(s, e);
//        }

//        private void BtnSave_Click(object sender, EventArgs e)
//        {
//            if (!ValidateInput())
//            {
//                MessageBox.Show("Vui lòng kiểm tra lại thông tin.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            var confirmMsg = (_mode == EditorMode.Add) ? "Thêm phương tiện mới?" : "Lưu thay đổi?";
//            if (MessageBox.Show(confirmMsg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
//            {
//                return;
//            }

//            Vehicle vehicleToSave = (_mode == EditorMode.Edit && _originalData != null) ? _originalData : new Vehicle();

//            vehicleToSave.PlateNo = txtPlateNo.Text.Trim().ToUpper(); // Chuẩn hóa biển số
//            vehicleToSave.Type = cmbType.Text; // Lấy text (nếu cho nhập) hoặc SelectedItem.ToString()
//            vehicleToSave.CapacityKg = int.TryParse(txtCapacity.Text, out int capacity) ? capacity : (int?)null;
//            vehicleToSave.Status = (VehicleStatus)cmbStatus.SelectedValue;
//            // LastMaintenanceDate và Notes đã bị bỏ

//            try
//            {
//                if (_mode == EditorMode.Add)
//                {
//                    // TODO: Implement CreateVehicle in VehicleService_Admin
//                    _vehicleSvc.CreateVehicle(vehicleToSave);
//                }
//                else
//                {
//                    // TODO: Implement UpdateVehicle in VehicleService_Admin
//                    _vehicleSvc.UpdateVehicle(vehicleToSave);
//                }

//                MessageBox.Show("Lưu thông tin phương tiện thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                this.FindForm()?.CloseDialog(DialogResult.OK); // Đóng form
//            }
//            catch (InvalidOperationException opEx) // Bắt lỗi nghiệp vụ (vd: trùng biển số)
//            {
//                MessageBox.Show(opEx.Message, "Lỗi Nghiệp Vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi lưu phương tiện: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void BtnCancel_Click(object sender, EventArgs e)
//        {
//            if (HasChanges())
//            {
//                if (MessageBox.Show("Thông tin đã thay đổi chưa được lưu. Hủy bỏ?", "Xác nhận hủy",
//                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
//                    return;
//            }
//            this.FindForm()?.CloseDialog(DialogResult.Cancel);
//        }

//        #endregion

//        #region Validation & Change Tracking

//        private bool ValidateInput()
//        {
//            errProvider.Clear();
//            bool isValid = true;

//            if (string.IsNullOrWhiteSpace(txtPlateNo.Text))
//            {
//                errProvider.SetError(txtPlateNo, "Biển số xe không được để trống.");
//                isValid = false;
//            }
//            // Thêm validate định dạng biển số nếu cần (Regex)

//            if (string.IsNullOrWhiteSpace(cmbType.Text)) // Kiểm tra Text thay vì SelectedIndex nếu cho nhập
//            {
//                errProvider.SetError(cmbType, "Vui lòng chọn hoặc nhập loại xe.");
//                isValid = false;
//            }

//            if (!string.IsNullOrWhiteSpace(txtCapacity.Text) && !int.TryParse(txtCapacity.Text, out int capacity) || capacity < 0)
//            {
//                errProvider.SetError(txtCapacity, "Trọng tải phải là số nguyên dương (hoặc bỏ trống).");
//                isValid = false;
//            }

//            if (cmbStatus.SelectedValue == null)
//            {
//                errProvider.SetError(cmbStatus, "Vui lòng chọn trạng thái.");
//                isValid = false;
//            }

//            return isValid;
//        }

//        private bool HasChanges()
//        {
//            if (_originalData == null && _mode == EditorMode.Edit) return false; // Lỗi load dữ liệu gốc

//            if (_mode == EditorMode.Add)
//            {
//                return !string.IsNullOrWhiteSpace(txtPlateNo.Text) ||
//                       !string.IsNullOrWhiteSpace(cmbType.Text) ||
//                       !string.IsNullOrWhiteSpace(txtCapacity.Text) ||
//                       cmbStatus.SelectedIndex != 0; // Khác trạng thái mặc định ban đầu
//            }
//            else // Edit Mode
//            {
//                if (_originalData == null) return false; // Thêm kiểm tra phòng hờ
//                int? currentCapacity = int.TryParse(txtCapacity.Text, out int cap) ? cap : (int?)null;
//                return _originalData.PlateNo != txtPlateNo.Text.Trim().ToUpper() ||
//                       _originalData.Type != cmbType.Text ||
//                       _originalData.CapacityKg != currentCapacity ||
//                       _originalData.Status != (VehicleStatus?)cmbStatus.SelectedValue;
//            }
//        }


//        #endregion

//        #region Form Dragging Logic
//        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left) { Form parentForm = this.FindForm(); if (parentForm != null) { isDragging = true; dragStartPoint = Cursor.Position; parentFormStartPoint = parentForm.Location; } }
//        }
//        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (isDragging) { Form parentForm = this.FindForm(); if (parentForm != null) { Point diff = Point.Subtract(Cursor.Position, new Size(dragStartPoint)); parentForm.Location = Point.Add(parentFormStartPoint, new Size(diff)); } }
//        }
//        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left) { isDragging = false; }
//        }
//        #endregion
//    }

//    // --- Extension Method Helper (đặt ở cuối file hoặc trong file riêng) ---
//    public static class FormExtensions
//    {
//        public static void CloseDialog(this Form form, DialogResult result)
//        {
//            if (form != null)
//            {
//                form.DialogResult = result;
//                form.Close();
//            }
//        }
//    }
//}

// LMS.GUI/VehicleAdmin/ucVehicleEditor_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Services; // Giả sử bạn sẽ tạo service này
using LMS.DAL.Models;
using LMS.GUI.RouteTemplateAdmin;
using System;
using System.Collections.Generic; // Cần cho Dictionary
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LMS.GUI.VehicleAdmin
{
    public partial class ucVehicleEditor_Admin : UserControl
    {
        public enum EditorMode { Add, Edit }

        // State & Dependencies
        private EditorMode _mode = EditorMode.Add;
        private int _vehicleId = 0;
        private readonly VehicleService_Admin _vehicleSvc = new VehicleService_Admin(); // Service mới
        private Vehicle _originalData; // Để theo dõi thay đổi

        // UI Helpers
        private ErrorProvider errProvider;

        // Dragging State
        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;
        private Point parentFormStartPoint = Point.Empty;

        public ucVehicleEditor_Admin()
        {
            InitializeComponent();
            if (this.errProvider == null) // Kiểm tra nếu chưa được khởi tạo bởi Designer
            {
                this.errProvider = new ErrorProvider { ContainerControl = this };
            }
            WireEvents();
            LoadComboBoxes();
            this.Load += (s, e) => { txtPlateNo.Focus(); }; // Focus biển số khi load
        }

        #region Data Loading & Mode Handling

        public void LoadData(EditorMode mode, int? vehicleId)
        {
            _mode = mode;
            errProvider.Clear(); // Xóa lỗi cũ

            Font titleFont = new Font("Segoe UI", 14F, FontStyle.Bold);
            if (lblTitle != null) lblTitle.Font = titleFont;

            if (mode == EditorMode.Edit)
            {
                if (!vehicleId.HasValue) throw new ArgumentNullException(nameof(vehicleId), "Vehicle ID is required for Edit mode.");
                _vehicleId = vehicleId.Value;
                if (lblTitle != null) lblTitle.Text = "Sửa Thông Tin Phương Tiện";

                try
                {
                    _originalData = _vehicleSvc.GetVehicleForEdit(_vehicleId);
                    if (_originalData == null) throw new Exception($"Phương tiện ID {_vehicleId} không tìm thấy.");

                    txtPlateNo.Text = _originalData.PlateNo;
                    cmbType.Text = _originalData.Type; // Giả sử cmbType cho nhập tự do hoặc chọn
                    txtCapacity.Text = _originalData.CapacityKg?.ToString() ?? "";
                    cmbStatus.SelectedValue = _originalData.Status;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi tải thông tin phương tiện: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.FindForm()?.CloseDialog(DialogResult.Cancel); // Đóng nếu lỗi
                }
            }
            else // Add Mode
            {
                if (lblTitle != null) lblTitle.Text = "Thêm Phương Tiện Mới";
                _vehicleId = 0;
                _originalData = new Vehicle(); // Dữ liệu gốc rỗng

                txtPlateNo.Clear();
                cmbType.SelectedIndex = -1;
                cmbType.Text = ""; // Xóa text nếu cho nhập tự do
                txtCapacity.Clear();
                cmbStatus.SelectedIndex = 0; // Mặc định là Active
            }
        }

        private void LoadComboBoxes()
        {
            var statusOptions = new Dictionary<VehicleStatus, string>
            {
                { VehicleStatus.Active, "Hoạt động" },
                { VehicleStatus.Maintenance, "Đang bảo trì" },
                { VehicleStatus.Inactive, "Ngừng hoạt động" }
            };
            cmbStatus.DataSource = new BindingSource(statusOptions, null);
            cmbStatus.DisplayMember = "Value";
            cmbStatus.ValueMember = "Key";
            cmbStatus.SelectedIndex = 0; // Mặc định là Active

            cmbType.Items.Clear();
            cmbType.Items.AddRange(new string[] { "Xe tải 1.5T", "Xe tải 5T", "Xe bán tải", "Xe máy", "Container" });
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList; // Chỉ cho chọn từ list
        }

        #endregion

        #region Event Wiring & Handlers

        private void WireEvents()
        {
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

            Control dragHandle = pnlTop; // Dùng panel top
            if (dragHandle != null)
            {
                dragHandle.MouseDown += DragHandle_MouseDown;
                dragHandle.MouseMove += DragHandle_MouseMove;
                dragHandle.MouseUp += DragHandle_MouseUp;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                MessageBox.Show("Vui lòng kiểm tra lại thông tin.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmMsg = (_mode == EditorMode.Add) ? "Thêm phương tiện mới?" : "Lưu thay đổi?";
            if (MessageBox.Show(confirmMsg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            Vehicle vehicleToSave = (_mode == EditorMode.Edit && _originalData != null) ? _originalData : new Vehicle();

            vehicleToSave.PlateNo = txtPlateNo.Text.Trim().ToUpper(); // Chuẩn hóa biển số
            vehicleToSave.Type = cmbType.Text; // Lấy text (nếu cho nhập) hoặc SelectedItem.ToString()
            vehicleToSave.CapacityKg = int.TryParse(txtCapacity.Text, out int capacity) ? capacity : (int?)null; // Parse lại capacity
            vehicleToSave.Status = (VehicleStatus)cmbStatus.SelectedValue;

            try
            {
                if (_mode == EditorMode.Add)
                {
                    _vehicleSvc.CreateVehicle(vehicleToSave);
                }
                else
                {
                    _vehicleSvc.UpdateVehicle(vehicleToSave);
                }

                MessageBox.Show("Lưu thông tin phương tiện thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.FindForm()?.CloseDialog(DialogResult.OK); // Đóng form
            }
            catch (InvalidOperationException opEx)
            {
                MessageBox.Show(opEx.Message, "Lỗi Nghiệp Vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu phương tiện: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (HasChanges())
            {
                if (MessageBox.Show("Thông tin đã thay đổi chưa được lưu. Hủy bỏ?", "Xác nhận hủy",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }
            this.FindForm()?.CloseDialog(DialogResult.Cancel);
        }

        #endregion

        #region Validation & Change Tracking

        private bool ValidateInput()
        {
            errProvider.Clear();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtPlateNo.Text))
            {
                errProvider.SetError(txtPlateNo, "Biển số xe không được để trống.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(cmbType.Text))
            {
                errProvider.SetError(cmbType, "Vui lòng chọn hoặc nhập loại xe.");
                isValid = false;
            }

            // *** SỬA LỖI LOGIC KIỂM TRA CAPACITY ***
            if (!string.IsNullOrWhiteSpace(txtCapacity.Text)) // Chỉ kiểm tra nếu không rỗng
            {
                if (!int.TryParse(txtCapacity.Text, out int capacityValue) || capacityValue < 0) // Thử parse, nếu thành công thì kiểm tra < 0
                {
                    errProvider.SetError(txtCapacity, "Trọng tải phải là số nguyên không âm (hoặc bỏ trống).");
                    isValid = false;
                }
            }

            if (cmbStatus.SelectedValue == null)
            {
                errProvider.SetError(cmbStatus, "Vui lòng chọn trạng thái.");
                isValid = false;
            }

            return isValid;
        }


        private bool HasChanges()
        {
            if (_originalData == null && _mode == EditorMode.Edit) return false;

            if (_mode == EditorMode.Add)
            {
                return !string.IsNullOrWhiteSpace(txtPlateNo.Text) ||
                       !string.IsNullOrWhiteSpace(cmbType.Text) ||
                       !string.IsNullOrWhiteSpace(txtCapacity.Text) ||
                       cmbStatus.SelectedIndex != 0;
            }
            else // Edit Mode
            {
                if (_originalData == null) return false;
                int? currentCapacity = int.TryParse(txtCapacity.Text, out int cap) ? cap : (int?)null;
                return _originalData.PlateNo != txtPlateNo.Text.Trim().ToUpper() ||
                       _originalData.Type != cmbType.Text ||
                       _originalData.CapacityKg != currentCapacity ||
                       _originalData.Status != (VehicleStatus?)cmbStatus.SelectedValue;
            }
        }


        #endregion

        #region Form Dragging Logic
        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { Form parentForm = this.FindForm(); if (parentForm != null) { isDragging = true; dragStartPoint = Cursor.Position; parentFormStartPoint = parentForm.Location; } }
        }
        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging) { Form parentForm = this.FindForm(); if (parentForm != null) { Point diff = Point.Subtract(Cursor.Position, new Size(dragStartPoint)); parentForm.Location = Point.Add(parentFormStartPoint, new Size(diff)); } }
        }
        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { isDragging = false; }
        }
        #endregion
    }

}