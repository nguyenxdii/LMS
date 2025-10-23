using Guna.UI2.WinForms;
using LMS.BUS.Dtos;
using LMS.BUS.Services;
using LMS.GUI.Main;
using System.ComponentModel;
using System.Windows.Forms;

namespace LMS.GUI.OrderDriver
{
    public partial class ucShipmentDetail_Drv : UserControl
    {
        private readonly int _driverId;
        private readonly int _shipmentId;
        private readonly DriverShipmentService _svc = new DriverShipmentService();

        // YÊU CẦU TRÊN DESIGNER: lblTitle,lblOrderNo,lblCustomer,lblRoute,lblDriver,lblVehicle,lblStatus,lblDuration,txtNotes,dgvStops,btnBack

        public ucShipmentDetail_Drv(int shipmentId, int driverId)
        {
            _shipmentId = shipmentId;
            _driverId = driverId;
            InitializeComponent();
            this.Load += (s, e) => { ReloadData(); Wire(); };
        }

        private void Wire()
        {
            btnBack.Click += (s, e) =>
            {
                var f = this.FindForm() as frmMain_Driver;
                //f?.btnMyShipments.PerformClick();
                //var f = this.FindForm() as frmMain_Driver;
                f?.OpenMyShipments();
            };
        }

        private void ReloadData()
        {
            var dto = _svc.GetDetail(_shipmentId, _driverId);

            lblTitle.Text = $"Chi tiết Shipment – {dto.Header.ShipmentNo}";
            lblOrderNo.Text = $"Đơn hàng: {dto.Header.OrderNo}";
            lblCustomer.Text = $"Khách: {dto.Header.CustomerName}";
            lblRoute.Text = $"Tuyến: {dto.Header.Route}";
            lblDriver.Text = $"Tài xế: {dto.DriverName}";
            lblVehicle.Text = $"Xe: {dto.VehicleNo}";
            lblStatus.Text = $"Trạng thái: {dto.Header.Status}";
            lblDuration.Text = dto.Duration.HasValue ? $"Thời lượng: {dto.Duration.Value:d\\.hh\\:mm}" : "Thời lượng: —";
            txtNotes.Text = dto.Notes ?? "";

            dgvStops.AutoGenerateColumns = true;
            dgvStops.DataSource = new BindingList<RouteStopLiteDto>(dto.Stops);
        }
    }
}
