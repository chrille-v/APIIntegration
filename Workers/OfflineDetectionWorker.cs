
namespace APIIntegration.Workers;

public class OfflineDetectionWorker : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}