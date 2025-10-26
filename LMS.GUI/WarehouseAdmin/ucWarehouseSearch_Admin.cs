// LMS.GUI/WarehouseAdmin/ucWarehouseSearch_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.WarehouseAdmin
{
    public partial class ucWarehouseSearch_Admin : UserControl
    {
        // Event to notify when a warehouse is picked
        public event EventHandler<int> WarehousePicked;

        // Service Dependency
        private readonly WarehouseService_Admin _warehouseSvc = new WarehouseService_Admin(); // Ensure this service exists

        // Grid Data & Sorting
        private BindingList<Warehouse> _bindingList; // Using Model directly
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        // Live Search Timer
        private readonly Timer _debounceTimer = new Timer { Interval = 350 }; // 350ms delay

        // Dragging State
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public ucWarehouseSearch_Admin()
        {
            InitializeComponent();
            this.Load += UcWarehouseSearch_Admin_Load;
            _debounceTimer.Tick += DebounceTimer_Tick; // Link timer event
        }

        private void UcWarehouseSearch_Admin_Load(object sender, EventArgs e)
        {
            // Set Title
            if (lblTitle != null) lblTitle.Text = "Tìm kiếm Kho";

            LoadFilters(); // Load combobox filter options
            ConfigureGrid();
            WireEvents();
            DoSearch(); // Initial search (load all active by default or based on filters)
            txtSearchName.Focus(); // Focus the name search box
        }

        #region Setup Controls (Filters & Grid)

        private void LoadFilters()
        {
            // Zone Filter ComboBox
            var zoneItems = new List<object> { new { Value = (Zone?)null, Text = "— Tất cả vùng —" } }; // Nullable for 'All'
            zoneItems.AddRange(Enum.GetValues(typeof(Zone))
                                   .Cast<Zone>()
                                   .Select(z => new { Value = (Zone?)z, Text = FormatZone(z) }));
            cmbSearchZone.DataSource = zoneItems;
            cmbSearchZone.DisplayMember = "Text";
            cmbSearchZone.ValueMember = "Value";
            cmbSearchZone.SelectedIndex = 0; // Default to 'All'

            // Status Filter ComboBox
            var statusItems = new List<object> {
                new { Value = (bool?)null, Text = "— Tất cả trạng thái —" }, // Nullable for 'All'
                new { Value = (bool?)true, Text = "Hoạt động" },
                new { Value = (bool?)false, Text = "Đã khóa" }
            };
            cmbSearchStatus.DataSource = statusItems;
            cmbSearchStatus.DisplayMember = "Text";
            cmbSearchStatus.ValueMember = "Value";
            cmbSearchStatus.SelectedIndex = 0; // Default to 'All'
        }

        private void ConfigureGrid()
        {
            var g = dgvSearchResults;
            g.Columns.Clear();
            g.ApplyBaseStyle(); // Use GridHelper extension

            // Define Columns (similar to main grid, maybe fewer details)
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "Tên Kho",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 30,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Address",
                DataPropertyName = "Address",
                HeaderText = "Địa chỉ",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Zone",
                DataPropertyName = "ZoneId",
                HeaderText = "Vùng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            // Type might not be needed for search results, add if desired
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                DataPropertyName = "IsActive",
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            // Add Event Handlers
            g.CellFormatting += DgvSearchResults_CellFormatting;
            g.RowPrePaint += DgvSearchResults_RowPrePaint; // Reuse coloring logic
            g.ColumnHeaderMouseClick += DgvSearchResults_ColumnHeaderMouseClick;
            g.CellDoubleClick += DgvSearchResults_CellDoubleClick; // Double-click to pick
        }

        // Cell Formatting (Copied and adapted from ucWarehouse_Admin)
        private void DgvSearchResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            string colName = dgvSearchResults.Columns[e.ColumnIndex].Name;
            if (colName == "Zone" && e.Value is Zone zone) { e.Value = FormatZone(zone); e.FormattingApplied = true; }
            else if (colName == "Status" && e.Value is bool isActive) { e.Value = isActive ? "Hoạt động" : "Đã khóa"; e.FormattingApplied = true; }
        }
        // Row PrePaint (Copied and adapted from ucWarehouse_Admin)
        private void DgvSearchResults_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvSearchResults.Rows[e.RowIndex];
            if (row.DataBoundItem is Warehouse wh) { row.DefaultCellStyle.ForeColor = wh.IsActive ? Color.Black : Color.Gray; }
        }
        // Enum Formatting Helpers (Copied from ucWarehouse_Admin)
        private string FormatZone(Zone zone) { switch (zone) { case Zone.North: return "Bắc"; case Zone.Central: return "Trung"; case Zone.South: return "Nam"; default: return zone.ToString(); } }

        #endregion

        #region Event Wiring & Live Search

        private void WireEvents()
        {
            // Filter Controls -> Trigger Debounce Timer
            txtSearchName.TextChanged += FilterControl_Changed;
            cmbSearchZone.SelectedIndexChanged += FilterControl_Changed;
            cmbSearchStatus.SelectedIndexChanged += FilterControl_Changed;

            // Buttons
            btnReset.Click += BtnReset_Click;
            btnClose.Click += (s, e) => this.FindForm()?.Close(); // Close the popup

            // Grid events are wired in ConfigureGrid

            // Drag and Drop for the parent Form
            Control dragHandle = pnlTop;
            if (dragHandle != null)
            {
                dragHandle.MouseDown += PnlTop_MouseDown;
                dragHandle.MouseMove += PnlTop_MouseMove;
                dragHandle.MouseUp += PnlTop_MouseUp;
            }
        }

        // When any filter control changes, reset the timer
        private void FilterControl_Changed(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        // When timer ticks (user stopped typing/changing selection), perform search
        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            DoSearch();
        }

        #endregion

        #region Actions (Search, Reset, Pick)

        private void DoSearch()
        {
            try
            {
                // Get filter values from controls
                string nameFilter = txtSearchName.Text.Trim();
                Zone? zoneFilter = cmbSearchZone.SelectedValue as Zone?; // Nullable Zone
                bool? statusFilter = cmbSearchStatus.SelectedValue as bool?; // Nullable bool

                // Call service method (assuming SearchWarehouses exists)
                var results = _warehouseSvc.SearchWarehouses(
                    string.IsNullOrWhiteSpace(nameFilter) ? null : nameFilter,
                    zoneFilter,
                    statusFilter
                );

                _bindingList = new BindingList<Warehouse>(results);
                dgvSearchResults.DataSource = _bindingList;

                ApplySort(); // Re-apply sorting
                UpdateSortGlyphs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm kho: {ex.Message}", "Lỗi");
                dgvSearchResults.DataSource = null;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            // Temporarily detach event handlers to prevent multiple searches
            txtSearchName.TextChanged -= FilterControl_Changed;
            cmbSearchZone.SelectedIndexChanged -= FilterControl_Changed;
            cmbSearchStatus.SelectedIndexChanged -= FilterControl_Changed;

            // Clear/reset controls
            txtSearchName.Clear();
            cmbSearchZone.SelectedIndex = 0; // "All"
            cmbSearchStatus.SelectedIndex = 0; // "All"

            // Reattach event handlers
            txtSearchName.TextChanged += FilterControl_Changed;
            cmbSearchZone.SelectedIndexChanged += FilterControl_Changed;
            cmbSearchStatus.SelectedIndexChanged += FilterControl_Changed;

            ResetSortGlyphs();
            DoSearch(); // Perform search with reset filters
            txtSearchName.Focus();
        }

        // Handle double-click on a row to pick the warehouse
        private void DgvSearchResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSearchResults.Rows[e.RowIndex].DataBoundItem is Warehouse selectedWarehouse)
            {
                try
                {
                    // Raise the event with the selected ID
                    WarehousePicked?.Invoke(this, selectedWarehouse.Id);
                    // The main UC (ucWarehouse_Admin) will handle closing the form via DialogResult.OK
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi chọn kho: {ex.Message}", "Lỗi");
                }
            }
        }

        #endregion

        #region Sorting Logic (Copied and adapted from ucWarehouse_Admin)

        private void DgvSearchResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;
            var newColumn = dgvSearchResults.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;
            if (_sortedColumn == newColumn) { _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
            else { if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _sortOrder = SortOrder.Ascending; _sortedColumn = newColumn; }
            ApplySort(); UpdateSortGlyphs();
        }
        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;
            string propertyName = _sortedColumn.DataPropertyName; PropertyInfo propInfo = typeof(Warehouse).GetProperty(propertyName);
            if (propInfo == null) return; List<Warehouse> items = _bindingList.ToList(); List<Warehouse> sortedList;
            try { if (_sortOrder == SortOrder.Ascending) sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); else sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); _bindingList = new BindingList<Warehouse>(sortedList); dgvSearchResults.DataSource = _bindingList; }
            catch (Exception ex) { MessageBox.Show($"Lỗi khi sắp xếp: {ex.Message}"); ResetSortGlyphs(); }
        }
        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvSearchResults.Columns) { if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None; if (_sortedColumn != null && col == _sortedColumn) col.HeaderCell.SortGlyphDirection = _sortOrder; }
        }
        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _sortedColumn = null; _sortOrder = SortOrder.None; UpdateSortGlyphs();
        }

        #endregion

        #region Form Dragging Logic

        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { Form parentForm = this.FindForm(); if (parentForm != null) { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = parentForm.Location; } }
        }
        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging) { Form parentForm = this.FindForm(); if (parentForm != null) { Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint)); parentForm.Location = Point.Add(dragFormPoint, new Size(dif)); } }
        }
        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { dragging = false; }
        }

        #endregion
    }
}