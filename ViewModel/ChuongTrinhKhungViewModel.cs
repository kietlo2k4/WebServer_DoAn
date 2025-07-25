using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebServer_DoAn.ViewModel
{
    public class ChuongTrinhKhungViewModel
    {
        public string? MaCTK { get; set; }

        public int? HocKy { get; set; }

        public bool BatBuoc { get; set; }

        public string? MaNganh { get; set; }

        public string? MaMonHoc { get; set; }

        public IEnumerable<SelectListItem>? NganhList { get; set; }

        public IEnumerable<SelectListItem>? MonHocList { get; set; }
    }

}
