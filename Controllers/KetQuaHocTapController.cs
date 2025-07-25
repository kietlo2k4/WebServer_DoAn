using WebServer_DoAn.Models;
using WebServer_DoAn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebServer_DoAn.Controllers
{
    public class KetQuaHocTapController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public KetQuaHocTapController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string maKhoa, string maNganh, string maLopHP)
        {
            // Lấy danh sách Khoa
            ViewBag.Khoas = new SelectList(await _context.Khoas.ToListAsync(),
                                           "MaKhoa", "TenKhoa", maKhoa);

            // Danh sách ngành theo khoa
            if (!string.IsNullOrEmpty(maKhoa))
            {
                ViewBag.Nganhs = new SelectList(await _context.Nganhs
                    .Where(n => n.MaKhoa == maKhoa)
                    .ToListAsync(), "MaNganh", "TenNganh", maNganh);
            }
            else
            {
                ViewBag.Nganhs = new SelectList(Enumerable.Empty<Nganh>(), "MaNganh", "TenNganh");
            }

            // Danh sách lớp học phần theo ngành
            if (!string.IsNullOrEmpty(maNganh))
            {
                ViewBag.LopHocPhans = new SelectList(await _context.LopHocPhans
                    .Include(l => l.MonHoc)
                    .Where(l => l.MonHoc.MaNganh == maNganh)
                    .ToListAsync(), "MaLopHP", "MaLopHP", maLopHP);
            }
            else
            {
                ViewBag.LopHocPhans = new SelectList(Enumerable.Empty<LopHocPhan>(), "MaLopHP", "MaLopHP");
            }

            // Nếu đã chọn lớp học phần, lấy danh sách sinh viên + điểm
            if (!string.IsNullOrEmpty(maLopHP))
            {
                var list = await _context.DangKyHocPhans
                    .Include(d => d.SinhVien)
                    .Include(d => d.LopHocPhan)
                    .ThenInclude(l => l.MonHoc)
                    .Where(d => d.MaLopHP == maLopHP)
                    .Select(d => new KetQuaHocTapViewModel
                    {
                        MaDangKy = d.MaDangKy,
                        MaSV = d.MaSV,
                        HoTen = d.SinhVien.HoTen,
                        MaLopHP = d.MaLopHP,
                        TenMonHoc = d.LopHocPhan.MonHoc.TenMonHoc,
                        DiemCC = d.DiemCC,
                        DiemGK = d.DiemGK,
                        DiemCK = d.DiemCK,
                        DiemTongKet = d.DiemTongKet
                    })
                    .ToListAsync();

                ViewBag.SelectedLopHP = maLopHP;
                return View(list);
            }

            return View(new List<KetQuaHocTapViewModel>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatDiem(List<KetQuaHocTapViewModel> diems, string maKhoa, string maNganh, string maLopHP)
        {
            if (diems == null || !diems.Any())
                return RedirectToAction(nameof(Index), new { maKhoa, maNganh, maLopHP });

            foreach (var item in diems)
            {
                var dk = await _context.DangKyHocPhans.FindAsync(item.MaDangKy);
                if (dk != null)
                {
                    dk.DiemCC = item.DiemCC;
                    dk.DiemGK = item.DiemGK;
                    dk.DiemCK = item.DiemCK;

                    // Tính tổng kết
                    dk.DiemTongKet =
                        (item.DiemCC ?? 0) * 0.1 +
                        (item.DiemGK ?? 0) * 0.3 +
                        (item.DiemCK ?? 0) * 0.6;
                }
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = "✅ Đã cập nhật điểm!";
            return RedirectToAction(nameof(Index), new { maKhoa, maNganh, maLopHP });
        }
    }
}
