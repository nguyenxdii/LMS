using LMS.DAL;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS.BUS.Services
{
    public class AccountUsageReport
    {
        public int UserId { get; set; }
        public UserRole Role { get; set; }
        // cho customer
        public int OrdersPending { get; set; }
        public int OrdersActive { get; set; }
        public int OrdersCancelled { get; set; }
        public int OrdersCompleted { get; set; }
        // cho driver
        public int ShipPending { get; set; }
        public int ShipActive { get; set; }
        public int ShipCancelled { get; set; }
        public int ShipCompleted { get; set; }
    }

    public class UserAccountService_Admin
    {
        // lấy danh sách tài khoản (có lọc)
        public List<UserAccount> GetAll(string usernameLike = null, string nameLike = null,
                                        UserRole? role = null, bool? isActive = null)
        {
            using (var db = new LogisticsDbContext())
            {
                var q = db.UserAccounts
                          .Include(u => u.Customer)
                          .Include(u => u.Driver)
                          .AsQueryable();

                if (!string.IsNullOrWhiteSpace(usernameLike))
                    q = q.Where(u => u.Username.Contains(usernameLike));
                if (!string.IsNullOrWhiteSpace(nameLike))
                    q = q.Where(u =>
                        (u.Customer != null && u.Customer.Name.Contains(nameLike)) ||
                        (u.Driver != null && u.Driver.FullName.Contains(nameLike)));
                if (role.HasValue)
                    q = q.Where(u => u.Role == role.Value);
                if (isActive.HasValue)
                    q = q.Where(u => u.IsActive == isActive.Value);

                return q.OrderBy(u => u.Role).ThenBy(u => u.Username).ToList();
            }
        }

        // cập nhật username + password
        public void UpdateBasic(int id, string newUsername, string newPassword = null)
        {
            using (var db = new LogisticsDbContext())
            {
                var u = db.UserAccounts.Find(id);
                if (u == null) throw new Exception("Không tìm thấy tài khoản.");

                if (!string.IsNullOrWhiteSpace(newUsername))
                {
                    if (db.UserAccounts.Any(x => x.Username == newUsername && x.Id != id))
                        throw new Exception("Tên tài khoản đã tồn tại.");
                    u.Username = newUsername;
                }

                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    u.PasswordHash = newPassword; // TODO: sau này hash
                    u.LastPasswordChangeAt = DateTime.Now;
                }

                db.SaveChanges();
            }
        }

        // khóa / mở tài khoản
        public void SetActive(int id, bool active)
        {
            using (var db = new LogisticsDbContext())
            {
                var u = db.UserAccounts.Find(id);
                if (u == null) throw new Exception("Không tìm thấy tài khoản.");
                u.IsActive = active;
                db.SaveChanges();
            }
        }

        // xóa tài khoản (không xóa profile)
        public void DeleteOnlyAccount(int id)
        {
            using (var db = new LogisticsDbContext())
            {
                var u = db.UserAccounts.Find(id);
                if (u == null) return;

                if (u.Role == UserRole.Admin)
                {
                    int adminCount = db.UserAccounts.Count(a => a.Role == UserRole.Admin);
                    if (adminCount <= 1) throw new Exception("Không thể xoá Admin cuối cùng.");
                }

                db.UserAccounts.Remove(u);
                db.SaveChanges();
            }
        }

        // xóa tài khoản (có kiểm tra admin)
        public void Delete(int id)
        {
            using (var db = new LogisticsDbContext())
            {
                var u = db.UserAccounts.Find(id);
                if (u == null) return;

                if (u.Role == UserRole.Admin)
                {
                    int adminCount = db.UserAccounts.Count(a => a.Role == UserRole.Admin);
                    if (adminCount <= 1) throw new Exception("Không thể xoá Admin cuối cùng.");
                }

                db.UserAccounts.Remove(u);
                db.SaveChanges();
            }
        }

        // báo cáo sử dụng tài khoản
        public AccountUsageReport InspectUsage(int userAccountId)
        {
            using (var db = new LogisticsDbContext())
            {
                var acc = db.UserAccounts
                            .Include(u => u.Customer)
                            .Include(u => u.Driver)
                            .SingleOrDefault(u => u.Id == userAccountId);

                if (acc == null) throw new Exception("Không tìm thấy tài khoản.");

                var rpt = new AccountUsageReport { UserId = acc.Id, Role = acc.Role };

                // helper: khớp từ khóa không phân biệt hoa/thường
                Func<string, string[], bool> hasAny = (name, keys) =>
                {
                    if (string.IsNullOrEmpty(name)) return false;
                    foreach (var k in keys)
                    {
                        if (!string.IsNullOrEmpty(k) &&
                            name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0)
                            return true;
                    }
                    return false;
                };

                Func<string, bool> isPendingName = n => hasAny(n, new[] { "Pending", "Assign", "Await", "Queue", "New", "Schedule" });
                Func<string, bool> isCompletedName = n => hasAny(n, new[] { "Completed", "Delivered", "Finished", "Done", "Closed" });
                Func<string, bool> isCancelledName = n => hasAny(n, new[] { "Cancel", "Rejected", "Reject", "Abort" });

                if (acc.Role == UserRole.Customer && acc.CustomerId != null)
                {
                    var statuses = db.Orders
                                     .Where(o => o.CustomerId == acc.CustomerId)
                                     .Select(o => o.Status)
                                     .ToList()
                                     .Select(s => s.ToString())
                                     .ToList();

                    rpt.OrdersPending = statuses.Count(n => isPendingName(n));
                    rpt.OrdersCompleted = statuses.Count(n => isCompletedName(n));
                    rpt.OrdersCancelled = statuses.Count(n => isCancelledName(n));

                    var total = statuses.Count;
                    rpt.OrdersActive = total - (rpt.OrdersPending + rpt.OrdersCompleted + rpt.OrdersCancelled);
                    if (rpt.OrdersActive < 0) rpt.OrdersActive = 0;
                }
                else if (acc.Role == UserRole.Driver && acc.DriverId != null)
                {
                    var statuses = db.Shipments
                                     .Where(x => x.DriverId == acc.DriverId)
                                     .Select(x => x.Status)
                                     .ToList()
                                     .Select(s => s.ToString())
                                     .ToList();

                    rpt.ShipPending = statuses.Count(n => isPendingName(n));
                    rpt.ShipCompleted = statuses.Count(n => isCompletedName(n));
                    rpt.ShipCancelled = statuses.Count(n => isCancelledName(n));

                    var total = statuses.Count;
                    rpt.ShipActive = total - (rpt.ShipPending + rpt.ShipCompleted + rpt.ShipCancelled);
                    if (rpt.ShipActive < 0) rpt.ShipActive = 0;
                }

                return rpt;
            }
        }
    }
}