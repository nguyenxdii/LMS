// LMS.BUS/Services/OrderService.cs
using LMS.BUS.Dtos;
using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS.BUS.Services
{
    public class OrderService_Customer
    {
        private readonly RoutePricingService _pricing = new RoutePricingService();

        public Order Create(OrderDraft draft)
        {
            using (var db = new LogisticsDbContext())
            {
                var order = new Order
                {
                    CustomerId = draft.CustomerId,
                    OriginWarehouseId = draft.OriginWarehouseId,
                    DestWarehouseId = draft.DestWarehouseId,

                    NeedPickup = draft.NeedPickup,
                    PickupAddress = draft.NeedPickup ? draft.PickupAddress : null,

                    PackageDescription = draft.PackageDescription,
                    DesiredTime = draft.DesiredTime,

                    RouteFee = _pricing.GetRouteFee(draft.OriginWarehouseId, draft.DestWarehouseId),
                    PickupFee = _pricing.GetPickupFee(draft.NeedPickup),
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.Now
                };

                order.TotalFee = order.RouteFee + order.PickupFee;
                order.DepositPercent = 0.35m;
                order.DepositAmount = Math.Round(order.TotalFee * order.DepositPercent, 0);

                db.Orders.Add(order);
                db.SaveChanges();
                return order;
            }
        }

        public List<Order> GetOrdersByCustomer(int customerId, OrderStatus? status = null)
        {
            using (var db = new LogisticsDbContext())
            {
                var q = db.Orders
                          .Include("OriginWarehouse")
                          .Include("DestWarehouse")
                          .Where(o => o.CustomerId == customerId);

                if (status.HasValue) q = q.Where(o => o.Status == status.Value);

                return q.OrderByDescending(o => o.CreatedAt).ToList();
            }
        }

        public Order GetOrderWithStops(int orderId)
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Orders
                         .Include("OriginWarehouse")
                         .Include("DestWarehouse")
                         .Include("Shipment.RouteStops.Warehouse")
                         .FirstOrDefault(o => o.Id == orderId);
            }
        }
    }
}
