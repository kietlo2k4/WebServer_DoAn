using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WebServer_DoAn.ViewModel
{
    public class SinhVienViewModel
    {
        public string? MaSV { get; set; }

        [Required]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }

        [Display(Name = "Quê quán")]
        public string QueQuan { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "SĐT")]
        public string SDT { get; set; }

        [Display(Name = "Tình trạng")]
        public string TinhTrang { get; set; }

        [Required]
        [Display(Name = "Tài khoản đăng nhập")]
        public string TaiKhoanDangNhap { get; set; }

        [Required]
        [Display(Name = "Lớp")]
        public string MaLop { get; set; }

        // Lists for dropdowns
        public IEnumerable<SelectListItem>? TaiKhoanList { get; set; }
        public IEnumerable<SelectListItem>? LopList { get; set; }
    }
}
