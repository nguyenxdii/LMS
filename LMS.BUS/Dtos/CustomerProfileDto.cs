// LMS.BUS/Dtos/CustomerProfileDto.cs
namespace LMS.BUS.Dtos
{
    // DTO này chỉ chứa các trường mà Customer được phép tự sửa
    public class CustomerProfileDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}