using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.DAL.Models
{
    public class RouteStop
    {
        public int Id { get; set; }

        public int ShipmentId { get; set; }
        public virtual Shipment Shipment { get; set; }

        // Nếu bạn muốn chỉ định warehouse
        public int? WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        // Hoặc tên điểm dừng tự do (ưu tiên dùng WarehouseId, StopName fallback)
        [StringLength(200)]
        public string StopName { get; set; }

        // BẮT BUỘC: thứ tự tuyến
        public int Seq { get; set; }

        public DateTime? PlannedETA { get; set; }
        public DateTime? ArrivedAt { get; set; }
        public DateTime? DepartedAt { get; set; }
        [StringLength(500)] // Giới hạn độ dài nếu cần
        public string Note { get; set; }

        public RouteStopStatus Status { get; set; } = RouteStopStatus.Waiting;
    }
}
