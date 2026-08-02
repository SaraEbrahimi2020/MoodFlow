using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moodflow.Domain.Entities;

namespace MoodFlow.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // معرفی موجودیت دفترچه به دیتابیس برای ساخت جدول
    public DbSet<MoodLog> MoodLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // یک تنظیم مهندسی برای اینکه انام حس و حال به صورت عدد در دیتابیس ذخیره شود
        modelBuilder.Entity<MoodLog>()
            .Property(m => m.MorningMoodScore)
            .HasConversion<int>();
    }
}