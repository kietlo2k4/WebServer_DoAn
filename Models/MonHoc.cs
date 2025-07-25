using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer_DoAn.Models
{
    public class MonHoc
    {
        [Key]
        [StringLength(50)] // 450 là quá dài nếu không cần, bạn nên dùng tối đa 50–100 ký tự
        public string MaMonHoc { get; set; }

        [Required]
        [StringLength(100)]
        public string TenMonHoc { get; set; }

        [StringLength(50)]
        public string? LoaiMon { get; set; }

        public int? SoTiet { get; set; }

        [StringLength(50)]
        public string? MaMonTienQuyet { get; set; }

        public int? SoTinChi { get; set; }

        [Required]
        [StringLength(50)]
        public string MaNganh { get; set; }

        [ForeignKey("MaNganh")]
        public Nganh Nganh { get; set; }

        public ICollection<ChuongTrinhKhung> ChuongTrinhKhungs { get; set; } = new List<ChuongTrinhKhung>();
        public ICollection<LopHocPhan> LopHocPhans { get; set; } = new List<LopHocPhan>();
    }
}
