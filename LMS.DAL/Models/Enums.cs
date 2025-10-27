// LMS.DAL/Models/Enums.cs
using System.ComponentModel.DataAnnotations;

namespace LMS.DAL.Models
{
    public enum UserRole { Admin = 0, Customer = 1, Driver = 2 }

    public enum OrderStatus // (đơn hàng – cho Customer/Admin)
    {
        Pending = 0,    // Customer vừa tạo
        Approved = 1,   // Admin duyệt (có thể kèm create Shipment)
        //InTransit = 2,  // Đang vận chuyển
        Completed = 3,  // Giao xong
        Cancelled = 4
    }

    public enum ShipmentStatus  // (chuyến – cho Driver)
    {
        Pending = 0,        // Admin gán, driver chưa nhận
        Assigned = 1,       // Driver đã nhận
        OnRoute = 2,        // Đang di chuyển giữa 2 kho
        AtWarehouse = 3,    // Đang đứng tại 1 kho (đã Arrive)
        ArrivedDestination = 4, // Đã tới kho cuối
        Delivered = 5,      // Hoàn thành
        Failed = 6          // Sự cố/hủy
    }

    public enum StopStatus
    {
        Waiting = 0,
        Reached = 1,
        Departed = 2,
        Skipped = 3,
        FinalDelivered = 4
    }

    public enum Zone
    {
        North = 0,
        Central = 1,
        South = 2
    }

    public enum WarehouseType
    {
        Main = 0,   // Kho chính (trung tâm vùng hoặc tỉnh)
        Hub = 1,    // Kho trung chuyển giữa các vùng
        Local = 2   // Kho cấp huyện / xã (sau này mở rộng)
    }
    public enum RouteStopStatus     // (điểm dừng – chi tiết A→B→C…)
    {
        Waiting = 0,
        Arrived = 1,
        Departed = 2
    }
    public enum VehicleStatus
    {
        [Display(Name = "Hoạt động")]
        Active,
        [Display(Name = "Đang bảo trì")]
        Maintenance,
        [Display(Name = "Ngừng hoạt động")]
        Inactive
    }
}
