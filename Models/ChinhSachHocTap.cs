using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer_DoAn.Models
{
    public class ChinhSachHocTap
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Học kỳ")]
        public int HocKy { get; set; }

        [Required]
        [Display(Name = "Năm học")]
        public int NamHoc { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Tín chỉ tối thiểu")]
        public int TinChiToiThieu { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Tín chỉ tối đa")]
        public int TinChiToiDa { get; set; }

        [Display(Name = "Ngày bắt đầu đăng ký")]
        [DataType(DataType.Date)]
        public DateTime? NgayBatDauDangKy { get; set; }

        [Display(Name = "Ngày kết thúc đăng ký")]
        [DataType(DataType.Date)]
        public DateTime? NgayKetThucDangKy { get; set; }

        [StringLength(250)]
        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Ngành áp dụng")]
        public string MaNganh { get; set; }

        [ForeignKey("MaNganh")]
        public Nganh? Nganh { get; set; }
    }
}
