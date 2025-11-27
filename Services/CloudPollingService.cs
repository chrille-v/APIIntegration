namespace APIIntegration.Services;

using System.IO.Pipes;
using APIIntegration.Infrastructure;
using APIIntegration.Core;

public class CloudPollingService : BackgroundService
{
    private readonly ICloudClient _cloudClient;
    private readonly ILanForwarder _lanForwarder;
    private readonly ILogger<CloudPollingService> _logger;
    
    public CloudPollingService(CloudClient cloudClient, LanForwarder lanForwarder, ILogger<CloudPollingService> logger)
    {
        _cloudClient = cloudClient;
        _lanForwarder = lanForwarder;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}