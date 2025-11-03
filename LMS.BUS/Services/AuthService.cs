using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Data.Entity; // giữ nếu dùng các tính năng EF khác
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

                if (!acc.IsActive) // kiểm tra tài khoản bị khóa
                    return new LoginResult { Ok = false, Reason = LoginFailReason.Locked };

                if (acc.PasswordHash != password)
                    return new LoginResult { Ok = false, Reason = LoginFailReason.WrongPassword };

                return new LoginResult { Ok = true, Account = acc };
            }
        }

        // ============ đăng ký khách hàng ============
        public void RegisterCustomer(string fullName, string username, string password,
                                     string address, string phone, string email, byte[] avatarData)
        {
            using (var db = new LogisticsDbContext())
            {
                if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    throw new InvalidOperationException("email không hợp lệ (chỉ dùng ký tự la-tinh, không dấu).");
                }

                if (db.UserAccounts.Any(u => u.Username == username))
                    throw new InvalidOperationException("tên tài khoản đã tồn tại.");

                var cus = new Customer
                {
                    Name = fullName,
                    Address = address,
                    Phone = phone,
                    Email = email,
                    AvatarData = avatarData
                };
                db.Customers.Add(cus);
                db.SaveChanges(); // lưu để lấy Id

                var acc = new UserAccount
                {
                    Username = username,
                    PasswordHash = password, // todo: hash mật khẩu này!
                    Role = UserRole.Customer,
                    CustomerId = cus.Id,
                    IsActive = true, // tài khoản mới luôn active
                    CreatedAt = DateTime.Now,
                };
                db.UserAccounts.Add(acc);
                db.SaveChanges();
            }
        }

        // ============ đăng ký tài xế ============
        public void RegisterDriver(string fullName, string username, string password,
                                   string phone, string licenseType, string citizenId, byte[] avatarData)
        {
            using (var db = new LogisticsDbContext())
            {
                if (db.UserAccounts.Any(u => u.Username == username))
                    throw new InvalidOperationException("tên tài khoản đã tồn tại.");

                var drv = new Driver
                {
                    FullName = fullName,
                    Phone = phone,
                    LicenseType = licenseType,
                    CitizenId = citizenId,
                    IsActive = true, // tài xế mới mặc định active
                    AvatarData = avatarData,
                };
                db.Drivers.Add(drv);
                db.SaveChanges(); // lưu để lấy Id

                var acc = new UserAccount
                {
                    Username = username,
                    PasswordHash = password, // todo: hash mật khẩu này!
                    Role = UserRole.Driver,
                    DriverId = drv.Id,
                    IsActive = true, // tài khoản mới luôn active
                    CreatedAt = DateTime.Now
                };
                db.UserAccounts.Add(acc);
                db.SaveChanges();
            }
        }
    }
}