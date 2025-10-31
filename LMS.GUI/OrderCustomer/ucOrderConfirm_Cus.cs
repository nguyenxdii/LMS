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
            // --- Hiển thị thông tin (giữ nguyên) ---
            using (var db = new LogisticsDbContext())
            {
                var o = db.Warehouses.Find(_draft.OriginWarehouseId);
                var d = db.Warehouses.Find(_draft.DestWarehouseId);
                lblOrigin.Text = $"Kho gửi: {o?.Name} ({o?.ZoneId})";
                lblDest.Text = $"Kho nhận: {d?.Name} ({d?.ZoneId})";
            }
            lblPickup.Text = _draft.NeedPickup ? $"Pickup: Có (+{_draft.PickupFee:N0} đ) - {_draft.PickupAddress}" : "Pickup: Không"; // Show actual fee
            lblDesiredTime.Text = _draft.DesiredTime.HasValue ? $"Thời điểm: {_draft.DesiredTime:dd/MM/yyyy HH:mm}" : "Thời điểm: —";
            lblPackageDesc.Text = $"Mô tả: {(string.IsNullOrWhiteSpace(_draft.PackageDescription) ? "—" : _draft.PackageDescription)}"; // Handle empty description
            lblRouteFee.Text = $"Phí tuyến: {_draft.RouteFee:N0} đ"; // Add "Phí"
            lblPickupFee.Text = $"Phí Pickup: {_draft.PickupFee:N0} đ"; // Add "Phí"
            lblTotalFee.Text = $"TỔNG PHÍ: {_draft.TotalFee:N0} đ";
            lblDepositPercent.Text = $"Tỷ lệ cọc: {_draft.DepositPercent:P0}"; // Add "Tỷ lệ"
            lblDepositAmount.Text = $"Thanh toán ngay: {_draft.DepositAmount:N0} đ";
            lblRemainingAmount.Text = $"Còn lại khi nhận: {(_draft.TotalFee - _draft.DepositAmount):N0} đ";
            // --- Kết thúc hiển thị thông tin ---

            // --- Gán sự kiện cho các nút ---

            // Nút Quay lại (sửa để chỉ đóng popup)
            btnBack.Click += (s, ev) =>
            {
                // Chỉ cần đóng form popup chứa UserControl này
                this.FindForm()?.Close();
            };

            // Nút Xác nhận tạo đơn
            btnConfirmCreate.Click += (s, ev) =>
            {
                try
                {
                    // Gọi service tạo đơn
                    var created = _orderSvc.Create(_draft);

                    // Không cần MessageBox ở đây nữa, ucOrderCreate_Cus sẽ hiển thị
                    // MessageBox.Show(...) 

                    // 2. PHÁT SỰ KIỆN báo thành công
                    OrderCreated?.Invoke(this, EventArgs.Empty);

                    // 3. Tự đóng popup sau khi phát sự kiện
                    this.FindForm()?.Close();
                }
                //catch (Exception ex)
                //{
                //    MessageBox.Show($"Lỗi khi tạo đơn hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    // Không đóng popup nếu có lỗi
                //}

                catch (Exception ex)
                {
                    // Tạo một chuỗi thông báo lỗi chi tiết, đi sâu vào InnerException
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
                    // Không đóng popup nếu có lỗi
                }
            };

            // Nút Hủy (sửa để chỉ đóng popup)
            btnCancel.Click += (s, ev) =>
            {
                // Chỉ cần đóng form popup chứa UserControl này
                this.FindForm()?.Close();
            };
        }
    }
}