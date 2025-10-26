//// LMS.GUI/DriverAdmin/ucDriver_Admin.cs
//using Guna.UI2.WinForms;
//using LMS.BUS.Dtos; // Cần cho DTOs
//using LMS.BUS.Helpers;
//using LMS.BUS.Services; // Cần cho DriverService
//using LMS.DAL.Models; // Cần cho Driver
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;

//namespace LMS.GUI.DriverAdmin // Đổi namespace
//{
//    public partial class ucDriver_Admin : UserControl
//    {
//        private readonly DriverService _driverSvc = new DriverService();
//        private BindingList<Driver> _bindingList;
//        private DataGridViewColumn _sortedColumn = null;
//        private SortOrder _sortOrder = SortOrder.None;

//        public ucDriver_Admin()
//        {
//            InitializeComponent();
//            // Đảm bảo bạn đã kéo thả các nút (btnAdd, btnEdit...) vào trong Designer
//        }

//        protected override void OnLoad(EventArgs e)
//        {
//            base.OnLoad(e);
//            ConfigureGrid();
//            WireEvents();
//            LoadData();
//        }

//        #region Grid Config
//        private void ConfigureGrid()
//        {
//            dgvDrivers.Columns.Clear(); // Giả sử tên là dgvDrivers
//            dgvDrivers.ApplyBaseStyle();

//            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "Id",
//                DataPropertyName = "Id",
//                Visible = false
//            });
//            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "FullName",
//                DataPropertyName = "FullName",
//                HeaderText = "Họ Tên",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
//                SortMode = DataGridViewColumnSortMode.Programmatic
//            });
//            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "Phone",
//                DataPropertyName = "Phone",
//                HeaderText = "Số điện thoại",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
//                SortMode = DataGridViewColumnSortMode.Programmatic
//            });
//            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "CitizenId",
//                DataPropertyName = "CitizenId",
//                HeaderText = "CCCD",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
//                SortMode = DataGridViewColumnSortMode.Programmatic
//            });
//            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "LicenseType",
//                DataPropertyName = "LicenseType",
//                HeaderText = "Hạng Bằng Lái",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
//                SortMode = DataGridViewColumnSortMode.Programmatic
//            });
//            // Cột IsActive (bạn có thể thêm nếu muốn, giống ucCustomer_Admin)
//            //dgvDrivers.Columns.Add(new DataGridViewCheckBoxColumn {
//            //    Name = "IsActive", DataPropertyName = "IsActive", HeaderText = "Hoạt động"
//            //});
//        }
//        #endregion

//        #region Load + Helpers
//        private void LoadData()
//        {
//            try
//            {
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
//            if (dgvDrivers.CurrentRow != null && dgvDrivers.CurrentRow.DataBoundItem is Driver driver)
//            {
//                return driver.Id;
//            }
//            return null;
//        }

//        private void UpdateButtonsState()
//        {
//            bool hasSelection = (GetSelectedDriverId() != null);
//            btnEdit.Enabled = hasSelection;
//            btnDelete.Enabled = hasSelection;
//            btnViewDetail.Enabled = hasSelection;
//        }
//        #endregion

//        #region Wire Events
//        private void WireEvents()
//        {
//            btnReload.Click += (s, e) => LoadData();
//            btnAdd.Click += (s, e) => OpenEditorPopup(null);
//            btnEdit.Click += (s, e) => OpenEditorPopup(GetSelectedDriverId());
//            btnDelete.Click += (s, e) => DeleteDriver();
//            btnViewDetail.Click += (s, e) => OpenDetailPopup(GetSelectedDriverId());

//            dgvDrivers.CellDoubleClick += (s, e) =>
//            {
//                if (e.RowIndex >= 0) // Đảm bảo không click header
//                    OpenDetailPopup(GetSelectedDriverId());
//            };

//            dgvDrivers.ColumnHeaderMouseClick += dgvDrivers_ColumnHeaderMouseClick;
//            dgvDrivers.SelectionChanged += (s, e) => UpdateButtonsState();
//        }
//        #endregion

//        #region Actions (Popup Opening & Deleting)

