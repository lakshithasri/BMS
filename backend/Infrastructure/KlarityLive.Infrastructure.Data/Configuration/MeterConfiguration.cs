using KlarityLive.Domain.Core.Entities.BMS;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Configuration
{
    public static class MeterConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meter>().ToTable("Meter", "BMS").HasKey(a => a.Id);

            modelBuilder.Entity<Meter>()
                .HasOne(m => m.Building)
                .WithMany(p => p.Meters)
                .HasForeignKey(m => m.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Meter>()
               .HasIndex(m => new { m.BuildingId, m.MeterName });

            modelBuilder.Entity<Meter>().Navigation(b => b.Building).AutoInclude();
        }
    }
}
