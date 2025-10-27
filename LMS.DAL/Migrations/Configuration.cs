// LMS.DAL/Migrations/Configuration.cs
using LMS.DAL.Models;
using LogisticsApp.DAL.Models; // Ensure this namespace is correct if RouteTemplate/Stop are here
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LMS.DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<LMS.DAL.LogisticsDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LMS.DAL.LogisticsDbContext db)
        {
            // ===== Seed Warehouses =====
            db.Warehouses.AddOrUpdate(x => x.Name, new[]
            {
                // (Keep your list of Warehouses here)
                new Warehouse { Name = "Kho Hà Nội", ZoneId = Zone.North, Type = WarehouseType.Main, Address = "Hà Nội" },
                new Warehouse { Name = "Kho Hải Phòng", ZoneId = Zone.North, Type = WarehouseType.Main, Address = "Hải Phòng" },
                new Warehouse { Name = "Kho Huế", ZoneId = Zone.Central, Type = WarehouseType.Main, Address = "Thừa Thiên Huế" },
                new Warehouse { Name = "Kho Đà Nẵng", ZoneId = Zone.Central, Type = WarehouseType.Main, Address = "Đà Nẵng" },
                new Warehouse { Name = "Kho TP. Hồ Chí Minh", ZoneId = Zone.South, Type = WarehouseType.Main, Address = "TP.HCM" },
                new Warehouse { Name = "Kho Cần Thơ", ZoneId = Zone.South, Type = WarehouseType.Main, Address = "Cần Thơ" },
                new Warehouse { Name = "Kho Bắc Ninh", ZoneId = Zone.North, Type = WarehouseType.Main, Address = "Bắc Ninh" },
                new Warehouse { Name = "Kho Cao Bằng", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Cao Bằng" },
                new Warehouse { Name = "Kho Điện Biên", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Điện Biên" },
                new Warehouse { Name = "Kho Lai Châu", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Lai Châu" },
                new Warehouse { Name = "Kho Lạng Sơn", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Lạng Sơn" },
                new Warehouse { Name = "Kho Lào Cai", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Lào Cai" },
                new Warehouse { Name = "Kho Hưng Yên", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Hưng Yên" },
                new Warehouse { Name = "Kho Ninh Bình", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Ninh Bình" },
                new Warehouse { Name = "Kho Phú Thọ", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Phú Thọ" },
                new Warehouse { Name = "Kho Quảng Ninh", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Quảng Ninh" },
                new Warehouse { Name = "Kho Sơn La", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Sơn La" },
                new Warehouse { Name = "Kho Thái Nguyên", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Thái Nguyên" },
                new Warehouse { Name = "Kho Tuyên Quang", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Tuyên Quang" },
                new Warehouse { Name = "Kho Thanh Hóa", ZoneId = Zone.Central, Type = WarehouseType.Hub, Address = "Thanh Hóa" },
                new Warehouse { Name = "Kho Hà Tĩnh", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Hà Tĩnh" },
                new Warehouse { Name = "Kho Nghệ An", ZoneId = Zone.Central, Type = WarehouseType.Main, Address = "Nghệ An" },
                new Warehouse { Name = "Kho Quảng Ngãi", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Quảng Ngãi" },
                new Warehouse { Name = "Kho Quảng Trị", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Quảng Trị" },
                new Warehouse { Name = "Kho Khánh Hòa", ZoneId = Zone.Central, Type = WarehouseType.Hub, Address = "Khánh Hòa" },
                new Warehouse { Name = "Kho Lâm Đồng", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Lâm Đồng" },
                new Warehouse { Name = "Kho Gia Lai", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Gia Lai" },
                new Warehouse { Name = "Kho An Giang", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "An Giang" },
                new Warehouse { Name = "Kho Đồng Nai", ZoneId = Zone.South, Type = WarehouseType.Main, Address = "Đồng Nai" },
                new Warehouse { Name = "Kho Đồng Tháp", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "Đồng Tháp" },
                new Warehouse { Name = "Kho Tây Ninh", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "Tây Ninh" },
                new Warehouse { Name = "Kho Vĩnh Long", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "Vĩnh Long" },
                new Warehouse { Name = "Kho Cà Mau", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "Cà Mau" }
            });
            db.SaveChanges(); // Save Warehouses

            // ===== Seed Customers =====
            db.Customers.AddOrUpdate(c => c.Email,
                new Customer { Name = "Alpha Co.", Email = "alpha@demo.com", Phone = "0901112223", Address = "Quận 1, TP.HCM" },
                new Customer { Name = "Beta Co.", Email = "beta@demo.com", Phone = "0903334445", Address = "Quận 7, TP.HCM" }
            );
            db.SaveChanges(); // Save Customers to get IDs
            // Get Customer ID by querying again
            var customerAlpha = db.Customers.FirstOrDefault(c => c.Email == "alpha@demo.com");
            int? customerAlphaId = customerAlpha?.Id; // Get ID if customerAlpha is not null

            // ===== Seed Vehicles (WITHOUT assigning DriverId here) =====
            db.Vehicles.AddOrUpdate(v => v.PlateNo,
                new Vehicle { PlateNo = "51F-111.11", Type = "Xe tải 1.5T", CapacityKg = 1500, Status = VehicleStatus.Active },
                new Vehicle { PlateNo = "60C-222.22", Type = "Xe tải 5T", CapacityKg = 5000, Status = VehicleStatus.Active },
                new Vehicle { PlateNo = "59-G1 333.33", Type = "Xe máy", Status = VehicleStatus.Active, CapacityKg = null, VolumeM3 = null } // Motorcycle might not need capacity/volume
            );
            db.SaveChanges(); // Save Vehicles to get IDs

            // Get Vehicle objects back after saving
            var vehicle1 = db.Vehicles.FirstOrDefault(v => v.PlateNo == "51F-111.11");
            var vehicle2 = db.Vehicles.FirstOrDefault(v => v.PlateNo == "60C-222.22");

            // ===== Seed Drivers (Assign VehicleId HERE) =====
            db.Drivers.AddOrUpdate(d => d.Phone,
                new Driver { FullName = "Nguyễn Văn Tài", Phone = "0905556667", LicenseType = "B2", CitizenId = "079011111111", IsActive = true, VehicleId = vehicle1?.Id }, // Assign VehicleId
                new Driver { FullName = "Trần Văn Xe", Phone = "0907778889", LicenseType = "C", CitizenId = "079022222222", IsActive = true, VehicleId = vehicle2?.Id }       // Assign VehicleId
            );
            db.SaveChanges(); // Save Drivers to get IDs
            // Get Driver ID by querying again
            var driverTai = db.Drivers.FirstOrDefault(d => d.Phone == "0905556667");
            int? driverTaiId = driverTai?.Id; // Get ID if driverTai is not null

            // ===== Seed UserAccounts (Use the retrieved IDs) =====
            // *** Ensure no incorrect reference to Driver_Id ***
            db.UserAccounts.AddOrUpdate(ua => ua.Username,
                new UserAccount { Username = "admin", PasswordHash = "123123", Role = UserRole.Admin, IsActive = true }, // TODO: Hash password
                new UserAccount { Username = "alpha", PasswordHash = "123123", Role = UserRole.Customer, IsActive = true, CustomerId = customerAlphaId }, // Use customerAlphaId
                new UserAccount { Username = "tai", PasswordHash = "123123", Role = UserRole.Driver, IsActive = true, DriverId = driverTaiId }       // Use driverTaiId
            );
            db.SaveChanges(); // Save UserAccounts

            // ======== ROUTE TEMPLATE SEED (Keep this part) ========
            var whs = db.Warehouses.ToList(); // Get latest warehouse list
            Warehouse W(string name) => whs.First(x => x.Name == name);

            db.RouteTemplates.AddOrUpdate(x => x.Name, new[]
            {
                // (Keep your list of RouteTemplates)
                 new RouteTemplate { Name = "Nam-Trung 1 (TP.HCM → Đà Nẵng)",  FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Đà Nẵng").Id, DistanceKm = 930 },
                 new RouteTemplate { Name = "Nam-Trung 2 (TP.HCM → Huế)",      FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Huế").Id, DistanceKm = 940 },
                 new RouteTemplate { Name = "Nam-Trung 3 (Cần Thơ → Đà Nẵng)",  FromWarehouseId = W("Kho Cần Thơ").Id,         ToWarehouseId = W("Kho Đà Nẵng").Id, DistanceKm = 1020 },
                 new RouteTemplate { Name = "Trung-Bắc 1 (Đà Nẵng → Hà Nội)",   FromWarehouseId = W("Kho Đà Nẵng").Id, ToWarehouseId = W("Kho Hà Nội").Id, DistanceKm = 770 },
                 new RouteTemplate { Name = "Trung-Bắc 2 (Huế → Hà Nội)",       FromWarehouseId = W("Kho Huế").Id,     ToWarehouseId = W("Kho Hà Nội").Id, DistanceKm = 670 },
                 new RouteTemplate { Name = "Trung-Bắc 3 (Đà Nẵng → Hải Phòng)",FromWarehouseId = W("Kho Đà Nẵng").Id, ToWarehouseId = W("Kho Hải Phòng").Id, DistanceKm = 860 },
                 new RouteTemplate { Name = "Nam-Bắc 1 (TP.HCM → Hà Nội)",      FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Hà Nội").Id,     DistanceKm = 1700 },
                 new RouteTemplate { Name = "Nam-Bắc 2 (TP.HCM → Hải Phòng)",   FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Hải Phòng").Id,  DistanceKm = 1760 },
                 new RouteTemplate { Name = "Nam-Bắc 3 (Cần Thơ → Hà Nội)",     FromWarehouseId = W("Kho Cần Thơ").Id,         ToWarehouseId = W("Kho Hà Nội").Id,     DistanceKm = 1800 },
                 new RouteTemplate { Name = "Nam-Bắc 4 (Cần Thơ → Hải Phòng)",  FromWarehouseId = W("Kho Cần Thơ").Id,         ToWarehouseId = W("Kho Hải Phòng").Id,  DistanceKm = 1850 },
                 new RouteTemplate { Name = "DEMO 3 chặng (TP.HCM → Khánh Hòa)", FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Khánh Hòa").Id, DistanceKm = 430 },
                 new RouteTemplate { Name = "DEMO 4 chặng (Huế → Hà Nội)", FromWarehouseId = W("Kho Huế").Id, ToWarehouseId = W("Kho Hà Nội").Id, DistanceKm = 670 },
            });
            db.SaveChanges(); // Save RouteTemplates

            // ===== Helpers (Get latest data after SaveChanges) =====
            var whByName = db.Warehouses.ToDictionary(x => x.Name);
            var tplByName = db.RouteTemplates.ToDictionary(x => x.Name);
            // Func remains the same
            Func<string, Warehouse> Wh = name => whByName[name];
            Func<string, RouteTemplate> Tpl = name => tplByName[name];

            // ===== Delete old stops then seed again =====
            // *** Add null check before RemoveRange ***
            var allStops = db.RouteTemplateStops.ToList();
            if (allStops != null && allStops.Any())
            {
                db.RouteTemplateStops.RemoveRange(allStops);
                db.SaveChanges();
            }

            // ===== Seed RouteTemplateStops (Keep this part, wrapped in try-catch) =====
            try
            {
                // (Keep all your db.RouteTemplateStops.Add(...) lines here)
                var t1 = Tpl("Nam-Trung 1 (TP.HCM → Đà Nẵng)");
                if (t1 != null)
                {
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t1.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t1.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t1.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t1.Id, Seq = 4, WarehouseId = Wh("Kho Đà Nẵng").Id });
                }
                var t2 = Tpl("Nam-Trung 2 (TP.HCM → Huế)");
                if (t2 != null)
                {
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t2.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t2.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t2.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t2.Id, Seq = 4, WarehouseId = Wh("Kho Huế").Id });
                }
                var t3 = Tpl("Nam-Trung 3 (Cần Thơ → Đà Nẵng)");
                if (t3 != null)
                {
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 1, WarehouseId = Wh("Kho Cần Thơ").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 2, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 3, WarehouseId = Wh("Kho Đồng Nai").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 4, WarehouseId = Wh("Kho Khánh Hòa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 5, WarehouseId = Wh("Kho Đà Nẵng").Id });
                }
                var t4 = Tpl("Trung-Bắc 1 (Đà Nẵng → Hà Nội)");
                if (t4 != null)
                {
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 1, WarehouseId = Wh("Kho Đà Nẵng").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 2, WarehouseId = Wh("Kho Huế").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 3, WarehouseId = Wh("Kho Nghệ An").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 4, WarehouseId = Wh("Kho Thanh Hóa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 5, WarehouseId = Wh("Kho Hà Nội").Id });
                }
                var t5 = Tpl("Trung-Bắc 2 (Huế → Hà Nội)");
                if (t5 != null)
                {
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t5.Id, Seq = 1, WarehouseId = Wh("Kho Huế").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t5.Id, Seq = 2, WarehouseId = Wh("Kho Nghệ An").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t5.Id, Seq = 3, WarehouseId = Wh("Kho Thanh Hóa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t5.Id, Seq = 4, WarehouseId = Wh("Kho Hà Nội").Id });
                }
                var t6 = Tpl("Trung-Bắc 3 (Đà Nẵng → Hải Phòng)");
                if (t6 != null)
                {
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 1, WarehouseId = Wh("Kho Đà Nẵng").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 2, WarehouseId = Wh("Kho Huế").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 3, WarehouseId = Wh("Kho Nghệ An").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 4, WarehouseId = Wh("Kho Thanh Hóa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 5, WarehouseId = Wh("Kho Hải Phòng").Id });
                }
                var t7 = Tpl("Nam-Bắc 1 (TP.HCM → Hà Nội)");
                if (t7 != null)
                {
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 4, WarehouseId = Wh("Kho Đà Nẵng").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 5, WarehouseId = Wh("Kho Huế").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 6, WarehouseId = Wh("Kho Nghệ An").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 7, WarehouseId = Wh("Kho Thanh Hóa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 8, WarehouseId = Wh("Kho Hà Nội").Id });
                }
                var t8 = Tpl("Nam-Bắc 2 (TP.HCM → Hải Phòng)");
                if (t8 != null)
                {
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 4, WarehouseId = Wh("Kho Đà Nẵng").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 5, WarehouseId = Wh("Kho Huế").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 6, WarehouseId = Wh("Kho Nghệ An").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 7, WarehouseId = Wh("Kho Thanh Hóa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 8, WarehouseId = Wh("Kho Hải Phòng").Id });
                }
                var t9 = Tpl("Nam-Bắc 3 (Cần Thơ → Hà Nội)");
                if (t9 != null)
                {
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 1, WarehouseId = Wh("Kho Cần Thơ").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 2, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 3, WarehouseId = Wh("Kho Đồng Nai").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 4, WarehouseId = Wh("Kho Khánh Hòa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 5, WarehouseId = Wh("Kho Đà Nẵng").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 6, WarehouseId = Wh("Kho Huế").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 7, WarehouseId = Wh("Kho Nghệ An").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 8, WarehouseId = Wh("Kho Thanh Hóa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 9, WarehouseId = Wh("Kho Hà Nội").Id });
                }
                var t10 = Tpl("Nam-Bắc 4 (Cần Thơ → Hải Phòng)");
                if (t10 != null)
                {
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 1, WarehouseId = Wh("Kho Cần Thơ").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 2, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 3, WarehouseId = Wh("Kho Đồng Nai").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 4, WarehouseId = Wh("Kho Khánh Hòa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 5, WarehouseId = Wh("Kho Đà Nẵng").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 6, WarehouseId = Wh("Kho Huế").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 7, WarehouseId = Wh("Kho Nghệ An").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 8, WarehouseId = Wh("Kho Thanh Hóa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 9, WarehouseId = Wh("Kho Hải Phòng").Id });
                }
                var td3 = Tpl("DEMO 3 chặng (TP.HCM → Khánh Hòa)");
                if (td3 != null)
                {
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td3.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td3.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td3.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });
                }
                var td4 = Tpl("DEMO 4 chặng (Huế → Hà Nội)");
                if (td4 != null)
                {
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td4.Id, Seq = 1, WarehouseId = Wh("Kho Huế").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td4.Id, Seq = 2, WarehouseId = Wh("Kho Nghệ An").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td4.Id, Seq = 3, WarehouseId = Wh("Kho Thanh Hóa").Id });
                    db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td4.Id, Seq = 4, WarehouseId = Wh("Kho Hà Nội").Id });
                }

                db.SaveChanges(); // Save RouteTemplateStops
            }
            catch (Exception ex)
            {
                // Ghi lỗi ra Output window để dễ debug
                System.Diagnostics.Debug.WriteLine($"Error seeding RouteTemplateStops: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                // Có thể ném lại lỗi nếu muốn dừng quá trình seed
                // throw;
            }
        }
    }
}