using KlarityLive.Domain.Core.Entities.BMS;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Configuration
{
    public static class MeterReadingConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeterReading>().ToTable("MeterReading", "BMS").HasKey(a => a.Id);

            modelBuilder.Entity<MeterReading>()
                .HasOne(mr => mr.Meter)
                .WithMany(m => m.MeterReadings)
                .HasForeignKey(mr => mr.MeterId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<MeterReading>()
            //    .HasOne(mr => mr.Tenancy)
            //    .WithMany(t => t.MeterReadings)
            //    .HasForeignKey(mr => mr.TenancyId)
            //    .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<MeterReading>()
                .HasIndex(mr => new { mr.MeterId, mr.ReadingDate });
        }
    }
}
