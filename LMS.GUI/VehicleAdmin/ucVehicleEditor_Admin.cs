//// LMS.GUI/VehicleAdmin/ucVehicleEditor_Admin.cs
//using Guna.UI2.WinForms;
//using LMS.BUS.Services; // Giả sử bạn sẽ tạo service này
//using LMS.DAL.Models;
//using LMS.GUI.RouteTemplateAdmin;
//using System;
//using System.Collections.Generic; // Cần cho Dictionary
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
//            if (this.errProvider == null) // Kiểm tra nếu chưa được khởi tạo bởi Designer
//            {
//                this.errProvider = new ErrorProvider { ContainerControl = this };
//            }
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
//                    _originalData = _vehicleSvc.GetVehicleForEdit(_vehicleId);
//                    if (_originalData == null) throw new Exception($"Phương tiện ID {_vehicleId} không tìm thấy.");

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

//                txtPlateNo.Clear();
//                cmbType.SelectedIndex = -1;
//                cmbType.Text = ""; // Xóa text nếu cho nhập tự do
//                txtCapacity.Clear();
//                cmbStatus.SelectedIndex = 0; // Mặc định là Active
//            }
//        }

//        private void LoadComboBoxes()
//        {
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

//            cmbType.Items.Clear();
//            cmbType.Items.AddRange(new string[] { "Xe tải 1.5T", "Xe tải 5T", "Xe bán tải", "Xe máy", "Container" });
//            cmbType.DropDownStyle = ComboBoxStyle.DropDownList; // Chỉ cho chọn từ list
//        }

//        #endregion

//        #region Event Wiring & Handlers

//        private void WireEvents()
//        {
//            btnSave.Click += BtnSave_Click;
//            btnCancel.Click += BtnCancel_Click;

//            Control dragHandle = pnlTop; // Dùng panel top
//            if (dragHandle != null)
//            {
//                dragHandle.MouseDown += DragHandle_MouseDown;
//                dragHandle.MouseMove += DragHandle_MouseMove;
//                dragHandle.MouseUp += DragHandle_MouseUp;
//            }
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
//            vehicleToSave.CapacityKg = int.TryParse(txtCapacity.Text, out int capacity) ? capacity : (int?)null; // Parse lại capacity
//            vehicleToSave.Status = (VehicleStatus)cmbStatus.SelectedValue;

//            try
//            {
//                if (_mode == EditorMode.Add)
//                {
//                    _vehicleSvc.CreateVehicle(vehicleToSave);
//                }
//                else
//                {
//                    _vehicleSvc.UpdateVehicle(vehicleToSave);
//                }

//                MessageBox.Show("Lưu thông tin phương tiện thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                this.FindForm()?.CloseDialog(DialogResult.OK); // Đóng form
//            }
//            catch (InvalidOperationException opEx)
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

//            if (string.IsNullOrWhiteSpace(cmbType.Text))
//            {
//                errProvider.SetError(cmbType, "Vui lòng chọn hoặc nhập loại xe.");
//                isValid = false;
//            }

//            // *** SỬA LỖI LOGIC KIỂM TRA CAPACITY ***
//            if (!string.IsNullOrWhiteSpace(txtCapacity.Text)) // Chỉ kiểm tra nếu không rỗng
//            {
//                if (!int.TryParse(txtCapacity.Text, out int capacityValue) || capacityValue < 0) // Thử parse, nếu thành công thì kiểm tra < 0
//                {
//                    errProvider.SetError(txtCapacity, "Trọng tải phải là số nguyên không âm (hoặc bỏ trống).");
//                    isValid = false;
//                }
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
//            if (_originalData == null && _mode == EditorMode.Edit) return false;

//            if (_mode == EditorMode.Add)
//            {
//                return !string.IsNullOrWhiteSpace(txtPlateNo.Text) ||
//                       !string.IsNullOrWhiteSpace(cmbType.Text) ||
//                       !string.IsNullOrWhiteSpace(txtCapacity.Text) ||
//                       cmbStatus.SelectedIndex != 0;
//            }
//            else // Edit Mode
//            {
//                if (_originalData == null) return false;
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

//}

