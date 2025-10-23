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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.GUI.OrderDriver
{
    public partial class ucShipmentDetail_Drv : UserControl
    {
        private readonly DriverShipmentService _svc = new DriverShipmentService();
        private int _shipmentId;
        private ShipmentDetailDto _dto;
        public ucShipmentDetail_Drv()
        {
            InitializeComponent();
            this.Load += (s, e) => { ConfigureGrid(); Wire(); };
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
            dgvStops.AutoGenerateColumns = false;
            dgvStops.AllowUserToAddRows = false;
            dgvStops.ReadOnly = true;
            dgvStops.RowHeadersVisible = false;
            dgvStops.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStops.MultiSelect = false;

            dgvStops.EnableHeadersVisualStyles = false;
            dgvStops.GridColor = Color.Gainsboro;
            dgvStops.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvStops.BorderStyle = BorderStyle.FixedSingle;
            dgvStops.ColumnHeadersHeight = 36;
            dgvStops.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 113, 185);
            dgvStops.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvStops.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "Seq", DataPropertyName = "Seq", HeaderText = "#" });
            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "StopName", DataPropertyName = "StopName", HeaderText = "Điểm dừng" });
            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "PlannedETA", DataPropertyName = "PlannedETA", HeaderText = "Kế hoạch" });
            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "ArrivedAt", DataPropertyName = "ArrivedAt", HeaderText = "Đến lúc" });
            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "DepartedAt", DataPropertyName = "DepartedAt", HeaderText = "Rời lúc" });
            dgvStops.Columns.Add(new DataGridViewTextBoxColumn { Name = "StopStatus", DataPropertyName = "StopStatus", HeaderText = "Trạng thái" });
        }

        private void Wire()
        {
            btnReload.Click += (s, e) => ReloadData();
            btnReceive.Click += (s, e) => { TryDo(() => _svc.ReceiveShipment(_shipmentId, DriverId), "Đã nhận shipment."); };
            btnDepart.Click += (s, e) => { TryDo(() => _svc.DepartCurrentStop(_shipmentId, DriverId), "Đã rời kho."); };
            btnArrive.Click += (s, e) => { TryDo(() => _svc.ArriveNextStop(_shipmentId, DriverId), "Đã đến kho kế tiếp."); };
            btnComplete.Click += (s, e) => { TryDo(() => _svc.CompleteShipment(_shipmentId, DriverId), "Đã hoàn tất chuyến và đơn hàng."); };

            // Optional notes:
            //if (btnSaveNotes != null)
            //    btnSaveNotes.Click += (s, e) => SaveNotes();
        }

        private void TryDo(Action action, string okMsg)
        {
            try
            {
                action();
                ReloadData();
                MessageBox.Show(okMsg, "LMS");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        }

        private void ReloadData()
        {
            try
            {
                _dto = _svc.GetDetail(_shipmentId, DriverId);
                // Header
                lblShipmentNo.Text = _dto.Header.ShipmentNo;
                lblOrderNo.Text = _dto.Header.OrderNo;
                lblCustomer.Text = _dto.Header.CustomerName;
                lblRoute.Text = _dto.Header.Route;
                lblStatus.Text = _dto.Header.Status;
                lblCurrentSeq.Text = _dto.Header.CurrentStopSeq?.ToString() ?? "-";

                //if (txtNotes != null) txtNotes.Text = _dto.Notes ?? "";

                dgvStops.DataSource = new BindingList<RouteStopLiteDto>(_dto.Stops);

                UpdateButtonsByState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nạp dữ liệu");
            }
        }

        private void UpdateButtonsByState()
        {
            // Reset
            btnReceive.Enabled = btnDepart.Enabled = btnArrive.Enabled = btnComplete.Enabled = false;

            if (!Enum.TryParse<ShipmentStatus>(_dto.Header.Status, out var st)) return;

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

        private void SaveNotes()
        {
            try
            {
                // bạn có thể thêm field Note chỉnh sửa trong DriverShipmentService nếu muốn
                // Ở đây demo: viết nhanh bằng DbContext
                using (var db = new LMS.DAL.LogisticsDbContext())
                {
                    var s = db.Shipments.Find(_shipmentId);
                    if (s == null) throw new Exception("Shipment không tồn tại.");
                    //s.Note = txtNotes.Text?.Trim();
                    s.UpdatedAt = DateTime.Now;
                    db.SaveChanges();
                }
                MessageBox.Show("Đã lưu ghi chú.", "LMS");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        }
    }
}
