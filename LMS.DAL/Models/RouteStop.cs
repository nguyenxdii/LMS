using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.DAL.Models
{
    public class RouteStop
    {
        public int Id { get; set; }

        public int ShipmentId { get; set; }
        public virtual Shipment Shipment { get; set; }

        public int? WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        [StringLength(200)]
        public string StopName { get; set; }

        public int Seq { get; set; }

        public DateTime? PlannedETA { get; set; }
        public DateTime? ArrivedAt { get; set; }
        public DateTime? DepartedAt { get; set; }
        [StringLength(500)]
        public string Note { get; set; }

        public RouteStopStatus Status { get; set; } = RouteStopStatus.Waiting;
    }
}
