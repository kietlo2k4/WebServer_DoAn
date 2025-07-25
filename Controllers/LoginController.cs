using WebServer_DoAn.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebServer_DoAn.Controllers
{
    public class LoginController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public LoginController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password, string role)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            var user = _context.TaiKhoans
                .FirstOrDefault(t => t.TaiKhoanDangNhap == username
                                  && t.MatKhau == password
                                  && t.VaiTro == role);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.TaiKhoanDangNhap);
                HttpContext.Session.SetString("Role", user.VaiTro);

                if (user.VaiTro == "Admin")
                    return RedirectToAction("Index", "Admin");
                else if (user.VaiTro == "SinhVien")
                    return RedirectToAction("Index", "SinhVien");
            }

            ViewBag.Error = "Tài khoản, mật khẩu hoặc vai trò không đúng.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }


}
