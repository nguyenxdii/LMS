//// LMS.GUI/ShipmentAdmin/ucShipment_Admin.cs
//using Guna.UI2.WinForms;
//using LMS.BUS.Dtos;
//using LMS.BUS.Helpers;
//using LMS.BUS.Services;
//using LMS.DAL.Models;
//using LMS.GUI.DriverAdmin; // Cần cho ucDriverPicker_Admin
//using LMS.GUI.OrderAdmin;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;

//namespace LMS.GUI.ShipmentAdmin
//{
//    public partial class ucShipment_Admin : UserControl
//    {
//        private readonly ShipmentService_Admin _shipmentSvc = new ShipmentService_Admin();
//        private readonly OrderService_Admin _orderSvc = new OrderService_Admin(); // Cần cho AssignDriver
//        private BindingList<ShipmentListItemAdminDto> _bindingList;
//        private DataGridViewColumn _sortedColumn = null;
//        private SortOrder _sortOrder = SortOrder.None;

//        public ucShipment_Admin()
//        {
//            InitializeComponent();
//        }

//        protected override void OnLoad(EventArgs e)
//        {
//            base.OnLoad(e);
//            ConfigureGrid();
//            WireEvents();
//            LoadData();
//        }

//        #region Grid Config
//        private void ConfigureGrid()
//        {
//            dgvShipments.Columns.Clear();
//            dgvShipments.ApplyBaseStyle();

//            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
//            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShipmentNo", DataPropertyName = "ShipmentNo", HeaderText = "Mã Chuyến", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo", DataPropertyName = "OrderNo", HeaderText = "Mã Đơn", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "DriverName", DataPropertyName = "DriverName", HeaderText = "Tài xế", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "Route", DataPropertyName = "Route", HeaderText = "Tuyến", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "UpdatedAt", DataPropertyName = "UpdatedAt", HeaderText = "Cập nhật", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "StartedAt", DataPropertyName = "StartedAt", HeaderText = "Bắt đầu", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }, SortMode = DataGridViewColumnSortMode.Programmatic });
//            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "DeliveredAt", DataPropertyName = "DeliveredAt", HeaderText = "Kết thúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }, SortMode = DataGridViewColumnSortMode.Programmatic });

//            dgvShipments.CellFormatting += DgvShipments_CellFormatting; // Format Status
//        }
//        #endregion

//        #region Load + Helpers
//        private void LoadData()
//        {
//            try
//            {
//                var shipments = _shipmentSvc.GetAllShipmentsForAdmin();
//                _bindingList = new BindingList<ShipmentListItemAdminDto>(shipments);
//                dgvShipments.DataSource = _bindingList;

//                if (dgvShipments.Rows.Count > 0)
//                {
//                    dgvShipments.ClearSelection();
//                    dgvShipments.Rows[0].Selected = true;
//                }

//                ResetSortGlyphs();
//                UpdateButtonsState();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tải dữ liệu chuyến hàng: {ex.Message}", "Lỗi");
//                dgvShipments.DataSource = null;
//            }
//        }

//        private ShipmentListItemAdminDto GetSelectedShipment()
//        {
//            return dgvShipments.CurrentRow?.DataBoundItem as ShipmentListItemAdminDto;
//        }

//        private void UpdateButtonsState()
//        {
//            var selected = GetSelectedShipment();
//            bool hasSelection = selected != null;

//            btnViewDetail.Enabled = hasSelection;

//            // Nút đổi tài xế chỉ bật khi có chọn và chuyến chưa kết thúc
//            bool canAssign = hasSelection &&
//                             selected.Status != ShipmentStatus.Delivered &&
//                             selected.Status != ShipmentStatus.Failed;
//            btnAssignDriver.Enabled = canAssign;
//            // btnSearch, btnReload thường luôn bật
//        }

//        private void SelectRowById(int shipmentId)
//        {
//            if (dgvShipments.Rows.Count == 0 || _bindingList == null) return;
//            ShipmentListItemAdminDto itemToSelect = _bindingList.FirstOrDefault(s => s.Id == shipmentId);
//            if (itemToSelect != null)
//            {
//                int rowIndex = _bindingList.IndexOf(itemToSelect);
//                if (rowIndex >= 0 && rowIndex < dgvShipments.Rows.Count)
//                {
//                    dgvShipments.ClearSelection();
//                    dgvShipments.Rows[rowIndex].Selected = true;
//                    if (dgvShipments.Columns.Contains("ShipmentNo"))
//                        dgvShipments.CurrentCell = dgvShipments.Rows[rowIndex].Cells["ShipmentNo"];

