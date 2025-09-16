using KlarityLive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Context.Configurations
{
    public static class TenantConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>().ToTable("Tenant", "Admin").HasKey(a => a.Id);

            modelBuilder.Entity<Tenant>()
                .HasIndex(t => t.Name);
        }
    }
}
