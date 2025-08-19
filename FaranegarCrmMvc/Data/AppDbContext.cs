using Microsoft.EntityFrameworkCore;
using FaranegarCrmMvc.Models;

namespace FaranegarCrmMvc.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<SalesReport> SalesReports => Set<SalesReport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(a =>
        {
            a.Property(x => x.Name).HasMaxLength(200).IsRequired();
            a.HasIndex(x => x.Code);
        });

        modelBuilder.Entity<SalesReport>(s =>
        {
            s.HasIndex(x => x.InvoiceRef);
            s.Property(x => x.Subject).HasMaxLength(128);
            s.Property(x => x.InvoiceRef).HasMaxLength(128);
            s.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");

            // ایندکس یکتای فیلترشده برای حذف تکراری‌ها
            s.HasIndex(x => x.RowFingerprint)
             .IsUnique()
             .HasFilter("[RowFingerprint] IS NOT NULL");
        });
    }
}