//        private void OpenEditorPopup(int? driverId)
//        {
//            if (driverId == null && !btnAdd.Enabled) return;
//            if (driverId.HasValue && !btnEdit.Enabled) return;

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
//                    // Đặt kích thước phù hợp với ucDriverEditor_Admin
//                    Width = 657, // Giống form Customer
//                    Height = 689 // Giống form Customer
//                })
//                {
//                    popupForm.Controls.Add(ucEditor);

//                    Form ownerForm = this.FindForm();
//                    if (ownerForm == null)
//                    {
//                        MessageBox.Show("Không thể xác định Form cha.", "Lỗi Hiển Thị");
//                        return;
//                    }
//                    var result = popupForm.ShowDialog(ownerForm);

//                    if (result == DialogResult.OK)
//                    {
//                        LoadData(); // Tải lại grid nếu lưu thành công
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi mở form {title}: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void OpenDetailPopup(int? driverId)
//        {
//            if (!driverId.HasValue)
//            {
//                MessageBox.Show("Vui lòng chọn một tài xế.", "Thông báo");
//                return;
//            }

//            try
//            {
//                var ucDetail = new ucDriverDetail_Admin(driverId.Value);

//                using (var popupForm = new Form
//                {
//                    StartPosition = FormStartPosition.CenterScreen,
//                    FormBorderStyle = FormBorderStyle.None,
//                    // Đặt kích thước phù hợp với ucDriverDetail_Admin
//                    Width = 1199, // Giống form Customer
//                    Height = 689  // Giống form Customer
//                })
//                {
//                    ucDetail.Dock = DockStyle.Fill;
//                    popupForm.Controls.Add(ucDetail);

//                    Form ownerForm = this.FindForm();
//                    if (ownerForm == null)
//                    {
//                        MessageBox.Show("Không thể xác định Form cha.", "Lỗi Hiển Thị");
//                        return;
//                    }
//                    popupForm.ShowDialog(ownerForm);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi mở chi tiết tài xế: {ex.Message}", "Lỗi");
//            }
//        }

//        private void DeleteDriver()
//        {
//            var driverId = GetSelectedDriverId();
//            if (!driverId.HasValue)
//            {
//                MessageBox.Show("Vui lòng chọn tài xế để xóa.", "Thông báo");
//                return;
//            }

//            try
//            {
//                // Logic xóa tương tự ucCustomer_Admin
//                if (_driverSvc.CheckDriverHasShipments(driverId.Value))
//                {
//                    MessageBox.Show("Không thể xóa tài xế này vì họ đã có chuyến hàng trong lịch sử.\n" +
//                                    "Vui lòng xem xét việc 'Khóa' tài khoản (trong Quản lý Tài khoản) thay vì xóa.",
//                                    "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                    return;
//                }

//                var confirm = MessageBox.Show("Tài xế này chưa có chuyến hàng.\n" +
//                                            "Bạn có chắc muốn xóa vĩnh viễn tài xế này và tất cả tài khoản liên quan?",
//                                            "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

//                if (confirm == DialogResult.Yes)
//                {
//                    _driverSvc.DeleteNewDriver(driverId.Value);
//                    MessageBox.Show("Đã xóa tài xế thành công.", "Hoàn tất");
//                    LoadData();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi xóa tài xế: {ex.Message}", "Lỗi");
//            }
//        }
//        #endregion

//        #region Sorting Logic (Giống hệt ucCustomer_Admin)
//        private void dgvDrivers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            if (_bindingList == null || _bindingList.Count == 0) return;

//            var newColumn = dgvDrivers.Columns[e.ColumnIndex];
//            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

//            if (_sortedColumn == newColumn)
//            {
//                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
//            }
//            else
//            {
//                if (_sortedColumn != null)
//                {
//                    _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//                }
//                _sortOrder = SortOrder.Ascending;
//                _sortedColumn = newColumn;
//            }

//            ApplySort();
//            if (_sortedColumn != null)
//                _sortedColumn.HeaderCell.SortGlyphDirection = _sortOrder;
//        }

