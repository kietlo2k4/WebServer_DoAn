using WebServer_DoAn.Models;
using WebServer_DoAn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebServer_DoAn.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public TaiKhoanController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        // 📋 Hiển thị danh sách
        public async Task<IActionResult> Index()
        {
            var list = await _context.TaiKhoans
                .Select(t => new TaiKhoanViewModel
                {
                    TaiKhoanDangNhap = t.TaiKhoanDangNhap,
                    MatKhau = t.MatKhau,
                    VaiTro = t.VaiTro
                })
                .ToListAsync();

            return View(list);
        }

        // 👀 Chi tiết
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var tk = await _context.TaiKhoans
                .FirstOrDefaultAsync(t => t.TaiKhoanDangNhap == id);

            if (tk == null) return NotFound();

            var vm = new TaiKhoanViewModel
            {
                TaiKhoanDangNhap = tk.TaiKhoanDangNhap,
                MatKhau = tk.MatKhau,
                VaiTro = tk.VaiTro
            };

            return View(vm);
        }

        // ➕ Tạo
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaiKhoanViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var tk = new TaiKhoan
                {
                    TaiKhoanDangNhap = vm.TaiKhoanDangNhap,
                    MatKhau = vm.MatKhau,
                    VaiTro = vm.VaiTro
                };

                _context.TaiKhoans.Add(tk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        // ✍️ Sửa
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var tk = await _context.TaiKhoans.FindAsync(id);
            if (tk == null) return NotFound();

            var vm = new TaiKhoanViewModel
            {
                TaiKhoanDangNhap = tk.TaiKhoanDangNhap,
                MatKhau = tk.MatKhau,
                VaiTro = tk.VaiTro
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, TaiKhoanViewModel vm)
        {
            if (id != vm.TaiKhoanDangNhap) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var tk = await _context.TaiKhoans.FindAsync(id);
                    if (tk == null) return NotFound();

                    tk.MatKhau = vm.MatKhau;
                    tk.VaiTro = vm.VaiTro;

                    _context.Update(tk);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanExists(vm.TaiKhoanDangNhap))
                        return NotFound();
                    else throw;
                }
            }

            return View(vm);
        }

        // ❌ Xóa
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var tk = await _context.TaiKhoans
                .FirstOrDefaultAsync(t => t.TaiKhoanDangNhap == id);

            if (tk == null) return NotFound();

            var vm = new TaiKhoanViewModel
            {
                TaiKhoanDangNhap = tk.TaiKhoanDangNhap,
                MatKhau = tk.MatKhau,
                VaiTro = tk.VaiTro
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tk = await _context.TaiKhoans.FindAsync(id);
            if (tk != null)
            {
                _context.TaiKhoans.Remove(tk);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TaiKhoanExists(string id)
        {
            return _context.TaiKhoans.Any(e => e.TaiKhoanDangNhap == id);
        }
    }
}
