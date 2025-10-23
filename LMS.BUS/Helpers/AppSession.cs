using Guna.UI2.WinForms;
using LMS.DAL.Models;
using System;

namespace LMS.BUS.Helpers
{
    /// <summary>
    /// Lưu thông tin tài khoản đăng nhập hiện tại (dùng chung giữa các Form).
    /// </summary>
    public static class AppSession
    {
        /// <summary>
        /// Tài khoản hiện tại sau khi đăng nhập thành công.
        /// </summary>
        public static UserAccount CurrentAccount { get; private set; }

        /// <summary>
        /// Cho biết người dùng đã đăng nhập chưa.
        /// </summary>
        public static bool IsLoggedIn => CurrentAccount != null;

        /// <summary>
        /// Vai trò của người đăng nhập (Admin / Customer / Driver).
        /// </summary>
        public static UserRole? Role => CurrentAccount?.Role;

        /// <summary>
        /// ID khách hàng (nếu là Customer).
        /// </summary>
        public static int? CustomerId => CurrentAccount?.CustomerId;

        /// <summary>
        /// ID tài xế (nếu là Driver).
        /// </summary>
        public static int? DriverId => CurrentAccount?.DriverId;

        /// <summary>
        /// Trả về thực thể Customer nếu tài khoản là Customer.
        /// </summary>
        public static Customer CurrentCustomer => CurrentAccount?.Customer;

        /// <summary>
        /// Trả về thực thể Driver nếu tài khoản là Driver.
        /// </summary>
        public static Driver CurrentDriver => CurrentAccount?.Driver;

        /// <summary>
        /// Tên hiển thị tùy theo vai trò.
        /// </summary>
        public static string DisplayName
        {
            get
            {
                if (CurrentAccount == null)
                    return string.Empty;

                switch (CurrentAccount.Role)
                {
                    case UserRole.Customer:
                        return CurrentAccount.Customer?.Name ?? CurrentAccount.Username;
                    case UserRole.Driver:
                        return CurrentAccount.Driver?.FullName ?? CurrentAccount.Username;
                    default:
                        return CurrentAccount.Username;
                }
            }
        }

        /// <summary>
        /// Đăng nhập và lưu tài khoản hiện tại.
        /// </summary>
        public static void SignIn(UserAccount acc)
        {
            CurrentAccount = acc;
        }

        /// <summary>
        /// Đăng xuất khỏi phiên làm việc.
        /// </summary>
        public static void SignOut()
        {
            CurrentAccount = null;
        }

        /// <summary>
        /// Helper class cho ComboBox vùng miền (nếu cần dùng ở GUI).
        /// </summary>
        private class ZoneItem
        {
            public Zone Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }
    }
}
