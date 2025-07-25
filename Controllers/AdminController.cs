using WebServer_DoAn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace WebServer_DoAn.Controllers
{
    public class AdminController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public AdminController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        // Trang chính Admin
        public IActionResult Index()
        {
            ViewData["TongSinhVien"] = _context.SinhViens.Count();
            ViewData["TongKhoa"] = _context.Khoas.Count();
            ViewData["TongNganh"] = _context.Nganhs.Count();
            ViewData["TongLop"] = _context.LopSinhViens.Count();
            ViewData["TongLopHP"] = _context.LopHocPhans.Count();
            ViewData["TongMonHoc"] = _context.MonHocs.Count();
            return View();
        }

    }
}
