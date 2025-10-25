using LMS.BUS.Dtos;
using LMS.BUS.Helpers;            // <-- thêm để dùng OrderCode
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
                db.SaveChanges(); // cần Id để sinh mã

                // GÁN MÃ ĐƠN CHÍNH THỨC rồi lưu lại (để nơi nào đọc OrderNo cũng có)
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
                          .Include("OriginWarehouse")
                          .Include("DestWarehouse")
                          .Where(o => o.CustomerId == customerId);

                if (status.HasValue) q = q.Where(o => o.Status == status.Value);

                return q.OrderByDescending(o => o.CreatedAt).ToList();
            }
        }

        //public Order GetOrderWithStops(int orderId)
        //{
        //    using (var db = new LogisticsDbContext())
        //    {
        //        return db.Orders
        //                 .Include("OriginWarehouse")
        //                 .Include("DestWarehouse")
        //                 .Include("Shipment.RouteStops.Warehouse")
        //                 .FirstOrDefault(o => o.Id == orderId);
        //    }
        //}
        // ============chay ngol r
        //public Order GetOrderWithStops(int orderId)
        //{
        //    using (var db = new LogisticsDbContext())
        //    {
        //        // Eager load như bình thường
        //        var order = db.Orders
        //                      .Include("OriginWarehouse")
        //                      .Include("DestWarehouse")
        //                      .Include("Shipment")
        //                      .Include("Shipment.RouteStops")
        //                      .Include("Shipment.RouteStops.Warehouse")
        //                      .FirstOrDefault(o => o.Id == orderId);

        //        // Fallback: nếu Shipment vẫn null nhưng có ShipmentId -> nạp thủ công
        //        if (order != null && order.Shipment == null && order.ShipmentId.HasValue)
        //        {
        //            order.Shipment = db.Shipments
        //                               .Include("RouteStops")
        //                               .Include("RouteStops.Warehouse")
        //                               .FirstOrDefault(s => s.Id == order.ShipmentId.Value);
        //        }

        //        return order;
        //    }
        //}
        // LMS.BUS/Services/OrderService_Customer.cs
        public Order GetOrderWithStops(int customerId, int orderId)
        {
            using (var db = new LogisticsDbContext())
            {
                var order = db.Orders
                              .Include("OriginWarehouse")
                              .Include("DestWarehouse")
                              .Include("Shipment")
                              .Include("Shipment.RouteStops")
                              .Include("Shipment.RouteStops.Warehouse")
                              .FirstOrDefault(o => o.CustomerId == customerId && o.Id == orderId);

                if (order != null && order.Shipment == null && order.ShipmentId.HasValue)
                {
                    order.Shipment = db.Shipments
                                       .Include("RouteStops")
                                       .Include("RouteStops.Warehouse")
                                       .FirstOrDefault(s => s.Id == order.ShipmentId.Value);
                }
                return order;
            }
        }

        // Tìm theo OrderNo, chấp nhận cả "03046" lẫn "ORD03046"
        //public Order GetOrderWithStopsByOrderNo(int customerId, string orderNoRaw)
        //{
        //    using (var db = new LogisticsDbContext())
        //    {
        //        var raw = (orderNoRaw ?? "").Trim().ToUpperInvariant();
        //        // chuẩn hóa: nếu người dùng gõ "03046" thì bổ sung "ORD"
        //        var withOrd = raw.StartsWith("ORD") ? raw : ("ORD" + raw);

        //        var order = db.Orders
        //                      .Include("OriginWarehouse")
        //                      .Include("DestWarehouse")
        //                      .Include("Shipment.RouteStops.Warehouse")
        //                      .FirstOrDefault(o => o.CustomerId == customerId &&
        //                                           (o.OrderNo.ToUpper() == withOrd));

        //        // fallback: nếu vẫn còn ShipmentId mà Shipment chưa load
        //        if (order != null && order.Shipment == null && order.ShipmentId.HasValue)
        //        {
        //            order.Shipment = db.Shipments
        //                               .Include("RouteStops")
        //                               .Include("RouteStops.Warehouse")
        //                               .FirstOrDefault(s => s.Id == order.ShipmentId.Value);
        //        }
        //        return order;
        //    }
        //}
        // Tìm theo OrderNo, chấp nhận cả "03046" lẫn "ORD03046"
        //public Order GetOrderWithStopsByOrderNo(int customerId, string orderNoRaw)
        //{
        //    using (var db = new LogisticsDbContext())
        //    {
        //        var raw = (orderNoRaw ?? "").Trim().ToUpperInvariant();
        //        // chuẩn hóa: nếu người dùng gõ "03046" thì bổ sung "ORD"
        //        var withOrd = raw.StartsWith("ORD") ? raw : ("ORD" + raw);

        //        var order = db.Orders
        //                      .Include("OriginWarehouse")
        //                      .Include("DestWarehouse")
        //                      .Include("Shipment.RouteStops.Warehouse")
        //                      .FirstOrDefault(o => o.CustomerId == customerId &&
        //                                           (o.OrderNo.ToUpper() == withOrd));

        //        // fallback: nếu vẫn còn ShipmentId mà Shipment chưa load
        //        if (order != null && order.Shipment == null && order.ShipmentId.HasValue)
        //        {
        //            order.Shipment = db.Shipments
        //                               .Include("RouteStops")
        //                               .Include("RouteStops.Warehouse")
        //                               .FirstOrDefault(s => s.Id == order.ShipmentId.Value);
        //        }
        //        return order;
        //    }
        //}
        // V2
        public Order GetOrderWithStopsByOrderNo(int customerId, string orderNoRaw)
        {
            using (var db = new LogisticsDbContext())
            {
                var raw = (orderNoRaw ?? "").Trim().ToUpperInvariant();
                var withOrd = raw.StartsWith("ORD") ? raw : ("ORD" + raw);

                var order = db.Orders
                              .Include("OriginWarehouse")
                              .Include("DestWarehouse")
                              .Include("Shipment.RouteStops.Warehouse")
                              .FirstOrDefault(o => o.CustomerId == customerId &&
                                                   (o.OrderNo.ToUpper() == withOrd || o.OrderNo.ToUpper() == raw));

                if (order != null && order.Shipment == null && order.ShipmentId.HasValue)
                {
                    order.Shipment = db.Shipments
                                       .Include("RouteStops")
                                       .Include("RouteStops.Warehouse")
                                       .FirstOrDefault(s => s.Id == order.ShipmentId.Value);
                }
                return order;
            }
        }




        public List<OrderListItemDto> GetAllByCustomer(int customerId)
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Orders
                         .AsNoTracking()
                         .Where(o => o.CustomerId == customerId)          // chỉ theo khách hàng
                         .OrderByDescending(o => o.CreatedAt)             // mới nhất lên đầu
                         .Select(o => new OrderListItemDto
                         {
                             Id = o.Id,
                             OrderNo = o.OrderNo,                          // hoặc OrderCode.PREFIX + o.Id
                             Pickup = o.PickupAddress,
                             //Dropoff = o.DropoffAddress,
                             //WeightKg = o.WeightKg,
                             //COD = o.COD,
                             Status = o.Status,
                             CreatedAt = o.CreatedAt
                         })
                         .ToList();
            }
        }

        // in OrderService_Customer
        //public Order GetOrderWithStopsByOrderNo(string orderNo)
        //{
        //    using (var db = new LogisticsDbContext())
        //    {
        //        return db.Orders
        //                 .Include("OriginWarehouse")
        //                 .Include("DestWarehouse")
        //                 .Include("Shipment.RouteStops.Warehouse")
        //                 .FirstOrDefault(o => o.OrderNo == orderNo);
        //    }
        //}


        public sealed class OrderListItemDto
        {
            public int Id { get; set; }
            public string OrderNo { get; set; }
            public string Pickup { get; set; }
            public string Dropoff { get; set; }
            public decimal WeightKg { get; set; }
            public decimal COD { get; set; }
            public OrderStatus Status { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
