using Microsoft.EntityFrameworkCore;
using WebServer_DoAn.Models;
using System.Text; // 👈 cần thiết cho Encoding

var builder = WebApplication.CreateBuilder(args);

// ✅ Đăng ký provider để hỗ trợ mã hóa như windows-1252
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// 📦 Add services to the container.
builder.Services.AddControllersWithViews();

// 🗄️ Đăng ký DbContext + MySQL
builder.Services.AddDbContext<QuanLyDaoTaoContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")) // tự phát hiện version MySQL
    ));

// 🗄️ Đăng ký DistributedMemoryCache & Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// 🌐 Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HTTP Strict Transport Security
}

app.UseHttpsRedirection(); // bắt buộc HTTPS
app.UseStaticFiles();      // phục vụ file tĩnh (css, js, img...)

app.UseRouting();          // bật routing
app.UseSession();          // thêm Session middleware
app.UseAuthorization();    // bật authorization (nếu có)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
