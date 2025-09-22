using KlarityLive.Domain.Core.Entities.BMS;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Configuration
{
    public static class MeterReadingConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MeterReading>()
                .ToContainer("MeterReadings")
                .HasPartitionKey(mr => mr.MeterId.ToString())
                .HasKey(mr => mr.Id);

            // Configure properties with JSON property names
            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.Id)
                .ToJsonProperty("id");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.MeterId)
                .ToJsonProperty("meterId");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.TenancyId)
                .ToJsonProperty("tenancyId");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.ReadingDate)
                .ToJsonProperty("readingDate");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.PeriodFromDate)
                .ToJsonProperty("periodFromDate");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.PeriodToDate)
                .ToJsonProperty("periodToDate");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.PeriodDays)
                .ToJsonProperty("periodDays");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.PreviousReading)
                .ToJsonProperty("previousReading");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.PresentReading)
                .ToJsonProperty("presentReading");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.Advance)
                .ToJsonProperty("advance");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.Multiplier)
                .ToJsonProperty("multiplier");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.Quantity)
                .ToJsonProperty("quantity");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.MeasurementUnit)
                .ToJsonProperty("measurementUnit");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.UtilityBillAmount)
                .ToJsonProperty("utilityBillAmount");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.UtilityBillUnit)
                .ToJsonProperty("utilityBillUnit");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.Source)
                .HasConversion<int>()
                .ToJsonProperty("source");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.Notes)
                .ToJsonProperty("notes");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.CreatedOn)
                .ToJsonProperty("createdOn");

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.UpdatedOn)
                .ToJsonProperty("updatedOn");

            // Ignore navigation properties since we're not storing related entities
            modelBuilder.Entity<MeterReading>()
                .Ignore(mr => mr.Meter);

            modelBuilder.Entity<MeterReading>()
                .Ignore(mr => mr.Tenancy);

            // Ignore calculated properties (they shouldn't be stored in Cosmos DB)
            modelBuilder.Entity<MeterReading>()
                .Ignore(mr => mr.ConsumptionDifference);

            modelBuilder.Entity<MeterReading>()
                .Ignore(mr => mr.AdjustedConsumption);

            modelBuilder.Entity<MeterReading>()
                .Ignore(mr => mr.DailyAverageConsumption);

            modelBuilder.Entity<MeterReading>()
                .Ignore(mr => mr.CostPerUnit);

            // Add composite indexes for better query performance
            // Note: Cosmos DB indexes are configured differently than SQL Server
            modelBuilder.Entity<MeterReading>()
                .HasIndex(mr => mr.ReadingDate)
                .HasDatabaseName("IX_MeterReading_ReadingDate");

            modelBuilder.Entity<MeterReading>()
                .HasIndex(mr => new { mr.MeterId, mr.ReadingDate })
                .HasDatabaseName("IX_MeterReading_MeterId_ReadingDate");
        }
    }
}
