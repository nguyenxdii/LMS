using LMS.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsApp.DAL.Models
{
    public class RouteTemplateStop
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Template))]
        public int TemplateId { get; set; }

        [Required]
        public int Seq { get; set; }

        public int? WarehouseId { get; set; }

        [StringLength(200)]
        public string StopName { get; set; }

        public double? PlannedOffsetHours { get; set; }

        [StringLength(200)]
        public string Note { get; set; }

        public virtual RouteTemplate Template { get; set; }

        [ForeignKey(nameof(WarehouseId))]
        public virtual Warehouse Warehouse { get; set; }
    }
}
