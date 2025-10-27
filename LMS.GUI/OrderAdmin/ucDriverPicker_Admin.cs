//// LMS.GUI/OrderAdmin/ucDriverPicker_Admin.cs (Hoặc namespace đúng)
//using LMS.DAL;
//using LMS.DAL.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;
//using LMS.BUS.Helpers;
//using LMS.BUS.Services; // Đảm bảo có using DriverService

//namespace LMS.GUI.OrderAdmin // Hoặc namespace đúng
//{
//    public partial class ucDriverPicker_Admin : UserControl
//    {
//        // ... (Event, biến sort, biến kéo thả giữ nguyên) ...
//        public event Action<int> DriverSelected;
//        private DataGridViewColumn _sortedColumn = null;
//        private SortOrder _sortOrder = SortOrder.None;

//        private readonly DriverService _driverSvc = new DriverService();


//        public ucDriverPicker_Admin()
//        {
//            InitializeComponent();
//            this.Load += UcDriverPicker_Admin_Load;
//        }

//        private void UcDriverPicker_Admin_Load(object sender, EventArgs e)
//        {
//            // Đặt tiêu đề cho Label trên UC (Ví dụ: nếu bạn có lblTitle)
//            var titleLabel = this.Controls.Find("lblTitle", true).FirstOrDefault() as Label;
//            if (titleLabel != null)
//            {
//                titleLabel.Text = "Chọn Tài xế (Chỉ hiển thị tài xế rảnh)"; // Cập nhật tiêu đề
//            }
//            ConfigureGrid();
//            LoadDrivers(); // *** Sẽ gọi hàm LoadDrivers đã sửa ***
//            WireEvents();
//        }

//        // ... (ConfigureGrid giữ nguyên) ...
//        private void ConfigureGrid()
//        {
//            var g = dgvDrivers;
//            g.Columns.Clear();
//            g.ApplyBaseStyle();
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "FullName", DataPropertyName = "FullName", HeaderText = "Họ tên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", DataPropertyName = "Phone", HeaderText = "Điện thoại", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "LicenseType", DataPropertyName = "LicenseType", HeaderText = "Hạng GPLX", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            // g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CitizenId", DataPropertyName = "CitizenId", HeaderText = "CCCD", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//        }


//        // *** HÀM NÀY ĐÃ ĐƯỢC SỬA Ở LẦN TRƯỚC - Đảm bảo nó đúng như sau ***
//        /// <summary>
//        /// Tải danh sách tài xế ĐANG RẢNH vào grid.
//        /// </summary>
//        private void LoadDrivers()
//        {
//            try
//            {
//                // *** Gọi hàm GetAvailableDriversForAdmin() ***
//                var availableDrivers = _driverSvc.GetAvailableDriversForAdmin();

//                // Project sang kiểu dữ liệu Anonymous Type để chỉ hiển thị các cột cần thiết
//                var displayData = availableDrivers.Select(d => new
//                {
//                    d.Id,
//                    d.FullName,
//                    d.Phone,
//                    d.LicenseType
//                    // d.CitizenId
//                }).ToList();

//                dgvDrivers.DataSource = null;
//                dgvDrivers.DataSource = displayData; // Gán nguồn mới chỉ chứa tài xế rảnh
//                ResetSortGlyphs();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tải danh sách tài xế rảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                dgvDrivers.DataSource = null;
//            }
//        }
//        // *** KẾT THÚC KIỂM TRA ***

//        // ... (WireEvents, BtnSelect_Click, BtnCancel_Click, Sorting Logic, Kéo thả giữ nguyên) ...
//        private void WireEvents()
//        {
//            btnSelect.Click += BtnSelect_Click;
//            btnCancel.Click += BtnCancel_Click;
//            dgvDrivers.ColumnHeaderMouseClick += dgvDrivers_ColumnHeaderMouseClick;
//        }