//                    if (!dgvShipments.Rows[rowIndex].Displayed)
//                    {
//                        int firstDisplayed = Math.Max(0, rowIndex - dgvShipments.DisplayedRowCount(false) / 2);
//                        dgvShipments.FirstDisplayedScrollingRowIndex = Math.Min(firstDisplayed, dgvShipments.RowCount - 1);
//                    }
//                }
//            }
//            UpdateButtonsState();
//        }
//        #endregion

//        #region Wire Events
//        private void WireEvents()
//        {
//            btnReload.Click += (s, e) => LoadData();
//            btnSearch.Click += (s, e) => OpenSearchPopup();
//            btnViewDetail.Click += (s, e) => OpenDetailPopup();
//            btnAssignDriver.Click += (s, e) => AssignNewDriver();

//            dgvShipments.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenDetailPopup(); };
//            dgvShipments.ColumnHeaderMouseClick += dgvShipments_ColumnHeaderMouseClick;
//            dgvShipments.SelectionChanged += (s, e) => UpdateButtonsState();
//        }
//        #endregion

//        #region Actions
//        private void OpenSearchPopup()
//        {
//            int? selectedId = null;
//            using (var searchForm = new Form
//            {
//                Text = "Tìm kiếm Chuyến hàng",
//                StartPosition = FormStartPosition.CenterScreen,
//                FormBorderStyle = FormBorderStyle.None,
//                Size = new Size(1199, 772) // Kích thước lớn hơn cho nhiều filter
//            })
//            {
//                var ucSearch = new ucShipmentSearch_Admin { Dock = DockStyle.Fill };
//                ucSearch.ShipmentPicked += (sender, id) => // Đổi tên sự kiện
//                {
//                    selectedId = id;
//                    searchForm.DialogResult = DialogResult.OK;
//                    searchForm.Close();
//                };
//                searchForm.Controls.Add(ucSearch);
//                Form ownerForm = this.FindForm();
//                if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }
//                if (searchForm.ShowDialog(ownerForm) == DialogResult.OK && selectedId.HasValue)
//                {
//                    SelectRowById(selectedId.Value);
//                }
//            }
//        }

//        private void OpenDetailPopup()
//        {
//            var selected = GetSelectedShipment();
//            if (selected == null) { MessageBox.Show("Vui lòng chọn chuyến hàng.", "Thông báo"); return; }
//            try
//            {
//                // Cần tạo ucShipmentDetail_Admin trước
//                var ucDetail = new ucShipmentDetail_Admin(selected.Id);
//                using (var popupForm = new Form
//                {
//                    Text = $"Chi tiết chuyến hàng {selected.ShipmentNo}",
//                    StartPosition = FormStartPosition.CenterScreen,
//                    FormBorderStyle = FormBorderStyle.None,
//                    // Đặt kích thước phù hợp
//                    Size = new Size(1277, 801)
//                })
//                {
//                    ucDetail.Dock = DockStyle.Fill;
//                    popupForm.Controls.Add(ucDetail);
//                    Form ownerForm = this.FindForm();
//                    if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }
//                    popupForm.ShowDialog(ownerForm);
//                }
//                // Tùy chọn: LoadData() nếu có thể có thay đổi trong detail (ví dụ: sửa note)
//            }
//            catch (Exception ex) { MessageBox.Show($"Lỗi mở chi tiết: {ex.Message}", "Lỗi"); }
//        }

//        private void AssignNewDriver()
//        {
//            var selectedShipment = GetSelectedShipment();
//            if (selectedShipment == null) { MessageBox.Show("Vui lòng chọn chuyến hàng.", "Thông báo"); return; }

//            int? selectedDriverId = null;
//            using (var fPicker = new Form
//            {
//                Text = "Chọn Tài Xế Mới",
//                StartPosition = FormStartPosition.CenterScreen,
//                Size = new Size(583, 539), // Size của ucDriverPicker
//                FormBorderStyle = FormBorderStyle.None,
//            })
//            {
//                var ucPicker = new ucDriverPicker_Admin(); // Dùng lại UC chọn Driver
//                ucPicker.DriverSelected += (selectedId) =>
//                {
//                    selectedDriverId = selectedId;
//                    fPicker.DialogResult = DialogResult.OK;
//                };
//                fPicker.Controls.Add(ucPicker);
//                Form ownerForm = this.FindForm();
//                if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }

