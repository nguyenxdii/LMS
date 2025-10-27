using System;

namespace LMS.BUS.Dtos
{
    public class ShipmentDriverLogDto
    {
        public DateTime Timestamp { get; set; }
        public string OldDriverName { get; set; }
        public string NewDriverName { get; set; }
        public int? StopSequenceNumber { get; set; } // Chặng dừng lúc đổi
        public string StopName { get; set; } // Tên kho dừng lúc đổi (optional)
        public string Reason { get; set; }
    }
}