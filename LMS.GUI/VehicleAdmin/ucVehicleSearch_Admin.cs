//// LMS.GUI/VehicleAdmin/ucVehicleSearch_Admin.cs
//using Guna.UI2.WinForms;
//using LMS.BUS.Helpers;
//using LMS.BUS.Services; // Giả sử bạn sẽ tạo service này
//using LMS.DAL.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;

//namespace LMS.GUI.VehicleAdmin
//{
//    public partial class ucVehicleSearch_Admin : UserControl
//    {
//        // Event báo ID xe đã được chọn
//        public event EventHandler<int> VehiclePicked;

//        private readonly VehicleService_Admin _vehicleSvc = new VehicleService_Admin();
//        private BindingList<Vehicle> _bindingList; // Hiển thị List<Vehicle>
//        private DataGridViewColumn _sortedColumn = null;
//        private SortOrder _sortOrder = SortOrder.None;

//        private readonly Timer _debounceTimer = new Timer { Interval = 350 }; // Timer cho live search

//        // Dragging State
//        private bool dragging = false;
//        private Point dragCursorPoint;
//        private Point dragFormPoint;

//        public ucVehicleSearch_Admin()
//        {
//            InitializeComponent();
//            this.Load += UcVehicleSearch_Admin_Load;
//            _debounceTimer.Tick += DebounceTimer_Tick;
//        }

//        private void UcVehicleSearch_Admin_Load(object sender, EventArgs e)
//        {
//            if (lblTitle != null) lblTitle.Text = "Tìm Kiếm Phương Tiện";
//            LoadFilters();
//            ConfigureGrid();
//            WireEvents();
//            DoSearch(); // Load dữ liệu ban đầu
//            txtSearchPlateNo.Focus();
//        }

//        #region Setup Controls (Filters & Grid)

//        private void LoadFilters()
//        {
//            // Type Filter ComboBox (Ví dụ)
//            cmbSearchType.Items.Clear();
//            cmbSearchType.Items.Add("— Tất cả —"); // Mục đầu tiên
//            cmbSearchType.Items.AddRange(new string[] { "Xe tải 1.5T", "Xe tải 5T", "Xe bán tải", "Xe máy", "Container" });
//            cmbSearchType.SelectedIndex = 0;

//            // Status Filter ComboBox
//            var statusItems = new List<object> { new { Value = (VehicleStatus?)null, Text = "— Tất cả —" } }; // Dùng nullable Enum
//            statusItems.AddRange(Enum.GetValues(typeof(VehicleStatus))
//                                     .Cast<VehicleStatus>()
//                                     .Select(s => new { Value = (VehicleStatus?)s, Text = FormatVehicleStatus(s) }));
//            cmbSearchStatus.DataSource = statusItems;
//            cmbSearchStatus.DisplayMember = "Text";
//            cmbSearchStatus.ValueMember = "Value";
//            cmbSearchStatus.SelectedIndex = 0;
//        }

//        private void ConfigureGrid()
//        {
//            var g = dgvSearchResults;
//            g.Columns.Clear();
//            g.ApplyBaseStyle();

//            // Các cột cần hiển thị trong kết quả tìm kiếm
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "PlateNo", DataPropertyName = "PlateNo", HeaderText = "Biển số xe", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Type", DataPropertyName = "Type", HeaderText = "Loại xe", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CapacityKg", DataPropertyName = "CapacityKg", HeaderText = "Trọng tải (kg)", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" }, SortMode = DataGridViewColumnSortMode.Programmatic });
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            // Thêm cột tài xế nếu cần
//            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DriverName", DataPropertyName = "DriverName", HeaderText = "Tài xế", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 30, SortMode = DataGridViewColumnSortMode.Programmatic });


//            // Gán sự kiện
//            g.CellFormatting += DgvSearchResults_CellFormatting;
//            g.ColumnHeaderMouseClick += dgvSearchResults_ColumnHeaderMouseClick;
//            g.CellDoubleClick += dgvSearchResults_CellDoubleClick; // Sự kiện double click
//        }