//                if (fPicker.ShowDialog(ownerForm) == DialogResult.OK && selectedDriverId.HasValue)
//                {
//                    try
//                    {
//                        // Gọi service để gán tài xế mới
//                        _orderSvc.AssignDriver(selectedShipment.Id, selectedDriverId.Value);
//                        MessageBox.Show("Đã đổi tài xế thành công.", "Thông báo");
//                        LoadData(); // Tải lại grid để cập nhật tên tài xế
//                        SelectRowById(selectedShipment.Id); // Chọn lại dòng vừa đổi
//                    }
//                    catch (Exception ex)
//                    {
//                        MessageBox.Show($"Lỗi khi đổi tài xế: {ex.Message}", "Lỗi");
//                    }
//                }
//            }
//        }
//        #endregion

//        #region Sorting Logic
//        // (Giữ nguyên các hàm dgvShipments_ColumnHeaderMouseClick, ApplySort, ResetSortGlyphs)
//        // Chỉ cần đổi typeof(Customer) thành typeof(ShipmentListItemAdminDto) trong ApplySort
//        // LMS.GUI/ShipmentAdmin/ucShipment_Admin.cs

//        // ... (rest of the code remains the same) ...

//        #region Sorting Logic
//        private void dgvShipments_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            if (_bindingList == null || _bindingList.Count == 0) return;
//            var newColumn = dgvShipments.Columns[e.ColumnIndex];
//            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

//            if (_sortedColumn == newColumn) { _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
//            else { if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _sortOrder = SortOrder.Ascending; _sortedColumn = newColumn; }

//            ApplySort(); // Call the updated ApplySort
//            if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = _sortOrder;
//        }

//        private void ApplySort()
//        {
//            if (_sortedColumn == null || _bindingList == null) return;

//            string propertyName = _sortedColumn.DataPropertyName;
//            List<ShipmentListItemAdminDto> items = _bindingList.ToList();
//            List<ShipmentListItemAdminDto> sortedList;

//            try
//            {
//                // === CUSTOM SORT LOGIC FOR OrderNo ===
//                if (propertyName == "OrderNo")
//                {
//                    if (_sortOrder == SortOrder.Ascending)
//                    {
//                        // Sort ascending by parsed ID
//                        sortedList = items.OrderBy(item => OrderCode.TryParseId(item.OrderNo, out int id) ? id : int.MaxValue).ToList();
//                    }
//                    else // Descending
//                    {
//                        // Sort descending by parsed ID
//                        sortedList = items.OrderByDescending(item => OrderCode.TryParseId(item.OrderNo, out int id) ? id : int.MinValue).ToList();
//                    }
//                }
//                // === DEFAULT SORT LOGIC FOR OTHER COLUMNS ===
//                else
//                {
//                    var propInfo = typeof(ShipmentListItemAdminDto).GetProperty(propertyName);
//                    if (propInfo == null) return; // Cannot sort if property not found

//                    if (_sortOrder == SortOrder.Ascending)
//                    {
//                        sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList();
//                    }
//                    else // Descending
//                    {
//                        sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();
//                    }
//                }

//                // Update BindingList and DataSource
//                _bindingList = new BindingList<ShipmentListItemAdminDto>(sortedList);
//                dgvShipments.DataSource = _bindingList;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi sắp xếp cột '{_sortedColumn.HeaderText}': {ex.Message}");
//                ResetSortGlyphs(); // Reset glyphs on error
//            }
//        }

//        private void ResetSortGlyphs()
//        {
//            if (_sortedColumn != null) { _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; }
//            _sortedColumn = null; _sortOrder = SortOrder.None;
//            foreach (DataGridViewColumn col in dgvShipments.Columns) { if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None; }
//        }
//        #endregion

//        // ... (rest of the code: CellFormatting, etc. remains the same) ...
//        #endregion

//        #region Cell Formatting
//        // Format Enum ShipmentStatus sang tiếng Việt
//        private void DgvShipments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            if (dgvShipments.Columns[e.ColumnIndex].Name == "Status" && e.Value is ShipmentStatus status)
//            {
//                switch (status)
//                {
//                    case ShipmentStatus.Pending: e.Value = "Chờ nhận"; break;
//                    case ShipmentStatus.Assigned: e.Value = "Đã nhận"; break;
//                    case ShipmentStatus.OnRoute: e.Value = "Đang đi đường"; break;
//                    case ShipmentStatus.AtWarehouse: e.Value = "Đang ở kho"; break;
//                    case ShipmentStatus.ArrivedDestination: e.Value = "Đã tới đích"; break;
//                    case ShipmentStatus.Delivered: e.Value = "Đã giao xong"; break;
//                    case ShipmentStatus.Failed: e.Value = "Gặp sự cố"; break;
//                    default: e.Value = status.ToString(); break;
//                }
//                e.FormattingApplied = true;
//            }
//        }
//        #endregion
//    }
//}

