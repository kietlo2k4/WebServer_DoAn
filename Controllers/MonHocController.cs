using WebServer_DoAn.Models;
using WebServer_DoAn.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebServer_DoAn.Controllers
{
    public class MonHocController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public MonHocController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string maKhoa, string maNganh)
        {
            ViewBag.Khoas = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", maKhoa);

            if (!string.IsNullOrEmpty(maKhoa))
            {
                ViewBag.Nganhs = new SelectList(
                    _context.Nganhs.Where(n => n.MaKhoa == maKhoa), "MaNganh", "TenNganh", maNganh);
            }
            else
            {
                ViewBag.Nganhs = new SelectList(_context.Nganhs, "MaNganh", "TenNganh", maNganh);
            }

            var query = _context.MonHocs.Include(m => m.Nganh).AsQueryable();

            if (!string.IsNullOrEmpty(maNganh))
            {
                query = query.Where(m => m.MaNganh == maNganh);
            }
            else if (!string.IsNullOrEmpty(maKhoa))
            {
                query = query.Where(m => m.Nganh.MaKhoa == maKhoa);
            }

            var data = await query
                .Select(m => new MonHocViewModel
                {
                    MaMonHoc = m.MaMonHoc,
                    TenMonHoc = m.TenMonHoc,
                    LoaiMon = m.LoaiMon,
                    SoTiet = m.SoTiet,
                    SoTinChi = m.SoTinChi,
                    MaMonTienQuyet = m.MaMonTienQuyet,
                    MaNganh = m.MaNganh,
                    TenNganh = m.Nganh.TenNganh
                }).ToListAsync();

            return View(data);
        }


        public IActionResult Create()
        {
            ViewBag.Nganhs = new SelectList(_context.Nganhs, "MaNganh", "TenNganh");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonHocViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Nganhs = new SelectList(_context.Nganhs, "MaNganh", "TenNganh", vm.MaNganh);
                return View(vm);
            }

            var mon = new MonHoc
            {
                MaMonHoc = vm.MaMonHoc,
                TenMonHoc = vm.TenMonHoc,
                LoaiMon = vm.LoaiMon,
                SoTiet = vm.SoTiet,
                MaMonTienQuyet = vm.MaMonTienQuyet,
                SoTinChi = vm.SoTinChi,
                MaNganh = vm.MaNganh
            };

            _context.MonHocs.Add(mon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var mon = await _context.MonHocs.FindAsync(id);
            if (mon == null) return NotFound();

            var vm = new MonHocViewModel
            {
                MaMonHoc = mon.MaMonHoc,
                TenMonHoc = mon.TenMonHoc,
                LoaiMon = mon.LoaiMon,
                SoTiet = mon.SoTiet,
                MaMonTienQuyet = mon.MaMonTienQuyet,
                SoTinChi = mon.SoTinChi,
                MaNganh = mon.MaNganh
            };

            ViewBag.Nganhs = new SelectList(_context.Nganhs, "MaNganh", "TenNganh", vm.MaNganh);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, MonHocViewModel vm)
        {
            if (id != vm.MaMonHoc) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Nganhs = new SelectList(_context.Nganhs, "MaNganh", "TenNganh", vm.MaNganh);
                return View(vm);
            }

            var mon = await _context.MonHocs.FindAsync(id);
            if (mon == null) return NotFound();

            mon.TenMonHoc = vm.TenMonHoc;
            mon.LoaiMon = vm.LoaiMon;
            mon.SoTiet = vm.SoTiet;
            mon.MaMonTienQuyet = vm.MaMonTienQuyet;
            mon.SoTinChi = vm.SoTinChi;
            mon.MaNganh = vm.MaNganh;

            _context.Update(mon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var mon = await _context.MonHocs
                .Include(m => m.Nganh)
                .FirstOrDefaultAsync(m => m.MaMonHoc == id);

            if (mon == null) return NotFound();

            var vm = new MonHocViewModel
            {
                MaMonHoc = mon.MaMonHoc,
                TenMonHoc = mon.TenMonHoc,
                LoaiMon = mon.LoaiMon,
                SoTiet = mon.SoTiet,
                MaMonTienQuyet = mon.MaMonTienQuyet,
                SoTinChi = mon.SoTinChi,
                MaNganh = mon.MaNganh,
                TenNganh = mon.Nganh?.TenNganh
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var mon = await _context.MonHocs.FindAsync(id);
            if (mon != null)
            {
                _context.MonHocs.Remove(mon);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
