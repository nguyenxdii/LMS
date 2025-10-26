//using Guna.UI2.WinForms;
//using LMS.BUS.Dtos; // Make sure DTOs are defined
//using LMS.BUS.Helpers;
//using LMS.BUS.Services;
//using LMS.DAL.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;

//namespace LMS.GUI.CustomerAdmin
//{
//    public partial class ucCustomer_Admin : UserControl
//    {
//        private readonly CustomerService _customerSvc = new CustomerService();
//        private BindingList<Customer> _bindingList;
//        private DataGridViewColumn _sortedColumn = null;
//        private SortOrder _sortOrder = SortOrder.None;

//        public ucCustomer_Admin()
//        {
//            InitializeComponent();
//            // Ensure controls (btnAdd, btnEdit, etc.) exist in your Designer.cs
//        }

//        // Use OnLoad or the Form/UC Load event handler
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
//            dgvCustomers.Columns.Clear();
//            dgvCustomers.ApplyBaseStyle(); // Use your helper

//            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "Id",
//                DataPropertyName = "Id",
//                Visible = false
//            });
//            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "Name",
//                DataPropertyName = "Name",
//                HeaderText = "Tên Khách Hàng",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
//                SortMode = DataGridViewColumnSortMode.Programmatic
//            });
//            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "Phone",
//                DataPropertyName = "Phone",
//                HeaderText = "Số điện thoại",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
//                SortMode = DataGridViewColumnSortMode.Programmatic
//            });
//            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "Email",
//                DataPropertyName = "Email",
//                HeaderText = "Email",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
//                SortMode = DataGridViewColumnSortMode.Programmatic
//            });
//            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "Address",
//                DataPropertyName = "Address",
//                HeaderText = "Địa chỉ",
//                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
//                FillWeight = 150, // Give Address more weight
//                SortMode = DataGridViewColumnSortMode.Programmatic
//            });
//            // Add IsActive column if needed in the future
//            //dgvCustomers.Columns.Add(new DataGridViewCheckBoxColumn {
//            //    Name = "IsActive", DataPropertyName = "IsActive", HeaderText = "Hoạt động"
//            //});
//        }
//        #endregion

//        #region Load + Helpers
//        private void LoadData()
//        {
//            try
//            {
//                var customers = _customerSvc.GetCustomersForAdmin();
//                _bindingList = new BindingList<Customer>(customers);
//                dgvCustomers.DataSource = _bindingList;

//                // Select the first row if data exists
//                if (dgvCustomers.Rows.Count > 0)
//                {
//                    dgvCustomers.ClearSelection();
//                    dgvCustomers.Rows[0].Selected = true;
//                    // Optional: Set current cell for focus
//                    // dgvCustomers.CurrentCell = dgvCustomers.Rows[0].Cells["Name"];
//                }

//                ResetSortGlyphs();
//                UpdateButtonsState();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tải dữ liệu khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                dgvCustomers.DataSource = null; // Clear grid on error
//            }
//        }

//        private int? GetSelectedCustomerId()
//        {
//            if (dgvCustomers.CurrentRow != null && dgvCustomers.CurrentRow.DataBoundItem is Customer customer)
//            {
//                return customer.Id;
//            }
//            return null;
//        }

//        private void UpdateButtonsState()
//        {
//            bool hasSelection = (GetSelectedCustomerId() != null);
//            btnEdit.Enabled = hasSelection;
//            btnDelete.Enabled = hasSelection;
//            btnViewDetail.Enabled = hasSelection;
//            // btnAdd and btnReload should usually always be enabled
//            // btnAdd.Enabled = true;
//            // btnReload.Enabled = true;
//        }
//        #endregion

//        #region Wire Events
//        private void WireEvents()
//        {
//            btnReload.Click += (s, e) => LoadData();
//            btnAdd.Click += (s, e) => OpenEditorPopup(null);
//            btnEdit.Click += (s, e) => OpenEditorPopup(GetSelectedCustomerId());
//            btnDelete.Click += (s, e) => DeleteCustomer();
//            btnViewDetail.Click += (s, e) => OpenDetailPopup(GetSelectedCustomerId());

