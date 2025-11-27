namespace APIIntegration.Services;

using System.IO.Pipes;
using APIIntegration.Infrastructure;
using APIIntegration.Core;

public class CloudPollingService : BackgroundService
{
    private readonly ICloudClient cloudClient;
    private readonly ILanForwarder lanForwarder;
    private readonly ILogger<CloudPollingService> logger;
    
    public CloudPollingService(ICloudClient cloudClient, ILanForwarder lanForwarder, ILogger<CloudPollingService> logger)
    {
        this.cloudClient = cloudClient;
        this.lanForwarder = lanForwarder;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("CloudPollingService is running as intended...");

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
        // throw new NotImplementedException();
    }
}