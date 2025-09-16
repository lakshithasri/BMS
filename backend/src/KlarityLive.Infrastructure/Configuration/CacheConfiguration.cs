using KlarityLive.Core.Security;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace KlarityLive.Infrastructure.Configuration
{
    public class CacheConfiguration
    {
        public static void Configure(IServiceCollection services, AppSecrets appSecrets)
        {
            services.AddTransient<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(appSecrets.RedisConnectionString);
            });
        }
    }
}
