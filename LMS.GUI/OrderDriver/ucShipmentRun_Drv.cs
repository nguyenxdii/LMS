//// LMS.GUI/OrderDriver/ucShipmentRun_Drv.cs
//using LMS.BUS.Dtos;
//using LMS.BUS.Helpers;
//using LMS.BUS.Services;
//using LMS.DAL.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;

//namespace LMS.GUI.OrderDriver
//{
//    // Đây là UC chi tiết thực hiện chuyến hàng (Popup từ ucShipmentDetail_Drv)
//    public partial class ucShipmentRun_Drv : UserControl
//    {
//        private readonly DriverShipmentService _svc = new DriverShipmentService();
//        private int _shipmentId;
//        private ShipmentDetailDto _dto;

//        public ucShipmentRun_Drv()
//        {
//            InitializeComponent();
//            this.Load += UcShipmentRun_Drv_Load;
//        }

//        private void UcShipmentRun_Drv_Load(object sender, EventArgs e)
//        {
//            if (DriverId <= 0)
//            {
//                MessageBox.Show("Không thể xác định tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }
//            ConfigureGrid();
//            Wire();
//        }

//        /// <summary>
//        /// Hàm public để Form/UC cha gọi và nạp dữ liệu
//        /// </summary>
//        public void LoadShipment(int shipmentId)
//        {
//            _shipmentId = shipmentId;
//            ReloadData();
//        }

//        private int DriverId => AppSession.DriverId ?? 0;

//        private void ConfigureGrid()
//        {
//            // Cấu hình DGV chi tiết các chặng dừng
//            dgvStops.Columns.Clear();
//            dgvStops.ApplyBaseStyle();

//            // Định nghĩa các cột
//            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "Seq", DataPropertyName = "Seq", HeaderText = "#", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
//            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "StopName", DataPropertyName = "StopName", HeaderText = "Điểm dừng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
//            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "PlannedETA", DataPropertyName = "PlannedETA", HeaderText = "Kế hoạch", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
//            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "ArrivedAt", DataPropertyName = "ArrivedAt", HeaderText = "Đến lúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
//            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "DepartedAt", DataPropertyName = "DepartedAt", HeaderText = "Rời lúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
//            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "StopStatus", DataPropertyName = "StopStatus", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

//            dgvStops.CellFormatting += DgvStops_CellFormatting;
//        }

//        private void Wire()
//        {
//            // Gán sự kiện cho các nút thao tác
//            // Giả định tên nút: btnReceive, btnDepart, btnArrive, btnComplete, btnReload
//            btnReload.Click += (s, e) => ReloadData();
//            btnReceive.Click += (s, e) => { TryDo(() => _svc.ReceiveShipment(_shipmentId, DriverId), "Đã nhận chuyến hàng."); };
//            btnDepart.Click += (s, e) => { TryDo(() => _svc.DepartCurrentStop(_shipmentId, DriverId), "Đã rời khỏi điểm dừng."); };
//            btnArrive.Click += (s, e) => { TryDo(() => _svc.ArriveNextStop(_shipmentId, DriverId), "Đã đến điểm dừng kế tiếp."); };
//            btnComplete.Click += (s, e) => { TryDo(() => _svc.CompleteShipment(_shipmentId, DriverId), "Đã hoàn tất chuyến hàng và đơn hàng."); };

//            // Optional notes:
//            //if (btnSaveNotes != null)
//            //    btnSaveNotes.Click += (s, e) => SaveNotes();
//        }

//        private void TryDo(Action action, string okMsg)
//        {
//            try
//            {
//                action();
//                ReloadData();
//                MessageBox.Show(okMsg, "LMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Thao tác thất bại: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            }
//        }

//        private void ReloadData()
//        {
//            if (_shipmentId <= 0) return;

//            try
//            {
//                _dto = _svc.GetDetail(_shipmentId, DriverId);

//                // Nếu có các Label hiển thị thông tin chuyến hàng chính, bạn có thể cập nhật chúng ở đây.
//                // Ví dụ:
//                // lblShipmentNo.Text = _dto.Header.ShipmentNo;
//                // lblStatus.Text = FormatShipmentStatus(_dto.Header.Status);
//                // lblCurrentSeq.Text = _dto.Header.CurrentStopSeq?.ToString() ?? "-";

//                // Cập nhật DGV
//                dgvStops.DataSource = new BindingList<RouteStopLiteDto>(_dto.Stops);

//                // Cập nhật trạng thái các nút
//                UpdateButtonsByState();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message, "Lỗi nạp dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                // Đảm bảo Form/UC cha đóng nếu lỗi nạp dữ liệu nghiêm trọng
//                if (this.FindForm() is Form popup)
//                {
//                    popup.Close();
//                }
//            }
//        }

//        private void UpdateButtonsByState()
//        {
//            // Reset
//            btnReceive.Enabled = btnDepart.Enabled = btnArrive.Enabled = btnComplete.Enabled = false;

//            if (_dto == null || !Enum.TryParse<ShipmentStatus>(_dto.Header.Status, out var st)) return;

