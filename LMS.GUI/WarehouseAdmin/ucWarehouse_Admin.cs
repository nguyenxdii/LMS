using Guna.UI2.WinForms;
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

namespace LMS.GUI.WarehouseAdmin
{
    public partial class ucWarehouse_Admin : UserControl
    {
        private readonly WarehouseService_Admin _warehouseSvc = new WarehouseService_Admin(); // Ensure this service exists

        private BindingList<Warehouse> _bindingList; // Using the Model directly for now
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        public ucWarehouse_Admin()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureGrid();
            WireEvents();
            LoadData(); // Load initial data
        }

        #region Grid Configuration & Formatting

        private void ConfigureGrid()
        {
            dgvWarehouses.Columns.Clear();
            dgvWarehouses.ApplyBaseStyle(); // Use GridHelper extension

            dgvWarehouses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                Visible = false
            });
            dgvWarehouses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "Tên Kho",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 30,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            dgvWarehouses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Address",
                DataPropertyName = "Address",
                HeaderText = "Địa chỉ",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            dgvWarehouses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Zone",
                DataPropertyName = "ZoneId",
                HeaderText = "Vùng", // Use ZoneId for sorting
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            dgvWarehouses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Type",
                DataPropertyName = "Type",
                HeaderText = "Loại Kho", // Use Type for sorting
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            dgvWarehouses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                DataPropertyName = "IsActive",
                HeaderText = "Trạng thái", // Use IsActive for sorting
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            dgvWarehouses.CellFormatting += DgvWarehouses_CellFormatting;
            dgvWarehouses.RowPrePaint += DgvWarehouses_RowPrePaint;
            dgvWarehouses.ColumnHeaderMouseClick += DgvWarehouses_ColumnHeaderMouseClick;
            dgvWarehouses.SelectionChanged += DgvWarehouses_SelectionChanged;
            dgvWarehouses.CellDoubleClick += (s, ev) => { if (ev.RowIndex >= 0) OpenEditorPopup(GetSelectedWarehouseId()); }; // Double-click to Edit
        }

        private void DgvWarehouses_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            string colName = dgvWarehouses.Columns[e.ColumnIndex].Name;

            if (colName == "Zone" && e.Value is Zone zone)
            {
                e.Value = FormatZone(zone);
                e.FormattingApplied = true;
            }
            else if (colName == "Type" && e.Value is WarehouseType type)
            {
                e.Value = FormatWarehouseType(type);
                e.FormattingApplied = true;
            }
            else if (colName == "Status" && e.Value is bool isActive)
            {
                e.Value = isActive ? "Hoạt động" : "Đã khóa";
                e.FormattingApplied = true;
            }
        }

        private void DgvWarehouses_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvWarehouses.Rows[e.RowIndex];
            if (row.DataBoundItem is Warehouse wh)
            {
                row.DefaultCellStyle.ForeColor = wh.IsActive ? Color.Black : Color.Gray;
            }
        }

        private void DgvWarehouses_SelectionChanged(object sender, EventArgs e)
        {
            // Cập nhật trạng thái các nút bấm dựa vào dòng đang được chọn
            UpdateButtonsState();
        }

        private string FormatZone(Zone zone)
        {
            switch (zone) { case Zone.North: return "Bắc"; case Zone.Central: return "Trung"; case Zone.South: return "Nam"; default: return zone.ToString(); }
        }
        private string FormatWarehouseType(WarehouseType type)
        {
            switch (type) { case WarehouseType.Main: return "Kho chính"; case WarehouseType.Hub: return "Trung chuyển"; case WarehouseType.Local: return "Địa phương"; default: return type.ToString(); }
        }

        #endregion

        #region Data Loading & Helpers

        private void LoadData()
        {
            try
            {
                var warehouses = _warehouseSvc.GetWarehouses(); // Assuming service returns List<Warehouse>
                _bindingList = new BindingList<Warehouse>(warehouses);
                dgvWarehouses.DataSource = _bindingList;

                ApplySort(); // Re-apply sort if needed
                UpdateSortGlyphs();
                UpdateButtonsState(); // Update buttons based on selection/data

                if (dgvWarehouses.Rows.Count > 0)
                {
                    dgvWarehouses.ClearSelection();
                    dgvWarehouses.Rows[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvWarehouses.DataSource = null;
            }
        }

        private int? GetSelectedWarehouseId()
        {
            if (dgvWarehouses.CurrentRow != null && dgvWarehouses.CurrentRow.DataBoundItem is Warehouse wh)
            {
                return wh.Id;
            }
            return null;
        }

        private Warehouse GetSelectedWarehouse()
        {
            return dgvWarehouses.CurrentRow?.DataBoundItem as Warehouse;
        }

        private void UpdateButtonsState()
        {
            Warehouse selectedWarehouse = GetSelectedWarehouse();
            bool hasSelection = selectedWarehouse != null;

            btnEdit.Enabled = hasSelection;
            btnToggleActive.Enabled = hasSelection;

            if (hasSelection)
            {
                btnToggleActive.Text = selectedWarehouse.IsActive ? "Khóa" : "Kích hoạt";
                btnToggleActive.FillColor = selectedWarehouse.IsActive ? Color.FromArgb(220, 53, 69) : Color.FromArgb(40, 167, 69); // Red for Lock, Green for Activate
            }
            else
            {
                btnToggleActive.Text = "Khóa / K.Hoạt";
                btnToggleActive.FillColor = Color.Gray; // Default disabled color
            }
        }

        private void SelectRowById(int warehouseId)
        {
            if (dgvWarehouses.Rows.Count == 0 || _bindingList == null) return;

            Warehouse itemToSelect = _bindingList.FirstOrDefault(w => w.Id == warehouseId);
            if (itemToSelect != null)
            {
                int rowIndex = _bindingList.IndexOf(itemToSelect);
                if (rowIndex >= 0 && rowIndex < dgvWarehouses.Rows.Count)
                {
                    dgvWarehouses.ClearSelection();
                    dgvWarehouses.Rows[rowIndex].Selected = true;
                    if (dgvWarehouses.Columns.Contains("Name"))
                        dgvWarehouses.CurrentCell = dgvWarehouses.Rows[rowIndex].Cells["Name"];

                    // Scroll to the row if it's not visible
                    if (!dgvWarehouses.Rows[rowIndex].Displayed)
                    {
                        int firstDisplayed = Math.Max(0, rowIndex - dgvWarehouses.DisplayedRowCount(false) / 2);
                        dgvWarehouses.FirstDisplayedScrollingRowIndex = Math.Min(firstDisplayed, dgvWarehouses.RowCount - 1);
                    }
                }
            }
            UpdateButtonsState();
        }

        #endregion

        #region Event Wiring

        private void WireEvents()
        {
            btnReload.Click += (s, e) => LoadData();
            btnAdd.Click += (s, e) => OpenEditorPopup(null); // null ID means Add mode
            btnEdit.Click += (s, e) => OpenEditorPopup(GetSelectedWarehouseId());
            btnToggleActive.Click += (s, e) => ToggleActiveStatus();
            btnSearch.Click += (s, e) => OpenSearchPopup();

            dgvWarehouses.SelectionChanged += (s, e) => UpdateButtonsState();
        }

        #endregion

        #region Actions (Popup Opening, Toggle Status)

        private void OpenEditorPopup(int? warehouseId)
        {
            if (warehouseId == null && !btnAdd.Enabled) return;
            if (warehouseId.HasValue && !btnEdit.Enabled) return;

            var mode = warehouseId.HasValue ? ucWarehouseEditor_Admin.EditorMode.Edit : ucWarehouseEditor_Admin.EditorMode.Add;

            try
            {
                var ucEditor = new ucWarehouseEditor_Admin();
                ucEditor.LoadData(mode, warehouseId); // Load data within the UC

                using (var popupForm = new Form
                {
                    StartPosition = FormStartPosition.CenterScreen, // Per requirement
                    FormBorderStyle = FormBorderStyle.None,       // Per requirement
                    Size = new Size(619, 485),                    // Per requirement
                })
                {
                    popupForm.Controls.Add(ucEditor);
                    ucEditor.Dock = DockStyle.Fill; // Fill the form

                    // Show as dialog and reload if OK
                    Form ownerForm = this.FindForm();
                    if (ownerForm == null) { MessageBox.Show("Cannot determine parent form.", "Error"); return; }

                    if (popupForm.ShowDialog(ownerForm) == DialogResult.OK)
                    {
                        LoadData(); // Reload grid if save was successful
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening editor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenSearchPopup()
        {
            int? selectedId = null; // To store the ID picked from search
            try
            {
                using (var searchForm = new Form
                {
                    StartPosition = FormStartPosition.CenterScreen, // Per requirement
                    FormBorderStyle = FormBorderStyle.None,       // Per requirement
                    Size = new Size(799, 781),                    // Per requirement
                })
                {
                    var ucSearch = new ucWarehouseSearch_Admin { Dock = DockStyle.Fill };

                    ucSearch.WarehousePicked += (sender, id) =>
                    {
                        selectedId = id;
                        searchForm.DialogResult = DialogResult.OK; // Signal success
                        searchForm.Close(); // Close the search popup
                    };

                    searchForm.Controls.Add(ucSearch);

                    Form ownerForm = this.FindForm();
                    if (ownerForm == null) { MessageBox.Show("Cannot determine parent form.", "Error"); return; }

                    if (searchForm.ShowDialog(ownerForm) == DialogResult.OK && selectedId.HasValue)
                    {
                        SelectRowById(selectedId.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToggleActiveStatus()
        {
            Warehouse selectedWarehouse = GetSelectedWarehouse();
            if (selectedWarehouse == null || !btnToggleActive.Enabled) return;

            bool currentStatus = selectedWarehouse.IsActive;
            string action = currentStatus ? "khóa" : "kích hoạt";
            string confirmMsg = $"Bạn có chắc muốn {action} kho '{selectedWarehouse.Name}'?";

            // Confirmation
            if (MessageBox.Show(confirmMsg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                if (currentStatus) // Only check if we are trying to DEACTIVATE (Lock)
                {
                    bool canDeactivate = _warehouseSvc.CheckIfWarehouseCanBeDeactivated(selectedWarehouse.Id); // Implement this in service
                    if (!canDeactivate)
                    {
                        MessageBox.Show("Không thể khóa kho này vì đang được sử dụng trong tuyến đường hoặc đơn hàng/chuyến hàng đang hoạt động.",
                                        "Không thể khóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                _warehouseSvc.ToggleActive(selectedWarehouse.Id); // Implement this in service

                int idToSelect = selectedWarehouse.Id;
                LoadData();
                SelectRowById(idToSelect);

                MessageBox.Show($"Đã {action} kho thành công.", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi {action} kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Sorting Logic

        private void DgvWarehouses_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;

            var newColumn = dgvWarehouses.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn)
            {
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                if (_sortedColumn != null)
                {
                    _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
                _sortOrder = SortOrder.Ascending;
                _sortedColumn = newColumn;
            }

            ApplySort();
            UpdateSortGlyphs();
        }

        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;

            string propertyName = _sortedColumn.DataPropertyName;
            PropertyInfo propInfo = typeof(Warehouse).GetProperty(propertyName); // Using Warehouse Model
            if (propInfo == null) return; // Should not happen if DataPropertyName is correct

            List<Warehouse> items = _bindingList.ToList();
            List<Warehouse> sortedList;

            try
            {
                if (_sortOrder == SortOrder.Ascending)
                {
                    sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList();
                }
                else
                {
                    sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();
                }

                _bindingList = new BindingList<Warehouse>(sortedList);
                dgvWarehouses.DataSource = _bindingList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sắp xếp: {ex.Message}", "Lỗi Sắp Xếp");
                ResetSortGlyphs(); // Reset on error
            }
        }

        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvWarehouses.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
                {
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
                if (_sortedColumn != null && col == _sortedColumn)
                {
                    col.HeaderCell.SortGlyphDirection = _sortOrder;
                }
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null)
            {
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            _sortedColumn = null;
            _sortOrder = SortOrder.None;
            UpdateSortGlyphs(); // Ensure all glyphs are visually reset
        }

        #endregion
    }
}