using System.ComponentModel.DataAnnotations;

namespace LMS.DAL.Models
{
    public class Warehouse
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; }

        [Required, StringLength(200)]
        public string Address { get; set; }

        public double? Lat { get; set; }
        public double? Lng { get; set; }

        [Required]
        public Zone ZoneId { get; set; }

        [Required]
        public WarehouseType Type { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
