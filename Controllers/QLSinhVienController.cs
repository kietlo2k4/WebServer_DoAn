using WebServer_DoAn.Models;
using WebServer_DoAn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebServer_DoAn.Controllers
{
    public class QLSinhVienController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public QLSinhVienController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        /// 📋 Danh sách Sinh viên với lọc
        public async Task<IActionResult> Index(string maKhoa, string maNganh, string maLop)
        {
            await PopulateFilters(maKhoa, maNganh, maLop);

            var query = _context.SinhViens.Include(s => s.LopSinhVien).AsQueryable();

            if (!string.IsNullOrEmpty(maLop))
                query = query.Where(s => s.MaLop == maLop);

            var list = await query.Select(s => new SinhVienViewModel
            {
                MaSV = s.MaSV,
                HoTen = s.HoTen,
                GioiTinh = s.GioiTinh,
                NgaySinh = s.NgaySinh,
                QueQuan = s.QueQuan,
                Email = s.Email,
                SDT = s.SDT,
                TinhTrang = s.TinhTrang,
                MaLop = s.MaLop,
                TaiKhoanDangNhap = s.TaiKhoanDangNhap
            }).ToListAsync();

            return View(list);
        }

        /// 👀 Chi tiết
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var sv = await _context.SinhViens.Include(s => s.LopSinhVien)
                                             .FirstOrDefaultAsync(s => s.MaSV == id);
            if (sv == null) return NotFound();

            return View(ToViewModel(sv));
        }

        /// ➕ Thêm
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SinhVienViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(vm.MaLop, vm.TaiKhoanDangNhap);
                return View(vm);
            }

            var sv = FromViewModel(vm);
            _context.Add(sv);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// ✏️ Sửa
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var sv = await _context.SinhViens.FindAsync(id);
            if (sv == null) return NotFound();

            await PopulateDropdowns(sv.MaLop, sv.TaiKhoanDangNhap);
            return View(ToViewModel(sv));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, SinhVienViewModel vm)
        {
            if (id != vm.MaSV) return NotFound();

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(vm.MaLop, vm.TaiKhoanDangNhap);
                return View(vm);
            }

            var sv = await _context.SinhViens.FindAsync(id);
            if (sv == null) return NotFound();

            UpdateEntity(sv, vm);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// ❌ Xóa
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var sv = await _context.SinhViens.Include(s => s.LopSinhVien)
                                             .FirstOrDefaultAsync(s => s.MaSV == id);
            if (sv == null) return NotFound();

            return View(ToViewModel(sv));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sv = await _context.SinhViens.FindAsync(id);
            if (sv != null)
            {
                _context.SinhViens.Remove(sv);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        /// ================================
        /// 🧹 Tiện ích
        /// ================================

        /// Populate dropdowns cho Create/Edit
        private async Task PopulateDropdowns(string? maLop = null, string? taiKhoan = null)
        {
            ViewData["MaLop"] = new SelectList(await _context.LopSinhViens.ToListAsync(), "MaLop", "TenLop", maLop);
            ViewData["TaiKhoanDangNhap"] = new SelectList(await _context.TaiKhoans.ToListAsync(), "TaiKhoanDangNhap", "TaiKhoanDangNhap", taiKhoan);
        }

        /// Populate bộ lọc
        private async Task PopulateFilters(string? maKhoa, string? maNganh, string? maLop)
        {
            ViewData["MaKhoa"] = new SelectList(await _context.Khoas.ToListAsync(), "MaKhoa", "TenKhoa", maKhoa);

            if (!string.IsNullOrEmpty(maKhoa))
                ViewData["MaNganh"] = new SelectList(
                    await _context.Nganhs.Where(n => n.MaKhoa == maKhoa).ToListAsync(), "MaNganh", "TenNganh", maNganh);
            else
                ViewData["MaNganh"] = new SelectList(Enumerable.Empty<Nganh>());

            if (!string.IsNullOrEmpty(maNganh))
                ViewData["MaLop"] = new SelectList(
                    await _context.LopSinhViens.Where(l => l.MaNganh == maNganh).ToListAsync(), "MaLop", "TenLop", maLop);
            else
                ViewData["MaLop"] = new SelectList(Enumerable.Empty<LopSinhVien>());
        }

        /// Chuyển Entity => ViewModel
        private SinhVienViewModel ToViewModel(SinhVien s) => new SinhVienViewModel
        {
            MaSV = s.MaSV,
            HoTen = s.HoTen,
            GioiTinh = s.GioiTinh,
            NgaySinh = s.NgaySinh,
            QueQuan = s.QueQuan,
            Email = s.Email,
            SDT = s.SDT,
            TinhTrang = s.TinhTrang,
            MaLop = s.MaLop,
            TaiKhoanDangNhap = s.TaiKhoanDangNhap
        };

        /// Chuyển ViewModel => Entity mới
        private SinhVien FromViewModel(SinhVienViewModel vm) => new SinhVien
        {
            MaSV = vm.MaSV ?? Guid.NewGuid().ToString(),
            HoTen = vm.HoTen,
            GioiTinh = vm.GioiTinh,
            NgaySinh = vm.NgaySinh,
            QueQuan = vm.QueQuan,
            Email = vm.Email,
            SDT = vm.SDT,
            TinhTrang = vm.TinhTrang,
            MaLop = vm.MaLop,
            TaiKhoanDangNhap = vm.TaiKhoanDangNhap
        };

        /// Cập nhật Entity từ ViewModel
        private void UpdateEntity(SinhVien sv, SinhVienViewModel vm)
        {
            sv.HoTen = vm.HoTen;
            sv.GioiTinh = vm.GioiTinh;
            sv.NgaySinh = vm.NgaySinh;
            sv.QueQuan = vm.QueQuan;
            sv.Email = vm.Email;
            sv.SDT = vm.SDT;
            sv.TinhTrang = vm.TinhTrang;
            sv.MaLop = vm.MaLop;
            sv.TaiKhoanDangNhap = vm.TaiKhoanDangNhap;
        }
    }
}
