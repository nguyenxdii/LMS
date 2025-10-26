// LMS.GUI/CustomerAdmin/ucCustomerSearch_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos; // Có thể không cần
using LMS.BUS.Helpers;
using LMS.BUS.Services; // Sử dụng CustomerService
using LMS.DAL;
using LMS.DAL.Models; // Sử dụng Customer
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LMS.GUI.CustomerAdmin // Đổi namespace
{
    public partial class ucCustomerSearch_Admin : UserControl
    {
        // Sự kiện báo ID khách hàng đã được chọn
        public event EventHandler<int> CustomerPicked; // Đổi tên sự kiện
        private readonly CustomerService _customerSvc = new CustomerService(); // Sử dụng CustomerService
        private BindingList<Customer> _bindingList; // Sẽ hiển thị List<Customer>
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        private readonly Timer _debounceTimer = new Timer { Interval = 350 };

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public ucCustomerSearch_Admin()
        {
            InitializeComponent();
            this.Load += UcCustomerSearch_Admin_Load;
            _debounceTimer.Tick += DebounceTimer_Tick;
        }

        private void UcCustomerSearch_Admin_Load(object sender, EventArgs e)
        {
            // Cập nhật tiêu đề (Giả sử Label tên là lblTitle)
            if (this.lblTitle != null)
            {
                this.lblTitle.Text = "Tìm Kiếm Khách Hàng"; // Đổi tiêu đề
            }

            // BindFilters(); // Không cần filter ComboBox cho Customer trong ví dụ này
            ConfigureGrid();
            WireEvents();
            DoSearch(); // Hiển thị dữ liệu ban đầu
            txtFullName.Focus(); // Focus vào ô tên (Giả sử là txtName)
        }

        #region Setup Controls (Grid)
        // Bỏ BindFilters nếu không có ComboBox
        // private void BindFilters() { ... }

        private void ConfigureGrid()
        {
            var g = dgvSearchResults;
            g.Columns.Clear();
            g.ApplyBaseStyle();

            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            // Cập nhật các cột cho Customer
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "Tên Khách Hàng", // Đổi HeaderText
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 30,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Phone",
                DataPropertyName = "Phone",
                HeaderText = "Số điện thoại",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                DataPropertyName = "Email",
                HeaderText = "Email", // Thêm cột Email
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Address",
                DataPropertyName = "Address",
                HeaderText = "Địa Chỉ", // Thêm cột Địa chỉ
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 50,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
        }
        #endregion

        #region Event Wiring
        private void WireEvents()
        {
            // Nút bấm
            btnReset.Click += BtnReset_Click;
            //btnClose.Click += (s, e) => this.FindForm()?.Close();

            // Gán sự kiện TextChanged cho các Filter Controls mới
            txtFullName.TextChanged += FilterControl_Changed; // Giả sử ô tên là txtName
            txtPhone.TextChanged += FilterControl_Changed;
            txtEmail.TextChanged += FilterControl_Changed; // Thêm Email
            txtAddress.TextChanged += FilterControl_Changed; // Thêm Address

            // Sự kiện Grid
            dgvSearchResults.ColumnHeaderMouseClick += dgvSearchResults_ColumnHeaderMouseClick;
            dgvSearchResults.CellDoubleClick += dgvSearchResults_CellDoubleClick;

            // Kéo thả
            if (this.pnlTop != null)
            {
                this.pnlTop.MouseDown += PnlTop_MouseDown;
                this.pnlTop.MouseMove += PnlTop_MouseMove;
                this.pnlTop.MouseUp += PnlTop_MouseUp;
            }
        }

        private void FilterControl_Changed(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            DoSearch();
        }
        #endregion

        #region Actions (Search, Reset, Select)
        private void DoSearch()
        {
            try
            {
                // Lấy giá trị từ các control lọc mới
                string nameFilter = txtFullName.Text.Trim(); // Giả sử ô tên là txtName
                string phoneFilter = txtPhone.Text.Trim();
                string emailFilter = txtEmail.Text.Trim(); // Thêm Email
                string addressFilter = txtAddress.Text.Trim(); // Thêm Address

                // Gọi Service để tìm kiếm (Hàm này cần được tạo trong CustomerService)
                var results = _customerSvc.SearchCustomersForAdmin( // Đổi tên hàm và Service
                    string.IsNullOrWhiteSpace(nameFilter) ? null : nameFilter,
                    string.IsNullOrWhiteSpace(phoneFilter) ? null : phoneFilter,
                    string.IsNullOrWhiteSpace(emailFilter) ? null : emailFilter,     // Thêm Email
                    string.IsNullOrWhiteSpace(addressFilter) ? null : addressFilter  // Thêm Address
                );

                _bindingList = new BindingList<Customer>(results); // Đổi kiểu dữ liệu
                dgvSearchResults.DataSource = _bindingList;

                ApplySort();
                UpdateSortGlyphs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm khách hàng: {ex.Message}", "Lỗi"); // Đổi thông báo lỗi
                dgvSearchResults.DataSource = null;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            // Xóa các ô text mới
            txtFullName.Clear(); // Giả sử ô tên là txtName
            txtPhone.Clear();
            txtEmail.Clear(); // Thêm Email
            txtAddress.Clear(); // Thêm Address

            ResetSortGlyphs();
            DoSearch(); // Tải lại danh sách đầy đủ
            txtFullName.Focus(); // Focus lại ô tên
        }

        private void dgvSearchResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    // Đổi kiểu dữ liệu sang Customer và sự kiện CustomerPicked
                    if (dgvSearchResults.Rows[e.RowIndex].DataBoundItem is Customer selectedCustomer)
                    {
                        CustomerPicked?.Invoke(this, selectedCustomer.Id); // Kích hoạt sự kiện mới
                        this.FindForm()?.Close();
                    }
                }
                catch (Exception ex) { MessageBox.Show($"Lỗi khi chọn khách hàng: {ex.Message}", "Lỗi"); } // Đổi thông báo lỗi
            }
        }
        #endregion

        #region Sorting Logic (Cần đổi typeof)
        private void dgvSearchResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;
            var newColumn = dgvSearchResults.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn) { _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
            else { if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _sortOrder = SortOrder.Ascending; _sortedColumn = newColumn; }

            ApplySort(); // ApplySort sẽ xử lý việc đổi typeof
            UpdateSortGlyphs();
        }

        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;
            string propertyName = _sortedColumn.DataPropertyName;
            PropertyInfo propInfo = typeof(Customer).GetProperty(propertyName); // Đổi typeof(Driver) thành typeof(Customer)
            if (propInfo == null) return;

            List<Customer> items = _bindingList.ToList(); // Đổi kiểu dữ liệu
            List<Customer> sortedList; // Đổi kiểu dữ liệu
            try
            {
                if (_sortOrder == SortOrder.Ascending)
                    sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList();
                else
                    sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

                _bindingList = new BindingList<Customer>(sortedList); // Đổi kiểu dữ liệu
                dgvSearchResults.DataSource = _bindingList;
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp: {ex.Message}"); ResetSortGlyphs(); }
        }

        // (UpdateSortGlyphs và ResetSortGlyphs giữ nguyên)
        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvSearchResults.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None;
                if (_sortedColumn != null && col == _sortedColumn) col.HeaderCell.SortGlyphDirection = _sortOrder;
            }
        }
        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            _sortedColumn = null; _sortOrder = SortOrder.None;
            UpdateSortGlyphs();
        }
        #endregion

        #region Kéo thả Form (Giữ nguyên)
        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            Form parentForm = this.FindForm(); if (parentForm == null) return;
            if (e.Button == MouseButtons.Left) { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = parentForm.Location; }
        }
        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            Form parentForm = this.FindForm(); if (parentForm == null) return;
            if (dragging) { Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint)); parentForm.Location = Point.Add(dragFormPoint, new Size(dif)); }
        }
        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { dragging = false; }
        }
        #endregion
    }
}