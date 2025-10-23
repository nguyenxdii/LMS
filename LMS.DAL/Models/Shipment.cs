using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.DAL.Models
{
    public class Shipment
    {
        public int Id { get; set; }

        // Liên kết đơn
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        // Tài xế & xe
        public int DriverId { get; set; }
        public virtual Driver Driver { get; set; }
        public int? VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        // Kho đầu/cuối
        public int FromWarehouseId { get; set; }
        public virtual Warehouse FromWarehouse { get; set; }
        public int ToWarehouseId { get; set; }
        public virtual Warehouse ToWarehouse { get; set; }

        // Trạng thái & mốc thời gian
        public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
        public DateTime? StartedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Note { get; set; }

        // Điểm hiện tại theo Seq
        public int? CurrentStopSeq { get; set; }

        // Tuyến thực tế của shipment
        public virtual ICollection<RouteStop> RouteStops { get; set; } = new HashSet<RouteStop>();
    }
}
