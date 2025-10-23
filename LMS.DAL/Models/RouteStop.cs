// LMS.DAL/Models/RouteStop.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.DAL.Models
{
    // Một điểm dừng trong tuyến (A->B->C->D)
    public class RouteStop
    {
        public int Id { get; set; }

        [Required]
        public int ShipmentId { get; set; }
        public virtual Shipment Shipment { get; set; }

        [Required]
        public int WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        // Thứ tự đi qua
        public int Sequence { get; set; }

        public StopStatus Status { get; set; } = StopStatus.Waiting;

        public DateTime? ArrivedAt { get; set; }
        public DateTime? DepartedAt { get; set; }

        // Nếu đây là kho đích, tick FinalDelivered khi giao xong
        public bool IsFinal { get; set; } = false;
    }
}
