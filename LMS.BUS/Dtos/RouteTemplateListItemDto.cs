// LMS.BUS/Dtos/RouteTemplateListItemDto.cs
namespace LMS.BUS.Dtos
{
    // DTO used for the main grid in ucRouteTemplate_Admin
    public class RouteTemplateListItemDto
    {
        public int Id { get; set; } // From RouteTemplate
        public string Name { get; set; } // From RouteTemplate
        public string FromWarehouseName { get; set; } // Joined from Warehouse
        public string ToWarehouseName { get; set; }   // Joined from Warehouse
        public int StopsCount { get; set; } // Calculated Count of RouteTemplateStop
        public string RoutePreview { get; set; }

    }
}