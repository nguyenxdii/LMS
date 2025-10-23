// LMS.GUI/OrderCustomer/ucOrderConfirm_Cus.cs
using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL;
using LMS.GUI.Main;
using System;
using System.Windows.Forms;
using static LMS.BUS.Services.OrderService_Admin;

namespace LMS.GUI.OrderCustomer
{
    public partial class ucOrderConfirm_Cus : UserControl
    {
        private readonly OrderDraft _draft;
        private readonly OrderService_Customer _orderSvc = new OrderService_Customer();

        public ucOrderConfirm_Cus(OrderDraft draft)
        {
            InitializeComponent();
            _draft = draft;
            this.Load += UcOrderConfirm_Cus_Load;
        }

        private void UcOrderConfirm_Cus_Load(object sender, EventArgs e)
        {
            //lblOrderCode.Text = $"Mã đơn: {OrderCode.ToCode()}";
            using (var db = new LogisticsDbContext())
            {
                var o = db.Warehouses.Find(_draft.OriginWarehouseId);
                var d = db.Warehouses.Find(_draft.DestWarehouseId);

                lblOrigin.Text = $"Kho gửi: {o?.Name} ({o?.ZoneId})";
                lblDest.Text = $"Kho nhận: {d?.Name} ({d?.ZoneId})";
            }

            lblPickup.Text = _draft.NeedPickup
                ? $"Pickup: Có (+100.000 đ) - {_draft.PickupAddress}"
                : "Pickup: Không";

            lblDesiredTime.Text = _draft.DesiredTime.HasValue ? $"Thời điểm: {_draft.DesiredTime:dd/MM/yyyy HH:mm}" : "Thời điểm: —";
            lblPackageDesc.Text = $"Mô tả: {_draft.PackageDescription}";
            lblRouteFee.Text = $"Tuyến: {_draft.RouteFee:N0} đ";
            lblPickupFee.Text = $"Pickup: {_draft.PickupFee:N0} đ";
            lblTotalFee.Text = $"TỔNG: {_draft.TotalFee:N0} đ";
            lblDepositPercent.Text = $"Đặt cọc: {_draft.DepositPercent:P0}";
            lblDepositAmount.Text = $"Thanh toán ngay: {_draft.DepositAmount:N0} đ";
            lblRemainingAmount.Text = $"Còn lại khi nhận: {(_draft.TotalFee - _draft.DepositAmount):N0} đ";

            btnBack.Click += (s, ev) =>
            {
                var host = this.FindForm() as frmMain_Customer;
                //host?.LoadUc(new ucOrderCreate_Cus(_draft.CustomerId));
                //host?.LoadUc(new ucOrderConfirm_Cus(_draft.CustomerId, _draft));
                host?.LoadUc(new ucOrderCreate_Cus(_draft.CustomerId, _draft));
            };

            btnConfirmCreate.Click += (s, ev) =>
            {
                var created = _orderSvc.Create(_draft);
                MessageBox.Show(
                    $"Tạo đơn #{OrderCode.ToCode(created.Id)} thành công!\n" +
                    $"Tổng phí: {created.TotalFee:N0} đ\n" +
                    $"Đặt cọc: {created.DepositAmount:N0} đ",
                    "LMS", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var host = this.FindForm() as frmMain_Customer;
                host?.LoadUc(new ucOrderCreate_Cus(_draft.CustomerId)); // hoặc chuyển sang UC danh sách
            };

            btnCancel.Click += (s, ev) =>
            {
                var host = this.FindForm() as frmMain_Customer;
                host?.LoadUc(new ucOrderCreate_Cus(_draft.CustomerId));
            };

        }
    }
}
