namespace LMS.BUS.Dtos
{
    public class KpiDto
    {

        public int TotalOrders { get; set; }
        public int ShipmentsInProgress { get; set; }

        public int ShipmentsCompleted { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}