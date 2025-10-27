//// LMS.BUS/Services/VehicleService_Admin.cs
//using LMS.DAL;
//using LMS.DAL.Models;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;

//namespace LMS.BUS.Services
//{
//    public class VehicleService_Admin
//    {
//        
//        public List<Vehicle> GetVehiclesForAdmin()
//        {
//            using (var db = new LogisticsDbContext())
//            {
//                return db.Vehicles
//                         .Include(v => v.Driver) // Tải kèm thông tin tài xế được gán
//                         .OrderBy(v => v.PlateNo)
//                         .ToList();
//            }
//        }

//        
//        public Vehicle GetVehicleForEdit(int vehicleId)
//        {
//            using (var db = new LogisticsDbContext())
//            {
//                // Include Driver ở đây để lấy thông tin DriverId nếu cần hiển thị/kiểm tra
//                var vehicle = db.Vehicles.Include(v => v.Driver).FirstOrDefault(v => v.Id == vehicleId);
//                if (vehicle == null)
//                {
//                    throw new Exception($"Không tìm thấy phương tiện với ID={vehicleId}.");
//                }
//                return vehicle;
//            }
//        }

//        /// <summary>
//        /// Tạo phương tiện mới.
//        /// </summary>
//        public void CreateVehicle(Vehicle newVehicle)
//        {
//            if (newVehicle == null) throw new ArgumentNullException(nameof(newVehicle));
//            if (string.IsNullOrWhiteSpace(newVehicle.PlateNo)) throw new ArgumentException("Biển số xe không được để trống.");
//            if (string.IsNullOrWhiteSpace(newVehicle.Type)) throw new ArgumentException("Loại xe không được để trống.");
//            newVehicle.PlateNo = newVehicle.PlateNo.Trim().ToUpper();

//            using (var db = new LogisticsDbContext())
//            {
//                if (db.Vehicles.Any(v => v.PlateNo == newVehicle.PlateNo))
//                {
//                    throw new InvalidOperationException($"Biển số xe '{newVehicle.PlateNo}' đã tồn tại.");
//                }

//                // *** SỬA: Không cần gán DriverId hay Driver ở đây nữa ***
//                // newVehicle.DriverId = null;
//                // newVehicle.Driver = null;

//                db.Vehicles.Add(newVehicle);
//                db.SaveChanges();
//            }
//        }

//        
//        public void UpdateVehicle(Vehicle updatedVehicle)
//        {
//            if (updatedVehicle == null) throw new ArgumentNullException(nameof(updatedVehicle));
//            if (string.IsNullOrWhiteSpace(updatedVehicle.PlateNo)) throw new ArgumentException("Biển số xe không được để trống.");
//            if (string.IsNullOrWhiteSpace(updatedVehicle.Type)) throw new ArgumentException("Loại xe không được để trống.");
//            updatedVehicle.PlateNo = updatedVehicle.PlateNo.Trim().ToUpper();

//            using (var db = new LogisticsDbContext())
//            {
//                var existing = db.Vehicles.Find(updatedVehicle.Id);
//                if (existing == null)
//                {
//                    throw new Exception($"Không tìm thấy phương tiện với ID={updatedVehicle.Id} để cập nhật.");
//                }

//                if (db.Vehicles.Any(v => v.PlateNo == updatedVehicle.PlateNo && v.Id != updatedVehicle.Id))
//                {
//                    throw new InvalidOperationException($"Biển số xe '{updatedVehicle.PlateNo}' đã được sử dụng bởi phương tiện khác.");
//                }

//                existing.PlateNo = updatedVehicle.PlateNo;
//                existing.Type = updatedVehicle.Type;
//                existing.CapacityKg = updatedVehicle.CapacityKg;
//                existing.Status = updatedVehicle.Status;
//                existing.LastMaintenanceDate = updatedVehicle.LastMaintenanceDate; // Giữ lại nếu model có
//                existing.Notes = updatedVehicle.Notes; // Giữ lại nếu model có

//                db.Entry(existing).State = EntityState.Modified;
//                db.SaveChanges();
//            }
//        }

//        /// <summary>
//        /// Xóa phương tiện.
//        /// Cần kiểm tra xem phương tiện có đang sử dụng không trước khi gọi hàm này.
//        /// </summary>
//        public void DeleteVehicle(int vehicleId)
//        {
//            using (var db = new LogisticsDbContext())
//            {
//                // Kiểm tra xem có Driver nào đang dùng không
//                if (IsVehicleInUseByDriver(vehicleId, db))
//                {
//                    throw new InvalidOperationException("Không thể xóa vì phương tiện đang được gán cho tài xế.");
//                }
//                // Kiểm tra xem có chuyến hàng nào đang dùng không
//                if (IsVehicleInActiveShipment(vehicleId, db))
//                {
//                    throw new InvalidOperationException("Không thể xóa vì phương tiện đang được sử dụng trong chuyến hàng đang hoạt động.");
//                }