//            dgvCustomers.CellDoubleClick += (s, e) =>
//            {
//                if (e.RowIndex >= 0) // Ensure not clicking header
//                    OpenDetailPopup(GetSelectedCustomerId());
//            };

//            dgvCustomers.ColumnHeaderMouseClick += dgvCustomers_ColumnHeaderMouseClick;
//            dgvCustomers.SelectionChanged += (s, e) => UpdateButtonsState();
//        }
//        #endregion

//        #region Actions (Popup Opening & Deleting)

//        private void OpenEditorPopup(int? customerId)
//        {
//            // Prevent opening if the corresponding button is disabled
//            if (customerId == null && !btnAdd.Enabled) return;
//            if (customerId.HasValue && !btnEdit.Enabled) return;

//            var mode = customerId.HasValue ? ucCustomerEditor_Admin.EditorMode.Edit : ucCustomerEditor_Admin.EditorMode.Add;
//            var title = (mode == ucCustomerEditor_Admin.EditorMode.Add) ? "Thêm Khách Hàng" : "Sửa Khách Hàng";

//            try
//            {
//                var ucEditor = new ucCustomerEditor_Admin();
//                // Load data *inside* the using block, before showing the form
//                // This ensures errors during load are caught before the form appears
//                ucEditor.LoadData(mode, customerId);

//                using (var popupForm = new Form
//                {
//                    //Text = title, // Set title here
//                    StartPosition = FormStartPosition.CenterScreen, // Use CenterScreen
//                    FormBorderStyle = FormBorderStyle.None, // Use FixedDialog for standard popup feel
//                    //MaximizeBox = false,
//                    //MinimizeBox = false,
//                    // Set Size or use AutoSize properties depending on ucEditor's design
//                    Width = 657, // Adjust as needed
//                    Height = 689 // Adjust as needed
//                })
//                {
//                    //ucEditor.Dock = DockStyle.Fill;
//                    popupForm.Controls.Add(ucEditor);

//                    //var result = popupForm.ShowDialog(); // Use ShowDialog to wait for closure
//                    //var result = popupForm.ShowDialog(this.FindForm());
//                    Form ownerForm = this.FindForm();
//                    if (ownerForm == null)
//                    {
//                        MessageBox.Show("Không thể xác định Form cha để hiển thị popup.", "Lỗi Hiển Thị", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        return; // Không hiển thị popup nếu không tìm thấy cha
//                    }
//                    var result = popupForm.ShowDialog(ownerForm); // Truyền ownerForm đã kiểm tra

//                    if (result == DialogResult.OK)
//                    {
//                        LoadData(); // Reload grid only if saved successfully
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi mở form {title}: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void OpenDetailPopup(int? customerId)
//        {
//            if (!customerId.HasValue)
//            {
//                // Optionally check btnViewDetail.Enabled as well
//                MessageBox.Show("Vui lòng chọn một khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                return;
//            }

//            try
//            {
//                // Create UC first to catch initialization errors
//                var ucDetail = new ucCustomerDetail_Admin(customerId.Value);

//                using (var popupForm = new Form
//                {
//                    //Text = "Chi Tiết Khách Hàng",
//                    StartPosition = FormStartPosition.CenterScreen, // Use CenterScreen
//                    FormBorderStyle = FormBorderStyle.None, // Use FixedDialog
//                    //MaximizeBox = false,
//                    //MinimizeBox = false,
//                    // Adjust size as needed for ucCustomerDetail_Admin
//                    Width = 1199,
//                    Height = 689
//                })
//                {
//                    ucDetail.Dock = DockStyle.Fill;
//                    popupForm.Controls.Add(ucDetail);
//                    //popupForm.ShowDialog(); // Use ShowDialog

