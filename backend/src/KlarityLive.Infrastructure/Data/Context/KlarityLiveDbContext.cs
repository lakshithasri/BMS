using KlarityLive.Domain.Entities;
using KlarityLive.Infrastructure.Data.Context.Configurations;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Context
{
    public class KlarityLiveDbContext(DbContextOptions<KlarityLiveDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UserConfiguration.Configure(modelBuilder);
        }
    }
}
