//using LMS.DAL.Models;
//using LogisticsApp.DAL.Models;
//using System;
//using System.Data.Entity.Migrations;
//using System.Linq;

//namespace LMS.DAL.Migrations
//{
//    internal sealed class Configuration : DbMigrationsConfiguration<LMS.DAL.LogisticsDbContext>
//    {
//        public Configuration()
//        {
//            AutomaticMigrationsEnabled = false;
//        }

//        protected override void Seed(LMS.DAL.LogisticsDbContext db)
//        {
//            db.Warehouses.AddOrUpdate(x => x.Name, new[]
//            {
//                // ===== THÀNH PHỐ TRỰC THUỘC TRUNG ƯƠNG =====
//                new Warehouse { Name = "Kho Hà Nội", ZoneId = Zone.North, Type = WarehouseType.Main, Address = "Hà Nội" },
//                new Warehouse { Name = "Kho Hải Phòng", ZoneId = Zone.North, Type = WarehouseType.Main, Address = "Hải Phòng" },
//                new Warehouse { Name = "Kho Huế", ZoneId = Zone.Central, Type = WarehouseType.Main, Address = "Thừa Thiên Huế" },
//                new Warehouse { Name = "Kho Đà Nẵng", ZoneId = Zone.Central, Type = WarehouseType.Main, Address = "Đà Nẵng" },
//                new Warehouse { Name = "Kho TP. Hồ Chí Minh", ZoneId = Zone.South, Type = WarehouseType.Main, Address = "TP.HCM" },
//                new Warehouse { Name = "Kho Cần Thơ", ZoneId = Zone.South, Type = WarehouseType.Main, Address = "Cần Thơ" },

//                // ===== MIỀN BẮC =====
//                new Warehouse { Name = "Kho Bắc Ninh", ZoneId = Zone.North, Type = WarehouseType.Main, Address = "Bắc Ninh" },
//                new Warehouse { Name = "Kho Cao Bằng", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Cao Bằng" },
//                new Warehouse { Name = "Kho Điện Biên", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Điện Biên" },
//                new Warehouse { Name = "Kho Lai Châu", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Lai Châu" },
//                new Warehouse { Name = "Kho Lạng Sơn", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Lạng Sơn" },
//                new Warehouse { Name = "Kho Lào Cai", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Lào Cai" },
//                new Warehouse { Name = "Kho Hưng Yên", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Hưng Yên" },
//                new Warehouse { Name = "Kho Ninh Bình", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Ninh Bình" },
//                new Warehouse { Name = "Kho Phú Thọ", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Phú Thọ" },
//                new Warehouse { Name = "Kho Quảng Ninh", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Quảng Ninh" },
//                new Warehouse { Name = "Kho Sơn La", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Sơn La" },
//                new Warehouse { Name = "Kho Thái Nguyên", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Thái Nguyên" },
//                new Warehouse { Name = "Kho Tuyên Quang", ZoneId = Zone.North, Type = WarehouseType.Local, Address = "Tuyên Quang" },

//                // ===== MIỀN TRUNG =====
//                new Warehouse { Name = "Kho Thanh Hóa", ZoneId = Zone.Central, Type = WarehouseType.Hub, Address = "Thanh Hóa" },
//                new Warehouse { Name = "Kho Hà Tĩnh", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Hà Tĩnh" },
//                new Warehouse { Name = "Kho Nghệ An", ZoneId = Zone.Central, Type = WarehouseType.Main, Address = "Nghệ An" },
//                new Warehouse { Name = "Kho Quảng Ngãi", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Quảng Ngãi" },
//                new Warehouse { Name = "Kho Quảng Trị", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Quảng Trị" },
//                new Warehouse { Name = "Kho Khánh Hòa", ZoneId = Zone.Central, Type = WarehouseType.Hub, Address = "Khánh Hòa" },
//                new Warehouse { Name = "Kho Lâm Đồng", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Lâm Đồng" },
//                new Warehouse { Name = "Kho Gia Lai", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Gia Lai" },

//                // ===== MIỀN NAM =====
//                new Warehouse { Name = "Kho An Giang", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "An Giang" },
//                new Warehouse { Name = "Kho Đồng Nai", ZoneId = Zone.South, Type = WarehouseType.Main, Address = "Đồng Nai" },
//                new Warehouse { Name = "Kho Đồng Tháp", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "Đồng Tháp" },
//                new Warehouse { Name = "Kho Tây Ninh", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "Tây Ninh" },
//                new Warehouse { Name = "Kho Vĩnh Long", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "Vĩnh Long" },
//                new Warehouse { Name = "Kho Cà Mau", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "Cà Mau" }
//            });



