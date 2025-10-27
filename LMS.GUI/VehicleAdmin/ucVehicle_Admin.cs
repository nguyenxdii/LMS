//using Guna.UI2.WinForms;
//using LMS.BUS.Helpers;
//using LMS.BUS.Services; // Cần VehicleService_Admin và DriverService
//using LMS.DAL.Models;
//using LMS.GUI.OrderAdmin; // Namespace của ucDriverPicker_Admin
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;

//namespace LMS.GUI.VehicleAdmin
//{
//    public partial class ucVehicle_Admin : UserControl
//    {
//        private readonly VehicleService_Admin _vehicleSvc = new VehicleService_Admin();
//        // private readonly DriverService _driverSvc = new DriverService(); // Bỏ comment nếu cần
//        private BindingList<Vehicle> _bindingList; // Binding trực tiếp với Model Vehicle
//        private DataGridViewColumn _sortedColumn = null;
//        private SortOrder _sortOrder = SortOrder.None;

//        public ucVehicle_Admin()
//        {
//            InitializeComponent();
//        }

//        protected override void OnLoad(EventArgs e)
//        {
//            base.OnLoad(e);
//            ConfigureGrid();
//            WireEvents();
//            LoadData();
//        }

//        #region Grid Config & Formatting
//        private void ConfigureGrid()
//        {
//            dgvVehicles.Columns.Clear();
//            dgvVehicles.ApplyBaseStyle();

//            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
//            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "PlateNo", DataPropertyName = "PlateNo", HeaderText = "Biển số xe", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Type", DataPropertyName = "Type", HeaderText = "Loại xe", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 30, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "CapacityKg", DataPropertyName = "CapacityKg", HeaderText = "Trọng tải (kg)", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" }, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            // *** SỬA DataPropertyName CHO CỘT DriverName ***
//            // DataPropertyName trỏ đến thuộc tính ảo Driver, CellFormatting sẽ lấy FullName
//            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "DriverName", DataPropertyName = "Driver", HeaderText = "Tài xế", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });

//            dgvVehicles.CellFormatting += DgvVehicles_CellFormatting;
//            dgvVehicles.ColumnHeaderMouseClick += dgvVehicles_ColumnHeaderMouseClick;
//            dgvVehicles.SelectionChanged += (s, e) => UpdateButtonsState();
//            dgvVehicles.CellDoubleClick += (s, ev) => { if (ev.RowIndex >= 0) OpenEditorPopup(GetSelectedVehicleId()); }; // Double click để sửa
//        }

//        // Format Status và lấy tên Driver từ thuộc tính điều hướng
//        private void DgvVehicles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            // Format Status
//            if (dgvVehicles.Columns[e.ColumnIndex].Name == "Status" && e.Value is VehicleStatus status)
//            {
//                e.Value = FormatVehicleStatus(status);
//                e.FormattingApplied = true;
//            }
//            // *** SỬA CÁCH LẤY TÊN DRIVER ***
//            // Lấy tên Driver từ thuộc tính điều hướng "Driver" (DataPropertyName="Driver")
//            else if (dgvVehicles.Columns[e.ColumnIndex].Name == "DriverName" && e.Value is Driver driver) // e.Value bây giờ là đối tượng Driver (nếu service Include)
//            {
//                e.Value = driver.FullName; // Lấy FullName từ đối tượng Driver
//                e.FormattingApplied = true;
//            }
//            else if (dgvVehicles.Columns[e.ColumnIndex].Name == "DriverName" && e.Value == null && e.RowIndex >= 0) // Xử lý trường hợp Driver là null
//            {
//                e.Value = "(Chưa gán)";
//                e.FormattingApplied = true;
//            }
//        }

//        // Helper format Enum Status
//        private string FormatVehicleStatus(VehicleStatus status)
//        {
//            switch (status)
//            {
//                case VehicleStatus.Active: return "Hoạt động";
//                case VehicleStatus.Maintenance: return "Bảo trì";
//                case VehicleStatus.Inactive: return "Ngừng hoạt động";
//                default: return status.ToString();
//            }
//        }

