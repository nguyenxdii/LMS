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
        // === 1. Dùng cho ucCustomer_Admin (Grid chính) ===
        public List<Customer> GetCustomersForAdmin()
        {
            using (var db = new LogisticsDbContext())
            {
                return db.Customers.OrderBy(c => c.Name).ToList();
            }
        }

        // === 2. Dùng cho ucCustomer_Admin (Nút Xóa) ===
        public bool CheckCustomerHasOrders(int customerId)
        {
            using (var db = new LogisticsDbContext())
            {
                // Kiểm tra xem có BẤT KỲ đơn hàng nào (kể cả cũ/hủy)
                return db.Orders.Any(o => o.CustomerId == customerId);
            }
        }

        public void DeleteNewCustomer(int customerId)
        {
            using (var db = new LogisticsDbContext())
            {
                // Phải xóa trong transaction để đảm bảo an toàn
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Xóa các UserAccount liên kết
                        var accounts = db.UserAccounts.Where(a => a.CustomerId == customerId).ToList();
                        if (accounts.Any())
                        {
                            db.UserAccounts.RemoveRange(accounts);
                        }

                        // 2. Xóa Customer
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
                        throw; // Ném lỗi ra ngoài để UC bắt
                    }
                }
            }
        }

        // === 3. Dùng cho ucCustomerDetail_Admin (View Detail "A") ===
        public CustomerDetailDto GetCustomerDetails(int customerId)
        {
            using (var db = new LogisticsDbContext())
            {
                var dto = new CustomerDetailDto();

                dto.Customer = db.Customers.Find(customerId);
                if (dto.Customer == null)
                    throw new Exception($"Không tìm thấy khách hàng ID={customerId}.");

                // Lấy tài khoản ĐẦU TIÊN (hoặc chính) của khách hàng
                dto.Account = db.UserAccounts
                                .FirstOrDefault(a => a.CustomerId == customerId);

                // Lấy lịch sử đơn hàng
                dto.Orders = db.Orders
                               .Where(o => o.CustomerId == customerId)
                               .Include(o => o.OriginWarehouse) // Load kèm kho
                               .Include(o => o.DestWarehouse)  // Load kèm kho
                               .OrderByDescending(o => o.CreatedAt)
                               .ToList();
                return dto;
            }
        }

        // === 4. Dùng cho ucCustomerEditor_Admin (Edit Mode "B") ===
        public CustomerEditorDto GetCustomerForEdit(int customerId)
        {
            using (var db = new LogisticsDbContext())
            {
                var customer = db.Customers.Find(customerId);
                if (customer == null)
                    throw new Exception($"Không tìm thấy khách hàng ID={customerId}.");

                var account = db.UserAccounts.FirstOrDefault(a => a.CustomerId == customerId);

                return new CustomerEditorDto
                {
                    Id = customer.Id,
                    FullName = customer.Name,
                    Phone = customer.Phone,
                    Email = customer.Email,
                    Address = customer.Address,
                    Username = account?.Username // Có thể null nếu khách hàng này bị tạo lỗi (chưa có TK)
                };
            }
        }

        // === 5. Dùng cho ucCustomerEditor_Admin (Save "B") ===
        public void CreateCustomerAndAccount(CustomerEditorDto dto)
        {
            using (var db = new LogisticsDbContext())
            {
                // Validation (Service-side)
                if (string.IsNullOrWhiteSpace(dto.FullName))
                    throw new InvalidOperationException("Vui lòng nhập họ tên.");
                if (db.UserAccounts.Any(u => u.Username == dto.Username))
                    throw new InvalidOperationException("Tên tài khoản đã tồn tại.");
                if (!Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new InvalidOperationException("Email không hợp lệ.");
                if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                    throw new InvalidOperationException("Mật khẩu phải từ 6 ký tự trở lên.");

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
                            // IsActive = true // Nếu sau này bạn dùng logic khóa/mở
                        };
                        db.Customers.Add(customer);
                        db.SaveChanges(); // Lưu để lấy CustomerId

                        var account = new UserAccount
                        {
                            Username = dto.Username,
                            PasswordHash = dto.Password, // TODO: Băm mật khẩu (hash + salt)
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
                    throw new Exception("Không tìm thấy khách hàng.");

                if (string.IsNullOrWhiteSpace(dto.FullName))
                    throw new InvalidOperationException("Vui lòng nhập họ tên.");
                if (!Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new InvalidOperationException("Email không hợp lệ.");

                // Cập nhật thông tin Customer
                customer.Name = dto.FullName;
                customer.Phone = dto.Phone;
                customer.Email = dto.Email;
                customer.Address = dto.Address;

                // Tìm tài khoản chính (hoặc tài khoản đầu tiên)
                var account = db.UserAccounts.FirstOrDefault(a => a.CustomerId == dto.Id);

                // Nếu khách hàng có tài khoản
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
                // (Nếu khách hàng chưa có tài khoản, bạn có thể cân nhắc tạo mới ở đây nếu muốn)

                db.SaveChanges();
            }
        }

        public List<Customer> SearchCustomersForAdmin(string nameLike, string phoneLike, string emailLike, string addressLike)
        {
            using (var db = new LogisticsDbContext())
            {
                var query = db.Customers.AsQueryable(); // Bắt đầu từ bảng Customers

                // Áp dụng các bộ lọc nếu có giá trị
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
                    // Có thể dùng StartsWith, EndsWith hoặc Contains tùy nhu cầu
                    query = query.Where(c => c.Email.Contains(emailLike));
                }
                if (!string.IsNullOrWhiteSpace(addressLike))
                {
                    query = query.Where(c => c.Address.Contains(addressLike));
                }

                // Sắp xếp và trả về List<Customer>
                return query.OrderBy(c => c.Name).ToList();
            }
        }
    }
}