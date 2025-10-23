using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsApp.DAL.Models
{
    public class RouteTemplateStop
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Template))]
        public int TemplateId { get; set; }

        /// <summary>Thứ tự chặng 1..N</summary>
        [Required]
        public int Seq { get; set; }

        /// <summary>Kho bắt buộc cho chặng (ưu tiên dùng)</summary>
        public int? WarehouseId { get; set; }

        /// <summary>Tên điểm dừng nếu không gắn kho</summary>
        [StringLength(200)]
        public string StopName { get; set; }

        /// <summary>Dự kiến thời gian bù so với điểm đầu (giờ), optional</summary>
        public double? PlannedOffsetHours { get; set; }

        /// <summary>Ghi chú</summary>
        [StringLength(200)]
        public string Note { get; set; }

        public virtual RouteTemplate Template { get; set; }
    }
}
