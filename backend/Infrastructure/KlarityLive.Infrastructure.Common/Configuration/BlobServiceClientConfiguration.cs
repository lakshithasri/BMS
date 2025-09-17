using Azure.Storage.Blobs;
using KlarityLive.Core.Common.Security;
using Microsoft.Extensions.DependencyInjection;

namespace KlarityLive.Infrastructure.Common.Configuration
{
    public static class BlobServiceClientConfiguration
    {
        public static void Configure(IServiceCollection services, AppSecrets appSecrets)
        {
            services.AddSingleton(x =>
            {
                return new BlobServiceClient(appSecrets.BlobStorageConnectionString);
            });

            services.AddSingleton(x =>
            {
                return new BlobContainerClient(appSecrets.BlobStorageConnectionString, "htmltemplates");
            });
        }
    }
}