// LMS.GUI/VehicleAdmin/ucVehicleEditor_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LMS.GUI.VehicleAdmin
{
    public partial class ucVehicleEditor_Admin : UserControl
    {
        public enum EditorMode { Add, Edit }

        // State & Dependencies
        private EditorMode _mode = EditorMode.Add;
        private int _vehicleId = 0;
        private readonly VehicleService_Admin _vehicleSvc = new VehicleService_Admin();
        private Vehicle _originalData;

        // UI Helpers
        private ErrorProvider errProvider;

        // Dragging State
        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;
        private Point parentFormStartPoint = Point.Empty;

        // *** NEW: Data structure for vehicle types ***
        public class VehicleTypeInfo
        {
            public string Name { get; set; }
            public int? CapacityKg { get; set; }
            public string Category { get; set; } // Thêm category để dễ tìm ngược

            // Override ToString for display in ComboBox
            public override string ToString() => Name;
        }

        private Dictionary<string, List<VehicleTypeInfo>> _vehicleTypesByCategory;
        private List<VehicleTypeInfo> _allVehicleTypes; // List phẳng để tìm kiếm

        public ucVehicleEditor_Admin()
        {
            InitializeComponent();
            if (this.errProvider == null)
            {
                this.errProvider = new ErrorProvider { ContainerControl = this };
            }
            InitializeVehicleTypeData();
            WireEvents();
            LoadComboBoxes(); // Load cmbType (category)
            this.Load += (s, e) => { txtPlateNo.Focus(); };
        }

        private void InitializeVehicleTypeData()
        {
            _vehicleTypesByCategory = new Dictionary<string, List<VehicleTypeInfo>>
            {
                { "Xe Van (nhỏ — nội thành)", new List<VehicleTypeInfo> {
                    new VehicleTypeInfo { Name = "Van 500Kg", CapacityKg = 500, Category = "Xe Van (nhỏ — nội thành)" },
                    new VehicleTypeInfo { Name = "Van 1000Kg (1 tấn)", CapacityKg = 1000, Category = "Xe Van (nhỏ — nội thành)" }
                }},
                { "Xe tải nhỏ & trung", new List<VehicleTypeInfo> {
                    new VehicleTypeInfo { Name = "Xe tải 1.5 tấn", CapacityKg = 1500, Category = "Xe tải nhỏ & trung" },
                    new VehicleTypeInfo { Name = "Xe tải 2.5 tấn", CapacityKg = 2500, Category = "Xe tải nhỏ & trung" },
                    new VehicleTypeInfo { Name = "Xe tải 5 tấn", CapacityKg = 5000, Category = "Xe tải nhỏ & trung" }
                }},
                { "Xe tải lớn / container", new List<VehicleTypeInfo> {
                    new VehicleTypeInfo { Name = "Xe tải 8 tấn", CapacityKg = 8000, Category = "Xe tải lớn / container" },
                    new VehicleTypeInfo { Name = "Xe tải 15 tấn", CapacityKg = 15000, Category = "Xe tải lớn / container" },
                    new VehicleTypeInfo { Name = "Container 20ft", CapacityKg = 26000, Category = "Xe tải lớn / container" },
                    new VehicleTypeInfo { Name = "Container 40ft", CapacityKg = 30000, Category = "Xe tải lớn / container" }
                }}
            };
            _allVehicleTypes = _vehicleTypesByCategory.SelectMany(kvp => kvp.Value).ToList();
        }


        #region Data Loading & Mode Handling

        public void LoadData(EditorMode mode, int? vehicleId)
        {
            _mode = mode;
            errProvider.Clear();

            Font titleFont = new Font("Segoe UI", 14F, FontStyle.Bold);
            if (lblTitle != null) lblTitle.Font = titleFont;

            // Reset cmbSpecificType trước khi load
            cmbSpecificType.DataSource = null;
            cmbSpecificType.Items.Clear();
            cmbSpecificType.Enabled = false; // Disable trước


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
                    cmbStatus.SelectedValue = _originalData.Status;

                    var vehicleInfo = _allVehicleTypes.FirstOrDefault(vt => vt.Name == _originalData.Type);
                    if (vehicleInfo != null)
                    {
                        cmbType.SelectedItem = vehicleInfo.Category;
                        PopulateSpecificTypes(vehicleInfo.Category);
                        cmbSpecificType.SelectedItem = vehicleInfo;
                    }
                    else // Trường hợp loại xe cũ không có trong list mới
                    {
                        cmbType.SelectedIndex = -1; // Hoặc có thể hiển thị loại xe cũ vào Text
                        cmbType.Text = _originalData.Type; // Hiển thị tạm
                        cmbSpecificType.Enabled = false; // Disable cái thứ 2
                        MessageBox.Show("Loại xe hiện tại không có trong danh sách gợi ý.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi tải thông tin phương tiện: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.FindForm()?.CloseDialog(DialogResult.Cancel);
                }
            }
            else // Add Mode
            {
                if (lblTitle != null) lblTitle.Text = "Thêm Phương Tiện Mới";
                _vehicleId = 0;
                _originalData = new Vehicle(); // Dữ liệu gốc rỗng

                txtPlateNo.Clear();
                cmbType.SelectedIndex = -1; // Bắt đầu chưa chọn category
                cmbStatus.SelectedIndex = 0; // Mặc định là Active
            }
        }

        private void LoadComboBoxes() // Chỉ load cmbType (categories) ban đầu
        {
            // Load Status ComboBox (như cũ)
            var statusOptions = new Dictionary<VehicleStatus, string>
            {
                { VehicleStatus.Active, "Hoạt động" },
                { VehicleStatus.Maintenance, "Đang bảo trì" },
                { VehicleStatus.Inactive, "Ngừng hoạt động" }
            };
            cmbStatus.DataSource = new BindingSource(statusOptions, null);
            cmbStatus.DisplayMember = "Value";
            cmbStatus.ValueMember = "Key";
            cmbStatus.SelectedIndex = 0;

            // *** NEW: Load cmbType with categories ***
            cmbType.Items.Clear();
            cmbType.Items.AddRange(_vehicleTypesByCategory.Keys.ToArray()); // Lấy danh sách category
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList; // Chỉ cho chọn
            cmbType.SelectedIndex = -1; // Chưa chọn gì ban đầu
        }

        // *** NEW: Method to populate cmbSpecificType based on selected category ***
        private void PopulateSpecificTypes(string category)
        {
            cmbSpecificType.DataSource = null; // Reset binding
            cmbSpecificType.Items.Clear();
            if (!string.IsNullOrEmpty(category) && _vehicleTypesByCategory.ContainsKey(category))
            {
                cmbSpecificType.DataSource = _vehicleTypesByCategory[category]; // Bind list VehicleTypeInfo
                cmbSpecificType.DisplayMember = "Name"; // Hiển thị tên
                cmbSpecificType.SelectedIndex = -1; // Reset selection
                cmbSpecificType.Enabled = true;
            }
            else
            {
                cmbSpecificType.Enabled = false;
            }
        }

        #endregion

        #region Event Wiring & Handlers

        private void WireEvents()
        {
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

            // *** NEW: Handle category selection change ***
            cmbType.SelectedIndexChanged += CmbType_SelectedIndexChanged;

            // Dragging events (như cũ)
            Control dragHandle = pnlTop;
            if (dragHandle != null)
            {
                // ... (code gán sự kiện kéo thả như cũ) ...
                dragHandle.MouseDown += DragHandle_MouseDown;
                dragHandle.MouseMove += DragHandle_MouseMove;
                dragHandle.MouseUp += DragHandle_MouseUp;
            }
        }

        // *** NEW: Event handler for category change ***
        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = cmbType.SelectedItem as string;
            PopulateSpecificTypes(selectedCategory);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                MessageBox.Show("Vui lòng kiểm tra lại thông tin.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ... (code confirm message như cũ) ...
            var confirmMsg = (_mode == EditorMode.Add) ? "Thêm phương tiện mới?" : "Lưu thay đổi?";
            if (MessageBox.Show(confirmMsg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }


            Vehicle vehicleToSave = (_mode == EditorMode.Edit && _originalData != null) ? _originalData : new Vehicle();

            // *** NEW: Get data from ComboBoxes ***
            var selectedSpecificType = cmbSpecificType.SelectedItem as VehicleTypeInfo;

            vehicleToSave.PlateNo = txtPlateNo.Text.Trim().ToUpper();
            vehicleToSave.Type = selectedSpecificType.Name; // Lấy tên từ loại xe cụ thể
            vehicleToSave.CapacityKg = selectedSpecificType.CapacityKg; // Lấy capacity từ loại xe cụ thể
            vehicleToSave.Status = (VehicleStatus)cmbStatus.SelectedValue;

            try
            {
                // ... (code gọi CreateVehicle hoặc UpdateVehicle như cũ) ...
                if (_mode == EditorMode.Add)
                {
                    _vehicleSvc.CreateVehicle(vehicleToSave);
                }
                else
                {
                    _vehicleSvc.UpdateVehicle(vehicleToSave);
                }

                // ... (code thông báo thành công và đóng form như cũ) ...
                MessageBox.Show("Lưu thông tin phương tiện thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.FindForm()?.CloseDialog(DialogResult.OK);
            }
            catch (InvalidOperationException opEx)
            {
                // ... (code xử lý lỗi như cũ) ...
                MessageBox.Show(opEx.Message, "Lỗi Nghiệp Vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                // ... (code xử lý lỗi như cũ) ...
                MessageBox.Show($"Lỗi khi lưu phương tiện: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // ... (code kiểm tra HasChanges và đóng form như cũ) ...
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

            // Validate PlateNo (như cũ)
            if (string.IsNullOrWhiteSpace(txtPlateNo.Text))
            {
                // ... (code cũ) ...
                errProvider.SetError(txtPlateNo, "Biển số xe không được để trống.");
                isValid = false;
            }

            // *** NEW: Validate ComboBox selections ***
            if (cmbType.SelectedIndex == -1)
            {
                errProvider.SetError(cmbType, "Vui lòng chọn nhóm xe.");
                isValid = false;
            }
            if (cmbSpecificType.SelectedIndex == -1)
            {
                // Chỉ báo lỗi nếu cmbType đã được chọn (tức là cmbSpecificType đã enable)
                if (cmbType.SelectedIndex != -1)
                {
                    errProvider.SetError(cmbSpecificType, "Vui lòng chọn loại xe cụ thể.");
                    isValid = false;
                }
            }

            // Remove capacity validation (không cần nữa)
            // if (!string.IsNullOrWhiteSpace(txtCapacity.Text)) { ... }

            // Validate Status (như cũ)
            if (cmbStatus.SelectedValue == null)
            {
                // ... (code cũ) ...
                errProvider.SetError(cmbStatus, "Vui lòng chọn trạng thái.");
                isValid = false;
            }

            return isValid;
        }


        private bool HasChanges()
        {
            if (_originalData == null && _mode == EditorMode.Edit) return false;

            var selectedSpecificType = cmbSpecificType.SelectedItem as VehicleTypeInfo;
            string currentType = selectedSpecificType?.Name ?? "";
            int? currentCapacity = selectedSpecificType?.CapacityKg;
            VehicleStatus currentStatus = (VehicleStatus?)cmbStatus.SelectedValue ?? VehicleStatus.Active; // Mặc định nếu null

            if (_mode == EditorMode.Add)
            {
                // Kiểm tra xem có trường nào được nhập/chọn khác giá trị mặc định không
                return !string.IsNullOrWhiteSpace(txtPlateNo.Text) ||
                       cmbType.SelectedIndex != -1 ||
                       cmbSpecificType.SelectedIndex != -1 ||
                       currentStatus != VehicleStatus.Active;
            }
            else // Edit Mode
            {
                if (_originalData == null) return false;
                // So sánh dữ liệu hiện tại với dữ liệu gốc
                return _originalData.PlateNo != txtPlateNo.Text.Trim().ToUpper() ||
                       _originalData.Type != currentType ||
                       _originalData.CapacityKg != currentCapacity ||
                       _originalData.Status != currentStatus;
            }
        }


        #endregion

        #region Form Dragging Logic
        // ... (code kéo thả form như cũ) ...
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

    // *** NEW: Extension method for DialogResult (optional, convenience) ***
    public static class FormExtensions
    {
        public static void CloseDialog(this Form form, DialogResult result)
        {
            if (form != null)
            {
                form.DialogResult = result;
                form.Close();
            }
        }
    }
}