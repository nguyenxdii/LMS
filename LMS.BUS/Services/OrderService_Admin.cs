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
        // gán tài xế mới cho chuyến
        public void AssignDriver(int shipmentId, int newDriverId, string reason = null)
        {
            using (var db = new LogisticsDbContext())
            {
                var ship = db.Shipments.Include(s => s.Driver).FirstOrDefault(s => s.Id == shipmentId);
                if (ship == null) throw new Exception("không tìm thấy shipment.");

                int? oldDriverId = ship.DriverId;

                var newDriver = db.Drivers.FirstOrDefault(d => d.Id == newDriverId && d.IsActive);
                if (newDriver == null)
                    throw new Exception("tài xế mới không hợp lệ hoặc không hoạt động.");

                var activeShipmentStatuses = new[] {
                    ShipmentStatus.Assigned, ShipmentStatus.OnRoute,
                    ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination
                };

                bool isNewDriverBusy = db.Shipments.Any(s => s.DriverId == newDriverId
                                                         && s.Id != shipmentId
                                                         && activeShipmentStatuses.Contains(s.Status));
                if (isNewDriverBusy)
                    throw new Exception($"tài xế '{newDriver.FullName}' đang bận chạy chuyến khác.");

                var allowedStatuses = new[] {
                    ShipmentStatus.Assigned, ShipmentStatus.OnRoute,
                    ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination
                };

                if (!allowedStatuses.Contains(ship.Status))
                    throw new Exception("không thể đổi tài xế cho chuyến hàng ở trạng thái này (" + ship.Status.ToString() + ").");

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ship.DriverId = newDriverId;
                        ship.Status = ShipmentStatus.Pending; // reset về chờ nhận
                        ship.UpdatedAt = DateTime.Now;

                        var logEntry = new ShipmentDriverLog
                        {
                            ShipmentId = shipmentId,
                            OldDriverId = oldDriverId,
                            NewDriverId = newDriverId,
                            Timestamp = DateTime.Now,
                            StopSequenceNumber = ship.CurrentStopSeq,
                            Reason = reason
                        };
                        db.ShipmentDriverLogs.Add(logEntry);

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("lỗi khi cập nhật và ghi log đổi tài xế.", ex);
                    }
                }
            }
        }

        // danh sách đơn hàng cho admin
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
                        o.OrderNo,
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
                    OrderNo = string.IsNullOrWhiteSpace(o.OrderNo) ? OrderCode.ToCode(o.Id) : o.OrderNo,
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

        // tìm kiếm đơn hàng cho admin
        public List<OrderListItemDto> SearchForAdmin(
            int? customerId, OrderStatus? status, int? originId, int? destId,
            DateTime? from, DateTime? to, string codeOrId)
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
                if (!string.IsNullOrWhiteSpace(codeOrId))
                {
                    if (OrderCode.TryParseId(codeOrId, out int parsedId))
                        q = q.Where(o => o.Id == parsedId || o.OrderNo == codeOrId);
                    else
                        q = q.Where(o => o.OrderNo == codeOrId);
                }

                var raw = q.OrderByDescending(o => o.CreatedAt)
                    .Select(o => new
                    {
                        o.Id,
                        o.OrderNo,
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
                    OrderNo = string.IsNullOrWhiteSpace(o.OrderNo) ? OrderCode.ToCode(o.Id) : o.OrderNo,
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

        // lấy đơn hàng chi tiết
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

        // duyệt đơn
        public void Approve(int id)
        {
            using (var db = new LogisticsDbContext())
            {
                var o = db.Orders.Find(id);
                if (o == null) throw new Exception("không tìm thấy đơn.");
                if (o.Status != OrderStatus.Pending) throw new Exception("chỉ duyệt đơn trạng thái pending.");
                o.Status = OrderStatus.Approved;
                db.SaveChanges();
            }
        }

        // từ chối đơn
        public void Reject(int id, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new InvalidOperationException("vui lòng nhập lý do từ chối đơn hàng.");

            using (var db = new LogisticsDbContext())
            {
                var o = db.Orders.Find(id);
                if (o == null) throw new Exception("không tìm thấy đơn.");
                if (o.Status != OrderStatus.Pending) throw new Exception("chỉ từ chối đơn trạng thái pending.");
                o.Status = OrderStatus.Cancelled;
                o.CancelReason = reason;
                db.SaveChanges();
            }
        }

        // xóa đơn
        public void Delete(int id)
        {
            using (var db = new LogisticsDbContext())
            {
                var o = db.Orders.Find(id);
                if (o == null) return;
                if (o.Status != OrderStatus.Cancelled && o.Status != OrderStatus.Completed)
                    throw new Exception("chỉ xoá được đơn ở trạng thái 'đã hủy' hoặc 'hoàn thành'.");
                if (o.ShipmentId != null)
                    throw new Exception("đơn đã gắn shipment, không thể xoá.");
                db.Orders.Remove(o);
                db.SaveChanges();
            }
        }

        // tạo shipment từ đơn
        public int CreateShipmentFromOrder(int orderId, int driverId)
        {
            using (var db = new LogisticsDbContext())
            {
                var o = db.Orders
                          .Include(x => x.OriginWarehouse)
                          .Include(x => x.DestWarehouse)
                          .SingleOrDefault(x => x.Id == orderId);

                if (o == null) throw new Exception("không tìm thấy đơn.");
                if (o.Status != OrderStatus.Approved) throw new Exception("chỉ tạo shipment cho đơn approved.");

                var existedShip = db.Shipments.FirstOrDefault(s => s.OrderId == o.Id);
                if (existedShip != null)
                {
                    if (o.ShipmentId != existedShip.Id)
                    {
                        o.ShipmentId = existedShip.Id;
                        db.SaveChanges();
                    }
                    throw new Exception("đơn đã có shipment.");
                }

                var driver = db.Drivers
                    .Include(d => d.Vehicle)
                    .FirstOrDefault(d => d.Id == driverId && d.IsActive);

                if (driver == null)
                    throw new Exception("driver không hợp lệ hoặc không hoạt động.");
                if (driver.Vehicle == null)
                    throw new Exception("tài xế chưa được gán xe.");

                var unavailableStatuses = new[] {
                    ShipmentStatus.Pending,
                    ShipmentStatus.Assigned,
                    ShipmentStatus.OnRoute,
                    ShipmentStatus.AtWarehouse,
                    ShipmentStatus.ArrivedDestination
                };

                bool isDriverUnavailable = db.Shipments.Any(s => s.DriverId == driverId && unavailableStatuses.Contains(s.Status));
                if (isDriverUnavailable)
                    throw new InvalidOperationException($"tài xế '{driver.FullName}' đang có chuyến hàng khác (đang chờ nhận hoặc đang chạy).\nvui lòng chọn tài xế khác.");

                var tpl = db.RouteTemplates
                            .FirstOrDefault(t => t.FromWarehouseId == o.OriginWarehouseId
                                              && t.ToWarehouseId == o.DestWarehouseId);

                var stopWarehouseIds = new List<int>();
                if (tpl == null)
                {
                    stopWarehouseIds.Add(o.OriginWarehouseId);
                    stopWarehouseIds.Add(o.DestWarehouseId);
                }
                else
                {
                    var stops = db.RouteTemplateStops
                                  .Where(s => s.TemplateId == tpl.Id)
                                  .OrderBy(s => s.Seq)
                                  .ToList();
                    if (stops.Count < 2)
                        throw new Exception("routetemplatestop phải có ít nhất 2 chặng.");
                    stopWarehouseIds = stops.Select(s => s.WarehouseId ?? 0).ToList();
                    if (stopWarehouseIds.Any(id => id == 0))
                        throw new Exception("có routetemplatestop chưa gắn warehouseid.");
                }

                var ship = new Shipment
                {
                    OrderId = o.Id,
                    DriverId = driverId,
                    VehicleId = driver.Vehicle.Id,
                    FromWarehouseId = stopWarehouseIds.First(),
                    ToWarehouseId = stopWarehouseIds.Last(),
                    Status = ShipmentStatus.Pending,
                    UpdatedAt = DateTime.Now
                };
                db.Shipments.Add(ship);
                db.SaveChanges();

                int seq = 1;
                foreach (var wid in stopWarehouseIds)
                {
                    db.RouteStops.Add(new RouteStop
                    {
                        ShipmentId = ship.Id,
                        WarehouseId = wid,
                        Seq = seq++,
                        Status = RouteStopStatus.Waiting
                    });
                }
                db.SaveChanges();

                o.ShipmentId = ship.Id;
                db.SaveChanges();

                return ship.Id;
            }
        }

        // thống kê trạng thái đơn
        public Dictionary<OrderStatus, int> GetOrderStatusCounts()
        {
            using (var db = new LogisticsDbContext())
            {
                var counts = db.Orders
                               .GroupBy(o => o.Status)
                               .Select(g => new { Status = g.Key, Count = g.Count() })
                               .ToDictionary(x => x.Status, x => x.Count);

                foreach (OrderStatus status in Enum.GetValues(typeof(OrderStatus)))
                {
                    if (!counts.ContainsKey(status))
                    {
                        counts.Add(status, 0);
                    }
                }

                return counts;
            }
        }
    }
}