//        // Helper format Enum Status (có thể đưa ra class riêng)
//        private string FormatVehicleStatus(VehicleStatus status)
//        {
//            switch (status)
//            {
//                case VehicleStatus.Active: return "Hoạt động";
//                case VehicleStatus.Maintenance: return "Bảo trì";
//                case VehicleStatus.Inactive: return "Ngừng hoạt động";
//                default: return status.ToString();
//            }
//        }
//        // Helper lấy tên tài xế (cần sửa lại cách lấy dữ liệu trong DoSearch)
//        private string GetDriverName(Vehicle vehicle)
//        {
//            // Cách 1: Nếu Vehicle model có navigation property Driver đã được load
//            // return vehicle.Driver?.FullName;

//            // Cách 2: Truy vấn lại DB (không hiệu quả nếu nhiều dòng) - Tạm thời dùng cách này
//            if (!vehicle.DriverId.HasValue) return "";
//            using (var tempDb = new DAL.LogisticsDbContext())
//            {
//                return tempDb.Drivers.Where(d => d.Id == vehicle.DriverId.Value).Select(d => d.FullName).FirstOrDefault();
//            }
//        }


//        #endregion

//        #region Event Wiring & Live Search

//        private void WireEvents()
//        {
//            // Filter Controls -> Trigger Debounce Timer
//            txtSearchPlateNo.TextChanged += FilterControl_Changed;
//            cmbSearchType.SelectedIndexChanged += FilterControl_Changed;
//            cmbSearchStatus.SelectedIndexChanged += FilterControl_Changed;

//            // Buttons
//            // btnSearchDo.Click += (s, e) => DoSearch(); // Bỏ nếu dùng live search
//            btnReset.Click += BtnReset_Click;
//            //if (btnClose != null) btnClose.Click += (s, e) => this.FindForm()?.Close();

//            // Grid events are wired in ConfigureGrid

//            // Drag and Drop
//            Control dragHandle = pnlTop;
//            if (dragHandle != null)
//            {
//                dragHandle.MouseDown += PnlTop_MouseDown;
//                dragHandle.MouseMove += PnlTop_MouseMove;
//                dragHandle.MouseUp += PnlTop_MouseUp;
//            }
//        }

//        private void FilterControl_Changed(object sender, EventArgs e)
//        {
//            _debounceTimer.Stop();
//            _debounceTimer.Start();
//        }

//        private void DebounceTimer_Tick(object sender, EventArgs e)
//        {
//            _debounceTimer.Stop();
//            DoSearch();
//        }

//        #endregion

//        #region Actions (Search, Reset, Pick)

//        private void DoSearch()
//        {
//            try
//            {
//                string plateFilter = txtSearchPlateNo.Text.Trim();
//                string typeFilter = cmbSearchType.SelectedIndex > 0 ? cmbSearchType.SelectedItem.ToString() : null;
//                VehicleStatus? statusFilter = cmbSearchStatus.SelectedValue as VehicleStatus?;

//                // TODO: Implement SearchVehicles in VehicleService_Admin
//                var results = _vehicleSvc.SearchVehicles(
//                    string.IsNullOrWhiteSpace(plateFilter) ? null : plateFilter,
//                    typeFilter,
//                    statusFilter
//                );

//                // *** SỬA LẠI ĐỂ BAO GỒM TÊN TÀI XẾ (NẾU CẦN HIỂN THỊ NGAY) ***
//                // Cách 1: Tạo Anonymous Type (đơn giản nếu chỉ dùng ở đây)
//                var displayData = results.Select(v => new
//                {
//                    v.Id,
//                    v.PlateNo,
//                    v.Type,
//                    v.CapacityKg,
//                    v.Status,
//                    DriverName = v.Driver?.FullName // Dùng navigation property nếu service có Include(v => v.Driver)
//                                                    // DriverName = GetDriverName(v) // Hoặc gọi hàm helper (chậm hơn)
//                }).ToList();
//                _bindingList = new BindingList<Vehicle>(results); // Vẫn giữ BindingList gốc để sort
//                                                                  // dgvSearchResults.DataSource = new BindingList<object>(displayData.Cast<object>().ToList()); // Gán anonymous type nếu cần

