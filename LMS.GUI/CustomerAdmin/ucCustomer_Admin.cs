
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