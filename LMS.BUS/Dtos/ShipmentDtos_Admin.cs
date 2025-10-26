// LMS.BUS/Dtos/ShipmentDtos_Admin.cs
using LMS.DAL.Models; // Cần cho ShipmentStatus
using System;

namespace LMS.BUS.Dtos
{
    // DTO cho grid chính và grid kết quả tìm kiếm của Admin
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
        // Thêm các trường khác nếu cần hiển thị/lọc
        public int OriginWarehouseId { get; set; }
        public int DestWarehouseId { get; set; }
    }
}