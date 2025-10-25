//// LMS.GUI/OrderDriver/ucMyShipments_Drv.cs
//using LMS.BUS.Helpers; // Cần cho GridHelper và AppSession
//using LMS.BUS.Services;
//using LMS.DAL.Models;
//using System;
//using System.Collections.Generic; // Cần cho BindingList
//using System.ComponentModel; // Cần cho BindingList, SortOrder
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Reflection; // Cần cho Reflection (Sort)
//using System.Windows.Forms;
//using LMS.BUS.Dtos; // Cần cho ShipmentRowDto
//using LMS.GUI.Main; // Cần để cast FindForm()

//namespace LMS.GUI.OrderDriver
//{
//    public partial class ucMyShipments_Drv : UserControl
//    {
//        private readonly DriverShipmentService _svc = new DriverShipmentService();
//        // Không cần biến sort nếu không implement sort

//        // Constructor mặc định (dùng AppSession)
//        public ucMyShipments_Drv() : this(AppSession.DriverId ?? 0) { }

//        // Constructor nhận driverId (nếu cần truyền từ form cha)
//        public ucMyShipments_Drv(int driverId)
//        {
//            InitializeComponent();
//            // _driverId = driverId; // Lưu lại nếu không dùng AppSession trực tiếp
//            this.Load += UcMyShipments_Drv_Load;
//        }


//        private void UcMyShipments_Drv_Load(object sender, EventArgs e)
//        {
//            if (DriverId <= 0)
//            {
//                MessageBox.Show("Không thể xác định tài khoản tài xế.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                // Disable controls or handle appropriately
//                return;
//            }
//            ConfigureGrids();
//            InitFilters();
//            LoadActive();
//            LoadAll();
//            Wire();
//        }

//        private void ConfigureGrids()
//        {
//            // ===== ACTIVE =====
//            var gActive = dgvActive;
//            gActive.Columns.Clear();
//            gActive.ApplyBaseStyle(); // Áp dụng style chung

//            gActive.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
//            gActive.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShipmentNo", DataPropertyName = "ShipmentNo", HeaderText = "Mã CH", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            gActive.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo", DataPropertyName = "OrderNo", HeaderText = "Mã đơn", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            gActive.Columns.Add(new DataGridViewTextBoxColumn { Name = "Route", DataPropertyName = "Route", HeaderText = "Tuyến", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
//            gActive.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            gActive.Columns.Add(new DataGridViewTextBoxColumn { Name = "UpdatedAt", DataPropertyName = "UpdatedAt", HeaderText = "Cập nhật", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });

//            // ===== ALL =====
//            var gAll = dgvAll;
//            gAll.Columns.Clear();
//            gAll.ApplyBaseStyle(); // Áp dụng style chung

//            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
//            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShipmentNo", DataPropertyName = "ShipmentNo", HeaderText = "Mã CH", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo", DataPropertyName = "OrderNo", HeaderText = "Mã đơn", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Route", DataPropertyName = "Route", HeaderText = "Tuyến", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Stops", DataPropertyName = "Stops", HeaderText = "Số chặng", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
//            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "StartedAt", DataPropertyName = "StartedAt", HeaderText = "Bắt đầu", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
//            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "DeliveredAt", DataPropertyName = "DeliveredAt", HeaderText = "Kết thúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
//            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Duration", DataPropertyName = "Duration", HeaderText = "Thời gian", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = @"hh\:mm\:ss", Alignment = DataGridViewContentAlignment.MiddleCenter, NullValue = "" } });

//            // Gán sự kiện Double Click
//            dgvActive.CellDoubleClick += (s, e) => OpenDetailFromGrid(dgvActive, e.RowIndex);
//            dgvAll.CellDoubleClick += (s, e) => OpenDetailFromGrid(dgvAll, e.RowIndex);
//        }

//        private int DriverId => AppSession.DriverId ?? 0;

//        private void InitFilters()
//        {
//            dtFrom.Value = DateTime.Today.AddDays(-30);
//            dtTo.Value = DateTime.Today.AddDays(1).AddSeconds(-1);

