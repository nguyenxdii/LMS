//using LMS.BUS.Helpers;
//using LMS.BUS.Services;
//using LMS.DAL.Models;
//using LMS.GUI.Main;
//using System;
//using System.Drawing;
//using System.Windows.Forms;

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
//            this.Size = new Size(500, 500);
//        }

//        private void UcOrderDetail_Admin_Load(object sender, EventArgs e)
//        {
//            LoadData();
//            this.Size = new Size(500, 500);

//            // nút Quay lại
//            btnBack.Click += (s, ev) =>
//            {
//                this.FindForm()?.Close(); // chỉ đóng popup chi tiết
//            };

//            // Xóa phần xử lý redundant của btnBack
//            // var parentForm = this.FindForm();
//            // if (btnBack != null) btnBack.Click += (s, ev) => parentForm?.Close(); // Không cần vì đã có trên
//        }

//        //private void LoadData()
//        //{
//        //    var o = _orderSvc.GetByIdWithAll(_orderId);
//        //    if (o == null)
//        //    {
//        //        MessageBox.Show("Không tìm thấy đơn hàng.", "LMS");
//        //        return;
//        //    }

//        //    // Gán dữ liệu vào các label
//        //    //lblOrderId.Text = o.Id.ToString();
//        //    lblOrderId.Text = OrderCode.ToCode(o.Id);
//        //    lblCustomer.Text = o.Customer?.Name ?? $"#{o.CustomerId}";
//        //    lblOrigin.Text = o.OriginWarehouse?.Name ?? $"#{o.OriginWarehouseId}";
//        //    lblDest.Text = o.DestWarehouse?.Name ?? $"#{o.DestWarehouseId}";
//        //    lblPickupAddress.Text = string.IsNullOrEmpty(o.PickupAddress) ? "—" : o.PickupAddress;
//        //    lblPackage.Text = string.IsNullOrEmpty(o.PackageDescription) ? "—" : o.PackageDescription;
//        //    lblTotalFee.Text = o.TotalFee.ToString("N0");
//        //    lblDepositAmount.Text = o.DepositAmount.ToString("N0");
//        //    lblStatus.Text = o.Status.ToString();
//        //    //lblCreatedAt.Text = o.CreatedAt.ToString("hh:mm tt dd/MM/yyyy"); // Hiển thị thời gian với định dạng 06:00 PM 22/10/2025
//        //    dtpCreatedAt.Value = o.CreatedAt;
//        //    dtpCreatedAt.Enabled = false;
//        //}
//        // Trong file LMS.GUI/OrderAdmin/ucOrderDetail_Admin.cs

//        private void LoadData()
//        {
//            var o = _orderSvc.GetByIdWithAll(_orderId);
//            if (o == null)
//            {
//                MessageBox.Show("Không tìm thấy đơn hàng.", "LMS");
//                // Quan trọng: Nên đóng form nếu không tìm thấy đơn hàng
//                this.FindForm()?.Close();
//                return;
//            }

//            // Gán dữ liệu vào các label
//            lblOrderId.Text = OrderCode.ToCode(o.Id);
//            lblCustomer.Text = o.Customer?.Name ?? $"#{o.CustomerId}";
//            lblOrigin.Text = o.OriginWarehouse?.Name ?? $"#{o.OriginWarehouseId}";
//            lblDest.Text = o.DestWarehouse?.Name ?? $"#{o.DestWarehouseId}";
//            lblPickupAddress.Text = string.IsNullOrEmpty(o.PickupAddress) ? "—" : o.PickupAddress;
//            lblPackage.Text = string.IsNullOrEmpty(o.PackageDescription) ? "—" : o.PackageDescription;
//            lblTotalFee.Text = o.TotalFee.ToString("N0");
//            lblDepositAmount.Text = o.DepositAmount.ToString("N0");

//            // === SỬA TỪ ĐÂY ===
//            // lblStatus.Text = o.Status.ToString(); // Dòng cũ

//            // Dùng switch để chuyển sang tiếng Việt
//            switch (o.Status)
//            {
//                case OrderStatus.Pending:
//                    lblStatus.Text = "Chờ duyệt";
//                    break;
//                case OrderStatus.Approved:
//                    lblStatus.Text = "Đã duyệt";
//                    break;
//                case OrderStatus.Completed:
//                    lblStatus.Text = "Hoàn thành";
//                    break;
//                case OrderStatus.Cancelled:
//                    lblStatus.Text = "Đã hủy";
//                    break;
//                default:
//                    lblStatus.Text = o.Status.ToString(); // Giữ nguyên nếu không khớp
//                    break;
//            }
//            // === ĐẾN ĐÂY ===

//            dtpCreatedAt.Value = o.CreatedAt;
//            dtpCreatedAt.Enabled = false;
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
            // No need to set Size here, rely on the Form containing it
            // this.Size = new Size(500, 500); 
        }

        private void UcOrderDetail_Admin_Load(object sender, EventArgs e)
        {
            LoadData();

            // Set Size AFTER LoadData if needed, but Dock=Fill is better
            // this.Size = new Size(500, 500); 

            // Back button click event
            btnBack.Click += (s, ev) =>
            {
                this.FindForm()?.Close(); // Close the parent Form
            };
        }

        // In LMS.GUI/OrderAdmin/ucOrderDetail_Admin.cs

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
    }
}