using WebServer_DoAn.Models;
using WebServer_DoAn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebServer_DoAn.Controllers
{
    public class LopHocPhanController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public LopHocPhanController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        // GET: LopHocPhan
        public async Task<IActionResult> Index(string maKhoa, string maNganh, string maMonHoc)
        {
            ViewData["MaKhoa"] = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", maKhoa);

            if (!string.IsNullOrEmpty(maKhoa))
            {
                ViewData["MaNganh"] = new SelectList(
                    _context.Nganhs.Where(n => n.MaKhoa == maKhoa), "MaNganh", "TenNganh", maNganh);
            }
            else
            {
                ViewData["MaNganh"] = new SelectList(Enumerable.Empty<Nganh>());
            }

            if (!string.IsNullOrEmpty(maNganh))
            {
                ViewData["MaMonHoc"] = new SelectList(
                    _context.MonHocs.Where(m => m.MaNganh == maNganh), "MaMonHoc", "TenMonHoc", maMonHoc);
            }
            else
            {
                ViewData["MaMonHoc"] = new SelectList(Enumerable.Empty<MonHoc>());
            }

            var query = _context.LopHocPhans
                                .Include(l => l.MonHoc)
                                .AsQueryable();

            // nếu chọn Môn học → lọc
            if (!string.IsNullOrEmpty(maMonHoc))
            {
                query = query.Where(l => l.MaMonHoc == maMonHoc);
            }
            else if (!string.IsNullOrEmpty(maNganh))
            {
                // nếu chọn Ngành → lọc theo ngành của môn học
                query = query.Where(l => l.MonHoc.MaNganh == maNganh);
            }
            else if (!string.IsNullOrEmpty(maKhoa))
            {
                // nếu chọn Khoa → lọc theo khoa của ngành của môn học
                query = query.Where(l => l.MonHoc.Nganh.MaKhoa == maKhoa);
            }

            // nếu không chọn gì → query giữ nguyên = tất cả
            var result = await query.ToListAsync();

            return View(result);
        }

        // GET: LopHocPhan/MoLop
        public IActionResult MoLop()
        {
            ViewBag.MonHocs = new SelectList(_context.MonHocs.Include(m => m.Nganh),
                                             "MaMonHoc", "TenMonHoc");
            return View();
        }


        // POST: LopHocPhan/MoLop
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoLop(LopHocPhanViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "❌ Dữ liệu chưa hợp lệ. Vui lòng kiểm tra lại.";
                ViewBag.MonHocs = new SelectList(_context.MonHocs.Include(m => m.Nganh),
                                                 "MaMonHoc", "TenMonHoc", vm.MaMonHoc);
                return View(vm);
            }

            // Lấy môn học và ngành của môn
            var monHoc = await _context.MonHocs
                            .Include(m => m.Nganh)
                            .FirstOrDefaultAsync(m => m.MaMonHoc == vm.MaMonHoc);

            if (monHoc == null)
            {
                TempData["Message"] = "❌ Môn học không hợp lệ.";
                ViewBag.MonHocs = new SelectList(_context.MonHocs.Include(m => m.Nganh),
                                                 "MaMonHoc", "TenMonHoc", vm.MaMonHoc);
                return View(vm);
            }

            var maNganh = monHoc.MaNganh;

            // Tìm lớp học phần cuối cùng của ngành này
            var lastLop = await _context.LopHocPhans
                                .Include(l => l.MonHoc)
                                .Where(l => l.MonHoc.MaNganh == maNganh)
                                .OrderByDescending(l => l.MaLopHP)
                                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastLop != null)
            {
                var parts = lastLop.MaLopHP.Split('_');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            var maLopHP = $"LHP_{maNganh}_{nextNumber:D3}";

            var lop = new LopHocPhan
            {
                MaLopHP = maLopHP,
                MaMonHoc = vm.MaMonHoc,
                HocKy = vm.HocKy,
                NamHoc = vm.NamHoc,
                Nhom = vm.Nhom,
                SoLuongToiDa = vm.SoLuongToiDa,
                SoLuongDangKy = 0,
                PhongHoc = vm.PhongHoc,
                Thu = vm.Thu,
                TietBatDau = vm.TietBatDau,
                TietKetThuc = vm.TietKetThuc,
                TrangThai = "Mở"
            };

            _context.LopHocPhans.Add(lop);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"✅ Đã mở lớp học phần: {lop.MaLopHP}";
            return RedirectToAction(nameof(Index));
        }


        // GET: LopHocPhan/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var lop = await _context.LopHocPhans.FindAsync(id);
            if (lop == null) return NotFound();

            var vm = new LopHocPhanViewModel
            {
                MaMonHoc = lop.MaMonHoc,
                HocKy = lop.HocKy,
                NamHoc = lop.NamHoc,
                Nhom = lop.Nhom,
                SoLuongToiDa = lop.SoLuongToiDa,
                PhongHoc = lop.PhongHoc,
                Thu = lop.Thu,
                TietBatDau = lop.TietBatDau,
                TietKetThuc = lop.TietKetThuc
            };

            ViewBag.MonHocs = new SelectList(_context.MonHocs, "MaMonHoc", "TenMonHoc", lop.MaMonHoc);

            ViewBag.LopHocPhanId = id; // lưu id để post

            return View(vm);
        }


        // POST: LopHocPhan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, LopHocPhanViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "❌ Dữ liệu chưa hợp lệ. Vui lòng kiểm tra lại.";
                ViewBag.MonHocs = new SelectList(_context.MonHocs, "MaMonHoc", "TenMonHoc", vm.MaMonHoc);
                ViewBag.LopHocPhanId = id;
                return View(vm);
            }

            var lop = await _context.LopHocPhans.FindAsync(id);
            if (lop == null) return NotFound();

            // Cập nhật
            lop.MaMonHoc = vm.MaMonHoc;
            lop.HocKy = vm.HocKy;
            lop.NamHoc = vm.NamHoc;
            lop.Nhom = vm.Nhom;
            lop.SoLuongToiDa = vm.SoLuongToiDa;
            lop.PhongHoc = vm.PhongHoc;
            lop.Thu = vm.Thu;
            lop.TietBatDau = vm.TietBatDau;
            lop.TietKetThuc = vm.TietKetThuc;

            _context.Update(lop);
            await _context.SaveChangesAsync();

            TempData["Message"] = "✅ Cập nhật lớp học phần thành công.";
            return RedirectToAction(nameof(Index));
        }


        // GET: LopHocPhan/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var lop = await _context.LopHocPhans
                .Include(l => l.MonHoc)
                .Include(l => l.DangKyHocPhans)
                    .ThenInclude(d => d.SinhVien)
                .FirstOrDefaultAsync(l => l.MaLopHP == id);

            if (lop == null) return NotFound();

            return View(lop);
        }

        // GET: LopHocPhan/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var lop = await _context.LopHocPhans
                        .Include(l => l.MonHoc)
                        .FirstOrDefaultAsync(l => l.MaLopHP == id);

            if (lop == null) return NotFound();

            return View(lop);
        }


        // POST: LopHocPhan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var lop = await _context.LopHocPhans
                .Include(l => l.ThanhToans)
                .Include(l => l.XemLichHocs)
                .Include(l => l.DangKyHocPhans)
                .FirstOrDefaultAsync(l => l.MaLopHP == id);

            if (lop != null)
            {
                // Xoá học phí
                if (lop.ThanhToans?.Any() == true)
                    _context.ThanhToans.RemoveRange(lop.ThanhToans);

                // Xoá lịch học
                if (lop.XemLichHocs?.Any() == true)
                    _context.XemLichHocs.RemoveRange(lop.XemLichHocs);

                // Xoá đăng ký học phần
                if (lop.DangKyHocPhans?.Any() == true)
                    _context.DangKyHocPhans.RemoveRange(lop.DangKyHocPhans);

                // Cuối cùng xoá lớp học phần
                _context.LopHocPhans.Remove(lop);

                await _context.SaveChangesAsync();

                TempData["Message"] = "✅ Đã xoá lớp học phần và toàn bộ dữ liệu liên quan.";
            }
            else
            {
                TempData["Message"] = "❌ Không tìm thấy lớp học phần.";
            }

            return RedirectToAction(nameof(Index));
        }




        private bool LopHocPhanExists(string id)
        {
            return _context.LopHocPhans.Any(e => e.MaLopHP == id);
        }

        /// <summary>
        /// Load các dropdown Khoa, Ngành, Môn học theo mã
        /// </summary>
        private void LoadDropdowns(string maKhoa, string maNganh, string maMonHoc)
        {
            ViewBag.Khoas = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", maKhoa);

            if (!string.IsNullOrEmpty(maKhoa))
            {
                ViewBag.Nganhs = new SelectList(_context.Nganhs.Where(n => n.MaKhoa == maKhoa), "MaNganh", "TenNganh", maNganh);
            }
            else
            {
                ViewBag.Nganhs = new SelectList(Enumerable.Empty<Nganh>(), "MaNganh", "TenNganh");
            }

            if (!string.IsNullOrEmpty(maNganh))
            {
                ViewBag.MonHocs = new SelectList(_context.MonHocs.Where(m => m.MaNganh == maNganh), "MaMonHoc", "TenMonHoc", maMonHoc);
            }
            else
            {
                ViewBag.MonHocs = new SelectList(Enumerable.Empty<MonHoc>(), "MaMonHoc", "TenMonHoc");
            }
        }

        /// <summary>
        /// Load các dropdown Khoa, Ngành, Môn học từ MonHoc
        /// </summary>
        private void LoadDropdownsFromMonHoc(MonHoc monHoc, string selectedMonHoc)
        {
            string maNganh = monHoc?.MaNganh;
            string maKhoa = _context.Nganhs.Find(maNganh)?.MaKhoa;

            ViewBag.Khoas = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", maKhoa);
            ViewBag.Nganhs = new SelectList(
                _context.Nganhs.Where(n => n.MaKhoa == maKhoa), "MaNganh", "TenNganh", maNganh);
            ViewBag.MonHocs = new SelectList(
                _context.MonHocs.Where(m => m.MaNganh == maNganh), "MaMonHoc", "TenMonHoc", selectedMonHoc);
            ViewBag.SelectedMonHoc = selectedMonHoc;
        }
    }
}