//            cmbStatus.Items.Clear();
//            var statusItems = new List<ShipmentStatusFilterItem>
//            {
//                new ShipmentStatusFilterItem { Value = null, Text = "— Tất cả —" }
//            };
//            statusItems.Add(new ShipmentStatusFilterItem { Value = ShipmentStatus.Pending, Text = "Chờ nhận" });
//            statusItems.Add(new ShipmentStatusFilterItem { Value = ShipmentStatus.Assigned, Text = "Đã nhận" });
//            statusItems.Add(new ShipmentStatusFilterItem { Value = ShipmentStatus.OnRoute, Text = "Đang đi đường" });
//            statusItems.Add(new ShipmentStatusFilterItem { Value = ShipmentStatus.AtWarehouse, Text = "Đang ở kho" });
//            statusItems.Add(new ShipmentStatusFilterItem { Value = ShipmentStatus.ArrivedDestination, Text = "Đã tới đích" });
//            statusItems.Add(new ShipmentStatusFilterItem { Value = ShipmentStatus.Delivered, Text = "Đã giao xong" });
//            statusItems.Add(new ShipmentStatusFilterItem { Value = ShipmentStatus.Failed, Text = "Gặp sự cố" });

//            cmbStatus.DataSource = statusItems;
//            cmbStatus.DisplayMember = "Text";
//            cmbStatus.ValueMember = "Value";
//        }

//        private void Wire()
//        {
//            btnReload.Click += BtnReload_Click;
//            btnReceive.Click += (s, e) => ReceiveSelected();

//            dtFrom.ValueChanged += (s, e) => LoadAll();
//            dtTo.ValueChanged += (s, e) => LoadAll();
//            cmbStatus.SelectedIndexChanged += (s, e) => LoadAll();

//            dgvActive.SelectionChanged += (s, e) => UpdateButtons();
//            dgvAll.SelectionChanged += (s, e) => UpdateButtons();
//            // Bỏ dòng liên quan đến tabControl1
//            // tabControl1.SelectedIndexChanged += (s, e) => UpdateButtons();

//            dgvActive.CellFormatting += DgvShipments_CellFormatting;
//            dgvAll.CellFormatting += DgvShipments_CellFormatting;
//        }

//        private void LoadActive()
//        {
//            try
//            {
//                var data = _svc.GetAssignedAndRunning(DriverId);
//                dgvActive.DataSource = new BindingList<ShipmentRowDto>(data);
//                UpdateButtons();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tải danh sách chuyến đang chạy: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                dgvActive.DataSource = null;
//            }
//        }

//        private void LoadAll()
//        {
//            try
//            {
//                DateTime? from = dtFrom.Value.Date;
//                DateTime? to = dtTo.Value.Date.AddDays(1).AddTicks(-1);
//                ShipmentStatus? st = (cmbStatus.SelectedItem as ShipmentStatusFilterItem)?.Value;

//                var data = _svc.GetAllMine(DriverId, from, to, st);
//                dgvAll.DataSource = new BindingList<ShipmentRowDto>(data);
//                UpdateButtons();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tải danh sách tất cả chuyến: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                dgvAll.DataSource = null;
//            }
//        }

//        // Lấy thông tin Shipment từ dòng đang chọn (ưu tiên Active)
//        private (int id, string status)? GetCurrentShipmentInfo()
//        {
//            // Thử lấy từ grid đang active trước
//            var activeSelection = CurrentShipmentFrom(dgvActive);
//            if (activeSelection != null) return activeSelection;

//            // Nếu không có trong active, thử lấy từ grid all
//            return CurrentShipmentFrom(dgvAll);
//        }

//        // Helper lấy thông tin từ grid cụ thể
//        private (int id, string status)? CurrentShipmentFrom(DataGridView grid)
//        {
//            var it = grid.CurrentRow?.DataBoundItem as ShipmentRowDto;
//            return it != null ? (it.Id, it.Status) : ((int, string)?)null;
//        }


//        private void ReceiveSelected()
//        {
//            // Lấy thông tin chuyến hàng đang được chọn (ưu tiên active)
//            var cur = GetCurrentShipmentInfo();

//            if (cur == null)
//            {
//                MessageBox.Show("Vui lòng chọn một chuyến hàng để nhận.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                return;
//            }

