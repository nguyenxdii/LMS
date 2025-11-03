using Guna.UI2.WinForms;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureGrid();
            WireEvents();
            LoadData();
        }

        // cấu hình lưới khách hàng
        private void ConfigureGrid()
        {
            dgvCustomers.Columns.Clear();
            dgvCustomers.ApplyBaseStyle();

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                Visible = false
            });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "Tên Khách Hàng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Phone",
                DataPropertyName = "Phone",
                HeaderText = "Số Điện Thoại",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                DataPropertyName = "Email",
                HeaderText = "Email",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Address",
                DataPropertyName = "Address",
                HeaderText = "Địa Chỉ",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 150,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
        }

        // tải dữ liệu và bind vào lưới
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
                MessageBox.Show($"Lỗi Tải Dữ Liệu Khách Hàng: {ex.Message}", "Lỗi");
                dgvCustomers.DataSource = null;
            }
        }

        // lấy id khách hàng được chọn
        private int? GetSelectedCustomerId()
        {
            if (dgvCustomers.CurrentRow != null && dgvCustomers.CurrentRow.DataBoundItem is Customer customer)
                return customer.Id;
            return null;
        }

        // bật/tắt nút theo trạng thái chọn
        private void UpdateButtonsState()
        {
            bool hasSelection = (GetSelectedCustomerId() != null);
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnViewDetail.Enabled = hasSelection;
        }

        // chọn dòng trên lưới theo id và cuộn tới vị trí
        private void SelectRowById(int customerId)
        {
            if (dgvCustomers.Rows.Count == 0 || _bindingList == null) return;

            var it = _bindingList.FirstOrDefault(c => c.Id == customerId);
            if (it == null) { UpdateButtonsState(); return; }

            int rowIndex = _bindingList.IndexOf(it);
            if (rowIndex < 0 || rowIndex >= dgvCustomers.Rows.Count) { UpdateButtonsState(); return; }

            dgvCustomers.ClearSelection();
            dgvCustomers.Rows[rowIndex].Selected = true;

            if (dgvCustomers.Columns.Contains("Name"))
                dgvCustomers.CurrentCell = dgvCustomers.Rows[rowIndex].Cells["Name"];

            if (!dgvCustomers.Rows[rowIndex].Displayed)
            {
                int firstDisplayed = Math.Max(0, rowIndex - dgvCustomers.DisplayedRowCount(false) / 2);
                dgvCustomers.FirstDisplayedScrollingRowIndex = Math.Min(firstDisplayed, dgvCustomers.RowCount - 1);
            }

            UpdateButtonsState();
        }

        // gắn các sự kiện điều khiển
        private void WireEvents()
        {
            btnReload.Click += (s, e) => LoadData();
            btnAdd.Click += (s, e) => OpenEditorPopup(null);
            btnEdit.Click += (s, e) => OpenEditorPopup(GetSelectedCustomerId());
            btnDelete.Click += (s, e) => DeleteCustomer();
            btnViewDetail.Click += (s, e) => OpenDetailPopup(GetSelectedCustomerId());
            btnSearch.Click += (s, e) => OpenSearchPopup();

            dgvCustomers.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0) OpenDetailPopup(GetSelectedCustomerId());
            };
            dgvCustomers.ColumnHeaderMouseClick += dgvCustomers_ColumnHeaderMouseClick;
            dgvCustomers.SelectionChanged += (s, e) => UpdateButtonsState();
        }

        // mở popup thêm/sửa khách hàng
        private void OpenEditorPopup(int? customerId)
        {
            if (customerId == null && !btnAdd.Enabled) return;
            if (customerId.HasValue && !btnEdit.Enabled) return;

            var mode = customerId.HasValue
                ? ucCustomerEditor_Admin.EditorMode.Edit
                : ucCustomerEditor_Admin.EditorMode.Add;

            try
            {
                var ucEditor = new ucCustomerEditor_Admin();
                ucEditor.LoadData(mode, customerId);

                using (var popupForm = new Form
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.None,
                    Width = 657,
                    Height = 689
                })
                {
                    popupForm.Controls.Add(ucEditor);
                    var ownerForm = this.FindForm();
                    if (ownerForm == null)
                    {
                        MessageBox.Show("Không Thể Xác Định Form Cha.", "Lỗi");
                        return;
                    }

                    var result = popupForm.ShowDialog(ownerForm);
                    if (result == DialogResult.OK) LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi Mở Form: {ex.Message}", "Lỗi");
            }
        }

        // mở popup chi tiết khách hàng
        private void OpenDetailPopup(int? customerId)
        {
            if (!customerId.HasValue)
            {
                MessageBox.Show("Vui Lòng Chọn Khách Hàng.", "Thông Báo");
                return;
            }

            try
            {
                var ucDetail = new ucCustomerDetail_Admin(customerId.Value);

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
                    if (ownerForm == null)
                    {
                        MessageBox.Show("Không Thể Xác Định Form Cha.", "Lỗi");
                        return;
                    }

                    popupForm.ShowDialog(ownerForm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi Mở Chi Tiết: {ex.Message}", "Lỗi");
            }
        }

        // xóa khách hàng (kiểm tra ràng buộc đơn hàng)
        private void DeleteCustomer()
        {
            var customerId = GetSelectedCustomerId();
            if (!customerId.HasValue)
            {
                MessageBox.Show("Vui Lòng Chọn Khách Hàng.", "Thông Báo");
                return;
            }

            try
            {
                if (_customerSvc.CheckCustomerHasOrders(customerId.Value))
                {
                    MessageBox.Show(
                        "Không Thể Xóa Vì Khách Hàng Đã Có Đơn Hàng.\nHãy Xem Xét Khóa Tài Khoản.",
                        "Không Thể Xóa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show(
                    "Khách Hàng Này Chưa Có Đơn Hàng.\nXóa Vĩnh Viễn Khách Hàng Và Tài Khoản Liên Quan?",
                    "Xác Nhận Xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    _customerSvc.DeleteNewCustomer(customerId.Value);
                    MessageBox.Show("Đã Xóa Khách Hàng.", "Hoàn Tất");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi Khi Xóa: {ex.Message}", "Lỗi");
            }
        }

        // mở popup tìm kiếm khách hàng và chọn dòng sau khi chọn
        private void OpenSearchPopup()
        {
            int? selectedId = null;

            using (var searchForm = new Form
            {
                Text = "Tìm Kiếm Khách Hàng",
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new Size(1186, 739)
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

                var ownerForm = this.FindForm();
                if (ownerForm == null)
                {
                    MessageBox.Show("Không Thể Xác Định Form Cha.", "Lỗi");
                    return;
                }

                if (searchForm.ShowDialog(ownerForm) == DialogResult.OK && selectedId.HasValue)
                    SelectRowById(selectedId.Value);
            }
        }

        // xử lý click tiêu đề cột để sắp xếp
        private void dgvCustomers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;

            var newColumn = dgvCustomers.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn)
            {
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                if (_sortedColumn != null)
                    _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;

                _sortOrder = SortOrder.Ascending;
                _sortedColumn = newColumn;
            }

            ApplySort();

            if (_sortedColumn != null)
                _sortedColumn.HeaderCell.SortGlyphDirection = _sortOrder;
        }

        // áp dụng sắp xếp trên danh sách đang bind
        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null) return;

            string propertyName = _sortedColumn.DataPropertyName;
            var propInfo = typeof(Customer).GetProperty(propertyName);
            if (propInfo == null) return;

            var items = _bindingList.ToList();
            List<Customer> sortedList = (_sortOrder == SortOrder.Ascending)
                ? items.OrderBy(x => propInfo.GetValue(x, null)).ToList()
                : items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

            _bindingList = new BindingList<Customer>(sortedList);
            dgvCustomers.DataSource = _bindingList;
        }

        // reset hiển thị mũi tên sắp xếp
        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null)
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;

            _sortedColumn = null;
            _sortOrder = SortOrder.None;

            foreach (DataGridViewColumn col in dgvCustomers.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }
    }
}
