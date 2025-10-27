using Guna.UI2.WinForms;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.GUI.OrderAdmin;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.VehicleAdmin
{
    public partial class ucVehicle_Admin : UserControl
    {
        private readonly VehicleService_Admin _vehicleSvc = new VehicleService_Admin();
        private BindingList<Vehicle> _bindingList;
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        public ucVehicle_Admin()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureGrid();
            WireEvents();
            LoadData();

            // Đăng ký nghe sự kiện thay đổi gán xe
            AppSession.VehicleAssignmentChanged += OnVehicleAssignmentChanged;

            // Hủy đăng ký khi control bị destroy (tránh cần override Dispose ở đây)
            this.HandleDestroyed += (s, _) =>
            {
                try { AppSession.VehicleAssignmentChanged -= OnVehicleAssignmentChanged; } catch { }
            };
        }

        private void OnVehicleAssignmentChanged()
        {
            if (!IsDisposed) LoadData();
        }

        // ========== GRID ==========
        private void ConfigureGrid()
        {
            dgvVehicles.Columns.Clear();
            dgvVehicles.ApplyBaseStyle();

            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "PlateNo", DataPropertyName = "PlateNo", HeaderText = "Biển số xe", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Type", DataPropertyName = "Type", HeaderText = "Loại xe", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 30, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "CapacityKg", DataPropertyName = "CapacityKg", HeaderText = "Trọng tải (kg)", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" }, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { Name = "DriverName", DataPropertyName = "Driver", HeaderText = "Tài xế", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });

            dgvVehicles.CellFormatting += DgvVehicles_CellFormatting;
            dgvVehicles.ColumnHeaderMouseClick += dgvVehicles_ColumnHeaderMouseClick;
            dgvVehicles.SelectionChanged += (s, e) => UpdateButtonsState();
            dgvVehicles.CellDoubleClick += (s, ev) => { if (ev.RowIndex >= 0) OpenEditorPopup(GetSelectedVehicleId()); };
        }

        private void DgvVehicles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvVehicles.Columns[e.ColumnIndex].Name == "Status" && e.Value is VehicleStatus st)
            {
                e.Value = st == VehicleStatus.Active ? "Hoạt động"
                         : st == VehicleStatus.Maintenance ? "Bảo trì"
                         : st == VehicleStatus.Inactive ? "Ngừng hoạt động"
                         : st.ToString();
                e.FormattingApplied = true;
            }
            else if (dgvVehicles.Columns[e.ColumnIndex].Name == "DriverName")
            {
                if (e.Value is Driver d) { e.Value = d.FullName; e.FormattingApplied = true; }
                else { e.Value = "(Chưa gán)"; e.FormattingApplied = true; }
            }
        }

        private void LoadData()
        {
            try
            {
                var vehicles = _vehicleSvc.GetVehiclesForAdmin();
                _bindingList = new BindingList<Vehicle>(vehicles);
                dgvVehicles.DataSource = _bindingList;

                if (dgvVehicles.Rows.Count > 0)
                {
                    dgvVehicles.ClearSelection();
                    dgvVehicles.Rows[0].Selected = true;
                }
                ResetSortGlyphs();
                UpdateButtonsState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu phương tiện: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvVehicles.DataSource = null;
            }
        }

        private int? GetSelectedVehicleId() => (dgvVehicles.CurrentRow?.DataBoundItem as Vehicle)?.Id;
        private Vehicle GetSelectedVehicle() => dgvVehicles.CurrentRow?.DataBoundItem as Vehicle;

        private void UpdateButtonsState()
        {
            var v = GetSelectedVehicle();
            bool has = v != null;

            btnEdit.Enabled = has;
            btnDelete.Enabled = has;
            btnAssignDriver.Enabled = has && v.Driver == null;
            btnUnassignDriver.Enabled = has && v.Driver != null;
        }

        private void SelectRowById(int id)
        {
            if (dgvVehicles.Rows.Count == 0 || _bindingList == null) return;
            var item = _bindingList.FirstOrDefault(x => x.Id == id);
            if (item == null) return;

            int rowIndex = _bindingList.IndexOf(item);
            if (rowIndex < 0) return;

            dgvVehicles.ClearSelection();
            dgvVehicles.Rows[rowIndex].Selected = true;
            if (dgvVehicles.Columns.Contains("PlateNo"))
                dgvVehicles.CurrentCell = dgvVehicles.Rows[rowIndex].Cells["PlateNo"];
        }

        private void WireEvents()
        {
            btnReload.Click += (s, e) => LoadData();
            btnAdd.Click += (s, e) => OpenEditorPopup(null);
            btnEdit.Click += (s, e) => OpenEditorPopup(GetSelectedVehicleId());
            btnDelete.Click += (s, e) => DeleteVehicle();
            btnSearch.Click += (s, e) => OpenSearchPopup();
            btnAssignDriver.Click += (s, e) => AssignDriver();
            btnUnassignDriver.Click += (s, e) => UnassignDriver();
        }

        private void OpenEditorPopup(int? vehicleId)
        {
            if (vehicleId == null && !btnAdd.Enabled) return;
            if (vehicleId.HasValue && !btnEdit.Enabled) return;

            var mode = vehicleId.HasValue ? ucVehicleEditor_Admin.EditorMode.Edit : ucVehicleEditor_Admin.EditorMode.Add;

            try
            {
                var ucEditor = new ucVehicleEditor_Admin();
                ucEditor.LoadData(mode, vehicleId);

                using (var popup = new Form
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.None,
                    Size = new System.Drawing.Size(916, 392)
                })
                {
                    popup.Controls.Add(ucEditor);
                    ucEditor.Dock = DockStyle.Fill;
                    var owner = this.FindForm();
                    if (owner == null) { MessageBox.Show("Không xác định form cha.", "Lỗi"); return; }

                    if (popup.ShowDialog(owner) == DialogResult.OK)
                    {
                        LoadData();
                        if (vehicleId.HasValue) SelectRowById(vehicleId.Value);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi mở trình chỉnh sửa: {ex.Message}", "Lỗi"); }
        }

        private void OpenSearchPopup()
        {
            int? selectedId = null;
            using (var f = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new System.Drawing.Size(988, 689)
            })
            {
                var ucSearch = new ucVehicleSearch_Admin { Dock = DockStyle.Fill };
                ucSearch.VehiclePicked += (s, id) => { selectedId = id; f.DialogResult = DialogResult.OK; f.Close(); };
                f.Controls.Add(ucSearch);

                var owner = this.FindForm();
                if (owner == null) { MessageBox.Show("Không xác định form cha.", "Lỗi"); return; }

                if (f.ShowDialog(owner) == DialogResult.OK && selectedId.HasValue)
                    SelectRowById(selectedId.Value);
            }
        }

        private void DeleteVehicle()
        {
            var id = GetSelectedVehicleId();
            if (!id.HasValue) { MessageBox.Show("Vui lòng chọn phương tiện.", "Thông báo"); return; }

            if (MessageBox.Show($"Xóa phương tiện ID {id.Value}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    _vehicleSvc.DeleteVehicle(id.Value);
                    MessageBox.Show("Đã xóa.", "Hoàn tất");
                    LoadData();
                }
                catch (InvalidOperationException opEx) { MessageBox.Show(opEx.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                catch (Exception ex) { MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi"); }
            }
        }

        private void AssignDriver()
        {
            var v = GetSelectedVehicle();
            if (v == null || !btnAssignDriver.Enabled)
            {
                MessageBox.Show("Chọn một xe chưa gán tài xế.", "Thông báo");
                return;
            }

            int? driverId = null;
            using (var f = new Form
            {
                Text = "Chọn Tài Xế (chỉ tài xế rảnh & chưa có xe)",
                StartPosition = FormStartPosition.CenterScreen,
                Size = new System.Drawing.Size(583, 539),
                FormBorderStyle = FormBorderStyle.None
            })
            {
                var picker = new ucDriverPicker_Admin();
                picker.DriverSelected += (id) => { driverId = id; f.DialogResult = DialogResult.OK; };
                f.Controls.Add(picker);
                var owner = this.FindForm();
                if (owner == null) { MessageBox.Show("Không xác định form cha.", "Lỗi"); return; }

                if (f.ShowDialog(owner) == DialogResult.OK && driverId.HasValue)
                {
                    if (MessageBox.Show($"Gán tài xế ID {driverId.Value} cho xe {v.PlateNo}?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            _vehicleSvc.AssignDriverToVehicle(v.Id, driverId.Value);
                            MessageBox.Show("Đã gán tài xế.", "Thông báo");
                            LoadData();
                            SelectRowById(v.Id);
                        }
                        catch (InvalidOperationException opEx) { MessageBox.Show(opEx.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                        catch (Exception ex) { MessageBox.Show($"Lỗi gán tài xế: {ex.Message}", "Lỗi"); }
                    }
                }
            }
        }

        private void UnassignDriver()
        {
            var v = GetSelectedVehicle();
            if (v == null || !btnUnassignDriver.Enabled)
            {
                MessageBox.Show("Chọn một xe đang gán tài xế.", "Thông báo");
                return;
            }

            if (MessageBox.Show($"Gỡ tài xế khỏi xe {v.PlateNo}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _vehicleSvc.UnassignDriverFromVehicle(v.Id);
                    MessageBox.Show("Đã gỡ tài xế.", "Thông báo");
                    LoadData();
                    SelectRowById(v.Id);
                }
                catch (Exception ex) { MessageBox.Show($"Lỗi gỡ tài xế: {ex.Message}", "Lỗi"); }
            }
        }

        // ===== Sorting =====
        private void dgvVehicles_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;
            var newColumn = dgvVehicles.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (newColumn.Name == "DriverName")
            {
                ApplySortNestedProperty("Driver.FullName");
                UpdateSortGlyphs();
                _sortedColumn = newColumn;
                return;
            }

            if (_sortedColumn == newColumn)
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            else
            {
                if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                _sortOrder = SortOrder.Ascending;
                _sortedColumn = newColumn;
            }

            ApplySort();
            UpdateSortGlyphs();
        }

        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;
            string prop = _sortedColumn.DataPropertyName;
            PropertyInfo pi = typeof(Vehicle).GetProperty(prop);
            if (pi == null) return;

            var items = _bindingList.ToList();
            var sorted = (_sortOrder == SortOrder.Ascending)
                ? items.OrderBy(x => pi.GetValue(x, null)).ToList()
                : items.OrderByDescending(x => pi.GetValue(x, null)).ToList();

            _bindingList = new BindingList<Vehicle>(sorted);
            dgvVehicles.DataSource = _bindingList;
        }

        private void ApplySortNestedProperty(string nested)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;

            Func<object, string, object> get = (obj, name) =>
            {
                if (obj == null) return null;
                object cur = obj;
                foreach (var part in name.Split('.'))
                {
                    if (cur == null) return null;
                    var p = cur.GetType().GetProperty(part);
                    if (p == null) return null;
                    cur = p.GetValue(cur, null);
                }
                return cur;
            };

            var items = _bindingList.ToList();
            var sorted = (_sortOrder == SortOrder.Ascending)
                ? items.OrderBy(x => get(x, nested) == null).ThenBy(x => get(x, nested)).ToList()
                : items.OrderBy(x => get(x, nested) == null).ThenByDescending(x => get(x, nested)).ToList();

            _bindingList = new BindingList<Vehicle>(sorted);
            dgvVehicles.DataSource = _bindingList;
        }

        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvVehicles.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
                if (_sortedColumn != null && col == _sortedColumn)
                    col.HeaderCell.SortGlyphDirection = _sortOrder;
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null && _sortedColumn.HeaderCell != null)
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            _sortedColumn = null; _sortOrder = SortOrder.None;
            UpdateSortGlyphs();
        }
    }
}
