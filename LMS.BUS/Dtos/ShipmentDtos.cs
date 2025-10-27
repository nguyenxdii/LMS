using System;
using System.Collections.Generic;

namespace LMS.BUS.Dtos
{
    public class ShipmentRunHeaderDto
    {
        public int ShipmentId { get; set; }
        public string ShipmentNo { get; set; }
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public string Route { get; set; }
        public string Status { get; set; }
        public int? CurrentStopSeq { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }

    public class RouteStopLiteDto
    {
        public int RouteStopId { get; set; }
        public int Seq { get; set; }
        public string StopName { get; set; }
        public DateTime? PlannedETA { get; set; }
        public DateTime? ArrivedAt { get; set; }
        public DateTime? DepartedAt { get; set; }
        public string StopStatus { get; set; }
    }

    public class ShipmentDetailDto
    {
        public ShipmentRunHeaderDto Header { get; set; }
        public List<RouteStopLiteDto> Stops { get; set; } = new List<RouteStopLiteDto>();
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string Notes { get; set; }
        public TimeSpan? Duration { get; set; }
        public List<ShipmentDriverLogDto> DriverHistory { get; set; } = new List<ShipmentDriverLogDto>();
    }
}
