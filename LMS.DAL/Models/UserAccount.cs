// LMS.DAL/Models/UserAccount.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.DAL.Models
{
    public class UserAccount
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, StringLength(256)]
        public string PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; }

        // Quan hệ một-một tuỳ Role
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int? DriverId { get; set; }
        public virtual Driver Driver { get; set; }

        // === BỔ SUNG ===
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastPasswordChangeAt { get; set; }
        //private void MakeFlat(Guna.UI2.WinForms.Guna2TextBox txt)
        //{
        //    txt.BorderThickness = 0;
        //    txt.BorderColor = Color.Transparent;
        //    txt.FillColor = this.BackColor;
        //    txt.FocusedState.BorderColor = Color.Transparent;
        //    txt.HoverState.BorderColor = Color.Transparent;
        //    txt.DisabledState.BorderColor = Color.Transparent;
        //    txt.DisabledState.FillColor = this.BackColor;
        //    txt.FocusedState.FillColor = this.BackColor;
        //    txt.HoverState.FillColor = this.BackColor;
        //    txt.BackColor = this.BackColor;
        //    txt.Cursor = Cursors.IBeam;
        //}
    }
}
