// LMS.DAL/Models/Order.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.DAL.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        // NEW: Kho gửi – Kho nhận (warehouse→warehouse)
        [Required]
        [ForeignKey(nameof(OriginWarehouse))]
        public int OriginWarehouseId { get; set; }
        public virtual Warehouse OriginWarehouse { get; set; }

        [Required]
        [ForeignKey(nameof(DestWarehouse))]
        public int DestWarehouseId { get; set; }
        public virtual Warehouse DestWarehouse { get; set; }

        // NEW: Pickup tận nơi (tuỳ chọn)
        public bool NeedPickup { get; set; } = false;

        [StringLength(200)]
        public string PickupAddress { get; set; }   // chỉ cần khi NeedPickup=true

        // Mô tả hàng (kg/m3 không bắt buộc trong đồ án)
        [StringLength(200)]
        public string PackageDescription { get; set; }

        // Thời điểm mong muốn (đến kho / lấy hàng)
        public DateTime? DesiredTime { get; set; }

        // NEW: Phí
        [Range(0, 100000000)]
        public decimal RouteFee { get; set; }       // 100k/125k/150k
        [Range(0, 100000000)]
        public decimal PickupFee { get; set; }      // 0 hoặc 100k
        [Range(0, 100000000)]
        public decimal TotalFee { get; set; }       // Route + Pickup

        // NEW: COD đặt cọc 35%
        [Range(0, 1)]
        public decimal DepositPercent { get; set; } = 0.35m;

        [Range(0, 100000000)]
        public decimal DepositAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // (gắn Shipment sau)
        public int? ShipmentId { get; set; }
        public virtual Shipment Shipment { get; set; }
    }
}
