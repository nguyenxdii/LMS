using Guna.UI2.WinForms;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;                 // Zone, WarehouseType, RouteTemplate, RouteTemplateStop
using LogisticsApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.RouteTemplateAdmin
{
    public partial class ucRouteTemplateEditor_Admin : UserControl
    {
        // --- Inner Classes for ComboBox Binding ---
        private class ZoneItem
        {
            public Zone Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }
        private class ComboItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }

        // --- Editor Mode ---
        public enum EditorMode { Add, Edit }

        // --- State & Dependencies ---
        private EditorMode _mode = EditorMode.Add;
        private int _templateId = 0;
        private readonly RouteTemplateService_Admin _templateSvc = new RouteTemplateService_Admin();
        private readonly WarehouseService_Admin _warehouseSvc = new WarehouseService_Admin();
        private RouteTemplate _originalTemplate;
        private List<RouteTemplateStop> _originalStops;

        // --- Data Sources for Grids ---
        private BindingList<WarehouseSelectionItem> _availableWarehouses = new BindingList<WarehouseSelectionItem>();
        private BindingList<RouteTemplateStopViewModel> _selectedStops = new BindingList<RouteTemplateStopViewModel>();

        // --- UI Helpers ---
        private ErrorProvider errProvider;

        private bool isDragging = false;
        private Point dragStartPoint = Point.Empty;
        private Point parentFormStartPoint = Point.Empty;

        private DataGridViewColumn _availableSortedColumn = null;
        private SortOrder _availableSortOrder = SortOrder.None;

        private class WarehouseSelectionItem
        {
            public int WarehouseId { get; set; }
            public string Name { get; set; }
            public string ZoneText { get; set; }
        }

        public class RouteTemplateStopViewModel
        {
            public int StopId { get; set; }
            public int WarehouseId { get; set; }
            public int Seq { get; set; }
            public string StopWHName { get; set; }
        }

        public ucRouteTemplateEditor_Admin()
        {
            InitializeComponent();
            this.errProvider = new ErrorProvider { ContainerControl = this };
            WireEvents();
            LoadZoneComboBoxes();
        }

        #region Data Loading & Mode Handling

        //public void LoadData(EditorMode mode, int? templateId)
        //{
        //    _mode = mode;
        //    errProvider.Clear();
        //    _selectedStops.Clear();
        //    _availableWarehouses.Clear();

        //    Font titleFont = new Font("Segoe UI", 14F, FontStyle.Bold);
        //    if (lblTitle != null)
        //    {
        //        lblTitle.Font = titleFont;
        //        lblTitle.Text = (_mode == EditorMode.Add) ? "Thêm Tuyến đường Mới" : "Sửa Tuyến đường";
        //    }

        //    try
        //    {
        //        if (_mode == EditorMode.Edit)
        //        {
        //            if (!templateId.HasValue)
        //                throw new ArgumentNullException(nameof(templateId), "ID Tuyến đường là bắt buộc khi sửa.");

        //            _templateId = templateId.Value;

        //            _originalTemplate = _templateSvc.GetTemplateWithStops(_templateId);
        //            if (_originalTemplate == null)
        //                throw new Exception($"Không tìm thấy tuyến đường ID {_templateId}.");

        //            _originalStops = (_originalTemplate.Stops ?? new List<RouteTemplateStop>())
        //                                .OrderBy(s => s.Seq)
        //                                .ToList();

        //            // Bind header
        //            txtTemplateName.Text = _originalTemplate.Name;
        //            SelectZoneAndWarehouse(cmbFromZone, cmbToWarehouse, _originalTemplate.FromWarehouseId);
        //            SelectZoneAndWarehouse(cmbToZone, cmbFromWarehouse, _originalTemplate.ToWarehouseId);

        //            ConfigureSelectedStopsGrid();

        //            _selectedStops.Clear();
        //            foreach (var stop in _originalStops)
        //            {
        //                _selectedStops.Add(new RouteTemplateStopViewModel
        //                {
        //                    StopId = stop.Id,
        //                    WarehouseId = stop.WarehouseId ?? 0,
        //                    Seq = stop.Seq,
        //                    StopWHName = stop.Warehouse?.Name ?? stop.StopName ?? $"ID {stop.WarehouseId}"
        //                });
        //            }

        //            // Rebind + chuẩn hóa thứ tự
        //            dgvSelectedStops.DataSource = null;
        //            dgvSelectedStops.DataSource = _selectedStops;
        //            RecalculateSelectedStopSeq();
        //            dgvSelectedStops.Refresh();
        //        }
        //        else
        //        {
        //            // Add mode
        //            _templateId = 0;
        //            _originalTemplate = new RouteTemplate();
        //            _originalStops = new List<RouteTemplateStop>();
        //            _selectedStops.Clear();

        //            txtTemplateName.Clear();
        //            cmbFromZone.SelectedIndex = -1; cmbToWarehouse.DataSource = null; cmbToWarehouse.Items.Clear();
        //            cmbToZone.SelectedIndex = -1; cmbFromWarehouse.DataSource = null; cmbFromWarehouse.Items.Clear();
        //            cmbFromWarehouse.SelectedIndex = -1; cmbToWarehouse.DataSource = null; cmbToWarehouse.Items.Clear();

        //            ConfigureSelectedStopsGrid();
        //        }

        //        // Grids
        //        ConfigureAvailableStopsGrid();
        //        LoadAvailableWarehouses();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi tải dữ liệu tuyến đường: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //    UpdateButtonStates();
        //}
        public void LoadData(EditorMode mode, int? templateId)
        {
            _mode = mode;
            errProvider.Clear();
            _selectedStops.Clear();
            _availableWarehouses.Clear();

            Font titleFont = new Font("Segoe UI", 14F, FontStyle.Bold);
            if (lblTitle != null)
            {
                lblTitle.Font = titleFont;
                lblTitle.Text = (_mode == EditorMode.Add) ? "Thêm Tuyến đường Mới" : "Sửa Tuyến đường";
            }

            try
            {
                if (_mode == EditorMode.Edit) // --- CHẾ ĐỘ SỬA ---
                {
                    if (!templateId.HasValue)
                        throw new ArgumentNullException(nameof(templateId), "ID Tuyến đường là bắt buộc khi sửa.");

                    _templateId = templateId.Value;

                    _originalTemplate = _templateSvc.GetTemplateWithStops(_templateId);
                    if (_originalTemplate == null)
                        throw new Exception($"Không tìm thấy tuyến đường ID {_templateId}.");

                    _originalStops = (_originalTemplate.Stops ?? new List<RouteTemplateStop>())
                                        .OrderBy(s => s.Seq)
                                        .ToList();

                    txtTemplateName.Text = _originalTemplate.Name;

                    SelectZoneAndWarehouse(cmbFromZone, cmbFromWarehouse, _originalTemplate.FromWarehouseId); // Cặp Vùng Đi / Kho Đi
                    SelectZoneAndWarehouse(cmbToZone, cmbToWarehouse, _originalTemplate.ToWarehouseId);     // Cặp Vùng Đến / Kho Đến
                                                                                                            // *** KẾT THÚC SỬA ***

                    ConfigureSelectedStopsGrid();

                    _selectedStops.Clear();
                    foreach (var stop in _originalStops)
                    {
                        _selectedStops.Add(new RouteTemplateStopViewModel
                        {
                            StopId = stop.Id,
                            WarehouseId = stop.WarehouseId ?? 0,
                            Seq = stop.Seq,
                            StopWHName = stop.Warehouse?.Name ?? stop.StopName ?? $"ID {stop.WarehouseId}"
                        });
                    }

                    dgvSelectedStops.DataSource = null;
                    dgvSelectedStops.DataSource = _selectedStops;
                    RecalculateSelectedStopSeq();
                    dgvSelectedStops.Refresh(); // Đảm bảo grid cập nhật
                }
                else // --- CHẾ ĐỘ THÊM ---
                {
                    _templateId = 0;
                    _originalTemplate = new RouteTemplate();
                    _originalStops = new List<RouteTemplateStop>();
                    _selectedStops.Clear();

                    txtTemplateName.Clear();

                    cmbFromZone.SelectedIndex = -1; cmbFromWarehouse.DataSource = null; cmbFromWarehouse.Items.Clear(); // Reset Kho Đi
                    cmbToZone.SelectedIndex = -1; cmbToWarehouse.DataSource = null; cmbToWarehouse.Items.Clear();   // Reset Kho Đến
                                                                                                                    // Reset ComboBox lọc chặng dừng (giữ nguyên)
                    cmbStopZone.SelectedIndex = -1; cmbStopWarehouse.DataSource = null; cmbStopWarehouse.Items.Clear();
                    ConfigureSelectedStopsGrid();
                }

                ConfigureAvailableStopsGrid();
                LoadAvailableWarehouses(); // Load danh sách kho có thể chọn ban đầu
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu tuyến đường: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateButtonStates();
        }

        private void LoadZoneComboBoxes()
        {
            var vnZones = Enum.GetValues(typeof(Zone))
                              .Cast<Zone>()
                              .Select(z => new ZoneItem { Value = z, Text = FormatZone(z) })
                              .ToList();

            cmbFromZone.DataSource = vnZones.ToList();
            cmbToZone.DataSource = vnZones.ToList();
            cmbFromWarehouse.DataSource = vnZones.ToList();

            cmbFromZone.DisplayMember = cmbToZone.DisplayMember = cmbFromWarehouse.DisplayMember = "Text";
            cmbFromZone.ValueMember = cmbToZone.ValueMember = cmbFromWarehouse.ValueMember = "Value";

            cmbFromZone.SelectedIndex = cmbToZone.SelectedIndex = cmbFromWarehouse.SelectedIndex = -1;
        }

        private void LoadWarehouseComboBox(Guna2ComboBox zoneCombo, Guna2ComboBox warehouseCombo)
        {
            int previouslySelectedId = (warehouseCombo.SelectedValue is int v) ? v : 0;
            warehouseCombo.DataSource = null;
            warehouseCombo.Items.Clear();

            if (zoneCombo.SelectedItem is ZoneItem selectedZone)
            {
                try
                {
                    var warehouses = _warehouseSvc.GetActiveWarehousesByZone(selectedZone.Value);
                    var comboItems = warehouses
                                      .Select(w => new ComboItem { Value = w.Id, Text = w.Name })
                                      .ToList();
                    warehouseCombo.DataSource = comboItems;
                    warehouseCombo.DisplayMember = "Text";
                    warehouseCombo.ValueMember = "Value";

                    if (previouslySelectedId != 0 && comboItems.Any(ci => ci.Value == previouslySelectedId))
                        warehouseCombo.SelectedValue = previouslySelectedId;
                    else
                        warehouseCombo.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi tải danh sách kho cho vùng {selectedZone.Text}: {ex.Message}", "Lỗi");
                }
            }
        }

        //private void SelectZoneAndWarehouse(Guna2ComboBox zoneCombo, Guna2ComboBox warehouseCombo, int warehouseId)
        //{
        //    zoneCombo.SelectedIndexChanged -= ZoneCombo_SelectedIndexChanged_LoadWarehouse;
        //    warehouseCombo.SelectedIndexChanged -= WarehouseCombo_SelectedIndexChanged_LoadAvailable;

        //    try
        //    {
        //        var warehouse = _warehouseSvc.GetWarehouseForEdit(warehouseId);
        //        if (warehouse != null)
        //        {
        //            zoneCombo.SelectedValue = warehouse.ZoneId;
        //            LoadWarehouseComboBox(zoneCombo, warehouseCombo);
        //            warehouseCombo.SelectedValue = warehouseId;
        //        }
        //        else
        //        {
        //            zoneCombo.SelectedIndex = -1;
        //            warehouseCombo.DataSource = null; warehouseCombo.Items.Clear();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi khi chọn kho ID {warehouseId}: {ex.Message}", "Lỗi");
        //        zoneCombo.SelectedIndex = -1;
        //        warehouseCombo.DataSource = null; warehouseCombo.Items.Clear();
        //    }
        //    finally
        //    {
        //        zoneCombo.SelectedIndexChanged += ZoneCombo_SelectedIndexChanged_LoadWarehouse;
        //        warehouseCombo.SelectedIndexChanged += WarehouseCombo_SelectedIndexChanged_LoadAvailable;
        //    }
        //}

        private void SelectZoneAndWarehouse(Guna2ComboBox zoneCombo, Guna2ComboBox warehouseCombo, int warehouseId)
        {
            // Tạm ngắt sự kiện (GIỮ NGUYÊN)
            zoneCombo.SelectedIndexChanged -= ZoneCombo_SelectedIndexChanged_LoadWarehouse;
            warehouseCombo.SelectedIndexChanged -= WarehouseCombo_SelectedIndexChanged_LoadAvailable;

            try
            {
                var warehouse = _warehouseSvc.GetWarehouseForEdit(warehouseId);
                if (warehouse != null)
                {
                    zoneCombo.SelectedValue = warehouse.ZoneId;
                    // Gọi LoadWarehouseComboBox với cặp đúng
                    LoadWarehouseComboBox(zoneCombo, warehouseCombo); // Hàm này không cần sửa
                    warehouseCombo.SelectedValue = warehouseId;
                }
                else
                {
                    zoneCombo.SelectedIndex = -1;
                    warehouseCombo.DataSource = null; warehouseCombo.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn kho ID {warehouseId}: {ex.Message}", "Lỗi");
                zoneCombo.SelectedIndex = -1;
                warehouseCombo.DataSource = null; warehouseCombo.Items.Clear();
            }
            finally
            {
                // Gắn lại sự kiện (GIỮ NGUYÊN)
                zoneCombo.SelectedIndexChanged += ZoneCombo_SelectedIndexChanged_LoadWarehouse;
                warehouseCombo.SelectedIndexChanged += WarehouseCombo_SelectedIndexChanged_LoadAvailable;
            }
        }
        private void LoadAvailableWarehouses()
        {
            _availableWarehouses.Clear();

            int fromWhId = (cmbToWarehouse.SelectedValue is int v1) ? v1 : 0;
            int toWhId = (cmbFromWarehouse.SelectedValue is int v2) ? v2 : 0;
            Zone? filterZone = (cmbFromWarehouse.SelectedValue is Zone z) ? z : (Zone?)null;

            try
            {
                var allActiveWarehouses = _warehouseSvc.SearchWarehouses(null, filterZone, true);

                foreach (var wh in allActiveWarehouses)
                {
                    if (wh.Id != fromWhId && wh.Id != toWhId && !_selectedStops.Any(s => s.WarehouseId == wh.Id))
                    {
                        _availableWarehouses.Add(new WarehouseSelectionItem
                        {
                            WarehouseId = wh.Id,
                            Name = wh.Name,
                            ZoneText = FormatZone(wh.ZoneId)
                        });
                    }
                }

                dgvAvailableStops.DataSource = _availableWarehouses;

                if (_availableSortedColumn != null)
                {
                    ApplyAvailableSort();
                    UpdateAvailableSortGlyphs();
                }
                else
                {
                    dgvAvailableStops.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách kho khả dụng: {ex.Message}", "Lỗi");
                dgvAvailableStops.DataSource = null;
            }
        }

        private void RecalculateSelectedStopSeq()
        {
            int currentSeq = 1;
            var tempList = _selectedStops.ToList();
            foreach (var stop in tempList) stop.Seq = currentSeq++;
            _selectedStops = new BindingList<RouteTemplateStopViewModel>(tempList);
            dgvSelectedStops.DataSource = _selectedStops; // Rebind
        }

        #endregion

        #region Grid Configuration

        private void ConfigureAvailableStopsGrid()
        {
            var g = dgvAvailableStops;
            g.Columns.Clear();
            g.ApplyBaseStyle();
            g.AutoGenerateColumns = false;
            g.AllowUserToAddRows = false;
            g.ReadOnly = true;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;

            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "colWhId", DataPropertyName = "WarehouseId", Visible = false });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colWhName",
                DataPropertyName = "Name",
                HeaderText = "Kho Khả Dụng",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colWhZone",
                DataPropertyName = "ZoneText",
                HeaderText = "Vùng",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            g.DataSource = _availableWarehouses;

            g.ColumnHeaderMouseClick += DgvAvailableStops_ColumnHeaderMouseClick;
            g.CellDoubleClick += DgvAvailableStops_CellDoubleClick; // Double-click để add chặng
        }

        private void ConfigureSelectedStopsGrid()
        {
            var g = dgvSelectedStops;
            g.Columns.Clear();
            g.ApplyBaseStyle();
            g.AutoGenerateColumns = false;
            g.AllowUserToAddRows = false;
            g.ReadOnly = true;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;

            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStopId", DataPropertyName = "StopId", Visible = false });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSelWhId", DataPropertyName = "WarehouseId", Visible = false });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSeq",
                DataPropertyName = "Seq",
                HeaderText = "#",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStopName",
                DataPropertyName = "StopWHName",
                HeaderText = "Kho Dừng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            g.DataSource = _selectedStops;

            // Double click để remove chặng
            g.CellDoubleClick += DgvSelectedStops_CellDoubleClick;
        }

        #endregion

        #region Event Wiring & Handlers

        private void WireEvents()
        {
            btnSave.Click += BtnSave_Click;

            cmbFromZone.SelectedIndexChanged += ZoneCombo_SelectedIndexChanged_LoadWarehouse;
            cmbToZone.SelectedIndexChanged += ZoneCombo_SelectedIndexChanged_LoadWarehouse;
            cmbFromWarehouse.SelectedIndexChanged += ZoneCombo_SelectedIndexChanged_LoadWarehouse;

            cmbToWarehouse.SelectedIndexChanged += WarehouseCombo_SelectedIndexChanged_LoadAvailable;
            cmbFromWarehouse.SelectedIndexChanged += WarehouseCombo_SelectedIndexChanged_LoadAvailable;

            btnMoveUp.Click += BtnMoveUp_Click;
            btnMoveDown.Click += BtnMoveDown_Click;

            dgvSelectedStops.SelectionChanged += (s, e) => UpdateButtonStates();

            Control dragHandle = pnlTop;
            if (dragHandle != null)
            {
                dragHandle.MouseDown += DragHandle_MouseDown;
                dragHandle.MouseMove += DragHandle_MouseMove;
                dragHandle.MouseUp += DragHandle_MouseUp;
            }
        }

        //private void ZoneCombo_SelectedIndexChanged_LoadWarehouse(object sender, EventArgs e)
        //{
        //    var zoneCombo = sender as Guna2ComboBox;
        //    if (zoneCombo == cmbFromZone) LoadWarehouseComboBox(cmbFromZone, cmbToWarehouse);
        //    else if (zoneCombo == cmbToZone) LoadWarehouseComboBox(cmbToZone, cmbFromWarehouse);
        //    else if (zoneCombo == cmbFromWarehouse)
        //    {
        //        LoadWarehouseComboBox(cmbFromWarehouse, cmbToWarehouse);
        //        LoadAvailableWarehouses();
        //    }
        //}
        private void ZoneCombo_SelectedIndexChanged_LoadWarehouse(object sender, EventArgs e)
        {
            var zoneCombo = sender as Guna2ComboBox;
            // *** SỬA CÁC DÒNG KIỂM TRA VÀ GỌI HÀM ***
            if (zoneCombo == cmbFromZone)
                LoadWarehouseComboBox(cmbFromZone, cmbFromWarehouse); // Sửa thành cmbFromWarehouse
            else if (zoneCombo == cmbToZone)
                LoadWarehouseComboBox(cmbToZone, cmbToWarehouse);   // Sửa thành cmbToWarehouse
            else if (zoneCombo == cmbStopZone)
            {
                // Khi chọn vùng lọc chặng, chỉ cần load lại danh sách kho khả dụng
                // Không cần load vào cmbStopWarehouse vì nó chỉ là filter
                // LoadWarehouseComboBox(cmbStopZone, cmbStopWarehouse); // <--- BỎ DÒNG NÀY
                LoadAvailableWarehouses(); // Giữ lại dòng này
            }
            // *** KẾT THÚC SỬA ***
        }

        private void WarehouseCombo_SelectedIndexChanged_LoadAvailable(object sender, EventArgs e)
        {
            if (sender is Guna2ComboBox cmb && cmb.SelectedIndex >= 0)
            {
                LoadAvailableWarehouses();
            }
        }

        private void DgvAvailableStops_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _availableWarehouses.Count) return;

            var itemToAdd = _availableWarehouses[e.RowIndex];
            if (itemToAdd == null) return;

            // 1) Add -> Selected
            _selectedStops.Add(new RouteTemplateStopViewModel
            {
                StopId = 0,
                WarehouseId = itemToAdd.WarehouseId,
                StopWHName = itemToAdd.Name
            });

            // 2) Remove khỏi available
            _availableWarehouses.RemoveAt(e.RowIndex);

            // 3) Recalc Seq & Update buttons
            RecalculateSelectedStopSeq();
            UpdateButtonStates();

            // 4) Reapply sort cho available nếu có
            if (_availableSortedColumn != null)
            {
                ApplyAvailableSort();
                UpdateAvailableSortGlyphs();
            }
            else
            {
                dgvAvailableStops.Refresh();
            }
        }

        private void DgvSelectedStops_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _selectedStops.Count) return;

            if (dgvSelectedStops.Rows[e.RowIndex].DataBoundItem is RouteTemplateStopViewModel itemToRemove)
            {
                int fromWhId = (cmbToWarehouse.SelectedValue is int v1) ? v1 : 0;
                int toWhId = (cmbFromWarehouse.SelectedValue is int v2) ? v2 : 0;
                Zone? filterZone = (cmbFromWarehouse.SelectedValue is Zone z) ? z : (Zone?)null;

                var removedWh = _warehouseSvc.GetWarehouseForEdit(itemToRemove.WarehouseId);

                if (removedWh != null && removedWh.Id != fromWhId && removedWh.Id != toWhId &&
                   (!filterZone.HasValue || removedWh.ZoneId == filterZone.Value))
                {
                    _availableWarehouses.Add(new WarehouseSelectionItem
                    {
                        WarehouseId = removedWh.Id,
                        Name = removedWh.Name,
                        ZoneText = FormatZone(removedWh.ZoneId)
                    });

                    if (_availableSortedColumn != null)
                    {
                        ApplyAvailableSort();
                        UpdateAvailableSortGlyphs();
                    }
                    else
                    {
                        dgvAvailableStops.Refresh();
                    }
                }

                _selectedStops.Remove(itemToRemove);
                RecalculateSelectedStopSeq();
            }
            UpdateButtonStates();
        }

        private void BtnMoveUp_Click(object sender, EventArgs e)
        {
            if (dgvSelectedStops.CurrentRow == null || dgvSelectedStops.CurrentRow.Index == 0) return;
            int currentIndex = dgvSelectedStops.CurrentRow.Index;
            if (_selectedStops[currentIndex] is RouteTemplateStopViewModel itemToMove)
            {
                _selectedStops.RemoveAt(currentIndex);
                _selectedStops.Insert(currentIndex - 1, itemToMove);
                RecalculateSelectedStopSeq();
                dgvSelectedStops.ClearSelection();
                if (currentIndex - 1 >= 0)
                {
                    dgvSelectedStops.Rows[currentIndex - 1].Selected = true;
                    dgvSelectedStops.CurrentCell = dgvSelectedStops.Rows[currentIndex - 1].Cells["colStopName"];
                }
            }
            UpdateButtonStates();
        }

        private void BtnMoveDown_Click(object sender, EventArgs e)
        {
            if (dgvSelectedStops.CurrentRow == null || dgvSelectedStops.CurrentRow.Index >= _selectedStops.Count - 1) return;
            int currentIndex = dgvSelectedStops.CurrentRow.Index;
            if (_selectedStops[currentIndex] is RouteTemplateStopViewModel itemToMove)
            {
                _selectedStops.RemoveAt(currentIndex);
                _selectedStops.Insert(currentIndex + 1, itemToMove);
                RecalculateSelectedStopSeq();
                dgvSelectedStops.ClearSelection();
                if (currentIndex + 1 < dgvSelectedStops.Rows.Count)
                {
                    dgvSelectedStops.Rows[currentIndex + 1].Selected = true;
                    dgvSelectedStops.CurrentCell = dgvSelectedStops.Rows[currentIndex + 1].Cells["colStopName"];
                }
            }
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = dgvSelectedStops.CurrentRow != null;
            int currentIndex = hasSelection ? dgvSelectedStops.CurrentRow.Index : -1;
            btnMoveUp.Enabled = hasSelection && currentIndex > 0;
            btnMoveDown.Enabled = hasSelection && currentIndex < _selectedStops.Count - 1;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                MessageBox.Show("Vui lòng kiểm tra lại thông tin.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmMsg = (_mode == EditorMode.Add) ? "Tạo tuyến đường?" : "Lưu thay đổi?";
            if (MessageBox.Show(confirmMsg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            RouteTemplate template = (_mode == EditorMode.Edit && _originalTemplate != null) ? _originalTemplate : new RouteTemplate();
            template.Name = txtTemplateName.Text.Trim();
            template.FromWarehouseId = (int)cmbToWarehouse.SelectedValue;
            template.ToWarehouseId = (int)cmbFromWarehouse.SelectedValue;

            var stopWarehouseIds = _selectedStops
                .OrderBy(s => s.Seq)
                .Select(s => s.WarehouseId)
                .ToList();

            try
            {
                if (_mode == EditorMode.Add)
                    _templateSvc.CreateTemplateWithStops(template, stopWarehouseIds);
                else
                    _templateSvc.UpdateTemplateWithStops(template, stopWarehouseIds);

                MessageBox.Show("Lưu thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.FindForm()?.CloseDialog(DialogResult.OK);
            }
            catch (InvalidOperationException opEx)
            {
                MessageBox.Show(opEx.Message, "Lỗi Nghiệp Vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}\n{ex.InnerException?.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (HasChanges())
            {
                if (MessageBox.Show("Hủy thay đổi chưa lưu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }
            this.FindForm()?.CloseDialog(DialogResult.Cancel);
        }

        #endregion

        #region Validation & Change Tracking

        private bool ValidateInput()
        {
            errProvider.Clear(); bool isValid = true;
            if (string.IsNullOrWhiteSpace(txtTemplateName.Text)) { errProvider.SetError(txtTemplateName, "Tên trống."); isValid = false; }
            if (cmbToWarehouse.SelectedValue == null || (int)cmbToWarehouse.SelectedValue <= 0) { errProvider.SetError(cmbToWarehouse, "Chọn kho đi."); isValid = false; }
            if (cmbFromWarehouse.SelectedValue == null || (int)cmbFromWarehouse.SelectedValue <= 0) { errProvider.SetError(cmbFromWarehouse, "Chọn kho đến."); isValid = false; }
            if (isValid && (int)cmbToWarehouse.SelectedValue == (int)cmbFromWarehouse.SelectedValue) { errProvider.SetError(cmbFromWarehouse, "Kho đến phải khác kho đi."); isValid = false; }

            if (isValid)
            {
                var fromZoneItem = cmbFromZone.SelectedItem as ZoneItem;
                var toZoneItem = cmbToZone.SelectedItem as ZoneItem;
                if (fromZoneItem != null && toZoneItem != null && fromZoneItem.Value != toZoneItem.Value && _selectedStops.Count == 0)
                {
                    errProvider.SetError(dgvSelectedStops, "Tuyến liên vùng phải có ít nhất 1 chặng dừng.");
                    isValid = false;
                }
            }
            return isValid;
        }

        private bool HasChanges()
        {
            if (_mode == EditorMode.Add)
            {
                return !string.IsNullOrWhiteSpace(txtTemplateName.Text)
                    || cmbToWarehouse.SelectedIndex >= 0
                    || cmbFromWarehouse.SelectedIndex >= 0
                    || _selectedStops.Any();
            }
            else
            {
                if (_originalTemplate == null || _originalStops == null) return false;

                int curFrom = (cmbToWarehouse.SelectedValue is int f) ? f : 0;
                int curTo = (cmbFromWarehouse.SelectedValue is int t) ? t : 0;

                bool infoChanged =
                    _originalTemplate.Name != txtTemplateName.Text.Trim()
                    || _originalTemplate.FromWarehouseId != curFrom
                    || _originalTemplate.ToWarehouseId != curTo;

                if (infoChanged) return true;

                var currentIds = _selectedStops.OrderBy(s => s.Seq).Select(s => s.WarehouseId).ToList();
                var originalIds = _originalStops.OrderBy(s => s.Seq).Select(s => s.WarehouseId ?? 0).ToList();

                if (currentIds.Count != originalIds.Count) return true;
                return !currentIds.SequenceEqual(originalIds);
            }
        }

        #endregion

        #region Form Dragging Logic

        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    isDragging = true;
                    dragStartPoint = Cursor.Position;
                    parentFormStartPoint = parentForm.Location;
                }
            }
        }
        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    Point diff = Point.Subtract(Cursor.Position, new Size(dragStartPoint));
                    parentForm.Location = Point.Add(parentFormStartPoint, new Size(diff));
                }
            }
        }
        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
                dragStartPoint = Point.Empty;
                parentFormStartPoint = Point.Empty;
            }
        }

        #endregion

        #region Available Grid Sorting Logic

        private void DgvAvailableStops_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_availableWarehouses == null || _availableWarehouses.Count == 0) return;
            var newColumn = dgvAvailableStops.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_availableSortedColumn == newColumn)
            {
                _availableSortOrder = (_availableSortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                if (_availableSortedColumn != null) _availableSortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                _availableSortOrder = SortOrder.Ascending;
                _availableSortedColumn = newColumn;
            }
            ApplyAvailableSort();
            UpdateAvailableSortGlyphs();
        }

        private void ApplyAvailableSort()
        {
            if (_availableSortedColumn == null || _availableWarehouses == null || _availableWarehouses.Count == 0) return;
            string propertyName = _availableSortedColumn.DataPropertyName;
            PropertyInfo propInfo = typeof(WarehouseSelectionItem).GetProperty(propertyName);
            if (propInfo == null) return;

            List<WarehouseSelectionItem> items = _availableWarehouses.ToList();
            List<WarehouseSelectionItem> sortedList;
            try
            {
                if (_availableSortOrder == SortOrder.Ascending) sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList();
                else sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();
                _availableWarehouses = new BindingList<WarehouseSelectionItem>(sortedList);
                dgvAvailableStops.DataSource = _availableWarehouses;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi sắp xếp kho khả dụng: {ex.Message}");
                ResetAvailableSortGlyphs();
            }
        }

        private void UpdateAvailableSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvAvailableStops.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None;
                if (_availableSortedColumn != null && col == _availableSortedColumn) col.HeaderCell.SortGlyphDirection = _availableSortOrder;
            }
        }

        private void ResetAvailableSortGlyphs()
        {
            if (_availableSortedColumn != null) _availableSortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            _availableSortedColumn = null; _availableSortOrder = SortOrder.None;
            UpdateAvailableSortGlyphs();
        }

        #endregion

        // --- Enum Formatting Helpers ---
        private string FormatZone(Zone zone)
        {
            switch (zone)
            {
                case Zone.North: return "Bắc";
                case Zone.Central: return "Trung";
                case Zone.South: return "Nam";
                default: return zone.ToString();
            }
        }
        private string FormatWarehouseType(WarehouseType type)
        {
            switch (type)
            {
                case WarehouseType.Main: return "Kho chính";
                case WarehouseType.Hub: return "Trung chuyển";
                case WarehouseType.Local: return "Địa phương";
                default: return type.ToString();
            }
        }
    }

    // --- Extension Method for Closing Dialogs ---
    public static class FormExtensions
    {
        public static void CloseDialog(this Form form, DialogResult result)
        {
            if (form != null)
            {
                form.DialogResult = result;
                form.Close();
            }
        }
    }
}
