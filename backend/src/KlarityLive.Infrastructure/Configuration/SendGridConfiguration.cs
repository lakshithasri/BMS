using KlarityLive.Core.Security;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;

namespace KlarityLive.Infrastructure.Configuration
{
    public static class SendGridConfiguration
    {
        public static void Configure(IServiceCollection services, AppSecrets appSecrets)
        {
            services.AddSingleton<ISendGridClient>(sp =>
            {
                return new SendGridClient(appSecrets.SendGridApiKey);
            });
        }
    }
}
