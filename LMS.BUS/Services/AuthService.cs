//using LMS.DAL;
//using LMS.DAL.Models;
//using System;
//using System.Data.Entity;
//using System.Linq;

//namespace LMS.BUS.Services
//{
//    // Lý do đăng nhập thất bại để GUI hiển thị đúng thông báo
//    public enum LoginFailReason { None, UserNotFound, WrongPassword, Locked }

//    // Kết quả đăng nhập
//    public class LoginResult
//    {
//        public bool Ok { get; set; }
//        public LoginFailReason Reason { get; set; } = LoginFailReason.None;
//        public UserAccount Account { get; set; }
//    }

//    public class AuthService
//    {
//        // ============ LOGIN ============
//        public LoginResult Login(string username, string password)
//        {
//            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
//                return new LoginResult { Ok = false, Reason = LoginFailReason.WrongPassword };

//            using (var db = new LogisticsDbContext())
//            {
//                var acc = db.UserAccounts
//                            .Include(u => u.Customer)
//                            .Include(u => u.Driver)
//                            .SingleOrDefault(u => u.Username == username);

//                if (acc == null)
//                    return new LoginResult { Ok = false, Reason = LoginFailReason.UserNotFound };

//                // chặn ngay nếu bị khóa
//                if (!acc.IsActive)
//                    return new LoginResult { Ok = false, Reason = LoginFailReason.Locked };

//                // demo: so sánh plain; sau này thay bằng hash + salt
//                if (acc.PasswordHash != password)
//                    return new LoginResult { Ok = false, Reason = LoginFailReason.WrongPassword };

//                return new LoginResult { Ok = true, Account = acc };
//            }
//        }

//        // ============ REGISTER CUSTOMER ============
//        public void RegisterCustomer(string fullName, string username, string password,
//                                     string address, string phone, string email)
//        {
//            using (var db = new LogisticsDbContext())
//            {
//                if (db.UserAccounts.Any(u => u.Username == username))
//                    throw new InvalidOperationException("Tên tài khoản đã tồn tại.");

//                var cus = new Customer
//                {
//                    Name = fullName,
//                    Address = address,
//                    Phone = phone,
//                    Email = email
//                };
//                db.Customers.Add(cus);
//                db.SaveChanges();

//                var acc = new UserAccount
//                {
//                    Username = username,
//                    PasswordHash = password,   // TODO: đổi sang hash
//                    Role = UserRole.Customer,
//                    CustomerId = cus.Id,
//                    IsActive = true
//                };
//                db.UserAccounts.Add(acc);
//                db.SaveChanges();
//            }
//        }

//        // ============ REGISTER DRIVER ============
//        public void RegisterDriver(string fullName, string username, string password,
//                                   string phone, string licenseType)
//        {
//            using (var db = new LogisticsDbContext())
//            {
//                if (db.UserAccounts.Any(u => u.Username == username))
//                    throw new InvalidOperationException("Tên tài khoản đã tồn tại.");

//                var drv = new Driver
//                {
//                    FullName = fullName,
//                    Phone = phone,
//                    LicenseType = licenseType,
//                    IsActive = true
//                };
//                db.Drivers.Add(drv);
//                db.SaveChanges();

//                var acc = new UserAccount
//                {
//                    Username = username,
//                    PasswordHash = password,   // TODO: đổi sang hash
//                    Role = UserRole.Driver,
//                    DriverId = drv.Id,
//                    IsActive = true
//                };
//                db.UserAccounts.Add(acc);
//                db.SaveChanges();
//            }
//        }
//    }
//}









using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Data.Entity; // Keep this if you use other EF features here
using System.Linq;

namespace LMS.BUS.Services
{
    // Lý do đăng nhập thất bại
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
        // ============ LOGIN (Đã kiểm tra IsActive) ============
        public LoginResult Login(string username, string password)
        {
            // Kiểm tra đầu vào cơ bản
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return new LoginResult { Ok = false, Reason = LoginFailReason.WrongPassword };

            using (var db = new LogisticsDbContext())
            {
                // Tìm tài khoản, kèm theo Customer/Driver nếu có
                var acc = db.UserAccounts
                            .Include(u => u.Customer)
                            .Include(u => u.Driver)
                            .SingleOrDefault(u => u.Username == username);

                // Trường hợp 1: Không tìm thấy tài khoản
                if (acc == null)
                    return new LoginResult { Ok = false, Reason = LoginFailReason.UserNotFound };

                // Trường hợp 2: Tài khoản bị khóa (IsActive == false)
                if (!acc.IsActive) // Dòng kiểm tra quan trọng!
                    return new LoginResult { Ok = false, Reason = LoginFailReason.Locked };

                // Trường hợp 3: Sai mật khẩu (Chỉ kiểm tra nếu tài khoản Active)
                // !!! LƯU Ý: Đây là so sánh mật khẩu dạng plain text, cần thay bằng hash !!!
                if (acc.PasswordHash != password)
                    return new LoginResult { Ok = false, Reason = LoginFailReason.WrongPassword };

                // Trường hợp 4: Đăng nhập thành công
                // (Tùy chọn: Cập nhật LastLoginAt)
                // acc.LastLoginAt = DateTime.Now;
                // db.SaveChanges();

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
                    // Thêm IsActive = true nếu bạn đã thêm cột này vào Customer
                    // IsActive = true
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
                    CreatedAt = DateTime.Now
                };
                db.UserAccounts.Add(acc);
                db.SaveChanges();
            }
        }

        // ============ REGISTER DRIVER ============
        public void RegisterDriver(string fullName, string username, string password,
                                   string phone, string licenseType, string citizenId)
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
                    IsActive = true // Driver mới mặc định Active
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