//        private void ApplySort()
//        {
//            if (_sortedColumn == null || _bindingList == null) return;

//            string propertyName = _sortedColumn.DataPropertyName;
//            var propInfo = typeof(Driver).GetProperty(propertyName); // Đổi thành typeof(Driver)
//            if (propInfo == null) return;

//            List<Driver> items = _bindingList.ToList();
//            List<Driver> sortedList;

//            if (_sortOrder == SortOrder.Ascending)
//            {
//                sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList();
//            }
//            else
//            {
//                sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();
//            }

//            _bindingList = new BindingList<Driver>(sortedList); // Đổi thành BindingList<Driver>
//            dgvDrivers.DataSource = _bindingList;
//        }

//        private void ResetSortGlyphs()
//        {
//            if (_sortedColumn != null)
//            {
//                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//            }
//            _sortedColumn = null;
//            _sortOrder = SortOrder.None;
//            foreach (DataGridViewColumn col in dgvDrivers.Columns)
//            {
//                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
//                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
//            }
//        }
//        #endregion
//    }
//}

// LMS.GUI/DriverAdmin/ucDriver_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private SortOrder _sortOrder = SortOrder.None;

        public ucDriver_Admin()
        {
            InitializeComponent();
            // Đảm bảo bạn đã kéo thả các nút (btnAdd, btnEdit, btnDelete, btnViewDetail, btnReload, btnSearch) vào trong Designer
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureGrid();
            WireEvents();
            LoadData();
        }

        #region Grid Config
        private void ConfigureGrid()
        {
            dgvDrivers.Columns.Clear();
            dgvDrivers.ApplyBaseStyle();

            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "FullName", DataPropertyName = "FullName", HeaderText = "Họ Tên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", DataPropertyName = "Phone", HeaderText = "Số điện thoại", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "CitizenId", DataPropertyName = "CitizenId", HeaderText = "CCCD", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn { Name = "LicenseType", DataPropertyName = "LicenseType", HeaderText = "Hạng Bằng Lái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
        }
        #endregion

        #region Load + Helpers
        private void LoadData()
        {
            try
            {
                var drivers = _driverSvc.GetDriversForAdmin();
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

        private int? GetSelectedDriverId()
        {
            if (dgvDrivers.CurrentRow != null && dgvDrivers.CurrentRow.DataBoundItem is Driver driver)
            {
                return driver.Id;
            }
            return null;
        }

        private void UpdateButtonsState()
        {
            bool hasSelection = (GetSelectedDriverId() != null);
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnViewDetail.Enabled = hasSelection;
            // Nút Thêm, Tải lại, Tìm kiếm thường luôn bật
            // btnAdd.Enabled = true;
            // btnReload.Enabled = true;
            // btnSearch.Enabled = true; 
        }

        // === THÊM HÀM NÀY: Để chọn dòng trên grid theo ID ===
        private void SelectRowById(int driverId)
        {
            if (dgvDrivers.Rows.Count == 0 || _bindingList == null) return;

            // Tìm index của Driver trong BindingList (nhanh hơn duyệt grid)
            Driver driverToSelect = _bindingList.FirstOrDefault(d => d.Id == driverId);
            if (driverToSelect != null)
            {
                int rowIndex = _bindingList.IndexOf(driverToSelect);
                if (rowIndex >= 0 && rowIndex < dgvDrivers.Rows.Count)
                {
                    dgvDrivers.ClearSelection();
                    dgvDrivers.Rows[rowIndex].Selected = true;
                    // Đặt làm ô hiện tại để focus
                    if (dgvDrivers.Columns.Contains("FullName")) // Chọn cột FullName
                        dgvDrivers.CurrentCell = dgvDrivers.Rows[rowIndex].Cells["FullName"];

                    // Cuộn đến dòng nếu nó bị che
                    if (!dgvDrivers.Rows[rowIndex].Displayed)
                    {
                        int firstDisplayed = Math.Max(0, rowIndex - dgvDrivers.DisplayedRowCount(false) / 2);
                        dgvDrivers.FirstDisplayedScrollingRowIndex = Math.Min(firstDisplayed, dgvDrivers.RowCount - 1);
                    }
                }
            }
            UpdateButtonsState(); // Cập nhật lại trạng thái nút sau khi chọn
        }
        // === KẾT THÚC THÊM HÀM ===

        #endregion

        #region Wire Events
        private void WireEvents()
        {
            btnReload.Click += (s, e) => LoadData();
            btnAdd.Click += (s, e) => OpenEditorPopup(null);
            btnEdit.Click += (s, e) => OpenEditorPopup(GetSelectedDriverId());
            btnDelete.Click += (s, e) => DeleteDriver();
            btnViewDetail.Click += (s, e) => OpenDetailPopup(GetSelectedDriverId());

            // === THÊM SỰ KIỆN CHO NÚT TÌM KIẾM ===
            btnSearch.Click += (s, e) => OpenSearchPopup();
            // === KẾT THÚC THÊM ===

            dgvDrivers.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    OpenDetailPopup(GetSelectedDriverId());
            };

            dgvDrivers.ColumnHeaderMouseClick += dgvDrivers_ColumnHeaderMouseClick;
            dgvDrivers.SelectionChanged += (s, e) => UpdateButtonsState();
        }
        #endregion

        #region Actions (Popup Opening & Deleting)

        // (Hàm OpenEditorPopup, OpenDetailPopup, DeleteDriver giữ nguyên)
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
                    Form ownerForm = this.FindForm();
                    if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }
                    var result = popupForm.ShowDialog(ownerForm);
                    if (result == DialogResult.OK) { LoadData(); }
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
                    Form ownerForm = this.FindForm();
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

        // === THÊM HÀM MỞ POPUP TÌM KIẾM ===
        private void OpenSearchPopup()
        {
            int? selectedId = null; // Biến lưu ID được chọn từ popup
            using (var searchForm = new Form
            {
                //Text = "Tìm kiếm Tài xế",
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None, // Hoặc FixedDialog
                // Đặt kích thước phù hợp với ucDriverSearch_Admin
                Size = new Size(1199, 689) // Ví dụ: kích thước giống form Detail
            })
            {
                var ucSearch = new ucDriverSearch_Admin { Dock = DockStyle.Fill };

                // Đăng ký sự kiện: Khi người dùng chọn tài xế trong ucSearch
                ucSearch.DriverPicked += (sender, id) =>
                {
                    selectedId = id; // Lưu lại ID
                    searchForm.DialogResult = DialogResult.OK; // Báo thành công
                    searchForm.Close(); // Đóng popup
                };

                searchForm.Controls.Add(ucSearch); // Thêm UC vào form

                // Hiển thị popup và chờ
                Form ownerForm = this.FindForm();
                if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }

                if (searchForm.ShowDialog(ownerForm) == DialogResult.OK && selectedId.HasValue)
                {
                    // Nếu người dùng chọn OK và có ID -> chọn dòng tương ứng trên grid chính
                    SelectRowById(selectedId.Value);
                }
                // Nếu người dùng đóng popup (Cancel/nút X) -> không làm gì cả
            }
        }
        // === KẾT THÚC THÊM HÀM ===

        #endregion

        #region Sorting Logic (Giữ nguyên)
        private void dgvDrivers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;
            var newColumn = dgvDrivers.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn) { _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
            else { if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _sortOrder = SortOrder.Ascending; _sortedColumn = newColumn; }

            ApplySort();
            if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = _sortOrder;
        }
        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null) return;
            string propertyName = _sortedColumn.DataPropertyName;
            var propInfo = typeof(Driver).GetProperty(propertyName);
            if (propInfo == null) return;

            List<Driver> items = _bindingList.ToList();
            List<Driver> sortedList;
            if (_sortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
            else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }

            _bindingList = new BindingList<Driver>(sortedList);
            dgvDrivers.DataSource = _bindingList;
        }
        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null) { _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; }
            _sortedColumn = null; _sortOrder = SortOrder.None;
            foreach (DataGridViewColumn col in dgvDrivers.Columns) { if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None; }
        }
        #endregion
    }
}