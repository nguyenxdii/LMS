//// LMS.GUI/DriverAdmin/ucDriver_Admin.cs
//using Guna.UI2.WinForms;
//using LMS.BUS.Dtos; // Đảm bảo using này nếu bạn có DTOs
//using LMS.BUS.Helpers;
//using LMS.BUS.Services;
//using LMS.DAL.Models;
//using LMS.GUI.OrderAdmin; // Đảm bảo namespace của ucDriverPicker_Admin là đúng
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data; // Có thể gây lỗi 'SortOrder', kiểm tra nếu không cần thì xóa
//using System.Drawing;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;
//// Nếu 'using System.Data.SqlClient;' tồn tại, hãy xóa nó đi

//namespace LMS.GUI.DriverAdmin
//{
//    public partial class ucDriver_Admin : UserControl
//    {
//        private readonly DriverService _driverSvc = new DriverService();
//        private BindingList<Driver> _bindingList;

//        // *** SỬA LỖI: Chỉ định rõ System.Windows.Forms.SortOrder ***
//        private DataGridViewColumn _sortedColumn = null;
//        private System.Windows.Forms.SortOrder _sortOrder = System.Windows.Forms.SortOrder.None;

//        public ucDriver_Admin()
//        {
//            InitializeComponent();
//            // Đảm bảo các nút btnAssignVehicle, btnUnassignVehicle tồn tại trong Designer (hoặc xóa code liên quan)
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
//            dgvDrivers.Columns.Clear();
//            dgvDrivers.ApplyBaseStyle();

//            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
//            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "FullName", DataPropertyName = "FullName", HeaderText = "Họ Tên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 30, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", DataPropertyName = "Phone", HeaderText = "Số điện thoại", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "CitizenId", DataPropertyName = "CitizenId", HeaderText = "CCCD", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "LicenseType", DataPropertyName = "LicenseType", HeaderText = "Hạng Bằng Lái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });

//            // *** SỬA CỘT NÀY: Hiển thị thông tin Phương tiện ***
//            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "VehicleInfo", // Tên cột mới
//                DataPropertyName = "Vehicle", // Trỏ đến thuộc tính điều hướng Vehicle
//                HeaderText = "Phương tiện",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
//                FillWeight = 40,
//                SortMode = DataGridViewColumnSortMode.Programmatic // Sẽ xử lý sort riêng
//            });

//            dgvDrivers.CellFormatting += DgvDrivers_CellFormatting;
//            dgvDrivers.ColumnHeaderMouseClick += dgvDrivers_ColumnHeaderMouseClick;
//            dgvDrivers.SelectionChanged += (s, e) => UpdateButtonsState();
//            dgvDrivers.CellDoubleClick += (s, ev) => { if (ev.RowIndex >= 0) OpenDetailPopup(GetSelectedDriverId()); };
//        }

//        // *** SỬA HÀM NÀY ***
//        // Format Status và lấy tên Driver
//        private void DgvDrivers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            if (e.RowIndex < 0 || e.Value == null) return;

//            // Xử lý cột Phương tiện
//            if (dgvDrivers.Columns[e.ColumnIndex].Name == "VehicleInfo" && e.Value is Vehicle vehicle)
//            {
//                // e.Value là đối tượng Vehicle (vì DataPropertyName = "Vehicle")
//                e.Value = $"{vehicle.PlateNo} ({vehicle.Type})"; // Hiển thị "Biển số (Loại xe)"
//                e.FormattingApplied = true;
//            }
//            // Xử lý khi xe là null (chưa gán)
//            else if (dgvDrivers.Columns[e.ColumnIndex].Name == "VehicleInfo" && e.Value == null)
//            {
//                e.Value = "(Chưa gán xe)";
//                e.FormattingApplied = true;
//            }
//        }

//        // Bỏ hàm FormatVehicleStatus (vì không có cột Status ở đây)
//        // Bỏ hàm GetDriverName (vì dùng thuộc tính điều hướng)

//        #endregion

