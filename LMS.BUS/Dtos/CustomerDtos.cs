using LMS.DAL.Models;
using System.Collections.Generic;

namespace LMS.BUS.Dtos
{
    // DTO dùng cho UC "B" (ucCustomerEditor_Admin)
    public class CustomerEditorDto
    {
        public int Id { get; set; } // 0 nếu là Add
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Chỉ set khi Add hoặc Reset
    }

    // DTO dùng cho UC "A" (ucCustomerDetail_Admin)
    public class CustomerDetailDto
    {
        public Customer Customer { get; set; }
        public UserAccount Account { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}