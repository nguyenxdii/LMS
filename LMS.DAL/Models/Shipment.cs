// LMS.DAL/Models/Shipment.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.DAL.Models
{
    public class Shipment
    {
        public int Id { get; set; }

        // Gán từ Admin
        public int? DriverId { get; set; }
        public virtual Driver Driver { get; set; }

        public ShipmentStatus Status { get; set; } = ShipmentStatus.Planned;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? StartedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public virtual ICollection<RouteStop> RouteStops { get; set; } = new HashSet<RouteStop>();
    }
}
