using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS.BUS.Services
{
    public class WarehouseService_Admin
    {
        // danh sách kho
        public List<Warehouse> GetWarehouses()
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Warehouses.OrderBy(w => w.Name).ToList();
            }
        }

        // tìm kiếm kho
        public List<Warehouse> SearchWarehouses(string nameLike, Zone? zone, bool? isActive)
        {
            using (var db = new LogisticsDbContext())
            {
                var query = db.Warehouses.AsQueryable();

                if (!string.IsNullOrWhiteSpace(nameLike))
                    query = query.Where(w => w.Name.Contains(nameLike));
                if (zone.HasValue)
                    query = query.Where(w => w.ZoneId == zone.Value);
                if (isActive.HasValue)
                    query = query.Where(w => w.IsActive == isActive.Value);

                return query.OrderBy(w => w.Name).ToList();
            }
        }

        // kho active toàn bộ
        public List<Warehouse> GetAllActiveWarehouses()
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Warehouses
                         .Where(w => w.IsActive)
                         .OrderBy(w => w.Name)
                         .ToList();
            }
        }

        // kho active theo zone
        public List<Warehouse> GetActiveWarehousesByZone(Zone zone)
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Warehouses
                         .Where(w => w.IsActive && w.ZoneId == zone)
                         .OrderBy(w => w.Name)
                         .ToList();
            }
        }

        // lấy kho để sửa
        public Warehouse GetWarehouseForEdit(int warehouseId)
        {
            using (var db = new LogisticsDbContext())
            {
                var warehouse = db.Warehouses.Find(warehouseId);
                if (warehouse == null)
                    throw new Exception($"Không tìm thấy kho với ID={warehouseId}.");

                return warehouse;
            }
        }

        // tạo kho mới
        public void CreateWarehouse(Warehouse newWarehouse)
        {
            if (string.IsNullOrWhiteSpace(newWarehouse.Name)) throw new ArgumentException("Tên kho không được để trống.");
            if (string.IsNullOrWhiteSpace(newWarehouse.Address)) throw new ArgumentException("Địa chỉ không được để trống.");

            using (var db = new LogisticsDbContext())
            {
                newWarehouse.IsActive = true;
                db.Warehouses.Add(newWarehouse);
                db.SaveChanges();
            }
        }

        // cập nhật kho
        public void UpdateWarehouse(Warehouse updatedWarehouse)
        {
            if (string.IsNullOrWhiteSpace(updatedWarehouse.Name)) throw new ArgumentException("Tên kho không được để trống.");
            if (string.IsNullOrWhiteSpace(updatedWarehouse.Address)) throw new ArgumentException("Địa chỉ không được để trống.");

            using (var db = new LogisticsDbContext())
            {
                var existing = db.Warehouses.Find(updatedWarehouse.Id);
                if (existing == null)
                    throw new Exception($"Không tìm thấy kho với ID={updatedWarehouse.Id} để cập nhật.");

                existing.Name = updatedWarehouse.Name;
                existing.Address = updatedWarehouse.Address;
                existing.ZoneId = updatedWarehouse.ZoneId;
                existing.Type = updatedWarehouse.Type;

                db.Entry(existing).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        // đảo trạng thái active
        public void ToggleActive(int warehouseId)
        {
            using (var db = new LogisticsDbContext())
            {
                var warehouse = db.Warehouses.Find(warehouseId);
                if (warehouse == null)
                    throw new Exception($"Không tìm thấy kho với ID={warehouseId}.");

                warehouse.IsActive = !warehouse.IsActive;
                db.SaveChanges();
            }
        }

        // kiểm tra có thể deactivate kho không
        public bool CheckIfWarehouseCanBeDeactivated(int warehouseId)
        {
            using (var db = new LogisticsDbContext())
            {
                // kiểm tra order active
                bool usedInActiveOrder = db.Orders.Any(o =>
                    (o.OriginWarehouseId == warehouseId || o.DestWarehouseId == warehouseId) &&
                    (o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled)
                );
                if (usedInActiveOrder) return false;

                // kiểm tra shipment active
                bool usedInActiveShipment = db.Shipments.Any(s =>
                    (s.FromWarehouseId == warehouseId || s.ToWarehouseId == warehouseId) &&
                    (s.Status != ShipmentStatus.Delivered && s.Status != ShipmentStatus.Failed)
                );
                if (usedInActiveShipment) return false;

                // kiểm tra routestop active
                bool usedInActiveRouteStop = db.RouteStops.Any(rs =>
                   rs.WarehouseId == warehouseId &&
                   rs.Shipment != null &&
                   (rs.Shipment.Status != ShipmentStatus.Delivered && rs.Shipment.Status != ShipmentStatus.Failed)
                );
                if (usedInActiveRouteStop) return false;

                return true;
            }
        }
    }
}