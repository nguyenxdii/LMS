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
    public partial class ucShipmentRun_Drv : UserControl
    {
        private readonly DriverShipmentService _svc = new DriverShipmentService();
        private int _shipmentId;
        public ucShipmentRun_Drv()
        {
            InitializeComponent();
            this.Load += (s, e) => { Wire(); };

        }
        public void BindShipment(int shipmentId)
        {
            _shipmentId = shipmentId;
            Reload();
        }

        private int DriverId => AppSession.DriverId ?? 0;

        private void Wire()
        {
            btnReload.Click += (s, e) => Reload();
            btnDepart.Click += (s, e) => { TryDo(() => _svc.DepartCurrentStop(_shipmentId, DriverId)); };
            btnArrive.Click += (s, e) => { TryDo(() => _svc.ArriveNextStop(_shipmentId, DriverId)); };
            btnComplete.Click += (s, e) => { TryDo(() => _svc.CompleteShipment(_shipmentId, DriverId)); };
        }

        private void TryDo(Action action)
        {
            try { action(); Reload(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        }

        private void Reload()
        {
            try
            {
                var dto = _svc.GetDetail(_shipmentId, DriverId);

                // Tính cặp chặng hiện tại
                var curSeq = dto.Header.CurrentStopSeq ?? dto.Stops.Min(s => s.Seq);
                var maxSeq = dto.Stops.Max(s => s.Seq);

                var cur = dto.Stops.FirstOrDefault(x => x.Seq == curSeq);
                var next = dto.Stops.FirstOrDefault(x => x.Seq == curSeq + 1);

                lblFromTo.Text = next == null
                    ? $"{cur?.StopName} → (đích)"
                    : $"{cur?.StopName} → {next.StopName}";

                lblPhase.Text = $"Trạng thái chuyến: {dto.Header.Status}";

                btnDepart.Enabled = btnArrive.Enabled = btnComplete.Enabled = false;

                if (!Enum.TryParse<ShipmentStatus>(dto.Header.Status, out var st)) return;

                if (st == ShipmentStatus.Assigned || st == ShipmentStatus.AtWarehouse) btnDepart.Enabled = true;
                if (st == ShipmentStatus.OnRoute) btnArrive.Enabled = true;
                if (st == ShipmentStatus.ArrivedDestination) btnComplete.Enabled = true;

                if (lblHint != null)
                {
                    lblHint.Text =
                        st == ShipmentStatus.Assigned ? "Nhấn 'Rời kho' để bắt đầu di chuyển." :
                        st == ShipmentStatus.AtWarehouse ? "Đang ở kho — nhấn 'Rời kho' để đi tiếp." :
                        st == ShipmentStatus.OnRoute ? "Đang di chuyển — nhấn 'Đến kho' khi tới điểm kế tiếp." :
                        st == ShipmentStatus.ArrivedDestination ? "Đã tới kho đích — nhấn 'Hoàn tất'." :
                        "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải dữ liệu");
            }
        }
    }
}
