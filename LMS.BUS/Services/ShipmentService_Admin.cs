using LMS.BUS.Dtos;
using LMS.BUS.Helpers; // Cho OrderCode
using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity; // Cần cho DbFunctions
using System.Linq;

namespace LMS.BUS.Services
{
    public class ShipmentService_Admin
    {
        public List<ShipmentListItemAdminDto> GetAllShipmentsForAdmin()
        {
            using (var db = new LogisticsDbContext())
            {
                // Include các bảng liên quan để lấy tên
                var query = db.Shipments
                              .Include(s => s.Order) // Cần để lấy OrderNo và OrderId
                              .Include(s => s.Driver) // Cần để lấy DriverName
                              .Include(s => s.FromWarehouse) // Cần để lấy tên kho đi
                              .Include(s => s.ToWarehouse);  // Cần để lấy tên kho đến

                return ProjectToListDto(query);
            }
        }

        // === TÌM KIẾM ===
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
                              .AsQueryable(); // Bắt đầu IQueryable

                // Áp dụng các bộ lọc
                if (driverId.HasValue && driverId > 0)
                    query = query.Where(s => s.DriverId == driverId.Value);

                if (status.HasValue)
                    query = query.Where(s => s.Status == status.Value);

                if (originId.HasValue && originId > 0)
                    query = query.Where(s => s.FromWarehouseId == originId.Value);

                if (destId.HasValue && destId > 0)
                    query = query.Where(s => s.ToWarehouseId == destId.Value);

                // Lọc theo ngày cập nhật (UpdatedAt)
                if (from.HasValue)
                    query = query.Where(s => DbFunctions.TruncateTime(s.UpdatedAt) >= DbFunctions.TruncateTime(from.Value));
                if (to.HasValue)
                    query = query.Where(s => DbFunctions.TruncateTime(s.UpdatedAt) <= DbFunctions.TruncateTime(to.Value));

                // Lọc theo Mã Chuyến (SHP...) hoặc Mã Đơn (ORD...)
                if (!string.IsNullOrWhiteSpace(code))
                {
                    string upperCode = code.Trim().ToUpper();
                    int? orderIdParsed = null;
                    if (OrderCode.TryParseId(upperCode, out int oid))
                    {
                        orderIdParsed = oid;
                    }

                    // Điều kiện lọc OR: Mã chuyến HOẶC Mã đơn HOẶC ID đơn
                    query = query.Where(s =>
                        ("SHP" + s.Id).ToUpper() == upperCode ||
                        (s.Order != null && s.Order.OrderNo != null && s.Order.OrderNo.ToUpper() == upperCode) ||
                        (orderIdParsed.HasValue && s.OrderId == orderIdParsed.Value)
                    );
                }

