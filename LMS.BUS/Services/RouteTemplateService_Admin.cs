using LMS.BUS.Dtos;
using LMS.DAL;
using LMS.DAL.Models;
using LogisticsApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS.BUS.Services
{
    public class RouteTemplateService_Admin
    {
        // danh sách tuyến cho grid
        public List<RouteTemplateListItemDto> GetListForGrid()
        {
            using (var db = new LogisticsDbContext())
            {
                var data = db.RouteTemplates
                    .Include(t => t.FromWarehouse)
                    .Include(t => t.ToWarehouse)
                    .Include(t => t.Stops.Select(s => s.Warehouse))
                    .ToList()
                    .Select(t => new RouteTemplateListItemDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        FromWarehouseName = t.FromWarehouse?.Name,
                        ToWarehouseName = t.ToWarehouse?.Name,
                        StopsCount = t.Stops.Count,
                        RoutePreview = BuildRoutePreview(t)
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

                return data;
            }
        }

        // danh sách tuyến ngắn gọn (dùng cho picker hoặc list đơn giản)
        public List<RouteTemplateListItemDto> GetRouteTemplateListItems()
        {
            using (var db = new LogisticsDbContext())
            {
                var query = db.RouteTemplates
                             .AsNoTracking()
                             .Include(rt => rt.FromWarehouse)
                             .Include(rt => rt.ToWarehouse)
                             .Include(rt => rt.Stops);

                var resultBeforeSelect = query.OrderBy(rt => rt.Name).ToList();

                return resultBeforeSelect.Select(rt => new RouteTemplateListItemDto
                {
                    Id = rt.Id,
                    Name = rt.Name,
                    FromWarehouseName = rt.FromWarehouse.Name,
                    ToWarehouseName = rt.ToWarehouse.Name,
                    StopsCount = rt.Stops.Count()
                }).ToList();
            }
        }

        // tìm kiếm tuyến
        public List<RouteTemplateListItemDto> SearchRouteTemplates(string nameLike, int? fromWarehouseId, int? toWarehouseId)
        {
            using (var db = new LogisticsDbContext())
            {
                var query = db.RouteTemplates
                              .Include(rt => rt.FromWarehouse)
                              .Include(rt => rt.ToWarehouse)
                              .Include(rt => rt.Stops)
                              .AsQueryable();

                if (!string.IsNullOrWhiteSpace(nameLike))
                {
                    query = query.Where(rt => rt.Name.Contains(nameLike));
                }
                if (fromWarehouseId.HasValue && fromWarehouseId > 0)
                {
                    query = query.Where(rt => rt.FromWarehouseId == fromWarehouseId.Value);
                }
                if (toWarehouseId.HasValue && toWarehouseId > 0)
                {
                    query = query.Where(rt => rt.ToWarehouseId == toWarehouseId.Value);
                }

                return query.OrderBy(rt => rt.Name)
                            .Select(rt => new RouteTemplateListItemDto
                            {
                                Id = rt.Id,
                                Name = rt.Name,
                                FromWarehouseName = rt.FromWarehouse.Name,
                                ToWarehouseName = rt.ToWarehouse.Name,
                                StopsCount = rt.Stops.Count()
                            })
                            .ToList();
            }
        }

        // lấy chi tiết tuyến kèm stops
        public LogisticsApp.DAL.Models.RouteTemplate GetByIdWithStops(int id)
        {
            using (var db = new LogisticsDbContext())
            {
                return db.RouteTemplates
                    .Include(t => t.FromWarehouse)
                    .Include(t => t.ToWarehouse)
                    .Include(t => t.Stops.Select(s => s.Warehouse))
                    .FirstOrDefault(t => t.Id == id);
            }
        }

        // lấy template kèm stops (dùng cho editor)
        public RouteTemplate GetTemplateWithStops(int templateId)
        {
            using (var db = new LogisticsDbContext())
            {
                return db.RouteTemplates
                         .Include(rt => rt.Stops.Select(s => s.Warehouse))
                         .FirstOrDefault(rt => rt.Id == templateId);
            }
        }

        // kiểm tra tuyến đang được dùng
        public bool IsRouteTemplateInUse(int templateId)
        {
            using (var db = new LogisticsDbContext())
            {
                var template = db.RouteTemplates.Find(templateId);
                if (template == null) return false;

                return db.Shipments.Any(s =>
                   s.FromWarehouseId == template.FromWarehouseId &&
                   s.ToWarehouseId == template.ToWarehouseId &&
                   s.Status != ShipmentStatus.Delivered &&
                   s.Status != ShipmentStatus.Failed);
            }
        }

        // xóa tuyến
        public void DeleteRouteTemplate(int templateId)
        {
            using (var db = new LogisticsDbContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var stops = db.RouteTemplateStops.Where(rs => rs.TemplateId == templateId).ToList();
                        if (stops.Any())
                        {
                            db.RouteTemplateStops.RemoveRange(stops);
                        }

                        var template = db.RouteTemplates.Find(templateId);
                        if (template != null)
                        {
                            db.RouteTemplates.Remove(template);
                        }
                        else
                        {
                            transaction.Rollback();
                            return;
                        }

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("lỗi khi xóa tuyến đường. vui lòng thử lại.", ex);
                    }
                }
            }
        }

        // tạo tuyến mới kèm stops
        public void CreateTemplateWithStops(RouteTemplate template, List<int> stopWarehouseIds)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (string.IsNullOrWhiteSpace(template.Name)) throw new ArgumentException("tên tuyến đường không được trống.");
            if (template.FromWarehouseId == 0) throw new ArgumentException("chưa chọn kho đi.");
            if (template.ToWarehouseId == 0) throw new ArgumentException("chưa chọn kho đến.");
            if (template.FromWarehouseId == template.ToWarehouseId) throw new ArgumentException("kho đi và kho đến không được trùng nhau.");

            using (var db = new LogisticsDbContext())
            {
                if (db.RouteTemplates.Any(rt => rt.Name.Equals(template.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"tên tuyến đường '{template.Name}' đã tồn tại.");
                }

                var fromWh = db.Warehouses.Find(template.FromWarehouseId);
                var toWh = db.Warehouses.Find(template.ToWarehouseId);
                if (fromWh == null || toWh == null) throw new InvalidOperationException("kho đi hoặc kho đến không hợp lệ.");

                if (fromWh.ZoneId != toWh.ZoneId && (stopWarehouseIds == null || !stopWarehouseIds.Any()))
                {
                    throw new InvalidOperationException("tuyến đường liên vùng/xa phải có ít nhất 1 kho trung chuyển.");
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.RouteTemplates.Add(template);
                        db.SaveChanges();

                        if (stopWarehouseIds != null && stopWarehouseIds.Any())
                        {
                            int seq = 1;
                            foreach (int whId in stopWarehouseIds)
                            {
                                var stopWh = db.Warehouses.FirstOrDefault(w => w.Id == whId && w.IsActive);
                                if (stopWh == null) throw new InvalidOperationException($"kho dừng id {whId} không hợp lệ hoặc đã bị khóa.");

                                var stop = new RouteTemplateStop
                                {
                                    TemplateId = template.Id,
                                    Seq = seq++,
                                    WarehouseId = whId,
                                    StopName = stopWh.Name
                                };
                                db.RouteTemplateStops.Add(stop);
                            }
                            db.SaveChanges();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("lỗi khi tạo tuyến đường và chặng dừng.", ex);
                    }
                }
            }
        }

        // cập nhật tuyến kèm stops
        public void UpdateTemplateWithStops(RouteTemplate template, List<int> stopWarehouseIds)
        {
            if (template == null || template.Id == 0) throw new ArgumentNullException(nameof(template));

            using (var db = new LogisticsDbContext())
            {
                var existingTemplate = db.RouteTemplates.Include(rt => rt.Stops).FirstOrDefault(rt => rt.Id == template.Id);
                if (existingTemplate == null) throw new Exception($"không tìm thấy tuyến đường id {template.Id} để cập nhật.");

                var fromWh = db.Warehouses.Find(template.FromWarehouseId);
                var toWh = db.Warehouses.Find(template.ToWarehouseId);
                if (fromWh == null || toWh == null) throw new InvalidOperationException("kho đi hoặc kho đến không hợp lệ.");

                if (fromWh.ZoneId != toWh.ZoneId && (stopWarehouseIds == null || !stopWarehouseIds.Any()))
                {
                    throw new InvalidOperationException("tuyến đường liên vùng/xa phải có ít nhất 1 kho trung chuyển.");
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        existingTemplate.Name = template.Name;
                        existingTemplate.FromWarehouseId = template.FromWarehouseId;
                        existingTemplate.ToWarehouseId = template.ToWarehouseId;

                        db.RouteTemplateStops.RemoveRange(existingTemplate.Stops);

                        if (stopWarehouseIds != null && stopWarehouseIds.Any())
                        {
                            int seq = 1;
                            foreach (int whId in stopWarehouseIds)
                            {
                                var stopWh = db.Warehouses.FirstOrDefault(w => w.Id == whId && w.IsActive);
                                if (stopWh == null) throw new InvalidOperationException($"kho dừng id {whId} không hợp lệ hoặc đã bị khóa.");

                                var stop = new RouteTemplateStop
                                {
                                    TemplateId = existingTemplate.Id,
                                    Seq = seq++,
                                    WarehouseId = whId,
                                    StopName = stopWh.Name
                                };
                                db.RouteTemplateStops.Add(stop);
                            }
                        }

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("lỗi khi cập nhật tuyến đường và chặng dừng.", ex);
                    }
                }
            }
        }

        // xây dựng preview tuyến (từ → stop1 → stop2 → đến)
        private static string BuildRoutePreview(LogisticsApp.DAL.Models.RouteTemplate t)
        {
            var names = new List<string>();
            names.Add(t.FromWarehouse?.Name ?? $"#{t.FromWarehouseId}");

            foreach (var s in t.Stops.OrderBy(x => x.Seq))
                names.Add(s.Warehouse?.Name ?? s.StopName ?? $"#{s.WarehouseId}");

            names.Add(t.ToWarehouse?.Name ?? $"#{t.ToWarehouseId}");

            return string.Join(" → ", names);
        }
    }
}