//        #region Data Loading & Helpers
//        private void LoadData()
//        {
//            try
//            {
//                // *** ĐẢM BẢO SERVICE CÓ .Include(d => d.Vehicle) ***
//                var drivers = _driverSvc.GetDriversForAdmin();
//                _bindingList = new BindingList<Driver>(drivers);
//                dgvDrivers.DataSource = _bindingList;

//                if (dgvDrivers.Rows.Count > 0)
//                {
//                    dgvDrivers.ClearSelection();
//                    dgvDrivers.Rows[0].Selected = true;
//                }

//                ResetSortGlyphs();
//                UpdateButtonsState();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tải dữ liệu tài xế: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                dgvDrivers.DataSource = null;
//            }
//        }

//        private int? GetSelectedDriverId()
//        {
//            return (dgvDrivers.CurrentRow?.DataBoundItem as Driver)?.Id;
//        }

//        private Driver GetSelectedDriver()
//        {
//            return dgvDrivers.CurrentRow?.DataBoundItem as Driver;
//        }

//        // *** SỬA HÀM NÀY ***
//        private void UpdateButtonsState()
//        {
//            Driver selectedDriver = GetSelectedDriver(); // Lấy đối tượng Driver đầy đủ
//            bool hasSelection = (selectedDriver != null);

//            btnEdit.Enabled = hasSelection;
//            btnDelete.Enabled = hasSelection;
//            btnViewDetail.Enabled = hasSelection;

//            // Giả sử bạn có 2 nút: btnAssignVehicle và btnUnassignVehicle
//            // Nút gán xe: Bật khi chọn tài xế VÀ tài xế đó CHƯA có xe
//            if (this.Controls.Find("btnAssignVehicle", true).FirstOrDefault() is Button btnAssignVehicle)
//                btnAssignVehicle.Enabled = hasSelection && selectedDriver.VehicleId == null;

//            // Nút gỡ xe: Bật khi chọn tài xế VÀ tài xế đó ĐANG có xe
//            if (this.Controls.Find("btnUnassignVehicle", true).FirstOrDefault() is Button btnUnassignVehicle)
//                btnUnassignVehicle.Enabled = hasSelection && selectedDriver.VehicleId != null;
//        }

//        private void SelectRowById(int driverId)
//        {
//            if (dgvDrivers.Rows.Count == 0 || _bindingList == null) return;
//            Driver driverToSelect = _bindingList.FirstOrDefault(d => d.Id == driverId);
//            if (driverToSelect != null)
//            {
//                int rowIndex = _bindingList.IndexOf(driverToSelect);
//                if (rowIndex >= 0 && rowIndex < dgvDrivers.Rows.Count)
//                {
//                    dgvDrivers.ClearSelection();
//                    dgvDrivers.Rows[rowIndex].Selected = true;
//                    if (dgvDrivers.Columns.Contains("FullName"))
//                        dgvDrivers.CurrentCell = dgvDrivers.Rows[rowIndex].Cells["FullName"];
//                    if (!dgvDrivers.Rows[rowIndex].Displayed)
//                    {
//                        int firstDisplayed = Math.Max(0, rowIndex - dgvDrivers.DisplayedRowCount(false) / 2);
//                        dgvDrivers.FirstDisplayedScrollingRowIndex = Math.Min(firstDisplayed, dgvDrivers.RowCount - 1);
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
//            btnAdd.Click += (s, e) => OpenEditorPopup(null);
//            btnEdit.Click += (s, e) => OpenEditorPopup(GetSelectedDriverId());
//            btnDelete.Click += (s, e) => DeleteDriver();
//            btnViewDetail.Click += (s, e) => OpenDetailPopup(GetSelectedDriverId());
//            btnSearch.Click += (s, e) => OpenSearchPopup();

//            // (Thêm sự kiện cho 2 nút gán/gỡ xe nếu có)
//            // if (btnAssignVehicle != null) btnAssignVehicle.Click += ...
//            // if (btnUnassignVehicle != null) btnUnassignVehicle.Click += ...
//        }
//        #endregion