//        // *** XÓA HÀM GetDriverName(Vehicle vehicle) ĐI VÌ KHÔNG DÙNG NỮA ***
//        // private string GetDriverName(Vehicle vehicle) { ... }

//        #endregion

//        #region Data Loading & Helpers
//        private void LoadData()
//        {
//            try
//            {
//                // Service GetVehiclesForAdmin cần Include(v => v.Driver)
//                var vehicles = _vehicleSvc.GetVehiclesForAdmin();

//                _bindingList = new BindingList<Vehicle>(vehicles);
//                dgvVehicles.DataSource = _bindingList;

//                if (dgvVehicles.Rows.Count > 0)
//                {
//                    dgvVehicles.ClearSelection();
//                    dgvVehicles.Rows[0].Selected = true;
//                }

//                ResetSortGlyphs();
//                UpdateButtonsState();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tải dữ liệu phương tiện: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                dgvVehicles.DataSource = null;
//            }
//        }

//        private int? GetSelectedVehicleId()
//        {
//            return (dgvVehicles.CurrentRow?.DataBoundItem as Vehicle)?.Id;
//        }
//        private Vehicle GetSelectedVehicle()
//        {
//            return dgvVehicles.CurrentRow?.DataBoundItem as Vehicle;
//        }


//        // *** SỬA HÀM NÀY ***
//        private void UpdateButtonsState()
//        {
//            Vehicle selectedVehicle = GetSelectedVehicle();
//            bool hasSelection = selectedVehicle != null;

//            btnEdit.Enabled = hasSelection;
//            btnDelete.Enabled = hasSelection;

//            // Nút gán chỉ bật khi có xe được chọn VÀ xe đó chưa có tài xế (kiểm tra thuộc tính Driver)
//            btnAssignDriver.Enabled = hasSelection && selectedVehicle.Driver == null;
//            // Nút gỡ chỉ bật khi có xe được chọn VÀ xe đó ĐANG có tài xế (kiểm tra thuộc tính Driver)
//            btnUnassignDriver.Enabled = hasSelection && selectedVehicle.Driver != null;
//        }


//        private void SelectRowById(int vehicleId)
//        {
//            if (dgvVehicles.Rows.Count == 0 || _bindingList == null) return;
//            Vehicle itemToSelect = _bindingList.FirstOrDefault(v => v.Id == vehicleId);
//            if (itemToSelect != null)
//            {
//                int rowIndex = _bindingList.IndexOf(itemToSelect);
//                if (rowIndex >= 0 && rowIndex < dgvVehicles.Rows.Count)
//                {
//                    dgvVehicles.ClearSelection();
//                    dgvVehicles.Rows[rowIndex].Selected = true;
//                    if (dgvVehicles.Columns.Contains("PlateNo"))
//                        dgvVehicles.CurrentCell = dgvVehicles.Rows[rowIndex].Cells["PlateNo"];

//                    if (!dgvVehicles.Rows[rowIndex].Displayed)
//                    {
//                        int firstDisplayed = Math.Max(0, rowIndex - dgvVehicles.DisplayedRowCount(false) / 2);
//                        dgvVehicles.FirstDisplayedScrollingRowIndex = Math.Min(firstDisplayed, dgvVehicles.RowCount - 1);
//                    }
//                }
//            }
//            UpdateButtonsState();
//        }
//        #endregion

//        #region Event Wiring
//        private void WireEvents()
//        {
//            btnReload.Click += (s, e) => LoadData();
//            btnAdd.Click += (s, e) => OpenEditorPopup(null); // Add mode
//            btnEdit.Click += (s, e) => OpenEditorPopup(GetSelectedVehicleId()); // Edit mode
//            btnDelete.Click += (s, e) => DeleteVehicle();
//            btnSearch.Click += (s, e) => OpenSearchPopup();
//            btnAssignDriver.Click += (s, e) => AssignDriver();
//            btnUnassignDriver.Click += (s, e) => UnassignDriver();

