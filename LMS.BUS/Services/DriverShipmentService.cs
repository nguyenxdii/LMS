//using LMS.BUS.Dtos;
//using LMS.DAL;
//using LMS.DAL.Models;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;

//namespace LMS.BUS.Services
//{
//    public class DriverShipmentService
//    {
//        private readonly LogisticsDbContext _db = new LogisticsDbContext();

//        // ====== LISTS ======
//        private static readonly ShipmentStatus[] ActiveStatuses =
//        {
//            ShipmentStatus.Pending, ShipmentStatus.Assigned, ShipmentStatus.OnRoute,
//            ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination
//        };

//        //public List<object> GetAssignedAndRunning(int driverId)
//        //{
//        //    var q = _db.Shipments
//        //        .Include(s => s.Order.Customer)
//        //        .Include(s => s.FromWarehouse)
//        //        .Include(s => s.ToWarehouse)
//        //        .Where(s => s.DriverId == driverId && ActiveStatuses.Contains(s.Status))
//        //        .OrderBy(s => s.Status).ThenByDescending(s => s.UpdatedAt)
//        //        .Select(s => new
//        //        {
//        //            Id = s.Id,
//        //            ShipmentNo = "SHP" + s.Id,
//        //            OrderNo = s.Order.OrderNo ?? ("ORD" + s.OrderId),
//        //            Route = s.FromWarehouse.Name + " → " + s.ToWarehouse.Name,
//        //            Status = s.Status.ToString(),
//        //            UpdatedAt = s.UpdatedAt
//        //        });

//        //    return q.ToList<object>();
//        //}
//        public List<ShipmentRowDto> GetAssignedAndRunning(int driverId)
//        {
//            var q = _db.Shipments
//                .Include(s => s.Order.Customer)
//                .Include(s => s.FromWarehouse)
//                .Include(s => s.ToWarehouse)
//                .Where(s => s.DriverId == driverId && ActiveStatuses.Contains(s.Status))
//                .OrderBy(s => s.Status).ThenByDescending(s => s.UpdatedAt)
//                .Select(s => new ShipmentRowDto
//                {
//                    Id = s.Id,
//                    ShipmentNo = "SHP" + s.Id,
//                    OrderNo = s.Order.OrderNo ?? ("ORD" + s.OrderId),
//                    Route = s.FromWarehouse.Name + " → " + s.ToWarehouse.Name,
//                    Status = s.Status.ToString(),
//                    UpdatedAt = s.UpdatedAt
//                });

//            return q.ToList();
//        }


//        //public List<object> GetAllMine(int driverId, DateTime? from = null, DateTime? to = null, ShipmentStatus? status = null)
//        //{
//        //    var q = _db.Shipments
//        //        .Include(s => s.Order.Customer)
//        //        .Include(s => s.FromWarehouse)
//        //        .Include(s => s.ToWarehouse)
//        //        .Include(s => s.RouteStops)
//        //        .Where(s => s.DriverId == driverId);

//        //    if (from.HasValue) q = q.Where(s => s.UpdatedAt >= from.Value);
//        //    if (to.HasValue) q = q.Where(s => s.UpdatedAt < to.Value);
//        //    if (status.HasValue) q = q.Where(s => s.Status == status.Value);

//        //    var list = q.OrderByDescending(s => s.UpdatedAt)
//        //        // B1: project các field + DurationSeconds để SQL có thể tính
//        //        .Select(s => new
//        //        {
//        //            Id = s.Id,
//        //            ShipmentNo = "SHP" + s.Id,
//        //            OrderNo = s.Order.OrderNo ?? ("ORD" + s.OrderId),
//        //            Route = s.FromWarehouse.Name + " → " + s.ToWarehouse.Name,
//        //            Status = s.Status.ToString(),
//        //            Stops = s.RouteStops.Count(),
//        //            StartedAt = s.StartedAt,
//        //            DeliveredAt = s.DeliveredAt,
//        //            DurationSeconds = DbFunctions.DiffSeconds(s.StartedAt, s.DeliveredAt),
//        //            UpdatedAt = s.UpdatedAt
//        //        })
//        //        // B2: qua LINQ to Objects để map về TimeSpan?
//        //        .AsEnumerable()
//        //        .Select(s => new
//        //        {
//        //            s.Id,
//        //            s.ShipmentNo,
//        //            s.OrderNo,
//        //            s.Route,
//        //            s.Status,
//        //            s.Stops,
//        //            s.StartedAt,
//        //            s.DeliveredAt,
//        //            Duration = s.DurationSeconds.HasValue
//        //                ? (TimeSpan?)TimeSpan.FromSeconds(s.DurationSeconds.Value)
//        //                : null,
//        //            s.UpdatedAt
//        //        })
//        //        .Cast<object>()
//        //        .ToList();

