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

        private string _sortProperty = string.Empty;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;

        public ucMyShipments_Drv() : this(AppSession.DriverId ?? 0) { }

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
            gAll.Columns.Add(new DataGridViewTextBoxColumn { Name = "Route", DataPropertyName = "Route", HeaderText = "Tuyến", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
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


        private void DgvAll_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Bỏ qua nếu click header

            // Theo yêu cầu: double click cũng mở màn hình "Các chuyến đang thực hiện"
            OpenActiveShipmentsScreen();
        }


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