using WebServer_DoAn.Models;
using WebServer_DoAn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebServer_DoAn.Controllers
{
    public class ChuongTrinhKhungController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public ChuongTrinhKhungController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        // 📋 Index
        public async Task<IActionResult> Index(string maNganh, int? hocKy, bool? batBuoc, string search, int page = 1, int pageSize = 10)
        {
            var query = _context.ChuongTrinhKhungs
                .Include(c => c.Nganh)
                .Include(c => c.MonHoc)
                .AsQueryable();

            if (!string.IsNullOrEmpty(maNganh))
                query = query.Where(c => c.MaNganh == maNganh);
            if (hocKy.HasValue)
                query = query.Where(c => c.HocKy == hocKy);
            if (batBuoc.HasValue)
                query = query.Where(c => c.BatBuoc == batBuoc);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(c => c.MonHoc.TenMonHoc.Contains(search));

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await query
                .OrderBy(c => c.HocKy)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["MaNganh"] = new SelectList(await _context.Nganhs.ToListAsync(), "MaNganh", "TenNganh", maNganh);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Search = search;
            ViewBag.SelectedHocKy = hocKy;

            return View(items);
        }

        // ➕ Create
        [HttpGet]
        public IActionResult Create() => View(PrepareViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChuongTrinhKhungViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(PrepareViewModel(vm));

            var entity = new ChuongTrinhKhung
            {
                MaCTK = Guid.NewGuid().ToString(),
                HocKy = vm.HocKy,
                BatBuoc = vm.BatBuoc,
                MaNganh = vm.MaNganh,
                MaMonHoc = vm.MaMonHoc
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();

            TempData["Message"] = "✅ Thêm chương trình khung thành công.";
            return RedirectToAction(nameof(Index));
        }

        // 📝 Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var model = await _context.ChuongTrinhKhungs.FindAsync(id);
            if (model == null) return NotFound();

            var vm = PrepareViewModel(new ChuongTrinhKhungViewModel
            {
                MaCTK = model.MaCTK,
                HocKy = model.HocKy,
                BatBuoc = model.BatBuoc,
                MaNganh = model.MaNganh,
                MaMonHoc = model.MaMonHoc
            });

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ChuongTrinhKhungViewModel vm)
        {
            if (id != vm.MaCTK) return NotFound();

            if (!ModelState.IsValid)
                return View(PrepareViewModel(vm));

            var model = await _context.ChuongTrinhKhungs.FindAsync(id);
            if (model == null) return NotFound();

            model.HocKy = vm.HocKy;
            model.BatBuoc = vm.BatBuoc;
            model.MaNganh = vm.MaNganh;
            model.MaMonHoc = vm.MaMonHoc;

            _context.Update(model);
            await _context.SaveChangesAsync();

            TempData["Message"] = "✅ Cập nhật thành công.";
            return RedirectToAction(nameof(Index));
        }

        // 👀 Details
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var model = await _context.ChuongTrinhKhungs
                .Include(c => c.Nganh)
                .Include(c => c.MonHoc)
                .FirstOrDefaultAsync(c => c.MaCTK == id);

            if (model == null) return NotFound();

            return View(model);
        }

        // ❌ Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var model = await _context.ChuongTrinhKhungs
                .Include(c => c.Nganh)
                .Include(c => c.MonHoc)
                .FirstOrDefaultAsync(c => c.MaCTK == id);

            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var model = await _context.ChuongTrinhKhungs.FindAsync(id);
            if (model != null)
            {
                _context.Remove(model);
                await _context.SaveChangesAsync();
            }

            TempData["Message"] = "✅ Đã xóa thành công.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Gán dropdownlist ngành + môn học
        /// </summary>
        private ChuongTrinhKhungViewModel PrepareViewModel(ChuongTrinhKhungViewModel? vm = null)
        {
            vm ??= new ChuongTrinhKhungViewModel();

            vm.NganhList = _context.Nganhs
                .Select(n => new SelectListItem
                {
                    Value = n.MaNganh,
                    Text = n.TenNganh
                }).ToList();

            vm.MonHocList = _context.MonHocs
                .Select(m => new SelectListItem
                {
                    Value = m.MaMonHoc,
                    Text = m.TenMonHoc
                }).ToList();

            return vm;
        }

        // 🌟 GET - Tạo Lớp Học Phần
        [HttpGet]
        public IActionResult TaoLopHocPhan()
        {
            ViewBag.CTKList = _context.ChuongTrinhKhungs
                .Include(c => c.MonHoc)
                .Select(c => new SelectListItem
                {
                    Value = c.MaCTK,
                    Text = $"{c.MaCTK} - {c.MonHoc.TenMonHoc}"
                }).ToList();

            return View();
        }

        // 🌟 POST - Tạo Lớp Học Phần
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TaoLopHocPhan(string maCTK)
        {
            if (string.IsNullOrEmpty(maCTK))
            {
                TempData["Message"] = "❌ Không tìm thấy CTK.";
                return RedirectToAction("Index");
            }

            var ctk = await _context.ChuongTrinhKhungs
                .Include(c => c.MonHoc)
                .FirstOrDefaultAsync(c => c.MaCTK == maCTK);

            if (ctk == null)
            {
                TempData["Message"] = "❌ CTK không tồn tại.";
                return RedirectToAction("Index");
            }

            var maNganh = ctk.MaNganh;

            var lastLhp = await _context.LopHocPhans
                .Where(l => l.MaMonHoc == ctk.MaMonHoc)
                .OrderByDescending(l => l.MaLopHP)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastLhp != null)
            {
                var parts = lastLhp.MaLopHP.Split('_');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastNum))
                    nextNumber = lastNum + 1;
            }

            string maLopHP = $"LHP_{maNganh}_{nextNumber:D3}";

            var sinhViens = await _context.SinhViens
                .Where(sv => sv.LopSinhVien.MaNganh == maNganh)
                .ToListAsync();

            var hocKy = ctk.HocKy ?? 1;
            var namHoc = DateTime.Now.Year;

            var lichDangKy = await _context.XemLichHocs
                .Where(x => sinhViens.Select(sv => sv.MaSV).Contains(x.MaSV)
                            && x.HocKy == hocKy && x.NamHoc == namHoc)
                .ToListAsync();

            var cacThu = new[] { "2", "3", "4", "5", "6", "7" };
            bool found = false;
            string thuChon = "";
            int tietBD = 0, tietKT = 0;

            foreach (var thu in cacThu)
            {
                for (int startTiet = 1; startTiet <= 10; startTiet += 2)
                {
                    int endTiet = startTiet + 1;
                    bool conflict = lichDangKy.Any(l =>
                        l.Thu == thu &&
                        !(l.TietKetThuc < startTiet || l.TietBatDau > endTiet));

                    if (!conflict)
                    {
                        thuChon = thu;
                        tietBD = startTiet;
                        tietKT = endTiet;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }

            if (!found)
            {
                TempData["Message"] = "⚠️ Không tìm được lịch trống cho LHP này.";
                return RedirectToAction("Index");
            }

            var lhp = new LopHocPhan
            {
                MaLopHP = maLopHP,
                MaMonHoc = ctk.MaMonHoc,
                HocKy = hocKy,
                NamHoc = namHoc,
                Nhom = "1",
                SoLuongToiDa = 50,
                SoLuongDangKy = 0,
                PhongHoc = "A101",
                Thu = thuChon,
                TietBatDau = tietBD,
                TietKetThuc = tietKT,
                TrangThai = "Mới tạo"
            };

            _context.LopHocPhans.Add(lhp);

            decimal donGia = 300_000;
            int soTinChi = ctk.MonHoc?.SoTinChi ?? 0;
            decimal tongHocPhi = donGia * soTinChi;

            foreach (var sv in sinhViens)
            {
                _context.DangKyHocPhans.Add(new DangKyHocPhan
                {
                    MaDangKy = Guid.NewGuid().ToString(),
                    MaSV = sv.MaSV,
                    MaLopHP = lhp.MaLopHP,
                    NgayDangKy = DateTime.Now,
                    TrangThai = "Đã đăng ký"
                });

                _context.XemLichHocs.Add(new XemLichHoc
                {
                    MaLichHoc = Guid.NewGuid().ToString(),
                    MaSV = sv.MaSV,
                    MaLopHP = lhp.MaLopHP,
                    MaMonHoc = lhp.MaMonHoc,
                    HoTen = sv.HoTen,
                    TenMonHoc = ctk.MonHoc?.TenMonHoc,
                    Nhom = lhp.Nhom,
                    Thu = lhp.Thu,
                    TietBatDau = lhp.TietBatDau,
                    TietKetThuc = lhp.TietKetThuc,
                    PhongHoc = lhp.PhongHoc,
                    HocKy = hocKy,
                    NamHoc = namHoc
                });

                _context.ThanhToans.Add(new ThanhToan
                {
                    MaThanhToan = Guid.NewGuid().ToString(),
                    MaSV = sv.MaSV,
                    MaLopHP = lhp.MaLopHP,
                    SoTien = tongHocPhi,
                    TrangThai = "Chưa thanh toán"
                });

                lhp.SoLuongDangKy++;
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = $"✅ Đã tạo LHP {lhp.MaLopHP}, thêm {sinhViens.Count} SV & tạo học phí ({tongHocPhi}đ/SV).";
            return RedirectToAction("Index");
        }
    }
}
