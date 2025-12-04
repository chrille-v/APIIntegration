using APIIntegration;
using APIIntegration.Config;
using APIIntegration.Core;
using APIIntegration.Infrastructure;
using APIIntegration.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using APIIntegration.Infrastructure.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<IntegrationDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB")));

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("Api"));

//registration of LAN services
builder.Services.Configure<LanSettings>(builder.Configuration.GetSection("Lan"));

//registration of Database settings
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("Database"));

// HttpClient for LAN forwarder
builder.Services.AddHttpClient<ILanForwarder, LanForwarder>();

builder.Services.AddSingleton<ICloudClient, CloudClient>();
builder.Services.AddSingleton<ILocalCache, LocalCache>();
builder.Services.AddSingleton<IIdempotencyService, IdempotencyService>();

builder.Services.AddHostedService<CloudPollingWorker>();
builder.Services.AddHostedService<LanWorkerService>();
// TODO Phase 4-6: Enable when OfflineDetectionService and ReplayService are implemented
// builder.Services.AddHostedService<OfflineDetectionService>();
// builder.Services.AddHostedService<ReplayService>();

// Registrating the outbox pattern cycle
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddScoped<ICustomerApiClient, MockCustomerApiClient>();

var host = builder.Build();
host.Run();
