using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer_DoAn.Models
{
    public class DangKyHocPhan
    {
        [Key]
        public string MaDangKy { get; set; }
        public DateTime? NgayDangKy { get; set; }
        public string TrangThai { get; set; }
        public double? DiemCC { get; set; }
        public double? DiemGK { get; set; }
        public double? DiemCK { get; set; }
        public double? DiemTongKet { get; set; }

        public string MaSV { get; set; }
        [ForeignKey("MaSV")]
        public SinhVien SinhVien { get; set; }

        public string MaLopHP { get; set; }
        [ForeignKey("MaLopHP")]
        public LopHocPhan LopHocPhan { get; set; }
    }


}