//            db.SaveChanges();

//            // ===== Customers (khóa theo Email) =====
//            db.Customers.AddOrUpdate(x => x.Email,
//                new Customer { Name = "Alpha Co.", Email = "alpha@demo.com", Phone = "0901112223", Address = "Quận 1, TP.HCM" },
//                new Customer { Name = "Beta Co.", Email = "beta@demo.com", Phone = "0903334445", Address = "Quận 7, TP.HCM" }
//            );
//            db.SaveChanges();
//            var c1 = db.Customers.FirstOrDefault(c => c.Email == "alpha@demo.com");

//            // ===== Drivers (khóa theo Phone) =====
//            db.Drivers.AddOrUpdate(x => x.Phone,
//                new Driver { FullName = "Nguyễn Văn Tài", Phone = "0905556667", LicenseType = "B2" },
//                new Driver { FullName = "Trần Văn Xe", Phone = "0907778889", LicenseType = "C" }
//            );
//            db.SaveChanges();
//            var d1 = db.Drivers.FirstOrDefault(x => x.Phone == "0905556667");

//            // ===== UserAccounts (khóa theo Username) =====
//            db.UserAccounts.AddOrUpdate(x => x.Username,
//                new UserAccount { Username = "admin", PasswordHash = "123123", Role = UserRole.Admin, IsActive = true },
//                new UserAccount { Username = "alpha", PasswordHash = "123123", Role = UserRole.Customer, IsActive = true, CustomerId = c1?.Id },
//                new UserAccount { Username = "tai", PasswordHash = "123123", Role = UserRole.Driver, IsActive = true, DriverId = d1?.Id }
//            );
//            // NOTE: PasswordHash đang để plain cho demo; khi rảnh hãy chuyển sang hash+salt.

//            // ======== ROUTE TEMPLATE SEED ========
//            var whs = db.Warehouses.ToList();
//            Warehouse W(string name) => whs.First(x => x.Name == name);

//            db.RouteTemplates.AddOrUpdate(x => x.Name, new[]
//            {
//                // ==== CORRIDOR NAM ↔ TRUNG ====
//                new RouteTemplate { Name = "Nam-Trung 1 (TP.HCM → Đà Nẵng)",  FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Đà Nẵng").Id, DistanceKm = 930 },
//                new RouteTemplate { Name = "Nam-Trung 2 (TP.HCM → Huế)",      FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Huế").Id, DistanceKm = 940 },
//                new RouteTemplate { Name = "Nam-Trung 3 (Cần Thơ → Đà Nẵng)",  FromWarehouseId = W("Kho Cần Thơ").Id, ToWarehouseId = W("Kho Đà Nẵng").Id, DistanceKm = 1020 },

//                // ==== CORRIDOR TRUNG ↔ BẮC ====
//                new RouteTemplate { Name = "Trung-Bắc 1 (Đà Nẵng → Hà Nội)",   FromWarehouseId = W("Kho Đà Nẵng").Id, ToWarehouseId = W("Kho Hà Nội").Id, DistanceKm = 770 },
//                new RouteTemplate { Name = "Trung-Bắc 2 (Huế → Hà Nội)",       FromWarehouseId = W("Kho Huế").Id, ToWarehouseId = W("Kho Hà Nội").Id, DistanceKm = 670 },
//                new RouteTemplate { Name = "Trung-Bắc 3 (Đà Nẵng → Hải Phòng)",FromWarehouseId = W("Kho Đà Nẵng").Id, ToWarehouseId = W("Kho Hải Phòng").Id, DistanceKm = 860 },

//                // ==== CORRIDOR NAM ↔ BẮC (XUYÊN VIỆT) ====
//                new RouteTemplate { Name = "Nam-Bắc 1 (TP.HCM → Hà Nội)",      FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Hà Nội").Id, DistanceKm = 1700 },
//                new RouteTemplate { Name = "Nam-Bắc 2 (TP.HCM → Hải Phòng)",   FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Hải Phòng").Id, DistanceKm = 1760 },
//                new RouteTemplate { Name = "Nam-Bắc 3 (Cần Thơ → Hà Nội)",     FromWarehouseId = W("Kho Cần Thơ").Id, ToWarehouseId = W("Kho Hà Nội").Id, DistanceKm = 1800 },
//                new RouteTemplate { Name = "Nam-Bắc 4 (Cần Thơ → Hải Phòng)",  FromWarehouseId = W("Kho Cần Thơ").Id, ToWarehouseId = W("Kho Hải Phòng").Id, DistanceKm = 1850 },
//            });

