// LMS.GUI/DriverAdmin/ucDriverSearch_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.DriverAdmin
{
    public partial class ucDriverSearch_Admin : UserControl
    {
        public event EventHandler<int> DriverPicked;
        private readonly DriverService _driverSvc = new DriverService();
        private BindingList<Driver> _bindingList;
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        // === (1) THÊM BIẾN TIMER CHO LIVE SEARCH ===
        private readonly Timer _debounceTimer = new Timer { Interval = 350 }; // Đợi 350ms sau khi ngừng gõ

        // Biến kéo thả
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public ucDriverSearch_Admin()
        {
            InitializeComponent();
            this.Load += UcDriverSearch_Admin_Load;

            // === (2) GÁN SỰ KIỆN TICK CHO TIMER ===
            _debounceTimer.Tick += DebounceTimer_Tick;
        }

        private void UcDriverSearch_Admin_Load(object sender, EventArgs e)
        {
            if (this.lblTitle != null)
            {
                this.lblTitle.Text = "Tìm Kiếm Tài Xế";
            }

            BindFilters();
            ConfigureGrid();
            WireEvents();
            DoSearch(); // === (3) GỌI DoSearch() NGAY KHI LOAD ĐỂ HIỂN THỊ DỮ LIỆU BAN ĐẦU ===
            txtFullName.Focus();
        }

        #region Setup Controls (Filters & Grid)
        // (Hàm BindFilters và ConfigureGrid giữ nguyên)
        private void BindFilters()
        {
            var licenseTypes = new List<object> { new { Value = "", Text = "— Tất cả —" } };
            licenseTypes.AddRange(new object[] {
                new { Value = "B2", Text = "B2" }, new { Value = "C", Text = "C" },
                new { Value = "CE", Text = "CE" }, new { Value = "D", Text = "D" },
                new { Value = "DE", Text = "DE" }, new { Value = "FC", Text = "FC" }
            });
            cmbLicenseType.DataSource = licenseTypes;
            cmbLicenseType.DisplayMember = "Text";
            cmbLicenseType.ValueMember = "Value";
            cmbLicenseType.SelectedIndex = 0;
        }
        private void ConfigureGrid()
        {
            var g = dgvSearchResults;
            g.Columns.Clear();
            g.ApplyBaseStyle();
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "FullName", DataPropertyName = "FullName", HeaderText = "Họ Tên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", DataPropertyName = "Phone", HeaderText = "Số điện thoại", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CitizenId", DataPropertyName = "CitizenId", HeaderText = "CCCD", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "LicenseType", DataPropertyName = "LicenseType", HeaderText = "Hạng Bằng Lái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
        }
        #endregion

        #region Event Wiring
        private void WireEvents()
        {
            // === (4) THAY ĐỔI CÁCH GÁN SỰ KIỆN ===
            // Nút bấm (có thể bỏ btnDoSearch nếu không cần)
            // btnDoSearch.Click += (s, e) => DoSearch(); // Bỏ hoặc giữ tùy ý
            btnReset.Click += BtnReset_Click;
            //btnClose.Click += (s, e) => this.FindForm()?.Close();

            // Gán sự kiện TextChanged/SelectedIndexChanged cho các Filter Controls
            txtFullName.TextChanged += FilterControl_Changed;
            txtPhone.TextChanged += FilterControl_Changed;
            txtCitizenId.TextChanged += FilterControl_Changed;
            cmbLicenseType.SelectedIndexChanged += FilterControl_Changed;
            // Bỏ sự kiện KeyDown nếu không cần Enter nữa
            // txtFullName.KeyDown -= Filter_KeyDown;
            // txtPhone.KeyDown -= Filter_KeyDown;
            // txtCitizenId.KeyDown -= Filter_KeyDown;

            // Sự kiện Grid (giữ nguyên)
            dgvSearchResults.ColumnHeaderMouseClick += dgvSearchResults_ColumnHeaderMouseClick;
            dgvSearchResults.CellDoubleClick += dgvSearchResults_CellDoubleClick;

            // Gán sự kiện kéo thả (giữ nguyên)
            if (this.pnlTop != null)
            {
                this.pnlTop.MouseDown += PnlTop_MouseDown;
                this.pnlTop.MouseMove += PnlTop_MouseMove;
                this.pnlTop.MouseUp += PnlTop_MouseUp;
            }
        }

        // === (5) HÀM XỬ LÝ CHUNG KHI FILTER THAY ĐỔI ===
        private void FilterControl_Changed(object sender, EventArgs e)
        {
            _debounceTimer.Stop(); // Dừng timer cũ (nếu đang chạy)
            _debounceTimer.Start(); // Bắt đầu đếm lại
        }

        // === (6) HÀM XỬ LÝ KHI TIMER TICK (NGƯỜI DÙNG NGỪNG GÕ) ===
        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop(); // Dừng timer hiện tại
            DoSearch(); // Thực hiện tìm kiếm
        }

        // (Hàm Filter_KeyDown có thể bỏ đi)
        // private void Filter_KeyDown(object sender, KeyEventArgs e) { ... }

        #endregion

        #region Actions (Search, Reset, Select)
        private void DoSearch()
        {
            try
            {
                // Lấy giá trị filter từ controls (giữ nguyên)
                string nameFilter = txtFullName.Text.Trim();
                string phoneFilter = txtPhone.Text.Trim();
                string citizenIdFilter = txtCitizenId.Text.Trim();
                string licenseFilter = cmbLicenseType.SelectedValue?.ToString();

                var results = _driverSvc.SearchDriversForAdmin(
                    string.IsNullOrWhiteSpace(nameFilter) ? null : nameFilter,
                    string.IsNullOrWhiteSpace(phoneFilter) ? null : phoneFilter,
                    string.IsNullOrWhiteSpace(citizenIdFilter) ? null : citizenIdFilter,
                    string.IsNullOrWhiteSpace(licenseFilter) ? null : licenseFilter
                );

                _bindingList = new BindingList<Driver>(results);
                dgvSearchResults.DataSource = _bindingList; // Cập nhật DataSource

                ApplySort();
                UpdateSortGlyphs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi");
                dgvSearchResults.DataSource = null;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            // === (7) SỬA LẠI RESET ĐỂ GỌI DoSearch() ===
            // Tạm ngắt sự kiện SelectedIndexChanged để tránh gọi DoSearch 2 lần
            cmbLicenseType.SelectedIndexChanged -= FilterControl_Changed;

            txtFullName.Clear();
            txtPhone.Clear();
            txtCitizenId.Clear();
            cmbLicenseType.SelectedIndex = 0; // Đặt về "Tất cả"

            // Gắn lại sự kiện
            cmbLicenseType.SelectedIndexChanged += FilterControl_Changed;

            // Reset sort
            ResetSortGlyphs();
            DoSearch(); // Gọi DoSearch để tải lại danh sách đầy đủ
            txtFullName.Focus();
        }

        // (Hàm dgvSearchResults_CellDoubleClick giữ nguyên)
        private void dgvSearchResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    if (dgvSearchResults.Rows[e.RowIndex].DataBoundItem is Driver selectedDriver)
                    {
                        DriverPicked?.Invoke(this, selectedDriver.Id);
                        this.FindForm()?.Close();
                    }
                }
                catch (Exception ex) { MessageBox.Show($"Lỗi khi chọn: {ex.Message}", "Lỗi"); }
            }
        }
        #endregion

        #region Sorting Logic (Giữ nguyên)
        private void dgvSearchResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;
            var newColumn = dgvSearchResults.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn) { _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
            else { if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _sortOrder = SortOrder.Ascending; _sortedColumn = newColumn; }
            ApplySort();
            UpdateSortGlyphs();
        }
        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;
            string propertyName = _sortedColumn.DataPropertyName;
            PropertyInfo propInfo = typeof(Driver).GetProperty(propertyName);
            if (propInfo == null) return;

            List<Driver> items = _bindingList.ToList();
            List<Driver> sortedList;
            try
            {
                if (_sortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
                else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }
                _bindingList = new BindingList<Driver>(sortedList);
                dgvSearchResults.DataSource = _bindingList;
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp: {ex.Message}"); ResetSortGlyphs(); }
        }
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