//            try
//            {
//                if (cur.Value.status != ShipmentStatus.Pending.ToString())
//                {
//                    MessageBox.Show("Chỉ có thể nhận chuyến hàng đang ở trạng thái 'Chờ nhận' (Pending).", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                    return;
//                }

//                _svc.ReceiveShipment(cur.Value.id, DriverId);

//                LoadActive();
//                LoadAll();
//                MessageBox.Show("Đã nhận chuyến hàng thành công.", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi nhận chuyến hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        // Cập nhật trạng thái nút Receive dựa trên lựa chọn hiện tại
//        private void UpdateButtons()
//        {
//            var cur = GetCurrentShipmentInfo(); // Lấy thông tin chuyến đang chọn
//            btnReceive.Enabled = cur != null && cur.Value.status == ShipmentStatus.Pending.ToString();
//        }

//        private void OpenDetailFromGrid(DataGridView grid, int rowIndex)
//        {
//            if (rowIndex < 0) return;
//            var it = grid.Rows[rowIndex].DataBoundItem as ShipmentRowDto;
//            if (it == null) return;
//            int id = it.Id;

//            using (var f = new Form
//            {
//                Text = $"Chi tiết chuyến hàng SHP{id}",
//                StartPosition = FormStartPosition.CenterParent,
//                Size = new Size(1000, 700)
//            })
//            {
//                var uc = new ucShipmentDetail_Drv { Dock = DockStyle.Fill };
//                uc.LoadShipment(id);
//                f.Controls.Add(uc);
//                f.ShowDialog(this.FindForm());
//            }

//            LoadActive();
//            LoadAll();
//        }

//        // Xử lý nút Reload
//        private void BtnReload_Click(object sender, EventArgs e)
//        {
//            if (cmbStatus.Items.Count > 0)
//            {
//                cmbStatus.SelectedIndex = 0; // Reset filter về "Tất cả"
//            }
//            LoadActive(); // Luôn tải lại tab active

//            // Nếu filter đã là "Tất cả", gọi LoadAll thủ công vì SelectedIndexChanged không chạy
//            if (cmbStatus.SelectedIndex == 0)
//            {
//                LoadAll();
//            }
//            // Nếu không, LoadAll() sẽ tự chạy do sự kiện SelectedIndexChanged
//        }

//        // Format Status trong cả 2 grid sang tiếng Việt
//        private void DgvShipments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            var grid = sender as DataGridView;
//            if (grid == null) return;

//            if (grid.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
//            {
//                if (Enum.TryParse<ShipmentStatus>(e.Value.ToString(), out var status))
//                {
//                    switch (status)
//                    {
//                        case ShipmentStatus.Pending: e.Value = "Chờ nhận"; break;
//                        case ShipmentStatus.Assigned: e.Value = "Đã nhận"; break;
//                        case ShipmentStatus.OnRoute: e.Value = "Đang đi đường"; break;
//                        case ShipmentStatus.AtWarehouse: e.Value = "Đang ở kho"; break;
//                        case ShipmentStatus.ArrivedDestination: e.Value = "Đã tới đích"; break;
//                        case ShipmentStatus.Delivered: e.Value = "Đã giao xong"; break;
//                        case ShipmentStatus.Failed: e.Value = "Gặp sự cố"; break;
//                        default: e.Value = status.ToString(); break;
//                    }
//                    e.FormattingApplied = true;
//                }
//            }
//        }

//        // Lớp helper cho ComboBox Filter Status
//        public class ShipmentStatusFilterItem
//        {
//            public ShipmentStatus? Value { get; set; }
//            public string Text { get; set; }
//            public override string ToString() => Text;
//        }
//    }
//}

/// LMS.GUI/OrderDriver/ucMyShipments_Drv.cs
using LMS.BUS.Helpers; // Cần cho GridHelper và AppSession
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Collections.Generic; // Cần cho BindingList
using System.ComponentModel; // Cần cho BindingList, SortOrder, TypeDescriptor
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using LMS.BUS.Dtos; // Cần cho ShipmentRowDto
using LMS.GUI.Main; // Cần để cast FindForm()

