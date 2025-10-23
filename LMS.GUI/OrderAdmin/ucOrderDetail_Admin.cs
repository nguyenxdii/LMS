//using LMS.BUS.Services;
//using LMS.DAL.Models;
//using LMS.GUI.Main;
//using System;
//using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;

//namespace LMS.GUI.OrderAdmin
//{
//    public partial class ucOrderDetail_Admin : UserControl
//    {
//        private readonly int _orderId;
//        private readonly OrderService_Admin _orderSvc = new OrderService_Admin();

//        public ucOrderDetail_Admin(int orderId)
//        {
//            InitializeComponent();
//            _orderId = orderId;
//            this.Load += UcOrderDetail_Admin_Load;
//        }

//        private void UcOrderDetail_Admin_Load(object sender, EventArgs e)
//        {
//            LoadData();
//            ReadOnly();

//            // nút Quay lại
//            //btnBack.Click += (s, ev) =>
//            //{
//            //    var host = this.FindForm() as frmMain_Admin;
//            //    host?.LoadUc(new ucOrder_Admin());
//            //};
//            btnBack.Click += (s, ev) =>
//            {
//                this.FindForm()?.Close(); // chỉ đóng popup chi tiết
//            };

//            // add
//            // trong UcOrderDetail_Admin_Load:
//            var parentForm = this.FindForm();
//            if (btnBack != null)
//                btnBack.Click += (s, ev) => parentForm?.Close();

//        }

//        private void ReadOnly()
//        {
//            txtCode.ReadOnly = true;
//            txtCustomer.ReadOnly = true;
//            txtDepositAmount.ReadOnly = true;
//            txtDest.ReadOnly = true;
//            txtOrderId.ReadOnly = true;
//            txtOrigin.ReadOnly = true;
//            txtPackage.ReadOnly = true;
//            txtPickupAddress.ReadOnly = true;
//            txtStatus.ReadOnly = true;
//            txtTotalFee.ReadOnly = true;
//            dtpCreatedAt.Enabled = false;
//        }

//        private void LoadData()
//        {
//            var o = _orderSvc.GetByIdWithAll(_orderId);
//            if (o == null)
//            {
//                MessageBox.Show("Không tìm thấy đơn hàng.", "LMS");
//                return;
//            }

//            // Gán dữ liệu vào các textbox
//            txtOrderId.Text = o.Id.ToString();
//            txtCustomer.Text = o.Customer?.Name ?? $"#{o.CustomerId}";
//            txtOrigin.Text = o.OriginWarehouse?.Name ?? $"#{o.OriginWarehouseId}";
//            txtDest.Text = o.DestWarehouse?.Name ?? $"#{o.DestWarehouseId}";
//            txtPickupAddress.Text = string.IsNullOrEmpty(o.PickupAddress) ? "—" : o.PickupAddress;
//            txtPackage.Text = string.IsNullOrEmpty(o.PackageDescription) ? "—" : o.PackageDescription;
//            txtTotalFee.Text = o.TotalFee.ToString("N0");
//            txtDepositAmount.Text = o.DepositAmount.ToString("N0");
//            txtStatus.Text = o.Status.ToString();
//            dtpCreatedAt.Value = o.CreatedAt;
//        }
//    }
//}

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
            this.Size = new Size(500, 500);
        }

        private void UcOrderDetail_Admin_Load(object sender, EventArgs e)
        {
            LoadData();
            this.Size = new Size(500, 500);

            // nút Quay lại
            btnBack.Click += (s, ev) =>
            {
                this.FindForm()?.Close(); // chỉ đóng popup chi tiết
            };

            // Xóa phần xử lý redundant của btnBack
            // var parentForm = this.FindForm();
            // if (btnBack != null) btnBack.Click += (s, ev) => parentForm?.Close(); // Không cần vì đã có trên
        }

        private void LoadData()
        {
            var o = _orderSvc.GetByIdWithAll(_orderId);
            if (o == null)
            {
                MessageBox.Show("Không tìm thấy đơn hàng.", "LMS");
                return;
            }

            // Gán dữ liệu vào các label
            //lblOrderId.Text = o.Id.ToString();
            lblOrderId.Text = OrderCode.ToCode(o.Id);
            lblCustomer.Text = o.Customer?.Name ?? $"#{o.CustomerId}";
            lblOrigin.Text = o.OriginWarehouse?.Name ?? $"#{o.OriginWarehouseId}";
            lblDest.Text = o.DestWarehouse?.Name ?? $"#{o.DestWarehouseId}";
            lblPickupAddress.Text = string.IsNullOrEmpty(o.PickupAddress) ? "—" : o.PickupAddress;
            lblPackage.Text = string.IsNullOrEmpty(o.PackageDescription) ? "—" : o.PackageDescription;
            lblTotalFee.Text = o.TotalFee.ToString("N0");
            lblDepositAmount.Text = o.DepositAmount.ToString("N0");
            lblStatus.Text = o.Status.ToString();
            //lblCreatedAt.Text = o.CreatedAt.ToString("hh:mm tt dd/MM/yyyy"); // Hiển thị thời gian với định dạng 06:00 PM 22/10/2025
            dtpCreatedAt.Value = o.CreatedAt;
            dtpCreatedAt.Enabled = false;
        }
    }
}