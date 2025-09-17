using KlarityLive.Domain.Core.Entities.BMS;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Configuration
{
    public static class TenantConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>().ToTable("Tenant", "BMS").HasKey(a => a.Id);
        }
    }
}
