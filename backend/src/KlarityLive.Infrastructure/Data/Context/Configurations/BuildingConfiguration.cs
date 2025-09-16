using KlarityLive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Context.Configurations
{
    public static class BuildingConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Building>().ToTable("Building", "Admin").HasKey(a => a.Id);

            modelBuilder.Entity<Building>()
                .HasIndex(p => p.Name);
        }
    }
}
