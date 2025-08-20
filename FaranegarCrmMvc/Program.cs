using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FaranegarCrmMvc.Data;
using FaranegarCrmMvc.Services;
using FaranegarCrmMvc.Models;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

var conn = builder.Configuration.GetConnectionString("DefaultConnection");

// 1) DbContext برای کنترلرها (Scoped) اما با optionsLifetime=Singleton تا با کارخانه سازگار شود
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(conn),
    contextLifetime: ServiceLifetime.Scoped,
    optionsLifetime: ServiceLifetime.Singleton);

// 2) کارخانهٔ DbContext برای سرویس بک‌گراند (Singleton)
builder.Services.AddDbContextFactory<AppDbContext>(
    options => options.UseSqlServer(conn));

// Excel 2003 XML
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

// 🔹 ثبت سرویس ایمپورت (لازم برای ImportController)
builder.Services.AddScoped<FaranegarImporter>();

// تنظیمات AMI
builder.Services.Configure<AmiOptions>(builder.Configuration.GetSection("AsteriskAmi"));
// سرویس شنونده AMI (از IDbContextFactory<AppDbContext> استفاده می‌کند)
builder.Services.AddHostedService<AmiListener>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SalesReports}/{action=Index}/{id?}");

// اجرای مایگریشن‌ها در استارتاپ
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