//            switch (st)
//            {
//                case ShipmentStatus.Pending:
//                    btnReceive.Enabled = true; break;
//                case ShipmentStatus.Assigned:
//                    btnDepart.Enabled = true; break;
//                case ShipmentStatus.OnRoute:
//                    btnArrive.Enabled = true; break;
//                case ShipmentStatus.AtWarehouse:
//                    // Ở kho trung gian, chỉ được Depart
//                    btnDepart.Enabled = true; break;
//                case ShipmentStatus.ArrivedDestination:
//                    // Ở kho cuối, chỉ được Complete
//                    btnComplete.Enabled = true; break;
//                default:
//                    break;
//            }
//        }

//        // ===== CÁC HÀM HỖ TRỢ FORMAT =====

//        /// <summary>
//        /// Format trạng thái chặng RouteStopStatus sang Tiếng Việt
//        /// </summary>
//        private void DgvStops_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            // Kiểm tra cột StopStatus và giá trị
//            if (dgvStops.Columns[e.ColumnIndex].DataPropertyName == "StopStatus" && e.Value != null)
//            {
//                if (Enum.TryParse<RouteStopStatus>(e.Value.ToString(), out var status))
//                {
//                    e.Value = FormatRouteStopStatus(status);
//                    e.FormattingApplied = true;
//                }
//            }

//            // Highlight dòng đang là CurrentStopSeq
//            if (_dto?.Header?.CurrentStopSeq.HasValue == true && e.RowIndex >= 0)
//            {
//                var rowData = dgvStops.Rows[e.RowIndex].DataBoundItem as RouteStopLiteDto;
//                if (rowData?.Seq == _dto.Header.CurrentStopSeq.Value)
//                {
//                    // Đánh dấu dòng hiện tại
//                    e.CellStyle.BackColor = Color.LightYellow;
//                }
//            }
//        }

//        private string FormatRouteStopStatus(RouteStopStatus status)
//        {
//            switch (status)
//            {
//                case RouteStopStatus.Waiting: return "Chờ đến";
//                case RouteStopStatus.Arrived: return "Đã đến";
//                case RouteStopStatus.Departed: return "Đã rời";
//                default: return status.ToString();
//            }
//        }

//        // Bạn cũng có thể thêm FormatShipmentStatus nếu cần hiển thị trạng thái chính trên popup.
//        /*
//        private string FormatShipmentStatus(ShipmentStatus status)
//        {
//            switch (status)
//            {
//                // ... (logic dịch) ...
//            }
//        }
//        */

//        // (Hàm SaveNotes giữ nguyên như bạn cung cấp)
//        private void SaveNotes()
//        {
//            try
//            {
//                using (var db = new LMS.DAL.LogisticsDbContext())
//                {
//                    var s = db.Shipments.Find(_shipmentId);
//                    if (s == null) throw new Exception("Shipment không tồn tại.");
//                    //s.Note = txtNotes.Text?.Trim(); // Giả sử bạn có txtNotes
//                    s.UpdatedAt = DateTime.Now;
//                    db.SaveChanges();
//                }
//                MessageBox.Show("Đã lưu ghi chú.", "LMS");
//            }
//            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
//        }
//    }
//}

// LMS.GUI/OrderDriver/ucShipmentRun_Drv.cs
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

namespace LMS.GUI.OrderDriver
{
    public partial class ucShipmentRun_Drv : UserControl
    {
        private readonly DriverShipmentService _svc = new DriverShipmentService();
        private int _shipmentId;
        private ShipmentDetailDto _dto;

        public ucShipmentRun_Drv()
        {
            InitializeComponent();
            this.Load += UcShipmentRun_Drv_Load;
        }

