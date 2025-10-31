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
        // === 1. Dùng cho ucDriver_Admin (Grid chính) ===
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
                // Kiểm tra xem có BẤT KỲ chuyến hàng nào (kể cả cũ/hủy)
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
                        // 1. Xóa các UserAccount liên kết
                        var accounts = db.UserAccounts.Where(a => a.DriverId == driverId).ToList();
                        if (accounts.Any())
                        {
                            db.UserAccounts.RemoveRange(accounts);
                        }

                        // 2. Xóa Driver
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
                        throw; // Ném lỗi ra ngoài để UC bắt
                    }
                }
            }
        }

        // === 3. Dùng cho ucDriverDetail_Admin (View Detail "A") ===
        //public DriverDetailDto GetDriverDetails(int driverId)
        //{
        //    using (var db = new LogisticsDbContext())
        //    {
        //        var dto = new DriverDetailDto();

        //        dto.Driver = db.Drivers.Find(driverId);
        //        if (dto.Driver == null)
        //            throw new Exception($"Không tìm thấy tài xế ID={driverId}.");

        //        dto.Account = db.UserAccounts
        //                        .FirstOrDefault(a => a.DriverId == driverId);

        //        // Lấy lịch sử chuyến hàng
        //        dto.Shipments = db.Shipments
        //                       .Where(s => s.DriverId == driverId)
        //                       .Include(s => s.Order) // Load kèm đơn hàng
        //                       .Include(s => s.FromWarehouse) // Load kèm kho đi
        //                       .Include(s => s.ToWarehouse)  // Load kèm kho đến
        //                       .OrderByDescending(s => s.UpdatedAt)
        //                       .ToList();
        //        return dto;
        //    }
        //}
        public DriverDetailDto GetDriverDetails(int driverId)
        {
            using (var db = new LogisticsDbContext())
            {
                var dto = new DriverDetailDto();

                // --- SỬA DÒNG NÀY ---
                // dto.Driver = db.Drivers.Find(driverId); // Dòng cũ
                dto.Driver = db.Drivers
                               .Include(d => d.Vehicle) // <-- THÊM INCLUDE ĐỂ TẢI KÈM XE
                               .FirstOrDefault(d => d.Id == driverId); // Dùng FirstOrDefault thay Find
                                                                       // --- KẾT THÚC SỬA ---

                if (dto.Driver == null)
                    throw new Exception($"Không tìm thấy tài xế ID={driverId}.");

                // Giữ nguyên phần tải Account và Shipments
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

        // === 4. Dùng cho ucDriverEditor_Admin (Edit Mode "B") ===
        public DriverEditorDto GetDriverForEdit(int driverId)
        {
            using (var db = new LogisticsDbContext())
            {
                var driver = db.Drivers.Find(driverId);
                if (driver == null)
                    throw new Exception($"Không tìm thấy tài xế ID={driverId}.");

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

        // === 5. Dùng cho ucDriverEditor_Admin (Save "B") ===
        public void CreateDriverAndAccount(DriverEditorDto dto)
        {
            using (var db = new LogisticsDbContext())
            {
                // Validation (Service-side)
                if (string.IsNullOrWhiteSpace(dto.FullName))
                    throw new InvalidOperationException("Vui lòng nhập họ tên.");
                if (db.UserAccounts.Any(u => u.Username == dto.Username))
                    throw new InvalidOperationException("Tên tài khoản đã tồn tại.");
                // Thêm kiểm tra CitizenId nếu nó là bắt buộc và duy nhất
                if (!string.IsNullOrWhiteSpace(dto.CitizenId) && db.Drivers.Any(d => d.CitizenId == dto.CitizenId))
                    throw new InvalidOperationException("Số Citizen ID (CCCD) này đã tồn tại.");
                if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                    throw new InvalidOperationException("Mật khẩu phải từ 6 ký tự trở lên.");

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
                            IsActive = true // Tài xế mới luôn Active
                        };
                        db.Drivers.Add(driver);
                        db.SaveChanges(); // Lưu để lấy DriverId

                        var account = new UserAccount
                        {
                            Username = dto.Username,
                            PasswordHash = dto.Password, // TODO: Băm mật khẩu (hash + salt)
                            Role = UserRole.Driver, // Vai trò là Driver
                            DriverId = driver.Id,   // Liên kết với DriverId
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
                    throw new Exception("Không tìm thấy tài xế.");

                if (string.IsNullOrWhiteSpace(dto.FullName))
                    throw new InvalidOperationException("Vui lòng nhập họ tên.");

                // Kiểm tra CitizenId trùng (trừ chính mình)
                if (!string.IsNullOrWhiteSpace(dto.CitizenId) && db.Drivers.Any(d => d.CitizenId == dto.CitizenId && d.Id != dto.Id))
                    throw new InvalidOperationException("Số Citizen ID (CCCD) này đã tồn tại.");

                // Cập nhật thông tin Driver
                driver.FullName = dto.FullName;
                driver.Phone = dto.Phone;
                driver.CitizenId = dto.CitizenId;
                driver.LicenseType = dto.LicenseType;

                // Tìm tài khoản chính
                var account = db.UserAccounts.FirstOrDefault(a => a.DriverId == dto.Id);

                if (account != null)
                {
                    // Cập nhật Username (nếu thay đổi)
                    if (account.Username != dto.Username)
                    {
                        if (db.UserAccounts.Any(u => u.Username == dto.Username && u.Id != account.Id))
                            throw new InvalidOperationException("Tên tài khoản đã tồn tại.");
                        account.Username = dto.Username;
                    }

                    // Cập nhật mật khẩu (nếu được cung cấp)
                    if (!string.IsNullOrWhiteSpace(dto.Password))
                    {
                        if (dto.Password.Length < 6)
                            throw new InvalidOperationException("Mật khẩu mới phải từ 6 ký tự.");

                        account.PasswordHash = dto.Password; // TODO: Băm mật khẩu
                        account.LastPasswordChangeAt = DateTime.Now;
                    }
                }

                db.SaveChanges();
            }
        }

        // === Dùng cho ucDriverSearch_Admin ===
        public List<Driver> SearchDriversForAdmin(string nameLike, string phoneLike, string citizenIdLike, string licenseType)
        {
            using (var db = new LogisticsDbContext())
            {
                var query = db.Drivers.AsQueryable(); // Bắt đầu truy vấn từ bảng Drivers

                // Áp dụng các bộ lọc nếu có giá trị
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
                    // So sánh chính xác hạng bằng lái
                    query = query.Where(d => d.LicenseType == licenseType);
                }

                // Sắp xếp kết quả và trả về List<Driver>
                return query.OrderBy(d => d.FullName).ToList();
            }
        }

        // === Dùng cho ucDriverPicker_Admin (Lấy tài xế rảnh) ===
        //public List<Driver> GetAvailableDriversForAdmin()
        //{
        //    using (var db = new LogisticsDbContext())
        //    {
        //        var activeShipmentStatuses = new[] {
        //            ShipmentStatus.Assigned, // Đã nhận
        //            ShipmentStatus.OnRoute, // Đang đi đường
        //            ShipmentStatus.AtWarehouse, // Đang ở kho
        //            ShipmentStatus.ArrivedDestination // Đã tới đích (nhưng chưa Complete)
        //        };

        //        var busyDriverIds = db.Shipments
        //                              .Where(s => s.DriverId.HasValue // Kiểm tra DriverId có giá trị không (là nullable int?)
        //                                       && activeShipmentStatuses.Contains(s.Status))
        //                              .Select(s => s.DriverId.Value) // Lấy giá trị int từ int?
        //                              .Distinct()
        //                              .ToList();

        //        var availableDrivers = db.Drivers
        //                                 .Where(d => d.IsActive && !busyDriverIds.Contains(d.Id))
        //                                 .OrderBy(d => d.FullName)
        //                                 .ToList();

        //        return availableDrivers;
        //    }
        //}

        //=================

        // driver không xe
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
                                  && d.Vehicle == null     // CHƯA có xe
                                  && !busyIds.Contains(d.Id))// KHÔNG bận
                         .OrderBy(d => d.FullName)
                         .ToList();
            }
        }

        // driver có xe để tạo shipment
        public List<Driver> GetDriversWithVehiclesForShipment() // <-- Hàm MỚI
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
                         .Include(d => d.Vehicle) // Cần Include để lấy thông tin xe nếu muốn hiển thị
                         .Where(d => d.IsActive
                                  && d.Vehicle != null      // <<< ĐÃ CÓ xe
                                  && !busyIds.Contains(d.Id)) // KHÔNG bận
                         .OrderBy(d => d.FullName)
                         .ToList();
            }
        }
        // 1. Lấy thông tin chi tiết (Driver + Account) bằng AccountId
        public DriverDetailDto GetDriverDetailsByAccountId(int accountId)
        {
            using (var db = new LogisticsDbContext())
            {
                var account = db.UserAccounts
                                .Include(a => a.Driver) // Load kèm Driver
                                .FirstOrDefault(a => a.Id == accountId);

                if (account == null || account.Driver == null)
                    throw new Exception("Không tìm thấy tài khoản hoặc hồ sơ tài xế.");

                // Dùng lại DTO bạn đã có
                return new DriverDetailDto
                {
                    Driver = account.Driver,
                    Account = account
                    // Shipments không cần load ở đây trừ khi bạn muốn hiển thị
                };
            }
        }

        // 2. Cập nhật thông tin cá nhân (Tên, SĐT, CCCD, GPLX)
        public void UpdateDriverProfile(DriverProfileDto dto)
        {
            using (var db = new LogisticsDbContext())
            {
                var driver = db.Drivers.Find(dto.DriverId);
                if (driver == null)
                    throw new Exception("Không tìm thấy tài xế.");

                // Validation
                if (string.IsNullOrWhiteSpace(dto.FullName))
                    throw new InvalidOperationException("Họ tên không được rỗng.");
                if (!string.IsNullOrWhiteSpace(dto.Phone) && !Regex.IsMatch(dto.Phone, @"^\d{9,15}$"))
                    throw new InvalidOperationException("SĐT không hợp lệ.");
                if (!string.IsNullOrWhiteSpace(dto.CitizenId) && !Regex.IsMatch(dto.CitizenId, @"^\d{12}$"))
                    throw new InvalidOperationException("Số CCCD phải gồm 12 chữ số.");
                // Kiểm tra CCCD trùng (trừ chính mình)
                if (!string.IsNullOrWhiteSpace(dto.CitizenId) && db.Drivers.Any(d => d.CitizenId == dto.CitizenId && d.Id != dto.DriverId))
                    throw new InvalidOperationException("Số CCCD này đã tồn tại.");
                if (string.IsNullOrWhiteSpace(dto.LicenseType))
                    throw new InvalidOperationException("Vui lòng chọn loại GPLX.");


                // Gán giá trị mới
                driver.FullName = dto.FullName;
                driver.Phone = dto.Phone;
                driver.CitizenId = dto.CitizenId;
                driver.LicenseType = dto.LicenseType;

                db.SaveChanges();
            }
        }

        // 3. Cập nhật ảnh đại diện Driver
        public void UpdateDriverAvatar(int driverId, byte[] avatarData)
        {
            using (var db = new LogisticsDbContext())
            {
                var driver = db.Drivers.Find(driverId);
                if (driver == null)
                    throw new Exception("Không tìm thấy tài xế.");

                if (avatarData == null || avatarData.Length == 0)
                    throw new InvalidOperationException("Dữ liệu ảnh không hợp lệ.");

                driver.AvatarData = avatarData;
                db.SaveChanges();
            }
        }

        // 4. Đổi mật khẩu Driver (Logic giống hệt Customer)
        public void ChangeDriverPassword(int accountId, string oldPassword, string newPassword)
        {
            using (var db = new LogisticsDbContext())
            {
                var account = db.UserAccounts.Find(accountId);
                if (account == null)
                    throw new Exception("Không tìm thấy tài khoản.");

                // TODO: THAY THẾ logic kiểm tra mật khẩu này bằng HASHING THẬT
                if (account.PasswordHash != oldPassword)
                    throw new InvalidOperationException("Mật khẩu cũ không chính xác.");

                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                    throw new InvalidOperationException("Mật khẩu mới phải từ 6 ký tự trở lên.");

                // Kiểm tra trùng MK cũ
                if (newPassword == oldPassword)
                    throw new InvalidOperationException("Mật khẩu mới phải khác mật khẩu cũ.");

                // TODO: HASH mật khẩu mới trước khi lưu
                account.PasswordHash = newPassword;
                account.LastPasswordChangeAt = DateTime.Now;

                db.SaveChanges();
            }
        }
    }
}