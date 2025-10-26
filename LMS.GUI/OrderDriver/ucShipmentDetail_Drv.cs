// LMS.GUI/OrderDriver/ucShipmentDetail_Drv.cs
using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

namespace LMS.GUI.OrderDriver
{
    public partial class ucShipmentDetail_Drv : UserControl
    {
        private readonly DriverShipmentService _svc = new DriverShipmentService();
        private int DriverId => AppSession.DriverId ?? 0;

        private string _sortProperty = string.Empty;
        // ĐÃ SỬA LỖI CS0039: Gõ sai tên kiểu dữ liệu
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;

        public ucShipmentDetail_Drv()
        {
            InitializeComponent();
            this.Load += UcShipmentDetail_Drv_Load;
        }

        private void UcShipmentDetail_Drv_Load(object sender, EventArgs e)
        {
            if (DriverId <= 0)
            {
                MessageBox.Show("Không thể xác định tài khoản tài xế.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ConfigureGrid();
            InitFilters();
            LoadShipments();
            Wire();
        }

        private void ConfigureGrid()
        {
            var g = dgvShipments;
            g.Columns.Clear();
            g.ApplyBaseStyle();

            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "ShipmentNo", DataPropertyName = "ShipmentNo", HeaderText = "Mã CH", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrderNo", DataPropertyName = "OrderNo", HeaderText = "Mã đơn", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Route", DataPropertyName = "Route", HeaderText = "Tuyến", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Stops", DataPropertyName = "Stops", HeaderText = "Số chặng", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "UpdatedAt", DataPropertyName = "UpdatedAt", HeaderText = "Cập nhật", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "StartedAt", DataPropertyName = "StartedAt", HeaderText = "Bắt đầu", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            g.Columns.Add(new DataGridViewTextBoxColumn { Name = "Duration", DataPropertyName = "Duration", HeaderText = "Thời gian", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = @"hh\:mm\:ss", Alignment = DataGridViewContentAlignment.MiddleCenter, NullValue = "" } });

            g.ColumnHeaderMouseClick += DgvShipments_ColumnHeaderMouseClick;
            g.CellDoubleClick += DgvShipments_CellDoubleClick;
            g.SelectionChanged += DgvShipments_SelectionChanged;
            g.CellFormatting += DgvShipments_CellFormatting;
        }

        private void InitFilters()
        {
            // Giá trị mặc định (7 ngày gần nhất)
            dtFrom.Value = DateTime.Today.AddDays(-7);
            dtTo.Value = DateTime.Today.AddDays(1).AddSeconds(-1);

            cmbStatus.Items.Clear();
            var statusItems = new List<ShipmentStatusFilterItem>
            {
                new ShipmentStatusFilterItem { Value = null, Text = "— Tất cả —" },
                new ShipmentStatusFilterItem { Value = ShipmentStatus.Pending, Text = "Chờ nhận" },
                new ShipmentStatusFilterItem { Value = ShipmentStatus.Assigned, Text = "Đã nhận" },
                new ShipmentStatusFilterItem { Value = ShipmentStatus.OnRoute, Text = "Đang đi đường" },
                new ShipmentStatusFilterItem { Value = ShipmentStatus.AtWarehouse, Text = "Đang ở kho" },
                new ShipmentStatusFilterItem { Value = ShipmentStatus.ArrivedDestination, Text = "Đã tới đích" },
                new ShipmentStatusFilterItem { Value = ShipmentStatus.Delivered, Text = "Đã giao xong" },
                new ShipmentStatusFilterItem { Value = ShipmentStatus.Failed, Text = "Gặp sự cố" }
            };

            cmbStatus.DataSource = statusItems;
            cmbStatus.DisplayMember = "Text";
            cmbStatus.ValueMember = "Value";
        }

        private void Wire()
        {
            btnReload.Click += BtnReload_Click;
            btnManageShipment.Click += (s, e) => OpenShipmentRunPopup();

            dtFrom.ValueChanged += (s, e) => LoadShipments();
            dtTo.ValueChanged += (s, e) => LoadShipments();
            cmbStatus.SelectedIndexChanged += (s, e) => LoadShipments();
        }

        /// <summary>
        /// Xử lý nút Tải lại: Reset filter về "Tất cả" và tải lại dữ liệu.
        /// </summary>
        private void BtnReload_Click(object sender, EventArgs e)
        {
            // 1. Reset Sort
            _sortProperty = string.Empty;
            _sortDirection = ListSortDirection.Ascending;

            // 2. Reset Filter Ngày tháng về mặc định (7 ngày gần nhất)
            dtFrom.Value = DateTime.Today.AddDays(-7);
            dtTo.Value = DateTime.Today.AddDays(1).AddSeconds(-1);

            // 3. Reset Filter Trạng thái về "Tất cả" (index 0)
            if (cmbStatus.SelectedIndex != 0)
            {
                cmbStatus.SelectedIndex = 0;
            }
            else
            {
                // Nếu Index không thay đổi, gọi LoadShipments() thủ công
                LoadShipments();
            }
        }

        private void LoadShipments()
        {
            try
            {
                DateTime? from = dtFrom.Value.Date;
                DateTime? to = dtTo.Value.Date.AddDays(1).AddTicks(-1);
                ShipmentStatus? st = (cmbStatus.SelectedItem as ShipmentStatusFilterItem)?.Value;

                List<ShipmentRowDto> data;

                // Lấy tất cả chuyến (Sử dụng các filter đã chọn)
                data = _svc.GetAllMine(DriverId, from, to, st);

                var bindingList = new BindingList<ShipmentRowDto>(data);

                if (!string.IsNullOrEmpty(_sortProperty))
                {
                    ApplySort(bindingList, _sortProperty, _sortDirection);
                }

                dgvShipments.DataSource = bindingList;
                UpdateButtons();
                UpdateSortGlyphs(dgvShipments);

                if (dgvShipments.Rows.Count > 0)
                {
                    dgvShipments.CurrentCell = dgvShipments.Rows[0].Cells["ShipmentNo"];
                }
                else
                {
                    UpdateLabels(null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách chuyến: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvShipments.DataSource = null;
            }
        }

        private int? GetCurrentShipmentId()
        {
            var it = dgvShipments.CurrentRow?.DataBoundItem as ShipmentRowDto;
            return it?.Id;
        }

        private void DgvShipments_SelectionChanged(object sender, EventArgs e)
        {
            var it = dgvShipments.CurrentRow?.DataBoundItem as ShipmentRowDto;
            UpdateLabels(it);
            UpdateButtons();
        }

        private void UpdateLabels(ShipmentRowDto dto)
        {
            if (dto != null)
            {
                string statusText = FormatShipmentStatus(dto.Status);
                string startedAtText = dto.StartedAt?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa bắt đầu";

                lblShipmentNo.Text = $"Mã CH: {dto.ShipmentNo}";
                lblOrderNo.Text = $"Mã đơn: {dto.OrderNo}";

                lblCustomerName.Text = $"Tên khách: {dto.CustomerName ?? "N/A"}";
                lblOriginWarehouse.Text = $"Kho gửi: {dto.OriginWarehouse ?? "N/A"}";
                lblDestinationWarehouse.Text = $"Kho nhận: {dto.DestinationWarehouse ?? "N/A"}";

                lblRoute.Text = $"Tuyến: {dto.Route}";
                lblStatus.Text = $"Trạng thái: {statusText}";
                lblTotalStops.Text = $"Số chặng: {dto.Stops.ToString("N0")}";
                lblStartedAt.Text = $"Bắt đầu: {startedAtText}";
            }
            else
            {
                lblShipmentNo.Text = "Mã CH: N/A";
                lblOrderNo.Text = "Mã đơn: N/A";
                lblCustomerName.Text = "Tên khách: N/A";
                lblOriginWarehouse.Text = "Kho gửi: N/A";
                lblDestinationWarehouse.Text = "Kho nhận: N/A";
                lblRoute.Text = "Tuyến: N/A";
                lblStatus.Text = "Trạng thái: N/A";
                lblTotalStops.Text = "Số chặng: N/A";
                lblStartedAt.Text = "Bắt đầu: N/A";
            }
        }

        private void UpdateButtons()
        {
            btnManageShipment.Enabled = (GetCurrentShipmentId() != null);
        }

        private void DgvShipments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            OpenShipmentRunPopup();
        }

        private void OpenShipmentRunPopup()
        {
            var id = GetCurrentShipmentId();
            if (id == null)
            {
                MessageBox.Show("Vui lòng chọn một chuyến hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var f = new Form
            {
                //Text = $"Thao tác chuyến hàng SHP{id.Value}",
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new Size(1195, 685)
            })
            {
                var uc = new ucShipmentRun_Drv();
                uc.Dock = DockStyle.Fill;
                uc.LoadShipment(id.Value);
                f.Controls.Add(uc);
                f.ShowDialog(this.FindForm());
            }
            LoadShipments();
        }

        private string FormatShipmentStatus(string statusString)
        {
            if (Enum.TryParse<ShipmentStatus>(statusString, out var status))
            {
                switch (status)
                {
                    case ShipmentStatus.Pending: return "Chờ nhận";
                    case ShipmentStatus.Assigned: return "Đã nhận";
                    case ShipmentStatus.OnRoute: return "Đang đi đường";
                    case ShipmentStatus.AtWarehouse: return "Đang ở kho";
                    case ShipmentStatus.ArrivedDestination: return "Đã tới đích";
                    case ShipmentStatus.Delivered: return "Đã giao xong";
                    case ShipmentStatus.Failed: return "Gặp sự cố";
                    default: return status.ToString();
                }
            }
            return statusString;
        }

        private void DgvShipments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null || e.Value == null) return;

            if (grid.Columns[e.ColumnIndex].DataPropertyName == "Status")
            {
                e.Value = FormatShipmentStatus(e.Value.ToString());
                e.FormattingApplied = true;
            }
        }

        // ===== CÁC HÀM HỖ TRỢ SORT (Giữ nguyên) =====
        private void DgvShipments_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = (DataGridView)sender;
            string newSortProperty = grid.Columns[e.ColumnIndex].DataPropertyName;
            if (string.IsNullOrEmpty(newSortProperty)) return;

            if (_sortProperty == newSortProperty)
            {
                _sortDirection = (_sortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }
            else
            {
                _sortProperty = newSortProperty;
                _sortDirection = ListSortDirection.Ascending;
            }

            var data = dgvShipments.DataSource as BindingList<ShipmentRowDto>;
            if (data == null) return;

            ApplySort(data, _sortProperty, _sortDirection);
            UpdateSortGlyphs(grid);
        }

        private void ApplySort(BindingList<ShipmentRowDto> data, string property, ListSortDirection direction)
        {
            var prop = TypeDescriptor.GetProperties(typeof(ShipmentRowDto)).Find(property, true);
            if (prop == null) return;

            var items = data.ToList();
            var sortedList = (direction == ListSortDirection.Ascending)
                           ? items.OrderBy(item => prop.GetValue(item)).ToList()
                           : items.OrderByDescending(item => prop.GetValue(item)).ToList();

            data.RaiseListChangedEvents = false;
            data.Clear();
            foreach (var item in sortedList) data.Add(item);
            data.RaiseListChangedEvents = true;
            data.ResetBindings();
        }

        private void UpdateSortGlyphs(DataGridView grid)
        {
            foreach (DataGridViewColumn col in grid.Columns)
            {
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
                if (col.DataPropertyName == _sortProperty)
                {
                    col.HeaderCell.SortGlyphDirection = (_sortDirection == ListSortDirection.Ascending) ? SortOrder.Ascending : SortOrder.Descending;
                }
            }
        }

        public class ShipmentStatusFilterItem
        {
            public ShipmentStatus? Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }
    }
}