//                var vehicle = db.Vehicles.Find(vehicleId);
//                if (vehicle != null)
//                {
//                    db.Vehicles.Remove(vehicle);
//                    db.SaveChanges();
//                }
//            }
//        }

//        /// <summary>
//        /// Tìm kiếm phương tiện theo tiêu chí.
//        /// </summary>
//        public List<Vehicle> SearchVehicles(string plateNoLike, string type, VehicleStatus? status)
//        {
//            using (var db = new LogisticsDbContext())
//            {
//                var query = db.Vehicles.Include(v => v.Driver).AsQueryable(); // Include Driver

//                if (!string.IsNullOrWhiteSpace(plateNoLike))
//                {
//                    string searchTerm = plateNoLike.Trim().ToUpper();
//                    query = query.Where(v => v.PlateNo.ToUpper().Contains(searchTerm));
//                }
//                if (!string.IsNullOrWhiteSpace(type))
//                {
//                    query = query.Where(v => v.Type == type);
//                }
//                if (status.HasValue)
//                {
//                    query = query.Where(v => v.Status == status.Value);
//                }

//                return query.OrderBy(v => v.PlateNo).ToList();
//            }
//        }

//        /// <summary>
//        /// Gán một tài xế (đang rảnh và chưa có xe) cho một phương tiện (chưa có tài xế).
//        /// </summary>
//        public void AssignDriverToVehicle(int vehicleId, int driverId)
//        {
//            using (var db = new LogisticsDbContext())
//            {
//                // Kiểm tra Vehicle (Load kèm Driver để biết đã có ai gán chưa)
//                var vehicle = db.Vehicles.Include(v => v.Driver).FirstOrDefault(v => v.Id == vehicleId);
//                if (vehicle == null) throw new Exception("Không tìm thấy phương tiện.");
//                // *** SỬA: Kiểm tra thông qua thuộc tính điều hướng Driver ***
//                if (vehicle.Driver != null) throw new InvalidOperationException($"Phương tiện '{vehicle.PlateNo}' đã được gán cho tài xế '{vehicle.Driver.FullName}'.");

//                // Kiểm tra Driver (Load kèm Vehicle để biết đã có xe chưa)
//                var driver = db.Drivers.Include(d => d.Vehicle).FirstOrDefault(d => d.Id == driverId);
//                if (driver == null) throw new Exception("Không tìm thấy tài xế.");
//                if (!driver.IsActive) throw new InvalidOperationException($"Tài xế '{driver.FullName}' đang không hoạt động.");
//                if (driver.VehicleId.HasValue) throw new InvalidOperationException($"Tài xế '{driver.FullName}' đã được gán cho phương tiện '{driver.Vehicle?.PlateNo}'.");

//                // Kiểm tra xem tài xế có đang chạy chuyến nào không
//                var driverService = new DriverService();
//                var availableDrivers = driverService.GetAvailableDriversForAdmin();
//                if (!availableDrivers.Any(d => d.Id == driverId))
//                {
//                    throw new InvalidOperationException($"Tài xế '{driver.FullName}' hiện đang bận hoặc không hoạt động.");
//                }

//                // Thực hiện gán (cập nhật khóa ngoại trong Driver)
//                driver.VehicleId = vehicleId;
//                // Không cần gán vehicle.DriverId nữa
//                db.SaveChanges();
//            }
//        }


//        /// <summary>
//        /// Gỡ tài xế khỏi phương tiện (Cập nhật Driver.VehicleId = null).
//        /// </summary>
//        public void UnassignDriverFromVehicle(int vehicleId)
//        {
//            using (var db = new LogisticsDbContext())
//            {
//                // Tìm tài xế đang được gán cho xe này
//                var driver = db.Drivers.FirstOrDefault(d => d.VehicleId == vehicleId);
//                if (driver == null) return; // Không có tài xế nào đang gán xe này

//                // Thực hiện gỡ bằng cách đặt VehicleId của Driver thành null
//                driver.VehicleId = null;
//                db.SaveChanges();
//            }
//        }

//        // --- Các hàm kiểm tra ràng buộc (Private Helpers) ---

