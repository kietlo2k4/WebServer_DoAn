using System.ComponentModel.DataAnnotations;

namespace WebServer_DoAn.Models
{
    public class TaiKhoan
    {
        [Key]
        public string TaiKhoanDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string VaiTro { get; set; }

        public ICollection<SinhVien> SinhViens { get; set; }
    }

}
