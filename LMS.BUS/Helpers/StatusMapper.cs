using LMS.DAL.Models;
using System;

namespace LMS.BUS.Helpers
{
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
        public static string ToShipmentVN(ShipmentStatus st)
        {
            switch (st)
            {
                case ShipmentStatus.Pending: return "Chờ nhận";
                case ShipmentStatus.Assigned: return "Đã nhận chuyến";
                case ShipmentStatus.OnRoute: return "Đang vận chuyển";
                case ShipmentStatus.AtWarehouse: return "Đang ở kho";
                case ShipmentStatus.ArrivedDestination: return "Đã tới đích";
                case ShipmentStatus.Delivered: return "Hoàn thành";
                case ShipmentStatus.Failed: return "Gặp sự cố / Hủy";
                default: return st.ToString();
            }
        }
        // Overload tiện dụng nếu bạn có string status (ví dụ từ DTO)
        public static string ToShipmentVN(string statusStr)
        {
            if (string.IsNullOrWhiteSpace(statusStr)) return "-";
            return System.Enum.TryParse(statusStr, out ShipmentStatus s)
                ? ToShipmentVN(s)
                : statusStr;
        }
        // Order (nếu cần dùng chung)
        public static string ToOrderVN(OrderStatus st)
        {
            switch (st)
            {
                case OrderStatus.Pending: return "Chờ duyệt";
                case OrderStatus.Approved: return "Đã duyệt";
                case OrderStatus.Completed: return "Hoàn thành";
                case OrderStatus.Cancelled: return "Đã hủy";
                default: return st.ToString();
            }
        }
    }
}