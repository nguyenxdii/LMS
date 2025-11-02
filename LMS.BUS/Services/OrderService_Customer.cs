using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
using LMS.DAL;
using LMS.DAL.Models;              // Order, Shipment, RouteStop, enums
using System;
using System.Collections.Generic;
using System.Data.Entity;          // Include/AsNoTracking
using System.Linq;

namespace LMS.BUS.Services
{
    public class OrderService_Customer
    {
        private readonly RoutePricingService _pricing = new RoutePricingService();

        public Order Create(OrderDraft draft)
        {
            if (draft == null) throw new ArgumentNullException(nameof(draft));
            if (draft.CustomerId <= 0) throw new ArgumentException("CustomerId không hợp lệ.", nameof(draft));

            using (var db = new LogisticsDbContext())
            {
                var order = new Order
                {
                    CustomerId = draft.CustomerId,
                    OriginWarehouseId = draft.OriginWarehouseId,
                    DestWarehouseId = draft.DestWarehouseId,
                    NeedPickup = draft.NeedPickup,
                    PickupAddress = draft.PickupAddress,
                    PackageDescription = draft.PackageDescription,
                    DesiredTime = draft.DesiredTime,
                    RouteFee = _pricing.GetRouteFee(draft.OriginWarehouseId, draft.DestWarehouseId),
                    PickupFee = _pricing.GetPickupFee(draft.NeedPickup),
                    TotalFee = 0m,                // sẽ tính ngay dưới
                    DepositPercent = 0.35m,
                    DepositAmount = 0m,
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.Now
                };

                order.TotalFee = order.RouteFee + order.PickupFee;
                order.DepositAmount = Math.Round(order.TotalFee * order.DepositPercent, 0);

                db.Orders.Add(order);
                db.SaveChanges();                 // cần Id

                // ✅ Sinh mã đơn chính thức từ Id
                order.OrderNo = OrderCode.ToCode(order.Id);
                db.SaveChanges();

                return order;
            }
        }

        public List<Order> GetOrdersByCustomer(int customerId, OrderStatus? status = null)
        {
            using (var db = new LogisticsDbContext())
            {
                var q = db.Orders
                          .AsNoTracking()
                          .Include(o => o.OriginWarehouse)
                          .Include(o => o.DestWarehouse)
                          .Where(o => o.CustomerId == customerId);

                if (status.HasValue)
                    q = q.Where(o => o.Status == status.Value);

                return q.OrderByDescending(o => o.CreatedAt).ToList();
            }
        }

        public Order GetOrderWithStops(int customerId, int orderId)
        {
            using (var db = new LogisticsDbContext())
            {
                // Lấy Order + Shipment + RouteStops (+ Warehouse) trước
                var order = db.Orders
                              .AsNoTracking()
                              .Include(o => o.OriginWarehouse)
                              .Include(o => o.DestWarehouse)
                              .Include(o => o.Shipment)
                              .Include(o => o.Shipment.Driver)
                              .Include(o => o.Shipment.Vehicle)
                              .Include(o => o.Shipment.RouteStops.Select(rs => rs.Warehouse))
                              .FirstOrDefault(o => o.Id == orderId && o.CustomerId == customerId);

                if (order != null && order.Shipment == null && order.ShipmentId.HasValue)
                {
                    order.Shipment = db.Shipments
                                       .AsNoTracking()
                                       .Include(s => s.RouteStops.Select(rs => rs.Warehouse))
                                       .Include(s => s.Driver)
                                       .Include(s => s.Vehicle)
                                       .FirstOrDefault(s => s.Id == order.ShipmentId.Value);
                }

                return order;
            }
        }


        public List<OrderListItemDto> GetAllByCustomer(int customerId)
        {
            using (var db = new LogisticsDbContext())
            {
                var query = db.Orders
                              .AsNoTracking()
                              .Where(o => o.CustomerId == customerId)
                              .OrderByDescending(o => o.CreatedAt);

                var list = query.Select(o => new OrderListItemDto
                {
                    Id = o.Id,
                    OrderNo = o.OrderNo,   // UI có thể fallback bằng OrderCode.ToCode(Id) nếu null/empty
                    CreatedAt = o.CreatedAt,
                    Status = o.Status
                })
                .ToList();

                foreach (var it in list)
                    it.StatusVN = ToVietnamese(it.Status);

                return list;
            }
        }

        public List<OrderListItemDto> GetLatestOfCustomer(int customerId, int take = 5)
            => GetAllByCustomer(customerId).Take(take).ToList();

        public int CountByStatus(int customerId, OrderStatus status)
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Orders.AsNoTracking()
                                .Count(o => o.CustomerId == customerId && o.Status == status);
            }
        }

        public static string ToVietnamese(OrderStatus s)
        {
            switch (s)
            {
                case OrderStatus.Pending: return "Chờ duyệt";
                case OrderStatus.Approved: return "Đã duyệt";
                case OrderStatus.Completed: return "Hoàn tất";
                case OrderStatus.Cancelled: return "Đã hủy";
                default: return s.ToString();
            }
        }
    }
}