//        #region Actions (Popup Opening & Deleting)

//        private void OpenEditorPopup(int? driverId)
//        {
//            if (driverId == null && !btnAdd.Enabled) return;
//            if (driverId.HasValue && !btnEdit.Enabled) return;

//            // *** SỬA: Phải tham chiếu đến enum của ucDriverEditor_Admin ***
//            var mode = driverId.HasValue ? ucDriverEditor_Admin.EditorMode.Edit : ucDriverEditor_Admin.EditorMode.Add;
//            var title = (mode == ucDriverEditor_Admin.EditorMode.Add) ? "Thêm Tài Xế" : "Sửa Tài Xế";

//            try
//            {
//                var ucEditor = new ucDriverEditor_Admin();
//                ucEditor.LoadData(mode, driverId);

//                using (var popupForm = new Form
//                {
//                    StartPosition = FormStartPosition.CenterScreen,
//                    FormBorderStyle = FormBorderStyle.None,
//                    Width = 657,
//                    Height = 689
//                })
//                {
//                    popupForm.Controls.Add(ucEditor);
//                    ucEditor.Dock = DockStyle.Fill; // Đặt Dock.Fill cho ucEditor
//                    Form ownerForm = this.FindForm();
//                    if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }
//                    var result = popupForm.ShowDialog(ownerForm);
//                    if (result == DialogResult.OK) { LoadData(); }
//                }
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi mở form {title}: {ex.Message}", "Lỗi"); }
//        }

//        private void OpenDetailPopup(int? driverId)
//        {
//            if (!driverId.HasValue) { MessageBox.Show("Vui lòng chọn tài xế.", "Thông báo"); return; }
//            try
//            {
//                var ucDetail = new ucDriverDetail_Admin(driverId.Value);
//                using (var popupForm = new Form
//                {
//                    StartPosition = FormStartPosition.CenterScreen,
//                    FormBorderStyle = FormBorderStyle.None,
//                    Width = 1199,
//                    Height = 689
//                })
//                {
//                    ucDetail.Dock = DockStyle.Fill;
//                    popupForm.Controls.Add(ucDetail);
//                    Form ownerForm = this.FindForm();
//                    if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }
//                    popupForm.ShowDialog(ownerForm);
//                }
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi mở chi tiết: {ex.Message}", "Lỗi"); }
//        }

//        private void DeleteDriver()
//        {
//            var driverId = GetSelectedDriverId();
//            if (!driverId.HasValue) { MessageBox.Show("Vui lòng chọn tài xế.", "Thông báo"); return; }
//            try
//            {
//                if (_driverSvc.CheckDriverHasShipments(driverId.Value))
//                {
//                    MessageBox.Show("Không thể xóa vì tài xế đã có lịch sử chuyến hàng.\nHãy xem xét khóa tài khoản.", "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                    return;
//                }
//                var confirm = MessageBox.Show("Tài xế này chưa có chuyến hàng.\nXóa vĩnh viễn tài xế và tài khoản liên quan?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                if (confirm == DialogResult.Yes)
//                {
//                    _driverSvc.DeleteNewDriver(driverId.Value);
//                    MessageBox.Show("Đã xóa tài xế.", "Hoàn tất");
//                    LoadData();
//                }
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi"); }
//        }

//        private void OpenSearchPopup()
//        {
//            int? selectedId = null;
//            using (var searchForm = new Form
//            {
//                StartPosition = FormStartPosition.CenterScreen,
//                FormBorderStyle = FormBorderStyle.None,
//                Size = new Size(1199, 689)
//            })
//            {
//                var ucSearch = new ucDriverSearch_Admin { Dock = DockStyle.Fill };
//                ucSearch.DriverPicked += (sender, id) =>
//                {
//                    selectedId = id;
//                    searchForm.DialogResult = DialogResult.OK;
//                    searchForm.Close();
//                };
//                searchForm.Controls.Add(ucSearch);
//                Form ownerForm = this.FindForm();
//                if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }

