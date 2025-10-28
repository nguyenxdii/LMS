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

        public void AssignDriver(int shipmentId, int newDriverId, string reason = null)
        {
            using (var db = new LogisticsDbContext())
            {
                // Lấy thông tin chuyến hàng, bao gồm cả tài xế hiện tại
                var ship = db.Shipments.Include(s => s.Driver).FirstOrDefault(s => s.Id == shipmentId);
                if (ship == null) throw new Exception("Không tìm thấy shipment.");

                int? oldDriverId = ship.DriverId; // Lưu lại ID tài xế cũ

                // Kiểm tra tài xế mới
                var newDriver = db.Drivers.FirstOrDefault(d => d.Id == newDriverId && d.IsActive);
                if (newDriver == null)
                    throw new Exception("Tài xế mới không hợp lệ hoặc không hoạt động.");

                // Kiểm tra tài xế mới có đang rảnh không (logic tương tự GetAvailableDriversForAdmin)
                var activeShipmentStatuses = new[] {
                    ShipmentStatus.Assigned, ShipmentStatus.OnRoute,
                    ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination
                 };
                bool isNewDriverBusy = db.Shipments.Any(s => s.DriverId == newDriverId
                                                         && s.Id != shipmentId // Loại trừ chính shipment này
                                                         && activeShipmentStatuses.Contains(s.Status));
                if (isNewDriverBusy)
                    throw new Exception($"Tài xế '{newDriver.FullName}' đang bận chạy chuyến khác.");


                // Kiểm tra trạng thái chuyến hàng hiện tại (chỉ cho phép đổi khi đang chạy)
                var allowedStatuses = new[] {
                    ShipmentStatus.Assigned, ShipmentStatus.OnRoute,
                    ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination
                    // Có thể thêm Pending nếu muốn đổi ngay cả khi chưa nhận
                 };
                if (!allowedStatuses.Contains(ship.Status))
                    throw new Exception("Không thể đổi tài xế cho chuyến hàng ở trạng thái này (" + ship.Status.ToString() + ").");


                // *** BẮT ĐẦU TRANSACTION ***
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Cập nhật Shipment
                        ship.DriverId = newDriverId;
                        ship.Status = ShipmentStatus.Pending; // *** Reset về chờ nhận cho tài xế mới ***
                        ship.UpdatedAt = DateTime.Now;
                        // Không reset StartedAt, DeliveredAt, CurrentStopSeq

                        // 2. Ghi Log thay đổi
                        var logEntry = new ShipmentDriverLog
                        {
                            ShipmentId = shipmentId,
                            OldDriverId = oldDriverId, // Tài xế cũ
                            NewDriverId = newDriverId, // Tài xế mới
                            Timestamp = DateTime.Now,  // Thời điểm đổi
                            StopSequenceNumber = ship.CurrentStopSeq, // Chặng hiện tại lúc đổi
                            Reason = reason // Lý do (nếu có)
                        };
                        db.ShipmentDriverLogs.Add(logEntry);

                        // 3. Lưu tất cả thay đổi
                        db.SaveChanges();
                        transaction.Commit(); // Hoàn tất transaction
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Hoàn tác nếu có lỗi
                        // Ném lại lỗi hoặc xử lý thêm
                        throw new Exception("Lỗi khi cập nhật và ghi log đổi tài xế.", ex);
                    }
                }
                // *** KẾT THÚC TRANSACTION ***
            }
        }
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

                if (o.Status != OrderStatus.Pending && o.Status != OrderStatus.Cancelled)
                    throw new Exception("Chỉ xoá được đơn Pending/Cancelled.");

                if (o.ShipmentId != null)
                    throw new Exception("Đơn đã gắn Shipment, không thể xoá.");

                db.Orders.Remove(o);
                db.SaveChanges();
            }
        }
        public int CreateShipmentFromOrder(int orderId, int driverId)
        {
            using (var db = new LogisticsDbContext())
            {
                var o = db.Orders
                          .Include(x => x.OriginWarehouse)
                          .Include(x => x.DestWarehouse)
                          .SingleOrDefault(x => x.Id == orderId);

                if (o == null) throw new Exception("Không tìm thấy đơn.");
                if (o.Status != OrderStatus.Approved) throw new Exception("Chỉ tạo Shipment cho đơn Approved.");

                var existedShip = db.Shipments.FirstOrDefault(s => s.OrderId == o.Id);
                if (existedShip != null)
                {
                    if (o.ShipmentId != existedShip.Id)
                    {
                        o.ShipmentId = existedShip.Id;
                        db.SaveChanges();
                    }
                    throw new Exception("Đơn đã có Shipment.");
                }

                var drvOk = db.Drivers.Any(d => d.Id == driverId && d.IsActive);
                if (!drvOk) throw new Exception("Driver không hợp lệ hoặc không hoạt động.");

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
                        throw new Exception("RouteTemplateStop phải có ít nhất 2 chặng.");

                    stopWarehouseIds = stops.Select(s => s.WarehouseId ?? 0).ToList();
                    if (stopWarehouseIds.Any(id => id == 0))
                        throw new Exception("Có RouteTemplateStop chưa gắn WarehouseId.");
                }

                var ship = new Shipment
                {
                    OrderId = o.Id,
                    DriverId = driverId,
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