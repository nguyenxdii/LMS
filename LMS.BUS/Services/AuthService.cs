using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace LMS.BUS.Services
{
    // Lý do đăng nhập thất bại để GUI hiển thị đúng thông báo
    public enum LoginFailReason { None, UserNotFound, WrongPassword, Locked }

    // Kết quả đăng nhập
    public class LoginResult
    {
        public bool Ok { get; set; }
        public LoginFailReason Reason { get; set; } = LoginFailReason.None;
        public UserAccount Account { get; set; }
    }

    public class AuthService
    {
        // ============ LOGIN ============
        public LoginResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return new LoginResult { Ok = false, Reason = LoginFailReason.WrongPassword };

            using (var db = new LogisticsDbContext())
            {
                var acc = db.UserAccounts
                            .Include(u => u.Customer)
                            .Include(u => u.Driver)
                            .SingleOrDefault(u => u.Username == username);

                if (acc == null)
                    return new LoginResult { Ok = false, Reason = LoginFailReason.UserNotFound };

                // chặn ngay nếu bị khóa
                if (!acc.IsActive)
                    return new LoginResult { Ok = false, Reason = LoginFailReason.Locked };

                // demo: so sánh plain; sau này thay bằng hash + salt
                if (acc.PasswordHash != password)
                    return new LoginResult { Ok = false, Reason = LoginFailReason.WrongPassword };

                return new LoginResult { Ok = true, Account = acc };
            }
        }

        // ============ REGISTER CUSTOMER ============
        public void RegisterCustomer(string fullName, string username, string password,
                                     string address, string phone, string email)
        {
            using (var db = new LogisticsDbContext())
            {
                if (db.UserAccounts.Any(u => u.Username == username))
                    throw new InvalidOperationException("Tên tài khoản đã tồn tại.");

                var cus = new Customer
                {
                    Name = fullName,
                    Address = address,
                    Phone = phone,
                    Email = email
                };
                db.Customers.Add(cus);
                db.SaveChanges();

                var acc = new UserAccount
                {
                    Username = username,
                    PasswordHash = password,   // TODO: đổi sang hash
                    Role = UserRole.Customer,
                    CustomerId = cus.Id,
                    IsActive = true
                };
                db.UserAccounts.Add(acc);
                db.SaveChanges();
            }
        }

        // ============ REGISTER DRIVER ============
        public void RegisterDriver(string fullName, string username, string password,
                                   string phone, string licenseType)
        {
            using (var db = new LogisticsDbContext())
            {
                if (db.UserAccounts.Any(u => u.Username == username))
                    throw new InvalidOperationException("Tên tài khoản đã tồn tại.");

                var drv = new Driver
                {
                    FullName = fullName,
                    Phone = phone,
                    LicenseType = licenseType,
                    IsActive = true
                };
                db.Drivers.Add(drv);
                db.SaveChanges();

                var acc = new UserAccount
                {
                    Username = username,
                    PasswordHash = password,   // TODO: đổi sang hash
                    Role = UserRole.Driver,
                    DriverId = drv.Id,
                    IsActive = true
                };
                db.UserAccounts.Add(acc);
                db.SaveChanges();
            }
        }
    }
}
