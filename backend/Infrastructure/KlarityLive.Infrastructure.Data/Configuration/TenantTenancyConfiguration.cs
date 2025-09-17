using KlarityLive.Domain.Core.Entities.BMS;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Configuration
{
    public class TenantTenancyConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TenantTenancy>().ToTable("TenantTenancy", "BMS").HasKey(a => a.Id);

            modelBuilder.Entity<TenantTenancy>()
                .HasOne(t => t.Tenant)
                .WithMany(t => t.Tenancies)
                .HasForeignKey(t => t.TenancyId)
                .HasPrincipalKey(a => a.Id);

            modelBuilder.Entity<TenantTenancy>().HasOne(t => t.Tenancy);

            modelBuilder.Entity<TenantTenancy>().Navigation(t => t.Tenant).AutoInclude();
            modelBuilder.Entity<TenantTenancy>().Navigation(t => t.Tenancy).AutoInclude();
        }
    }
}
