using System;

namespace LMS.BUS.Dtos
{
    public class OrderDraft
    {
        public int CustomerId { get; set; }
        public int OriginWarehouseId { get; set; }
        public int DestWarehouseId { get; set; }
        public bool NeedPickup { get; set; }
        public string PickupAddress { get; set; }
        public string PackageDescription { get; set; }
        public DateTime? DesiredTime { get; set; }

        public decimal RouteFee { get; set; }
        public decimal PickupFee { get; set; }
        public decimal TotalFee { get; set; }

        public decimal DepositPercent { get; set; } = 0.35m;
        public decimal DepositAmount { get; set; }
    }
}
