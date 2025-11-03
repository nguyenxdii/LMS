using System;
using LMS.DAL.Models;

namespace LMS.BUS.Dtos
{
    public class OrderListItemDto
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public string OriginWarehouseName { get; set; }
        public string DestWarehouseName { get; set; }
        public decimal TotalFee { get; set; }
        public decimal DepositAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StatusVN { get; set; }

    }
}