//        //    return list;
//        //}
//        public List<ShipmentRowDto> GetAllMine(int driverId, DateTime? from = null, DateTime? to = null, ShipmentStatus? status = null)
//        {
//            var q = _db.Shipments
//                .Include(s => s.Order.Customer)
//                .Include(s => s.FromWarehouse)
//                .Include(s => s.ToWarehouse)
//                .Include(s => s.RouteStops)
//                .Where(s => s.DriverId == driverId);

//            if (from.HasValue) q = q.Where(s => s.UpdatedAt >= from.Value);
//            if (to.HasValue) q = q.Where(s => s.UpdatedAt < to.Value);
//            if (status.HasValue) q = q.Where(s => s.Status == status.Value);

//            var list = q.OrderByDescending(s => s.UpdatedAt)
//                // B1: SQL tính DurationSeconds
//                .Select(s => new
//                {
//                    s.Id,
//                    ShipmentNo = "SHP" + s.Id,
//                    OrderNo = s.Order.OrderNo ?? ("ORD" + s.OrderId),
//                    Route = s.FromWarehouse.Name + " → " + s.ToWarehouse.Name,
//                    Status = s.Status.ToString(),
//                    Stops = s.RouteStops.Count(),
//                    s.StartedAt,
//                    s.DeliveredAt,
//                    DurationSeconds = DbFunctions.DiffSeconds(s.StartedAt, s.DeliveredAt),
//                    s.UpdatedAt
//                })
//                // B2: qua LINQ-to-Objects, map về DTO mạnh kiểu
//                .AsEnumerable()
//                .Select(s => new ShipmentRowDto
//                {
//                    Id = s.Id,
//                    ShipmentNo = s.ShipmentNo,
//                    OrderNo = s.OrderNo,
//                    Route = s.Route,
//                    Status = s.Status,
//                    Stops = s.Stops,
//                    StartedAt = s.StartedAt,
//                    DeliveredAt = s.DeliveredAt,
//                    Duration = s.DurationSeconds.HasValue ? (TimeSpan?)TimeSpan.FromSeconds(s.DurationSeconds.Value) : null,
//                    UpdatedAt = s.UpdatedAt
//                })
//                .ToList();

//            return list;
//        }


//        // ====== DETAIL ======
//        public ShipmentDetailDto GetDetail(int shipmentId, int driverId)
//        {
//            var s = _db.Shipments
//                .Include(x => x.Order.Customer)
//                .Include(x => x.FromWarehouse)
//                .Include(x => x.ToWarehouse)
//                .Include(x => x.RouteStops.Select(rs => rs.Warehouse))
//                .Include(x => x.Driver)
//                .Include(x => x.Vehicle)
//                .FirstOrDefault(x => x.Id == shipmentId);

//            if (s == null) throw new Exception("Shipment không tồn tại.");
//            if (s.DriverId != driverId) throw new Exception("Bạn không có quyền xem shipment này.");

//            var dto = new ShipmentDetailDto
//            {
//                Header = new ShipmentRunHeaderDto
//                {
//                    ShipmentId = s.Id,
//                    ShipmentNo = "SHP" + s.Id,
//                    OrderNo = s.Order?.OrderNo ?? ("ORD" + s.OrderId),
//                    CustomerName = s.Order?.Customer?.Name,
//                    Route = $"{s.FromWarehouse?.Name} → {s.ToWarehouse?.Name}",
//                    Status = s.Status.ToString(),
//                    CurrentStopSeq = s.CurrentStopSeq,
//                    StartedAt = s.StartedAt,
//                    DeliveredAt = s.DeliveredAt
//                },
//                DriverName = s.Driver?.FullName,
//                VehicleNo = s.Vehicle?.PlateNo,
//                Notes = s.Note,
//                Duration = (s.StartedAt.HasValue && s.DeliveredAt.HasValue) ? s.DeliveredAt.Value - s.StartedAt.Value : (TimeSpan?)null
//            };

