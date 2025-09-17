using KlarityLive.Domain.Core.Entities.BMS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace KlarityLive.Infrastructure.Data.DbContexts
{
    public class KlarityLiveCosmosDbContext : DbContext
    {
        public KlarityLiveCosmosDbContext(DbContextOptions<KlarityLiveCosmosDbContext> options) : base(options)
        {
        }

        // Only MeterReading entity for Cosmos DB
        public DbSet<MeterReading> MeterReadings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure MeterReading entity for Cosmos DB
            ConfigureMeterReadingForCosmos(modelBuilder);
        }

        private void ConfigureMeterReadingForCosmos(ModelBuilder modelBuilder)
        {
            // Configure MeterReading entity
            modelBuilder.Entity<MeterReading>()
                .ToContainer("MeterReadings")
                .HasPartitionKey(mr => mr.MeterId.ToString())
                .HasKey(mr => mr.Id);

            // Configure properties
            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.ReadingDate)
                .IsRequired();

            modelBuilder.Entity<MeterReading>()
                .Property(mr => mr.MeterId)
                .IsRequired();

            // Since we're not storing related entities in Cosmos, 
            // we'll ignore navigation properties or handle them differently
            modelBuilder.Entity<MeterReading>()
                .Ignore(mr => mr.Meter);

            // Add any additional indexes for query performance
            modelBuilder.Entity<MeterReading>()
                .HasIndex(mr => mr.ReadingDate)
                .HasName("IX_MeterReading_ReadingDate");

            modelBuilder.Entity<MeterReading>()
                .HasIndex(mr => new { mr.MeterId, mr.ReadingDate })
                .HasName("IX_MeterReading_MeterId_ReadingDate");
        }
    }
}
