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

        // ====== LISTS ======
        public List<object> GetAssignedAndRunning(int driverId)
        {
            var active = new[] { ShipmentStatus.Pending, ShipmentStatus.Assigned, ShipmentStatus.OnRoute, ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination };

            var q = _db.Shipments
                .Include(s => s.Order.Customer)
                .Include(s => s.FromWarehouse)
                .Include(s => s.ToWarehouse)
                .Include(s => s.RouteStops)
                .Where(s => s.DriverId == driverId && active.Contains(s.Status))
                .OrderBy(s => s.Status).ThenByDescending(s => s.UpdatedAt)
                .Select(s => new
                {
                    Id = s.Id,
                    ShipmentNo = "SHP" + s.Id,
                    OrderNo = s.Order.OrderNo,
                    Route = s.FromWarehouse.Name + " → " + s.ToWarehouse.Name,
                    Status = s.Status.ToString(),
                    UpdatedAt = s.UpdatedAt
                });

            return q.ToList<object>();
        }

        public List<object> GetAllMine(int driverId, DateTime? from = null, DateTime? to = null, ShipmentStatus? status = null)
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
                    Id = s.Id,
                    ShipmentNo = "SHP" + s.Id,
                    OrderNo = s.Order.OrderNo,
                    Route = s.FromWarehouse.Name + " → " + s.ToWarehouse.Name,
                    Status = s.Status.ToString(),
                    Stops = s.RouteStops.Count(),
                    StartedAt = s.StartedAt,
                    DeliveredAt = s.DeliveredAt,
                    Duration = (TimeSpan?)((s.StartedAt.HasValue && s.DeliveredAt.HasValue) ? s.DeliveredAt.Value - s.StartedAt.Value : (TimeSpan?)null),
                    UpdatedAt = s.UpdatedAt
                }).ToList<object>();

            return list;
        }

        // ====== DETAIL ======
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

            if (s == null) throw new Exception("Shipment không tồn tại.");
            if (s.DriverId != driverId) throw new Exception("Bạn không có quyền xem shipment này.");

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
                    DeliveredAt = s.DeliveredAt
                },
                DriverName = s.Driver?.FullName,
                //VehicleNo = s.Vehicle?.PlateNumber,
                Notes = s.Note,
                Duration = (s.StartedAt.HasValue && s.DeliveredAt.HasValue) ? s.DeliveredAt.Value - s.StartedAt.Value : (TimeSpan?)null
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
                    StopStatus = r.Status.ToString()
                }).ToList();

            return dto;
        }

        // ====== WORKFLOW ======
        // 1) Driver nhận chuyến (Pending -> Assigned)
        public void ReceiveShipment(int shipmentId, int driverId)
        {
            var s = _db.Shipments.Include(x => x.RouteStops).FirstOrDefault(x => x.Id == shipmentId);
            if (s == null) throw new Exception("Shipment không tồn tại.");
            if (s.DriverId != driverId) throw new Exception("Không phải shipment của bạn.");
            if (s.Status != ShipmentStatus.Pending) throw new Exception("Chuyến đã được nhận hoặc đang chạy.");

            s.Status = ShipmentStatus.Assigned;
            s.CurrentStopSeq = s.RouteStops.OrderBy(x => x.Seq).Select(x => (int?)x.Seq).FirstOrDefault();
            s.UpdatedAt = DateTime.Now;
            _db.SaveChanges();
        }

        // 2) Rời kho hiện tại (Assigned/AtWarehouse -> OnRoute), đánh dấu Depart
        public void DepartCurrentStop(int shipmentId, int driverId)
        {
            var s = _db.Shipments.Include(x => x.RouteStops).FirstOrDefault(x => x.Id == shipmentId);
            if (s == null) throw new Exception("Shipment không tồn tại.");
            if (s.DriverId != driverId) throw new Exception("Không phải shipment của bạn.");
            if (!(s.Status == ShipmentStatus.Assigned || s.Status == ShipmentStatus.AtWarehouse))
                throw new Exception("Chỉ có thể rời kho khi đã nhận chuyến hoặc đang ở kho.");

            if (!s.CurrentStopSeq.HasValue) throw new Exception("Chưa xác định stop hiện tại.");
            var cur = s.RouteStops.FirstOrDefault(x => x.Seq == s.CurrentStopSeq.Value);
            if (cur == null) throw new Exception("Stop hiện tại không tồn tại.");

            // Không được depart nếu chưa arrive (trừ stop đầu tiên)
            if (cur.Seq > s.RouteStops.Min(x => x.Seq) && !cur.ArrivedAt.HasValue)
                throw new Exception("Phải Arrive stop hiện tại trước khi Depart.");

            cur.Status = RouteStopStatus.Departed;
            cur.DepartedAt = DateTime.Now;

            s.Status = ShipmentStatus.OnRoute;
            s.UpdatedAt = DateTime.Now;

            _db.SaveChanges();
        }

        // 3) Đến kho kế tiếp (OnRoute -> AtWarehouse hoặc ArrivedDestination)
        public void ArriveNextStop(int shipmentId, int driverId)
        {
            var s = _db.Shipments.Include(x => x.RouteStops).FirstOrDefault(x => x.Id == shipmentId);
            if (s == null) throw new Exception("Shipment không tồn tại.");
            if (s.DriverId != driverId) throw new Exception("Không phải shipment của bạn.");
            if (s.Status != ShipmentStatus.OnRoute) throw new Exception("Chỉ có thể Arrive khi đang di chuyển (OnRoute).");

            if (!s.CurrentStopSeq.HasValue) throw new Exception("Chưa xác định stop hiện tại.");

            var nextSeq = s.CurrentStopSeq.Value + 1;
            var next = s.RouteStops.FirstOrDefault(x => x.Seq == nextSeq);
            if (next == null) throw new Exception("Không còn stop kế tiếp.");

            next.Status = RouteStopStatus.Arrived;
            next.ArrivedAt = DateTime.Now;

            s.CurrentStopSeq = nextSeq;
            s.UpdatedAt = DateTime.Now;

            var isLast = (nextSeq == s.RouteStops.Max(x => x.Seq));
            s.Status = isLast ? ShipmentStatus.ArrivedDestination : ShipmentStatus.AtWarehouse;

            _db.SaveChanges();
        }

        // 4) Hoàn thành đơn tại kho cuối
        public void CompleteShipment(int shipmentId, int driverId)
        {
            var s = _db.Shipments.Include(x => x.RouteStops).FirstOrDefault(x => x.Id == shipmentId);
            if (s == null) throw new Exception("Shipment không tồn tại.");
            if (s.DriverId != driverId) throw new Exception("Không phải shipment của bạn.");
            if (s.Status != ShipmentStatus.ArrivedDestination) throw new Exception("Chỉ hoàn thành khi đã đến kho cuối.");

            s.Status = ShipmentStatus.Delivered;
            s.DeliveredAt = DateTime.Now;
            s.UpdatedAt = DateTime.Now;

            var ord = _db.Orders.Find(s.OrderId);
            if (ord != null) ord.Status = OrderStatus.Completed;

            _db.SaveChanges();
        }
    }
}
