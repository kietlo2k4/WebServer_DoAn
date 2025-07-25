using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer_DoAn.Models
{
    public class ChuongTrinhKhung
    {
        [Key]
        [StringLength(50)]
        [Display(Name = "Mã CTK")]
        public string MaCTK { get; set; }

        [Display(Name = "Học kỳ")]
        [Range(1, 20, ErrorMessage = "Học kỳ phải từ 1 đến 20")]
        public int? HocKy { get; set; }

        [Display(Name = "Bắt buộc")]
        public bool BatBuoc { get; set; }

        [Required(ErrorMessage = "Ngành không được bỏ trống")]
        [StringLength(50)]
        [Display(Name = "Mã ngành")]
        public string MaNganh { get; set; }

        [ForeignKey(nameof(MaNganh))]
        public Nganh? Nganh { get; set; }

        [Required(ErrorMessage = "Môn học không được bỏ trống")]
        [StringLength(50)]
        [Display(Name = "Mã môn học")]
        public string MaMonHoc { get; set; }

        [ForeignKey(nameof(MaMonHoc))]
        public MonHoc? MonHoc { get; set; }
    }
}
