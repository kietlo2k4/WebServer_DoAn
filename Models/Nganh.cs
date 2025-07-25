using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer_DoAn.Models
{
    public class Nganh
    {
        [Key]
        [StringLength(50)] 
        public string MaNganh { get; set; }

        [Required]
        [StringLength(100)]
        public string TenNganh { get; set; }

        [Required]
        [StringLength(50)]
        public string MaKhoa { get; set; }

        [ForeignKey("MaKhoa")]
        public Khoa Khoa { get; set; }

        public ICollection<LopSinhVien> LopSinhViens { get; set; } = new List<LopSinhVien>();
        public ICollection<MonHoc> MonHocs { get; set; } = new List<MonHoc>();
        public ICollection<ChuongTrinhKhung> ChuongTrinhKhungs { get; set; } = new List<ChuongTrinhKhung>();
    }
}
