using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL.Models;
using LMS.GUI.Main;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LMS.GUI.OrderAdmin
{
    public partial class ucOrderDetail_Admin : UserControl
    {
        private readonly int _orderId;
        private readonly OrderService_Admin _orderSvc = new OrderService_Admin();

        public ucOrderDetail_Admin(int orderId)
        {
            InitializeComponent();
            _orderId = orderId;
            this.Load += UcOrderDetail_Admin_Load;
        }

        private void UcOrderDetail_Admin_Load(object sender, EventArgs e)
        {
            LoadData();

            btnBack.Click += (s, ev) =>
            {
                this.FindForm()?.Close(); // Close the parent Form
            };
        }


        private void LoadData()
        {
            var o = _orderSvc.GetByIdWithAll(_orderId);
            if (o == null)
            {
                MessageBox.Show("Không tìm thấy đơn hàng.", "LMS");
                this.FindForm()?.Close();
                return;
            }

            // --- Assign data to Labels with descriptive text ---
            lblOrderId.Text = $"Mã ĐH: {OrderCode.ToCode(o.Id)}"; // Add "Mã ĐH: "
            lblCustomer.Text = $"Khách hàng: {o.Customer?.Name ?? $"#{o.CustomerId}"}"; // Add "Khách hàng: "
            lblOrigin.Text = $"Kho gửi: {o.OriginWarehouse?.Name ?? $"#{o.OriginWarehouseId}"}"; // Add "Kho gửi: "
            lblDest.Text = $"Kho nhận: {o.DestWarehouse?.Name ?? $"#{o.DestWarehouseId}"}"; // Add "Kho nhận: "
            lblPickupAddress.Text = $"Địa chỉ lấy: {(string.IsNullOrEmpty(o.PickupAddress) ? "—" : o.PickupAddress)}"; // Add "Địa chỉ lấy: "
            lblPackage.Text = $"Ghi Chú: {(string.IsNullOrEmpty(o.PackageDescription) ? "—" : o.PackageDescription)}"; // Add "Gói hàng: "
            lblTotalFee.Text = $"Tổng phí: {o.TotalFee.ToString("N0")}"; // Add "Tổng phí: "
            lblDepositAmount.Text = $"Đặt cọc: {o.DepositAmount.ToString("N0")}"; // Add "Đặt cọc: "
            string statusText;
            switch (o.Status)
            {
                case OrderStatus.Pending: statusText = "Chờ duyệt"; break;
                case OrderStatus.Approved: statusText = "Đã duyệt"; break;
                case OrderStatus.Completed: statusText = "Hoàn thành"; break;
                case OrderStatus.Cancelled: statusText = "Đã hủy"; break;
                default: statusText = o.Status.ToString(); break;
            }
            lblStatus.Text = $"Trạng thái: {statusText}"; // Add "Trạng thái: "
            //dtpCreatedAt.Value = o.CreatedAt;
            //dtpCreatedAt.Enabled = false;
            lblCreatedAt.Text = $"Ngày tạo: {o.CreatedAt.ToString("dd/MM/yyyy HH:mm")}"; // Chọn định dạng bạn muốn
            grpDetail.Text = $"Chi tiết đơn hàng {OrderCode.ToCode(o.Id)}";
        }

        private void ShowOrderDetailDialog(int orderId)
        {
            using (var f = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                Width = 900,
                Height = 650,
                Text = $"Chi tiết đơn hàng {OrderCode.ToCode(orderId)}"
            })
            {
                var uc = new LMS.GUI.OrderAdmin.ucOrderDetail_Admin(orderId) { Dock = DockStyle.Fill };
                f.Controls.Add(uc);
                f.ShowDialog(this);
            }
        }

    }
}