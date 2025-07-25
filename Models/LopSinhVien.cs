using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer_DoAn.Models
{
    public class LopSinhVien
    {
        [Key]
        public string MaLop { get; set; }
        public string TenLop { get; set; }
        public string KhoaHoc { get; set; }

        [Required]
        public string MaNganh { get; set; }
        [ForeignKey("MaNganh")]
        public Nganh Nganh { get; set; }

        public ICollection<SinhVien> SinhViens { get; set; }
    }



}
