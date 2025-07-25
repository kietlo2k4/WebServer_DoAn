using WebServer_DoAn.Models;
using WebServer_DoAn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebServer_DoAn.Controllers
{
    public class NganhController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public NganhController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        // 📋 Danh sách ngành + lọc theo khoa
        public IActionResult Index(string maKhoa)
        {
            ViewData["MaKhoa"] = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", maKhoa);

            var query = _context.Nganhs.Include(n => n.Khoa).AsQueryable();

            if (!string.IsNullOrEmpty(maKhoa))
            {
                query = query.Where(n => n.MaKhoa == maKhoa);
            }

            var result = query.Select(n => new NganhViewModel
            {
                MaNganh = n.MaNganh,
                TenNganh = n.TenNganh,
                MaKhoa = n.MaKhoa
            }).ToList();

            return View(result);
        }

        // ➕ Thêm
        public IActionResult Create()
        {
            ViewData["MaKhoa"] = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NganhViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var entity = new Nganh
                {
                    MaNganh = vm.MaNganh,
                    TenNganh = vm.TenNganh,
                    MaKhoa = vm.MaKhoa
                };
                _context.Nganhs.Add(entity);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaKhoa"] = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", vm.MaKhoa);
            return View(vm);
        }

        // ✏️ Sửa
        public IActionResult Edit(string id)
        {
            if (id == null) return NotFound();

            var n = _context.Nganhs.Find(id);
            if (n == null) return NotFound();

            var vm = new NganhViewModel
            {
                MaNganh = n.MaNganh,
                TenNganh = n.TenNganh,
                MaKhoa = n.MaKhoa
            };

            ViewData["MaKhoa"] = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", vm.MaKhoa);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, NganhViewModel vm)
        {
            if (id != vm.MaNganh) return NotFound();

            if (ModelState.IsValid)
            {
                var entity = _context.Nganhs.Find(id);
                if (entity == null) return NotFound();

                entity.TenNganh = vm.TenNganh;
                entity.MaKhoa = vm.MaKhoa;

                _context.Update(entity);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaKhoa"] = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", vm.MaKhoa);
            return View(vm);
        }

        // 👀 Chi tiết
        public IActionResult Details(string id)
        {
            if (id == null) return NotFound();

            var n = _context.Nganhs
                .Include(x => x.Khoa)
                .FirstOrDefault(x => x.MaNganh == id);

            if (n == null) return NotFound();

            var vm = new NganhViewModel
            {
                MaNganh = n.MaNganh,
                TenNganh = n.TenNganh,
                MaKhoa = n.MaKhoa
            };

            return View(vm);
        }

        // ❌ Xóa
        public IActionResult Delete(string id)
        {
            if (id == null) return NotFound();

            var n = _context.Nganhs
                .Include(x => x.Khoa)
                .FirstOrDefault(x => x.MaNganh == id);

            if (n == null) return NotFound();

            var vm = new NganhViewModel
            {
                MaNganh = n.MaNganh,
                TenNganh = n.TenNganh,
                MaKhoa = n.MaKhoa
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var n = _context.Nganhs.Find(id);
            if (n != null)
            {
                _context.Nganhs.Remove(n);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