//                    Form ownerForm = this.FindForm();
//                    if (ownerForm == null)
//                    {
//                        MessageBox.Show("Không thể xác định Form cha để hiển thị popup.", "Lỗi Hiển Thị", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        return;
//                    }
//                    popupForm.ShowDialog(ownerForm);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi mở chi tiết khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void DeleteCustomer()
//        {
//            var customerId = GetSelectedCustomerId();
//            if (!customerId.HasValue)
//            {
//                MessageBox.Show("Vui lòng chọn khách hàng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                return;
//            }

//            try
//            {
//                if (_customerSvc.CheckCustomerHasOrders(customerId.Value))
//                {
//                    MessageBox.Show("Không thể xóa khách hàng này vì họ đã có đơn hàng trong lịch sử.\n" +
//                                    "Vui lòng xem xét việc 'Khóa' tài khoản thay vì xóa.",
//                                    "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                    return;
//                }

//                var confirm = MessageBox.Show("Khách hàng này chưa có đơn hàng.\n" +
//                                            "Bạn có chắc muốn xóa vĩnh viễn khách hàng này và tất cả tài khoản liên quan?",
//                                            "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

//                if (confirm == DialogResult.Yes)
//                {
//                    _customerSvc.DeleteNewCustomer(customerId.Value);
//                    MessageBox.Show("Đã xóa khách hàng thành công.", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    LoadData();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi xóa khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }
//        #endregion

//        #region Sorting Logic
//        private void dgvCustomers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            if (_bindingList == null || _bindingList.Count == 0) return;

//            var newColumn = dgvCustomers.Columns[e.ColumnIndex];
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
//            if (_sortedColumn != null) // Check again after ApplySort
//                _sortedColumn.HeaderCell.SortGlyphDirection = _sortOrder;
//        }

//        private void ApplySort()
//        {
//            if (_sortedColumn == null || _bindingList == null) return;

//            string propertyName = _sortedColumn.DataPropertyName;
//            // Use reflection helper for potentially nullable properties if needed later
//            var propInfo = typeof(Customer).GetProperty(propertyName);
//            if (propInfo == null) return;

//            // Sort directly on the BindingList source if possible, or create new list
//            List<Customer> items = _bindingList.ToList(); // Work with a copy
//            List<Customer> sortedList;

//            if (_sortOrder == SortOrder.Ascending)
//            {
//                sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList();
//            }
//            else
//            {
//                sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();
//            }

//            // Recreate BindingList and rebind DataSource for DataGridView update
//            _bindingList = new BindingList<Customer>(sortedList);
//            dgvCustomers.DataSource = _bindingList;

//            // Restore selection if needed after sorting (optional)
//            // if (dgvCustomers.Rows.Count > 0) dgvCustomers.Rows[0].Selected = true;
//        }

//        private void ResetSortGlyphs()
//        {
//            if (_sortedColumn != null)
//            {
//                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//            }
//            _sortedColumn = null;
//            _sortOrder = SortOrder.None;
//            // Ensure all columns are reset visually
//            foreach (DataGridViewColumn col in dgvCustomers.Columns)
//            {
//                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
//                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
//            }
//        }
//        #endregion
//    }
//}


// LMS.GUI/CustomerAdmin/ucCustomer_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos; // Make sure DTOs are defined
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

namespace LMS.GUI.CustomerAdmin
{
    public partial class ucCustomer_Admin : UserControl
    {
        private readonly CustomerService _customerSvc = new CustomerService();
        private BindingList<Customer> _bindingList;
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        public ucCustomer_Admin()
        {
            InitializeComponent();
            // Ensure controls (btnAdd, btnEdit, btnDelete, btnViewDetail, btnReload, btnSearch) exist in your Designer.cs
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
            dgvCustomers.Columns.Clear();
            dgvCustomers.ApplyBaseStyle(); // Use your helper

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", DataPropertyName = "Name", HeaderText = "Tên Khách Hàng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", DataPropertyName = "Phone", HeaderText = "Số điện thoại", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", DataPropertyName = "Email", HeaderText = "Email", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Address", DataPropertyName = "Address", HeaderText = "Địa chỉ", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 150, SortMode = DataGridViewColumnSortMode.Programmatic });
        }
        #endregion