namespace LMS.GUI.OrderDriver
{
    public partial class ucMyShipments_Drv : UserControl
    {
        private readonly DriverShipmentService _svc = new DriverShipmentService();
        private int DriverId => AppSession.DriverId ?? 0;

        // THÊM: Biến để theo dõi sắp xếp
        private string _sortProperty = string.Empty;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;

        // Constructor mặc định (dùng AppSession)
        public ucMyShipments_Drv() : this(AppSession.DriverId ?? 0) { }

        // Constructor nhận driverId (nếu cần truyền từ form cha)
        public ucMyShipments_Drv(int driverId)
        {
            InitializeComponent();
            this.Load += UcMyShipments_Drv_Load;
        }

        private void UcMyShipments_Drv_Load(object sender, EventArgs e)
        {
            if (DriverId <= 0)
            {
                MessageBox.Show("Không thể xác định tài khoản tài xế.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ConfigureGrid();
            LoadAll();
            Wire();
        }

        private void ConfigureGrid()
        {
            var gAll = dgvAll;
            gAll.Columns.Clear();
            gAll.ApplyBaseStyle();

            // Định nghĩa các cột
            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShipmentNo", DataPropertyName = "ShipmentNo", HeaderText = "Mã CH", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo", DataPropertyName = "OrderNo", HeaderText = "Mã đơn", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Route", DataPropertyName = "Route", HeaderText = "Tuyến", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Stops", DataPropertyName = "Stops", HeaderText = "Số chặng", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "StartedAt", DataPropertyName = "StartedAt", HeaderText = "Bắt đầu", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "DeliveredAt", DataPropertyName = "DeliveredAt", HeaderText = "Kết thúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Duration", DataPropertyName = "Duration", HeaderText = "Thời gian", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = @"hh\:mm\:ss", Alignment = DataGridViewContentAlignment.MiddleCenter, NullValue = "" } });

            // THÊM: Gán sự kiện Click Header để Sort và Double Click để mở
            gAll.ColumnHeaderMouseClick += DgvAll_ColumnHeaderMouseClick;
            gAll.CellDoubleClick += DgvAll_CellDoubleClick;
        }

        private void Wire()
        {
            btnReload.Click += BtnReload_Click;
            btnReceive.Click += (s, e) => ReceiveSelected();
            dgvAll.SelectionChanged += (s, e) => UpdateButtons();
            dgvAll.CellFormatting += DgvShipments_CellFormatting;
        }

        private void LoadAll()
        {
            try
            {
                var data = _svc.GetAllMine(DriverId, null, null, null);
                var bindingList = new BindingList<ShipmentRowDto>(data);

                // SỬA: Áp dụng lại sắp xếp hiện tại (nếu có) sau khi tải
                if (!string.IsNullOrEmpty(_sortProperty))
                {
                    ApplySort(bindingList, _sortProperty, _sortDirection);
                }

                dgvAll.DataSource = bindingList;
                UpdateButtons();

                // SỬA: Cập nhật mũi tên sắp xếp trên header
                UpdateSortGlyphs(dgvAll);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách tất cả chuyến: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvAll.DataSource = null;
            }
        }

        private (int id, string status)? GetCurrentShipmentInfo()
        {
            var it = dgvAll.CurrentRow?.DataBoundItem as ShipmentRowDto;
            return it != null ? (it.Id, it.Status) : ((int, string)?)null;
        }

