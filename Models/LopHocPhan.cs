using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer_DoAn.Models
{
    public class LopHocPhan
    {
        [Key]
        public string MaLopHP { get; set; }
        public int? HocKy { get; set; }
        public int? NamHoc { get; set; }
        public string Nhom { get; set; }
        public int? SoLuongToiDa { get; set; }
        public int? SoLuongDangKy { get; set; }
        public string PhongHoc { get; set; }
        public string Thu { get; set; }
        public int? TietBatDau { get; set; }
        public int? TietKetThuc { get; set; }
        public string TrangThai { get; set; }

        public string MaMonHoc { get; set; }
        [ForeignKey("MaMonHoc")]
        public MonHoc MonHoc { get; set; }

        public ICollection<DangKyHocPhan> DangKyHocPhans { get; set; }
        public ICollection<ThanhToan> ThanhToans { get; set; }
        public ICollection<XemLichHoc> XemLichHocs { get; set; }
    }


}
