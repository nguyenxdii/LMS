using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.DAL.Models
{
    public class ShipmentDriverLog
    {
        public int Id { get; set; }

        [Required]
        public int ShipmentId { get; set; }

        public int? OldDriverId { get; set; }

        [Required]
        public int NewDriverId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public int? StopSequenceNumber { get; set; }

        [StringLength(200)]
        public string Reason { get; set; }

        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }

        [ForeignKey("OldDriverId")]
        public virtual Driver OldDriver { get; set; }

        [ForeignKey("NewDriverId")]
        public virtual Driver NewDriver { get; set; }
    }
}