using WebServer_DoAn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebServer_DoAn.Controllers
{
    public class ThongKeHocPhiController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public ThongKeHocPhiController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        public IActionResult Index(string maKhoa = null, string maNganh = null, string maLop = null)
        {
            // Query toàn bộ ThanhToan
            var query = _context.ThanhToans
                                .Include(t => t.SinhVien)
                                    .ThenInclude(sv => sv.LopSinhVien)
                                        .ThenInclude(lsv => lsv.Nganh)
                                            .ThenInclude(n => n.Khoa)
                                .Include(t => t.LopHocPhan)
                                    .ThenInclude(lhp => lhp.MonHoc)
                                .AsQueryable();

            // Lọc Khoa
            if (!string.IsNullOrEmpty(maKhoa))
            {
                query = query.Where(t => t.SinhVien.LopSinhVien.Nganh.Khoa.MaKhoa == maKhoa);
            }

            // Lọc Ngành
            if (!string.IsNullOrEmpty(maNganh))
            {
                query = query.Where(t => t.SinhVien.LopSinhVien.MaNganh == maNganh);
            }

            // Lọc Lớp
            if (!string.IsNullOrEmpty(maLop))
            {
                query = query.Where(t => t.SinhVien.MaLop == maLop);
            }

            // Đổ dữ liệu dropdown
            ViewBag.MaKhoa = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", maKhoa);

            if (!string.IsNullOrEmpty(maKhoa))
            {
                ViewBag.MaNganh = new SelectList(
                    _context.Nganhs.Where(n => n.MaKhoa == maKhoa),
                    "MaNganh", "TenNganh", maNganh);
            }
            else
            {
                ViewBag.MaNganh = new SelectList(Enumerable.Empty<Nganh>(), "MaNganh", "TenNganh");
            }

            if (!string.IsNullOrEmpty(maNganh))
            {
                ViewBag.MaLop = new SelectList(
                    _context.LopSinhViens.Where(l => l.MaNganh == maNganh),
                    "MaLop", "TenLop", maLop);
            }
            else
            {
                ViewBag.MaLop = new SelectList(Enumerable.Empty<LopSinhVien>(), "MaLop", "TenLop");
            }

            // Thực hiện tính toán tổng
            var ds = query.ToList();

            var tongDaThanhToan = ds
                .Where(t => t.TrangThai == "Đã thanh toán" && t.SoTien.HasValue)
                .Sum(t => t.SoTien.Value);

            var tongChuaThanhToan = ds
                .Where(t => t.TrangThai != "Đã thanh toán" && t.SoTien.HasValue)
                .Sum(t => t.SoTien.Value);

            var model = Tuple.Create(tongDaThanhToan, tongChuaThanhToan, ds.AsEnumerable());

            return View(model);
        }

    }
}
