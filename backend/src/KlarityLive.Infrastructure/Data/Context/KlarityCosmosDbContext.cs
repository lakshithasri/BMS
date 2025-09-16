using KlarityLive.Domain.Entities.Cosmos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlarityLive.Infrastructure.Data.Context
{
    public class KlarityCosmosDbContext : DbContext
    {
        public KlarityCosmosDbContext(DbContextOptions<KlarityCosmosDbContext> options)
            : base(options)
        {
        }

        public DbSet<MeterReadingDocument> MeterReadings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Cosmos DB container and partition key
            modelBuilder.Entity<MeterReadingDocument>()
                .ToContainer("MeterReadings")        // Container name
                .HasPartitionKey(m => m.MeterId)     // Partition key
                .HasKey(m => m.Id);                  // Id property

            base.OnModelCreating(modelBuilder);
        }
    }
}
