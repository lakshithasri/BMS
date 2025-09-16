using KlarityLive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Context.Configurations
{
    public static class TenancyConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenancy>().ToTable("Tenancy", "Admin").HasKey(a => a.Id);

            modelBuilder.Entity<Tenancy>()
               .HasOne(t => t.Building)
               .WithMany(p => p.Tenancies)
               .HasForeignKey(t => t.PropertyId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tenancy>()
                .HasOne(t => t.Tenant)
                .WithMany(t => t.Tenancies)
                .HasForeignKey(t => t.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tenancy>()
                .HasIndex(t => new { t.PropertyId, t.TenantId });
        }
    }
}
