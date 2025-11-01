using System;

namespace LMS.BUS.Helpers
{
    /// <summary>
    /// Helper chuyển đổi trạng thái (enum/string) sang tiếng Việt
    /// Dùng chung cho Order, Shipment, Invoice, v.v.
    /// </summary>
    public static class StatusMapper
    {
        public static string ToVietnamese(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return "Không rõ";

            switch (status.Trim().ToLowerInvariant())
            {
                case "pending":
                case "created":
                case "new":
                    return "Mới tạo";

                case "approved":
                case "confirmed":
                    return "Đã duyệt";

                case "inprogress":
                case "processing":
                case "shipping":
                    return "Đang xử lý";

                case "completed":
                case "done":
                case "delivered":
                    return "Hoàn thành";

                case "cancelled":
                case "canceled":
                    return "Đã hủy";

                case "returned":
                    return "Đã trả hàng";

                default:
                    return status; // nếu đã là tiếng Việt hoặc giá trị khác
            }
        }
    }
}
