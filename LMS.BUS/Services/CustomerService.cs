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
    public class CustomerService
    {
        public List<Customer> GetCustomersForAdmin()
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Customers.OrderBy(c => c.Name).ToList();
            }
        }

        public bool CheckCustomerHasOrders(int customerId)
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Orders.Any(o => o.CustomerId == customerId);
            }
        }

        public void DeleteNewCustomer(int customerId)
        {
            using (var db = new LogisticsDbContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var accounts = db.UserAccounts.Where(a => a.CustomerId == customerId).ToList();
                        if (accounts.Any())
                        {
                            db.UserAccounts.RemoveRange(accounts);
                        }
                        var customer = db.Customers.Find(customerId);
                        if (customer != null)
                        {
                            db.Customers.Remove(customer);
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

        public CustomerDetailDto GetCustomerDetails(int customerId)
        {
            using (var db = new LogisticsDbContext())
            {
                var dto = new CustomerDetailDto();
                dto.Customer = db.Customers.Find(customerId);
                if (dto.Customer == null)
                    throw new Exception($"không tìm thấy khách hàng ID={customerId}.");
                dto.Account = db.UserAccounts
                                .FirstOrDefault(a => a.CustomerId == customerId);
                dto.Orders = db.Orders
                               .Where(o => o.CustomerId == customerId)
                               .Include(o => o.OriginWarehouse) // load kèm kho
                               .Include(o => o.DestWarehouse) // load kèm kho
                               .OrderByDescending(o => o.CreatedAt)
                               .ToList();
                return dto;
            }
        }

        public CustomerEditorDto GetCustomerForEdit(int customerId)
        {
            using (var db = new LogisticsDbContext())
            {
                var customer = db.Customers.Find(customerId);
                if (customer == null)
                    throw new Exception($"không tìm thấy khách hàng ID={customerId}.");
                var account = db.UserAccounts.FirstOrDefault(a => a.CustomerId == customerId);
                return new CustomerEditorDto
                {
                    Id = customer.Id,
                    FullName = customer.Name,
                    Phone = customer.Phone,
                    Email = customer.Email,
                    Address = customer.Address,
                    Username = account?.Username // có thể null nếu khách hàng chưa có tài khoản
                };
            }
        }

        public void CreateCustomerAndAccount(CustomerEditorDto dto)
        {
            using (var db = new LogisticsDbContext())
            {
                if (string.IsNullOrWhiteSpace(dto.FullName))
                    throw new InvalidOperationException("vui lòng nhập họ tên.");
                if (db.UserAccounts.Any(u => u.Username == dto.Username))
                    throw new InvalidOperationException("tên tài khoản đã tồn tại.");
                if (string.IsNullOrWhiteSpace(dto.Email) || !Regex.IsMatch(dto.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                    throw new InvalidOperationException("email không hợp lệ (chỉ dùng ký tự la-tinh, không dấu).");
                if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                    throw new InvalidOperationException("mật khẩu phải từ 6 ký tự trở lên.");
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var customer = new Customer
                        {
                            Name = dto.FullName,
                            Phone = dto.Phone,
                            Email = dto.Email,
                            Address = dto.Address,
                            AvatarData = dto.AvatarData,
                        };
                        db.Customers.Add(customer);
                        db.SaveChanges(); // lưu để lấy Id
                        var account = new UserAccount
                        {
                            Username = dto.Username,
                            PasswordHash = dto.Password, // todo: băm mật khẩu (hash + salt)
                            Role = UserRole.Customer,
                            CustomerId = customer.Id,
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

        public void UpdateCustomerAndAccount(CustomerEditorDto dto)
        {
            using (var db = new LogisticsDbContext())
            {
                var customer = db.Customers.Find(dto.Id);
                if (customer == null)
                    throw new Exception("không tìm thấy khách hàng.");
                if (string.IsNullOrWhiteSpace(dto.FullName))
                    throw new InvalidOperationException("vui lòng nhập họ tên.");
                if (string.IsNullOrWhiteSpace(dto.Email) || !Regex.IsMatch(dto.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                    throw new InvalidOperationException("email không hợp lệ.");
                customer.Name = dto.FullName;
                customer.Phone = dto.Phone;
                customer.Email = dto.Email;
                customer.Address = dto.Address;
                var account = db.UserAccounts.FirstOrDefault(a => a.CustomerId == dto.Id);
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
                        account.PasswordHash = dto.Password; // todo: băm mật khẩu
                        account.LastPasswordChangeAt = DateTime.Now;
                    }
                }
                if (dto.AvatarData != null && dto.AvatarData.Length > 0)
                {
                    customer.AvatarData = dto.AvatarData;
                }
                db.SaveChanges();
            }
        }

        public List<Customer> SearchCustomersForAdmin(string nameLike, string phoneLike, string emailLike, string addressLike)
        {
            using (var db = new LogisticsDbContext())
            {
                var query = db.Customers.AsQueryable(); // bắt đầu từ bảng customers
                if (!string.IsNullOrWhiteSpace(nameLike))
                {
                    query = query.Where(c => c.Name.Contains(nameLike));
                }
                if (!string.IsNullOrWhiteSpace(phoneLike))
                {
                    query = query.Where(c => c.Phone.Contains(phoneLike));
                }
                if (!string.IsNullOrWhiteSpace(emailLike))
                {
                    query = query.Where(c => c.Email.Contains(emailLike));
                }
                if (!string.IsNullOrWhiteSpace(addressLike))
                {
                    query = query.Where(c => c.Address.Contains(addressLike));
                }
                return query.OrderBy(c => c.Name).ToList();
            }
        }

        // profile customer
        // 1. lấy thông tin chi tiết (customer + account)
        public CustomerDetailDto GetCustomerDetailsByAccountId(int accountId)
        {
            using (var db = new LogisticsDbContext())
            {
                var account = db.UserAccounts
                                .Include(a => a.Customer) // load kèm customer
                                .FirstOrDefault(a => a.Id == accountId);
                if (account == null || account.Customer == null)
                    throw new Exception("không tìm thấy tài khoản hoặc hồ sơ khách hàng.");
                return new CustomerDetailDto
                {
                    Customer = account.Customer,
                    Account = account
                };
            }
        }

        // 2. cập nhật thông tin cá nhân (tên, sđt, email, địa chỉ)
        public void UpdateCustomerProfile(CustomerProfileDto dto)
        {
            using (var db = new LogisticsDbContext())
            {
                var customer = db.Customers.Find(dto.CustomerId);
                if (customer == null)
                    throw new Exception("không tìm thấy khách hàng.");
                if (string.IsNullOrWhiteSpace(dto.FullName))
                    throw new InvalidOperationException("họ tên không được rỗng.");
                if (!string.IsNullOrWhiteSpace(dto.Email) && !Regex.IsMatch(dto.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                    throw new InvalidOperationException("email không hợp lệ (không dấu).");
                if (!string.IsNullOrWhiteSpace(dto.Phone) && !Regex.IsMatch(dto.Phone, @"^\d{9,15}$"))
                    throw new InvalidOperationException("sđt không hợp lệ.");
                customer.Name = dto.FullName;
                customer.Phone = dto.Phone;
                customer.Email = dto.Email;
                customer.Address = dto.Address;
                db.SaveChanges();
            }
        }

        // 3. cập nhật ảnh đại diện
        public void UpdateCustomerAvatar(int customerId, byte[] avatarData)
        {
            using (var db = new LogisticsDbContext())
            {
                var customer = db.Customers.Find(customerId);
                if (customer == null)
                    throw new Exception("không tìm thấy khách hàng.");
                if (avatarData == null || avatarData.Length == 0)
                    throw new InvalidOperationException("dữ liệu ảnh không hợp lệ.");
                customer.AvatarData = avatarData;
                db.SaveChanges();
            }
        }

        // 4. đổi mật khẩu
        public void ChangeCustomerPassword(int accountId, string oldPassword, string newPassword)
        {
            using (var db = new LogisticsDbContext())
            {
                var account = db.UserAccounts.Find(accountId);
                if (account == null)
                    throw new Exception("không tìm thấy tài khoản.");
                // todo: thay thế logic kiểm tra mật khẩu bằng hashing thật
                if (account.PasswordHash != oldPassword)
                    throw new InvalidOperationException("mật khẩu cũ không chính xác.");
                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                    throw new InvalidOperationException("mật khẩu mới phải từ 6 ký tự trở lên.");
                // todo: hash mật khẩu mới trước khi lưu
                account.PasswordHash = newPassword;
                account.LastPasswordChangeAt = DateTime.Now;
                db.SaveChanges();
            }
        }
    }
}