using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.DriverAdmin
{
    public partial class ucDriver_Admin : UserControl
    {
        private readonly DriverService _driverSvc = new DriverService();
        private BindingList<Driver> _bindingList;

        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        public ucDriver_Admin()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureGrid();
            WireEvents();
            LoadData();

            // đăng ký lắng nghe sự kiện reload khi gán/huỷ gán xe
            AppSession.VehicleAssignmentChanged += OnVehicleAssignmentChanged;
            this.HandleDestroyed += (s, _) =>
            {
                try { AppSession.VehicleAssignmentChanged -= OnVehicleAssignmentChanged; } catch { }
            };
        }

        private void OnVehicleAssignmentChanged()
        {
            if (!IsDisposed) LoadData();
        }

        // cấu hình lưới tài xế
        private void ConfigureGrid()
        {
            dgvDrivers.Columns.Clear();
            dgvDrivers.ApplyBaseStyle();

            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                Visible = false
            });
            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                DataPropertyName = "FullName",
                HeaderText = "Họ Tên",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 30,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Phone",
                DataPropertyName = "Phone",
                HeaderText = "Số Điện Thoại",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CitizenId",
                DataPropertyName = "CitizenId",
                HeaderText = "CCCD",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LicenseType",
                DataPropertyName = "LicenseType",
                HeaderText = "Hạng Bằng Lái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "VehicleInfo",
                DataPropertyName = "Vehicle",
                HeaderText = "Phương Tiện",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });

            dgvDrivers.CellFormatting += DgvDrivers_CellFormatting;
            dgvDrivers.ColumnHeaderMouseClick += dgvDrivers_ColumnHeaderMouseClick;
            dgvDrivers.SelectionChanged += (s, e) => UpdateButtonsState();
            dgvDrivers.CellDoubleClick += (s, ev) =>
            {
                if (ev.RowIndex >= 0) OpenDetailPopup(GetSelectedDriverId());
            };
        }

        // hiển thị thông tin xe gọn gàng
        private void DgvDrivers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvDrivers.Columns[e.ColumnIndex].Name == "VehicleInfo")
            {
                if (e.Value is Vehicle vehicle)
                {
                    e.Value = $"{vehicle.PlateNo} ({vehicle.Type})";
                    e.FormattingApplied = true;
                }
                else
                {
                    e.Value = "(Chưa Gán Xe)";
                    e.FormattingApplied = true;
                }
            }
        }

        // nạp dữ liệu tài xế
        private void LoadData()
        {
            try
            {
                var drivers = _driverSvc.GetDriversForAdmin(); // include vehicle nếu service đã thiết lập
                _bindingList = new BindingList<Driver>(drivers);
                dgvDrivers.DataSource = _bindingList;

                if (dgvDrivers.Rows.Count > 0)
                {
                    dgvDrivers.ClearSelection();
                    dgvDrivers.Rows[0].Selected = true;
                }

                ResetSortGlyphs();
                UpdateButtonsState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi Tải Dữ Liệu Tài Xế: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvDrivers.DataSource = null;
            }
        }

        // tiện ích lấy driver đang chọn
        private int? GetSelectedDriverId() => (dgvDrivers.CurrentRow?.DataBoundItem as Driver)?.Id;
        private Driver GetSelectedDriver() => dgvDrivers.CurrentRow?.DataBoundItem as Driver;

        // bật/tắt nút theo trạng thái chọn + gán xe
        private void UpdateButtonsState()
        {
            var d = GetSelectedDriver();
            bool has = d != null;

            btnEdit.Enabled = has;
            btnDelete.Enabled = has;
            btnViewDetail.Enabled = has;

            if (this.Controls.Find("btnAssignVehicle", true).FirstOrDefault() is Button btnAssignVehicle)
                btnAssignVehicle.Enabled = has && d.Vehicle == null;

            if (this.Controls.Find("btnUnassignVehicle", true).FirstOrDefault() is Button btnUnassignVehicle)
                btnUnassignVehicle.Enabled = has && d.Vehicle != null;
        }

        // chọn dòng theo id và cuộn đến vị trí
        private void SelectRowById(int driverId)
        {
            if (dgvDrivers.Rows.Count == 0 || _bindingList == null) return;

            var item = _bindingList.FirstOrDefault(d => d.Id == driverId);
            if (item == null) return;

            int rowIndex = _bindingList.IndexOf(item);
            if (rowIndex < 0 || rowIndex >= dgvDrivers.Rows.Count) return;

            dgvDrivers.ClearSelection();
            dgvDrivers.Rows[rowIndex].Selected = true;

            if (dgvDrivers.Columns.Contains("FullName"))
                dgvDrivers.CurrentCell = dgvDrivers.Rows[rowIndex].Cells["FullName"];

            if (!dgvDrivers.Rows[rowIndex].Displayed)
            {
                int firstDisplayed = Math.Max(0, rowIndex - dgvDrivers.DisplayedRowCount(false) / 2);
                dgvDrivers.FirstDisplayedScrollingRowIndex = Math.Min(firstDisplayed, dgvDrivers.RowCount - 1);
            }

            UpdateButtonsState();
        }

        // gắn sự kiện điều khiển chính
        private void WireEvents()
        {
            btnReload.Click += (s, e) => LoadData();
            btnAdd.Click += (s, e) => OpenEditorPopup(null);
            btnEdit.Click += (s, e) => OpenEditorPopup(GetSelectedDriverId());
            btnDelete.Click += (s, e) => DeleteDriver();
            btnViewDetail.Click += (s, e) => OpenDetailPopup(GetSelectedDriverId());
            btnSearch.Click += (s, e) => OpenSearchPopup();
        }

        // mở popup thêm/sửa
        private void OpenEditorPopup(int? driverId)
        {
            if (driverId == null && !btnAdd.Enabled) return;
            if (driverId.HasValue && !btnEdit.Enabled) return;

            var mode = driverId.HasValue ? ucDriverEditor_Admin.EditorMode.Edit : ucDriverEditor_Admin.EditorMode.Add;
            var title = (mode == ucDriverEditor_Admin.EditorMode.Add) ? "Thêm Tài Xế" : "Sửa Tài Xế";

            try
            {
                var ucEditor = new ucDriverEditor_Admin();
                ucEditor.LoadData(mode, driverId);

                using (var popupForm = new Form
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.None,
                    Width = 657,
                    Height = 689
                })
                {
                    popupForm.Controls.Add(ucEditor);
                    ucEditor.Dock = DockStyle.Fill;

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
                MessageBox.Show($"Lỗi Mở Form {title}: {ex.Message}", "Lỗi");
            }
        }

        // mở popup chi tiết
        private void OpenDetailPopup(int? driverId)
        {
            if (!driverId.HasValue)
            {
                MessageBox.Show("Vui Lòng Chọn Tài Xế.", "Thông Báo");
                return;
            }

            try
            {
                var ucDetail = new ucDriverDetail_Admin(driverId.Value);

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

        // xoá tài xế (kiểm tra ràng buộc chuyến)
        private void DeleteDriver()
        {
            var driverId = GetSelectedDriverId();
            if (!driverId.HasValue)
            {
                MessageBox.Show("Vui Lòng Chọn Tài Xế.", "Thông Báo");
                return;
            }

            try
            {
                if (_driverSvc.CheckDriverHasShipments(driverId.Value))
                {
                    MessageBox.Show(
                        "Không Thể Xóa Vì Tài Xế Đã Có Lịch Sử Chuyến Hàng.\nHãy Xem Xét Khóa Tài Khoản.",
                        "Không Thể Xóa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show(
                    "Tài Xế Này Chưa Có Chuyến Hàng.\nXóa Vĩnh Viễn Tài Xế Và Tài Khoản Liên Quan?",
                    "Xác Nhận Xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    _driverSvc.DeleteNewDriver(driverId.Value);
                    MessageBox.Show("Đã Xóa Tài Xế.", "Hoàn Tất");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi Khi Xóa: {ex.Message}", "Lỗi");
            }
        }

        // mở popup tìm kiếm tài xế
        private void OpenSearchPopup()
        {
            int? selectedId = null;

            using (var searchForm = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new System.Drawing.Size(1199, 689)
            })
            {
                var ucSearch = new ucDriverSearch_Admin { Dock = DockStyle.Fill };

                ucSearch.DriverPicked += (sender, id) =>
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

        // click tiêu đề cột để sắp xếp
        private void dgvDrivers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;

            var newColumn = dgvDrivers.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn)
            {
                _sortOrder = (_sortOrder == SortOrder.Ascending)
                    ? SortOrder.Descending
                    : SortOrder.Ascending;
            }
            else
            {
                if (_sortedColumn != null)
                    _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;

                _sortOrder = SortOrder.Ascending;
                _sortedColumn = newColumn;
            }

            if (newColumn.Name == "VehicleInfo")
                ApplySortNestedProperty("Vehicle.PlateNo");
            else
                ApplySort();

            UpdateSortGlyphs();
        }

        // sắp xếp theo thuộc tính đơn
        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;

            string propertyName = _sortedColumn.DataPropertyName;
            PropertyInfo propInfo = typeof(Driver).GetProperty(propertyName);
            if (propInfo == null) return;

            var items = _bindingList.ToList();
            var sorted = (_sortOrder == SortOrder.Ascending)
                ? items.OrderBy(x => propInfo.GetValue(x, null)).ToList()
                : items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

            _bindingList = new BindingList<Driver>(sorted);
            dgvDrivers.DataSource = _bindingList;
        }

        // sắp xếp theo thuộc tính lồng (vd Vehicle.PlateNo)
        private void ApplySortNestedProperty(string nestedPropertyName)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;

            Func<object, string, object> GetNested = (obj, name) =>
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
                ? items.OrderBy(x => GetNested(x, nestedPropertyName) == null)
                        .ThenBy(x => GetNested(x, nestedPropertyName))
                        .ToList()
                : items.OrderBy(x => GetNested(x, nestedPropertyName) == null)
                        .ThenByDescending(x => GetNested(x, nestedPropertyName))
                        .ToList();

            _bindingList = new BindingList<Driver>(sorted);
            dgvDrivers.DataSource = _bindingList;
        }

        // cập nhật mũi tên sắp xếp
        private void UpdateSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvDrivers.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable)
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;

                if (_sortedColumn != null && col == _sortedColumn)
                    col.HeaderCell.SortGlyphDirection = _sortOrder;
            }
        }

        // reset trạng thái sắp xếp
        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null && _sortedColumn.HeaderCell != null)
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;

            _sortedColumn = null;
            _sortOrder = SortOrder.None;
            UpdateSortGlyphs();
        }
    }
}
