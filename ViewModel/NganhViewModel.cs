using System.ComponentModel.DataAnnotations;

namespace WebServer_DoAn.ViewModel
{
    public class NganhViewModel
    {
        [Required]
        public string MaNganh { get; set; }

        [Required]
        public string TenNganh { get; set; }

        [Required]
        public string MaKhoa { get; set; }
    }
}
