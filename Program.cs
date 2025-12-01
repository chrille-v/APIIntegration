using APIIntegration;
using APIIntegration.Core;
using APIIntegration.Infrastructure;
using APIIntegration.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using APIIntegration.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<IntegrationDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB")));

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("Api"));
// builder.Services.AddHttpClient<ICloudClient, CloudClient>();
builder.Services.AddSingleton<ICloudClient, CloudClient>();
builder.Services.AddSingleton<ILanForwarder, LanForwarder>();
builder.Services.AddSingleton<ILocalCache, LocalCache>();
builder.Services.AddSingleton<IIdempotencyService, IdempotencyService>();

builder.Services.AddHostedService<CloudPollingService>();
builder.Services.AddHostedService<OfflineDetectionService>();
builder.Services.AddHostedService<ReplayService>();

var host = builder.Build();
host.Run();
