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
                // ===== Miền Bắc (12 tỉnh) =====
                new Warehouse { Name = "Kho Hà Nội",             Address = "Trung tâm khai thác Hà Nội",              ZoneId = Zone.North },
                new Warehouse { Name = "Kho Hải Phòng",          Address = "Trung tâm khai thác Hải Phòng",           ZoneId = Zone.North },
                new Warehouse { Name = "Kho Quảng Ninh",         Address = "Trung tâm khai thác Quảng Ninh",          ZoneId = Zone.North },
                new Warehouse { Name = "Kho Thái Nguyên",        Address = "Trung tâm khai thác Thái Nguyên",         ZoneId = Zone.North },
                new Warehouse { Name = "Kho Bắc Giang",          Address = "Trung tâm khai thác Bắc Giang",           ZoneId = Zone.North },
                new Warehouse { Name = "Kho Phú Thọ",            Address = "Trung tâm khai thác Phú Thọ",             ZoneId = Zone.North },
                new Warehouse { Name = "Kho Vĩnh Phúc",          Address = "Trung tâm khai thác Vĩnh Phúc",           ZoneId = Zone.North },
                new Warehouse { Name = "Kho Bắc Ninh",           Address = "Trung tâm khai thác Bắc Ninh",            ZoneId = Zone.North },
                new Warehouse { Name = "Kho Nam Định",           Address = "Trung tâm khai thác Nam Định",            ZoneId = Zone.North },
                new Warehouse { Name = "Kho Ninh Bình",          Address = "Trung tâm khai thác Ninh Bình",           ZoneId = Zone.North },
                new Warehouse { Name = "Kho Thái Bình",          Address = "Trung tâm khai thác Thái Bình",           ZoneId = Zone.North },
                new Warehouse { Name = "Kho Hưng Yên",           Address = "Trung tâm khai thác Hưng Yên",            ZoneId = Zone.North },

                // ===== Miền Trung (11 tỉnh) =====
                new Warehouse { Name = "Kho Thanh Hóa",          Address = "Trung tâm khai thác Thanh Hóa",           ZoneId = Zone.Central },
                new Warehouse { Name = "Kho Nghệ An",            Address = "Trung tâm khai thác Nghệ An",             ZoneId = Zone.Central },
                new Warehouse { Name = "Kho Hà Tĩnh",            Address = "Trung tâm khai thác Hà Tĩnh",             ZoneId = Zone.Central },
                new Warehouse { Name = "Kho Quảng Bình",         Address = "Trung tâm khai thác Quảng Bình",          ZoneId = Zone.Central },
                new Warehouse { Name = "Kho Thừa Thiên Huế",     Address = "Trung tâm khai thác Thừa Thiên Huế",      ZoneId = Zone.Central },
                new Warehouse { Name = "Kho Đà Nẵng",            Address = "Trung tâm khai thác Đà Nẵng",             ZoneId = Zone.Central },
                new Warehouse { Name = "Kho Quảng Nam",          Address = "Trung tâm khai thác Quảng Nam",           ZoneId = Zone.Central },
                new Warehouse { Name = "Kho Quảng Ngãi",         Address = "Trung tâm khai thác Quảng Ngãi",          ZoneId = Zone.Central },
                new Warehouse { Name = "Kho Bình Định",          Address = "Trung tâm khai thác Bình Định",           ZoneId = Zone.Central },
                new Warehouse { Name = "Kho Khánh Hòa",          Address = "Trung tâm khai thác Khánh Hòa",           ZoneId = Zone.Central },
                new Warehouse { Name = "Kho Lâm Đồng",           Address = "Trung tâm khai thác Lâm Đồng",            ZoneId = Zone.Central },

                // ===== Miền Nam (11 tỉnh) =====
                new Warehouse { Name = "Kho Bình Phước",         Address = "Trung tâm khai thác Bình Phước",          ZoneId = Zone.South },
                new Warehouse { Name = "Kho Tây Ninh",           Address = "Trung tâm khai thác Tây Ninh",            ZoneId = Zone.South },
                new Warehouse { Name = "Kho Bình Dương",         Address = "Trung tâm khai thác Bình Dương",          ZoneId = Zone.South },
                new Warehouse { Name = "Kho Đồng Nai",           Address = "Trung tâm khai thác Đồng Nai",            ZoneId = Zone.South },
                new Warehouse { Name = "Kho Bà Rịa - Vũng Tàu",  Address = "Trung tâm khai thác BR-VT",               ZoneId = Zone.South },
                new Warehouse { Name = "Kho TP. Hồ Chí Minh",    Address = "Trung tâm khai thác TP.HCM",              ZoneId = Zone.South },
                new Warehouse { Name = "Kho Long An",            Address = "Trung tâm khai thác Long An",             ZoneId = Zone.South },
                new Warehouse { Name = "Kho Tiền Giang",         Address = "Trung tâm khai thác Tiền Giang",          ZoneId = Zone.South },
                new Warehouse { Name = "Kho Cần Thơ",            Address = "Trung tâm khai thác Cần Thơ",             ZoneId = Zone.South },
                new Warehouse { Name = "Kho An Giang",           Address = "Trung tâm khai thác An Giang",            ZoneId = Zone.South },
                new Warehouse { Name = "Kho Cà Mau",             Address = "Trung tâm khai thác Cà Mau",              ZoneId = Zone.South }
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
                new UserAccount { Username = "admin", PasswordHash = "123456", Role = UserRole.Admin, IsActive = true },
                new UserAccount { Username = "alpha", PasswordHash = "123456", Role = UserRole.Customer, IsActive = true, CustomerId = c1?.Id },
                new UserAccount { Username = "tai", PasswordHash = "123456", Role = UserRole.Driver, IsActive = true, DriverId = d1?.Id }
            );
            // NOTE: PasswordHash đang để plain cho demo; khi rảnh hãy chuyển sang hash+salt.
        }
    }
}
