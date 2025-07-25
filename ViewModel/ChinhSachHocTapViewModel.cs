using System.ComponentModel.DataAnnotations;

namespace WebServer_DoAn.ViewModel
{
    public class ChinhSachHocTapViewModel
    {
        public int Id { get; set; }

        [Required]
        public int HocKy { get; set; }

        [Required]
        public int NamHoc { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TinChiToiThieu { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TinChiToiDa { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgayBatDauDangKy { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgayKetThucDangKy { get; set; }

        [StringLength(250)]
        public string? GhiChu { get; set; }

        [Required]
        public string MaNganh { get; set; }

        public string? TenNganh { get; set; }
    }

}