                // Gọi hàm helper đã sửa để project và xử lý ToCode
                return ProjectToListDto(query);
            }
        }

        // === LẤY CHI TIẾT ===
        // Lưu ý: Tạm thời gọi lại DriverShipmentService.GetDetail
        // Cân nhắc tạo hàm GetDetail riêng cho Admin nếu cần logic khác hoặc GetDetail gốc kiểm tra driverId quá chặt
        //public ShipmentDetailDto GetShipmentDetailForAdmin(int shipmentId)
        //{
        //    var driverSvc = new DriverShipmentService();
        //    using (var db = new LogisticsDbContext())
        //    {
        //        var ship = db.Shipments.Include(s => s.Driver).FirstOrDefault(s => s.Id == shipmentId);
        //        if (ship == null) throw new Exception("Không tìm thấy chuyến hàng.");

        //        try
        //        {
        //            // Thử gọi GetDetail với driverId = 0 (hoặc ID của admin nếu có)
        //            // Hoặc thay bằng hàm GetDetailForAdmin riêng nếu bạn tạo nó
        //            return driverSvc.GetDetail(shipmentId, 0); // Giả sử GetDetail chấp nhận driverId = 0 cho Admin
        //        }
        //        // Bắt lỗi cụ thể nếu GetDetail gốc kiểm tra quyền Driver
        //        catch (Exception ex) when (ex.Message.Contains("Bạn không có quyền") || ex.Message.Contains("Không phải shipment của bạn"))
        //        {
        //            // Nếu lỗi quyền xảy ra, cần có logic GetDetail riêng cho Admin ở đây
        //            // Logic này sẽ tương tự GetDetail gốc nhưng không check DriverId
        //            // Ví dụ (cần hoàn thiện dựa trên GetDetail gốc):
        //            var s = db.Shipments
        //                      .Include(x => x.Order.Customer)
        //                      .Include(x => x.FromWarehouse)
        //                      .Include(x => x.ToWarehouse)
        //                      .Include(x => x.RouteStops.Select(rs => rs.Warehouse))
        //                      .Include(x => x.Driver)
        //                      .Include(x => x.Vehicle)
        //                      .FirstOrDefault(x => x.Id == shipmentId);

        //            if (s == null) throw new Exception("Không tìm thấy chuyến hàng (Admin)."); // Check lại

        //            var dto = new ShipmentDetailDto { /* ... Ánh xạ dữ liệu từ s sang dto ... */ };
        //            // ... (Code ánh xạ tương tự trong DriverShipmentService.GetDetail) ...
        //            dto.Header = new ShipmentRunHeaderDto
        //            {
        //                ShipmentId = s.Id,
        //                ShipmentNo = "SHP" + s.Id,
        //                OrderNo = s.Order?.OrderNo ?? ("ORD" + s.OrderId),
        //                CustomerName = s.Order?.Customer?.Name,
        //                Route = $"{s.FromWarehouse?.Name} → {s.ToWarehouse?.Name}",
        //                Status = s.Status.ToString(),
        //                CurrentStopSeq = s.CurrentStopSeq,
        //                StartedAt = s.StartedAt,
        //                DeliveredAt = s.DeliveredAt
        //            };
        //            dto.DriverName = s.Driver?.FullName; dto.VehicleNo = s.Vehicle?.PlateNo; dto.Notes = s.Note;
        //            dto.Duration = (s.StartedAt.HasValue && s.DeliveredAt.HasValue) ? s.DeliveredAt.Value - s.StartedAt.Value : (TimeSpan?)null;
        //            dto.Stops = s.RouteStops.OrderBy(r => r.Seq).Select(r => new RouteStopLiteDto
        //            {
        //                RouteStopId = r.Id,
        //                Seq = r.Seq,
        //                StopName = r.StopName ?? r.Warehouse.Name,
        //                PlannedETA = r.PlannedETA,
        //                ArrivedAt = r.ArrivedAt,
        //                DepartedAt = r.DepartedAt,
        //                StopStatus = r.Status.ToString()
        //            }).ToList();

        //            return dto;
        //            //throw new NotImplementedException("Cần triển khai GetShipmentDetail riêng cho Admin.", ex);
        //        }
        //    }
        //}
        //public ShipmentDetailDto GetShipmentDetailForAdmin(int shipmentId)
        //{
        //    var driverSvc = new DriverShipmentService(); // Hoặc logic lấy detail trực tiếp
        //    using (var db = new LogisticsDbContext())
        //    {
        //        var s = db.Shipments
        //                    .Include(x => x.Order.Customer)
        //                    .Include(x => x.FromWarehouse)
        //                    .Include(x => x.ToWarehouse)
        //                    .Include(x => x.RouteStops.Select(rs => rs.Warehouse))
        //                    .Include(x => x.Driver) // Lấy tài xế hiện tại
        //                    .Include(x => x.Vehicle)
        //                    .FirstOrDefault(x => x.Id == shipmentId);

        //        if (s == null) throw new Exception("Không tìm thấy chuyến hàng (Admin).");

        //        var dto = new ShipmentDetailDto { /* ... Ánh xạ dữ liệu Header, Stops, Misc như cũ ... */ };
        //        dto.Header = new ShipmentRunHeaderDto { /* ... */ };
        //        dto.DriverName = s.Driver?.FullName; dto.VehicleNo = s.Vehicle?.PlateNo; dto.Notes = s.Note;
        //        dto.Duration = (s.StartedAt.HasValue && s.DeliveredAt.HasValue) ? s.DeliveredAt.Value - s.StartedAt.Value : (TimeSpan?)null;
        //        dto.Stops = s.RouteStops.OrderBy(r => r.Seq).Select(r => new RouteStopLiteDto { /* ... */ }).ToList();


        //        dto.DriverHistory = db.ShipmentDriverLogs
        //                              .Where(log => log.ShipmentId == shipmentId)
        //                              .Include(log => log.OldDriver) // Lấy tên tài xế cũ
        //                              .Include(log => log.NewDriver) // Lấy tên tài xế mới
        //                              .OrderBy(log => log.Timestamp) // Sắp xếp theo thời gian
        //                              .Select(log => new ShipmentDriverLogDto // Tạo DTO mới cho lịch sử
        //                              {
        //                                  Timestamp = log.Timestamp,
        //                                  OldDriverName = log.OldDriver != null ? log.OldDriver.FullName : "(Bắt đầu)", // Hiển thị "(Bắt đầu)" nếu là lần gán đầu
        //                                  NewDriverName = log.NewDriver.FullName,
        //                                  StopSequenceNumber = log.StopSequenceNumber,
        //                                  Reason = log.Reason
        //                              })
        //                              .ToList();
        //        return dto;
        //    }
        //}
        public ShipmentDetailDto GetShipmentDetailForAdmin(int shipmentId)
        {
            using (var db = new LogisticsDbContext())
            {
                // Lấy dữ liệu Shipment cùng các bảng liên quan cần thiết
                var s = db.Shipments
                          .Include(x => x.Order.Customer)      // Lấy Order và Customer liên quan
                          .Include(x => x.FromWarehouse)       // Lấy kho đi
                          .Include(x => x.ToWarehouse)         // Lấy kho đến
                          .Include(x => x.RouteStops.Select(rs => rs.Warehouse)) // Lấy các chặng và kho của chặng
                          .Include(x => x.Driver)              // Lấy tài xế hiện tại
                          .Include(x => x.Vehicle)             // Lấy xe hiện tại
                          .FirstOrDefault(x => x.Id == shipmentId); // Tìm theo ID

                // Nếu không tìm thấy shipment, ném lỗi
                if (s == null) throw new Exception($"Không tìm thấy chuyến hàng ID={shipmentId} (Admin).");

                // Khởi tạo DTO chính để chứa kết quả
                var dto = new ShipmentDetailDto();

                // --- Ánh xạ dữ liệu Header ---
                dto.Header = new ShipmentRunHeaderDto
                {
                    ShipmentId = s.Id,
                    ShipmentNo = "SHP" + s.Id, // Tạo mã Shipment
                    OrderNo = s.Order?.OrderNo ?? OrderCode.ToCode(s.OrderId), // Lấy OrderNo hoặc tạo mã Order nếu null
                    CustomerName = s.Order?.Customer?.Name, // Lấy tên khách hàng (có thể null)
                    Route = $"{s.FromWarehouse?.Name} → {s.ToWarehouse?.Name}", // Tạo chuỗi tuyến đường
                    Status = s.Status.ToString(), // Chuyển Enum Status thành chuỗi
                    CurrentStopSeq = s.CurrentStopSeq, // Lấy chặng hiện tại
                    StartedAt = s.StartedAt, // Lấy thời gian bắt đầu
                    DeliveredAt = s.DeliveredAt // Lấy thời gian kết thúc
                };

                // --- Ánh xạ các thuộc tính Misc (thông tin thêm) ---
                dto.DriverName = s.Driver?.FullName; // Lấy tên tài xế (có thể null)
                dto.VehicleNo = s.Vehicle?.PlateNo; // Lấy biển số xe (có thể null)
                dto.Notes = s.Note; // Lấy ghi chú

                // Tính toán thời gian chạy (Duration) nếu có cả StartedAt và DeliveredAt
                dto.Duration = (s.StartedAt.HasValue && s.DeliveredAt.HasValue)
                             ? s.DeliveredAt.Value - s.StartedAt.Value // Tính TimeSpan
                             : (TimeSpan?)null; // Trả về null nếu chưa đủ thông tin

                // --- Ánh xạ danh sách các chặng dừng (Stops) ---
                dto.Stops = s.RouteStops
                             .OrderBy(r => r.Seq) // Sắp xếp các chặng theo thứ tự Seq
                             .Select(r => new RouteStopLiteDto // Tạo DTO cho mỗi chặng
                             {
                                 RouteStopId = r.Id, // ID của RouteStop
                                 Seq = r.Seq, // Thứ tự chặng
                                 StopName = r.Warehouse?.Name ?? r.StopName ?? $"#{r.WarehouseId}", // Ưu tiên tên Kho, fallback về StopName tự do hoặc ID
                                 PlannedETA = r.PlannedETA, // Thời gian dự kiến (nếu có)
                                 ArrivedAt = r.ArrivedAt, // Thời gian đến thực tế
                                 DepartedAt = r.DepartedAt, // Thời gian rời thực tế
                                 StopStatus = r.Status.ToString(), // Trạng thái chặng (Waiting, Arrived, Departed...)
                                 Note = r.Note,
                             })
                             .ToList(); // Chuyển kết quả thành List

                // --- Lấy lịch sử thay đổi tài xế (DriverHistory) ---
                dto.DriverHistory = db.ShipmentDriverLogs
                                      .Where(log => log.ShipmentId == shipmentId) // Lọc theo ShipmentId
                                      .Include(log => log.OldDriver) // Lấy thông tin tài xế cũ
                                      .Include(log => log.NewDriver) // Lấy thông tin tài xế mới
                                      .OrderBy(log => log.Timestamp) // Sắp xếp theo thời gian thay đổi
                                      .Select(log => new ShipmentDriverLogDto // Tạo DTO cho mỗi log
                                      {
                                          Timestamp = log.Timestamp, // Thời điểm thay đổi
                                          OldDriverName = log.OldDriver != null ? log.OldDriver.FullName : "(Bắt đầu)", // Tên tài xế cũ, "(Bắt đầu)" nếu là lần gán đầu
                                          NewDriverName = log.NewDriver.FullName, // Tên tài xế mới
                                          StopSequenceNumber = log.StopSequenceNumber, // Chặng dừng lúc đổi (nếu có)
                                          // StopName = ... (Nếu bạn cần lấy tên Kho tại chặng đó, cần join phức tạp hơn)
                                          Reason = log.Reason // Lý do thay đổi (nếu có)
                                      })
                                      .ToList(); // Chuyển kết quả thành List

                // Trả về DTO đã hoàn thiện
                return dto;
            }
        }

        private List<ShipmentListItemAdminDto> ProjectToListDto(IQueryable<Shipment> query)
        {
            var intermediateResult = query
                .OrderByDescending(s => s.UpdatedAt) // Sắp xếp trước khi lấy
                .Select(s => new
                {
                    s.Id,
                    s.OrderId,                   // Lấy OrderId
                    OrderNoFromDb = s.Order.OrderNo, // Lấy OrderNo gốc (có thể null)
                    DriverName = s.Driver.FullName,
                    s.DriverId,
                    FromWarehouseName = s.FromWarehouse.Name,
                    ToWarehouseName = s.ToWarehouse.Name,
                    s.Status,                    // Giữ Enum Status
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
                // Áp dụng ToCode an toàn ở đây
                OrderNo = s.OrderNoFromDb ?? OrderCode.ToCode(s.OrderId),
                DriverName = s.DriverName,
                DriverId = s.DriverId,
                Route = s.FromWarehouseName + " → " + s.ToWarehouseName,
                Status = s.Status, // Giữ Enum
                UpdatedAt = s.UpdatedAt,
                StartedAt = s.StartedAt,
                DeliveredAt = s.DeliveredAt,
                OriginWarehouseId = s.FromWarehouseId,
                DestWarehouseId = s.ToWarehouseId
            }).ToList(); // Trả về List<DTO>
        }
    }
}