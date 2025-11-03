using LMS.BUS.Dtos;
using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS.BUS.Services
{
    public class StatisticsService
    {
        private readonly LogisticsDbContext db = new LogisticsDbContext();

        // kpi tổng quan
        public KpiDto GetKpis(DateTime from, DateTime to)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            var totalOrders = db.Orders.Count(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate);

            var inProgress = db.Shipments.Count(s =>
                s.StartedAt >= fromDate && s.StartedAt <= toDate &&
                s.Status != ShipmentStatus.Delivered &&
                s.Status != ShipmentStatus.Failed
            );

            var completed = db.Shipments.Count(s =>
                s.Status == ShipmentStatus.Delivered &&
                s.DeliveredAt >= fromDate && s.DeliveredAt <= toDate
            );

            var revenue = db.Shipments
                .Where(s => s.Status == ShipmentStatus.Delivered &&
                            s.DeliveredAt >= fromDate &&
                            s.DeliveredAt <= toDate &&
                            s.Order != null)
                .Sum(s => (decimal?)s.Order.TotalFee) ?? 0m;

            return new KpiDto
            {
                TotalOrders = totalOrders,
                ShipmentsInProgress = inProgress,
                ShipmentsCompleted = completed,
                TotalRevenue = revenue
            };
        }

        // pie: trạng thái đơn hàng
        public List<ChartDataPoint> GetOrderStatusCounts(DateTime from, DateTime to)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            var ef = db.Orders
                .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate)
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            return ef.Select(x => new ChartDataPoint
            {
                Label = FormatOrderStatus(x.Status),
                Value = x.Count
            })
            .OrderBy(d => d.Label)
            .ToList();
        }

        // line/column: doanh thu theo thời gian
        public List<TimeSeriesDataPoint> GetRevenueOverTime(DateTime from, DateTime to)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            var rows = db.Shipments
                .Where(s => s.Status == ShipmentStatus.Delivered &&
                            s.DeliveredAt >= fromDate &&
                            s.DeliveredAt <= toDate &&
                            s.Order != null)
                .Select(s => new
                {
                    DeliveredDate = DbFunctions.TruncateTime(s.DeliveredAt.Value),
                    TotalFee = s.Order.TotalFee
                })
                .ToList();

            var grouped = rows
                .GroupBy(x => x.DeliveredDate)
                .Select(g => new TimeSeriesDataPoint
                {
                    Date = g.Key.Value,
                    Value = g.Sum(x => x.TotalFee)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return grouped;
        }

        // pie: trạng thái shipment
        public List<ChartDataPoint> GetShipmentStatusCounts(DateTime from, DateTime to)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            var ef = db.Shipments
                .Where(s => s.UpdatedAt >= fromDate && s.UpdatedAt <= toDate)
                .GroupBy(s => s.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            return ef.Select(x => new ChartDataPoint
            {
                Label = FormatShipmentStatus(x.Status),
                Value = x.Count
            })
            .OrderBy(d => d.Label)
            .ToList();
        }

        // bar horizontal: top routes
        public List<ChartDataPoint> GetTopRoutes(DateTime from, DateTime to, int topN = 5)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            return db.Orders
                .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate)
                .GroupBy(o => new { Origin = o.OriginWarehouse.Name, Dest = o.DestWarehouse.Name })
                .Select(g => new ChartDataPoint
                {
                    Label = g.Key.Origin + " → " + g.Key.Dest,
                    Value = g.Count()
                })
                .OrderByDescending(x => x.Value)
                .Take(topN)
                .ToList();
        }

        // bar horizontal: top customers
        public List<ChartDataPoint.TopCustomerDto> GetTopCustomers(DateTime from, DateTime to, int topN = 5)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            return db.Shipments
                .Where(s => s.Status == ShipmentStatus.Delivered &&
                            s.DeliveredAt >= fromDate &&
                            s.DeliveredAt <= toDate &&
                            s.Order != null &&
                            s.Order.Customer != null)
                .GroupBy(s => s.Order.Customer)
                .Select(g => new ChartDataPoint.TopCustomerDto
                {
                    CustomerId = g.Key.Id,
                    CustomerName = g.Key.Name,
                    Phone = g.Key.Phone,
                    Email = g.Key.Email,
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Sum(s => s.Order.TotalFee)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(topN)
                .ToList();
        }

        // bar horizontal: top drivers
        public List<ChartDataPoint.TopDriverDto> GetTopDrivers(DateTime from, DateTime to, int topN = 5)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            return db.Shipments
                .Where(s => s.Status == ShipmentStatus.Delivered &&
                            s.DeliveredAt >= fromDate &&
                            s.DeliveredAt <= toDate &&
                            s.DriverId != null)
                .GroupBy(s => s.Driver)
                .Select(g => new ChartDataPoint.TopDriverDto
                {
                    DriverId = g.Key.Id,
                    DriverName = g.Key.FullName,
                    Phone = g.Key.Phone,
                    LicenseType = g.Key.LicenseType,
                    VehiclePlate = g.Key.Vehicle != null ? g.Key.Vehicle.PlateNo : "",
                    TotalShipments = g.Count()
                })
                .OrderByDescending(x => x.TotalShipments)
                .Take(topN)
                .ToList();
        }

        // bảng: trạng thái đơn hàng
        public List<OrderStatusDetailDto> GetOrderStatusDetails(DateTime from, DateTime to)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            return db.Orders
                .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate)
                .Include(o => o.Customer)
                .Select(o => new OrderStatusDetailDto
                {
                    OrderNo = o.OrderNo ?? ("ORD" + o.Id),
                    CustomerName = o.Customer != null ? o.Customer.Name : "",
                    Status = o.Status.ToString(),
                    TotalFee = o.TotalFee,
                    CreatedAt = o.CreatedAt
                })
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
        }

        // bảng: doanh thu chi tiết
        public List<RevenueDetailDto> GetRevenueDetails(DateTime from, DateTime to)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            return db.Shipments
                .Where(s => s.Status == ShipmentStatus.Delivered &&
                            s.DeliveredAt >= fromDate &&
                            s.DeliveredAt <= toDate &&
                            s.Order != null &&
                            s.Order.Customer != null)
                .Select(s => new RevenueDetailDto
                {
                    OrderNo = s.Order.OrderNo ?? ("ORD" + s.Order.Id),
                    CustomerName = s.Order.Customer.Name,
                    DeliveredAt = s.DeliveredAt.Value,
                    TotalFee = s.Order.TotalFee
                })
                .OrderByDescending(o => o.DeliveredAt)
                .ToList();
        }

        // format helpers
        private string FormatOrderStatus(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Pending: return "Chờ duyệt";
                case OrderStatus.Approved: return "Đã duyệt";
                case OrderStatus.Completed: return "Hoàn thành";
                case OrderStatus.Cancelled: return "Đã hủy";
                default: return status.ToString();
            }
        }

        private string FormatShipmentStatus(ShipmentStatus status)
        {
            switch (status)
            {
                case ShipmentStatus.Pending: return "Chờ nhận";
                case ShipmentStatus.Assigned: return "Đã nhận";
                case ShipmentStatus.OnRoute: return "Đang đi đường";
                case ShipmentStatus.AtWarehouse: return "Đang ở kho";
                case ShipmentStatus.ArrivedDestination: return "Đã tới đích";
                case ShipmentStatus.Delivered: return "Đã giao xong";
                case ShipmentStatus.Failed: return "Gặp sự cố";
                default: return status.ToString();
            }
        }
    }
}