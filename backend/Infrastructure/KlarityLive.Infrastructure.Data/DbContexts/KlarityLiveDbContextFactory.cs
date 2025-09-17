using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace KlarityLive.Infrastructure.Data.DbContexts
{
    public class KlarityLiveDbContextFactory : IDesignTimeDbContextFactory<KlarityLiveDbContext>
    {
        public KlarityLiveDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<KlarityLiveDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Server=localhost;Database=KlarityLive;Trusted_Connection=true;TrustServerCertificate=true;";

            builder.UseSqlServer(connectionString);

            return new KlarityLiveDbContext(builder.Options);
        }
    }
}
