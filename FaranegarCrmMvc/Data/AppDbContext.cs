using Microsoft.EntityFrameworkCore;
using FaranegarCrmMvc.Models;

namespace FaranegarCrmMvc.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<SalesReport> SalesReports => Set<SalesReport>();

    // تماس‌ها
    public DbSet<CallLog> CallLogs => Set<CallLog>();

    // ریزرویدادهای صف
    public DbSet<QueueLog> QueueLogs => Set<QueueLog>();

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

            s.HasIndex(x => x.RowFingerprint)
             .IsUnique()
             .HasFilter("[RowFingerprint] IS NOT NULL");
        });

        // CallLog
        modelBuilder.Entity<CallLog>(e =>
        {
            e.HasIndex(x => x.UniqueId).IsUnique();
            e.HasIndex(x => x.StartAt);

            e.Property(x => x.UniqueId).HasMaxLength(64).IsRequired();
            e.Property(x => x.Direction).HasMaxLength(16);
            e.Property(x => x.CallerIdNum).HasMaxLength(64);
            e.Property(x => x.CallerIdName).HasMaxLength(128);
            e.Property(x => x.Src).HasMaxLength(64);
            e.Property(x => x.Dst).HasMaxLength(64);
            e.Property(x => x.SrcChannel).HasMaxLength(128);
            e.Property(x => x.DstChannel).HasMaxLength(128);
            e.Property(x => x.Disposition).HasMaxLength(32);
            e.Property(x => x.HangupCauseText).HasMaxLength(128);
            e.Property(x => x.RecordingFile).HasMaxLength(260);

            // صف + داخلی پاسخ‌گو
            e.Property(x => x.QueueName).HasMaxLength(128);
            e.Property(x => x.AgentExt).HasMaxLength(32);
            e.Property(x => x.AgentChannel).HasMaxLength(128);
        });

        // QueueLog
        modelBuilder.Entity<QueueLog>(e =>
        {
            e.HasIndex(x => x.OccurredAt);
            e.HasIndex(x => x.UniqueId);
            e.Property(x => x.UniqueId).HasMaxLength(64);
            e.Property(x => x.Event).HasMaxLength(64).IsRequired();
            e.Property(x => x.Queue).HasMaxLength(128);
            e.Property(x => x.CallerIdNum).HasMaxLength(64);
            e.Property(x => x.MemberName).HasMaxLength(128);
            e.Property(x => x.Interface).HasMaxLength(128);
            e.Property(x => x.AgentChannel).HasMaxLength(128);
            e.Property(x => x.Reason).HasMaxLength(128);
        });
    }
}
