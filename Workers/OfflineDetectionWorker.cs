
namespace APIIntegration.Services;

public class OfflineDetectionService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}