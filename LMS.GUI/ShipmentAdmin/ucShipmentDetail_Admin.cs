// LMS.GUI/ShipmentAdmin/ucShipmentDetail_Admin.cs
using Guna.UI2.WinForms;
using LMS.BUS.Dtos; // Cần cho ShipmentDetailDto, RouteStopLiteDto
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models; // Cần cho Enums
using System;
using System.Collections.Generic;
using System.ComponentModel; // Cần cho BindingList
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LMS.GUI.ShipmentAdmin
{
    public partial class ucShipmentDetail_Admin : UserControl
    {
        private readonly int _shipmentId;
        private readonly ShipmentService_Admin _shipmentSvc = new ShipmentService_Admin(); // Service Admin
        private ShipmentDetailDto _currentDto; // Lưu trữ DTO để truy cập

        // Biến sort cho grid RouteStop
        private DataGridViewColumn _stopSortedColumn = null;
        private SortOrder _stopSortOrder = SortOrder.None;

        public ucShipmentDetail_Admin(int shipmentId)
        {
            InitializeComponent();
            _shipmentId = shipmentId;
            this.Load += UcShipmentDetail_Admin_Load;
        }

        private void UcShipmentDetail_Admin_Load(object sender, EventArgs e)
        {
            if (this.lblTitle != null)
            {
                this.lblTitle.Text = "Chi Tiết Chuyến Hàng";
            }
            ConfigureRouteStopGrid();
            LoadDetails();
            WireEvents();
        }

        #region Setup Controls
        private void ConfigureRouteStopGrid()
        {
            var g = dgvRouteStops; // Giả sử tên grid là dgvRouteStops
            g.Columns.Clear();
            g.ApplyBaseStyle();

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Seq",
                DataPropertyName = "Seq",
                HeaderText = "#",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StopName",
                DataPropertyName = "StopName",
                HeaderText = "Điểm dừng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StopStatus",
                DataPropertyName = "StopStatus",
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ArrivedAt",
                DataPropertyName = "ArrivedAt",
                HeaderText = "Đến lúc",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "dd/MM HH:mm" },
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DepartedAt",
                DataPropertyName = "DepartedAt",
                HeaderText = "Rời lúc",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = { Format = "dd/MM HH:mm" },
                SortMode = DataGridViewColumnSortMode.Programmatic
            });
            //g.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    Name = "PlannedETA",
            //    DataPropertyName = "PlannedETA",
            //    HeaderText = "Dự kiến",
            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            //    DefaultCellStyle = { Format = "dd/MM HH:mm" },
            //    SortMode = DataGridViewColumnSortMode.Programmatic
            //});

            g.CellFormatting += DgvRouteStops_CellFormatting; // Format Status chặng
        }

        private void WireEvents()
        {
            btnClose.Click += (s, e) => this.FindForm()?.Close();
            // Gán sự kiện sort cho grid RouteStop
            dgvRouteStops.ColumnHeaderMouseClick += dgvRouteStops_ColumnHeaderMouseClick;
            // (Không cần nút Lưu Ghi Chú theo thảo luận trước)
        }
        #endregion

        #region Load Data & Binding
        private void LoadDetails()
        {
            try
            {
                _currentDto = _shipmentSvc.GetShipmentDetailForAdmin(_shipmentId); // Gọi hàm service Admin
                if (_currentDto == null || _currentDto.Header == null)
                {
                    throw new Exception("Không tải được dữ liệu chi tiết chuyến hàng.");
                }

                BindHeader(_currentDto.Header);
                BindMisc(_currentDto);
                BindStops(_currentDto.Stops);
                ResetStopSortGlyphs(); // Reset sort grid stop
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải chi tiết: {ex.Message}", "Lỗi");
                this.FindForm()?.Close(); // Đóng popup nếu lỗi
            }
        }

        private void BindHeader(ShipmentRunHeaderDto header)
        {
            lblShipmentNo.Text = $"Mã Chuyến: {header.ShipmentNo}";
            lblOrderNo.Text = $"Mã Đơn: {header.OrderNo}";
            lblCustomerName.Text = $"Khách hàng: {header.CustomerName ?? "N/A"}";
            lblRoute.Text = $"Tuyến: {header.Route ?? "N/A"}";
            lblStatus.Text = $"Trạng thái: {FormatShipmentStatusString(header.Status)}"; // Format Status
            lblStartedAt.Text = $"Bắt đầu: {header.StartedAt?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa bắt đầu"}";
            lblDeliveredAt.Text = $"Kết thúc: {header.DeliveredAt?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa kết thúc"}";
        }

        private void BindMisc(ShipmentDetailDto detail)
        {
            lblDriverName.Text = $"Tài xế: {detail.DriverName ?? "N/A"}";
            lblVehicleNo.Text = $"Biển số xe: {detail.VehicleNo ?? "N/A"}";
            lblDuration.Text = $"Thời gian: {detail.Duration?.ToString(@"hh\:mm\:ss") ?? "N/A"}";
            txtNotes.Text = detail.Notes ?? ""; // Hiển thị ghi chú
            txtNotes.ReadOnly = true; // Chỉ cho xem
        }

        private void BindStops(List<RouteStopLiteDto> stops)
        {
            dgvRouteStops.DataSource = new BindingList<RouteStopLiteDto>(stops ?? new List<RouteStopLiteDto>());
        }
        #endregion

        #region Stop Grid Sorting
        private void dgvRouteStops_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var list = dgvRouteStops.DataSource as BindingList<RouteStopLiteDto>;
            if (list == null || list.Count == 0) return;
            var newColumn = dgvRouteStops.Columns[e.ColumnIndex];
            if (newColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

            if (_stopSortedColumn == newColumn) { _stopSortOrder = (_stopSortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending; }
            else { if (_stopSortedColumn != null) _stopSortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; _stopSortOrder = SortOrder.Ascending; _stopSortedColumn = newColumn; }

            ApplyStopSort(list);
            UpdateStopSortGlyphs();
        }

        private void ApplyStopSort(BindingList<RouteStopLiteDto> data)
        {
            if (_stopSortedColumn == null || data == null || data.Count == 0) return;
            string propertyName = _stopSortedColumn.DataPropertyName;
            PropertyInfo propInfo = typeof(RouteStopLiteDto).GetProperty(propertyName);
            if (propInfo == null) return;

            List<RouteStopLiteDto> items = data.ToList();
            List<RouteStopLiteDto> sortedList;
            try
            {
                if (_stopSortOrder == SortOrder.Ascending) { sortedList = items.OrderBy(x => propInfo.GetValue(x, null)).ToList(); }
                else { sortedList = items.OrderByDescending(x => propInfo.GetValue(x, null)).ToList(); }
                // Cập nhật lại DataSource (không cần BindingList mới vì đã có từ đầu)
                dgvRouteStops.DataSource = new BindingList<RouteStopLiteDto>(sortedList);
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi sắp xếp chặng: {ex.Message}"); ResetStopSortGlyphs(); }
        }

        private void UpdateStopSortGlyphs()
        {
            foreach (DataGridViewColumn col in dgvRouteStops.Columns)
            {
                if (col.SortMode != DataGridViewColumnSortMode.NotSortable) col.HeaderCell.SortGlyphDirection = SortOrder.None;
                if (_stopSortedColumn != null && col == _stopSortedColumn) col.HeaderCell.SortGlyphDirection = _stopSortOrder;
            }
        }
        private void ResetStopSortGlyphs()
        {
            if (_stopSortedColumn != null) _stopSortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            _stopSortedColumn = null; _stopSortOrder = SortOrder.None;
            UpdateStopSortGlyphs();
        }
        #endregion

        #region Formatting Helpers
        // Format Status Chặng (RouteStopStatus)
        private void DgvRouteStops_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvRouteStops.Columns[e.ColumnIndex].Name == "StopStatus" && e.Value != null)
            {
                if (Enum.TryParse<RouteStopStatus>(e.Value.ToString(), out var status))
                {
                    e.Value = FormatRouteStopStatus(status);
                    e.FormattingApplied = true;
                }
            }

            // Highlight dòng hiện tại (nếu có)
            if (_currentDto?.Header?.CurrentStopSeq.HasValue == true && e.RowIndex >= 0)
            {
                var rowData = dgvRouteStops.Rows[e.RowIndex].DataBoundItem as RouteStopLiteDto;
                if (rowData?.Seq == _currentDto.Header.CurrentStopSeq.Value)
                {
                    e.CellStyle.BackColor = Color.LightYellow; // Màu highlight
                }
            }
        }

        // Format Enum ShipmentStatus sang tiếng Việt (dùng cho Label header)
        private string FormatShipmentStatusString(string statusString)
        {
            if (Enum.TryParse<ShipmentStatus>(statusString, out var status))
            {
                return FormatShipmentStatus(status); // Gọi hàm helper chung
            }
            return statusString ?? "N/A";
        }

        // Hàm helper format ShipmentStatus (có thể đưa ra class Helper)
        private string FormatShipmentStatus(ShipmentStatus status)
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

        // Hàm helper format RouteStopStatus (có thể đưa ra class Helper)
        private string FormatRouteStopStatus(RouteStopStatus status)
        {
            switch (status)
            {
                case RouteStopStatus.Waiting: return "Chờ";
                case RouteStopStatus.Arrived: return "Đã đến";
                case RouteStopStatus.Departed: return "Đã rời";
                // Thêm các trạng thái khác nếu có (Skipped...)
                default: return status.ToString();
            }
        }
        #endregion
    }
}