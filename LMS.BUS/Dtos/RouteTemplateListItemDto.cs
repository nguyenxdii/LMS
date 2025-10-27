namespace LMS.BUS.Dtos
{
    public class RouteTemplateListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FromWarehouseName { get; set; }
        public string ToWarehouseName { get; set; }
        public int StopsCount { get; set; }
        public string RoutePreview { get; set; }

    }
}