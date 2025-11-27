namespace APIIntegration.Services;

using System.IO.Pipes;
using APIIntegration.Infrastructure;

public class CloudPollingService : BackgroundService
{
    private readonly CloudClient _cloudClient;
    private readonly ILogger<CloudPollingService> _logger;

    public CloudPollingService(CloudClient cloudClient, ILogger<CloudPollingService> logger)
    {
        _cloudClient = cloudClient;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}