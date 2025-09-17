using KlarityLive.Application.Common.CommandHandlers;
using KlarityLive.Application.Common.Commands;
using KlarityLive.Application.Common.Queries;
using KlarityLive.Application.Common.QueryHandlers;
using KlarityLive.Core.Common.AutoMapper;
using KlarityLive.Core.Common.Security;
using KlarityLive.Core.Common.Services.Concrete;
using KlarityLive.Core.Common.Services.Interfaces;
using KlarityLive.Domain.Core.Entities.Base;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace KlarityLive.Infrastructure.Common.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName?.StartsWith("KlarityLive") == true))
            .AddClasses(classes => classes.Where(type =>
                !type.IsAbstract &&
                !type.IsInterface &&
                type.GetInterfaces().Any(i =>
                    i.Name.Equals($"I{type.Name}", StringComparison.OrdinalIgnoreCase))))
            .AsMatchingInterface()
            .WithTransientLifetime());

            services.AddAutoMapper(options =>
            {
                options.AddProfile(typeof(MappingProfile));
            });

            AddGenericCrudHandlers(services);

            services.AddTransient<IKeyVaultService, KeyVaultService>();

            services.AddAutoMapper(options =>
            {
                options.AddProfile(typeof(MappingProfile));
            });

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
            services.AddHttpClient();
            services.AddScoped<IDbConnection>(provider => new SqlConnection(appSecrets.DbConnectionString));
            services.AddScoped<IGoogleRecaptchaService, GoogleRecaptchaService>();

            CacheConfiguration.Configure(services, appSecrets);
            DatabaseConfiguration.Configure(services, appSecrets);
            SendGridConfiguration.Configure(services, appSecrets);
            BlobServiceClientConfiguration.Configure(services, appSecrets);

            return services;
        }

        public static void AddGenericCrudHandlers(IServiceCollection services)
        {
            // Get all entity types from the domain assembly
            var entityTypes = typeof(BaseEntity).Assembly.GetTypes()
                .Where(t => !t.IsAbstract &&
                           !t.IsInterface &&
                           typeof(BaseEntity).IsAssignableFrom(t))
                .ToList();

            foreach (var entityType in entityTypes)
            {
                // Create command handler
                var createCommandType = typeof(CreateCommand<>).MakeGenericType(entityType);
                var createHandlerType = typeof(GenericCreateCommandHandler<>).MakeGenericType(entityType);
                var createRequestHandlerType = typeof(IRequestHandler<,>).MakeGenericType(createCommandType, entityType);
                services.AddScoped(createRequestHandlerType, createHandlerType);

                // Update command handler
                var updateCommandType = typeof(UpdateCommand<>).MakeGenericType(entityType);
                var updateHandlerType = typeof(GenericUpdateCommandHandler<>).MakeGenericType(entityType);
                var updateRequestHandlerType = typeof(IRequestHandler<,>).MakeGenericType(updateCommandType, entityType);
                services.AddScoped(updateRequestHandlerType, updateHandlerType);

                // Delete command handler
                var deleteCommandType = typeof(DeleteCommand<>).MakeGenericType(entityType);
                var deleteHandlerType = typeof(GenericDeleteCommandHandler<>).MakeGenericType(entityType);
                var deleteRequestHandlerType = typeof(IRequestHandler<,>).MakeGenericType(deleteCommandType, typeof(bool));
                services.AddScoped(deleteRequestHandlerType, deleteHandlerType);

                // Get by id query handler
                var getByIdQueryType = typeof(GetByIdQuery<>).MakeGenericType(entityType);
                var getByIdHandlerType = typeof(GenericGetByIdQueryHandler<>).MakeGenericType(entityType);
                var getByIdRequestHandlerType = typeof(IRequestHandler<,>).MakeGenericType(getByIdQueryType, entityType);
                services.AddScoped(getByIdRequestHandlerType, getByIdHandlerType);

                // Get all query handler
                var getAllQueryType = typeof(GetAllQuery<>).MakeGenericType(entityType);
                var getAllHandlerType = typeof(GenericGetAllQueryHandler<>).MakeGenericType(entityType);
                var enumerableType = typeof(IEnumerable<>).MakeGenericType(entityType);
                var getAllRequestHandlerType = typeof(IRequestHandler<,>).MakeGenericType(getAllQueryType, enumerableType);
                services.AddScoped(getAllRequestHandlerType, getAllHandlerType);

                Console.WriteLine($"Registered generic handlers for: {entityType.Name}");
            }
        }
    }
}
