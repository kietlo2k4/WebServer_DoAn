using WebServer_DoAn.Models;
using WebServer_DoAn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebServer_DoAn.Controllers
{
    public class LopSinhVienController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public LopSinhVienController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        public IActionResult Index(string maNganh)
        {
            ViewData["MaNganh"] = new SelectList(_context.Nganhs, "MaNganh", "TenNganh", maNganh);

            var query = _context.LopSinhViens.Include(l => l.Nganh).AsQueryable();

            if (!string.IsNullOrEmpty(maNganh))
                query = query.Where(l => l.MaNganh == maNganh);

            var list = query.Select(l => new LopSinhVienViewModel
            {
                MaLop = l.MaLop,
                TenLop = l.TenLop,
                KhoaHoc = l.KhoaHoc,
                MaNganh = l.MaNganh
            }).ToList();

            return View(list);
        }

        public IActionResult Create()
        {
            ViewData["MaNganh"] = new SelectList(_context.Nganhs, "MaNganh", "TenNganh");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LopSinhVienViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var model = new LopSinhVien
                {
                    MaLop = vm.MaLop,
                    TenLop = vm.TenLop,
                    KhoaHoc = vm.KhoaHoc,
                    MaNganh = vm.MaNganh
                };
                _context.LopSinhViens.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNganh"] = new SelectList(_context.Nganhs, "MaNganh", "TenNganh", vm.MaNganh);
            return View(vm);
        }

        public IActionResult Edit(string id)
        {
            if (id == null) return NotFound();

            var model = _context.LopSinhViens.Find(id);
            if (model == null) return NotFound();

            var vm = new LopSinhVienViewModel
            {
                MaLop = model.MaLop,
                TenLop = model.TenLop,
                KhoaHoc = model.KhoaHoc,
                MaNganh = model.MaNganh
            };

            ViewData["MaNganh"] = new SelectList(_context.Nganhs, "MaNganh", "TenNganh", vm.MaNganh);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, LopSinhVienViewModel vm)
        {
            if (id != vm.MaLop) return NotFound();

            if (ModelState.IsValid)
            {
                var model = _context.LopSinhViens.Find(id);
                if (model == null) return NotFound();

                model.TenLop = vm.TenLop;
                model.KhoaHoc = vm.KhoaHoc;
                model.MaNganh = vm.MaNganh;

                _context.Update(model);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNganh"] = new SelectList(_context.Nganhs, "MaNganh", "TenNganh", vm.MaNganh);
            return View(vm);
        }

        public IActionResult Delete(string id)
        {
            if (id == null) return NotFound();

            var model = _context.LopSinhViens.Include(l => l.Nganh).FirstOrDefault(l => l.MaLop == id);
            if (model == null) return NotFound();

            var vm = new LopSinhVienViewModel
            {
                MaLop = model.MaLop,
                TenLop = model.TenLop,
                KhoaHoc = model.KhoaHoc,
                MaNganh = model.MaNganh
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var model = _context.LopSinhViens.Find(id);
            if (model != null)
            {
                _context.LopSinhViens.Remove(model);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(string id)
        {
            if (id == null) return NotFound();

            var lop = _context.LopSinhViens
                        .Include(l => l.Nganh)
                        .Include(l => l.SinhViens)
                        .ThenInclude(sv => sv.TaiKhoan) // nếu muốn lấy cả tài khoản
                        .FirstOrDefault(l => l.MaLop == id);

            if (lop == null) return NotFound();

            return View(lop);
        }

    }
}
