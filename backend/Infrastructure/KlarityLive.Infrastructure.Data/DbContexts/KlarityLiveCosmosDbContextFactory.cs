using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlarityLive.Infrastructure.Data.DbContexts
{
    public class KlarityLiveCosmosDbContextFactory : IDesignTimeDbContextFactory<KlarityLiveCosmosDbContext>
    {
        public KlarityLiveCosmosDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<KlarityLiveCosmosDbContext>();

            var connectionString = configuration.GetConnectionString("CosmosDbConnection")
                ?? "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

            var databaseName = configuration["CosmosDb:DatabaseName"] ?? "KlarityLiveDb";

            builder.UseCosmos(connectionString, databaseName);

            return new KlarityLiveCosmosDbContext(builder.Options);
        }
    }
}
