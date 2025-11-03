using LMS.BUS.Dtos;
using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS.BUS.Services
{
    public class DriverShipmentService
    {
        private readonly LogisticsDbContext _db = new LogisticsDbContext();

        // trạng thái đang hoạt động
        private static readonly ShipmentStatus[] ActiveStatuses =
        {
            ShipmentStatus.Pending,
            ShipmentStatus.Assigned,
            ShipmentStatus.OnRoute,
            ShipmentStatus.AtWarehouse,
            ShipmentStatus.ArrivedDestination
        };

        // ====== danh sách chuyến ======
        public List<ShipmentRowDto> GetAssignedAndRunning(int driverId)
        {
            var q = _db.Shipments
                .Include(s => s.Order.Customer)
                .Include(s => s.FromWarehouse)
                .Include(s => s.ToWarehouse)
                .Include(s => s.RouteStops)
                .Where(s => s.DriverId == driverId && ActiveStatuses.Contains(s.Status))
                .OrderBy(s => s.Status)
                .ThenByDescending(s => s.UpdatedAt)
                .Select(s => new ShipmentRowDto
                {
                    Id = s.Id,
                    ShipmentNo = "SHP" + s.Id,
                    OrderNo = s.Order.OrderNo ?? ("ORD" + s.OrderId),
                    Route = s.FromWarehouse.Name + " → " + s.ToWarehouse.Name,
                    Status = s.Status.ToString(),
                    Stops = s.RouteStops.Count(),
                    StartedAt = s.StartedAt,
                    DeliveredAt = s.DeliveredAt,
                    UpdatedAt = s.UpdatedAt,
                    CustomerName = s.Order.Customer.Name,
                    OriginWarehouse = s.FromWarehouse.Name,
                    DestinationWarehouse = s.ToWarehouse.Name,
                    Duration = (TimeSpan?)null
                });

            return q.ToList();
        }

        public List<ShipmentRowDto> GetAllMine(int driverId, DateTime? from = null, DateTime? to = null, ShipmentStatus? status = null)
        {
            var q = _db.Shipments
                .Include(s => s.Order.Customer)
                .Include(s => s.FromWarehouse)
                .Include(s => s.ToWarehouse)
                .Include(s => s.RouteStops)
                .Where(s => s.DriverId == driverId);

            if (from.HasValue) q = q.Where(s => s.UpdatedAt >= from.Value);
            if (to.HasValue) q = q.Where(s => s.UpdatedAt < to.Value);
            if (status.HasValue) q = q.Where(s => s.Status == status.Value);

            var list = q.OrderByDescending(s => s.UpdatedAt)
                .Select(s => new
                {
                    s.Id,
                    ShipmentNo = "SHP" + s.Id,
                    OrderNo = s.Order.OrderNo ?? ("ORD" + s.OrderId),
                    Route = s.FromWarehouse.Name + " → " + s.ToWarehouse.Name,
                    Status = s.Status.ToString(),
                    Stops = s.RouteStops.Count(),
                    s.StartedAt,
                    s.DeliveredAt,
                    DurationSeconds = DbFunctions.DiffSeconds(s.StartedAt, s.DeliveredAt),
                    s.UpdatedAt,
                    CustomerName = s.Order.Customer.Name,
                    OriginWarehouse = s.FromWarehouse.Name,
                    DestinationWarehouse = s.ToWarehouse.Name
                })
                .AsEnumerable()
                .Select(s => new ShipmentRowDto
                {
                    Id = s.Id,
                    ShipmentNo = s.ShipmentNo,
                    OrderNo = s.OrderNo,
                    Route = s.Route,
                    Status = s.Status,
                    Stops = s.Stops,
                    StartedAt = s.StartedAt,
                    DeliveredAt = s.DeliveredAt,
                    Duration = s.DurationSeconds.HasValue
                        ? (TimeSpan?)TimeSpan.FromSeconds(s.DurationSeconds.Value)
                        : null,
                    UpdatedAt = s.UpdatedAt,
                    CustomerName = s.CustomerName,
                    OriginWarehouse = s.OriginWarehouse,
                    DestinationWarehouse = s.DestinationWarehouse
                })
                .ToList();

            return list;
        }

        // ====== chi tiết chuyến ======
        public ShipmentDetailDto GetDetail(int shipmentId, int driverId)
        {
            var s = _db.Shipments
                .Include(x => x.Order.Customer)
                .Include(x => x.FromWarehouse)
                .Include(x => x.ToWarehouse)
                .Include(x => x.RouteStops.Select(rs => rs.Warehouse))
                .Include(x => x.Driver)
                .Include(x => x.Vehicle)
                .FirstOrDefault(x => x.Id == shipmentId);

            if (s == null) throw new Exception("shipment không tồn tại.");
            if (s.DriverId != driverId) throw new Exception("bạn không có quyền xem shipment này.");

            var dto = new ShipmentDetailDto
            {
                Header = new ShipmentRunHeaderDto
                {
                    ShipmentId = s.Id,
                    ShipmentNo = "SHP" + s.Id,
                    OrderNo = s.Order?.OrderNo ?? ("ORD" + s.OrderId),
                    CustomerName = s.Order?.Customer?.Name,
                    Route = $"{s.FromWarehouse?.Name} → {s.ToWarehouse?.Name}",
                    Status = s.Status.ToString(),
                    CurrentStopSeq = s.CurrentStopSeq,
                    StartedAt = s.StartedAt,
                    DeliveredAt = s.DeliveredAt,
                },
                DriverName = s.Driver?.FullName,
                VehicleNo = s.Vehicle?.PlateNo,
                Notes = s.Note,
                Duration = (s.StartedAt.HasValue && s.DeliveredAt.HasValue)
                    ? s.DeliveredAt.Value - s.StartedAt.Value
                    : (TimeSpan?)null
            };

            dto.Stops = s.RouteStops
                .OrderBy(r => r.Seq)
                .Select(r => new RouteStopLiteDto
                {
                    RouteStopId = r.Id,
                    Seq = r.Seq,
                    StopName = r.StopName ?? r.Warehouse.Name,
                    PlannedETA = r.PlannedETA,
                    ArrivedAt = r.ArrivedAt,
                    DepartedAt = r.DepartedAt,
                    StopStatus = r.Status.ToString(),
                    Note = s.Note,
                })
                .ToList();

            return dto;
        }

        // ====== workflow ======
        public void ReceiveShipment(int shipmentId, int driverId)
        {
            var s = _db.Shipments.Include(x => x.RouteStops).FirstOrDefault(x => x.Id == shipmentId);
            if (s == null) throw new Exception("shipment không tồn tại.");
            if (s.DriverId != driverId) throw new Exception("không phải shipment của bạn.");
            if (s.Status != ShipmentStatus.Pending) throw new Exception("chỉ nhận shipment trạng thái pending.");

            var minSeq = s.RouteStops.Any() ? s.RouteStops.Min(x => x.Seq) : (int?)null;
            if (!minSeq.HasValue) throw new Exception("shipment chưa có route stops.");

            s.CurrentStopSeq = minSeq.Value;
            s.Status = ShipmentStatus.Assigned;
            s.UpdatedAt = DateTime.Now;
            s.StartedAt = s.StartedAt ?? DateTime.Now;

            _db.SaveChanges();
        }

        public void DepartCurrentStop(int shipmentId, int driverId)
        {
            var s = _db.Shipments.Include(x => x.RouteStops).FirstOrDefault(x => x.Id == shipmentId);
            if (s == null) throw new Exception("shipment không tồn tại.");
            if (s.DriverId != driverId) throw new Exception("không phải shipment của bạn.");
            if (!(s.Status == ShipmentStatus.Assigned || s.Status == ShipmentStatus.AtWarehouse))
                throw new Exception("chỉ có thể rời kho khi đã nhận chuyến hoặc đang ở kho.");
            if (!s.CurrentStopSeq.HasValue) throw new Exception("chưa xác định stop hiện tại.");

            var cur = s.RouteStops.FirstOrDefault(x => x.Seq == s.CurrentStopSeq.Value);
            if (cur == null) throw new Exception("stop hiện tại không tồn tại.");

            var minSeq = s.RouteStops.Min(x => x.Seq);
            if (cur.Seq > minSeq && !cur.ArrivedAt.HasValue)
                throw new Exception("phải 'đến kho' (arrive) stop hiện tại trước khi 'rời kho'.");
            if (cur.DepartedAt.HasValue)
                throw new Exception("stop hiện tại đã 'rời kho' trước đó.");

            cur.Status = RouteStopStatus.Departed;
            cur.DepartedAt = DateTime.Now;
            s.Status = ShipmentStatus.OnRoute;
            s.UpdatedAt = DateTime.Now;

            _db.SaveChanges();
        }

        public void ArriveNextStop(int shipmentId, int driverId)
        {
            var s = _db.Shipments.Include(x => x.RouteStops).FirstOrDefault(x => x.Id == shipmentId);
            if (s == null) throw new Exception("shipment không tồn tại.");
            if (s.DriverId != driverId) throw new Exception("không phải shipment của bạn.");
            if (s.Status != ShipmentStatus.OnRoute) throw new Exception("chỉ có thể 'đến kho' khi đang di chuyển (onroute).");
            if (!s.CurrentStopSeq.HasValue) throw new Exception("chưa xác định stop hiện tại.");

            var nextSeq = s.CurrentStopSeq.Value + 1;
            var next = s.RouteStops.FirstOrDefault(x => x.Seq == nextSeq);
            if (next == null) throw new Exception("không còn stop kế tiếp.");
            if (next.ArrivedAt.HasValue)
                throw new Exception("stop kế tiếp đã được 'đến' trước đó.");

            next.Status = RouteStopStatus.Arrived;
            next.ArrivedAt = DateTime.Now;
            s.CurrentStopSeq = nextSeq;
            s.UpdatedAt = DateTime.Now;

            var maxSeq = s.RouteStops.Max(x => x.Seq);
            s.Status = (nextSeq == maxSeq) ? ShipmentStatus.ArrivedDestination : ShipmentStatus.AtWarehouse;

            _db.SaveChanges();
        }

        public void CompleteShipment(int shipmentId, int driverId)
        {
            var s = _db.Shipments.Include(x => x.RouteStops).FirstOrDefault(x => x.Id == shipmentId);
            if (s == null) throw new Exception("shipment không tồn tại.");
            if (s.DriverId != driverId) throw new Exception("không phải shipment của bạn.");
            if (s.Status != ShipmentStatus.ArrivedDestination) throw new Exception("chỉ hoàn thành khi đã đến kho cuối.");

            s.Status = ShipmentStatus.Delivered;
            s.DeliveredAt = DateTime.Now;
            s.UpdatedAt = DateTime.Now;

            var ord = _db.Orders.Find(s.OrderId);
            if (ord != null) ord.Status = OrderStatus.Completed;

            _db.SaveChanges();
        }

        public void SaveStopNote(int shipmentId, int driverId, string noteContent)
        {
            var s = _db.Shipments
                       .Include(x => x.RouteStops)
                       .FirstOrDefault(x => x.Id == shipmentId);

            if (s == null) throw new Exception("shipment không tồn tại.");
            if (s.DriverId != driverId) throw new Exception("không phải shipment của bạn.");

            var activeStatuses = new[] { ShipmentStatus.Assigned, ShipmentStatus.OnRoute, ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination };
            if (!activeStatuses.Contains(s.Status))
            {
                throw new Exception("không thể ghi chú cho chuyến hàng ở trạng thái này.");
            }

            if (!s.CurrentStopSeq.HasValue)
            {
                throw new Exception("chưa xác định được chặng dừng hiện tại để lưu ghi chú.");
            }
            else
            {
                var currentStop = s.RouteStops.FirstOrDefault(rs => rs.Seq == s.CurrentStopSeq.Value);
                if (currentStop == null)
                {
                    throw new Exception($"không tìm thấy chặng dừng số {s.CurrentStopSeq.Value}.");
                }
                currentStop.Note = noteContent?.Trim();
            }

            s.UpdatedAt = DateTime.Now;
            _db.SaveChanges();
        }
    }
}