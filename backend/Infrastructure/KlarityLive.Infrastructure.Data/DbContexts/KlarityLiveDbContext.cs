using KlarityLive.Domain.Core.Entities.Amin;
using KlarityLive.Domain.Core.Entities.BMS;
using KlarityLive.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.DbContexts
{
    public class KlarityLiveDbContext(DbContextOptions<KlarityLiveDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Tenancy> Tenancies { get; set; }
        public DbSet<Meter> Meters { get; set; }
        public DbSet<TenantTenancy> TenantTenancies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UserConfiguration.Configure(modelBuilder);
            BuildingConfiguration.Configure(modelBuilder);
            TenancyConfiguration.Configure(modelBuilder);
            TenantConfiguration.Configure(modelBuilder);
            MeterConfiguration.Configure(modelBuilder);
            TenantTenancyConfiguration.Configure(modelBuilder);
        }
    }
}
