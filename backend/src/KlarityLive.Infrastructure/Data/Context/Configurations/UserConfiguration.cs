using KlarityLive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KlarityLive.Infrastructure.Data.Context.Configurations
{
    public static class UserConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User", "Admin").HasKey(a => a.Id);
        }
    }
}
