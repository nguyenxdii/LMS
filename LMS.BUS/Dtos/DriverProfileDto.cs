namespace LMS.BUS.Dtos
{
    // DTO chứa các trường Driver được phép tự sửa
    public class DriverProfileDto
    {
        public int DriverId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string CitizenId { get; set; }
        public string LicenseType { get; set; }
    }
}