//            // ===== Helpers tra cứu 1 lần dùng suốt Seed (đặt 1 lần duy nhất) =====
//            var whByName = db.Warehouses.ToList().ToDictionary(x => x.Name);       // Kho theo tên
//            var tplByName = db.RouteTemplates.ToList().ToDictionary(x => x.Name);   // Template theo tên

//            Func<string, Warehouse> Wh = name => whByName[name];
//            Func<string, RouteTemplate> Tpl = name => tplByName[name];

//            // ===== Xóa stops cũ (idempotent) rồi seed lại =====
//            var allStops = db.RouteTemplateStops.ToList();
//            if (allStops.Any())
//            {
//                db.RouteTemplateStops.RemoveRange(allStops);
//                db.SaveChanges();
//            }

//            // ==== NAM–TRUNG 1 (TP.HCM → Đà Nẵng) ====
//            var t1 = Tpl("Nam-Trung 1 (TP.HCM → Đà Nẵng)");
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t1.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t1.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t1.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t1.Id, Seq = 4, WarehouseId = Wh("Kho Đà Nẵng").Id });

//            // ==== NAM–TRUNG 2 (TP.HCM → Huế) ====
//            var t2 = Tpl("Nam-Trung 2 (TP.HCM → Huế)");
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t2.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t2.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t2.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t2.Id, Seq = 4, WarehouseId = Wh("Kho Huế").Id });

