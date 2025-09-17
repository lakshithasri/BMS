using KlarityLive.Domain.Core.Entities.BMS;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Configuration
{
    public static class BuildingConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Building>().ToTable("Building", "BMS").HasKey(a => a.Id);

            modelBuilder.Entity<Building>()
                .HasIndex(p => p.Name);

            //modelBuilder.Entity<Building>().Navigation(b => b.Tenancies).AutoInclude();
            modelBuilder.Entity<Building>().Navigation(b => b.Meters).AutoInclude();
        }
    }
}