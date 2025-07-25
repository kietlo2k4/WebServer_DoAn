using WebServer_DoAn.Models;
using WebServer_DoAn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebServer_DoAn.Controllers
{
    public class ChinhSachHocTapController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public ChinhSachHocTapController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        // 📋 Index
        public async Task<IActionResult> Index()
        {
            var list = await _context.ChinhSachHocTaps
                .Include(c => c.Nganh)
                .Select(c => new ChinhSachHocTapViewModel
                {
                    Id = c.Id,
                    HocKy = c.HocKy,
                    NamHoc = c.NamHoc,
                    TinChiToiThieu = c.TinChiToiThieu,
                    TinChiToiDa = c.TinChiToiDa,
                    NgayBatDauDangKy = c.NgayBatDauDangKy,
                    NgayKetThucDangKy = c.NgayKetThucDangKy,
                    GhiChu = c.GhiChu,
                    MaNganh = c.MaNganh,
                    TenNganh = c.Nganh.TenNganh
                }).ToListAsync();

            return View(list);
        }

        private void LoadNganh(string? selected = null)
        {
            ViewBag.Nganhs = new SelectList(_context.Nganhs, "MaNganh", "TenNganh", selected);
        }

        // ➕ Create
        public IActionResult Create()
        {
            LoadNganh();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChinhSachHocTapViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var entity = new ChinhSachHocTap
                {
                    HocKy = vm.HocKy,
                    NamHoc = vm.NamHoc,
                    TinChiToiThieu = vm.TinChiToiThieu,
                    TinChiToiDa = vm.TinChiToiDa,
                    NgayBatDauDangKy = vm.NgayBatDauDangKy,
                    NgayKetThucDangKy = vm.NgayKetThucDangKy,
                    GhiChu = vm.GhiChu,
                    MaNganh = vm.MaNganh
                };
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            LoadNganh(vm.MaNganh);
            return View(vm);
        }

        // ✍️ Edit
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _context.ChinhSachHocTaps.FindAsync(id);
            if (entity == null) return NotFound();

            var vm = new ChinhSachHocTapViewModel
            {
                Id = entity.Id,
                HocKy = entity.HocKy,
                NamHoc = entity.NamHoc,
                TinChiToiThieu = entity.TinChiToiThieu,
                TinChiToiDa = entity.TinChiToiDa,
                NgayBatDauDangKy = entity.NgayBatDauDangKy,
                NgayKetThucDangKy = entity.NgayKetThucDangKy,
                GhiChu = entity.GhiChu,
                MaNganh = entity.MaNganh
            };

            LoadNganh(vm.MaNganh);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChinhSachHocTapViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var entity = await _context.ChinhSachHocTaps.FindAsync(id);
                if (entity == null) return NotFound();

                entity.HocKy = vm.HocKy;
                entity.NamHoc = vm.NamHoc;
                entity.TinChiToiThieu = vm.TinChiToiThieu;
                entity.TinChiToiDa = vm.TinChiToiDa;
                entity.NgayBatDauDangKy = vm.NgayBatDauDangKy;
                entity.NgayKetThucDangKy = vm.NgayKetThucDangKy;
                entity.GhiChu = vm.GhiChu;
                entity.MaNganh = vm.MaNganh;

                _context.Update(entity);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            LoadNganh(vm.MaNganh);
            return View(vm);
        }

        // 🗑️ Delete
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.ChinhSachHocTaps
                .Include(c => c.Nganh)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null) return NotFound();

            var vm = new ChinhSachHocTapViewModel
            {
                Id = entity.Id,
                HocKy = entity.HocKy,
                NamHoc = entity.NamHoc,
                TinChiToiThieu = entity.TinChiToiThieu,
                TinChiToiDa = entity.TinChiToiDa,
                NgayBatDauDangKy = entity.NgayBatDauDangKy,
                NgayKetThucDangKy = entity.NgayKetThucDangKy,
                GhiChu = entity.GhiChu,
                MaNganh = entity.MaNganh,
                TenNganh = entity.Nganh.TenNganh
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _context.ChinhSachHocTaps.FindAsync(id);
            if (entity != null)
            {
                _context.ChinhSachHocTaps.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // 👀 Details
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _context.ChinhSachHocTaps
                .Include(c => c.Nganh)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null) return NotFound();

            var vm = new ChinhSachHocTapViewModel
            {
                Id = entity.Id,
                HocKy = entity.HocKy,
                NamHoc = entity.NamHoc,
                TinChiToiThieu = entity.TinChiToiThieu,
                TinChiToiDa = entity.TinChiToiDa,
                NgayBatDauDangKy = entity.NgayBatDauDangKy,
                NgayKetThucDangKy = entity.NgayKetThucDangKy,
                GhiChu = entity.GhiChu,
                MaNganh = entity.MaNganh,
                TenNganh = entity.Nganh.TenNganh
            };

            return View(vm);
        }
    }
}
