using System.ComponentModel.DataAnnotations;

namespace WebServer_DoAn.Models
{
    public class Khoa
    {
        [Key]
        [StringLength(50)] 
        public string MaKhoa { get; set; }

        [Required]
        [StringLength(100)]
        public string TenKhoa { get; set; }

        public ICollection<Nganh> Nganhs { get; set; } = new List<Nganh>();
    }
}
