using System;

namespace LMS.BUS.Dtos
{
    // Vận hành (Operations)
    public class ShipmentDetailDto1
    {
        public string ShipmentNo { get; set; }
        public string DriverName { get; set; }
        public string VehiclePlate { get; set; }
        public string Status { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    // Khách hàng (Customers)
    public class CustomerOrderDetailDto
    {
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public decimal TotalFee { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Tài xế (Drivers)
    public class DriverShipmentDetailDto
    {
        public string ShipmentNo { get; set; }
        public string DriverName { get; set; }
        public string VehiclePlate { get; set; }
        public string Status { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    // Doanh thu (Revenue)
    public class RevenueDetailDto
    {
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalFee { get; set; }
        public DateTime DeliveredAt { get; set; }
    }

    // Tổng quan (Overview)
    public class OrderStatusDetailDto
    {
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public decimal TotalFee { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Biểu đồ theo thời gian (Revenue Chart)
    public class TimeSeriesDataPoint
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }
}
