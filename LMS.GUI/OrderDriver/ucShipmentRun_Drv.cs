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

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

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

            // Đặt tiêu đề cho Label trên UC (Nếu có lblTitle)
            var titleLabel = this.Controls.Find("lblTitle", true).FirstOrDefault() as Label;
            if (titleLabel != null)
            {
                titleLabel.Text = "Điều Hành Chuyến Hàng";
            }

            // Đặt tiêu đề Form (Text của Form chứa UC)
            var parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Text = "Theo dõi Chuyến hàng";
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
            btnReload.Click += (s, e) => ReloadData();
            btnReceive.Click += (s, e) => { TryDo(() => _svc.ReceiveShipment(_shipmentId, DriverId), "Đã nhận chuyến hàng."); };
            btnDepart.Click += (s, e) => { TryDo(() => _svc.DepartCurrentStop(_shipmentId, DriverId), "Đã rời khỏi điểm dừng."); };
            btnArrive.Click += (s, e) => { TryDo(() => _svc.ArriveNextStop(_shipmentId, DriverId), "Đã đến điểm dừng kế tiếp."); };
            btnComplete.Click += (s, e) => { TryDo(() => _svc.CompleteShipment(_shipmentId, DriverId), "Đã hoàn tất chuyến hàng và đơn hàng."); };

            btnSaveNote.Click += (s, e) => SaveNotes();

            Control pnlTop = this.Controls.Find("pnlTop", true).FirstOrDefault() as Control;
            if (pnlTop != null)
            {
                EnableDrag(pnlTop);
            }
        }

        private void EnableDrag(Control control)
        {
            control.MouseDown += DragHandle_MouseDown;
            control.MouseMove += DragHandle_MouseMove;
            control.MouseUp += DragHandle_MouseUp;

            foreach (Control child in control.Controls)
            {
                EnableDrag(child);
            }
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

        //private void ReloadData()
        //{
        //    if (_shipmentId <= 0) return;

        //    try
        //    {
        //        _dto = _svc.GetDetail(_shipmentId, DriverId);

        //        dgvStops.DataSource = new BindingList<RouteStopLiteDto>(_dto.Stops);

        //        if (txtNotes != null)
        //        {
        //            txtNotes.Text = _dto.Notes ?? "";
        //        }

        //        UpdateButtonsByState();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Lỗi nạp dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        if (this.FindForm() is Form popup)
        //        {
        //            popup.Close();
        //        }
        //    }
        //}
        private void ReloadData()
        {
            if (_shipmentId <= 0) return;

            try
            {
                _dto = _svc.GetDetail(_shipmentId, DriverId); // GetDetail giờ đã có Note trong Stops

                dgvStops.DataSource = new BindingList<RouteStopLiteDto>(_dto.Stops);

                // *** THAY ĐỔI Ở ĐÂY ***
                // Tìm ghi chú của chặng hiện tại để hiển thị
                string currentStopNote = "";
                if (_dto?.Header?.CurrentStopSeq.HasValue == true)
                {
                    var currentStopDto = _dto.Stops.FirstOrDefault(stop => stop.Seq == _dto.Header.CurrentStopSeq.Value);
                    currentStopNote = currentStopDto?.Note ?? ""; // Lấy note từ DTO
                }

                if (txtNotes != null)
                {
                    txtNotes.Text = currentStopNote; // Hiển thị ghi chú của chặng hiện tại
                }
                // *** KẾT THÚC THAY ĐỔI ***

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

        //private void SaveNotes()
        //{
        //    try
        //    {
        //        if (_shipmentId <= 0)
        //            throw new Exception("Không thể lưu ghi chú vì Shipment ID không hợp lệ.");

        //        string noteContent = txtNotes?.Text?.Trim(); // Hoặc sử dụng Find nếu không khai báo

        //        _svc.SaveShipmentNote(_shipmentId, DriverId, noteContent);

        //        ReloadData();
        //        MessageBox.Show("Đã lưu ghi chú thành công.", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi khi lưu ghi chú: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}
        private void SaveNotes()
        {
            try
            {
                if (_shipmentId <= 0)
                    throw new Exception("Không thể lưu ghi chú vì Shipment ID không hợp lệ.");

                string noteContent = txtNotes?.Text?.Trim();

                // *** THAY ĐỔI Ở ĐÂY: Gọi hàm mới SaveStopNote ***
                _svc.SaveStopNote(_shipmentId, DriverId, noteContent);
                // *** KẾT THÚC THAY ĐỔI ***

                // Không cần ReloadData() ở đây nữa vì chỉ lưu note, không đổi trạng thái
                // ReloadData();
                MessageBox.Show("Đã lưu ghi chú cho chặng hiện tại.", "LMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu ghi chú: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


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

        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm == null) return;

            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = parentForm.Location;
            }
        }

        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm == null) return;

            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                parentForm.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = false;
            }
        }
    }
}