// LMS.BUS/Dtos/DriverDtos.cs
using LMS.DAL.Models;
using System.Collections.Generic;

namespace LMS.BUS.Dtos
{
    // DTO dùng cho UC "B" (ucDriverEditor_Admin)
    public class DriverEditorDto
    {
        public int Id { get; set; } // 0 nếu là Add
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string CitizenId { get; set; } // Thêm trường CCCD
        public string LicenseType { get; set; } // Thêm trường Bằng lái
        public string Username { get; set; }
        public string Password { get; set; } // Chỉ set khi Add hoặc Reset
    }

    // DTO dùng cho UC "A" (ucDriverDetail_Admin)
    public class DriverDetailDto
    {
        public Driver Driver { get; set; }
        public UserAccount Account { get; set; }
        public List<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}