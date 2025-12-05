using APIIntegration;
using APIIntegration.Config;
using APIIntegration.Core;
using APIIntegration.Infrastructure;
using APIIntegration.Infrastructure.Data;
using APIIntegration.Services;
using APIIntegration.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<IntegrationDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB")));

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("Api"));

//registration of LAN services
builder.Services.Configure<LanSettings>(builder.Configuration.GetSection("Lan"));

//registration of Database settings
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("Database"));

// HttpClient for LAN forwarder
builder.Services.AddHttpClient<ILanForwarder, LanForwarder>();

// HttpClient for cloud API health checks
builder.Services.AddHttpClient("CloudApi", client =>
{
    var apiSettings = builder.Configuration.GetSection("Api");
    client.BaseAddress = new Uri(apiSettings["BaseUrl"] ?? "https://api.example.com");
    client.Timeout = TimeSpan.FromSeconds(5); // short timeout for health checks
});

builder.Services.AddSingleton<ICloudClient, CloudClient>();
builder.Services.AddSingleton<ILocalCache, LocalCache>();
builder.Services.AddSingleton<IIdempotencyService, IdempotencyService>();

builder.Services.AddScoped<IReplayService, ReplayService>();
builder.Services.AddSingleton<IConnectivityService, ConnectivityService>();


builder.Services.AddHostedService<CloudPollingWorker>();
builder.Services.AddHostedService<LanWorkerService>();
builder.Services.AddHostedService<OfflineDetectionWorker>();

// Registrating the outbox pattern cycle
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddScoped<ICustomerApiClient, MockCustomerApiClient>();

var host = builder.Build();
host.Run();
