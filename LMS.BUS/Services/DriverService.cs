using LMS.BUS.Dtos;
using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;

namespace LMS.BUS.Services
{
    public class DriverService
    {
        // 1. dùng cho ucDriver_Admin (grid chính)
        public List<Driver> GetDriversForAdmin()
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Drivers
                         .Include(d => d.Vehicle)
                         .OrderBy(d => d.FullName)
                         .ToList();
            }
        }

        public bool CheckDriverHasShipments(int driverId)
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Shipments.Any(s => s.DriverId == driverId);
            }
        }

        public void DeleteNewDriver(int driverId)
        {
            using (var db = new LogisticsDbContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var accounts = db.UserAccounts.Where(a => a.DriverId == driverId).ToList();
                        if (accounts.Any())
                        {
                            db.UserAccounts.RemoveRange(accounts);
                        }
                        var driver = db.Drivers.Find(driverId);
                        if (driver != null)
                        {
                            db.Drivers.Remove(driver);
                        }
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // 3. dùng cho ucDriverDetail_Admin (view detail)
        public DriverDetailDto GetDriverDetails(int driverId)
        {
            using (var db = new LogisticsDbContext())
            {
                var dto = new DriverDetailDto();
                dto.Driver = db.Drivers
                               .Include(d => d.Vehicle) // load kèm xe
                               .FirstOrDefault(d => d.Id == driverId);
                if (dto.Driver == null)
                    throw new Exception($"không tìm thấy tài xế ID={driverId}.");
                dto.Account = db.UserAccounts
                                .FirstOrDefault(a => a.DriverId == driverId);
                dto.Shipments = db.Shipments
                               .Where(s => s.DriverId == driverId)
                               .Include(s => s.Order)
                               .Include(s => s.FromWarehouse)
                               .Include(s => s.ToWarehouse)
                               .OrderByDescending(s => s.UpdatedAt)
                               .ToList();
                return dto;
            }
        }

        // 4. dùng cho ucDriverEditor_Admin (edit mode)
        public DriverEditorDto GetDriverForEdit(int driverId)
        {
            using (var db = new LogisticsDbContext())
            {
                var driver = db.Drivers.Find(driverId);
                if (driver == null)
                    throw new Exception($"không tìm thấy tài xế ID={driverId}.");
                var account = db.UserAccounts.FirstOrDefault(a => a.DriverId == driverId);
                return new DriverEditorDto
                {
                    Id = driver.Id,
                    FullName = driver.FullName,
                    Phone = driver.Phone,
                    CitizenId = driver.CitizenId,
                    LicenseType = driver.LicenseType,
                    Username = account?.Username
                };
            }
        }

        // 5. dùng cho ucDriverEditor_Admin (save)
        public void CreateDriverAndAccount(DriverEditorDto dto)
        {
            using (var db = new LogisticsDbContext())
            {
                if (string.IsNullOrWhiteSpace(dto.FullName))
                    throw new InvalidOperationException("vui lòng nhập họ tên.");
                if (db.UserAccounts.Any(u => u.Username == dto.Username))
                    throw new InvalidOperationException("tên tài khoản đã tồn tại.");
                if (!string.IsNullOrWhiteSpace(dto.CitizenId) && db.Drivers.Any(d => d.CitizenId == dto.CitizenId))
                    throw new InvalidOperationException("số citizen id (cccd) này đã tồn tại.");
                if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                    throw new InvalidOperationException("mật khẩu phải từ 6 ký tự trở lên.");
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var driver = new Driver
                        {
                            FullName = dto.FullName,
                            Phone = dto.Phone,
                            CitizenId = dto.CitizenId,
                            LicenseType = dto.LicenseType,
                            IsActive = true // tài xế mới luôn active
                        };
                        db.Drivers.Add(driver);
                        db.SaveChanges(); // lưu để lấy Id
                        var account = new UserAccount
                        {
                            Username = dto.Username,
                            PasswordHash = dto.Password,
                            Role = UserRole.Driver,
                            DriverId = driver.Id,
                            IsActive = true,
                            CreatedAt = DateTime.Now
                        };
                        db.UserAccounts.Add(account);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateDriverAndAccount(DriverEditorDto dto)
        {
            using (var db = new LogisticsDbContext())
            {
                var driver = db.Drivers.Find(dto.Id);
                if (driver == null)
                    throw new Exception("không tìm thấy tài xế.");
                if (string.IsNullOrWhiteSpace(dto.FullName))
                    throw new InvalidOperationException("vui lòng nhập họ tên.");
                if (!string.IsNullOrWhiteSpace(dto.CitizenId) && db.Drivers.Any(d => d.CitizenId == dto.CitizenId && d.Id != dto.Id))
                    throw new InvalidOperationException("số citizen id (cccd) này đã tồn tại.");
                driver.FullName = dto.FullName;
                driver.Phone = dto.Phone;
                driver.CitizenId = dto.CitizenId;
                driver.LicenseType = dto.LicenseType;
                var account = db.UserAccounts.FirstOrDefault(a => a.DriverId == dto.Id);
                if (account != null)
                {
                    if (account.Username != dto.Username)
                    {
                        if (db.UserAccounts.Any(u => u.Username == dto.Username && u.Id != account.Id))
                            throw new InvalidOperationException("tên tài khoản đã tồn tại.");
                        account.Username = dto.Username;
                    }
                    if (!string.IsNullOrWhiteSpace(dto.Password))
                    {
                        if (dto.Password.Length < 6)
                            throw new InvalidOperationException("mật khẩu mới phải từ 6 ký tự.");
                        account.PasswordHash = dto.Password;
                        account.LastPasswordChangeAt = DateTime.Now;
                    }
                }
                db.SaveChanges();
            }
        }

        // dùng cho ucDriverSearch_Admin
        public List<Driver> SearchDriversForAdmin(string nameLike, string phoneLike, string citizenIdLike, string licenseType)
        {
            using (var db = new LogisticsDbContext())
            {
                var query = db.Drivers.AsQueryable();
                if (!string.IsNullOrWhiteSpace(nameLike))
                {
                    query = query.Where(d => d.FullName.Contains(nameLike));
                }
                if (!string.IsNullOrWhiteSpace(phoneLike))
                {
                    query = query.Where(d => d.Phone.Contains(phoneLike));
                }
                if (!string.IsNullOrWhiteSpace(citizenIdLike))
                {
                    query = query.Where(d => d.CitizenId.Contains(citizenIdLike));
                }
                if (!string.IsNullOrWhiteSpace(licenseType))
                {
                    query = query.Where(d => d.LicenseType == licenseType);
                }
                return query.OrderBy(d => d.FullName).ToList();
            }
        }

        // driver không xe (dùng cho gán xe)
        public List<Driver> GetDriversWithoutVehicles()
        {
            using (var db = new LogisticsDbContext())
            {
                var active = new[] { ShipmentStatus.Assigned, ShipmentStatus.OnRoute, ShipmentStatus.AtWarehouse, ShipmentStatus.ArrivedDestination };
                var busyIds = db.Shipments
                                .Where(s => s.DriverId.HasValue && active.Contains(s.Status))
                                .Select(s => s.DriverId.Value)
                                .Distinct()
                                .ToList();
                return db.Drivers
                         .Where(d => d.IsActive
                                  && d.Vehicle == null // chưa có xe
                                  && !busyIds.Contains(d.Id)) // không bận
                         .OrderBy(d => d.FullName)
                         .ToList();
            }
        }

        // driver có xe để tạo shipment
        public List<Driver> GetDriversWithVehiclesForShipment()
        {
            using (var db = new LogisticsDbContext())
            {
                var active = new[] {
                    ShipmentStatus.Pending,
                    ShipmentStatus.Assigned,
                    ShipmentStatus.OnRoute,
                    ShipmentStatus.AtWarehouse,
                    ShipmentStatus.ArrivedDestination };
                var busyIds = db.Shipments
                                .Where(s => s.DriverId.HasValue && active.Contains(s.Status))
                                .Select(s => s.DriverId.Value)
                                .Distinct()
                                .ToList();
                return db.Drivers
                         .Include(d => d.Vehicle) // load thông tin xe
                         .Where(d => d.IsActive
                                  && d.Vehicle != null // đã có xe
                                  && !busyIds.Contains(d.Id)) // không bận
                         .OrderBy(d => d.FullName)
                         .ToList();
            }
        }

        // 1. lấy thông tin chi tiết bằng accountId
        public DriverDetailDto GetDriverDetailsByAccountId(int accountId)
        {
            using (var db = new LogisticsDbContext())
            {
                var account = db.UserAccounts
                                .Include(a => a.Driver)
                                .FirstOrDefault(a => a.Id == accountId);
                if (account == null || account.Driver == null)
                    throw new Exception("không tìm thấy tài khoản hoặc hồ sơ tài xế.");
                return new DriverDetailDto
                {
                    Driver = account.Driver,
                    Account = account
                };
            }
        }

        // 2. cập nhật thông tin cá nhân
        public void UpdateDriverProfile(DriverProfileDto dto)
        {
            using (var db = new LogisticsDbContext())
            {
                var driver = db.Drivers.Find(dto.DriverId);
                if (driver == null)
                    throw new Exception("không tìm thấy tài xế.");
                if (string.IsNullOrWhiteSpace(dto.FullName))
                    throw new InvalidOperationException("họ tên không được rỗng.");
                if (!string.IsNullOrWhiteSpace(dto.Phone) && !Regex.IsMatch(dto.Phone, @"^\d{9,15}$"))
                    throw new InvalidOperationException("sđt không hợp lệ.");
                if (!string.IsNullOrWhiteSpace(dto.CitizenId) && !Regex.IsMatch(dto.CitizenId, @"^\d{12}$"))
                    throw new InvalidOperationException("số cccd phải gồm 12 chữ số.");
                if (!string.IsNullOrWhiteSpace(dto.CitizenId) && db.Drivers.Any(d => d.CitizenId == dto.CitizenId && d.Id != dto.DriverId))
                    throw new InvalidOperationException("số cccd này đã tồn tại.");
                if (string.IsNullOrWhiteSpace(dto.LicenseType))
                    throw new InvalidOperationException("vui lòng chọn loại gplx.");
                driver.FullName = dto.FullName;
                driver.Phone = dto.Phone;
                driver.CitizenId = dto.CitizenId;
                driver.LicenseType = dto.LicenseType;
                db.SaveChanges();
            }
        }

        // 3. cập nhật ảnh đại diện
        public void UpdateDriverAvatar(int driverId, byte[] avatarData)
        {
            using (var db = new LogisticsDbContext())
            {
                var driver = db.Drivers.Find(driverId);
                if (driver == null)
                    throw new Exception("không tìm thấy tài xế.");
                if (avatarData == null || avatarData.Length == 0)
                    throw new InvalidOperationException("dữ liệu ảnh không hợp lệ.");
                driver.AvatarData = avatarData;
                db.SaveChanges();
            }
        }

        // 4. đổi mật khẩu
        public void ChangeDriverPassword(int accountId, string oldPassword, string newPassword)
        {
            using (var db = new LogisticsDbContext())
            {
                var account = db.UserAccounts.Find(accountId);
                if (account == null)
                    throw new Exception("không tìm thấy tài khoản.");
                if (account.PasswordHash != oldPassword)
                    throw new InvalidOperationException("mật khẩu cũ không chính xác.");
                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                    throw new InvalidOperationException("mật khẩu mới phải từ 6 ký tự trở lên.");
                if (newPassword == oldPassword)
                    throw new InvalidOperationException("mật khẩu mới phải khác mật khẩu cũ.");
                account.PasswordHash = newPassword;
                account.LastPasswordChangeAt = DateTime.Now;
                db.SaveChanges();
            }
        }
    }
}