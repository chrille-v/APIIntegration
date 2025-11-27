using APIIntegration;
using APIIntegration.Core;
using APIIntegration.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("Api"));
builder.Services.AddHttpClient<ICloudClient, CloudClient>();
builder.Services.AddSingleton<ILanForwarder, LanForwarder>();
builder.Services.AddSingleton<ILocalCache, LocalCache>();
builder.Services.AddSingleton<IIdempotencyService, IdempotencyService>();

var host = builder.Build();
host.Run();