//                if (searchForm.ShowDialog(ownerForm) == DialogResult.OK && selectedId.HasValue)
//                {
//                    SelectRowById(selectedId.Value);
//                }
//            }
//        }

//        // (Thêm hàm AssignDriver và UnassignDriver nếu bạn đã thêm nút)
//        // ...

//        #endregion

//        #region Sorting Logic
//        // *** SỬA LẠI HÀM NÀY ***
//        private void dgvDrivers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            if (_bindingList == null || _bindingList.Count == 0) return;
//            var newColumn = dgvDrivers.Columns[e.ColumnIndex];
//            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

//            // *** SỬA LỖI: Chỉ định rõ System.Windows.Forms.SortOrder ***
//            if (_sortedColumn == newColumn)
//            {
//                _sortOrder = (_sortOrder == System.Windows.Forms.SortOrder.Ascending) ? System.Windows.Forms.SortOrder.Descending : System.Windows.Forms.SortOrder.Ascending;
//            }
//            else
//            {
//                if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
//                _sortOrder = System.Windows.Forms.SortOrder.Ascending;
//                _sortedColumn = newColumn; // Cập nhật cột đang sort
//            }

//            // Xử lý riêng cột VehicleInfo (vì nó là object)
//            if (newColumn.Name == "VehicleInfo")
//            {
//                ApplySortNestedProperty("Vehicle.PlateNo"); // Sort theo Biển số xe
//            }
//            else // Sort cho các cột thông thường
//            {
//                ApplySort();
//            }
//            UpdateSortGlyphs();
//        }

//        // *** SỬA HÀM NÀY (Đổi typeof(Driver) -> typeof(Driver)) ***
//        // Hàm này đã ĐÚNG, chỉ cần đảm bảo nó tồn tại
//        private void ApplySort()
//        {
//            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;
//            string propertyName = _sortedColumn.DataPropertyName;
//            PropertyInfo propInfo = typeof(Driver).GetProperty(propertyName); // Dùng typeof(Driver)
//            if (propInfo == null) return;

//            List<Driver> items = _bindingList.ToList();
//            List<Driver> sortedList;
//            try
//            {
//                // *** SỬA LỖI: Chỉ định rõ System.Windows.Forms.SortOrder ***
//                if (_sortOrder == System.Windows.Forms.SortOrder.Ascending)
//                {
//                    sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList();
//                }
//                else
//                {
//                    sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();
//                }

//                _bindingList = new BindingList<Driver>(sortedList);
//                dgvDrivers.DataSource = _bindingList;
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp: {ex.Message}"); ResetSortGlyphs(); }
//        }

//        // *** THÊM HÀM NÀY ***
//        private void ApplySortNestedProperty(string nestedPropertyName)
//        {
//            if (_bindingList == null || _bindingList.Count == 0) return;
//            List<Driver> items = _bindingList.ToList();
//            List<Driver> sortedList;

//            try
//            {
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

//                // *** SỬA LỖI: Chỉ định rõ System.Windows.Forms.SortOrder ***
//                if (_sortOrder == System.Windows.Forms.SortOrder.Ascending)
//                {
//                    sortedList = items.OrderBy(x => GetNestedPropertyValue(x, nestedPropertyName) == null)
//                                      .ThenBy(x => GetNestedPropertyValue(x, nestedPropertyName))
//                                      .ToList();
//                }
//                else
//                {
//                    sortedList = items.OrderBy(x => GetNestedPropertyValue(x, nestedPropertyName) == null)
//                                      .ThenByDescending(x => GetNestedPropertyValue(x, nestedPropertyName))
//                                      .ToList();
//                }
//                _bindingList = new BindingList<Driver>(sortedList);
//                dgvDrivers.DataSource = _bindingList;
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp lồng nhau: {ex.Message}"); ResetSortGlyphs(); }
//        }

