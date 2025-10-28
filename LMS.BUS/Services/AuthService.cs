using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Data.Entity; // Keep this if you use other EF features here
using System.Linq;
using System.Text.RegularExpressions;

namespace LMS.BUS.Services
{
    public enum LoginFailReason { None, UserNotFound, WrongPassword, Locked }

    public class LoginResult
    {
        public bool Ok { get; set; }
        public LoginFailReason Reason { get; set; } = LoginFailReason.None;
        public UserAccount Account { get; set; }
    }

    public class AuthService
    {
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

                if (!acc.IsActive) // Dòng kiểm tra quan trọng!
                    return new LoginResult { Ok = false, Reason = LoginFailReason.Locked };

                if (acc.PasswordHash != password)
                    return new LoginResult { Ok = false, Reason = LoginFailReason.WrongPassword };

                return new LoginResult { Ok = true, Account = acc };
            }
        }

        // ============ REGISTER CUSTOMER ============
        public void RegisterCustomer(string fullName, string username, string password,
                                     string address, string phone, string email, byte[] avatarData)
        {
            using (var db = new LogisticsDbContext())
            {
                if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    throw new InvalidOperationException("Email không hợp lệ (Chỉ dùng ký tự la-tinh, không dấu).");
                }

                if (db.UserAccounts.Any(u => u.Username == username))
                    throw new InvalidOperationException("Tên tài khoản đã tồn tại.");

                var cus = new Customer
                {
                    Name = fullName,
                    Address = address,
                    Phone = phone,
                    Email = email,
                    AvatarData = avatarData
                };
                db.Customers.Add(cus);
                db.SaveChanges(); // Lưu để lấy Id

                var acc = new UserAccount
                {
                    Username = username,
                    PasswordHash = password,   // TODO: Hash mật khẩu này!
                    Role = UserRole.Customer,
                    CustomerId = cus.Id,
                    IsActive = true, // Tài khoản mới luôn Active
                    CreatedAt = DateTime.Now,
                };
                db.UserAccounts.Add(acc);
                db.SaveChanges();
            }
        }

        // ============ REGISTER DRIVER ============
        public void RegisterDriver(string fullName, string username, string password,
                                   string phone, string licenseType, string citizenId, byte[] avatarData)
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
                    CitizenId = citizenId,
                    IsActive = true, // Driver mới mặc định Active
                    AvatarData = avatarData,
                };
                db.Drivers.Add(drv);
                db.SaveChanges(); // Lưu để lấy Id

                var acc = new UserAccount
                {
                    Username = username,
                    PasswordHash = password,   // TODO: Hash mật khẩu này!
                    Role = UserRole.Driver,
                    DriverId = drv.Id,
                    IsActive = true, // Tài khoản mới luôn Active
                    CreatedAt = DateTime.Now
                };
                db.UserAccounts.Add(acc);
                db.SaveChanges();
            }
        }
    }
}