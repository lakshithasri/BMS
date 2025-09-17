using KlarityLive.Domain.Core.Entities.BMS;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Configuration
{
    public static class TenancyConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenancy>().ToTable("Tenancy", "BMS").HasKey(a => a.Id);
            modelBuilder.Entity<Tenant>().HasIndex(t => t.Name);
        }
    }
}