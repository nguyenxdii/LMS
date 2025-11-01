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
        //private readonly LmsDbContext _context;

        // ==== KPI OVERVIEW ====
        public KpiDto GetKpis(DateTime from, DateTime to)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            // 1) Tổng đơn tạo trong kỳ
            var totalOrders = db.Orders.Count(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate);

            // 2) Shipment đang vận hành (bắt đầu trong kỳ và chưa kết thúc)
            var inProgress = db.Shipments.Count(s =>
                s.StartedAt >= fromDate && s.StartedAt <= toDate &&
                s.Status != ShipmentStatus.Delivered &&
                s.Status != ShipmentStatus.Failed
            );

            // 3) Shipment giao thành công trong kỳ
            var completed = db.Shipments.Count(s =>
                s.Status == ShipmentStatus.Delivered &&
                s.DeliveredAt >= fromDate && s.DeliveredAt <= toDate
            );

            // 4) Doanh thu: tổng tiền các Order thuộc shipment đã giao trong kỳ
            //var revenue = db.Orders
            //    //.Include(o => o.Shipment)
            //    .Where(o => o.Shipment != null &&
            //                o.Shipment.Status == ShipmentStatus.Delivered &&
            //                o.Shipment.DeliveredAt >= fromDate &&
            //                o.Shipment.DeliveredAt <= toDate &&
            //                o.Order != null)
            //    .Sum(o => (decimal?)o.TotalFee) ?? 0m;
            var revenue = db.Shipments
                .Where(s => s.Status == ShipmentStatus.Delivered &&
                            s.DeliveredAt >= fromDate &&
                            s.DeliveredAt <= toDate &&
                            s.Order != null) // Đảm bảo có Order liên kết
                .Sum(s => (decimal?)s.Order.TotalFee) ?? 0m;

            return new KpiDto
            {
                TotalOrders = totalOrders,
                ShipmentsInProgress = inProgress,
                ShipmentsCompleted = completed,
                TotalRevenue = revenue
            };
        }

        // ==== PIE: TRẠNG THÁI ĐƠN HÀNG ====
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

        // ==== LINE/COLUMN: DOANH THU THEO THỜI GIAN ====
        public List<ChartDataPoint.TimeSeriesDataPoint> GetRevenueOverTime(DateTime from, DateTime to)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            //var rows = db.Orders
            //    .Where(o => o.Shipment != null &&
            //                o.Shipment.Status == ShipmentStatus.Delivered &&
            //                o.Shipment.DeliveredAt >= fromDate &&
            //                o.Shipment.DeliveredAt <= toDate)
            //    .Select(o => new
            //    {
            //        DeliveredDate = DbFunctions.TruncateTime(o.Shipment.DeliveredAt.Value),
            //        o.TotalFee
            //    })
            //    .ToList();
            var rows = db.Shipments // <-- Sửa: Bắt đầu từ Shipments
                .Where(s => s.Status == ShipmentStatus.Delivered && // <-- Sửa: Dùng s.Status
                            s.DeliveredAt >= fromDate &&
                            s.DeliveredAt <= toDate &&
                            s.Order != null) // <-- Thêm kiểm tra
                .Select(s => new // <-- Sửa: Select từ s (Shipment)
                {
                    DeliveredDate = DbFunctions.TruncateTime(s.DeliveredAt.Value),
                    TotalFee = s.Order.TotalFee // <-- Sửa: Lấy phí từ s.Order
                })
                .ToList();

            var grouped = rows
                .GroupBy(x => x.DeliveredDate)
                .Select(g => new ChartDataPoint.TimeSeriesDataPoint
                {
                    Date = g.Key.Value,
                    Value = g.Sum(x => x.TotalFee)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return grouped;
        }

        // ==== PIE: TRẠNG THÁI SHIPMENT (tab Vận hành) ====
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

        // ==== BAR HORIZONTAL: TOP ROUTES (đã FIX lỗi anonymous type) ====
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

        // ==== BAR HORIZONTAL: TOP CUSTOMERS ====
        public List<ChartDataPoint.TopCustomerDto> GetTopCustomers(DateTime from, DateTime to, int topN = 5)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            //return db.Orders
            //    .Where(o => o.Status == OrderStatus.Completed &&
            //                o.Shipment.DeliveredAt >= fromDate &&
            //                o.Shipment.DeliveredAt <= toDate)
            //    .GroupBy(o => o.Customer)
            //    .Select(g => new ChartDataPoint.TopCustomerDto
            //    {
            //        CustomerId = g.Key.Id,
            //        CustomerName = g.Key.Name,
            //        Phone = g.Key.Phone,
            //        Email = g.Key.Email,
            //        TotalOrders = g.Count(),
            //        TotalRevenue = g.Sum(o => o.TotalFee)
            //    })
            //    .OrderByDescending(x => x.TotalRevenue)
            //    .Take(topN)
            //    .ToList();
            return db.Shipments // <-- Sửa: Bắt đầu từ Shipments
                .Where(s => s.Status == ShipmentStatus.Delivered && // <-- Sửa: Dùng s.Status
                            s.DeliveredAt >= fromDate &&
                            s.DeliveredAt <= toDate &&
                            s.Order != null && // <-- Thêm kiểm tra
                            s.Order.Customer != null) // <-- Thêm kiểm tra
                .GroupBy(s => s.Order.Customer) // <-- Sửa: Group theo s.Order.Customer
                .Select(g => new ChartDataPoint.TopCustomerDto
                {
                    CustomerId = g.Key.Id,
                    CustomerName = g.Key.Name,
                    Phone = g.Key.Phone,
                    Email = g.Key.Email,
                    TotalOrders = g.Count(), // Đây là tổng số chuyến hàng (Shipment) đã giao
                    TotalRevenue = g.Sum(s => s.Order.TotalFee) // <-- Sửa: Sum TotalFee từ s.Order
                })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(topN)
                .ToList();
        }

        // ==== BAR HORIZONTAL: TOP DRIVERS ====
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
                    VehiclePlate = g.Key.Vehicle.PlateNo,
                    TotalShipments = g.Count()
                })
                .OrderByDescending(x => x.TotalShipments)
                .Take(topN)
                .ToList();
        }

        // --- THÊM HÀM MỚI NÀY ĐỂ LẤY DỮ LIỆU CHO BẢNG ---
        public List<ChartDataPoint.OrderStatusDetailDto> GetOrderStatusDetails(DateTime from, DateTime to)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);

            return db.Orders
                .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate)
                .Include(o => o.Customer) // Lấy kèm thông tin Khách hàng
                .Select(o => new ChartDataPoint.OrderStatusDetailDto
                {
                    OrderNo = o.OrderNo ?? ("ORD" + o.Id), // Dùng mã đơn hoặc tạo mã
                    CustomerName = o.Customer.Name,
                    Status = o.Status.ToString(), // Sẽ format lại ở RDLC hoặc Service
                    TotalFee = o.TotalFee,
                    CreatedAt = o.CreatedAt
                })
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
        }

        // ====== format helpers ======
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
