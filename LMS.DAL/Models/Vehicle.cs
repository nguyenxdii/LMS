// LMS.DAL/Models/Vehicle.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.DAL.Models
{
    // Giả định enum VehicleStatus được định nghĩa trong file Enums.cs
    // Nếu không, bạn cần định nghĩa nó ở đây hoặc trong Enums.cs:
    /*
    public enum VehicleStatus
    {
        [Display(Name = "Hoạt động")]
        Active,
        [Display(Name = "Đang bảo trì")]
        Maintenance,
        [Display(Name = "Ngừng hoạt động")]
        Inactive
    }
    */

    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Biển số xe là bắt buộc")]
        [StringLength(15)]
        [Index(IsUnique = true)] // Đảm bảo biển số là duy nhất
        public string PlateNo { get; set; }

        [Required(ErrorMessage = "Loại xe là bắt buộc")]
        [StringLength(50)]
        public string Type { get; set; } // Loại xe (vd: "Xe tải 1.5T")

        [Range(0, int.MaxValue, ErrorMessage = "Trọng tải phải là số nguyên dương")]
        public int? CapacityKg { get; set; } // Trọng tải (kg), cho phép null

        [Range(0, double.MaxValue, ErrorMessage = "Thể tích phải là số dương")]
        public decimal? VolumeM3 { get; set; } // Thể tích (m3), cho phép null

        [Required]
        public VehicleStatus Status { get; set; } // Trạng thái xe (sử dụng Enum)

        public DateTime? LastMaintenanceDate { get; set; } // Ngày bảo dưỡng cuối (tùy chọn)

        [StringLength(500)]
        public string Notes { get; set; } // Ghi chú thêm (tùy chọn)

        // --- Liên kết Tài xế (Một-Một tùy chọn, FK đặt ở Driver) ---
        // Thuộc tính điều hướng ngược lại từ Vehicle sang Driver
        // Khóa ngoại (VehicleId) được đặt trong lớp Driver
        public virtual Driver Driver { get; set; }

        // --- Liên kết Chuyến hàng (Một-Nhiều tùy chọn) ---
        // Thuộc tính điều hướng này tùy chọn, chỉ cần nếu bạn muốn truy vấn
        // danh sách các chuyến hàng của một xe cụ thể từ đối tượng Vehicle.
        // public virtual ICollection<Shipment> Shipments { get; set; } = new HashSet<Shipment>();
    }
}