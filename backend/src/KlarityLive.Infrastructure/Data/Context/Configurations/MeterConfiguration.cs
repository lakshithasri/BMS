using KlarityLive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Context.Configurations
{
    public static class MeterConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meter>().ToTable("Meter", "Admin").HasKey(a => a.Id);

            modelBuilder.Entity<Meter>()
                .HasOne(m => m.Building)
                .WithMany(p => p.Meters)
                .HasForeignKey(m => m.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Meter>()
               .HasIndex(m => new { m.PropertyId, m.MeterName });
        }
    }
}
