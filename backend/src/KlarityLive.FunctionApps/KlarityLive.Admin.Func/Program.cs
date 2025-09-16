using KlarityLive.Application.Features.Base.CommandHandlers;
using KlarityLive.Application.Features.Base.QueryHandlers;
using KlarityLive.Application.Features.Base.Services;
using KlarityLive.Infrastructure.Configuration;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

DependencyInjection.RegisterServices(builder.Services);

builder.Services.AddScoped<IQueryBuilder, SqlServerQueryBuilder>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GenericCreateCommandHandler<>).Assembly);
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GenericUpdateCommandHandler<>).Assembly);
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GenericDeleteCommandHandler<>).Assembly);
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GenericGetByIdQueryHandler<>).Assembly);
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GenericGetAllQueryHandler<>).Assembly);
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GenericGetCountQueryHandler<>).Assembly);
});

builder.Build().Run();