//            // Grid events wired in ConfigureGrid
//        }
//        #endregion

//        #region Actions (Popup Opening, Delete, Assign/Unassign)

//        // *** SỬA HÀM NÀY ***
//        private void OpenEditorPopup(int? vehicleId)
//        {
//            if (vehicleId == null && !btnAdd.Enabled) return;
//            if (vehicleId.HasValue && !btnEdit.Enabled) return;

//            // Xác định chế độ dựa trên vehicleId, không dùng enum EditorMode ở đây
//            var mode = vehicleId.HasValue ? ucVehicleEditor_Admin.EditorMode.Edit : ucVehicleEditor_Admin.EditorMode.Add;

//            try
//            {
//                var ucEditor = new ucVehicleEditor_Admin();
//                // Truyền chế độ và ID vào hàm LoadData của editor
//                ucEditor.LoadData(mode, vehicleId);

//                using (var popupForm = new Form
//                {
//                    StartPosition = FormStartPosition.CenterScreen,
//                    FormBorderStyle = FormBorderStyle.None,
//                    Size = new Size(916, 392), // Kích thước yêu cầu
//                })
//                {
//                    popupForm.Controls.Add(ucEditor);
//                    ucEditor.Dock = DockStyle.Fill;

//                    Form ownerForm = this.FindForm();
//                    if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }

//                    if (popupForm.ShowDialog(ownerForm) == DialogResult.OK)
//                    {
//                        LoadData(); // Reload nếu lưu thành công
//                        // Chọn lại dòng vừa sửa/thêm (nếu có ID trả về từ editor thì tốt hơn)
//                        if (vehicleId.HasValue)
//                        {
//                            SelectRowById(vehicleId.Value);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi mở trình chỉnh sửa: {ex.Message}", "Lỗi"); }
//        }


//        // (Các hàm OpenSearchPopup, DeleteVehicle, AssignDriver, UnassignDriver giữ nguyên)
//        private void OpenSearchPopup()
//        {
//            int? selectedId = null;
//            using (var searchForm = new Form
//            {
//                StartPosition = FormStartPosition.CenterScreen,
//                FormBorderStyle = FormBorderStyle.None,
//                Size = new Size(988, 689), // Kích thước yêu cầu
//            })
//            {
//                var ucSearch = new ucVehicleSearch_Admin { Dock = DockStyle.Fill };
//                ucSearch.VehiclePicked += (sender, id) =>
//                {
//                    selectedId = id;
//                    searchForm.DialogResult = DialogResult.OK;
//                    searchForm.Close(); // Đóng khi đã chọn
//                };
//                searchForm.Controls.Add(ucSearch);

//                Form ownerForm = this.FindForm();
//                if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }

//                if (searchForm.ShowDialog(ownerForm) == DialogResult.OK && selectedId.HasValue)
//                {
//                    SelectRowById(selectedId.Value); // Chọn dòng trên grid chính
//                }
//            }
//        }
//        private void DeleteVehicle()
//        {
//            var vehicleId = GetSelectedVehicleId();
//            if (!vehicleId.HasValue) { MessageBox.Show("Vui lòng chọn phương tiện.", "Thông báo"); return; }

//            // TODO: Add check in VehicleService_Admin if vehicle is in use by active shipments
//            // bool isInUse = _vehicleSvc.IsVehicleInUse(vehicleId.Value);
//            // if (isInUse) { MessageBox.Show("Không thể xóa phương tiện đang được sử dụng...", "Không thể xóa"); return; }

//            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa phương tiện ID {vehicleId.Value}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
//            if (confirm == DialogResult.Yes)
//            {
//                try
//                {
//                    // TODO: Implement DeleteVehicle in VehicleService_Admin
//                    _vehicleSvc.DeleteVehicle(vehicleId.Value);
//                    MessageBox.Show("Đã xóa phương tiện.", "Hoàn tất");
//                    LoadData(); // Load lại sau khi xóa
//                }
//                catch (InvalidOperationException opEx) // Bắt lỗi nghiệp vụ (vd: đang sử dụng)
//                {
//                    MessageBox.Show(opEx.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi");
//                }
//            }
//        }
//        private void AssignDriver()
//        {
//            var selectedVehicle = GetSelectedVehicle();
//            if (selectedVehicle == null || !btnAssignDriver.Enabled)
//            {
//                MessageBox.Show("Vui lòng chọn một phương tiện chưa được gán tài xế.", "Thông báo");
//                return;
//            }

