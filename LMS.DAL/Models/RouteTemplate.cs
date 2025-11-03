using LMS.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogisticsApp.DAL.Models
{
    public class RouteTemplate
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int FromWarehouseId { get; set; }

        [Required]
        public int ToWarehouseId { get; set; }

        public double DistanceKm { get; set; }

        public virtual Warehouse FromWarehouse { get; set; }
        public virtual Warehouse ToWarehouse { get; set; }
        public virtual ICollection<RouteTemplateStop> Stops { get; set; } = new HashSet<RouteTemplateStop>();
    }
}
