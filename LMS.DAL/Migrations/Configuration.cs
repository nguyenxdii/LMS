using LMS.DAL.Models;
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

            // ===== Customers (khóa theo Email) =====
            db.Customers.AddOrUpdate(x => x.Email,
                new Customer { Name = "Alpha Co.", Email = "alpha@demo.com", Phone = "0901112223", Address = "Quận 1, TP.HCM" },
                new Customer { Name = "Beta Co.", Email = "beta@demo.com", Phone = "0903334445", Address = "Quận 7, TP.HCM" }
            );
            db.SaveChanges();
            var c1 = db.Customers.FirstOrDefault(c => c.Email == "alpha@demo.com");

            // ===== Drivers (khóa theo Phone) =====
            db.Drivers.AddOrUpdate(x => x.Phone,
                new Driver { FullName = "Nguyễn Văn Tài", Phone = "0905556667", LicenseType = "B2" },
                new Driver { FullName = "Trần Văn Xe", Phone = "0907778889", LicenseType = "C" }
            );
            db.SaveChanges();
            var d1 = db.Drivers.FirstOrDefault(x => x.Phone == "0905556667");

            // ===== UserAccounts (khóa theo Username) =====
            db.UserAccounts.AddOrUpdate(x => x.Username,
                new UserAccount { Username = "admin", PasswordHash = "123123", Role = UserRole.Admin, IsActive = true },
                new UserAccount { Username = "alpha", PasswordHash = "123123", Role = UserRole.Customer, IsActive = true, CustomerId = c1?.Id },
                new UserAccount { Username = "tai", PasswordHash = "123123", Role = UserRole.Driver, IsActive = true, DriverId = d1?.Id }
            );
            // NOTE: PasswordHash đang để plain cho demo; khi rảnh hãy chuyển sang hash+salt.
        }
    }
}
