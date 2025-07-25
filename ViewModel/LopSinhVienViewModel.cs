using System.ComponentModel.DataAnnotations;

namespace WebServer_DoAn.ViewModel
{
    public class LopSinhVienViewModel
    {
        [Required]
        public string MaLop { get; set; }

        [Required]
        public string TenLop { get; set; }

        [Required]
        public string KhoaHoc { get; set; }

        [Required]
        public string MaNganh { get; set; }
    }
}
