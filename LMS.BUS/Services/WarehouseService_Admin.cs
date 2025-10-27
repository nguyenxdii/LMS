using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity; // Cần cho Include và kiểm tra liên kết
using System.Linq;

namespace LMS.BUS.Services
{
    public class WarehouseService_Admin
    {

        public List<Warehouse> GetWarehouses()
        {
            using (var db = new LogisticsDbContext())
            {
                // Sắp xếp theo tên để hiển thị dễ nhìn hơn
                return db.Warehouses.OrderBy(w => w.Name).ToList();
            }
        }


        public Warehouse GetWarehouseForEdit(int warehouseId)
        {
            using (var db = new LogisticsDbContext())
            {
                var warehouse = db.Warehouses.Find(warehouseId);
                if (warehouse == null)
                {
                    throw new Exception($"Không tìm thấy kho với ID={warehouseId}.");
                }
                return warehouse;
            }
        }

        public void CreateWarehouse(Warehouse newWarehouse)
        {
            // Validation cơ bản (có thể thêm kiểm tra trùng tên nếu muốn)
            if (string.IsNullOrWhiteSpace(newWarehouse.Name)) throw new ArgumentException("Tên kho không được để trống.");
            if (string.IsNullOrWhiteSpace(newWarehouse.Address)) throw new ArgumentException("Địa chỉ không được để trống.");
            // ZoneId và Type là Enum nên thường không cần kiểm tra null trừ khi ComboBox cho phép không chọn

            using (var db = new LogisticsDbContext())
            {
                // Đặt trạng thái Active mặc định khi tạo mới
                newWarehouse.IsActive = true; //
                db.Warehouses.Add(newWarehouse);
                db.SaveChanges();
            }
        }

        public void UpdateWarehouse(Warehouse updatedWarehouse)
        {
            if (string.IsNullOrWhiteSpace(updatedWarehouse.Name)) throw new ArgumentException("Tên kho không được để trống.");
            if (string.IsNullOrWhiteSpace(updatedWarehouse.Address)) throw new ArgumentException("Địa chỉ không được để trống.");

            using (var db = new LogisticsDbContext())
            {
                var existing = db.Warehouses.Find(updatedWarehouse.Id);
                if (existing == null)
                {
                    throw new Exception($"Không tìm thấy kho với ID={updatedWarehouse.Id} để cập nhật.");
                }

                // Cập nhật các thuộc tính
                existing.Name = updatedWarehouse.Name;
                existing.Address = updatedWarehouse.Address;
                existing.ZoneId = updatedWarehouse.ZoneId;
                existing.Type = updatedWarehouse.Type;
                // Không cập nhật IsActive ở đây, dùng hàm ToggleActive riêng

                db.Entry(existing).State = EntityState.Modified;
                db.SaveChanges();
            }
        }


        public void ToggleActive(int warehouseId)
        {
            using (var db = new LogisticsDbContext())
            {
                var warehouse = db.Warehouses.Find(warehouseId);
                if (warehouse == null)
                {
                    throw new Exception($"Không tìm thấy kho với ID={warehouseId}.");
                }
                warehouse.IsActive = !warehouse.IsActive; // Đảo trạng thái
                db.SaveChanges();
            }
        }


        public bool CheckIfWarehouseCanBeDeactivated(int warehouseId)
        {
            using (var db = new LogisticsDbContext())
            {
                // 1. Kiểm tra RouteTemplate (cả kho đi, kho đến, và kho dừng) - Tạm thời bỏ qua vì model RouteTemplate chưa có trong project bạn gửi
                // bool usedInTemplate = db.RouteTemplates.Any(rt => rt.FromWarehouseId == warehouseId || rt.ToWarehouseId == warehouseId) ||
                //                       db.RouteTemplateStops.Any(rs => rs.WarehouseId == warehouseId);
                // if (usedInTemplate) return false; // Không khóa được nếu đang dùng trong template

                // 2. Kiểm tra Order chưa hoàn thành/hủy
                bool usedInActiveOrder = db.Orders.Any(o =>
                    (o.OriginWarehouseId == warehouseId || o.DestWarehouseId == warehouseId) &&
                    (o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled)
                );
                if (usedInActiveOrder) return false;

                // 3. Kiểm tra Shipment chưa hoàn thành/hủy
                bool usedInActiveShipment = db.Shipments.Any(s =>
                    (s.FromWarehouseId == warehouseId || s.ToWarehouseId == warehouseId) &&
                    (s.Status != ShipmentStatus.Delivered && s.Status != ShipmentStatus.Failed)
                );
                if (usedInActiveShipment) return false;

                // 4. Kiểm tra RouteStop của Shipment chưa hoàn thành/hủy
                bool usedInActiveRouteStop = db.RouteStops.Any(rs =>
                   rs.WarehouseId == warehouseId &&
                   rs.Shipment != null && // Đảm bảo có shipment liên kết
                   (rs.Shipment.Status != ShipmentStatus.Delivered && rs.Shipment.Status != ShipmentStatus.Failed)
                );
                if (usedInActiveRouteStop) return false;

                return true; // Có thể khóa
            }
        }

        public List<Warehouse> GetActiveWarehousesByZone(Zone zone) //
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Warehouses
                         .Where(w => w.IsActive && w.ZoneId == zone) // Lọc theo IsActive và ZoneId
                         .OrderBy(w => w.Name)
                         .ToList();
            }
        }

        public List<Warehouse> GetAllActiveWarehouses()
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Warehouses
                         .Where(w => w.IsActive) // Chỉ lấy kho đang hoạt động
                         .OrderBy(w => w.Name)
                         .ToList();
            }
        }

        public List<Warehouse> SearchWarehouses(string nameLike, Zone? zone, bool? isActive)
        {
            using (var db = new LogisticsDbContext())
            {
                var query = db.Warehouses.AsQueryable();

                if (!string.IsNullOrWhiteSpace(nameLike))
                {
                    query = query.Where(w => w.Name.Contains(nameLike));
                }
                if (zone.HasValue)
                {
                    query = query.Where(w => w.ZoneId == zone.Value);
                }
                if (isActive.HasValue)
                {
                    query = query.Where(w => w.IsActive == isActive.Value);
                }

                return query.OrderBy(w => w.Name).ToList();
            }
        }
    }
}