//            dto.Stops = s.RouteStops
//                .OrderBy(r => r.Seq)
//                .Select(r => new RouteStopLiteDto
//                {
//                    RouteStopId = r.Id,
//                    Seq = r.Seq,
//                    StopName = r.StopName ?? r.Warehouse.Name,
//                    PlannedETA = r.PlannedETA,
//                    ArrivedAt = r.ArrivedAt,
//                    DepartedAt = r.DepartedAt,
//                    StopStatus = r.Status.ToString()
//                }).ToList();

//            return dto;
//        }

//        // ====== WORKFLOW ======
//        // 1) Driver nhận chuyến (Pending -> Assigned)
//        public void ReceiveShipment(int shipmentId, int driverId)
//        {
//            var s = _db.Shipments.Include(x => x.RouteStops).FirstOrDefault(x => x.Id == shipmentId);
//            if (s == null) throw new Exception("Shipment không tồn tại.");
//            if (s.DriverId != driverId) throw new Exception("Không phải shipment của bạn.");
//            if (s.Status != ShipmentStatus.Pending) throw new Exception("Chỉ nhận shipment trạng thái Pending.");

//            // Set current stop = stop đầu tiên theo Seq
//            var minSeq = s.RouteStops.Any() ? s.RouteStops.Min(x => x.Seq) : (int?)null;
//            if (!minSeq.HasValue) throw new Exception("Shipment chưa có RouteStops.");
//            s.CurrentStopSeq = minSeq.Value;

//            s.Status = ShipmentStatus.Assigned;
//            s.UpdatedAt = DateTime.Now;
//            s.StartedAt = s.StartedAt ?? DateTime.Now;

//            _db.SaveChanges();
//        }

//        // 2) Rời kho hiện tại (Assigned/AtWarehouse -> OnRoute)
//        public void DepartCurrentStop(int shipmentId, int driverId)
//        {
//            var s = _db.Shipments.Include(x => x.RouteStops).FirstOrDefault(x => x.Id == shipmentId);
//            if (s == null) throw new Exception("Shipment không tồn tại.");
//            if (s.DriverId != driverId) throw new Exception("Không phải shipment của bạn.");
//            if (!(s.Status == ShipmentStatus.Assigned || s.Status == ShipmentStatus.AtWarehouse))
//                throw new Exception("Chỉ có thể rời kho khi đã nhận chuyến hoặc đang ở kho.");

//            if (!s.CurrentStopSeq.HasValue) throw new Exception("Chưa xác định stop hiện tại.");
//            var cur = s.RouteStops.FirstOrDefault(x => x.Seq == s.CurrentStopSeq.Value);
//            if (cur == null) throw new Exception("Stop hiện tại không tồn tại.");

//            var minSeq = s.RouteStops.Min(x => x.Seq);
//            // Stop đầu tiên có thể Depart không cần Arrived
//            if (cur.Seq > minSeq && !cur.ArrivedAt.HasValue)
//                throw new Exception("Phải 'Đến kho' (Arrive) stop hiện tại trước khi 'Rời kho'.");

//            if (cur.DepartedAt.HasValue)
//                throw new Exception("Stop hiện tại đã 'Rời kho' trước đó.");

//            cur.Status = RouteStopStatus.Departed;
//            cur.DepartedAt = DateTime.Now;

//            s.Status = ShipmentStatus.OnRoute;
//            s.UpdatedAt = DateTime.Now;

//            _db.SaveChanges();
//        }

