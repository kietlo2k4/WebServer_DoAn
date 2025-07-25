using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer_DoAn.Models
{
    public class SinhVien
    {
        [Key]
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string QueQuan { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string TinhTrang { get; set; }

        public string TaiKhoanDangNhap { get; set; }
        [ForeignKey("TaiKhoanDangNhap")]
        public TaiKhoan TaiKhoan { get; set; }

        public string MaLop { get; set; }
        [ForeignKey("MaLop")]
        public LopSinhVien LopSinhVien { get; set; }

        public ICollection<DangKyHocPhan> DangKyHocPhans { get; set; }
        public ICollection<ThanhToan> ThanhToans { get; set; }
        public ICollection<XemLichHoc> XemLichHocs { get; set; }
    }


}
