using LMS.DAL.Models;
using System;

namespace LMS.BUS.Dtos
{
    public class ShipmentListItemAdminDto
    {
        public int Id { get; set; }
        public string ShipmentNo { get; set; } // SHP...
        public string OrderNo { get; set; }    // ORD...
        public string DriverName { get; set; } // Tên tài xế
        public int? DriverId { get; set; }     // ID tài xế (để lọc)
        public string Route { get; set; }      // Kho đi -> Kho đến
        public ShipmentStatus Status { get; set; } // Giữ Enum để format/sort
        public DateTime? UpdatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public int OriginWarehouseId { get; set; }
        public int DestWarehouseId { get; set; }
    }
}