//        /// <summary>
//        /// Kiểm tra xem phương tiện có đang được gán cho tài xế nào không.
//        /// </summary>
//        private bool IsVehicleInUseByDriver(int vehicleId, LogisticsDbContext db)
//        {
//            // Kiểm tra xem có Driver nào có VehicleId là vehicleId này không
//            return db.Drivers.Any(d => d.VehicleId == vehicleId);
//        }


//        /// <summary>
//        /// Kiểm tra xem phương tiện có đang được sử dụng trong chuyến hàng nào đang hoạt động không.
//        /// </summary>
//        private bool IsVehicleInActiveShipment(int vehicleId, LogisticsDbContext db)
//        {
//            // *** GIẢ SỬ BẠN ĐÃ THÊM CỘT VehicleId VÀO BẢNG Shipment ***
//            var activeStatuses = new[] {
//                 ShipmentStatus.Assigned, ShipmentStatus.OnRoute,
//                 ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination
//             };
//            // Nếu bảng Shipments chưa có cột VehicleId, bạn cần thêm cột này bằng migration trước
//            // return db.Shipments.Any(s => s.VehicleId == vehicleId && activeStatuses.Contains(s.Status));

//            // *** CÁCH KIỂM TRA GIÁN TIẾP NẾU Shipment CHƯA CÓ VehicleId ***
//            // Tìm DriverId đang gán cho Vehicle này (nếu có)
//            int? driverId = db.Vehicles
//                               .Where(v => v.Id == vehicleId)
//                               .Select(v => v.Driver.Id) // Lấy Id của Driver liên kết
//                               .FirstOrDefault(); // Lấy Id hoặc null nếu xe chưa có tài xế

//            if (!driverId.HasValue || driverId.Value == 0) return false; // Nếu xe không có tài xế thì không thể đang chạy chuyến

//            // Kiểm tra xem tài xế đó có đang chạy chuyến nào không
//            return db.Shipments.Any(s => s.DriverId == driverId.Value && activeStatuses.Contains(s.Status));
//        }

//    }
//}












//================








