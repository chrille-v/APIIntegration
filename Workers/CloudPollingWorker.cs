namespace APIIntegration.Services;

using System.IO.Pipes;
using APIIntegration.Infrastructure;
using APIIntegration.Core;

public class CloudPollingWorker : BackgroundService
{
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<CloudPollingWorker> logger;
    private readonly int batchSize;
    private readonly int delayMs;

    public CloudPollingWorker(ILogger<CloudPollingWorker> logger, IConfiguration config, IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        batchSize = config.GetValue<int>("CloudWorker:BatchSize");
        delayMs = config.GetValue<int>("CloudWorker:PollIntervals");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Cloud Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
            var client = scope.ServiceProvider.GetRequiredService<ICustomerApiClient>();

            var messages = await repo.GetPendingAsync(batchSize);

            if (messages.Count > 0)
                logger.LogInformation("Found {count} messages in outbox", messages.Count);

            foreach (var msg in messages)
            {
                try
                {
                    var result = await client.SendAsync(msg);

                    if (result.Success)
                    {
                        await repo.MarkAsSentAsync(msg.Id);
                        logger.LogInformation("Send: {id}", msg.Id);
                    }
                    else
                    {
                        await repo.MarkAsFailedAsync(msg.Id, result.Error);
                        logger.LogWarning("Failed: {id} - {err}", msg.Id, result.Error);
                    }
                }
                catch (Exception ex)
                {
                    await repo.IncrementRetryAsync(msg.Id);
                    logger.LogError(ex, "Error while sending: {id}", msg.Id);
                }
            }

            await Task.Delay(delayMs, stoppingToken);
        }

        logger.LogInformation("Cloud Worker shutting down");
    }
}