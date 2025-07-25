using WebServer_DoAn.Models;
using WebServer_DoAn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebServer_DoAn.Controllers
{
    public class KhoaController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public KhoaController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.Khoas
                .Select(k => new KhoaViewModel
                {
                    MaKhoa = k.MaKhoa,
                    TenKhoa = k.TenKhoa
                }).ToListAsync();

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhoaViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var model = new Khoa
                {
                    MaKhoa = vm.MaKhoa,
                    TenKhoa = vm.TenKhoa
                };
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var model = await _context.Khoas.FindAsync(id);
            if (model == null) return NotFound();

            var vm = new KhoaViewModel
            {
                MaKhoa = model.MaKhoa,
                TenKhoa = model.TenKhoa
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, KhoaViewModel vm)
        {
            if (id != vm.MaKhoa) return NotFound();

            if (ModelState.IsValid)
            {
                var model = await _context.Khoas.FindAsync(id);
                if (model == null) return NotFound();

                model.TenKhoa = vm.TenKhoa;

                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var khoa = await _context.Khoas
                        .Include(k => k.Nganhs)
                        .FirstOrDefaultAsync(k => k.MaKhoa == id);

            if (khoa == null) return NotFound();

            // Đưa cả Khoa và danh sách Nganhs vào ViewBag
            ViewBag.Khoa = khoa;
            ViewBag.Nganhs = khoa.Nganhs;

            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var model = await _context.Khoas.FindAsync(id);
            if (model == null) return NotFound();

            var vm = new KhoaViewModel
            {
                MaKhoa = model.MaKhoa,
                TenKhoa = model.TenKhoa
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var model = await _context.Khoas.FindAsync(id);
            if (model != null)
            {
                _context.Khoas.Remove(model);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
