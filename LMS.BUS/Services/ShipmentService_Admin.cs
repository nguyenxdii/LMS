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
    public class ShipmentService_Admin
    {
        // danh sách toàn bộ chuyến cho admin
        public List<ShipmentListItemAdminDto> GetAllShipmentsForAdmin()
        {
            using (var db = new LogisticsDbContext())
            {
                var query = db.Shipments
                              .Include(s => s.Order)
                              .Include(s => s.Driver)
                              .Include(s => s.FromWarehouse)
                              .Include(s => s.ToWarehouse);

                return ProjectToListDto(query);
            }
        }

        // tìm kiếm chuyến cho admin
        public List<ShipmentListItemAdminDto> SearchShipmentsForAdmin(
            int? driverId, ShipmentStatus? status, int? originId, int? destId,
            DateTime? from, DateTime? to, string code)
        {
            using (var db = new LogisticsDbContext())
            {
                var query = db.Shipments
                              .Include(s => s.Order)
                              .Include(s => s.Driver)
                              .Include(s => s.FromWarehouse)
                              .Include(s => s.ToWarehouse)
                              .AsQueryable();

                if (driverId.HasValue && driverId > 0)
                    query = query.Where(s => s.DriverId == driverId.Value);
                if (status.HasValue)
                    query = query.Where(s => s.Status == status.Value);
                if (originId.HasValue && originId > 0)
                    query = query.Where(s => s.FromWarehouseId == originId.Value);
                if (destId.HasValue && destId > 0)
                    query = query.Where(s => s.ToWarehouseId == destId.Value);
                if (from.HasValue)
                    query = query.Where(s => DbFunctions.TruncateTime(s.UpdatedAt) >= DbFunctions.TruncateTime(from.Value));
                if (to.HasValue)
                    query = query.Where(s => DbFunctions.TruncateTime(s.UpdatedAt) <= DbFunctions.TruncateTime(to.Value));
                if (!string.IsNullOrWhiteSpace(code))
                {
                    string upperCode = code.Trim().ToUpper();
                    int? orderIdParsed = null;
                    if (OrderCode.TryParseId(upperCode, out int oid))
                    {
                        orderIdParsed = oid;
                    }

                    query = query.Where(s =>
                        ("SHP" + s.Id).ToUpper() == upperCode ||
                        (s.Order != null && s.Order.OrderNo != null && s.Order.OrderNo.ToUpper() == upperCode) ||
                        (orderIdParsed.HasValue && s.OrderId == orderIdParsed.Value)
                    );
                }

                return ProjectToListDto(query);
            }
        }

        // chi tiết chuyến cho admin (kèm stops + lịch sử đổi tài xế)
        public ShipmentDetailDto GetShipmentDetailForAdmin(int shipmentId)
        {
            using (var db = new LogisticsDbContext())
            {
                var s = db.Shipments
                          .Include(x => x.Order.Customer)
                          .Include(x => x.FromWarehouse)
                          .Include(x => x.ToWarehouse)
                          .Include(x => x.RouteStops.Select(rs => rs.Warehouse))
                          .Include(x => x.Driver)
                          .Include(x => x.Vehicle)
                          .Include(x => x.Driver.Vehicle)
                          .FirstOrDefault(x => x.Id == shipmentId);

                if (s == null) throw new Exception($"Không tìm thấy chuyến hàng id={shipmentId} (Admin).");

                var dto = new ShipmentDetailDto();

                dto.Header = new ShipmentRunHeaderDto
                {
                    ShipmentId = s.Id,
                    ShipmentNo = "SHP" + s.Id,
                    OrderNo = s.Order?.OrderNo ?? OrderCode.ToCode(s.OrderId),
                    CustomerName = s.Order?.Customer?.Name,
                    Route = $"{s.FromWarehouse?.Name} → {s.ToWarehouse?.Name}",
                    Status = s.Status.ToString(),
                    CurrentStopSeq = s.CurrentStopSeq,
                    StartedAt = s.StartedAt,
                    DeliveredAt = s.DeliveredAt
                };

                dto.DriverName = s.Driver?.FullName;
                dto.VehicleNo = s.Vehicle?.PlateNo ?? s.Driver?.Vehicle?.PlateNo;
                dto.Notes = s.Note;
                dto.Duration = (s.StartedAt.HasValue && s.DeliveredAt.HasValue)
                             ? s.DeliveredAt.Value - s.StartedAt.Value
                             : (TimeSpan?)null;

                dto.Stops = s.RouteStops
                             .OrderBy(r => r.Seq)
                             .Select(r => new RouteStopLiteDto
                             {
                                 RouteStopId = r.Id,
                                 Seq = r.Seq,
                                 StopName = r.Warehouse?.Name ?? r.StopName ?? $"#{r.WarehouseId}",
                                 PlannedETA = r.PlannedETA,
                                 ArrivedAt = r.ArrivedAt,
                                 DepartedAt = r.DepartedAt,
                                 StopStatus = r.Status.ToString(),
                                 Note = r.Note,
                             })
                             .ToList();

                dto.DriverHistory = db.ShipmentDriverLogs
                                      .Where(log => log.ShipmentId == shipmentId)
                                      .Include(log => log.OldDriver)
                                      .Include(log => log.NewDriver)
                                      .OrderBy(log => log.Timestamp)
                                      .Select(log => new ShipmentDriverLogDto
                                      {
                                          Timestamp = log.Timestamp,
                                          OldDriverName = log.OldDriver != null ? log.OldDriver.FullName : "(Bắt đầu)",
                                          NewDriverName = log.NewDriver.FullName,
                                          StopSequenceNumber = log.StopSequenceNumber,
                                          Reason = log.Reason
                                      })
                                      .ToList();

                return dto;
            }
        }

        // project query sang list dto (xử lý orderno an toàn)
        private List<ShipmentListItemAdminDto> ProjectToListDto(IQueryable<Shipment> query)
        {
            var intermediateResult = query
                .OrderByDescending(s => s.UpdatedAt)
                .Select(s => new
                {
                    s.Id,
                    s.OrderId,
                    OrderNoFromDb = s.Order.OrderNo,
                    DriverName = s.Driver.FullName,
                    s.DriverId,
                    FromWarehouseName = s.FromWarehouse.Name,
                    ToWarehouseName = s.ToWarehouse.Name,
                    s.Status,
                    s.UpdatedAt,
                    s.StartedAt,
                    s.DeliveredAt,
                    s.FromWarehouseId,
                    s.ToWarehouseId
                })
                .ToList();

            return intermediateResult.Select(s => new ShipmentListItemAdminDto
            {
                Id = s.Id,
                ShipmentNo = "SHP" + s.Id,
                OrderNo = s.OrderNoFromDb ?? OrderCode.ToCode(s.OrderId),
                DriverName = s.DriverName,
                DriverId = s.DriverId,
                Route = s.FromWarehouseName + " → " + s.ToWarehouseName,
                Status = s.Status,
                UpdatedAt = s.UpdatedAt,
                StartedAt = s.StartedAt,
                DeliveredAt = s.DeliveredAt,
                OriginWarehouseId = s.FromWarehouseId,
                DestWarehouseId = s.ToWarehouseId
            }).ToList();
        }
    }
}