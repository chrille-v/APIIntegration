using APIIntegration;
using APIIntegration.Config;
using APIIntegration.Core;
using APIIntegration.Infrastructure;
using APIIntegration.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("Api"));

//registration of LAN services
builder.Services.Configure<LanSettings>(builder.Configuration.GetSection("Lan"));

// HttpClient for LAN forwarder
builder.Services.AddHttpClient<ILanForwarder, LanForwarder>();

builder.Services.AddSingleton<ICloudClient, CloudClient>();
builder.Services.AddSingleton<ILocalCache, LocalCache>();
builder.Services.AddSingleton<IIdempotencyService, IdempotencyService>();

builder.Services.AddHostedService<CloudPollingService>();
builder.Services.AddHostedService<OfflineDetectionService>();
builder.Services.AddHostedService<ReplayService>();

var host = builder.Build();
host.Run();