//                // Cách 2: Dùng BindingList<Vehicle> và xử lý tên tài xế trong CellFormatting
//                _bindingList = new BindingList<Vehicle>(results);
//                dgvSearchResults.DataSource = _bindingList;


//                ApplySort();
//                UpdateSortGlyphs();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi tìm kiếm phương tiện: {ex.Message}", "Lỗi");
//                dgvSearchResults.DataSource = null;
//            }
//        }


//        private void BtnReset_Click(object sender, EventArgs e)
//        {
//            // Tạm ngắt event
//            txtSearchPlateNo.TextChanged -= FilterControl_Changed;
//            cmbSearchType.SelectedIndexChanged -= FilterControl_Changed;
//            cmbSearchStatus.SelectedIndexChanged -= FilterControl_Changed;

//            txtSearchPlateNo.Clear();
//            cmbSearchType.SelectedIndex = 0;
//            cmbSearchStatus.SelectedIndex = 0;

//            // Gắn lại event
//            txtSearchPlateNo.TextChanged += FilterControl_Changed;
//            cmbSearchType.SelectedIndexChanged += FilterControl_Changed;
//            cmbSearchStatus.SelectedIndexChanged += FilterControl_Changed;

//            ResetSortGlyphs();
//            DoSearch(); // Tìm lại với filter trống
//            txtSearchPlateNo.Focus();
//        }

//        // Xử lý double click để chọn xe
//        private void dgvSearchResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex >= 0 && dgvSearchResults.Rows[e.RowIndex].DataBoundItem is Vehicle selectedVehicle)
//            {
//                try
//                {
//                    VehiclePicked?.Invoke(this, selectedVehicle.Id); // Gửi ID ra ngoài
//                                                                     // Form cha (ucVehicle_Admin) sẽ tự đóng popup này
//                    this.FindForm()?.Close(); // Hoặc đóng trực tiếp nếu cần
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Lỗi khi chọn phương tiện: {ex.Message}", "Lỗi");
//                }
//            }
//        }

//        #endregion

//        #region Sorting Logic (Cần đổi typeof)
//        private void dgvSearchResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            if (_bindingList == null || _bindingList.Count == 0) return;
//            var newColumn = dgvSearchResults.Columns[e.ColumnIndex];
//            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

//            // Xử lý riêng cho cột DriverName (vì không có trong Model Vehicle)
//            if (newColumn.Name == "DriverName")
//            {
//                MessageBox.Show("Chức năng sắp xếp theo tên tài xế chưa được hỗ trợ trong tìm kiếm này.");
//                // Hoặc thực hiện sort phức tạp hơn nếu muốn (cần lấy tên tài xế trước)
//                ResetSortGlyphs(); // Reset nếu không sort được
//                return;
//            }


//            if (_sortedColumn == newColumn) { _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
//            else { if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _sortOrder = SortOrder.Ascending; _sortedColumn = newColumn; }
//            ApplySort();
//            UpdateSortGlyphs();
//        }

//        private void ApplySort()
//        {
//            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;
//            string propertyName = _sortedColumn.DataPropertyName;
//            PropertyInfo propInfo = typeof(Vehicle).GetProperty(propertyName); // Đổi typeof
//            if (propInfo == null) return;

//            List<Vehicle> items = _bindingList.ToList(); // Đổi typeof
//            List<Vehicle> sortedList; // Đổi typeof
//            try
//            {
//                if (_sortOrder == SortOrder.Ascending)
//                    sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList();
//                else
//                    sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

//                _bindingList = new BindingList<Vehicle>(sortedList); // Đổi typeof
//                dgvSearchResults.DataSource = _bindingList;
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp: {ex.Message}"); ResetSortGlyphs(); }
//        }

//        private void UpdateSortGlyphs()
//        {
//            foreach (DataGridViewColumn col in dgvSearchResults.Columns)
//            {
//                if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None;
//                if (_sortedColumn != null && col == _sortedColumn) col.HeaderCell.SortGlyphDirection = _sortOrder;
//            }
//        }
//        private void ResetSortGlyphs()
//        {
//            if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
//            _sortedColumn = null; _sortOrder = SortOrder.None;
//            UpdateSortGlyphs();
//        }
//        #endregion

