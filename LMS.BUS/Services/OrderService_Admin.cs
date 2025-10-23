// LMS.BUS/Services/OrderService_Admin.cs
using LMS.BUS.Dtos;
using LMS.BUS.Helpers;
using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS.BUS.Services
{
    public class OrderService_Admin
    {
        public List<OrderListItemDto> GetAllForAdmin()
        {
            using (var db = new LogisticsDbContext())
            {
                var raw = db.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OriginWarehouse)
                    .Include(o => o.DestWarehouse)
                    .OrderByDescending(o => o.CreatedAt)
                    .Select(o => new
                    {
                        o.Id,
                        CustName = o.Customer.Name,
                        o.CustomerId,
                        OriginName = o.OriginWarehouse.Name,
                        o.OriginWarehouseId,
                        DestName = o.DestWarehouse.Name,
                        o.DestWarehouseId,
                        o.TotalFee,
                        o.DepositAmount,
                        o.Status,
                        o.CreatedAt
                    })
                    .ToList();

                return raw.Select(o => new OrderListItemDto
                {
                    Id = o.Id,
                    //OrderNo = o.Id.ToString("D8"), // 8 chữ số: 00003045
                    OrderNo = OrderCode.ToCode(o.Id), // Mã đơn: 03040
                    CustomerName = string.IsNullOrEmpty(o.CustName) ? ("#" + o.CustomerId) : o.CustName,
                    OriginWarehouseName = string.IsNullOrEmpty(o.OriginName) ? ("#" + o.OriginWarehouseId) : o.OriginName,
                    DestWarehouseName = string.IsNullOrEmpty(o.DestName) ? ("#" + o.DestWarehouseId) : o.DestName,
                    TotalFee = o.TotalFee,
                    DepositAmount = o.DepositAmount,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt
                }).ToList();
            }
        }

        public List<OrderListItemDto> SearchForAdmin(
            int? customerId, OrderStatus? status, int? originId, int? destId,
            DateTime? from, DateTime? to, string codeOrId
        )
        {
            using (var db = new LogisticsDbContext())
            {
                var q = db.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OriginWarehouse)
                    .Include(o => o.DestWarehouse)
                    .AsQueryable();

                if (customerId.HasValue) q = q.Where(o => o.CustomerId == customerId.Value);
                if (status.HasValue) q = q.Where(o => o.Status == status.Value);
                if (originId.HasValue) q = q.Where(o => o.OriginWarehouseId == originId.Value);
                if (destId.HasValue) q = q.Where(o => o.DestWarehouseId == destId.Value);
                if (from.HasValue) q = q.Where(o => DbFunctions.TruncateTime(o.CreatedAt) >= DbFunctions.TruncateTime(from.Value));
                if (to.HasValue) q = q.Where(o => DbFunctions.TruncateTime(o.CreatedAt) <= DbFunctions.TruncateTime(to.Value));

                // Nhập "Mã đơn" (OrderNo) thực chất là Id có padding.
                // Nếu người dùng gõ số -> lọc theo Id

                //if (!string.IsNullOrWhiteSpace(codeOrId) && int.TryParse(codeOrId.Trim(), out int id))
                //    q = q.Where(o => o.Id == id);
                if (!string.IsNullOrWhiteSpace(codeOrId))
                {
                    if (OrderCode.TryParseId(codeOrId, out int parsedId))
                        q = q.Where(o => o.Id == parsedId);
                }

                var raw = q.OrderByDescending(o => o.CreatedAt)
                    .Select(o => new
                    {
                        o.Id,
                        CustName = o.Customer.Name,
                        o.CustomerId,
                        OriginName = o.OriginWarehouse.Name,
                        o.OriginWarehouseId,
                        DestName = o.DestWarehouse.Name,
                        o.DestWarehouseId,
                        o.TotalFee,
                        o.DepositAmount,
                        o.Status,
                        o.CreatedAt
                    })
                    .ToList();

                return raw.Select(o => new OrderListItemDto
                {
                    Id = o.Id,
                    //OrderNo = o.Id.ToString("D8"),
                    OrderNo = OrderCode.ToCode(o.Id),
                    CustomerName = string.IsNullOrEmpty(o.CustName) ? ("#" + o.CustomerId) : o.CustName,
                    OriginWarehouseName = string.IsNullOrEmpty(o.OriginName) ? ("#" + o.OriginWarehouseId) : o.OriginName,
                    DestWarehouseName = string.IsNullOrEmpty(o.DestName) ? ("#" + o.DestWarehouseId) : o.DestName,
                    TotalFee = o.TotalFee,
                    DepositAmount = o.DepositAmount,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt
                }).ToList();
            }
        }

        public Order GetByIdWithAll(int id)
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OriginWarehouse)
                    .Include(o => o.DestWarehouse)
                    .Include(o => o.Shipment.RouteStops.Select(rs => rs.Warehouse))
                    .FirstOrDefault(o => o.Id == id);
            }
        }

        public void Approve(int id)
        {
            using (var db = new LogisticsDbContext())
            {
                var o = db.Orders.Find(id);
                if (o == null) throw new Exception("Không tìm thấy đơn.");
                if (o.Status != OrderStatus.Pending) throw new Exception("Chỉ duyệt đơn trạng thái Pending.");
                o.Status = OrderStatus.Approved;
                db.SaveChanges();
            }
        }

        public void Reject(int id)
        {
            using (var db = new LogisticsDbContext())
            {
                var o = db.Orders.Find(id);
                if (o == null) throw new Exception("Không tìm thấy đơn.");
                if (o.Status != OrderStatus.Pending) throw new Exception("Chỉ từ chối đơn trạng thái Pending.");
                o.Status = OrderStatus.Cancelled;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new LogisticsDbContext())
            {
                var o = db.Orders.Find(id);
                if (o == null) return;

                // Cho phép xoá nếu Pending hoặc Cancelled
                if (o.Status != OrderStatus.Pending && o.Status != OrderStatus.Cancelled)
                    throw new Exception("Chỉ xoá được đơn Pending/Cancelled.");

                if (o.ShipmentId != null)
                    throw new Exception("Đơn đã gắn Shipment, không thể xoá.");

                db.Orders.Remove(o);
                db.SaveChanges();
            }
        }

        //public int CreateShipmentFromOrder(int id)
        //{
        //    using (var db = new LogisticsDbContext())
        //    {
        //        var o = db.Orders.Find(id);
        //        if (o == null) throw new Exception("Không tìm thấy đơn.");
        //        if (o.Status != OrderStatus.Approved) throw new Exception("Chỉ tạo Shipment cho đơn Approved.");
        //        if (o.ShipmentId != null) throw new Exception("Đơn đã có Shipment.");

        //        var ship = new Shipment { Status = ShipmentStatus.Planned, CreatedAt = DateTime.Now };
        //        db.Shipments.Add(ship);
        //        db.SaveChanges();

        //        db.RouteStops.Add(new RouteStop
        //        {
        //            ShipmentId = ship.Id,
        //            WarehouseId = o.OriginWarehouseId,
        //            Sequence = 1,
        //            Status = StopStatus.Waiting,
        //            IsFinal = false
        //        });
        //        db.RouteStops.Add(new RouteStop
        //        {
        //            ShipmentId = ship.Id,
        //            WarehouseId = o.DestWarehouseId,
        //            Sequence = 2,
        //            Status = StopStatus.Waiting,
        //            IsFinal = true
        //        });

        //        o.ShipmentId = ship.Id;
        //        db.SaveChanges();
        //        return ship.Id;
        //    }
        //}

        public int CreateShipmentFromOrder(int id)
        {
            using (var db = new LogisticsDbContext())
            {
                var o = db.Orders.Find(id);
                if (o == null) throw new Exception("Không tìm thấy đơn.");
                if (o.Status != OrderStatus.Approved) throw new Exception("Chỉ tạo Shipment cho đơn Approved.");
                if (o.ShipmentId != null) throw new Exception("Đơn đã có Shipment.");

                // LƯU Ý: nếu Shipment.DriverId hiện đang NOT NULL trong DB,
                // bạn cần gán driver ở đây. Khuyên: đổi DriverId -> int? trong model.
                var ship = new Shipment
                {
                    OrderId = o.Id,
                    // DriverId = null, // nếu bạn đã cho phép nullable
                    FromWarehouseId = o.OriginWarehouseId,
                    ToWarehouseId = o.DestWarehouseId,
                    Status = ShipmentStatus.Pending,
                    UpdatedAt = DateTime.Now
                };
                db.Shipments.Add(ship);
                db.SaveChanges();

                // Tạo 2 stop A -> B, dùng đúng thuộc tính & enum của model:
                db.RouteStops.Add(new RouteStop
                {
                    ShipmentId = ship.Id,
                    WarehouseId = o.OriginWarehouseId,
                    Seq = 1,
                    Status = RouteStopStatus.Waiting
                });
                db.RouteStops.Add(new RouteStop
                {
                    ShipmentId = ship.Id,
                    WarehouseId = o.DestWarehouseId,
                    Seq = 2,
                    Status = RouteStopStatus.Waiting
                });
                db.SaveChanges();

                o.ShipmentId = ship.Id;
                db.SaveChanges();
                return ship.Id;
            }
        }


        //public static class OrderCode
        //{
        //    public const string PREFIX = "0304";

        //    public static string ToCode(int id) => PREFIX + id.ToString();         // 03041, 03042...
        //    public static bool TryParseId(string code, out int id)
        //    {
        //        id = 0;
        //        if (string.IsNullOrWhiteSpace(code)) return false;
        //        code = code.Trim();
        //        if (code.StartsWith(PREFIX)) code = code.Substring(PREFIX.Length);
        //        return int.TryParse(code, out id);
        //    }
        //}
    }
}
