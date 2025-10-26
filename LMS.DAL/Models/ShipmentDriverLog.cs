// LMS.DAL/Models/ShipmentDriverLog.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.DAL.Models
{
    public class ShipmentDriverLog
    {
        public int Id { get; set; }

        [Required]
        public int ShipmentId { get; set; } // Chuyến hàng bị đổi tài xế

        public int? OldDriverId { get; set; } // Tài xế cũ (có thể null nếu là lần gán đầu tiên)

        [Required]
        public int NewDriverId { get; set; } // Tài xế mới được gán

        [Required]
        public DateTime Timestamp { get; set; } // Thời điểm thay đổi

        public int? StopSequenceNumber { get; set; } // Chặng dừng hiện tại lúc đổi (optional)

        [StringLength(200)]
        public string Reason { get; set; } // Lý do thay đổi (optional)

        // Navigation properties (optional but recommended)
        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }

        [ForeignKey("OldDriverId")]
        public virtual Driver OldDriver { get; set; }

        [ForeignKey("NewDriverId")]
        public virtual Driver NewDriver { get; set; }
    }
}