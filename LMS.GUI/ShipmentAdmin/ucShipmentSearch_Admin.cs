// LMS.GUI/ShipmentAdmin/ucShipmentSearch_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity; // Cần cho Include và DbFunctions
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.ShipmentAdmin
{
    public partial class ucShipmentSearch_Admin : UserControl
    {
        public event EventHandler<int> ShipmentPicked; // Đổi tên sự kiện
        private readonly ShipmentService_Admin _shipmentSvc = new ShipmentService_Admin(); // Service mới
        private readonly DriverService _driverSvc = new DriverService(); // Cần để lấy danh sách Driver
        private readonly LogisticsDbContext _dbContextForFilters = new LogisticsDbContext(); // Để lấy danh sách Kho

        private BindingList<ShipmentListItemAdminDto> _bindingList; // Dùng DTO mới
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        private readonly Timer _debounceTimer = new Timer { Interval = 350 };

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public ucShipmentSearch_Admin()
        {
            InitializeComponent();
            this.Load += UcShipmentSearch_Admin_Load;
            _debounceTimer.Tick += DebounceTimer_Tick;
            this.Disposed += (s, e) => _dbContextForFilters.Dispose(); // Dispose DbContext khi UC đóng
        }

        private void UcShipmentSearch_Admin_Load(object sender, EventArgs e)
        {
            if (this.lblTitle != null) { this.lblTitle.Text = "Tìm Kiếm Chuyến Hàng"; }
            BindFilters();
            ConfigureGrid();
            WireEvents();
            DoSearch(); // Load data ban đầu
            // Không focus control nào để người dùng tự chọn
        }

        #region Setup Controls
        private void BindFilters()
        {
            // --- Tài xế ---
            var drivers = _driverSvc.GetDriversForAdmin()
                                    .Select(d => new { d.Id, Name = d.FullName })
                                    .ToList();
            drivers.Insert(0, new { Id = 0, Name = "— Tất cả tài xế —" }); // Thêm mục "Tất cả"
            cmbDriver.DataSource = drivers;
            cmbDriver.DisplayMember = "Name";
            cmbDriver.ValueMember = "Id";

            // --- Trạng thái ---
            var statuses = new List<object> { new { Value = (ShipmentStatus?)null, Text = "— Tất cả trạng thái —" } };
            statuses.AddRange(Enum.GetValues(typeof(ShipmentStatus))
                                  .Cast<ShipmentStatus>()
                                  .Select(s => new { Value = (ShipmentStatus?)s, Text = FormatShipmentStatus(s) })); // Format tiếng Việt
            cmbStatus.DataSource = statuses;
            cmbStatus.DisplayMember = "Text";
            cmbStatus.ValueMember = "Value";

            // --- Kho hàng ---
            var warehouses = _dbContextForFilters.Warehouses
                                                .OrderBy(w => w.Name)
                                                .Select(w => new { w.Id, w.Name })
                                                .ToList();
            var warehouseListAll = new List<object> { new { Id = 0, Name = "— Tất cả kho —" } };
            warehouseListAll.AddRange(warehouses.Cast<object>());

            cmbOrigin.DataSource = new List<object>(warehouseListAll); // Tạo list mới cho từng combo
            cmbOrigin.DisplayMember = "Name";
            cmbOrigin.ValueMember = "Id";

            cmbDest.DataSource = new List<object>(warehouseListAll); // Tạo list mới
            cmbDest.DisplayMember = "Name";
            cmbDest.ValueMember = "Id";

            // --- Ngày tháng ---
            dtFrom.Checked = false; // Mặc định không check
            dtTo.Checked = false;
        }

        private void ConfigureGrid()
        {
            var g = dgvSearchResults;
            g.Columns.Clear();
            g.ApplyBaseStyle();

            // Cấu hình cột giống grid chính (ucShipment_Admin)
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShipmentNo", DataPropertyName = "ShipmentNo", HeaderText = "Mã Chuyến", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo", DataPropertyName = "OrderNo", HeaderText = "Mã Đơn", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DriverName", DataPropertyName = "DriverName", HeaderText = "Tài xế", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Route", DataPropertyName = "Route", HeaderText = "Tuyến", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "UpdatedAt", DataPropertyName = "UpdatedAt", HeaderText = "Cập nhật", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }, SortMode = DataGridViewColumnSortMode.Programmatic });

            g.CellFormatting += DgvSearchResults_CellFormatting; // Format Status
        }
        #endregion

        #region Event Wiring
        private void WireEvents()
        {
            // Nút bấm
            btnReset.Click += BtnReset_Click;
            //btnClose.Click += (s, e) => this.FindForm()?.Close();

            // Gán sự kiện Changed cho các Filter Controls
            cmbDriver.SelectedIndexChanged += FilterControl_Changed;
            cmbStatus.SelectedIndexChanged += FilterControl_Changed;
            cmbOrigin.SelectedIndexChanged += FilterControl_Changed;
            cmbDest.SelectedIndexChanged += FilterControl_Changed;
            dtFrom.ValueChanged += FilterControl_Changed; // ValueChanged tốt hơn CheckedChanged
            dtTo.ValueChanged += FilterControl_Changed;
            txtCode.TextChanged += FilterControl_Changed;

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
            // Chỉ restart timer nếu sender không phải DateTimePicker và Checked=false
            if (sender is Guna2DateTimePicker dtp && !dtp.Checked)
            {
                // Nếu bỏ check DateTimePicker, tìm kiếm ngay lập tức
                DoSearch();
            }
            else
            {
                _debounceTimer.Stop();
                _debounceTimer.Start();
            }
        }


        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            DoSearch();
        }
        #endregion

        #region Actions
        private void DoSearch()
        {
            try
            {
                // Lấy giá trị filter
                int? driverId = (cmbDriver.SelectedValue as int?) ?? 0;
                ShipmentStatus? status = cmbStatus.SelectedValue as ShipmentStatus?;
                int? originId = (cmbOrigin.SelectedValue as int?) ?? 0;
                int? destId = (cmbDest.SelectedValue as int?) ?? 0;
                DateTime? from = dtFrom.Checked ? dtFrom.Value.Date : (DateTime?)null;
                DateTime? to = dtTo.Checked ? dtTo.Value.Date.AddDays(1).AddTicks(-1) : (DateTime?)null; // Đến cuối ngày
                string code = txtCode.Text.Trim();

                var results = _shipmentSvc.SearchShipmentsForAdmin(
                    driverId <= 0 ? null : driverId, // Chuyển 0 thành null
                    status,
                    originId <= 0 ? null : originId,
                    destId <= 0 ? null : destId,
                    from,
                    to,
                    string.IsNullOrWhiteSpace(code) ? null : code
                );

                _bindingList = new BindingList<ShipmentListItemAdminDto>(results);
                dgvSearchResults.DataSource = _bindingList;

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
            // Tạm ngắt sự kiện
            cmbDriver.SelectedIndexChanged -= FilterControl_Changed;
            cmbStatus.SelectedIndexChanged -= FilterControl_Changed;
            cmbOrigin.SelectedIndexChanged -= FilterControl_Changed;
            cmbDest.SelectedIndexChanged -= FilterControl_Changed;
            dtFrom.ValueChanged -= FilterControl_Changed;
            dtTo.ValueChanged -= FilterControl_Changed;

            // Reset controls
            cmbDriver.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;
            cmbOrigin.SelectedIndex = 0;
            cmbDest.SelectedIndex = 0;
            dtFrom.Checked = false;
            dtTo.Checked = false;
            txtCode.Clear();

            // Gắn lại sự kiện
            cmbDriver.SelectedIndexChanged += FilterControl_Changed;
            cmbStatus.SelectedIndexChanged += FilterControl_Changed;
            cmbOrigin.SelectedIndexChanged += FilterControl_Changed;
            cmbDest.SelectedIndexChanged += FilterControl_Changed;
            dtFrom.ValueChanged += FilterControl_Changed;
            dtTo.ValueChanged += FilterControl_Changed;

            ResetSortGlyphs();
            DoSearch(); // Load lại tất cả
        }

        private void dgvSearchResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    if (dgvSearchResults.Rows[e.RowIndex].DataBoundItem is ShipmentListItemAdminDto selected)
                    {
                        ShipmentPicked?.Invoke(this, selected.Id); // Gửi ID ra ngoài
                        this.FindForm()?.Close();
                    }
                }
                catch (Exception ex) { MessageBox.Show($"Lỗi khi chọn: {ex.Message}", "Lỗi"); }
            }
        }
        #endregion

        #region Sorting Logic
        // (Giữ nguyên các hàm dgvSearchResults_ColumnHeaderMouseClick, ApplySort, UpdateSortGlyphs, ResetSortGlyphs)
        // Chỉ cần đổi typeof(Driver) thành typeof(ShipmentListItemAdminDto) trong ApplySort
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
            // *** ĐỔI typeof ***
            PropertyInfo propInfo = typeof(ShipmentListItemAdminDto).GetProperty(propertyName);
            if (propInfo == null) return;

            // *** ĐỔI typeof ***
            List<ShipmentListItemAdminDto> items = _bindingList.ToList();
            List<ShipmentListItemAdminDto> sortedList;
            try
            {
                if (_sortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
                else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }

                // *** ĐỔI typeof ***
                _bindingList = new BindingList<ShipmentListItemAdminDto>(sortedList);
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

        #region Cell Formatting & Helpers
        // Format Enum ShipmentStatus sang tiếng Việt (giống ucShipment_Admin)
        private void DgvSearchResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSearchResults.Columns[e.ColumnIndex].Name == "Status" && e.Value is ShipmentStatus status)
            {
                e.Value = FormatShipmentStatus(status); // Gọi hàm helper
                e.FormattingApplied = true;
            }
        }

        // Hàm helper để format Status (có thể đặt trong class Helper chung nếu muốn)
        private string FormatShipmentStatus(ShipmentStatus status)
        {
            switch (status)
            {
                case ShipmentStatus.Pending: return "Chờ nhận";
                case ShipmentStatus.Assigned: return "Đã nhận";
                case ShipmentStatus.OnRoute: return "Đang đi đường";
                case ShipmentStatus.AtWarehouse: return "Đang ở kho";
                case ShipmentStatus.ArrivedDestination: return "Đã tới đích";
                case ShipmentStatus.Delivered: return "Đã giao xong";
                case ShipmentStatus.Failed: return "Gặp sự cố";
                default: return status.ToString();
            }
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