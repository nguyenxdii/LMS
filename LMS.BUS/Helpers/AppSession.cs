using Guna.UI2.WinForms;
using LMS.DAL.Models;
using System;

namespace LMS.BUS.Helpers
{
    public static class AppSession
    {
        public static UserAccount CurrentAccount { get; private set; }
        public static bool IsLoggedIn => CurrentAccount != null;
        public static UserRole? Role => CurrentAccount?.Role;
        public static int? CustomerId => CurrentAccount?.CustomerId;
        public static int? DriverId => CurrentAccount?.DriverId;
        public static Customer CurrentCustomer => CurrentAccount?.Customer;
        public static Driver CurrentDriver => CurrentAccount?.Driver;
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
        public static void SignIn(UserAccount acc)
        {
            CurrentAccount = acc;
        }
        public static void SignOut()
        {
            CurrentAccount = null;
        }

        private class ZoneItem
        {
            public Zone Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }

        public static event Action VehicleAssignmentChanged;

        public static void RaiseVehicleAssignmentChanged()
        {
            try { VehicleAssignmentChanged?.Invoke(); }
            catch { }
        }
    }
}