//        #region Cell Formatting
//        private void DgvSearchResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            // Format cột Status
//            if (dgvSearchResults.Columns[e.ColumnIndex].Name == "Status" && e.Value is VehicleStatus status)
//            {
//                e.Value = FormatVehicleStatus(status);
//                e.FormattingApplied = true;
//            }
//            // Lấy tên tài xế (nếu dùng cách binding trực tiếp List<Vehicle>)
//            else if (dgvSearchResults.Columns[e.ColumnIndex].Name == "DriverName" && e.RowIndex >= 0)
//            {
//                if (dgvSearchResults.Rows[e.RowIndex].DataBoundItem is Vehicle vehicle)
//                {
//                    // Ưu tiên dùng navigation property nếu service đã Include
//                    if (vehicle.Driver != null)
//                    {
//                        e.Value = vehicle.Driver.FullName;
//                    }
//                    // Fallback: Gọi helper (chậm hơn) - bỏ nếu service đã Include
//                    // else if (vehicle.DriverId.HasValue)
//                    // {
//                    //     e.Value = GetDriverName(vehicle); // Có thể gây chậm nếu nhiều dòng
//                    // }
//                    else
//                    {
//                        e.Value = ""; // Hoặc "(Chưa gán)"
//                    }
//                    e.FormattingApplied = true;
//                }
//            }
//        }
//        #endregion