//            int? selectedDriverId = null;
//            using (var fPicker = new Form
//            {
//                Text = "Chọn Tài Xế (Chỉ tài xế chưa có xe)", // Cập nhật tiêu đề
//                StartPosition = FormStartPosition.CenterScreen,
//                Size = new Size(583, 539), // Size của ucDriverPicker
//                FormBorderStyle = FormBorderStyle.None,
//            })
//            {
//                // *** QUAN TRỌNG: Cần sửa ucDriverPicker_Admin hoặc tạo UC mới
//                // để chỉ hiển thị tài xế CHƯA được gán xe khác ***
//                // Tạm thời vẫn dùng ucDriverPicker_Admin gốc (có thể báo lỗi nếu chọn tài xế đã có xe)
//                var ucPicker = new ucDriverPicker_Admin(); // Cần sửa để lọc tài xế
//                ucPicker.DriverSelected += (selectedId) =>
//                {
//                    selectedDriverId = selectedId;
//                    fPicker.DialogResult = DialogResult.OK;
//                };
//                fPicker.Controls.Add(ucPicker);
//                Form ownerForm = this.FindForm();
//                if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }

//                if (fPicker.ShowDialog(ownerForm) == DialogResult.OK && selectedDriverId.HasValue)
//                {
//                    if (MessageBox.Show($"Gán tài xế ID {selectedDriverId.Value} cho xe {selectedVehicle.PlateNo}?",
//                                        "Xác nhận gán tài xế", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//                    {
//                        try
//                        {
//                            // TODO: Implement AssignDriverToVehicle in VehicleService_Admin
//                            _vehicleSvc.AssignDriverToVehicle(selectedVehicle.Id, selectedDriverId.Value);
//                            MessageBox.Show("Đã gán tài xế thành công.", "Thông báo");
//                            LoadData();
//                            SelectRowById(selectedVehicle.Id);
//                        }
//                        catch (InvalidOperationException opEx) // Bắt lỗi nghiệp vụ (vd: tài xế đã có xe)
//                        {
//                            MessageBox.Show(opEx.Message, "Lỗi Nghiệp Vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                        }
//                        catch (Exception ex)
//                        {
//                            MessageBox.Show($"Lỗi khi gán tài xế: {ex.Message}", "Lỗi");
//                        }
//                    }
//                }
//            }
//        }
//        private void UnassignDriver()
//        {
//            var selectedVehicle = GetSelectedVehicle();
//            if (selectedVehicle == null || !btnUnassignDriver.Enabled)
//            {
//                MessageBox.Show("Vui lòng chọn một phương tiện đang được gán tài xế.", "Thông báo");
//                return;
//            }

//            if (MessageBox.Show($"Bạn có chắc muốn gỡ tài xế khỏi xe {selectedVehicle.PlateNo}?",
//                                "Xác nhận gỡ tài xế", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//            {
//                try
//                {
//                    // TODO: Implement UnassignDriverFromVehicle in VehicleService_Admin
//                    _vehicleSvc.UnassignDriverFromVehicle(selectedVehicle.Id);
//                    MessageBox.Show("Đã gỡ tài xế khỏi phương tiện.", "Thông báo");
//                    LoadData();
//                    SelectRowById(selectedVehicle.Id);
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Lỗi khi gỡ tài xế: {ex.Message}", "Lỗi");
//                }
//            }
//        }


//        #endregion

//        #region Sorting Logic
//        private void dgvVehicles_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            if (_bindingList == null || _bindingList.Count == 0) return;
//            var newColumn = dgvVehicles.Columns[e.ColumnIndex];
//            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

