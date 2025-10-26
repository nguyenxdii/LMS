using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.DAL.Models
{
    public class Driver
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string FullName { get; set; }

        [Required, StringLength(15)]
        public string Phone { get; set; }
        [StringLength(12)] // CCCD có 12 số
        public string CitizenId { get; set; }

        // Loại bằng lái (B2, C, D, E, FC, ...)
        [Required, StringLength(10)]
        public string LicenseType { get; set; }

        public bool IsActive { get; set; } = true;

        // Gán xe sau này (nếu cần)
        public int? VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        public virtual ICollection<UserAccount> Accounts { get; set; } = new HashSet<UserAccount>();
        public virtual ICollection<Shipment> Shipments { get; set; } = new HashSet<Shipment>();
    }
}
