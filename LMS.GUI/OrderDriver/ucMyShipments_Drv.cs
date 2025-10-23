using Guna.UI2.WinForms;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.GUI.Main;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace LMS.GUI.OrderDriver
{
    public partial class ucMyShipments_Drv : UserControl
    {
        private readonly int _driverId;
        private readonly DriverShipmentService _svc = new DriverShipmentService();

        // YÊU CẦU TRÊN DESIGNER: dgvAssigned, dgvHistory, btnReload, btnSearchHistory, cmbStatus, dtFrom, dtTo

        public ucMyShipments_Drv(int driverId)
        {
            _driverId = driverId;
            InitializeComponent();
            this.Load += (s, e) => { LoadAssigned(); LoadHistory(); Wire(); };
        }

        private void Wire()
        {
            btnReload.Click += (s, e) => { LoadAssigned(); LoadHistory(); };
            btnSearchHistory.Click += (s, e) => LoadHistory();

            dgvAssigned.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var id = (int)dgvAssigned.Rows[e.RowIndex].Cells["Id"].Value;
                var f = this.FindForm() as frmMain_Driver;
                f?.ShowUc(new ucShipmentRun_Drv(id, _driverId));
            };

            dgvHistory.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var id = (int)dgvHistory.Rows[e.RowIndex].Cells["Id"].Value;
                var f = this.FindForm() as frmMain_Driver;
                f?.ShowUc(new ucShipmentDetail_Drv(id, _driverId));
            };
        }

        private void LoadAssigned()
        {
            var data = _svc.GetAssignedAndRunning(_driverId);
            dgvAssigned.AutoGenerateColumns = true; // bạn đã thiết kế cột thì để false & map DataPropertyName
            dgvAssigned.DataSource = new BindingList<object>(data);
        }

        private void LoadHistory()
        {
            ShipmentStatus? st = null;
            if (cmbStatus.SelectedItem is ShipmentStatus s) st = s;

            var data = _svc.GetAllMine(_driverId, dtFrom.Value.Date, dtTo.Value.Date.AddDays(1), st);
            dgvHistory.AutoGenerateColumns = true;
            dgvHistory.DataSource = new BindingList<object>(data);
        }
    }
}