//            // Xử lý riêng cột DriverName (vì nó là object)
//            if (newColumn.Name == "DriverName")
//            {
//                // Sort dựa trên FullName của Driver object
//                string propertyName = "Driver.FullName"; // Chuỗi truy cập thuộc tính lồng nhau
//                ApplySortNestedProperty(propertyName);
//                UpdateSortGlyphs();
//                _sortedColumn = newColumn; // Cập nhật cột đang sort
//                return; // Kết thúc sớm vì đã xử lý sort riêng
//            }

//            // Xử lý sort cho các cột thông thường
//            if (_sortedColumn == newColumn) { _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
//            else { if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _sortOrder = SortOrder.Ascending; _sortedColumn = newColumn; }

//            ApplySort();
//            UpdateSortGlyphs();
//        }

//        // Hàm sort cho các thuộc tính thông thường
//        private void ApplySort()
//        {
//            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;
//            string propertyName = _sortedColumn.DataPropertyName;
//            PropertyInfo propInfo = typeof(Vehicle).GetProperty(propertyName);
//            if (propInfo == null) return;

//            List<Vehicle> items = _bindingList.ToList();
//            List<Vehicle> sortedList;
//            try
//            {
//                if (_sortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
//                else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }
//                _bindingList = new BindingList<Vehicle>(sortedList);
//                dgvVehicles.DataSource = _bindingList;
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp: {ex.Message}"); ResetSortGlyphs(); }
//        }

//        // Hàm sort riêng cho thuộc tính lồng nhau (Driver.FullName)
//        private void ApplySortNestedProperty(string nestedPropertyName)
//        {
//            if (_bindingList == null || _bindingList.Count == 0) return;
//            List<Vehicle> items = _bindingList.ToList();
//            List<Vehicle> sortedList;

//            try
//            {
//                // Hàm helper để lấy giá trị thuộc tính lồng nhau một cách an toàn
//                Func<object, string, object> GetNestedPropertyValue = (obj, propName) =>
//                {
//                    if (obj == null) return null;
//                    string[] parts = propName.Split('.');
//                    object currentObj = obj;
//                    foreach (var part in parts)
//                    {
//                        if (currentObj == null) return null;
//                        var prop = currentObj.GetType().GetProperty(part);
//                        if (prop == null) return null;
//                        currentObj = prop.GetValue(currentObj, null);
//                    }
//                    return currentObj;
//                };


//                if (_sortOrder == SortOrder.Ascending)
//                {
//                    // Xử lý null: Đẩy null về cuối khi tăng dần
//                    sortedList = items.OrderBy(x => GetNestedPropertyValue(x, nestedPropertyName) == null)
//                                      .ThenBy(x => GetNestedPropertyValue(x, nestedPropertyName))
//                                      .ToList();
//                }
//                else // Descending
//                {
//                    // Xử lý null: Đẩy null về cuối khi giảm dần
//                    sortedList = items.OrderBy(x => GetNestedPropertyValue(x, nestedPropertyName) == null)
//                                      .ThenByDescending(x => GetNestedPropertyValue(x, nestedPropertyName))
//                                      .ToList();
//                }
//                _bindingList = new BindingList<Vehicle>(sortedList);
//                dgvVehicles.DataSource = _bindingList;
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp lồng nhau: {ex.Message}"); ResetSortGlyphs(); }
//        }


//        private void UpdateSortGlyphs()
//        {
//            foreach (DataGridViewColumn col in dgvVehicles.Columns)
//            {
//                if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None;
//                if (_sortedColumn != null && col == _sortedColumn) col.HeaderCell.SortGlyphDirection = _sortOrder;
//            }
//        }
//        private void ResetSortGlyphs()
//        {
//            if (_sortedColumn != null && _sortedColumn.HeaderCell != null) // Check HeaderCell null
//                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//            _sortedColumn = null; _sortOrder = SortOrder.None;
//            UpdateSortGlyphs();
//        }
//        #endregion
//    }
//}

