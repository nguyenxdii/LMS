using LMS.DAL;
using LMS.DAL.Models;

namespace LMS.BUS.Services
{
    public class RoutePricingService
    {
        public decimal GetRouteFee(int originWarehouseId, int destWarehouseId)
        {
            using (var db = new LogisticsDbContext())
            {
                var o = db.Warehouses.Find(originWarehouseId);
                var d = db.Warehouses.Find(destWarehouseId);
                if (o == null || d == null) return 0m;

                if (o.ZoneId == d.ZoneId) return 100_000m;
                if (IsAdjacent(o.ZoneId, d.ZoneId)) return 150_000m;
                return 200_000m;
            }
        }

        private bool IsAdjacent(Zone a, Zone b)
        {
            if (a == b) return false; // cùng vùng đã xử lý ở trên
            return (a == Zone.North && b == Zone.Central) ||
                   (a == Zone.Central && b == Zone.North) ||
                   (a == Zone.Central && b == Zone.South) ||
                   (a == Zone.South && b == Zone.Central);
        }

        public decimal GetPickupFee(bool needPickup) => needPickup ? 100_000m : 0m;
    }
}