using LMS.BUS.Helpers;
using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS.BUS.Services
{
    public class VehicleService_Admin
    {
        public List<Vehicle> GetVehiclesForAdmin()
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Vehicles
                         .Include(v => v.Driver)
                         .OrderBy(v => v.PlateNo)
                         .ToList();
            }
        }

        public Vehicle GetVehicleForEdit(int vehicleId)
        {
            using (var db = new LogisticsDbContext())
            {
                var vehicle = db.Vehicles.Include(v => v.Driver)
                                         .FirstOrDefault(v => v.Id == vehicleId);
                if (vehicle == null)
                    throw new Exception($"Không tìm thấy phương tiện với ID={vehicleId}.");
                return vehicle;
            }
        }

        public void CreateVehicle(Vehicle newVehicle)
        {
            if (newVehicle == null) throw new ArgumentNullException(nameof(newVehicle));
            if (string.IsNullOrWhiteSpace(newVehicle.PlateNo)) throw new ArgumentException("Biển số xe không được để trống.");
            if (string.IsNullOrWhiteSpace(newVehicle.Type)) throw new ArgumentException("Loại xe không được để trống.");
            newVehicle.PlateNo = newVehicle.PlateNo.Trim().ToUpper();

            using (var db = new LogisticsDbContext())
            {
                if (db.Vehicles.Any(v => v.PlateNo == newVehicle.PlateNo))
                    throw new InvalidOperationException($"Biển số xe '{newVehicle.PlateNo}' đã tồn tại.");

                db.Vehicles.Add(newVehicle);
                db.SaveChanges();
            }
        }

        public void UpdateVehicle(Vehicle updatedVehicle)
        {
            if (updatedVehicle == null) throw new ArgumentNullException(nameof(updatedVehicle));
            if (string.IsNullOrWhiteSpace(updatedVehicle.PlateNo)) throw new ArgumentException("Biển số xe không được để trống.");
            if (string.IsNullOrWhiteSpace(updatedVehicle.Type)) throw new ArgumentException("Loại xe không được để trống.");
            updatedVehicle.PlateNo = updatedVehicle.PlateNo.Trim().ToUpper();

            using (var db = new LogisticsDbContext())
            {
                var existing = db.Vehicles.Find(updatedVehicle.Id)
                               ?? throw new Exception($"Không tìm thấy phương tiện với ID={updatedVehicle.Id} để cập nhật.");

                if (db.Vehicles.Any(v => v.PlateNo == updatedVehicle.PlateNo && v.Id != updatedVehicle.Id))
                    throw new InvalidOperationException($"Biển số xe '{updatedVehicle.PlateNo}' đã được sử dụng.");

                existing.PlateNo = updatedVehicle.PlateNo;
                existing.Type = updatedVehicle.Type;
                existing.CapacityKg = updatedVehicle.CapacityKg;
                existing.Status = updatedVehicle.Status;
                existing.LastMaintenanceDate = updatedVehicle.LastMaintenanceDate;
                existing.Notes = updatedVehicle.Notes;

                db.Entry(existing).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteVehicle(int vehicleId)
        {
            using (var db = new LogisticsDbContext())
            {
                if (db.Drivers.Any(d => d.VehicleId == vehicleId))
                    throw new InvalidOperationException("Không thể xóa vì phương tiện đang được gán cho tài xế.");

                var active = new[] { ShipmentStatus.Assigned, ShipmentStatus.OnRoute, ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination };
                var driverId = db.Drivers.Where(d => d.VehicleId == vehicleId).Select(d => (int?)d.Id).FirstOrDefault();
                if (driverId.HasValue && db.Shipments.Any(s => s.DriverId == driverId.Value && active.Contains(s.Status)))
                    throw new InvalidOperationException("Không thể xóa vì phương tiện đang được dùng trong chuyến hàng hoạt động.");

                var vehicle = db.Vehicles.Find(vehicleId);
                if (vehicle != null)
                {
                    db.Vehicles.Remove(vehicle);
                    db.SaveChanges();
                }
            }
        }

        // ============ RÀNG BUỘC 1 XE - 1 TÀI XẾ ============

        public void AssignDriverToVehicle(int vehicleId, int driverId)
        {
            using (var db = new LogisticsDbContext())
            using (var tx = db.Database.BeginTransaction())
            {
                var vehicle = db.Vehicles.Include(v => v.Driver).FirstOrDefault(v => v.Id == vehicleId)
                             ?? throw new Exception("Không tìm thấy phương tiện.");
                if (vehicle.Driver != null)
                    throw new InvalidOperationException($"Xe '{vehicle.PlateNo}' đã gán cho '{vehicle.Driver.FullName}'.");

                var driver = db.Drivers.Include(d => d.Vehicle).FirstOrDefault(d => d.Id == driverId)
                             ?? throw new Exception("Không tìm thấy tài xế.");
                if (!driver.IsActive)
                    throw new InvalidOperationException($"Tài xế '{driver.FullName}' đang không hoạt động.");
                if (driver.VehicleId.HasValue)
                    throw new InvalidOperationException($"Tài xế '{driver.FullName}' đã có xe '{driver.Vehicle?.PlateNo}'. Hãy gỡ trước.");

                var busy = new[] { ShipmentStatus.Assigned, ShipmentStatus.OnRoute, ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination };
                if (db.Shipments.Any(s => s.DriverId == driverId && busy.Contains(s.Status)))
                    throw new InvalidOperationException($"Tài xế '{driver.FullName}' hiện đang bận.");

                // Gán
                driver.VehicleId = vehicleId;
                db.SaveChanges();
                tx.Commit();

                // Thông báo toàn app
                AppSession.RaiseVehicleAssignmentChanged();
            }
        }

        public void UnassignDriverFromVehicle(int vehicleId)
        {
            using (var db = new LogisticsDbContext())
            using (var tx = db.Database.BeginTransaction())
            {
                var driver = db.Drivers.FirstOrDefault(d => d.VehicleId == vehicleId);
                if (driver == null) return;

                driver.VehicleId = null;
                db.SaveChanges();
                tx.Commit();

                AppSession.RaiseVehicleAssignmentChanged();
            }
        }

        // ============ TÌM KIẾM ============

        public List<Vehicle> SearchVehicles(string plateNoLike, string type, VehicleStatus? status)
        {
            using (var db = new LogisticsDbContext())
            {
                var q = db.Vehicles.Include(v => v.Driver).AsQueryable();

                if (!string.IsNullOrWhiteSpace(plateNoLike))
                {
                    var term = plateNoLike.Trim().ToUpper();
                    q = q.Where(v => v.PlateNo.ToUpper().Contains(term));
                }
                if (!string.IsNullOrWhiteSpace(type)) q = q.Where(v => v.Type == type);
                if (status.HasValue) q = q.Where(v => v.Status == status.Value);

                return q.OrderBy(v => v.PlateNo).ToList();
            }
        }
    }
}