//        #region Form Dragging Logic
//        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left) { Form parentForm = this.FindForm(); if (parentForm != null) { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = parentForm.Location; } }
//        }
//        private void PnlTop_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (dragging) { Form parentForm = this.FindForm(); if (parentForm != null) { Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint)); parentForm.Location = Point.Add(dragFormPoint, new Size(dif)); } }
//        }
//        private void PnlTop_MouseUp(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left) { dragging = false; }
//        }
//        #endregion
//    }
//}
// LMS.GUI/VehicleAdmin/ucVehicleSearch_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Helpers;
using LMS.BUS.Services; // Giả sử bạn sẽ tạo service này
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.VehicleAdmin
{
    public partial class ucVehicleSearch_Admin : UserControl
    {
        public event EventHandler<int> VehiclePicked;

        private readonly VehicleService_Admin _vehicleSvc = new VehicleService_Admin();
        private BindingList<Vehicle> _bindingList; // Hiển thị List<Vehicle>
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        private readonly Timer _debounceTimer = new Timer { Interval = 350 };

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public ucVehicleSearch_Admin()
        {
            InitializeComponent();
            this.Load += UcVehicleSearch_Admin_Load;
            _debounceTimer.Tick += DebounceTimer_Tick;
        }

        private void UcVehicleSearch_Admin_Load(object sender, EventArgs e)
        {
            if (lblTitle != null) lblTitle.Text = "Tìm Kiếm Phương Tiện";
            LoadFilters();
            ConfigureGrid();
            WireEvents();
            DoSearch();
            txtSearchPlateNo.Focus();
        }

        #region Setup Controls (Filters & Grid)

        private void LoadFilters()
        {
            cmbSearchType.Items.Clear();
            cmbSearchType.Items.Add("— Tất cả —");
            cmbSearchType.Items.AddRange(new string[] { "Xe tải 1.5T", "Xe tải 5T", "Xe bán tải", "Xe máy", "Container" });
            cmbSearchType.SelectedIndex = 0;

            var statusItems = new List<object> { new { Value = (VehicleStatus?)null, Text = "— Tất cả —" } };
            statusItems.AddRange(Enum.GetValues(typeof(VehicleStatus))
                                     .Cast<VehicleStatus>()
                                     .Select(s => new { Value = (VehicleStatus?)s, Text = FormatVehicleStatus(s) }));
            cmbSearchStatus.DataSource = statusItems;
            cmbSearchStatus.DisplayMember = "Text";
            cmbSearchStatus.ValueMember = "Value";
            cmbSearchStatus.SelectedIndex = 0;
        }

        private void ConfigureGrid()
        {
            var g = dgvSearchResults;
            g.Columns.Clear();
            g.ApplyBaseStyle();

            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "PlateNo", DataPropertyName = "PlateNo", HeaderText = "Biển số xe", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Type", DataPropertyName = "Type", HeaderText = "Loại xe", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CapacityKg", DataPropertyName = "CapacityKg", HeaderText = "Trọng tải (kg)", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" }, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            // *** SỬA DataPropertyName CHO CỘT DriverName ***
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DriverName", DataPropertyName = "Driver", HeaderText = "Tài xế", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 30, SortMode = DataGridViewColumnSortMode.Programmatic }); // Dùng "Driver"

            g.CellFormatting += DgvSearchResults_CellFormatting;
            g.ColumnHeaderMouseClick += dgvSearchResults_ColumnHeaderMouseClick;
            g.CellDoubleClick += dgvSearchResults_CellDoubleClick;
        }

        // Helper format Enum Status
        private string FormatVehicleStatus(VehicleStatus status)
        {
            switch (status)
            {
                case VehicleStatus.Active: return "Hoạt động";
                case VehicleStatus.Maintenance: return "Bảo trì";
                case VehicleStatus.Inactive: return "Ngừng hoạt động";
                default: return status.ToString();
            }
        }


        #endregion

        #region Event Wiring & Live Search

        private void WireEvents()
        {
            txtSearchPlateNo.TextChanged += FilterControl_Changed;
            cmbSearchType.SelectedIndexChanged += FilterControl_Changed;
            cmbSearchStatus.SelectedIndexChanged += FilterControl_Changed;

            btnReset.Click += BtnReset_Click;

            Control dragHandle = pnlTop;
            if (dragHandle != null)
            {
                dragHandle.MouseDown += PnlTop_MouseDown;
                dragHandle.MouseMove += PnlTop_MouseMove;
                dragHandle.MouseUp += PnlTop_MouseUp;
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

        #region Actions (Search, Reset, Pick)

        private void DoSearch()
        {
            try
            {
                string plateFilter = txtSearchPlateNo.Text.Trim();
                string typeFilter = cmbSearchType.SelectedIndex > 0 ? cmbSearchType.SelectedItem.ToString() : null;
                VehicleStatus? statusFilter = cmbSearchStatus.SelectedValue as VehicleStatus?;

                var results = _vehicleSvc.SearchVehicles(
                    string.IsNullOrWhiteSpace(plateFilter) ? null : plateFilter,
                    typeFilter,
                    statusFilter
                );

                _bindingList = new BindingList<Vehicle>(results);
                dgvSearchResults.DataSource = _bindingList;

                ApplySort(); // Apply sort nếu có trạng thái cũ
                UpdateSortGlyphs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm phương tiện: {ex.Message}", "Lỗi");
                dgvSearchResults.DataSource = null;
            }
        }


        private void BtnReset_Click(object sender, EventArgs e)
        {
            // Tạm ngắt event
            txtSearchPlateNo.TextChanged -= FilterControl_Changed;
            cmbSearchType.SelectedIndexChanged -= FilterControl_Changed;
            cmbSearchStatus.SelectedIndexChanged -= FilterControl_Changed;

            txtSearchPlateNo.Clear();
            cmbSearchType.SelectedIndex = 0;
            cmbSearchStatus.SelectedIndex = 0;

            // Gắn lại event
            txtSearchPlateNo.TextChanged += FilterControl_Changed;
            cmbSearchType.SelectedIndexChanged += FilterControl_Changed;
            cmbSearchStatus.SelectedIndexChanged += FilterControl_Changed;

            ResetSortGlyphs();
            DoSearch();
            txtSearchPlateNo.Focus();
        }

        private void dgvSearchResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSearchResults.Rows[e.RowIndex].DataBoundItem is Vehicle selectedVehicle)
            {
                try
                {
                    VehiclePicked?.Invoke(this, selectedVehicle.Id);
                    this.FindForm()?.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi chọn phương tiện: {ex.Message}", "Lỗi");
                }
            }
        }

        #endregion

        #region Sorting Logic (Đã sửa)
        private void dgvSearchResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;
            var newColumn = dgvSearchResults.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            // Xử lý riêng cột DriverName
            if (newColumn.Name == "DriverName")
            {
                string propertyName = "Driver.FullName"; // Thuộc tính lồng
                                                         // Xác định hướng sort cho cột lồng
                if (_sortedColumn == newColumn) { _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
                else { if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _sortOrder = SortOrder.Ascending; _sortedColumn = newColumn; } // Cập nhật _sortedColumn

                ApplySortNestedProperty(propertyName); // Gọi hàm sort lồng
                UpdateSortGlyphs();
                return; // Kết thúc sớm
            }

            // Xử lý sort cho các cột thông thường
            if (_sortedColumn == newColumn) { _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
            else { if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _sortOrder = SortOrder.Ascending; _sortedColumn = newColumn; }
            ApplySort();
            UpdateSortGlyphs();
        }

        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null || _bindingList.Count == 0) return;
            string propertyName = _sortedColumn.DataPropertyName;
            PropertyInfo propInfo = typeof(Vehicle).GetProperty(propertyName);
            if (propInfo == null) return;

            List<Vehicle> items = _bindingList.ToList();
            List<Vehicle> sortedList;
            try
            {
                if (_sortOrder == SortOrder.Ascending)
                    sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList();
                else
                    sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

                _bindingList = new BindingList<Vehicle>(sortedList);
                dgvSearchResults.DataSource = _bindingList;
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp: {ex.Message}"); ResetSortGlyphs(); }
        }

        private void ApplySortNestedProperty(string nestedPropertyName)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;
            List<Vehicle> items = _bindingList.ToList();
            List<Vehicle> sortedList;

            try
            {
                Func<object, string, object> GetNestedPropertyValue = (obj, propName) =>
                { /* ... (giữ nguyên helper này) ... */
                    if (obj == null) return null;
                    string[] parts = propName.Split('.');
                    object currentObj = obj;
                    foreach (var part in parts)
                    {
                        if (currentObj == null) return null;
                        var prop = currentObj.GetType().GetProperty(part);
                        if (prop == null) return null;
                        currentObj = prop.GetValue(currentObj, null);
                    }
                    return currentObj;
                };

                if (_sortOrder == SortOrder.Ascending)
                {
                    sortedList = items.OrderBy(x => GetNestedPropertyValue(x, nestedPropertyName) == null)
                                      .ThenBy(x => GetNestedPropertyValue(x, nestedPropertyName))
                                      .ToList();
                }
                else // Descending
                {
                    sortedList = items.OrderBy(x => GetNestedPropertyValue(x, nestedPropertyName) == null)
                                      .ThenByDescending(x => GetNestedPropertyValue(x, nestedPropertyName))
                                      .ToList();
                }
                _bindingList = new BindingList<Vehicle>(sortedList);
                dgvSearchResults.DataSource = _bindingList;
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp lồng nhau: {ex.Message}"); ResetSortGlyphs(); }
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
            if (_sortedColumn != null && _sortedColumn.HeaderCell != null) // Check HeaderCell null
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            _sortedColumn = null; _sortOrder = SortOrder.None;
            UpdateSortGlyphs();
        }
        #endregion

        #region Cell Formatting (Đã sửa)
        private void DgvSearchResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Format cột Status
            if (dgvSearchResults.Columns[e.ColumnIndex].Name == "Status" && e.Value is VehicleStatus status)
            {
                e.Value = FormatVehicleStatus(status);
                e.FormattingApplied = true;
            }
            else if (dgvSearchResults.Columns[e.ColumnIndex].Name == "DriverName" && e.Value is Driver driver) // e.Value là Driver object
            {
                e.Value = driver.FullName; // Lấy FullName
                e.FormattingApplied = true;
            }
            else if (dgvSearchResults.Columns[e.ColumnIndex].Name == "DriverName" && e.Value == null && e.RowIndex >= 0) // Xử lý null
            {
                e.Value = "(Chưa gán)";
                e.FormattingApplied = true;
            }
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