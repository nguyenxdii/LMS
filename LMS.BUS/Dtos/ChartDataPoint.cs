using System;

namespace LMS.BUS.Dtos
{
    public class ChartDataPoint
    {
        public string Label { get; set; }
        public decimal Value { get; set; }

        public class TopCustomerDto
        {
            public int CustomerId { get; set; }
            public string CustomerName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public int TotalOrders { get; set; }
            public decimal TotalRevenue { get; set; }
        }

        public class TopDriverDto
        {
            public int DriverId { get; set; }
            public string DriverName { get; set; }
            public string Phone { get; set; }
            public string LicenseType { get; set; }
            public string VehiclePlate { get; set; }
            public int TotalShipments { get; set; }
        }
    }
}
