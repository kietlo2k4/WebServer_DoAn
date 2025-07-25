using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer_DoAn.Models
{
    public class XemLichHoc
    {
        [Key]
        public string MaLichHoc { get; set; }
        public string HoTen { get; set; }
        public string TenMonHoc { get; set; }
        public string Nhom { get; set; }
        public string Thu { get; set; }
        public int? TietBatDau { get; set; }
        public int? TietKetThuc { get; set; }
        public string PhongHoc { get; set; }
        public int? HocKy { get; set; }
        public int? NamHoc { get; set; }

        public string MaSV { get; set; }
        [ForeignKey("MaSV")]
        public SinhVien SinhVien { get; set; }

        public string MaMonHoc { get; set; }
        [ForeignKey("MaMonHoc")]
        public MonHoc MonHoc { get; set; }

        public string MaLopHP { get; set; }
        [ForeignKey("MaLopHP")]
        public LopHocPhan LopHocPhan { get; set; }
    }


}
