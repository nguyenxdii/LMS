using LMS.DAL.Models;

namespace LMS.BUS.Dtos
{
    public class VehicleListItemDto
    {
        public int Id { get; set; }
        public string PlateNo { get; set; }
        public string Type { get; set; }
        public int? CapacityKg { get; set; }
        public string StatusText { get; set; } // Hiển thị tiếng Việt
        public string DriverName { get; set; } // Tên tài xế
        public VehicleStatus StatusEnum { get; set; } // Giữ Enum để sort/filter nếu cần
        public int? DriverId { get; set; } // Giữ ID để kiểm tra gán/gỡ
    }
}