//================





// LMS.GUI/VehicleAdmin/ucVehicle_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.GUI.OrderAdmin;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.VehicleAdmin
{
    public partial class ucVehicle_Admin : UserControl
    {
        private readonly VehicleService_Admin _vehicleSvc = new VehicleService_Admin();
        private BindingList<Vehicle> _bindingList;
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        public ucVehicle_Admin()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureGrid();
            WireEvents();
            LoadData();

            // Đăng ký nghe sự kiện thay đổi gán xe
            AppSession.VehicleAssignmentChanged += OnVehicleAssignmentChanged;

            // Hủy đăng ký khi control bị destroy (tránh cần override Dispose ở đây)
            this.HandleDestroyed += (s, _) =>
            {
                try { AppSession.VehicleAssignmentChanged -= OnVehicleAssignmentChanged; } catch { }
            };
        }

        private void OnVehicleAssignmentChanged()
        {
            if (!IsDisposed) LoadData();
        }

        // ========== GRID ==========
        private void ConfigureGrid()
        {
            dgvVehicles.Columns.Clear();
            dgvVehicles.ApplyBaseStyle();

            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "PlateNo", DataPropertyName = "PlateNo", HeaderText = "Biển số xe", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Type", DataPropertyName = "Type", HeaderText = "Loại xe", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 30, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "CapacityKg", DataPropertyName = "CapacityKg", HeaderText = "Trọng tải (kg)", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" }, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "DriverName", DataPropertyName = "Driver", HeaderText = "Tài xế", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });

            dgvVehicles.CellFormatting += DgvVehicles_CellFormatting;
            dgvVehicles.ColumnHeaderMouseClick += dgvVehicles_ColumnHeaderMouseClick;
            dgvVehicles.SelectionChanged += (s, e) => UpdateButtonsState();
            dgvVehicles.CellDoubleClick += (s, ev) => { if (ev.RowIndex >= 0) OpenEditorPopup(GetSelectedVehicleId()); };
        }

        private void DgvVehicles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvVehicles.Columns[e.ColumnIndex].Name == "Status" && e.Value is VehicleStatus st)
            {
                e.Value = st == VehicleStatus.Active ? "Hoạt động"
                         : st == VehicleStatus.Maintenance ? "Bảo trì"
                         : st == VehicleStatus.Inactive ? "Ngừng hoạt động"
                         : st.ToString();
                e.FormattingApplied = true;
            }
            else if (dgvVehicles.Columns[e.ColumnIndex].Name == "DriverName")
            {
                if (e.Value is Driver d) { e.Value = d.FullName; e.FormattingApplied = true; }
                else { e.Value = "(Chưa gán)"; e.FormattingApplied = true; }
            }
        }

        private void LoadData()
        {
            try
            {
                var vehicles = _vehicleSvc.GetVehiclesForAdmin();
                _bindingList = new BindingList<Vehicle>(vehicles);
                dgvVehicles.DataSource = _bindingList;

                if (dgvVehicles.Rows.Count > 0)
                {
                    dgvVehicles.ClearSelection();
                    dgvVehicles.Rows[0].Selected = true;
                }
                ResetSortGlyphs();
                UpdateButtonsState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu phương tiện: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvVehicles.DataSource = null;
            }
        }

        private int? GetSelectedVehicleId() => (dgvVehicles.CurrentRow?.DataBoundItem as Vehicle)?.Id;
        private Vehicle GetSelectedVehicle() => dgvVehicles.CurrentRow?.DataBoundItem as Vehicle;

        private void UpdateButtonsState()
        {
            var v = GetSelectedVehicle();
            bool has = v != null;

            btnEdit.Enabled = has;
            btnDelete.Enabled = has;
            btnAssignDriver.Enabled = has && v.Driver == null;
            btnUnassignDriver.Enabled = has && v.Driver != null;
        }

        private void SelectRowById(int id)
        {
            if (dgvVehicles.Rows.Count == 0 || _bindingList == null) return;
            var item = _bindingList.FirstOrDefault(x => x.Id == id);
            if (item == null) return;

            int rowIndex = _bindingList.IndexOf(item);
            if (rowIndex < 0) return;

            dgvVehicles.ClearSelection();
            dgvVehicles.Rows[rowIndex].Selected = true;
            if (dgvVehicles.Columns.Contains("PlateNo"))
                dgvVehicles.CurrentCell = dgvVehicles.Rows[rowIndex].Cells["PlateNo"];
        }

        private void WireEvents()
        {
            btnReload.Click += (s, e) => LoadData();
            btnAdd.Click += (s, e) => OpenEditorPopup(null);
            btnEdit.Click += (s, e) => OpenEditorPopup(GetSelectedVehicleId());
            btnDelete.Click += (s, e) => DeleteVehicle();
            btnSearch.Click += (s, e) => OpenSearchPopup();
            btnAssignDriver.Click += (s, e) => AssignDriver();
            btnUnassignDriver.Click += (s, e) => UnassignDriver();
        }

        private void OpenEditorPopup(int? vehicleId)
        {
            if (vehicleId == null && !btnAdd.Enabled) return;
            if (vehicleId.HasValue && !btnEdit.Enabled) return;

            var mode = vehicleId.HasValue ? ucVehicleEditor_Admin.EditorMode.Edit : ucVehicleEditor_Admin.EditorMode.Add;

            try
            {
                var ucEditor = new ucVehicleEditor_Admin();
                ucEditor.LoadData(mode, vehicleId);

                using (var popup = new Form
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.None,
                    Size = new System.Drawing.Size(916, 392)
                })
                {
                    popup.Controls.Add(ucEditor);
                    ucEditor.Dock = DockStyle.Fill;
                    var owner = this.FindForm();
                    if (owner == null) { MessageBox.Show("Không xác định form cha.", "Lỗi"); return; }

                    if (popup.ShowDialog(owner) == DialogResult.OK)
                    {
                        LoadData();
                        if (vehicleId.HasValue) SelectRowById(vehicleId.Value);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi mở trình chỉnh sửa: {ex.Message}", "Lỗi"); }
        }

        private void OpenSearchPopup()
        {
            int? selectedId = null;
            using (var f = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new System.Drawing.Size(988, 689)
            })
            {
                var ucSearch = new ucVehicleSearch_Admin { Dock = DockStyle.Fill };
                ucSearch.VehiclePicked += (s, id) => { selectedId = id; f.DialogResult = DialogResult.OK; f.Close(); };
                f.Controls.Add(ucSearch);

                var owner = this.FindForm();
                if (owner == null) { MessageBox.Show("Không xác định form cha.", "Lỗi"); return; }

                if (f.ShowDialog(owner) == DialogResult.OK && selectedId.HasValue)
                    SelectRowById(selectedId.Value);
            }
        }

        private void DeleteVehicle()
        {
            var id = GetSelectedVehicleId();
            if (!id.HasValue) { MessageBox.Show("Vui lòng chọn phương tiện.", "Thông báo"); return; }

            if (MessageBox.Show($"Xóa phương tiện ID {id.Value}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    _vehicleSvc.DeleteVehicle(id.Value);
                    MessageBox.Show("Đã xóa.", "Hoàn tất");
                    LoadData();
                }
                catch (InvalidOperationException opEx) { MessageBox.Show(opEx.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                catch (Exception ex) { MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi"); }
            }
        }

        private void AssignDriver()
        {
            var v = GetSelectedVehicle();
            if (v == null || !btnAssignDriver.Enabled)
            {
                MessageBox.Show("Chọn một xe chưa gán tài xế.", "Thông báo");
                return;
            }

            int? driverId = null;
            using (var f = new Form
            {
                Text = "Chọn Tài Xế (chỉ tài xế rảnh & chưa có xe)",
                StartPosition = FormStartPosition.CenterScreen,
                Size = new System.Drawing.Size(583, 539),
                FormBorderStyle = FormBorderStyle.None
            })
            {
                var picker = new ucDriverPicker_Admin();
                picker.DriverSelected += (id) => { driverId = id; f.DialogResult = DialogResult.OK; };
                f.Controls.Add(picker);
                var owner = this.FindForm();
                if (owner == null) { MessageBox.Show("Không xác định form cha.", "Lỗi"); return; }

                if (f.ShowDialog(owner) == DialogResult.OK && driverId.HasValue)
                {
                    if (MessageBox.Show($"Gán tài xế ID {driverId.Value} cho xe {v.PlateNo}?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            _vehicleSvc.AssignDriverToVehicle(v.Id, driverId.Value);
                            MessageBox.Show("Đã gán tài xế.", "Thông báo");
                            LoadData();
                            SelectRowById(v.Id);
                        }
                        catch (InvalidOperationException opEx) { MessageBox.Show(opEx.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                        catch (Exception ex) { MessageBox.Show($"Lỗi gán tài xế: {ex.Message}", "Lỗi"); }
                    }
                }
            }
        }

        private void UnassignDriver()
        {
            var v = GetSelectedVehicle();
            if (v == null || !btnUnassignDriver.Enabled)
            {
                MessageBox.Show("Chọn một xe đang gán tài xế.", "Thông báo");
                return;
            }

            if (MessageBox.Show($"Gỡ tài xế khỏi xe {v.PlateNo}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _vehicleSvc.UnassignDriverFromVehicle(v.Id);
                    MessageBox.Show("Đã gỡ tài xế.", "Thông báo");
                    LoadData();
                    SelectRowById(v.Id);
                }
                catch (Exception ex) { MessageBox.Show($"Lỗi gỡ tài xế: {ex.Message}", "Lỗi"); }
            }
        }

        // ===== Sorting =====
        private void dgvVehicles_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;
            var newColumn = dgvVehicles.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (newColumn.Name == "DriverName")
            {
                ApplySortNestedProperty("Driver.FullName");
                UpdateSortGlyphs();
                _sortedColumn = newColumn;
                return;
            }

            if (_sortedColumn == newColumn)
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            else
            {
                if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                _sortOrder = SortOrder.Ascending;
                _sortedColumn = newColumn;
            }

            ApplySort();
            UpdateSortGlyphs();
        }

        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;
            string prop = _sortedColumn.DataPropertyName;
            PropertyInfo pi = typeof(Vehicle).GetProperty(prop);
            if (pi == null) return;

            var items = _bindingList.ToList();
            var sorted = (_sortOrder == SortOrder.Ascending)
                ? items.OrderBy(x => pi.GetValue(x, null)).ToList()
                : items.OrderByDescending(x => pi.GetValue(x, null)).ToList();

            _bindingList = new BindingList<Vehicle>(sorted);
            dgvVehicles.DataSource = _bindingList;
        }

        private void ApplySortNestedProperty(string nested)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;

            Func<object, string, object> get = (obj, name) =>
            {
                if (obj == null) return null;
                object cur = obj;
                foreach (var part in name.Split('.'))
                {
                    if (cur == null) return null;
                    var p = cur.GetType().GetProperty(part);
                    if (p == null) return null;
                    cur = p.GetValue(cur, null);
                }
                return cur;
            };

            var items = _bindingList.ToList();
            var sorted = (_sortOrder == SortOrder.Ascending)
                ? items.OrderBy(x => get(x, nested) == null).ThenBy(x => get(x, nested)).ToList()
                : items.OrderBy(x => get(x, nested) == null).ThenByDescending(x => get(x, nested)).ToList();

            _bindingList = new BindingList<Vehicle>(sorted);
            dgvVehicles.DataSource = _bindingList;
        }

        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvVehicles.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
                if (_sortedColumn != null && col == _sortedColumn)
                    col.HeaderCell.SortGlyphDirection = _sortOrder;
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null && _sortedColumn.HeaderCell != null)
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            _sortedColumn = null; _sortOrder = SortOrder.None;
            UpdateSortGlyphs();
        }
    }
}
