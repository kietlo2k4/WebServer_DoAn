using WebServer_DoAn.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QRCoder;
namespace WebServer_DoAn.Controllers
{

    public class SinhVienController : Controller
    {
        private readonly QuanLyDaoTaoContext _context;

        public SinhVienController(QuanLyDaoTaoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Login");

            var sv = _context.SinhViens
                .Include(s => s.LopSinhVien)
                .ThenInclude(l => l.Nganh)
                .ThenInclude(n => n.Khoa)
                .FirstOrDefault(s => s.TaiKhoanDangNhap == username);

            if (sv == null)
                return RedirectToAction("Login", "Login");

            return View(sv);
        }


        public IActionResult ChuongTrinhKhung()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Login");

            var sv = _context.SinhViens
                .Include(s => s.LopSinhVien)
                .FirstOrDefault(s => s.TaiKhoanDangNhap == username);

            if (sv == null)
                return RedirectToAction("Login", "Login");

            var ctk = _context.ChuongTrinhKhungs
                .Include(c => c.MonHoc)
                .Where(c => c.MaNganh == sv.LopSinhVien.MaNganh)
                .OrderBy(c => c.HocKy)
                .ToList();

            return View(ctk);
        }



        public IActionResult LopHocPhan(int? hocKy = null, int? namHoc = null)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Login");

            var sv = _context.SinhViens
                             .Include(s => s.LopSinhVien)
                             .FirstOrDefault(s => s.TaiKhoanDangNhap == username);

            if (sv == null)
                return RedirectToAction("Login", "Login");

            var maNganh = sv.LopSinhVien?.MaNganh;
            if (string.IsNullOrEmpty(maNganh))
            {
                TempData["Message"] = "Không tìm thấy ngành của sinh viên.";
                return RedirectToAction("Index");
            }

            // Lấy tất cả học kỳ & năm học
            var hocKys = _context.LopHocPhans
                                 .Where(l => l.MonHoc.MaNganh == maNganh)
                                 .Select(l => l.HocKy)
                                 .Distinct()
                                 .OrderBy(h => h)
                                 .ToList();

            var namHocs = _context.LopHocPhans
                                  .Where(l => l.MonHoc.MaNganh == maNganh)
                                  .Select(l => l.NamHoc)
                                  .Distinct()
                                  .OrderByDescending(n => n)
                                  .ToList();

            // Nếu chưa chọn, chọn học kỳ + năm học mới nhất
            if (!hocKy.HasValue || !namHoc.HasValue)
            {
                var newest = _context.LopHocPhans
                    .Where(l => l.MonHoc.MaNganh == maNganh)
                    .OrderByDescending(l => l.NamHoc)
                    .ThenByDescending(l => l.HocKy)
                    .FirstOrDefault();

                hocKy = hocKy ?? newest?.HocKy;
                namHoc = namHoc ?? newest?.NamHoc;
            }

            ViewData["HocKy"] = new SelectList(hocKys, hocKy);
            ViewData["NamHoc"] = new SelectList(namHocs, namHoc);

            // Lọc theo MaNganh + học kỳ + năm học
            var query = _context.LopHocPhans
                        .Include(l => l.MonHoc)
                        .Where(l => l.MonHoc.MaNganh == maNganh);

            if (hocKy.HasValue)
                query = query.Where(l => l.HocKy == hocKy);

            if (namHoc.HasValue)
                query = query.Where(l => l.NamHoc == namHoc);

            var lopHocPhans = query.ToList();

            var dangKy = _context.DangKyHocPhans
                        .Include(d => d.LopHocPhan)
                        .ThenInclude(l => l.MonHoc)
                        .Where(d => d.MaSV == sv.MaSV)
                        .ToList();