//        // 3) Đến kho kế tiếp (OnRoute -> AtWarehouse hoặc ArrivedDestination)
//        public void ArriveNextStop(int shipmentId, int driverId)
//        {
//            var s = _db.Shipments.Include(x => x.RouteStops).FirstOrDefault(x => x.Id == shipmentId);
//            if (s == null) throw new Exception("Shipment không tồn tại.");
//            if (s.DriverId != driverId) throw new Exception("Không phải shipment của bạn.");
//            if (s.Status != ShipmentStatus.OnRoute) throw new Exception("Chỉ có thể 'Đến kho' khi đang di chuyển (OnRoute).");

//            if (!s.CurrentStopSeq.HasValue) throw new Exception("Chưa xác định stop hiện tại.");

//            var nextSeq = s.CurrentStopSeq.Value + 1;
//            var next = s.RouteStops.FirstOrDefault(x => x.Seq == nextSeq);
//            if (next == null) throw new Exception("Không còn stop kế tiếp.");

//            if (next.ArrivedAt.HasValue)
//                throw new Exception("Stop kế tiếp đã được 'Đến' trước đó.");

//            next.Status = RouteStopStatus.Arrived;
//            next.ArrivedAt = DateTime.Now;

//            s.CurrentStopSeq = nextSeq;
//            s.UpdatedAt = DateTime.Now;

//            var maxSeq = s.RouteStops.Max(x => x.Seq);
//            s.Status = (nextSeq == maxSeq) ? ShipmentStatus.ArrivedDestination : ShipmentStatus.AtWarehouse;

//            _db.SaveChanges();
//        }

//        // 4) Hoàn thành đơn tại kho cuối
//        public void CompleteShipment(int shipmentId, int driverId)
//        {
//            var s = _db.Shipments.Include(x => x.RouteStops).FirstOrDefault(x => x.Id == shipmentId);
//            if (s == null) throw new Exception("Shipment không tồn tại.");
//            if (s.DriverId != driverId) throw new Exception("Không phải shipment của bạn.");
//            if (s.Status != ShipmentStatus.ArrivedDestination) throw new Exception("Chỉ hoàn thành khi đã đến kho cuối.");

//            s.Status = ShipmentStatus.Delivered;
//            s.DeliveredAt = DateTime.Now;
//            s.UpdatedAt = DateTime.Now;

//            var ord = _db.Orders.Find(s.OrderId);
//            if (ord != null) ord.Status = OrderStatus.Completed;

