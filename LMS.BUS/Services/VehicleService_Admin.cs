using LMS.BUS.Helpers;
using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace LMS.BUS.Services
{
    public class VehicleService_Admin
    {
        public List<Vehicle> GetVehiclesForAdmin()
        {
            //using (var db = new LogisticsDbContext())
            //{
            //    return db.Vehicles
            //             .Include(v => v.Driver)
            //             .AsNoTracking()
            //             .OrderBy(v => v.PlateNo)
            //             .ToList();
            //}
            try // <-- Thêm try
            {
                using (var db = new LogisticsDbContext())
                {
                    // Đặt breakpoint ở dòng return này
                    var result = db.Vehicles
                                   .Include(v => v.Driver)
                                   .AsNoTracking()
                                   .OrderBy(v => v.PlateNo)
                                   .ToList();
                    return result; // <-- Breakpoint ở đây
                }
            }
            catch (Exception ex) // <-- Thêm catch
            {
                // Đặt breakpoint ở dòng MessageBox này
                MessageBox.Show($"Lỗi bên trong GetVehiclesForAdmin: {ex.ToString()}"); // <-- Breakpoint ở đây
                return null; // Hoặc trả về new List<Vehicle>() để tránh null
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

        //public void DeleteVehicle(int vehicleId)
        //{
        //    using (var db = new LogisticsDbContext())
        //    {
        //        if (db.Drivers.Any(d => d.VehicleId == vehicleId))
        //            throw new InvalidOperationException("Không thể xóa vì phương tiện đang được gán cho tài xế.");

        //        var active = new[] { ShipmentStatus.Assigned, ShipmentStatus.OnRoute, ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination };
        //        var driverId = db.Drivers.Where(d => d.VehicleId == vehicleId).Select(d => (int?)d.Id).FirstOrDefault();
        //        if (driverId.HasValue && db.Shipments.Any(s => s.DriverId == driverId.Value && active.Contains(s.Status)))
        //            throw new InvalidOperationException("Không thể xóa vì phương tiện đang được dùng trong chuyến hàng hoạt động.");

        //        var vehicle = db.Vehicles.Find(vehicleId);
        //        if (vehicle != null)
        //        {
        //            db.Vehicles.Remove(vehicle);
        //            db.SaveChanges();
        //        }
        //    }
        //}
        public void DeleteVehicle(int vehicleId)
        {
            using (var db = new LogisticsDbContext())
            {
                var vehicle = db.Vehicles
                                .Include(v => v.Driver)
                                .FirstOrDefault(v => v.Id == vehicleId);

                if (vehicle == null) return;

                // nếu xe đang có tài xế -> không cho xóa
                if (vehicle.Driver != null)
                    throw new InvalidOperationException("Không thể xóa vì xe đang được gán cho tài xế.");

                // nếu có chuyến hàng hoạt động sử dụng tài xế của xe này -> cũng chặn
                var active = new[] { ShipmentStatus.Assigned, ShipmentStatus.OnRoute, ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination };
                if (vehicle.Driver != null &&
                    db.Shipments.Any(s => s.DriverId == vehicle.Driver.Id && active.Contains(s.Status)))
                    throw new InvalidOperationException("Không thể xóa vì xe đang liên quan đến chuyến hàng hoạt động.");

                db.Vehicles.Remove(vehicle);
                db.SaveChanges();
            }
        }


        // ============ RÀNG BUỘC 1 XE - 1 TÀI XẾ ============

        //public void AssignDriverToVehicle(int vehicleId, int driverId)
        //{
        //    using (var db = new LogisticsDbContext())
        //    using (var tx = db.Database.BeginTransaction())
        //    {
        //        var vehicle = db.Vehicles.Include(v => v.Driver).FirstOrDefault(v => v.Id == vehicleId)
        //                     ?? throw new Exception("Không tìm thấy phương tiện.");
        //        if (vehicle.Driver != null)
        //            throw new InvalidOperationException($"Xe '{vehicle.PlateNo}' đã gán cho '{vehicle.Driver.FullName}'.");

        //        var driver = db.Drivers.Include(d => d.Vehicle).FirstOrDefault(d => d.Id == driverId)
        //                     ?? throw new Exception("Không tìm thấy tài xế.");
        //        if (!driver.IsActive)
        //            throw new InvalidOperationException($"Tài xế '{driver.FullName}' đang không hoạt động.");
        //        if (driver.VehicleId.HasValue)
        //            throw new InvalidOperationException($"Tài xế '{driver.FullName}' đã có xe '{driver.Vehicle?.PlateNo}'. Hãy gỡ trước.");

        //        var busy = new[] { ShipmentStatus.Assigned, ShipmentStatus.OnRoute, ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination };
        //        if (db.Shipments.Any(s => s.DriverId == driverId && busy.Contains(s.Status)))
        //            throw new InvalidOperationException($"Tài xế '{driver.FullName}' hiện đang bận.");

        //        // Gán
        //        driver.VehicleId = vehicleId;
        //        db.SaveChanges();
        //        tx.Commit();

        //        // Thông báo toàn app
        //        AppSession.RaiseVehicleAssignmentChanged();
        //    }
        //}
        //public void AssignDriverToVehicle(int vehicleId, int driverId)
        //{
        //    using (var db = new LogisticsDbContext())
        //    using (var tx = db.Database.BeginTransaction())
        //    {
        //        var vehicle = db.Vehicles
        //                        .Include(v => v.Driver)
        //                        .FirstOrDefault(v => v.Id == vehicleId)
        //                        ?? throw new Exception("Không tìm thấy phương tiện.");
        //        if (vehicle.Driver != null)
        //            throw new InvalidOperationException($"Xe '{vehicle.PlateNo}' đã gán cho '{vehicle.Driver.FullName}'.");

        //        var driver = db.Drivers
        //                       .Include(d => d.Vehicle)
        //                       .FirstOrDefault(d => d.Id == driverId)
        //                       ?? throw new Exception("Không tìm thấy tài xế.");
        //        if (!driver.IsActive)
        //            throw new InvalidOperationException($"Tài xế '{driver.FullName}' đang không hoạt động.");
        //        if (driver.Vehicle != null) // KHÔNG dựa vào driver.VehicleId nữa
        //            throw new InvalidOperationException($"Tài xế '{driver.FullName}' đã có xe '{driver.Vehicle.PlateNo}'. Hãy gỡ trước.");

        //        var busy = new[] { ShipmentStatus.Assigned, ShipmentStatus.OnRoute, ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination };
        //        if (db.Shipments.Any(s => s.DriverId == driverId && busy.Contains(s.Status)))
        //            throw new InvalidOperationException($"Tài xế '{driver.FullName}' hiện đang bận.");

        //        // --- GÁN ĐÚNG HƯỚNG (phía Vehicle) ---
        //        vehicle.Driver = driver;          // <- điểm mấu chốt
        //        db.SaveChanges();
        //        tx.Commit();

        //        AppSession.RaiseVehicleAssignmentChanged();
        //    }
        //}

        // [Dán vào file VehicleService_Admin.cs]

        public void AssignDriverToVehicle(int vehicleId, int driverId)
        {
            using (var db = new LogisticsDbContext())
            using (var tx = db.Database.BeginTransaction())
            {
                // 1. Kiểm tra xe (giữ nguyên)
                var vehicle = db.Vehicles
                                .Include(v => v.Driver)
                                .FirstOrDefault(v => v.Id == vehicleId)
                                ?? throw new Exception("Không tìm thấy phương tiện.");
                if (vehicle.Driver != null)
                    throw new InvalidOperationException($"Xe '{vehicle.PlateNo}' đã gán cho '{vehicle.Driver.FullName}'.");

                // 2. Kiểm tra tài xế (logic mới)
                var driver = db.Drivers
                               // .Include(d => d.Vehicle) // <-- BỎ DÒNG NÀY
                               .FirstOrDefault(d => d.Id == driverId)
                               ?? throw new Exception("Không tìm thấy tài xế.");
                if (!driver.IsActive)
                    throw new InvalidOperationException($"Tài xế '{driver.FullName}' đang không hoạt động.");

                // 3. THAY ĐỔI LOGIC KIỂM TRA TÀI XẾ RẢNH
                // Logic cũ (lỗi): if (driver.Vehicle != null)
                // Logic mới: Kiểm tra xem có xe NÀO KHÁC đang giữ tài xế này không
                var otherVehicle = db.Vehicles.FirstOrDefault(v => v.Driver.Id == driverId);
                if (otherVehicle != null)
                    throw new InvalidOperationException($"Tài xế '{driver.FullName}' đã có xe '{otherVehicle.PlateNo}'. Hãy gỡ trước.");

                // 4. Kiểm tra tài xế có bận (giữ nguyên)
                var busy = new[] { ShipmentStatus.Assigned, ShipmentStatus.OnRoute, ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination };
                if (db.Shipments.Any(s => s.DriverId == driverId && busy.Contains(s.Status)))
                    throw new InvalidOperationException($"Tài xế '{driver.FullName}' hiện đang bận.");

                // --- GÁN ĐÚNG HƯỚNG (phía Vehicle) ---
                vehicle.Driver = driver; // Gán tài xế vào xe
                db.SaveChanges();
                tx.Commit();

                AppSession.RaiseVehicleAssignmentChanged();
            }
        }


        //public void UnassignDriverFromVehicle(int vehicleId)
        //{
        //    using (var db = new LogisticsDbContext())
        //    using (var tx = db.Database.BeginTransaction())
        //    {
        //        var driver = db.Drivers.FirstOrDefault(d => d.VehicleId == vehicleId);
        //        if (driver == null) return;

        //        driver.VehicleId = null;
        //        db.SaveChanges();
        //        tx.Commit();

        //        AppSession.RaiseVehicleAssignmentChanged();
        //    }
        //}
        public void UnassignDriverFromVehicle(int vehicleId)
        {
            using (var db = new LogisticsDbContext())
            using (var tx = db.Database.BeginTransaction())
            {
                var vehicle = db.Vehicles
                                .Include(v => v.Driver)
                                .FirstOrDefault(v => v.Id == vehicleId);
                if (vehicle == null) return;

                vehicle.Driver = null;            // <- bỏ liên kết tại phía Vehicle
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
