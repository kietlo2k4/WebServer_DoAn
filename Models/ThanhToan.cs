using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer_DoAn.Models
{
    public class ThanhToan
    {
        [Key]
        public string MaThanhToan { get; set; }

        public DateTime? NgayThanhToan { get; set; }

        [Precision(18, 2)] // 👈 Thêm dòng này để tránh lỗi EF
        public decimal? SoTien { get; set; }

        public string TrangThai { get; set; }

        public string MaSV { get; set; }
        [ForeignKey("MaSV")]
        public SinhVien SinhVien { get; set; }

        public string MaLopHP { get; set; }
        [ForeignKey("MaLopHP")]
        public LopHocPhan LopHocPhan { get; set; }
    }
}
