using Azure.Storage.Blobs;
using KlarityLive.Core.Security;
using Microsoft.Extensions.DependencyInjection;

namespace KlarityLive.Infrastructure.Configuration
{
    public static class BlobServiceClientConfiguration
    {
        public static void Configure(IServiceCollection services, AppSecrets appSecrets)
        {
            services.AddSingleton(x =>
            {
                return new BlobServiceClient(appSecrets.BlobStorageConnectionString);
            });
        }
    }
}
