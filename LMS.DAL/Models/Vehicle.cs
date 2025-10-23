using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.DAL.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required, StringLength(15)]
        public string PlateNo { get; set; }

        [StringLength(50)]
        public string Model { get; set; }    // ví dụ: Isuzu QKR, Hino 300...

        public decimal CapacityKg { get; set; }
        public decimal VolumeM3 { get; set; }

        public bool IsAvailable { get; set; } = true;

        // liên kết tài xế hiện tại (nếu có)
        public virtual ICollection<Driver> Drivers { get; set; } = new HashSet<Driver>();
    }
}
