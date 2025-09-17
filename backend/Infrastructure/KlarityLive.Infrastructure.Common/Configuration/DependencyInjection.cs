using KlarityLive.Core.Common.Security;
using KlarityLive.Core.Common.Services.Concrete;
using KlarityLive.Core.Common.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace KlarityLive.Infrastructure.Common.Configuration
{
    public static class DependencyInjection
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //List<string> assemblyNames = ["KlarityLive.Domain", "KlarityLive.Application", "KlarityLive.Contracts", "KlarityLive.Core", "KlarityLive.Infrastructure"];
            //var workingOn = "";
            //foreach (var lib in assemblyNames)
            //{
            //    workingOn = lib;
            //    var assembly = Assembly.Load(lib);

            //    var interfaces = assembly.ExportedTypes.Where(x => x.IsInterface);
            //    foreach (var @interface in interfaces)
            //    {
            //        var implementation = assembly
            //            .ExportedTypes
            //            .FirstOrDefault(x => !x.IsInterface
            //                && !x.IsAbstract
            //                && x.Name.Equals(@interface.Name.Substring(1), StringComparison.CurrentCultureIgnoreCase));

            //        if (implementation != null)
            //        {
            //            services.AddTransient(@interface, implementation);
            //        }
            //    }
            //}

            services.AddTransient<IKeyVaultService, KeyVaultService>();

            var tempProvider = services.BuildServiceProvider();
            var keyVaultService = tempProvider.GetRequiredService<IKeyVaultService>();

            var appSecrets = new AppSecrets
            {
                DbConnectionString = keyVaultService.GetKeyAsync("DbConnectionString").GetAwaiter().GetResult()
                       ?? throw new ArgumentException("DbConnectionString could not be found"),

                RedisConnectionString = keyVaultService.GetKeyAsync("RedisConnectionString").GetAwaiter().GetResult()
                       ?? throw new ArgumentException("RedisConnectionString could not be found"),

                SendGridApiKey = keyVaultService.GetKeyAsync("SendGridApiKey").GetAwaiter().GetResult()
                       ?? throw new ArgumentException("SendGridApiKey could not be found"),

                BlobStorageConnectionString = keyVaultService.GetKeyAsync("BlobStorageConnectionString").GetAwaiter().GetResult()
                       ?? throw new ArgumentException("BlobStorageConnectionString could not be found"),
            };

            // Register AppSecrets as singleton instance
            services.AddSingleton(appSecrets);

            services.AddScoped<IDbConnection>(provider => new SqlConnection(appSecrets.DbConnectionString));

            CacheConfiguration.Configure(services, appSecrets);
            DatabaseConfiguration.Configure(services, appSecrets);
            SendGridConfiguration.Configure(services, appSecrets);
            BlobServiceClientConfiguration.Configure(services, appSecrets);
        }
    }
}
