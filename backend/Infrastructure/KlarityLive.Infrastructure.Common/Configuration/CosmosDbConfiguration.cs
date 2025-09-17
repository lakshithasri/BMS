using KlarityLive.Core.Common.Security;
using KlarityLive.Infrastructure.Data.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore; // Add this using directive
using Microsoft.Extensions.Logging;   // Add this using directive
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlarityLive.Infrastructure.Common.Configuration
{
    public static class CosmosDbConfiguration
    {
        public static void Configure(IServiceCollection services, AppSecrets appSecrets)
        {
            try
            {
                services.AddDbContext<KlarityLiveCosmosDbContext>(
                    (sp, options) =>
                    {
                        var connectionString = appSecrets.CosmosDbConnectionString;
                        var databaseName = "KlarityLiveDb"; // or get from configuration

                        options.UseCosmos(connectionString, databaseName)
                            .LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging();
                    }, ServiceLifetime.Scoped
                );
            }
            catch
            {
                throw;
            }
        }
    }
}