        private void ReceiveSelected()
        {
            var cur = GetCurrentShipmentInfo();

            if (cur == null)
            {
                MessageBox.Show("Vui lòng chọn một chuyến hàng để nhận.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cur.Value.status != ShipmentStatus.Pending.ToString())
            {
                MessageBox.Show("Chỉ có thể nhận chuyến hàng đang ở trạng thái 'Chờ nhận' (Pending).", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _svc.ReceiveShipment(cur.Value.id, DriverId);
                MessageBox.Show("Đã nhận chuyến hàng thành công.", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAll();
                OpenActiveShipmentsScreen(); // Chuyển sang màn hình "đang chạy"
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi nhận chuyến hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenActiveShipmentsScreen()
        {
            // 1. Tìm Form cha (frmMain_Driver) đang chứa UserControl này
            var mainForm = this.FindForm() as frmMain_Driver;

            if (mainForm != null)
            {
                // 2. Gọi hàm public mới của Form cha
                mainForm.NavigateToShipmentDetail();
            }
            else
            {
                // Fallback phòng trường hợp không tìm thấy Form cha
                MessageBox.Show("Lỗi điều hướng: Không tìm thấy Form chính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // 3. Tải lại grid này (để cập nhật nếu user quay lại)
            // Bạn có thể bỏ dòng này nếu thấy không cần thiết
            LoadAll();
        }

        private void UpdateButtons()
        {
            var cur = GetCurrentShipmentInfo();
            btnReceive.Enabled = cur != null && cur.Value.status == ShipmentStatus.Pending.ToString();
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            // THÊM: Reset sort khi reload
            _sortProperty = string.Empty;
            _sortDirection = ListSortDirection.Ascending;
            LoadAll();
        }

        // (Hàm DgvShipments_CellFormatting giữ nguyên)
        private void DgvShipments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null || e.Value == null) return;

            if (grid.Columns[e.ColumnIndex].DataPropertyName == "Status")
            {
                if (Enum.TryParse<ShipmentStatus>(e.Value.ToString(), out var status))
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
        }

        // ===== THÊM: CÁC HÀM HỖ TRỢ SORT VÀ DOUBLE CLICK =====

        /// <summary>
        /// Xử lý sự kiện Double Click: Mở màn hình active
        /// </summary>
        private void DgvAll_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Bỏ qua nếu click header

            // Theo yêu cầu: double click cũng mở màn hình "Các chuyến đang thực hiện"
            OpenActiveShipmentsScreen();
        }

        /// <summary>
        /// Xử lý sự kiện click header để sắp xếp
        /// </summary>
        private void DgvAll_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = (DataGridView)sender;
            string newSortProperty = grid.Columns[e.ColumnIndex].DataPropertyName;

            if (string.IsNullOrEmpty(newSortProperty)) return; // Cột không có DataPropertyName

            // Đảo chiều nếu click cùng cột
            if (_sortProperty == newSortProperty)
            {
                _sortDirection = (_sortDirection == ListSortDirection.Ascending)
                         ? ListSortDirection.Descending
                         : ListSortDirection.Ascending;
            }
            else // Cột mới, mặc định tăng dần
            {
                _sortProperty = newSortProperty;
                _sortDirection = ListSortDirection.Ascending;
            }

            // Lấy BindingList từ DataSource
            var data = dgvAll.DataSource as BindingList<ShipmentRowDto>;
            if (data == null) return;

            // Áp dụng sắp xếp
            ApplySort(data, _sortProperty, _sortDirection);

            // Cập nhật mũi tên
            UpdateSortGlyphs(grid);
        }

        /// <summary>
        /// Sắp xếp một BindingList<ShipmentRowDto>
        /// </summary>
        private void ApplySort(BindingList<ShipmentRowDto> data, string property, ListSortDirection direction)
        {
            var prop = TypeDescriptor.GetProperties(typeof(ShipmentRowDto)).Find(property, true);
            if (prop == null) return;

            // Lấy danh sách hiện tại
            var items = data.ToList();

            // Sắp xếp
            var sortedList = (direction == ListSortDirection.Ascending)
              ? items.OrderBy(item => prop.GetValue(item)).ToList()
              : items.OrderByDescending(item => prop.GetValue(item)).ToList();

            // Tải lại BindingList (tắt event để tăng tốc)
            data.RaiseListChangedEvents = false;
            data.Clear();
            foreach (var item in sortedList)
            {
                data.Add(item);
            }
            data.RaiseListChangedEvents = true;
            data.ResetBindings(); // Báo cho DGV cập nhật
        }

        /// <summary>
        /// Cập nhật mũi tên sắp xếp trên header của DGV
        /// </summary>
        private void UpdateSortGlyphs(DataGridView grid)
        {
            foreach (DataGridViewColumn col in grid.Columns)
            {
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
                if (col.DataPropertyName == _sortProperty)
                {
                    col.HeaderCell.SortGlyphDirection = (_sortDirection == ListSortDirection.Ascending)
                                     ? SortOrder.Ascending
                                     : SortOrder.Descending;
                }
            }
        }
    }
}