//        private void BtnSelect_Click(object sender, EventArgs e)
//        {
//            if (dgvDrivers.CurrentRow?.DataBoundItem != null)
//            {
//                dynamic selectedItem = dgvDrivers.CurrentRow.DataBoundItem;
//                try
//                {
//                    int selectedId = (int)selectedItem.Id;
//                    DriverSelected?.Invoke(selectedId);
//                    this.FindForm()?.Close();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Không thể lấy ID tài xế đã chọn: {ex.Message}", "Lỗi");
//                }
//            }
//            else
//            {
//                MessageBox.Show("Vui lòng chọn một tài xế.", "Thông báo");
//            }
//        }


//        private void BtnCancel_Click(object sender, EventArgs e)
//        {
//            this.FindForm()?.Close();
//        }

//        private void dgvDrivers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            var list = dgvDrivers.DataSource as IEnumerable<object>;
//            if (list == null || !list.Any()) return;

//            var newColumn = dgvDrivers.Columns[e.ColumnIndex];
//            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

//            if (_sortedColumn == null) _sortOrder = SortOrder.Ascending;
//            else if (_sortedColumn == newColumn) _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
//            else { _sortOrder = SortOrder.Ascending; if (_sortedColumn?.HeaderCell != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; }

//            _sortedColumn = newColumn;

//            try
//            {
//                string propertyName = newColumn.DataPropertyName;
//                var firstItem = list.FirstOrDefault();
//                if (firstItem == null) return;
//                var propInfo = firstItem.GetType().GetProperty(propertyName);
//                if (propInfo == null) return;

//                IEnumerable<object> sortedList;
//                if (_sortOrder == SortOrder.Ascending)
//                    sortedList = list.OrderBy(x => propInfo.GetValue(x, null));
//                else
//                    sortedList = list.OrderByDescending(x => propInfo.GetValue(x, null));

//                dgvDrivers.DataSource = null;
//                dgvDrivers.DataSource = sortedList.ToList();
//                if (newColumn.HeaderCell != null) newColumn.HeaderCell.SortGlyphDirection = _sortOrder;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Sort Error on {newColumn.Name}: {ex.Message}");
//                ResetSortGlyphs();
//            }
//        }

//        private void ResetSortGlyphs()
//        {
//            if (_sortedColumn?.HeaderCell != null)
//            {
//                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//            }
//            _sortedColumn = null;
//            _sortOrder = SortOrder.None;
//            foreach (DataGridViewColumn col in dgvDrivers.Columns)
//            {
//                if (col.SortMode != DataGridViewColumnSortMode.NotSortable && col.HeaderCell != null)
//                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
//            }
//        }

//    }
//}

//==========================
// LMS.GUI/OrderAdmin/ucDriverPicker_Admin.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;

namespace LMS.GUI.OrderAdmin
{
    public partial class ucDriverPicker_Admin : UserControl
    {
        // Bắn ra Id tài xế được chọn cho form cha (popup)
        public event Action<int> DriverSelected;

        private readonly DriverService _driverSvc = new DriverService();
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        public ucDriverPicker_Admin()
        {
            InitializeComponent();
            this.Load += UcDriverPicker_Admin_Load;
        }

        private void UcDriverPicker_Admin_Load(object sender, EventArgs e)
        {
            // Đổi tiêu đề nếu bạn có label tên "lblTitle" trong Designer
            if (this.Controls.Find("lblTitle", true).FirstOrDefault() is Label lbl)
                lbl.Text = "Chọn Tài xế (chỉ hiển thị tài xế rảnh & chưa có xe)";

            ConfigureGrid();
            LoadDrivers();
            WireEvents();

            // Tự reload khi ở màn khác vừa gán/gỡ xe
            AppSession.VehicleAssignmentChanged += OnVehicleAssignmentChanged;
            // Hủy đăng ký khi control bị hủy (tránh trùng Dispose với Designer)
            this.HandleDestroyed += (s, _) =>
            {
                try { AppSession.VehicleAssignmentChanged -= OnVehicleAssignmentChanged; } catch { }
            };
        }

        private void OnVehicleAssignmentChanged()
        {
            if (!IsDisposed) LoadDrivers();
        }

