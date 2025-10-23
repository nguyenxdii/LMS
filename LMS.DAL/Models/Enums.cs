// LMS.DAL/Models/Enums.cs
namespace LMS.DAL.Models
{
    public enum UserRole { Admin = 0, Customer = 1, Driver = 2 }

    public enum OrderStatus
    {
        Pending = 0,    // Customer vừa tạo
        Approved = 1,   // Admin duyệt (có thể kèm create Shipment)
        InTransit = 2,  // Đang vận chuyển
        Completed = 3,  // Giao xong
        Cancelled = 4
    }

    public enum ShipmentStatus
    {
        Planned = 0,    // Admin lập kế hoạch, chưa start
        Assigned = 1,   // Đã gán Driver
        PickingUp = 2,  // (tuỳ chọn)
        OnRoute = 3,    // Đang di chuyển qua các kho
        Delivered = 4,  // Đã giao
        Failed = 5
    }

    public enum StopStatus
    {
        Waiting = 0, Reached = 1, Departed = 2, Skipped = 3, FinalDelivered = 4
    }

    public enum Zone
    {
        North = 0, Central = 1, South = 2
    }
}
