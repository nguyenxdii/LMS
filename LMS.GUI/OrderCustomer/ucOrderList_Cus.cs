using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.GUI.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel; // Cần cho SortOrder
using System.Drawing;
using System.Linq;
using System.Reflection; // Cần cho Reflection (khi sort)
using System.Windows.Forms;

namespace LMS.GUI.OrderCustomer
{
    public partial class ucOrderList_Cus : UserControl
    {
        private readonly int _customerId;
        private readonly OrderService_Customer _orderSvc = new OrderService_Customer();
        private DataGridViewColumn _sortedColumn = null; // Cột đang được sort
        private SortOrder _sortOrder = SortOrder.None; // Hướng sort hiện tại

        public ucOrderList_Cus(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;

            this.Load += UcOrderList_Cus_Load;

            // Gán sự kiện Click
            btnReload.Click += BtnReload_Click;
            btnTrack.Click += (s, e) => OpenTrack();
            dgvOrders.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenTrack(); };
        }

        private void UcOrderList_Cus_Load(object sender, EventArgs e)
        {
            SetupStatusFilter();
            ConfigureOrderGrid();
            LoadData(); // Tải dữ liệu lần đầu

            // Gán sự kiện cho DataGridView
            dgvOrders.ColumnHeaderMouseClick += dgvOrders_ColumnHeaderMouseClick;
            dgvOrders.CellFormatting += dgvOrders_CellFormatting;
        }

        // Cài đặt ComboBox lọc trạng thái
        private void SetupStatusFilter()
        {
            cmbStatusFilter.Items.Clear();
            var statusItems = new List<StatusFilterItem>
            {
                new StatusFilterItem { Value = null, Text = "— Tất cả —" } // "Tất cả" có giá trị null
            };
            // Thêm các trạng thái với tên tiếng Việt
            statusItems.Add(new StatusFilterItem { Value = OrderStatus.Pending, Text = "Chờ duyệt" });
            statusItems.Add(new StatusFilterItem { Value = OrderStatus.Approved, Text = "Đã duyệt" });
            statusItems.Add(new StatusFilterItem { Value = OrderStatus.Completed, Text = "Hoàn thành" });
            statusItems.Add(new StatusFilterItem { Value = OrderStatus.Cancelled, Text = "Đã hủy" });

            cmbStatusFilter.DataSource = statusItems;
            cmbStatusFilter.DisplayMember = "Text";
            cmbStatusFilter.ValueMember = "Value";

            // Tự động tải lại dữ liệu khi đổi filter
            cmbStatusFilter.SelectedIndexChanged += (s, ev) => LoadData();
        }