        // ================== GRID ==================
        private void ConfigureGrid()
        {
            var g = dgvDrivers;
            g.Columns.Clear();
            g.ApplyBaseStyle(); // extension style của bạn
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.RowHeadersVisible = false;
            g.AllowUserToAddRows = false;
            g.ReadOnly = true;

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                Visible = false
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                DataPropertyName = "FullName",
                HeaderText = "Họ tên",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Phone",
                DataPropertyName = "Phone",
                HeaderText = "Điện thoại",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LicenseType",
                DataPropertyName = "LicenseType",
                HeaderText = "Hạng GPLX",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            // Nếu muốn hiện CCCD thì mở dòng dưới:
            // g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CitizenId", DataPropertyName = "CitizenId", HeaderText = "CCCD", AutoSizeMode = AllCells, SortMode = Programmatic });
        }

        /// <summary>
        /// Tải danh sách tài xế đang rảnh & chưa có xe.
        /// </summary>
        private void LoadDrivers()
        {
            try
            {
                var availableDrivers = _driverSvc.GetAvailableDriversForAdmin(); // đã lọc VehicleId == null & không bận

                // Bind chỉ những cột cần hiển thị (ẩn bớt dữ liệu)
                var data = availableDrivers.Select(d => new
                {
                    d.Id,
                    d.FullName,
                    d.Phone,
                    d.LicenseType
                    // d.CitizenId
                }).ToList();

                dgvDrivers.DataSource = null;
                dgvDrivers.DataSource = data;

                if (dgvDrivers.Rows.Count > 0)
                {
                    dgvDrivers.ClearSelection();
                    dgvDrivers.Rows[0].Selected = true;
                }
                ResetSortGlyphs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách tài xế rảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvDrivers.DataSource = null;
            }
        }

        // ================== WIRES ==================
        private void WireEvents()
        {
            // Các control này phải có sẵn trong Designer
            if (this.Controls.Find("btnSelect", true).FirstOrDefault() is Button btnSelect)
                btnSelect.Click += BtnSelect_Click;
            if (this.Controls.Find("btnCancel", true).FirstOrDefault() is Button btnCancel)
                btnCancel.Click += (s, e) => this.FindForm()?.Close();

            dgvDrivers.ColumnHeaderMouseClick += dgvDrivers_ColumnHeaderMouseClick;
            dgvDrivers.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) PickCurrent(); };
            dgvDrivers.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; PickCurrent(); } };
        }

        // ================== PICK ==================
        private void BtnSelect_Click(object sender, EventArgs e) => PickCurrent();

        private void PickCurrent()
        {
            if (dgvDrivers.CurrentRow?.DataBoundItem == null)
            {
                MessageBox.Show("Vui lòng chọn một tài xế.", "Thông báo");
                return;
            }

            try
            {
                // DataSource là anonymous type => dùng dynamic hoặc reflection
                dynamic row = dgvDrivers.CurrentRow.DataBoundItem;
                int id = (int)row.Id;
                DriverSelected?.Invoke(id);
                this.FindForm()?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể lấy ID tài xế đã chọn: {ex.Message}", "Lỗi");
            }
        }

        // ================== SORT ==================
        private void dgvDrivers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var list = dgvDrivers.DataSource as IEnumerable<object>;
            if (list == null || !list.Any()) return;

            var newColumn = dgvDrivers.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn)
                _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            else
            {
                if (_sortedColumn?.HeaderCell != null)
                    _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                _sortedColumn = newColumn;
                _sortOrder = SortOrder.Ascending;
            }

            try
            {
                string propName = newColumn.DataPropertyName;
                var first = list.FirstOrDefault();
                if (first == null) return;

                PropertyInfo pi = first.GetType().GetProperty(propName);
                if (pi == null) return;

                IEnumerable<object> sorted = (_sortOrder == SortOrder.Ascending)
                    ? list.OrderBy(x => pi.GetValue(x, null))
                    : list.OrderByDescending(x => pi.GetValue(x, null));

                dgvDrivers.DataSource = sorted.ToList();
                if (newColumn.HeaderCell != null) newColumn.HeaderCell.SortGlyphDirection = _sortOrder;
            }
            catch
            {
                ResetSortGlyphs();
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn?.HeaderCell != null)
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            _sortedColumn = null;
            _sortOrder = SortOrder.None;

            foreach (DataGridViewColumn col in dgvDrivers.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable && col.HeaderCell != null)
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }
    }
}