//        private void UpdateSortGlyphs()
//        {
//            foreach (DataGridViewColumn col in dgvDrivers.Columns)
//            {
//                // *** SỬA LỖI: Chỉ định rõ System.Windows.Forms.SortOrder ***
//                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
//                    col.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
//                if (_sortedColumn != null && col == _sortedColumn)
//                    col.HeaderCell.SortGlyphDirection = _sortOrder;
//            }
//        }

//        private void ResetSortGlyphs()
//        {
//            // *** SỬA LỖI: Chỉ định rõ System.Windows.Forms.SortOrder ***
//            if (_sortedColumn != null && _sortedColumn.HeaderCell != null)
//                _sortedColumn.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
//            _sortedColumn = null;
//            _sortOrder = System.Windows.Forms.SortOrder.None;
//            UpdateSortGlyphs();
//        }
//        #endregion
//    }
//}

//=====================

// LMS.GUI/DriverAdmin/ucDriver_Admin.cs
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

namespace LMS.GUI.DriverAdmin
{
    public partial class ucDriver_Admin : UserControl
    {
        private readonly DriverService _driverSvc = new DriverService();
        private BindingList<Driver> _bindingList;

        private DataGridViewColumn _sortedColumn = null;
        private System.Windows.Forms.SortOrder _sortOrder = System.Windows.Forms.SortOrder.None;

        public ucDriver_Admin()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureGrid();
            WireEvents();
            LoadData();

            // Đăng ký lắng nghe sự kiện reload
            AppSession.VehicleAssignmentChanged += OnVehicleAssignmentChanged;
            // Khi control bị phá hủy, tự hủy đăng ký (tránh memory leak) — không cần override Dispose
            this.HandleDestroyed += (s, _) => { try { AppSession.VehicleAssignmentChanged -= OnVehicleAssignmentChanged; } catch { } };
        }

        private void OnVehicleAssignmentChanged()
        {
            if (!IsDisposed) LoadData();
        }

        #region Grid Config & Formatting
        private void ConfigureGrid()
        {
            dgvDrivers.Columns.Clear();
            dgvDrivers.ApplyBaseStyle();

            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "FullName", DataPropertyName = "FullName", HeaderText = "Họ Tên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 30, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", DataPropertyName = "Phone", HeaderText = "Số điện thoại", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "CitizenId", DataPropertyName = "CitizenId", HeaderText = "CCCD", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "LicenseType", DataPropertyName = "LicenseType", HeaderText = "Hạng Bằng Lái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });

            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "VehicleInfo",
                DataPropertyName = "Vehicle",
                HeaderText = "Phương tiện",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            dgvDrivers.CellFormatting += DgvDrivers_CellFormatting;
            dgvDrivers.ColumnHeaderMouseClick += dgvDrivers_ColumnHeaderMouseClick;
            dgvDrivers.SelectionChanged += (s, e) => UpdateButtonsState();
            dgvDrivers.CellDoubleClick += (s, ev) => { if (ev.RowIndex >= 0) OpenDetailPopup(GetSelectedDriverId()); };
        }

