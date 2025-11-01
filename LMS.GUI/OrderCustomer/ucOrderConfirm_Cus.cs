// LMS.GUI/OrderCustomer/ucOrderConfirm_Cus.cs
using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
using LMS.BUS.Services;
using LMS.DAL;
using LMS.GUI.Main;
using System;
using System.Windows.Forms;
// Removed: using static LMS.BUS.Services.OrderService_Admin; // Not needed here

namespace LMS.GUI.OrderCustomer
{
    public partial class ucOrderConfirm_Cus : UserControl
    {
        // 1. THÊM ĐỊNH NGHĨA SỰ KIỆN
        public event EventHandler OrderCreated;

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
            using (var db = new LogisticsDbContext())
            {
                var o = db.Warehouses.Find(_draft.OriginWarehouseId);
                var d = db.Warehouses.Find(_draft.DestWarehouseId);

                lblOrigin.Text = $"Kho gửi: {o?.Name} ({o?.ZoneId})";
                lblDest.Text = $"Kho nhận: {d?.Name} ({d?.ZoneId})";
            }

            if (_draft.NeedPickup)
            {
                lblPickup.Text = $"Lấy hàng: Có (+{_draft.PickupFee:N0}đ)\n" +
                                 $"Địa chỉ: {_draft.PickupAddress}";
            }
            else
            {
                lblPickup.Text = "Lấy hàng: Không";
            }

            lblDesiredTime.Text = _draft.DesiredTime.HasValue
                ? $"Thời điểm: {_draft.DesiredTime:dd/MM/yyyy HH:mm}"
                : "Thời điểm: —";

            // Yêu cầu 2: Hiển thị "---" nếu ghi chú trống
            //lblPackageDesc
            lblPackageDesc.Text = string.IsNullOrWhiteSpace(_draft.PackageDescription)
                ? "Ghi chú: ---"
                : $"Ghi chú: {_draft.PackageDescription}";

            lblRouteFee.Text = $"Phí tuyến: {_draft.RouteFee:N0} đ";

            // Yêu cầu 4: Đổi tên "Phí Pickup"
            lblPickupFee.Text = $"Phí lấy hàng: {_draft.PickupFee:N0} đ"; // Đã đổi tên

            lblTotalFee.Text = $"TỔNG PHÍ: {_draft.TotalFee:N0} đ";
            lblDepositPercent.Text = $"Tỷ lệ cọc: {_draft.DepositPercent:P0}";
            lblDepositAmount.Text = $"Thanh toán ngay: {_draft.DepositAmount:N0} đ";
            lblRemainingAmount.Text = $"Còn lại khi nhận: {(_draft.TotalFee - _draft.DepositAmount):N0} đ";
            // --- Kết thúc hiển thị thông tin ---

            // --- Gán sự kiện cho các nút (Giữ nguyên) ---
            btnBack.Click += (s, ev) =>
            {
                this.FindForm()?.Close();
            };

            btnConfirmCreate.Click += (s, ev) =>
            {
                try
                {
                    var created = _orderSvc.Create(_draft);
                    OrderCreated?.Invoke(this, EventArgs.Empty);
                    this.FindForm()?.Close();
                }
                catch (Exception ex)
                {
                    // (Sử dụng code hiển thị lỗi chi tiết bạn đã thêm)
                    string errorMessage = "Lỗi khi tạo đơn hàng:\n";
                    errorMessage += $"Lỗi chính: {ex.Message}\n\n";
                    Exception inner = ex.InnerException;
                    int count = 1;
                    while (inner != null)
                    {
                        errorMessage += $"Chi tiết cấp {count}: {inner.Message}\n";
                        inner = inner.InnerException;
                        count++;
                    }
                    MessageBox.Show(errorMessage, "Lỗi Chi Tiết", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }
    }
}