        #region Load + Helpers
        private void LoadData()
        {
            try
            {
                var customers = _customerSvc.GetCustomersForAdmin();
                _bindingList = new BindingList<Customer>(customers);
                dgvCustomers.DataSource = _bindingList;

                if (dgvCustomers.Rows.Count > 0)
                {
                    dgvCustomers.ClearSelection();
                    dgvCustomers.Rows[0].Selected = true;
                }

                ResetSortGlyphs();
                UpdateButtonsState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu khách hàng: {ex.Message}", "Lỗi");
                dgvCustomers.DataSource = null;
            }
        }

        private int? GetSelectedCustomerId()
        {
            if (dgvCustomers.CurrentRow != null && dgvCustomers.CurrentRow.DataBoundItem is Customer customer)
            {
                return customer.Id;
            }
            return null;
        }

        private void UpdateButtonsState()
        {
            bool hasSelection = (GetSelectedCustomerId() != null);
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnViewDetail.Enabled = hasSelection;
            // btnSearch.Enabled = true; // Nút tìm kiếm thường luôn bật
        }

        // === THÊM HÀM NÀY: Để chọn dòng trên grid theo ID ===
        private void SelectRowById(int customerId)
        {
            if (dgvCustomers.Rows.Count == 0 || _bindingList == null) return;

            Customer customerToSelect = _bindingList.FirstOrDefault(c => c.Id == customerId);
            if (customerToSelect != null)
            {
                int rowIndex = _bindingList.IndexOf(customerToSelect);
                if (rowIndex >= 0 && rowIndex < dgvCustomers.Rows.Count)
                {
                    dgvCustomers.ClearSelection();
                    dgvCustomers.Rows[rowIndex].Selected = true;
                    if (dgvCustomers.Columns.Contains("Name")) // Chọn cột Name
                        dgvCustomers.CurrentCell = dgvCustomers.Rows[rowIndex].Cells["Name"];

                    if (!dgvCustomers.Rows[rowIndex].Displayed)
                    {
                        int firstDisplayed = Math.Max(0, rowIndex - dgvCustomers.DisplayedRowCount(false) / 2);
                        dgvCustomers.FirstDisplayedScrollingRowIndex = Math.Min(firstDisplayed, dgvCustomers.RowCount - 1);
                    }
                }
            }
            UpdateButtonsState();
        }
        // === KẾT THÚC THÊM HÀM ===

        #endregion

        #region Wire Events
        private void WireEvents()
        {
            btnReload.Click += (s, e) => LoadData();
            btnAdd.Click += (s, e) => OpenEditorPopup(null);
            btnEdit.Click += (s, e) => OpenEditorPopup(GetSelectedCustomerId());
            btnDelete.Click += (s, e) => DeleteCustomer();
            btnViewDetail.Click += (s, e) => OpenDetailPopup(GetSelectedCustomerId());

            // === THÊM SỰ KIỆN CHO NÚT TÌM KIẾM ===
            btnSearch.Click += (s, e) => OpenSearchPopup();
            // === KẾT THÚC THÊM ===

            dgvCustomers.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    OpenDetailPopup(GetSelectedCustomerId());
            };

            dgvCustomers.ColumnHeaderMouseClick += dgvCustomers_ColumnHeaderMouseClick;
            dgvCustomers.SelectionChanged += (s, e) => UpdateButtonsState();
        }
        #endregion

        #region Actions (Popup Opening & Deleting)

