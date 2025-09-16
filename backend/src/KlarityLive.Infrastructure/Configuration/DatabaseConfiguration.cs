﻿using KlarityLive.Core.Security;
using KlarityLive.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KlarityLive.Infrastructure.Configuration
{
    public static class DatabaseConfiguration
    {
        public static void Configure(IServiceCollection services, AppSecrets appSecrets)
        {
            try
            {
                services.AddDbContext<KlarityLiveDbContext>(
                   (sp, options) =>
                   {
                       options.UseSqlServer(appSecrets.DbConnectionString, providerOptions =>
                       {
                           providerOptions.EnableRetryOnFailure();
                           providerOptions.CommandTimeout(180);
                       })
                       .LogTo(Console.WriteLine, LogLevel.Error)
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
