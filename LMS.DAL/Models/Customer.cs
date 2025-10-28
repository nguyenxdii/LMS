using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static TheArtOfDevHtmlRenderer.Adapters.RGraphicsPath;

namespace LMS.DAL.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        [EmailAddress, StringLength(120)]
        public string Email { get; set; }

        [Required, StringLength(200)]
        public string Address { get; set; }
        public byte[] AvatarData { get; set; } // lưu trực tiếp vào db, nên là không khuyên dùng

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
        public virtual ICollection<UserAccount> Accounts { get; set; } = new HashSet<UserAccount>();
    }
}