// LMS.GUI/ShipmentAdmin/ucShipment_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
//using LMS.GUI.DriverAdmin; // Cần cho ucDriverPicker_Admin
using LMS.GUI.OrderAdmin;   // ucDriverPicker_Admin đang ở đây
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.ShipmentAdmin
{
    public partial class ucShipment_Admin : UserControl
    {
        // ... (Các biến và Constructor giữ nguyên) ...
        private readonly ShipmentService_Admin _shipmentSvc = new ShipmentService_Admin();
        private readonly OrderService_Admin _orderSvc = new OrderService_Admin();
        private BindingList<ShipmentListItemAdminDto> _bindingList;
        private DataGridViewColumn _sortedColumn = null;
        private SortOrder _sortOrder = SortOrder.None;

        public ucShipment_Admin()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureGrid();
            WireEvents();
            LoadData();
        }


        // ... (ConfigureGrid, LoadData, GetSelectedShipment giữ nguyên) ...
        #region Grid Config
        private void ConfigureGrid()
        {
            dgvShipments.Columns.Clear();
            dgvShipments.ApplyBaseStyle();

            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShipmentNo", DataPropertyName = "ShipmentNo", HeaderText = "Mã Chuyến", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo", DataPropertyName = "OrderNo", HeaderText = "Mã Đơn", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "DriverName", DataPropertyName = "DriverName", HeaderText = "Tài xế", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "Route", DataPropertyName = "Route", HeaderText = "Tuyến", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic }); // Đổi AllCells thành Fill
            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "UpdatedAt", DataPropertyName = "UpdatedAt", HeaderText = "Cập nhật", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "StartedAt", DataPropertyName = "StartedAt", HeaderText = "Bắt đầu", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }, SortMode = DataGridViewColumnSortMode.Programmatic });
            dgvShipments.Columns.Add(new DataGridViewTextBoxColumn { Name = "DeliveredAt", DataPropertyName = "DeliveredAt", HeaderText = "Kết thúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }, SortMode = DataGridViewColumnSortMode.Programmatic });

            dgvShipments.CellFormatting += DgvShipments_CellFormatting; // Format Status
        }
        #endregion

        #region Load + Helpers
        private void LoadData()
        {
            try
            {
                var shipments = _shipmentSvc.GetAllShipmentsForAdmin();
                _bindingList = new BindingList<ShipmentListItemAdminDto>(shipments);
                dgvShipments.DataSource = _bindingList;

                if (dgvShipments.Rows.Count > 0)
                {
                    dgvShipments.ClearSelection();
                    dgvShipments.Rows[0].Selected = true;
                }

                ResetSortGlyphs();
                UpdateButtonsState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu chuyến hàng: {ex.Message}", "Lỗi");
                dgvShipments.DataSource = null;
            }
        }

        private ShipmentListItemAdminDto GetSelectedShipment()
        {
            return dgvShipments.CurrentRow?.DataBoundItem as ShipmentListItemAdminDto;
        }

        // *** SỬA HÀM NÀY ***
        private void UpdateButtonsState()
        {
            var selected = GetSelectedShipment();
            bool hasSelection = selected != null;

            btnViewDetail.Enabled = hasSelection;

            // Nút đổi tài xế chỉ bật khi có chọn VÀ chuyến đang trong quá trình vận chuyển
            var runningStatuses = new[] {
                ShipmentStatus.Assigned,
                ShipmentStatus.OnRoute,
                ShipmentStatus.AtWarehouse,
                ShipmentStatus.ArrivedDestination
                // Có thể thêm Pending nếu muốn đổi ngay cả khi chưa nhận
            };
            bool canAssign = hasSelection && runningStatuses.Contains(selected.Status);
            btnAssignDriver.Enabled = canAssign; // Giả sử nút tên là btnAssignDriver
            // btnSearch, btnReload thường luôn bật
        }

        // ... (SelectRowById giữ nguyên) ...
        private void SelectRowById(int shipmentId)
        {
            if (dgvShipments.Rows.Count == 0 || _bindingList == null) return;
            ShipmentListItemAdminDto itemToSelect = _bindingList.FirstOrDefault(s => s.Id == shipmentId);
            if (itemToSelect != null)
            {
                int rowIndex = _bindingList.IndexOf(itemToSelect);
                if (rowIndex >= 0 && rowIndex < dgvShipments.Rows.Count)
                {
                    dgvShipments.ClearSelection();
                    dgvShipments.Rows[rowIndex].Selected = true;
                    if (dgvShipments.Columns.Contains("ShipmentNo"))
                        dgvShipments.CurrentCell = dgvShipments.Rows[rowIndex].Cells["ShipmentNo"];

                    if (!dgvShipments.Rows[rowIndex].Displayed)
                    {
                        int firstDisplayed = Math.Max(0, rowIndex - dgvShipments.DisplayedRowCount(false) / 2);
                        dgvShipments.FirstDisplayedScrollingRowIndex = Math.Min(firstDisplayed, dgvShipments.RowCount - 1);
                    }
                }
            }
            UpdateButtonsState();
        }
        #endregion

        #region Wire Events
        // ... (WireEvents giữ nguyên, chỉ đảm bảo có gán sự kiện cho btnAssignDriver.Click) ...
        private void WireEvents()
        {
            btnReload.Click += (s, e) => LoadData();
            btnSearch.Click += (s, e) => OpenSearchPopup();
            btnViewDetail.Click += (s, e) => OpenDetailPopup();
            btnAssignDriver.Click += (s, e) => AssignNewDriver(); // *** Đảm bảo dòng này tồn tại ***

            dgvShipments.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenDetailPopup(); };
            dgvShipments.ColumnHeaderMouseClick += dgvShipments_ColumnHeaderMouseClick;
            dgvShipments.SelectionChanged += (s, e) => UpdateButtonsState();
        }
        #endregion

        #region Actions
        private void OpenSearchPopup()
        {
            int? selectedId = null;
            using (var searchForm = new Form
            {
                Text = "Tìm kiếm Chuyến hàng",
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new Size(1186, 739)
            })
            {
                var ucSearch = new ucShipmentSearch_Admin { Dock = DockStyle.Fill };
                ucSearch.ShipmentPicked += (sender, id) =>
                {
                    selectedId = id;
                    searchForm.DialogResult = DialogResult.OK;
                    searchForm.Close();
                };
                searchForm.Controls.Add(ucSearch);
                Form ownerForm = this.FindForm();
                if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }
                if (searchForm.ShowDialog(ownerForm) == DialogResult.OK && selectedId.HasValue)
                {
                    SelectRowById(selectedId.Value);
                }
            }
        }
        private void OpenDetailPopup()
        {
            var selected = GetSelectedShipment();
            if (selected == null) { MessageBox.Show("Vui lòng chọn chuyến hàng.", "Thông báo"); return; }
            try
            {
                var ucDetail = new ucShipmentDetail_Admin(selected.Id);
                using (var popupForm = new Form
                {
                    Text = $"Chi tiết chuyến hàng {selected.ShipmentNo}",
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.None,
                    Size = new Size(1430, 786) // *** SỬA SIZE CHO PHÙ HỢP UC DETAIL ***
                })
                {
                    ucDetail.Dock = DockStyle.Fill;
                    popupForm.Controls.Add(ucDetail);
                    Form ownerForm = this.FindForm();
                    if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }
                    popupForm.ShowDialog(ownerForm);
                    // LoadData(); // Load lại nếu Detail có thể thay đổi dữ liệu
                }
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi mở chi tiết: {ex.Message}", "Lỗi"); }
        }

        private void AssignNewDriver()
        {
            var selectedShipment = GetSelectedShipment();
            // Kiểm tra null và nút có đang bật không (để tránh lỗi logic)
            if (selectedShipment == null || !btnAssignDriver.Enabled)
            {
                MessageBox.Show("Vui lòng chọn chuyến hàng đang hoạt động để đổi tài xế.", "Thông báo");
                return;
            }

            int? selectedDriverId = null;
            using (var fPicker = new Form
            {
                Text = "Chọn Tài Xế Mới (Chỉ hiển thị tài xế rảnh)", // Cập nhật tiêu đề
                StartPosition = FormStartPosition.CenterScreen,
                Size = new Size(583, 539), // Size của ucDriverPicker
                FormBorderStyle = FormBorderStyle.None,
            })
            {
                var ucPicker = new ucDriverPicker_Admin(); // Nó sẽ tự gọi hàm lấy tài xế rảnh trong Load
                ucPicker.DriverSelected += (selectedId) =>
                {
                    if (selectedId == selectedShipment.DriverId)
                    {
                        MessageBox.Show("Bạn đã chọn lại tài xế hiện tại.", "Thông báo");
                        return; // Không làm gì cả
                    }
                    selectedDriverId = selectedId;
                    fPicker.DialogResult = DialogResult.OK;
                };
                fPicker.Controls.Add(ucPicker);
                Form ownerForm = this.FindForm();
                if (ownerForm == null) { MessageBox.Show("Không thể xác định Form cha.", "Lỗi"); return; }

                if (fPicker.ShowDialog(ownerForm) == DialogResult.OK && selectedDriverId.HasValue)
                {
                    if (MessageBox.Show($"Bạn có chắc muốn đổi tài xế cho chuyến {selectedShipment.ShipmentNo} thành tài xế ID {selectedDriverId.Value}?\nTrạng thái chuyến sẽ được đặt lại thành 'Chờ nhận'.",
                                        "Xác nhận đổi tài xế", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            _orderSvc.AssignDriver(selectedShipment.Id, selectedDriverId.Value /*, reason: "Admin đổi tài xế"*/ ); // Có thể thêm lý do
                            MessageBox.Show("Đã đổi tài xế thành công.\nTài xế mới cần vào ứng dụng để 'Nhận chuyến'.", "Thông báo");
                            LoadData(); // Tải lại grid
                            SelectRowById(selectedShipment.Id); // Chọn lại dòng vừa đổi
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi đổi tài xế: {ex.Message}", "Lỗi");
                        }
                    }
                }
            }
        }
        #endregion

        #region Sorting Logic
        private void dgvShipments_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_bindingList == null || _bindingList.Count == 0) return;
            var newColumn = dgvShipments.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_sortedColumn == newColumn) { _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
            else { if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _sortOrder = SortOrder.Ascending; _sortedColumn = newColumn; }

            ApplySort();
            if (_sortedColumn != null) _sortedColumn.HeaderCell.SortGlyphDirection = _sortOrder;
        }

        private void ApplySort()
        {
            if (_sortedColumn == null || _bindingList == null) return;

            string propertyName = _sortedColumn.DataPropertyName;
            List<ShipmentListItemAdminDto> items = _bindingList.ToList();
            List<ShipmentListItemAdminDto> sortedList;

            try
            {
                if (propertyName == "OrderNo")
                {
                    if (_sortOrder == SortOrder.Ascending)
                    {
                        sortedList = items.OrderBy(item => OrderCode.TryParseId(item.OrderNo, out int id) ? id : int.MaxValue).ToList();
                    }
                    else
                    {
                        sortedList = items.OrderByDescending(item => OrderCode.TryParseId(item.OrderNo, out int id) ? id : int.MinValue).ToList();
                    }
                }
                else
                {
                    var propInfo = typeof(ShipmentListItemAdminDto).GetProperty(propertyName);
                    if (propInfo == null) return;

                    if (_sortOrder == SortOrder.Ascending)
                    {
                        sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList();
                    }
                    else
                    {
                        sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();
                    }
                }

                _bindingList = new BindingList<ShipmentListItemAdminDto>(sortedList);
                dgvShipments.DataSource = _bindingList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi sắp xếp cột '{_sortedColumn.HeaderText}': {ex.Message}");
                ResetSortGlyphs();
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn != null) { _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; }
            _sortedColumn = null; _sortOrder = SortOrder.None;
            foreach (DataGridViewColumn col in dgvShipments.Columns) { if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None; }
        }
        #endregion

        #region Cell Formatting
        private void DgvShipments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvShipments.Columns[e.ColumnIndex].Name == "Status" && e.Value is ShipmentStatus status)
            {
                switch (status)
                {
                    case ShipmentStatus.Pending: e.Value = "Chờ nhận"; break;
                    case ShipmentStatus.Assigned: e.Value = "Đã nhận"; break;
                    case ShipmentStatus.OnRoute: e.Value = "Đang đi đường"; break;
                    case ShipmentStatus.AtWarehouse: e.Value = "Đang ở kho"; break;
                    case ShipmentStatus.ArrivedDestination: e.Value = "Đã tới đích"; break;
                    case ShipmentStatus.Delivered: e.Value = "Đã giao xong"; break;
                    case ShipmentStatus.Failed: e.Value = "Gặp sự cố"; break;
                    default: e.Value = status.ToString(); break;
                }
                e.FormattingApplied = true;
            }
        }
        #endregion

    }
}