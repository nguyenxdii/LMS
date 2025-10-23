using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Services;
using LMS.GUI.Main;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LMS.GUI.OrderDriver
{
    public partial class ucShipmentRun_Drv : UserControl
    {
        private readonly int _driverId;
        private readonly int _shipmentId;
        private readonly DriverShipmentService _svc = new DriverShipmentService();
        private ShipmentDetailDto _dto;

        // YÊU CẦU TRÊN DESIGNER: lblTitle, lblStatus, lblCurrentStop (optional), dgvRoute,
        // btnReceive, btnDepart, btnArrive, btnComplete, btnBack

        public ucShipmentRun_Drv(int shipmentId, int driverId)
        {
            _shipmentId = shipmentId;
            _driverId = driverId;
            InitializeComponent();
            this.Load += (s, e) => { ReloadData(); Wire(); };
        }

        private void Wire()
        {
            btnReceive.Click += (s, e) => { TryDo(() => _svc.ReceiveShipment(_shipmentId, _driverId)); };
            btnDepart.Click += (s, e) => { TryDo(() => _svc.DepartCurrentStop(_shipmentId, _driverId)); };
            btnArrive.Click += (s, e) => { TryDo(() => _svc.ArriveNextStop(_shipmentId, _driverId)); };
            btnComplete.Click += (s, e) => { TryDo(() => _svc.CompleteShipment(_shipmentId, _driverId)); };
            btnBack.Click += (s, e) =>
            {
                var f = this.FindForm() as frmMain_Driver;
                //f?.btnMyShipments.PerformClick();
                //var f = this.FindForm() as frmMain_Driver;
                f?.OpenMyShipments();
            };
        }

        private void TryDo(Action action)
        {
            try { action(); ReloadData(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        }

        private void ReloadData()
        {
            _dto = _svc.GetDetail(_shipmentId, _driverId);

            lblTitle.Text = $"Chi tiết chuyến – {_dto.Header.ShipmentNo}";
            lblStatus.Text = $"Trạng thái: {_dto.Header.Status}";

            // nếu có lblCurrentStop:
            if (this.Controls.ContainsKey("lblCurrentStop") && _dto.Header.CurrentStopSeq.HasValue)
            {
                var cur = _dto.Stops.FirstOrDefault(x => x.Seq == _dto.Header.CurrentStopSeq.Value);
                if (cur != null) ((Label)this.Controls["lblCurrentStop"]).Text = $"Đang ở: {cur.StopName}";
            }

            dgvRoute.AutoGenerateColumns = true; // bạn đã setup cột thì để false & map
            dgvRoute.DataSource = new BindingList<RouteStopLiteDto>(_dto.Stops);

            UpdateButtonStates();
            HighlightCurrentRow();
        }

        private void UpdateButtonStates()
        {
            var st = _dto.Header.Status;
            btnReceive.Enabled = st == "Pending";
            btnDepart.Enabled = st == "Assigned" || st == "AtWarehouse";
            btnArrive.Enabled = st == "OnRoute";
            btnComplete.Enabled = st == "ArrivedDestination";
        }

        private void HighlightCurrentRow()
        {
            if (!_dto.Header.CurrentStopSeq.HasValue) return;
            var idx = _dto.Stops.FindIndex(x => x.Seq == _dto.Header.CurrentStopSeq.Value);
            if (idx >= 0 && idx < dgvRoute.Rows.Count)
            {
                var row = dgvRoute.Rows[idx];
                row.DefaultCellStyle.BackColor = Color.FromArgb(230, 245, 255);
                row.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgvRoute.FirstDisplayedScrollingRowIndex = row.Index;
            }
        }
    }
}
