//namespace LMS.BUS.Dtos
//{
//    public class ShipmentRowDto
//    {
//        public int Id { get; set; }
//        public string ShipmentNo { get; set; }
//        public string OrderNo { get; set; }
//        public string Route { get; set; }
//        public string Status { get; set; }
//        public int Stops { get; set; }
//        public System.DateTime? StartedAt { get; set; }
//        public System.DateTime? DeliveredAt { get; set; }
//        public System.TimeSpan? Duration { get; set; }
//        public System.DateTime? UpdatedAt { get; set; }
//    }
//}

// LMS.BUS.Dtos/ShipmentRowDto.cs
namespace LMS.BUS.Dtos
{
    public class ShipmentRowDto
    {
        public int Id { get; set; }
        public string ShipmentNo { get; set; }
        public string OrderNo { get; set; }
        public string Route { get; set; }
        public string Status { get; set; }
        public int Stops { get; set; }
        public System.DateTime? StartedAt { get; set; }
        public System.DateTime? DeliveredAt { get; set; }
        public System.TimeSpan? Duration { get; set; }
        public System.DateTime? UpdatedAt { get; set; }

        // CÁC THUỘC TÍNH BỔ SUNG ĐỂ HIỂN THỊ CHI TIẾT
        public string CustomerName { get; set; } // Tên khách
        public string OriginWarehouse { get; set; } // Kho gửi
        public string DestinationWarehouse { get; set; } // Kho nhận
        // public string CurrentLocation { get; set; } // Vị trí (nếu cần)
    }
}