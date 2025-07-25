using System.ComponentModel.DataAnnotations;

namespace WebServer_DoAn.ViewModel
{
    public class KhoaViewModel
    {
        
            public string MaKhoa { get; set; }

            [Required]
            [StringLength(100)]
            public string TenKhoa { get; set; }
        
    }
}