        private void UcShipmentRun_Drv_Load(object sender, EventArgs e)
        {
            if (DriverId <= 0)
            {
                MessageBox.Show("Không thể xác định tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ConfigureGrid();
            Wire();
        }

        public void LoadShipment(int shipmentId)
        {
            _shipmentId = shipmentId;
            ReloadData();
        }

        private int DriverId => AppSession.DriverId ?? 0;

        private void ConfigureGrid()
        {
            dgvStops.Columns.Clear();
            dgvStops.ApplyBaseStyle();

            // Định nghĩa các cột
            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "Seq", DataPropertyName = "Seq", HeaderText = "#", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "StopName", DataPropertyName = "StopName", HeaderText = "Điểm dừng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "PlannedETA", DataPropertyName = "PlannedETA", HeaderText = "Kế hoạch", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "ArrivedAt", DataPropertyName = "ArrivedAt", HeaderText = "Đến lúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "DepartedAt", DataPropertyName = "DepartedAt", HeaderText = "Rời lúc", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, DefaultCellStyle = { Format = "dd/MM HH:mm", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "StopStatus", DataPropertyName = "StopStatus", HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

            dgvStops.CellFormatting += DgvStops_CellFormatting;
        }

        private void Wire()
        {
            // Gán sự kiện cho các nút thao tác
            btnReload.Click += (s, e) => ReloadData();
            btnReceive.Click += (s, e) => { TryDo(() => _svc.ReceiveShipment(_shipmentId, DriverId), "Đã nhận chuyến hàng."); };
            btnDepart.Click += (s, e) => { TryDo(() => _svc.DepartCurrentStop(_shipmentId, DriverId), "Đã rời khỏi điểm dừng."); };
            btnArrive.Click += (s, e) => { TryDo(() => _svc.ArriveNextStop(_shipmentId, DriverId), "Đã đến điểm dừng kế tiếp."); };
            btnComplete.Click += (s, e) => { TryDo(() => _svc.CompleteShipment(_shipmentId, DriverId), "Đã hoàn tất chuyến hàng và đơn hàng."); };

            // THÊM: Gán sự kiện cho nút Lưu ghi chú
            btnSaveNote.Click += (s, e) => SaveNotes();
        }

        private void TryDo(Action action, string okMsg)
        {
            try
            {
                action();
                ReloadData();
                MessageBox.Show(okMsg, "LMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Thao tác thất bại: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ReloadData()
        {
            if (_shipmentId <= 0) return;

            try
            {
                _dto = _svc.GetDetail(_shipmentId, DriverId);

                // Cập nhật DGV
                dgvStops.DataSource = new BindingList<RouteStopLiteDto>(_dto.Stops);

                // Hiển thị ghi chú hiện tại
                // Giả sử txtNotes tồn tại
                if (txtNotes != null)
                {
                    txtNotes.Text = _dto.Notes ?? "";
                }

                // Cập nhật trạng thái các nút
                UpdateButtonsByState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nạp dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (this.FindForm() is Form popup)
                {
                    popup.Close();
                }
            }
        }

        private void UpdateButtonsByState()
        {
            btnReceive.Enabled = btnDepart.Enabled = btnArrive.Enabled = btnComplete.Enabled = false;

            if (_dto == null || !Enum.TryParse<ShipmentStatus>(_dto.Header.Status, out var st)) return;

            switch (st)
            {
                case ShipmentStatus.Pending:
                    btnReceive.Enabled = true; break;
                case ShipmentStatus.Assigned:
                    btnDepart.Enabled = true; break;
                case ShipmentStatus.OnRoute:
                    btnArrive.Enabled = true; break;
                case ShipmentStatus.AtWarehouse:
                    btnDepart.Enabled = true; break;
                case ShipmentStatus.ArrivedDestination:
                    btnComplete.Enabled = true; break;
                default:
                    break;
            }
        }

        // ===== THÊM/SỬA HÀM GHI CHÚ =====
        // LMS.GUI/OrderDriver/ucShipmentRun_Drv.cs

        private void SaveNotes()
        {
            try
            {
                // 1. Kiểm tra Shipment ID
                if (_shipmentId <= 0)
                    throw new Exception("Không thể lưu ghi chú vì Shipment ID không hợp lệ.");

                // LƯU Ý: Phải sử dụng đúng tên control của bạn (ví dụ: txtNotes)
                string noteContent = (this.Controls.Find("txtNotes", true).FirstOrDefault() as Guna.UI2.WinForms.Guna2TextBox)?.Text?.Trim();

                // Nếu bạn đã khai báo txtNotes là một trường (field) trong UserControl này:
                // string noteContent = txtNotes?.Text?.Trim(); 

                // Kiểm tra xem nội dung có rỗng không (tùy chọn)
                if (string.IsNullOrWhiteSpace(noteContent))
                {
                    // Nếu ghi chú rỗng, bạn có thể thông báo hoặc để nó lưu giá trị null/string.Empty
                    // Ở đây tôi cho phép lưu để xóa ghi chú cũ nếu cần
                }

                // 2. Gọi Service để lưu ghi chú
                _svc.SaveShipmentNote(_shipmentId, DriverId, noteContent);

                ReloadData();
                MessageBox.Show("Đã lưu ghi chú thành công.", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu ghi chú: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Cần thêm hàm SaveShipmentNote vào DriverShipmentService.cs (Xem hướng dẫn dưới)


        // ===== CÁC HÀM HỖ TRỢ FORMAT =====

        private void DgvStops_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvStops.Columns[e.ColumnIndex].DataPropertyName == "StopStatus" && e.Value != null)
            {
                if (Enum.TryParse<RouteStopStatus>(e.Value.ToString(), out var status))
                {
                    e.Value = FormatRouteStopStatus(status);
                    e.FormattingApplied = true;
                }
            }

            // Highlight dòng đang là CurrentStopSeq
            if (_dto?.Header?.CurrentStopSeq.HasValue == true && e.RowIndex >= 0)
            {
                var rowData = dgvStops.Rows[e.RowIndex].DataBoundItem as RouteStopLiteDto;
                if (rowData?.Seq == _dto.Header.CurrentStopSeq.Value)
                {
                    e.CellStyle.BackColor = Color.LightYellow;
                }
            }
        }

        private string FormatRouteStopStatus(RouteStopStatus status)
        {
            switch (status)
            {
                case RouteStopStatus.Waiting: return "Chờ đến";
                case RouteStopStatus.Arrived: return "Đã đến";
                case RouteStopStatus.Departed: return "Đã rời";
                default: return status.ToString();
            }
        }
    }
}