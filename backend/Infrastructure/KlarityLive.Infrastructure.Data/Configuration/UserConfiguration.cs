using KlarityLive.Domain.Core.Entities.Amin;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Configuration
{
    public static class UserConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User", "Admin").HasKey(a => a.Id);
        }
    }
}