        // Cấu hình giao diện DataGridView
        private void ConfigureOrderGrid()
        {
            var g = dgvOrders;
            g.Columns.Clear();
            g.ApplyBaseStyle();

            // --- Định nghĩa Cột ---
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false }); // Cột ID ẩn
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo", DataPropertyName = "OrderNo", HeaderText = "Mã đơn", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", DataPropertyName = "CreatedAt", HeaderText = "Ngày tạo", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OriginWarehouseName", DataPropertyName = "OriginWarehouseName", HeaderText = "Kho gửi", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "DestWarehouseName", DataPropertyName = "DestWarehouseName", HeaderText = "Kho nhận", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalFee", DataPropertyName = "TotalFee", HeaderText = "Tổng phí", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });

            TryEnableDoubleBuffer(g); // Tăng tốc độ vẽ
        }

        private static void TryEnableDoubleBuffer(DataGridView grid)
        {
            try
            {
                var prop = grid.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                prop?.SetValue(grid, true, null);
            }
            catch { /* Bỏ qua lỗi */ }
        }

        private void LoadData()
        {
            OrderStatus? status = (cmbStatusFilter.SelectedItem as StatusFilterItem)?.Value;

            var list = _orderSvc.GetOrdersByCustomer(_customerId, status)
                                .Select(o => new
                                {
                                    o.Id,
                                    OrderNo = string.IsNullOrWhiteSpace(o.OrderNo) ? OrderCode.ToCode(o.Id) : o.OrderNo,
                                    o.CreatedAt,
                                    OriginWarehouseName = o.OriginWarehouse?.Name ?? $"#{o.OriginWarehouseId}",
                                    DestWarehouseName = o.DestWarehouse?.Name ?? $"#{o.DestWarehouseId}",
                                    o.TotalFee,
                                    o.Status // Giữ giá trị Enum để format và sort
                                })
                                .ToList(); // Thực thi truy vấn

            dgvOrders.DataSource = null; // Xóa DataSource cũ để refresh
            dgvOrders.DataSource = list;

            dgvOrders.ScrollBars = ScrollBars.Both; // Hiển thị thanh cuộn nếu cần

            ResetSortGlyphs(); // Xóa mũi tên sort khi tải lại
        }

        private int? GetSelectedOrderId()
        {
            if (dgvOrders.CurrentRow?.Cells["Id"]?.Value != null &&
                int.TryParse(dgvOrders.CurrentRow.Cells["Id"].Value.ToString(), out int id))
            {
                return id;
            }
            return null;
        }

        // Mở màn hình theo dõi đơn hàng
        private void OpenTrack()
        {
            var orderId = GetSelectedOrderId();
            if (orderId == null)
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để theo dõi.", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var host = this.FindForm() as frmMain_Customer;
            host?.LoadUc(new ucTracking_Cus(_customerId, orderId.Value));
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            if (cmbStatusFilter.Items.Count > 0)
            {
                cmbStatusFilter.SelectedIndex = 0; // Đặt lại filter về "Tất cả"
            }
        }


        private void dgvOrders_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var list = dgvOrders.DataSource as IEnumerable<object>;
            if (list == null || !list.Any()) return;

            var newColumn = dgvOrders.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            // Xác định hướng sort
            if (_sortedColumn == null) _sortOrder = SortOrder.Ascending;
            else if (_sortedColumn == newColumn) _sortOrder = (_sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            else { _sortOrder = SortOrder.Ascending; if (_sortedColumn?.HeaderCell != null) _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; }

            _sortedColumn = newColumn;

            try
            {
                string propertyName = newColumn.DataPropertyName;
                var propInfo = list.First().GetType().GetProperty(propertyName);
                if (propInfo == null) return;

                IEnumerable<object> sortedList;
                if (_sortOrder == SortOrder.Ascending)
                    sortedList = list.OrderBy(x => propInfo.GetValue(x, null));
                else
                    sortedList = list.OrderByDescending(x => propInfo.GetValue(x, null));

                dgvOrders.DataSource = null;
                dgvOrders.DataSource = sortedList.ToList();
                newColumn.HeaderCell.SortGlyphDirection = _sortOrder;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sort Error on {newColumn.Name}: {ex.Message}");
                ResetSortGlyphs(); // Reset nếu lỗi
            }
        }

        private void ResetSortGlyphs()
        {
            if (_sortedColumn?.HeaderCell != null)
            {
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            _sortedColumn = null;
            _sortOrder = SortOrder.None;
        }

        private void dgvOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvOrders.Columns[e.ColumnIndex].Name == "Status" && e.Value is OrderStatus status)
            {
                switch (status)
                {
                    case OrderStatus.Pending: e.Value = "Chờ duyệt"; break;
                    case OrderStatus.Approved: e.Value = "Đã duyệt"; break;
                    case OrderStatus.Completed: e.Value = "Hoàn thành"; break;
                    case OrderStatus.Cancelled: e.Value = "Đã hủy"; break;
                    default: e.Value = status.ToString(); break;
                }
                e.FormattingApplied = true;
            }
        }

        public class StatusFilterItem
        {
            public OrderStatus? Value { get; set; } // Dùng kiểu nullable để đại diện cho "Tất cả"
            public string Text { get; set; }
            public override string ToString() => Text; // Hiển thị Text trong ComboBox
        }
    }
}