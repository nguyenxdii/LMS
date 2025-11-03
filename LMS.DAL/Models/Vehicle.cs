using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.DAL.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Biển số xe là bắt buộc")]
        [StringLength(15)]
        [Index(IsUnique = true)]
        public string PlateNo { get; set; }

        [Required(ErrorMessage = "Loại xe là bắt buộc")]
        [StringLength(50)]
        public string Type { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Trọng tải phải là số nguyên dương")]
        public int? CapacityKg { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Thể tích phải là số dương")]
        public decimal? VolumeM3 { get; set; }

        [Required]
        public VehicleStatus Status { get; set; }

        public DateTime? LastMaintenanceDate { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public virtual Driver Driver { get; set; }
    }
}