        // (Hàm OpenEditorPopup, OpenDetailPopup, DeleteCustomer giữ nguyên)
        private void OpenEditorPopup(int? customerId)
        {
            if (customerId == null && !btnAdd.Enabled) return;
            if (customerId.HasValue && !btnEdit.Enabled) return;
            var mode = customerId.HasValue ? ucCustomerEditor_Admin.EditorMode.Edit : ucCustomerEditor_Admin.EditorMode.Add;
            try
            {
                var ucEditor = new ucCustomerEditor_Admin();
                ucEditor.LoadData(mode, customerId);
                using (var popupForm = new Form { StartPosition = FormStartPosition.CenterScreen, FormBorderStyle = FormBorderStyle.None, Width = 657, Height = 689 })
                {
                    popupForm.Controls.Add(ucEditor);
                    Form ownerForm = this.FindForm();
                    if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }
                    var result = popupForm.ShowDialog(ownerForm);
                    if (result == DialogResult.OK) { LoadData(); }
                }
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi mở form: {ex.Message}", "Lỗi"); }
        }
        private void OpenDetailPopup(int? customerId)
        {
            if (!customerId.HasValue) { MessageBox.Show("Vui lòng chọn khách hàng.", "Thông báo"); return; }
            try
            {
                var ucDetail = new ucCustomerDetail_Admin(customerId.Value);
                using (var popupForm = new Form { StartPosition = FormStartPosition.CenterScreen, FormBorderStyle = FormBorderStyle.None, Width = 1199, Height = 689 })
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
        private void DeleteCustomer()
        {
            var customerId = GetSelectedCustomerId();
            if (!customerId.HasValue) { MessageBox.Show("Vui lòng chọn khách hàng.", "Thông báo"); return; }
            try
            {
                if (_customerSvc.CheckCustomerHasOrders(customerId.Value))
                {
                    MessageBox.Show("Không thể xóa vì khách hàng đã có đơn hàng.\nHãy xem xét khóa tài khoản.", "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var confirm = MessageBox.Show("Khách hàng này chưa có đơn hàng.\nXóa vĩnh viễn khách hàng và tài khoản liên quan?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    _customerSvc.DeleteNewCustomer(customerId.Value);
                    MessageBox.Show("Đã xóa khách hàng.", "Hoàn tất");
                    LoadData();
                }
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi"); }
        }

        // === THÊM HÀM MỞ POPUP TÌM KIẾM ===
        private void OpenSearchPopup()
        {
            int? selectedId = null;
            using (var searchForm = new Form
            {
                Text = "Tìm kiếm Khách hàng",
                StartPosition = FormStartPosition.CenterScreen, // Theo yêu cầu
                FormBorderStyle = FormBorderStyle.None,       // Theo yêu cầu
                Size = new Size(1186, 739)                    // Theo yêu cầu
            })
            {
                var ucSearch = new ucCustomerSearch_Admin { Dock = DockStyle.Fill };

                ucSearch.CustomerPicked += (sender, id) =>
                {
                    selectedId = id;
                    searchForm.DialogResult = DialogResult.OK;
                    searchForm.Close();
                };

                searchForm.Controls.Add(ucSearch);

                Form ownerForm = this.FindForm();
                if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }

                if (searchForm.ShowDialog(ownerForm) == DialogResult.OK && selectedId.HasValue)
                {
                    SelectRowById(selectedId.Value); // Chọn dòng trên grid chính
                }
            }
        }
        // === KẾT THÚC THÊM HÀM ===

        #endregion

        #region Sorting Logic (Giữ nguyên)
        private void dgvCustomers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;
            var newColumn = dgvCustomers.Columns[e.ColumnIndex];
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
            var propInfo = typeof(Customer).GetProperty(propertyName);
            if (propInfo == null) return;

            List<Customer> items = _bindingList.ToList();
            List<Customer> sortedList;
            if (_sortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
            else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }

            _bindingList = new BindingList<Customer>(sortedList);
            dgvCustomers.DataSource = _bindingList;
        }
        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null) { _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; }
            _sortedColumn = null; _sortOrder = SortOrder.None;
            foreach (DataGridViewColumn col in dgvCustomers.Columns) { if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None; }
        }
        #endregion
    }
}