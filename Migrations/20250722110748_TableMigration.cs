using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebServer_DoAn.Migrations
{
    /// <inheritdoc />
    public partial class TableMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Khoas",
                columns: table => new
                {
                    MaKhoa = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenKhoa = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khoas", x => x.MaKhoa);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TaiKhoans",
                columns: table => new
                {
                    TaiKhoanDangNhap = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MatKhau = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VaiTro = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoans", x => x.TaiKhoanDangNhap);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Nganhs",
                columns: table => new
                {
                    MaNganh = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenNganh = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaKhoa = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nganhs", x => x.MaNganh);
                    table.ForeignKey(
                        name: "FK_Nganhs_Khoas_MaKhoa",
                        column: x => x.MaKhoa,
                        principalTable: "Khoas",
                        principalColumn: "MaKhoa",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChinhSachHocTaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HocKy = table.Column<int>(type: "int", nullable: false),
                    NamHoc = table.Column<int>(type: "int", nullable: false),
                    TinChiToiThieu = table.Column<int>(type: "int", nullable: false),
                    TinChiToiDa = table.Column<int>(type: "int", nullable: false),
                    NgayBatDauDangKy = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NgayKetThucDangKy = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    GhiChu = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaNganh = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChinhSachHocTaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChinhSachHocTaps_Nganhs_MaNganh",
                        column: x => x.MaNganh,
                        principalTable: "Nganhs",
                        principalColumn: "MaNganh",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LopSinhViens",
                columns: table => new
                {
                    MaLop = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenLop = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KhoaHoc = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaNganh = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LopSinhViens", x => x.MaLop);
                    table.ForeignKey(
                        name: "FK_LopSinhViens_Nganhs_MaNganh",
                        column: x => x.MaNganh,
                        principalTable: "Nganhs",
                        principalColumn: "MaNganh",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MonHocs",
                columns: table => new
                {
                    MaMonHoc = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenMonHoc = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoaiMon = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SoTiet = table.Column<int>(type: "int", nullable: true),
                    MaMonTienQuyet = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SoTinChi = table.Column<int>(type: "int", nullable: true),
                    MaNganh = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonHocs", x => x.MaMonHoc);
                    table.ForeignKey(
                        name: "FK_MonHocs_Nganhs_MaNganh",
                        column: x => x.MaNganh,
                        principalTable: "Nganhs",
                        principalColumn: "MaNganh",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SinhViens",
                columns: table => new
                {
                    MaSV = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HoTen = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NgaySinh = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    GioiTinh = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QueQuan = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SDT = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TinhTrang = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TaiKhoanDangNhap = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaLop = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhViens", x => x.MaSV);
                    table.ForeignKey(
                        name: "FK_SinhViens_LopSinhViens_MaLop",
                        column: x => x.MaLop,
                        principalTable: "LopSinhViens",
                        principalColumn: "MaLop",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SinhViens_TaiKhoans_TaiKhoanDangNhap",
                        column: x => x.TaiKhoanDangNhap,
                        principalTable: "TaiKhoans",
                        principalColumn: "TaiKhoanDangNhap",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChuongTrinhKhungs",
                columns: table => new
                {
                    MaCTK = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HocKy = table.Column<int>(type: "int", nullable: true),
                    BatBuoc = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MaNganh = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaMonHoc = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuongTrinhKhungs", x => x.MaCTK);
                    table.ForeignKey(
                        name: "FK_ChuongTrinhKhungs_MonHocs_MaMonHoc",
                        column: x => x.MaMonHoc,
                        principalTable: "MonHocs",
                        principalColumn: "MaMonHoc",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChuongTrinhKhungs_Nganhs_MaNganh",
                        column: x => x.MaNganh,
                        principalTable: "Nganhs",
                        principalColumn: "MaNganh",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LopHocPhans",
                columns: table => new
                {
                    MaLopHP = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HocKy = table.Column<int>(type: "int", nullable: true),
                    NamHoc = table.Column<int>(type: "int", nullable: true),
                    Nhom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SoLuongToiDa = table.Column<int>(type: "int", nullable: true),
                    SoLuongDangKy = table.Column<int>(type: "int", nullable: true),
                    PhongHoc = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Thu = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TietBatDau = table.Column<int>(type: "int", nullable: true),
                    TietKetThuc = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaMonHoc = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LopHocPhans", x => x.MaLopHP);
                    table.ForeignKey(
                        name: "FK_LopHocPhans_MonHocs_MaMonHoc",
                        column: x => x.MaMonHoc,
                        principalTable: "MonHocs",
                        principalColumn: "MaMonHoc",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DangKyHocPhans",
                columns: table => new
                {
                    MaDangKy = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NgayDangKy = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TrangThai = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiemCC = table.Column<double>(type: "double", nullable: true),
                    DiemGK = table.Column<double>(type: "double", nullable: true),
                    DiemCK = table.Column<double>(type: "double", nullable: true),
                    DiemTongKet = table.Column<double>(type: "double", nullable: true),
                    MaSV = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaLopHP = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKyHocPhans", x => x.MaDangKy);
                    table.ForeignKey(
                        name: "FK_DangKyHocPhans_LopHocPhans_MaLopHP",
                        column: x => x.MaLopHP,
                        principalTable: "LopHocPhans",
                        principalColumn: "MaLopHP",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DangKyHocPhans_SinhViens_MaSV",
                        column: x => x.MaSV,
                        principalTable: "SinhViens",
                        principalColumn: "MaSV",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ThanhToans",
                columns: table => new
                {
                    MaThanhToan = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TrangThai = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaSV = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaLopHP = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToans", x => x.MaThanhToan);
                    table.ForeignKey(
                        name: "FK_ThanhToans_LopHocPhans_MaLopHP",
                        column: x => x.MaLopHP,
                        principalTable: "LopHocPhans",
                        principalColumn: "MaLopHP",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThanhToans_SinhViens_MaSV",
                        column: x => x.MaSV,
                        principalTable: "SinhViens",
                        principalColumn: "MaSV",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "XemLichHocs",
                columns: table => new
                {
                    MaLichHoc = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HoTen = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenMonHoc = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nhom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Thu = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TietBatDau = table.Column<int>(type: "int", nullable: true),
                    TietKetThuc = table.Column<int>(type: "int", nullable: true),
                    PhongHoc = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HocKy = table.Column<int>(type: "int", nullable: true),
                    NamHoc = table.Column<int>(type: "int", nullable: true),
                    MaSV = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaMonHoc = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaLopHP = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XemLichHocs", x => x.MaLichHoc);
                    table.ForeignKey(
                        name: "FK_XemLichHocs_LopHocPhans_MaLopHP",
                        column: x => x.MaLopHP,
                        principalTable: "LopHocPhans",
                        principalColumn: "MaLopHP",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_XemLichHocs_MonHocs_MaMonHoc",
                        column: x => x.MaMonHoc,
                        principalTable: "MonHocs",
                        principalColumn: "MaMonHoc",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_XemLichHocs_SinhViens_MaSV",
                        column: x => x.MaSV,
                        principalTable: "SinhViens",
                        principalColumn: "MaSV",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ChinhSachHocTaps_MaNganh",
                table: "ChinhSachHocTaps",
                column: "MaNganh");

            migrationBuilder.CreateIndex(
                name: "IX_ChuongTrinhKhungs_MaMonHoc",
                table: "ChuongTrinhKhungs",
                column: "MaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_ChuongTrinhKhungs_MaNganh",
                table: "ChuongTrinhKhungs",
                column: "MaNganh");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyHocPhans_MaLopHP",
                table: "DangKyHocPhans",
                column: "MaLopHP");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyHocPhans_MaSV",
                table: "DangKyHocPhans",
                column: "MaSV");

            migrationBuilder.CreateIndex(
                name: "IX_LopHocPhans_MaMonHoc",
                table: "LopHocPhans",
                column: "MaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_LopSinhViens_MaNganh",
                table: "LopSinhViens",
                column: "MaNganh");

            migrationBuilder.CreateIndex(
                name: "IX_MonHocs_MaNganh",
                table: "MonHocs",
                column: "MaNganh");

            migrationBuilder.CreateIndex(
                name: "IX_Nganhs_MaKhoa",
                table: "Nganhs",
                column: "MaKhoa");

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_MaLop",
                table: "SinhViens",
                column: "MaLop");

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_TaiKhoanDangNhap",
                table: "SinhViens",
                column: "TaiKhoanDangNhap");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToans_MaLopHP",
                table: "ThanhToans",
                column: "MaLopHP");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToans_MaSV",
                table: "ThanhToans",
                column: "MaSV");

            migrationBuilder.CreateIndex(
                name: "IX_XemLichHocs_MaLopHP",
                table: "XemLichHocs",
                column: "MaLopHP");

            migrationBuilder.CreateIndex(
                name: "IX_XemLichHocs_MaMonHoc",
                table: "XemLichHocs",
                column: "MaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_XemLichHocs_MaSV",
                table: "XemLichHocs",
                column: "MaSV");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChinhSachHocTaps");

            migrationBuilder.DropTable(
                name: "ChuongTrinhKhungs");

            migrationBuilder.DropTable(
                name: "DangKyHocPhans");

            migrationBuilder.DropTable(
                name: "ThanhToans");

            migrationBuilder.DropTable(
                name: "XemLichHocs");

            migrationBuilder.DropTable(
                name: "LopHocPhans");

            migrationBuilder.DropTable(
                name: "SinhViens");

            migrationBuilder.DropTable(
                name: "MonHocs");

            migrationBuilder.DropTable(
                name: "LopSinhViens");

            migrationBuilder.DropTable(
                name: "TaiKhoans");

            migrationBuilder.DropTable(
                name: "Nganhs");

            migrationBuilder.DropTable(
                name: "Khoas");
        }
    }
}