//            _db.SaveChanges();
//        }
//    }
//}


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
        private static readonly ShipmentStatus[] ActiveStatuses =
        {
            ShipmentStatus.Pending, ShipmentStatus.Assigned, ShipmentStatus.OnRoute,
            ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination
        };

        public List<ShipmentRowDto> GetAssignedAndRunning(int driverId)
        {
            var q = _db.Shipments
                // Bổ sung Includes để truy cập dữ liệu Order, Customer, và Warehouse
                .Include(s => s.Order.Customer)
                .Include(s => s.FromWarehouse)
                .Include(s => s.ToWarehouse)
                .Include(s => s.RouteStops) // Cần cho Stops.Count()
                .Where(s => s.DriverId == driverId && ActiveStatuses.Contains(s.Status))
                .OrderBy(s => s.Status).ThenByDescending(s => s.UpdatedAt)
                .Select(s => new ShipmentRowDto
                {
                    Id = s.Id,
                    ShipmentNo = "SHP" + s.Id,
                    OrderNo = s.Order.OrderNo ?? ("ORD" + s.OrderId),
                    Route = s.FromWarehouse.Name + " → " + s.ToWarehouse.Name,
                    Status = s.Status.ToString(),

                    // ÁNH XẠ ĐẦY ĐỦ CÁC THUỘC TÍNH
                    Stops = s.RouteStops.Count(), // Số chặng
                    StartedAt = s.StartedAt,
                    DeliveredAt = s.DeliveredAt,
                    UpdatedAt = s.UpdatedAt,

                    // THÔNG TIN BỔ SUNG
                    CustomerName = s.Order.Customer.Name,
                    OriginWarehouse = s.FromWarehouse.Name,
                    DestinationWarehouse = s.ToWarehouse.Name,
                    // Không tính Duration vì đang là LINQ to Entities (sẽ được tính trong GetAllMine)
                    Duration = (TimeSpan?)null
                });

            return q.ToList();
        }

        public List<ShipmentRowDto> GetAllMine(int driverId, DateTime? from = null, DateTime? to = null, ShipmentStatus? status = null)
        {
            // Bắt buộc phải có Include cho các thuộc tính liên quan
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
                // B1: LINQ to Entities (Project Anonymous Type)
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
                    // Tính DurationSeconds trên SQL
                    DurationSeconds = DbFunctions.DiffSeconds(s.StartedAt, s.DeliveredAt),
                    s.UpdatedAt,

                    // THÔNG TIN BỔ SUNG (LINQ to Entities)
                    CustomerName = s.Order.Customer.Name,
                    OriginWarehouse = s.FromWarehouse.Name,
                    DestinationWarehouse = s.ToWarehouse.Name
                })
                // B2: LINQ to Objects (Map về ShipmentRowDto)
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
                    // Chuyển DurationSeconds sang TimeSpan?
                    Duration = s.DurationSeconds.HasValue
                             ? (TimeSpan?)TimeSpan.FromSeconds(s.DurationSeconds.Value)
                             : null,
                    UpdatedAt = s.UpdatedAt,

                    // ÁNH XẠ CÁC FIELD BỔ SUNG
                    CustomerName = s.CustomerName,
                    OriginWarehouse = s.OriginWarehouse,
                    DestinationWarehouse = s.DestinationWarehouse
                })
                .ToList();

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
                VehicleNo = s.Vehicle?.PlateNo,
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
            if (s.Status != ShipmentStatus.Pending) throw new Exception("Chỉ nhận shipment trạng thái Pending.");

            // Set current stop = stop đầu tiên theo Seq
            var minSeq = s.RouteStops.Any() ? s.RouteStops.Min(x => x.Seq) : (int?)null;
            if (!minSeq.HasValue) throw new Exception("Shipment chưa có RouteStops.");
            s.CurrentStopSeq = minSeq.Value;

            s.Status = ShipmentStatus.Assigned;
            s.UpdatedAt = DateTime.Now;
            s.StartedAt = s.StartedAt ?? DateTime.Now;

            _db.SaveChanges();
        }

        // 2) Rời kho hiện tại (Assigned/AtWarehouse -> OnRoute)
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

            var minSeq = s.RouteStops.Min(x => x.Seq);
            // Stop đầu tiên có thể Depart không cần Arrived
            if (cur.Seq > minSeq && !cur.ArrivedAt.HasValue)
                throw new Exception("Phải 'Đến kho' (Arrive) stop hiện tại trước khi 'Rời kho'.");

            if (cur.DepartedAt.HasValue)
                throw new Exception("Stop hiện tại đã 'Rời kho' trước đó.");

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
            if (s.Status != ShipmentStatus.OnRoute) throw new Exception("Chỉ có thể 'Đến kho' khi đang di chuyển (OnRoute).");

            if (!s.CurrentStopSeq.HasValue) throw new Exception("Chưa xác định stop hiện tại.");

            var nextSeq = s.CurrentStopSeq.Value + 1;
            var next = s.RouteStops.FirstOrDefault(x => x.Seq == nextSeq);
            if (next == null) throw new Exception("Không còn stop kế tiếp.");

            if (next.ArrivedAt.HasValue)
                throw new Exception("Stop kế tiếp đã được 'Đến' trước đó.");

            next.Status = RouteStopStatus.Arrived;
            next.ArrivedAt = DateTime.Now;

            s.CurrentStopSeq = nextSeq;
            s.UpdatedAt = DateTime.Now;

            var maxSeq = s.RouteStops.Max(x => x.Seq);
            s.Status = (nextSeq == maxSeq) ? ShipmentStatus.ArrivedDestination : ShipmentStatus.AtWarehouse;

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

        public void SaveShipmentNote(int shipmentId, int driverId, string noteContent)
        {
            var s = _db.Shipments.Find(shipmentId);
            if (s == null) throw new Exception("Shipment không tồn tại.");
            if (s.DriverId != driverId) throw new Exception("Không phải shipment của bạn.");

            s.Note = noteContent; // Lưu nội dung ghi chú
            s.UpdatedAt = DateTime.Now;

            _db.SaveChanges();
        }
    }
}