//            // ==== NAM–TRUNG 3 (Cần Thơ → Đà Nẵng) ====
//            var t3 = Tpl("Nam-Trung 3 (Cần Thơ → Đà Nẵng)");
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 1, WarehouseId = Wh("Kho Cần Thơ").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 2, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 3, WarehouseId = Wh("Kho Đồng Nai").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 4, WarehouseId = Wh("Kho Khánh Hòa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 5, WarehouseId = Wh("Kho Đà Nẵng").Id });

//            // ==== TRUNG–BẮC 1 (Đà Nẵng → Hà Nội) ====
//            var t4 = Tpl("Trung-Bắc 1 (Đà Nẵng → Hà Nội)");
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 1, WarehouseId = Wh("Kho Đà Nẵng").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 2, WarehouseId = Wh("Kho Huế").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 3, WarehouseId = Wh("Kho Nghệ An").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 4, WarehouseId = Wh("Kho Thanh Hóa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 5, WarehouseId = Wh("Kho Hà Nội").Id });

//            // ==== TRUNG–BẮC 2 (Huế → Hà Nội) ====
//            var t5 = Tpl("Trung-Bắc 2 (Huế → Hà Nội)");
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t5.Id, Seq = 1, WarehouseId = Wh("Kho Huế").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t5.Id, Seq = 2, WarehouseId = Wh("Kho Nghệ An").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t5.Id, Seq = 3, WarehouseId = Wh("Kho Thanh Hóa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t5.Id, Seq = 4, WarehouseId = Wh("Kho Hà Nội").Id });

//            // ==== TRUNG–BẮC 3 (Đà Nẵng → Hải Phòng) ====
//            var t6 = Tpl("Trung-Bắc 3 (Đà Nẵng → Hải Phòng)");
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 1, WarehouseId = Wh("Kho Đà Nẵng").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 2, WarehouseId = Wh("Kho Huế").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 3, WarehouseId = Wh("Kho Nghệ An").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 4, WarehouseId = Wh("Kho Thanh Hóa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 5, WarehouseId = Wh("Kho Hải Phòng").Id });

//            // ==== NAM–BẮC 1 (TP.HCM → Hà Nội) ====
//            var t7 = Tpl("Nam-Bắc 1 (TP.HCM → Hà Nội)");
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 4, WarehouseId = Wh("Kho Đà Nẵng").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 5, WarehouseId = Wh("Kho Huế").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 6, WarehouseId = Wh("Kho Nghệ An").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 7, WarehouseId = Wh("Kho Thanh Hóa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 8, WarehouseId = Wh("Kho Hà Nội").Id });

//            // ==== NAM–BẮC 2 (TP.HCM → Hải Phòng) ====
//            var t8 = Tpl("Nam-Bắc 2 (TP.HCM → Hải Phòng)");
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 4, WarehouseId = Wh("Kho Đà Nẵng").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 5, WarehouseId = Wh("Kho Huế").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 6, WarehouseId = Wh("Kho Nghệ An").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 7, WarehouseId = Wh("Kho Thanh Hóa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 8, WarehouseId = Wh("Kho Hải Phòng").Id });

//            // ==== NAM–BẮC 3 (Cần Thơ → Hà Nội) ====
//            var t9 = Tpl("Nam-Bắc 3 (Cần Thơ → Hà Nội)");
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 1, WarehouseId = Wh("Kho Cần Thơ").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 2, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 3, WarehouseId = Wh("Kho Đồng Nai").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 4, WarehouseId = Wh("Kho Khánh Hòa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 5, WarehouseId = Wh("Kho Đà Nẵng").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 6, WarehouseId = Wh("Kho Huế").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 7, WarehouseId = Wh("Kho Nghệ An").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 8, WarehouseId = Wh("Kho Thanh Hóa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 9, WarehouseId = Wh("Kho Hà Nội").Id });

//            // ==== NAM–BẮC 4 (Cần Thơ → Hải Phòng) ====
//            var t10 = Tpl("Nam-Bắc 4 (Cần Thơ → Hải Phòng)");
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 1, WarehouseId = Wh("Kho Cần Thơ").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 2, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 3, WarehouseId = Wh("Kho Đồng Nai").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 4, WarehouseId = Wh("Kho Khánh Hòa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 5, WarehouseId = Wh("Kho Đà Nẵng").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 6, WarehouseId = Wh("Kho Huế").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 7, WarehouseId = Wh("Kho Nghệ An").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 8, WarehouseId = Wh("Kho Thanh Hóa").Id });
//            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 9, WarehouseId = Wh("Kho Hải Phòng").Id });

//            db.SaveChanges();
//        }
//    }
//}

using LMS.DAL.Models;
using LogisticsApp.DAL.Models;
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
            db.Warehouses.AddOrUpdate(x => x.Name, new[]
            {
                // ===== THÀNH PHỐ TRỰC THUỘC TRUNG ƯƠNG =====
                new Warehouse { Name = "Kho Hà Nội", ZoneId = Zone.North, Type = WarehouseType.Main, Address = "Hà Nội" },
                new Warehouse { Name = "Kho Hải Phòng", ZoneId = Zone.North, Type = WarehouseType.Main, Address = "Hải Phòng" },
                new Warehouse { Name = "Kho Huế", ZoneId = Zone.Central, Type = WarehouseType.Main, Address = "Thừa Thiên Huế" },
                new Warehouse { Name = "Kho Đà Nẵng", ZoneId = Zone.Central, Type = WarehouseType.Main, Address = "Đà Nẵng" },
                new Warehouse { Name = "Kho TP. Hồ Chí Minh", ZoneId = Zone.South, Type = WarehouseType.Main, Address = "TP.HCM" },
                new Warehouse { Name = "Kho Cần Thơ", ZoneId = Zone.South, Type = WarehouseType.Main, Address = "Cần Thơ" },

                // ===== MIỀN BẮC =====
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

                // ===== MIỀN TRUNG =====
                new Warehouse { Name = "Kho Thanh Hóa", ZoneId = Zone.Central, Type = WarehouseType.Hub, Address = "Thanh Hóa" },
                new Warehouse { Name = "Kho Hà Tĩnh", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Hà Tĩnh" },
                new Warehouse { Name = "Kho Nghệ An", ZoneId = Zone.Central, Type = WarehouseType.Main, Address = "Nghệ An" },
                new Warehouse { Name = "Kho Quảng Ngãi", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Quảng Ngãi" },
                new Warehouse { Name = "Kho Quảng Trị", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Quảng Trị" },
                new Warehouse { Name = "Kho Khánh Hòa", ZoneId = Zone.Central, Type = WarehouseType.Hub, Address = "Khánh Hòa" },
                new Warehouse { Name = "Kho Lâm Đồng", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Lâm Đồng" },
                new Warehouse { Name = "Kho Gia Lai", ZoneId = Zone.Central, Type = WarehouseType.Local, Address = "Gia Lai" },

                // ===== MIỀN NAM =====
                new Warehouse { Name = "Kho An Giang", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "An Giang" },
                new Warehouse { Name = "Kho Đồng Nai", ZoneId = Zone.South, Type = WarehouseType.Main, Address = "Đồng Nai" },
                new Warehouse { Name = "Kho Đồng Tháp", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "Đồng Tháp" },
                new Warehouse { Name = "Kho Tây Ninh", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "Tây Ninh" },
                new Warehouse { Name = "Kho Vĩnh Long", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "Vĩnh Long" },
                new Warehouse { Name = "Kho Cà Mau", ZoneId = Zone.South, Type = WarehouseType.Local, Address = "Cà Mau" }
            });

            db.SaveChanges();

            // ===== Customers =====
            db.Customers.AddOrUpdate(x => x.Email,
                new Customer { Name = "Alpha Co.", Email = "alpha@demo.com", Phone = "0901112223", Address = "Quận 1, TP.HCM" },
                new Customer { Name = "Beta Co.", Email = "beta@demo.com", Phone = "0903334445", Address = "Quận 7, TP.HCM" }
            );
            db.SaveChanges();
            var c1 = db.Customers.FirstOrDefault(c => c.Email == "alpha@demo.com");

            // ===== Drivers =====
            db.Drivers.AddOrUpdate(x => x.Phone,
                new Driver { FullName = "Nguyễn Văn Tài", Phone = "0905556667", LicenseType = "B2" },
                new Driver { FullName = "Trần Văn Xe", Phone = "0907778889", LicenseType = "C" }
            );
            db.SaveChanges();
            var d1 = db.Drivers.FirstOrDefault(x => x.Phone == "0905556667");

            // ===== UserAccounts =====
            db.UserAccounts.AddOrUpdate(x => x.Username,
                new UserAccount { Username = "admin", PasswordHash = "123123", Role = UserRole.Admin, IsActive = true },
                new UserAccount { Username = "alpha", PasswordHash = "123123", Role = UserRole.Customer, IsActive = true, CustomerId = c1?.Id },
                new UserAccount { Username = "tai", PasswordHash = "123123", Role = UserRole.Driver, IsActive = true, DriverId = d1?.Id }
            );

            // ======== ROUTE TEMPLATE SEED ========
            var whs = db.Warehouses.ToList();
            Warehouse W(string name) => whs.First(x => x.Name == name);

            db.RouteTemplates.AddOrUpdate(x => x.Name, new[]
            {
                // ==== CORRIDOR NAM ↔ TRUNG ====
                new RouteTemplate { Name = "Nam-Trung 1 (TP.HCM → Đà Nẵng)",  FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Đà Nẵng").Id, DistanceKm = 930 },
                new RouteTemplate { Name = "Nam-Trung 2 (TP.HCM → Huế)",      FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Huế").Id, DistanceKm = 940 },
                new RouteTemplate { Name = "Nam-Trung 3 (Cần Thơ → Đà Nẵng)",  FromWarehouseId = W("Kho Cần Thơ").Id,         ToWarehouseId = W("Kho Đà Nẵng").Id, DistanceKm = 1020 },

                // ==== CORRIDOR TRUNG ↔ BẮC ====
                new RouteTemplate { Name = "Trung-Bắc 1 (Đà Nẵng → Hà Nội)",   FromWarehouseId = W("Kho Đà Nẵng").Id, ToWarehouseId = W("Kho Hà Nội").Id, DistanceKm = 770 },
                new RouteTemplate { Name = "Trung-Bắc 2 (Huế → Hà Nội)",       FromWarehouseId = W("Kho Huế").Id,     ToWarehouseId = W("Kho Hà Nội").Id, DistanceKm = 670 },
                new RouteTemplate { Name = "Trung-Bắc 3 (Đà Nẵng → Hải Phòng)",FromWarehouseId = W("Kho Đà Nẵng").Id, ToWarehouseId = W("Kho Hải Phòng").Id, DistanceKm = 860 },

                // ==== CORRIDOR NAM ↔ BẮC (XUYÊN VIỆT) ====
                new RouteTemplate { Name = "Nam-Bắc 1 (TP.HCM → Hà Nội)",      FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Hà Nội").Id,     DistanceKm = 1700 },
                new RouteTemplate { Name = "Nam-Bắc 2 (TP.HCM → Hải Phòng)",   FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Hải Phòng").Id,  DistanceKm = 1760 },
                new RouteTemplate { Name = "Nam-Bắc 3 (Cần Thơ → Hà Nội)",     FromWarehouseId = W("Kho Cần Thơ").Id,         ToWarehouseId = W("Kho Hà Nội").Id,     DistanceKm = 1800 },
                new RouteTemplate { Name = "Nam-Bắc 4 (Cần Thơ → Hải Phòng)",  FromWarehouseId = W("Kho Cần Thơ").Id,         ToWarehouseId = W("Kho Hải Phòng").Id,  DistanceKm = 1850 },

                // ==== DEMO THÊM MỚI: 3 & 4 CHẶNG ====
                new RouteTemplate { Name = "DEMO 3 chặng (TP.HCM → Khánh Hòa)",
                                    FromWarehouseId = W("Kho TP. Hồ Chí Minh").Id, ToWarehouseId = W("Kho Khánh Hòa").Id, DistanceKm = 430 },

                new RouteTemplate { Name = "DEMO 4 chặng (Huế → Hà Nội)",
                                    FromWarehouseId = W("Kho Huế").Id, ToWarehouseId = W("Kho Hà Nội").Id, DistanceKm = 670 },
            });
            db.SaveChanges();

            // ===== Helpers =====
            var whByName = db.Warehouses.ToList().ToDictionary(x => x.Name);
            var tplByName = db.RouteTemplates.ToList().ToDictionary(x => x.Name);
            Func<string, Warehouse> Wh = name => whByName[name];
            Func<string, RouteTemplate> Tpl = name => tplByName[name];

            // ===== Xóa stops cũ rồi seed lại =====
            var allStops = db.RouteTemplateStops.ToList();
            if (allStops.Any())
            {
                db.RouteTemplateStops.RemoveRange(allStops);
                db.SaveChanges();
            }

            // ==== NAM–TRUNG 1 (TP.HCM → Đà Nẵng) ==== (4 chặng)
            var t1 = Tpl("Nam-Trung 1 (TP.HCM → Đà Nẵng)");
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t1.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t1.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t1.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t1.Id, Seq = 4, WarehouseId = Wh("Kho Đà Nẵng").Id });

            // ==== NAM–TRUNG 2 (TP.HCM → Huế) ==== (4 chặng)
            var t2 = Tpl("Nam-Trung 2 (TP.HCM → Huế)");
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t2.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t2.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t2.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t2.Id, Seq = 4, WarehouseId = Wh("Kho Huế").Id });

            // ==== NAM–TRUNG 3 (Cần Thơ → Đà Nẵng) ==== (5 chặng)
            var t3 = Tpl("Nam-Trung 3 (Cần Thơ → Đà Nẵng)");
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 1, WarehouseId = Wh("Kho Cần Thơ").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 2, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 3, WarehouseId = Wh("Kho Đồng Nai").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 4, WarehouseId = Wh("Kho Khánh Hòa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t3.Id, Seq = 5, WarehouseId = Wh("Kho Đà Nẵng").Id });

            // ==== TRUNG–BẮC 1 (Đà Nẵng → Hà Nội) ==== (5 chặng)
            var t4 = Tpl("Trung-Bắc 1 (Đà Nẵng → Hà Nội)");
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 1, WarehouseId = Wh("Kho Đà Nẵng").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 2, WarehouseId = Wh("Kho Huế").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 3, WarehouseId = Wh("Kho Nghệ An").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 4, WarehouseId = Wh("Kho Thanh Hóa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t4.Id, Seq = 5, WarehouseId = Wh("Kho Hà Nội").Id });

            // ==== TRUNG–BẮC 2 (Huế → Hà Nội) ==== (4 chặng)
            var t5 = Tpl("Trung-Bắc 2 (Huế → Hà Nội)");
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t5.Id, Seq = 1, WarehouseId = Wh("Kho Huế").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t5.Id, Seq = 2, WarehouseId = Wh("Kho Nghệ An").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t5.Id, Seq = 3, WarehouseId = Wh("Kho Thanh Hóa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t5.Id, Seq = 4, WarehouseId = Wh("Kho Hà Nội").Id });

            // ==== TRUNG–BẮC 3 (Đà Nẵng → Hải Phòng) ==== (5 chặng)
            var t6 = Tpl("Trung-Bắc 3 (Đà Nẵng → Hải Phòng)");
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 1, WarehouseId = Wh("Kho Đà Nẵng").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 2, WarehouseId = Wh("Kho Huế").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 3, WarehouseId = Wh("Kho Nghệ An").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 4, WarehouseId = Wh("Kho Thanh Hóa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t6.Id, Seq = 5, WarehouseId = Wh("Kho Hải Phòng").Id });

            // ==== NAM–BẮC 1 (TP.HCM → Hà Nội) ==== (8 chặng)
            var t7 = Tpl("Nam-Bắc 1 (TP.HCM → Hà Nội)");
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 4, WarehouseId = Wh("Kho Đà Nẵng").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 5, WarehouseId = Wh("Kho Huế").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 6, WarehouseId = Wh("Kho Nghệ An").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 7, WarehouseId = Wh("Kho Thanh Hóa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t7.Id, Seq = 8, WarehouseId = Wh("Kho Hà Nội").Id });

            // ==== NAM–BẮC 2 (TP.HCM → Hải Phòng) ==== (8 chặng)
            var t8 = Tpl("Nam-Bắc 2 (TP.HCM → Hải Phòng)");
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 4, WarehouseId = Wh("Kho Đà Nẵng").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 5, WarehouseId = Wh("Kho Huế").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 6, WarehouseId = Wh("Kho Nghệ An").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 7, WarehouseId = Wh("Kho Thanh Hóa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t8.Id, Seq = 8, WarehouseId = Wh("Kho Hải Phòng").Id });

            // ==== NAM–BẮC 3 (Cần Thơ → Hà Nội) ==== (9 chặng)
            var t9 = Tpl("Nam-Bắc 3 (Cần Thơ → Hà Nội)");
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 1, WarehouseId = Wh("Kho Cần Thơ").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 2, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 3, WarehouseId = Wh("Kho Đồng Nai").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 4, WarehouseId = Wh("Kho Khánh Hòa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 5, WarehouseId = Wh("Kho Đà Nẵng").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 6, WarehouseId = Wh("Kho Huế").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 7, WarehouseId = Wh("Kho Nghệ An").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 8, WarehouseId = Wh("Kho Thanh Hóa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t9.Id, Seq = 9, WarehouseId = Wh("Kho Hà Nội").Id });

            // ==== NAM–BẮC 4 (Cần Thơ → Hải Phòng) ==== (9 chặng)
            var t10 = Tpl("Nam-Bắc 4 (Cần Thơ → Hải Phòng)");
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 1, WarehouseId = Wh("Kho Cần Thơ").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 2, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 3, WarehouseId = Wh("Kho Đồng Nai").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 4, WarehouseId = Wh("Kho Khánh Hòa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 5, WarehouseId = Wh("Kho Đà Nẵng").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 6, WarehouseId = Wh("Kho Huế").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 7, WarehouseId = Wh("Kho Nghệ An").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 8, WarehouseId = Wh("Kho Thanh Hóa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = t10.Id, Seq = 9, WarehouseId = Wh("Kho Hải Phòng").Id });

            // ==== DEMO 3 CHẶNG ==== (TP.HCM → Khánh Hòa)
            var td3 = Tpl("DEMO 3 chặng (TP.HCM → Khánh Hòa)");
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td3.Id, Seq = 1, WarehouseId = Wh("Kho TP. Hồ Chí Minh").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td3.Id, Seq = 2, WarehouseId = Wh("Kho Đồng Nai").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td3.Id, Seq = 3, WarehouseId = Wh("Kho Khánh Hòa").Id });

            // ==== DEMO 4 CHẶNG ==== (Huế → Hà Nội)
            var td4 = Tpl("DEMO 4 chặng (Huế → Hà Nội)");
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td4.Id, Seq = 1, WarehouseId = Wh("Kho Huế").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td4.Id, Seq = 2, WarehouseId = Wh("Kho Nghệ An").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td4.Id, Seq = 3, WarehouseId = Wh("Kho Thanh Hóa").Id });
            db.RouteTemplateStops.Add(new RouteTemplateStop { TemplateId = td4.Id, Seq = 4, WarehouseId = Wh("Kho Hà Nội").Id });

            db.SaveChanges();
        }
    }
}
