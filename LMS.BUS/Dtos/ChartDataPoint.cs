using System;

namespace LMS.BUS.Dtos
{

    public class ChartDataPoint
    {

        public string Label { get; set; }

        public decimal Value { get; set; }
        // DTO cho biểu đồ đường (Doanh thu theo thời gian)
        public class TimeSeriesDataPoint
        {
            public DateTime Date { get; set; }
            public decimal Value { get; set; }
        }

        // DTO cho DataGridView Top Khách hàng
        public class TopCustomerDto
        {
            public int CustomerId { get; set; }
            public string CustomerName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public int TotalOrders { get; set; }
            public decimal TotalRevenue { get; set; }
        }

        // DTO cho DataGridView Top Tài xế
        public class TopDriverDto
        {
            public int DriverId { get; set; }
            public string DriverName { get; set; }
            public string Phone { get; set; }
            public string LicenseType { get; set; }
            public string VehiclePlate { get; set; } // Biển số xe
            public int TotalShipments { get; set; }
        }

        public class OrderStatusDetailDto
        {
            public string OrderNo { get; set; }
            public string CustomerName { get; set; }
            public string Status { get; set; } // Trạng thái (Pending, Approved...)
            public decimal TotalFee { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }

}