            return View(Tuple.Create<IEnumerable<LopHocPhan>, IEnumerable<DangKyHocPhan>>(lopHocPhans, dangKy));
        }





        public IActionResult DangKy(string maLopHP)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(maLopHP))
                return RedirectToAction("LopHocPhan");

            var sv = _context.SinhViens
                             .Include(s => s.LopSinhVien).ThenInclude(l => l.Nganh)
                             .FirstOrDefault(s => s.TaiKhoanDangNhap == username);

            var lop = _context.LopHocPhans
                              .Include(l => l.MonHoc)
                              .FirstOrDefault(l => l.MaLopHP == maLopHP);

            if (sv == null || lop == null)
                return RedirectToAction("LopHocPhan");

            if (_context.DangKyHocPhans.Any(d => d.MaSV == sv.MaSV && d.MaLopHP == maLopHP))
            {
                TempData["Message"] = "⚠️ Bạn đã đăng ký lớp học phần này rồi.";
                return RedirectToAction("LopHocPhan");
            }

            if ((lop.SoLuongDangKy ?? 0) >= (lop.SoLuongToiDa ?? 0))
            {
                TempData["Message"] = "⚠️ Lớp học phần này đã đủ số lượng.";
                return RedirectToAction("LopHocPhan");
            }

            if (!string.IsNullOrEmpty(lop.MonHoc?.MaMonTienQuyet))
            {
                bool daQua = _context.DangKyHocPhans
                    .Any(d => d.MaSV == sv.MaSV &&
                              d.LopHocPhan.MaMonHoc == lop.MonHoc.MaMonTienQuyet &&
                              d.DiemTongKet >= 5);
                if (!daQua)
                {
                    TempData["Message"] = $"⚠️ Bạn chưa hoàn thành môn tiên quyết: {lop.MonHoc.MaMonTienQuyet}";
                    return RedirectToAction("LopHocPhan");
                }
            }

            bool trungLich = _context.DangKyHocPhans
                .Include(d => d.LopHocPhan)
                .Any(d => d.MaSV == sv.MaSV
                       && d.LopHocPhan.HocKy == lop.HocKy
                       && d.LopHocPhan.NamHoc == lop.NamHoc
                       && d.LopHocPhan.Thu == lop.Thu
                       && d.LopHocPhan.TietBatDau <= lop.TietKetThuc
                       && d.LopHocPhan.TietKetThuc >= lop.TietBatDau);

            if (trungLich)
            {
                TempData["Message"] = "⚠️ Lịch học của lớp này bị trùng với lớp bạn đã đăng ký.";
                return RedirectToAction("LopHocPhan");
            }

            var cs = _context.ChinhSachHocTaps.FirstOrDefault(c =>
                 c.MaNganh == sv.LopSinhVien.MaNganh &&
                 c.HocKy == lop.HocKy &&
                 c.NamHoc == lop.NamHoc);

            if (cs != null)
            {
                var now = DateTime.Now;
                if (cs.NgayBatDauDangKy != null && cs.NgayKetThucDangKy != null)
                {
                    if (now < cs.NgayBatDauDangKy || now > cs.NgayKetThucDangKy)
                    {
                        TempData["Message"] = $"⚠️ Đã quá hạn đăng ký (từ {cs.NgayBatDauDangKy:dd/MM/yyyy} đến {cs.NgayKetThucDangKy:dd/MM/yyyy}).";
                        return RedirectToAction("LopHocPhan");
                    }
                }

                int tongTinChi = _context.DangKyHocPhans
                    .Where(d => d.MaSV == sv.MaSV &&
                                d.LopHocPhan.HocKy == lop.HocKy &&
                                d.LopHocPhan.NamHoc == lop.NamHoc)
                    .Select(d => d.LopHocPhan.MonHoc.SoTinChi ?? 0)
                    .Sum();

                int dangKyThem = lop.MonHoc.SoTinChi ?? 0;

                if (tongTinChi + dangKyThem > cs.TinChiToiDa)
                {
                    TempData["Message"] = $"⚠️ Tổng tín chỉ sau khi đăng ký ({tongTinChi + dangKyThem}) vượt quá giới hạn ({cs.TinChiToiDa})!";
                    return RedirectToAction("LopHocPhan");
                }
            }

            var dkMoi = new DangKyHocPhan
            {
                MaDangKy = Guid.NewGuid().ToString(),
                MaSV = sv.MaSV,
                MaLopHP = maLopHP,
                NgayDangKy = DateTime.Now,
                TrangThai = "Đã đăng ký"
            };
            _context.DangKyHocPhans.Add(dkMoi);

            lop.SoLuongDangKy = (lop.SoLuongDangKy ?? 0) + 1;

            _context.XemLichHocs.Add(new XemLichHoc
            {
                MaLichHoc = Guid.NewGuid().ToString(),
                MaSV = sv.MaSV,
                MaLopHP = lop.MaLopHP,
                MaMonHoc = lop.MaMonHoc,
                HoTen = sv.HoTen,
                TenMonHoc = lop.MonHoc?.TenMonHoc,
                Nhom = lop.Nhom,
                Thu = lop.Thu,
                TietBatDau = lop.TietBatDau,
                TietKetThuc = lop.TietKetThuc,
                PhongHoc = lop.PhongHoc,
                HocKy = lop.HocKy,
                NamHoc = lop.NamHoc
            });

            decimal donGia = 300000;
            decimal tongHocPhi = donGia * (lop.MonHoc.SoTinChi ?? 0);

            _context.ThanhToans.Add(new ThanhToan
            {
                MaThanhToan = Guid.NewGuid().ToString(),
                MaSV = sv.MaSV,
                MaLopHP = maLopHP,
                SoTien = tongHocPhi,
                TrangThai = "Chưa thanh toán"
            });

            _context.SaveChanges();

            TempData["Message"] = "✅ Đăng ký thành công!";
            return RedirectToAction("LopHocPhan");
        }



        public IActionResult HuyDangKy(string maDangKy)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                TempData["Message"] = "⚠️ Bạn chưa đăng nhập.";
                return RedirectToAction("LopHocPhan");
            }

            // Load sinh viên & lớp sinh viên
            var sv = _context.SinhViens
                .Include(s => s.LopSinhVien)
                .FirstOrDefault(s => s.TaiKhoanDangNhap == username);

            if (sv == null)
            {
                TempData["Message"] = "⚠️ Không tìm thấy thông tin sinh viên.";
                return RedirectToAction("LopHocPhan");
            }

            if (sv.LopSinhVien == null || string.IsNullOrEmpty(sv.LopSinhVien.MaNganh))
            {
                TempData["Message"] = "⚠️ Không tìm thấy thông tin lớp hoặc ngành của sinh viên.";
                return RedirectToAction("LopHocPhan");
            }

            var maNganh = sv.LopSinhVien.MaNganh;

            // Load đăng ký + lớp học phần
            var dk = _context.DangKyHocPhans
                .Include(d => d.LopHocPhan)
                .ThenInclude(l => l.MonHoc)
                .FirstOrDefault(d => d.MaDangKy == maDangKy && d.MaSV == sv.MaSV);

            if (dk == null || dk.LopHocPhan == null || dk.LopHocPhan.MonHoc == null)
            {
                TempData["Message"] = "⚠️ Không tìm thấy thông tin đăng ký hoặc lớp học phần.";
                return RedirectToAction("LopHocPhan");
            }

            var maMonHoc = dk.LopHocPhan.MaMonHoc;

            // Kiểm tra học phần bắt buộc
            bool isBatBuoc = _context.ChuongTrinhKhungs
                .Any(c => c.MaMonHoc == maMonHoc && c.MaNganh == maNganh && c.BatBuoc);

            if (isBatBuoc)
            {
                TempData["Message"] = "⚠️ Đây là học phần bắt buộc, không thể hủy.";
                return RedirectToAction("LopHocPhan");
            }

            // Kiểm tra thời gian hủy <= 7 ngày
            var soNgay = (DateTime.Now - dk.NgayDangKy)?.TotalDays ?? 0;
            if (soNgay > 7)
            {
                TempData["Message"] = "⚠️ Bạn chỉ có thể hủy trong vòng 7 ngày sau khi đăng ký.";
                return RedirectToAction("LopHocPhan");
            }

            // Giảm sĩ số
            if (dk.LopHocPhan.SoLuongDangKy.HasValue && dk.LopHocPhan.SoLuongDangKy > 0)
                dk.LopHocPhan.SoLuongDangKy--;

            // Xóa lịch học
            var lich = _context.XemLichHocs
                .FirstOrDefault(x => x.MaSV == sv.MaSV && x.MaLopHP == dk.MaLopHP);

            if (lich != null)
                _context.XemLichHocs.Remove(lich);

            // Xử lý học phí
            var hocPhi = _context.ThanhToans
                .FirstOrDefault(t => t.MaSV == sv.MaSV && t.MaLopHP == dk.MaLopHP);

            if (hocPhi != null)
            {
                if (hocPhi.TrangThai == "Chưa thanh toán")
                {
                    _context.ThanhToans.Remove(hocPhi);
                }
                else
                {
                    TempData["Message"] = "⚠️ Đăng ký đã thanh toán nên không thể hủy. Vui lòng liên hệ phòng đào tạo.";
                    return RedirectToAction("LopHocPhan");
                }
            }

            // Xóa đăng ký
            _context.DangKyHocPhans.Remove(dk);
            _context.SaveChanges();

            TempData["Message"] = "✅ Hủy đăng ký thành công và học phí đã được hủy.";
            return RedirectToAction("LopHocPhan");
        }

        public IActionResult LichHoc()
        {
            var username = HttpContext.Session.GetString("Username");
            var sv = _context.SinhViens.FirstOrDefault(s => s.TaiKhoanDangNhap == username);

            if (sv == null)
                return RedirectToAction("Login", "Login");

            var lich = _context.XemLichHocs
                .Include(x => x.LopHocPhan)
                .ThenInclude(lhp => lhp.MonHoc)
                .Where(l => l.MaSV == sv.MaSV)
                .ToList();

            return View(lich);
        }
        public IActionResult ExportLichHoc()
        {
            var username = HttpContext.Session.GetString("Username");
            var sv = _context.SinhViens.FirstOrDefault(s => s.TaiKhoanDangNhap == username);

            if (sv == null)
                return RedirectToAction("Login", "Login");

            var lich = _context.XemLichHocs
                .Include(x => x.LopHocPhan)
                .ThenInclude(lhp => lhp.MonHoc)
                .Where(l => l.MaSV == sv.MaSV)
                .ToList();

            using var ms = new MemoryStream();
            var doc = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            // 📝 Load font Unicode
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            var bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            var titleFont = new Font(bf, 16, Font.BOLD, BaseColor.BLACK);
            var headerFont = new Font(bf, 12, Font.BOLD, BaseColor.BLACK);
            var cellFont = new Font(bf, 10, Font.NORMAL, BaseColor.BLACK);

            doc.Add(new Paragraph("📚 LỊCH HỌC CỦA SINH VIÊN", titleFont) { Alignment = Element.ALIGN_CENTER });
            doc.Add(new Paragraph($"\nHọ tên: {sv.HoTen}   -   MSSV: {sv.MaSV}\n\n", cellFont));

            var table = new PdfPTable(6) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 3, 2, 2, 2, 1, 1 });

            string[] headers = { "Môn học", "Thứ", "Tiết", "Phòng", "Học kỳ", "Năm học" };
            foreach (var h in headers)
            {
                var cell = new PdfPCell(new Phrase(h, headerFont))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                table.AddCell(cell);
            }

            foreach (var item in lich)
            {
                string tiet = "-";
                if (item.TietBatDau.HasValue && item.TietKetThuc.HasValue)
                {
                    tiet = item.TietBatDau == item.TietKetThuc
                        ? $"Tiết {item.TietBatDau}"
                        : $"Tiết {item.TietBatDau}-{item.TietKetThuc}";
                }

                table.AddCell(new Phrase(item.TenMonHoc ?? "-", cellFont));
                table.AddCell(new Phrase(item.Thu ?? "-", cellFont));
                table.AddCell(new Phrase(tiet, cellFont));
                table.AddCell(new Phrase(item.PhongHoc ?? "-", cellFont));
                table.AddCell(new Phrase(item.HocKy?.ToString() ?? "-", cellFont));
                table.AddCell(new Phrase(item.NamHoc?.ToString() ?? "-", cellFont));
            }

            doc.Add(table);
            doc.Close();

            return File(ms.ToArray(), "application/pdf", $"LichHoc_{sv.MaSV}.pdf");
        }

        public IActionResult HocPhi()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Login");

            var sv = _context.SinhViens.FirstOrDefault(s => s.TaiKhoanDangNhap == username);
            if (sv == null)
                return RedirectToAction("Login", "Login");

            var hp = _context.ThanhToans
                .Include(t => t.LopHocPhan)
                    .ThenInclude(lhp => lhp.MonHoc) // đảm bảo load cả MonHoc
                .Where(t => t.MaSV == sv.MaSV)
                .ToList();

            return View(hp);
        }


        public IActionResult KetQua()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                TempData["Message"] = "⚠️ Bạn chưa đăng nhập.";
                return RedirectToAction("Login", "Account");
            }

            var sv = _context.SinhViens.FirstOrDefault(s => s.TaiKhoanDangNhap == username);
            if (sv == null)
            {
                TempData["Message"] = "⚠️ Không tìm thấy thông tin sinh viên.";
                return RedirectToAction("Login", "Account");
            }

            var kq = _context.DangKyHocPhans
                .Include(d => d.LopHocPhan)
                .ThenInclude(l => l.MonHoc)
                .Where(d => d.MaSV == sv.MaSV)
                .ToList();

            return View(kq);
        }

        public IActionResult XuatKetQuaHocTapPDF()
        {
            var username = HttpContext.Session.GetString("Username");
            var sv = _context.SinhViens.FirstOrDefault(s => s.TaiKhoanDangNhap == username);

            if (sv == null)
                return RedirectToAction("Login", "Login");

            var ketQua = _context.DangKyHocPhans
                .Include(d => d.LopHocPhan)
                .ThenInclude(lhp => lhp.MonHoc)
                .Where(d => d.MaSV == sv.MaSV)
                .ToList();

            using var ms = new MemoryStream();
            var doc = new Document(PageSize.A4, 20, 20, 20, 20);
            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            // Load font Unicode
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            var bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            var titleFont = new Font(bf, 16, Font.BOLD, BaseColor.BLACK);
            var headerFont = new Font(bf, 12, Font.BOLD, BaseColor.BLACK);
            var cellFont = new Font(bf, 10, Font.NORMAL, BaseColor.BLACK);

            doc.Add(new Paragraph("📚 KẾT QUẢ HỌC TẬP", titleFont) { Alignment = Element.ALIGN_CENTER });
            doc.Add(new Paragraph($"\nHọ tên: {sv.HoTen}   -   MSSV: {sv.MaSV}\n\n", cellFont));

            var table = new PdfPTable(6) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 4, 1, 1, 1, 1, 1 });

            string[] headers = { "Môn học", "Điểm CC", "Điểm GK", "Điểm CK", "Tổng kết", "Trạng thái" };
            foreach (var h in headers)
            {
                var cell = new PdfPCell(new Phrase(h, headerFont))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                table.AddCell(cell);
            }

            foreach (var item in ketQua)
            {
                double? tongKet = null;
                if (item.DiemCC.HasValue || item.DiemGK.HasValue || item.DiemCK.HasValue)
                {
                    tongKet =
                        (item.DiemCC ?? 0) * 0.1 +
                        (item.DiemGK ?? 0) * 0.3 +
                        (item.DiemCK ?? 0) * 0.6;
                }

                table.AddCell(new Phrase(item.LopHocPhan.MonHoc.TenMonHoc, cellFont));
                table.AddCell(new Phrase(item.DiemCC?.ToString("0.0") ?? "-", cellFont));
                table.AddCell(new Phrase(item.DiemGK?.ToString("0.0") ?? "-", cellFont));
                table.AddCell(new Phrase(item.DiemCK?.ToString("0.0") ?? "-", cellFont));
                table.AddCell(new Phrase(tongKet?.ToString("0.0") ?? "-", cellFont));
                table.AddCell(new Phrase(item.TrangThai ?? "-", cellFont));
            }

            doc.Add(table);
            doc.Close();

            return File(ms.ToArray(), "application/pdf", $"KetQuaHocTap_{sv.MaSV}.pdf");
        }

        public IActionResult XuatHocPhiPDF()
        {
            var username = HttpContext.Session.GetString("Username");
            var sv = _context.SinhViens.FirstOrDefault(s => s.TaiKhoanDangNhap == username);

            if (sv == null)
                return RedirectToAction("Login", "Login");

            var hocPhi = _context.ThanhToans
                .Include(t => t.LopHocPhan).ThenInclude(l => l.MonHoc)
                .Where(t => t.MaSV == sv.MaSV).ToList();

            using var ms = new MemoryStream();
            var doc = new Document(PageSize.A4, 20, 20, 20, 20);
            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            // Load font Unicode
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            var bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            var titleFont = new Font(bf, 16, Font.BOLD, BaseColor.BLACK);
            var headerFont = new Font(bf, 12, Font.BOLD, BaseColor.BLACK);
            var cellFont = new Font(bf, 10, Font.NORMAL, BaseColor.BLACK);

            doc.Add(new Paragraph("💰 HỌC PHÍ SINH VIÊN", titleFont) { Alignment = Element.ALIGN_CENTER });
            doc.Add(new Paragraph($"\nHọ tên: {sv.HoTen}   -   MSSV: {sv.MaSV}\n\n", cellFont));

            var table = new PdfPTable(4) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 4, 2, 2, 2 });

            string[] headers = { "Môn học", "Số tiền", "Ngày thanh toán", "Trạng thái" };
            foreach (var h in headers)
            {
                var cell = new PdfPCell(new Phrase(h, headerFont))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                table.AddCell(cell);
            }

            foreach (var item in hocPhi)
            {
                table.AddCell(new Phrase(item.LopHocPhan?.MonHoc?.TenMonHoc ?? "-", cellFont));
                table.AddCell(new Phrase($"{item.SoTien:N0} đ", cellFont));
                table.AddCell(new Phrase(item.NgayThanhToan?.ToString("dd/MM/yyyy") ?? "Chưa thanh toán", cellFont));
                table.AddCell(new Phrase(item.TrangThai ?? "-", cellFont));
            }

            doc.Add(table);
            doc.Close();

            return File(ms.ToArray(), "application/pdf", $"HocPhi_{sv.MaSV}.pdf");
        }

        public IActionResult ThanhToan(string maDangKy)
        {
            var dk = _context.DangKyHocPhans
                             .Include(x => x.LopHocPhan)
                             .ThenInclude(l => l.MonHoc)
                             .Include(x => x.SinhVien)
                             .FirstOrDefault(x => x.MaDangKy == maDangKy);

            if (dk == null) return NotFound();

            // Kiểm tra hoặc tạo mới bản ghi ThanhToan
            var thanhToan = _context.ThanhToans
                                    .FirstOrDefault(t => t.MaSV == dk.MaSV && t.MaLopHP == dk.MaLopHP);

            if (thanhToan == null)
            {
                thanhToan = new ThanhToan
                {
                    MaThanhToan = Guid.NewGuid().ToString(),
                    MaSV = dk.MaSV,
                    MaLopHP = dk.MaLopHP,
                    SoTien = 1000000, // tuỳ tính toán
                    TrangThai = "Chưa thanh toán"
                };
                _context.ThanhToans.Add(thanhToan);
                _context.SaveChanges();
            }

            return View(thanhToan);
        }


        public IActionResult ThanhToanQR(string maThanhToan)
        {
            var thanhToan = _context.ThanhToans
                                    .Include(t => t.SinhVien)
                                    .Include(t => t.LopHocPhan)
                                    .ThenInclude(l => l.MonHoc)
                                    .FirstOrDefault(t => t.MaThanhToan == maThanhToan);

            if (thanhToan == null) return NotFound();

            string noiDungChuyenKhoan = $"Thanh toán học phí: {thanhToan.SinhVien.HoTen} - Môn: {thanhToan.LopHocPhan.MonHoc.TenMonHoc} - {thanhToan.SoTien} VNĐ";
            string stk = "0359759657";
            string tenNguoiNhan = "Vũ Anh Kiệt";

            string qrContent = $"STK: {stk}\nTên: {tenNguoiNhan}\nNội dung: {noiDungChuyenKhoan}";

            var generator = new QRCodeGenerator();
            var qrData = generator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrData);
            byte[] qrBytes = qrCode.GetGraphic(20);

            return File(qrBytes, "image/QR.jpg");
        }

    }
}
