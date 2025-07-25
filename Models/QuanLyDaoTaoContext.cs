using Microsoft.EntityFrameworkCore;

namespace WebServer_DoAn.Models
{
    public class QuanLyDaoTaoContext : DbContext
    {
        public QuanLyDaoTaoContext(DbContextOptions<QuanLyDaoTaoContext> options) : base(options) { }

        public DbSet<Khoa> Khoas { get; set; }
        public DbSet<Nganh> Nganhs { get; set; }
        public DbSet<LopSinhVien> LopSinhViens { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<MonHoc> MonHocs { get; set; }
        public DbSet<ChuongTrinhKhung> ChuongTrinhKhungs { get; set; }
        public DbSet<LopHocPhan> LopHocPhans { get; set; }
        public DbSet<DangKyHocPhan> DangKyHocPhans { get; set; }
        public DbSet<ThanhToan> ThanhToans { get; set; }
        public DbSet<XemLichHoc> XemLichHocs { get; set; }

        public DbSet<ChinhSachHocTap> ChinhSachHocTaps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ChuongTrinhKhung
            modelBuilder.Entity<ChuongTrinhKhung>()
                .HasOne(ctk => ctk.Nganh)
                .WithMany(n => n.ChuongTrinhKhungs)
                .HasForeignKey(ctk => ctk.MaNganh)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChuongTrinhKhung>()
                .HasOne(ctk => ctk.MonHoc)
                .WithMany(m => m.ChuongTrinhKhungs)
                .HasForeignKey(ctk => ctk.MaMonHoc)
                .OnDelete(DeleteBehavior.Restrict);

            // DangKyHocPhan
            modelBuilder.Entity<DangKyHocPhan>()
                .HasOne(dk => dk.SinhVien)
                .WithMany(sv => sv.DangKyHocPhans)
                .HasForeignKey(dk => dk.MaSV)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DangKyHocPhan>()
                .HasOne(dk => dk.LopHocPhan)
                .WithMany(lhp => lhp.DangKyHocPhans)
                .HasForeignKey(dk => dk.MaLopHP)
                .OnDelete(DeleteBehavior.Restrict);

            // ThanhToan
            modelBuilder.Entity<ThanhToan>()
                .HasOne(tt => tt.SinhVien)
                .WithMany(sv => sv.ThanhToans)
                .HasForeignKey(tt => tt.MaSV)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ThanhToan>()
                .HasOne(tt => tt.LopHocPhan)
                .WithMany(lhp => lhp.ThanhToans)
                .HasForeignKey(tt => tt.MaLopHP)
                .OnDelete(DeleteBehavior.Restrict);

            // XemLichHoc
            modelBuilder.Entity<XemLichHoc>()
                .HasOne(x => x.SinhVien)
                .WithMany(sv => sv.XemLichHocs)
                .HasForeignKey(x => x.MaSV)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<XemLichHoc>()
                .HasOne(x => x.LopHocPhan)
                .WithMany(lhp => lhp.XemLichHocs)
                .HasForeignKey(x => x.MaLopHP)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<XemLichHoc>()
                .HasOne(x => x.MonHoc)
                .WithMany()
                .HasForeignKey(x => x.MaMonHoc)
                .OnDelete(DeleteBehavior.Restrict);

            // ChinhSachHocTap
            modelBuilder.Entity<ChinhSachHocTap>()
                .HasOne(cs => cs.Nganh)
                .WithMany() // Nếu Nganh có ICollection<ChinhSachHocTap> thì thay bằng WithMany(n => n.ChinhSachHocTaps)
                .HasForeignKey(cs => cs.MaNganh)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