        private void DgvDrivers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvDrivers.Columns[e.ColumnIndex].Name == "VehicleInfo")
            {
                if (e.Value is Vehicle vehicle)
                {
                    e.Value = $"{vehicle.PlateNo} ({vehicle.Type})";
                    e.FormattingApplied = true;
                }
                else
                {
                    e.Value = "(Chưa gán xe)";
                    e.FormattingApplied = true;
                }
            }
        }
        #endregion

        #region Data Loading & Helpers
        private void LoadData()
        {
            try
            {
                var drivers = _driverSvc.GetDriversForAdmin(); // Include(d => d.Vehicle)
                _bindingList = new BindingList<Driver>(drivers);
                dgvDrivers.DataSource = _bindingList;

                if (dgvDrivers.Rows.Count > 0)
                {
                    dgvDrivers.ClearSelection();
                    dgvDrivers.Rows[0].Selected = true;
                }

                ResetSortGlyphs();
                UpdateButtonsState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu tài xế: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvDrivers.DataSource = null;
            }
        }

        private int? GetSelectedDriverId() => (dgvDrivers.CurrentRow?.DataBoundItem as Driver)?.Id;
        private Driver GetSelectedDriver() => dgvDrivers.CurrentRow?.DataBoundItem as Driver;

        private void UpdateButtonsState()
        {
            var d = GetSelectedDriver();
            bool has = d != null;

            btnEdit.Enabled = has;
            btnDelete.Enabled = has;
            btnViewDetail.Enabled = has;

            if (this.Controls.Find("btnAssignVehicle", true).FirstOrDefault() is Button btnAssignVehicle)
                btnAssignVehicle.Enabled = has && d.VehicleId == null;

            if (this.Controls.Find("btnUnassignVehicle", true).FirstOrDefault() is Button btnUnassignVehicle)
                btnUnassignVehicle.Enabled = has && d.VehicleId != null;
        }

        private void SelectRowById(int driverId)
        {
            if (dgvDrivers.Rows.Count == 0 || _bindingList == null) return;
            var item = _bindingList.FirstOrDefault(d => d.Id == driverId);
            if (item == null) return;

            int rowIndex = _bindingList.IndexOf(item);
            if (rowIndex < 0 || rowIndex >= dgvDrivers.Rows.Count) return;

            dgvDrivers.ClearSelection();
            dgvDrivers.Rows[rowIndex].Selected = true;
            if (dgvDrivers.Columns.Contains("FullName"))
                dgvDrivers.CurrentCell = dgvDrivers.Rows[rowIndex].Cells["FullName"];

            if (!dgvDrivers.Rows[rowIndex].Displayed)
            {
                int firstDisplayed = Math.Max(0, rowIndex - dgvDrivers.DisplayedRowCount(false) / 2);
                dgvDrivers.FirstDisplayedScrollingRowIndex = Math.Min(firstDisplayed, dgvDrivers.RowCount - 1);
            }

            UpdateButtonsState();
        }
        #endregion

        #region Event Wiring
        private void WireEvents()
        {
            btnReload.Click += (s, e) => LoadData();
            btnAdd.Click += (s, e) => OpenEditorPopup(null);
            btnEdit.Click += (s, e) => OpenEditorPopup(GetSelectedDriverId());
            btnDelete.Click += (s, e) => DeleteDriver();
            btnViewDetail.Click += (s, e) => OpenDetailPopup(GetSelectedDriverId());
            btnSearch.Click += (s, e) => OpenSearchPopup();
        }
        #endregion

        #region Actions (Popup Opening & Deleting)
        private void OpenEditorPopup(int? driverId)
        {
            if (driverId == null && !btnAdd.Enabled) return;
            if (driverId.HasValue && !btnEdit.Enabled) return;

            var mode = driverId.HasValue ? ucDriverEditor_Admin.EditorMode.Edit : ucDriverEditor_Admin.EditorMode.Add;
            var title = (mode == ucDriverEditor_Admin.EditorMode.Add) ? "Thêm Tài Xế" : "Sửa Tài Xế";

            try
            {
                var ucEditor = new ucDriverEditor_Admin();
                ucEditor.LoadData(mode, driverId);

                using (var popupForm = new Form
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.None,
                    Width = 657,
                    Height = 689
                })
                {
                    popupForm.Controls.Add(ucEditor);
                    ucEditor.Dock = DockStyle.Fill;

                    var ownerForm = this.FindForm();
                    if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }

                    var result = popupForm.ShowDialog(ownerForm);
                    if (result == DialogResult.OK) LoadData();
                }
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi mở form {title}: {ex.Message}", "Lỗi"); }
        }

        private void OpenDetailPopup(int? driverId)
        {
            if (!driverId.HasValue) { MessageBox.Show("Vui lòng chọn tài xế.", "Thông báo"); return; }
            try
            {
                var ucDetail = new ucDriverDetail_Admin(driverId.Value);
                using (var popupForm = new Form
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.None,
                    Width = 1199,
                    Height = 689
                })
                {
                    ucDetail.Dock = DockStyle.Fill;
                    popupForm.Controls.Add(ucDetail);

                    var ownerForm = this.FindForm();
                    if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }

                    popupForm.ShowDialog(ownerForm);
                }
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi mở chi tiết: {ex.Message}", "Lỗi"); }
        }

        private void DeleteDriver()
        {
            var driverId = GetSelectedDriverId();
            if (!driverId.HasValue) { MessageBox.Show("Vui lòng chọn tài xế.", "Thông báo"); return; }
            try
            {
                if (_driverSvc.CheckDriverHasShipments(driverId.Value))
                {
                    MessageBox.Show("Không thể xóa vì tài xế đã có lịch sử chuyến hàng.\nHãy xem xét khóa tài khoản.", "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var confirm = MessageBox.Show("Tài xế này chưa có chuyến hàng.\nXóa vĩnh viễn tài xế và tài khoản liên quan?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    _driverSvc.DeleteNewDriver(driverId.Value);
                    MessageBox.Show("Đã xóa tài xế.", "Hoàn tất");
                    LoadData();
                }
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi"); }
        }

        private void OpenSearchPopup()
        {
            int? selectedId = null;
            using (var searchForm = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new System.Drawing.Size(1199, 689)
            })
            {
                var ucSearch = new ucDriverSearch_Admin { Dock = DockStyle.Fill };
                ucSearch.DriverPicked += (sender, id) =>
                {
                    selectedId = id;
                    searchForm.DialogResult = DialogResult.OK;
                    searchForm.Close();
                };
                searchForm.Controls.Add(ucSearch);

                var ownerForm = this.FindForm();
                if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }

                if (searchForm.ShowDialog(ownerForm) == DialogResult.OK && selectedId.HasValue)
                    SelectRowById(selectedId.Value);
            }
        }
        #endregion

        #region Sorting Logic
        private void dgvDrivers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;
            var newColumn = dgvDrivers.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn)
                _sortOrder = (_sortOrder == System.Windows.Forms.SortOrder.Ascending) ? System.Windows.Forms.SortOrder.Descending : System.Windows.Forms.SortOrder.Ascending;
            else
            {
                if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                _sortOrder = System.Windows.Forms.SortOrder.Ascending;
                _sortedColumn = newColumn;
            }

            if (newColumn.Name == "VehicleInfo") ApplySortNestedProperty("Vehicle.PlateNo");
            else ApplySort();

            UpdateSortGlyphs();
        }

        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;
            string propertyName = _sortedColumn.DataPropertyName;
            PropertyInfo propInfo = typeof(Driver).GetProperty(propertyName);
            if (propInfo == null) return;

            var items = _bindingList.ToList();
            var sorted = (_sortOrder == System.Windows.Forms.SortOrder.Ascending)
                ? items.OrderBy(x => propInfo.GetValue(x, null)).ToList()
                : items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

            _bindingList = new BindingList<Driver>(sorted);
            dgvDrivers.DataSource = _bindingList;
        }

        private void ApplySortNestedProperty(string nestedPropertyName)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;

            Func<object, string, object> GetNested = (obj, name) =>
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
            var sorted = (_sortOrder == System.Windows.Forms.SortOrder.Ascending)
                ? items.OrderBy(x => GetNested(x, nestedPropertyName) == null)
                      .ThenBy(x => GetNested(x, nestedPropertyName))
                      .ToList()
                : items.OrderBy(x => GetNested(x, nestedPropertyName) == null)
                      .ThenByDescending(x => GetNested(x, nestedPropertyName))
                      .ToList();

            _bindingList = new BindingList<Driver>(sorted);
            dgvDrivers.DataSource = _bindingList;
        }

        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvDrivers.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
                    col.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                if (_sortedColumn != null && col == _sortedColumn)
                    col.HeaderCell.SortGlyphDirection = _sortOrder;
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null && _sortedColumn.HeaderCell != null)
                _sortedColumn.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
            _sortedColumn = null;
            _sortOrder = System.Windows.Forms.SortOrder.None;
            UpdateSortGlyphs();
        }
        #endregion
    }
}
