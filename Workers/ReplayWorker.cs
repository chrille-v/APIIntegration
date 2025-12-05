using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using APIIntegration.Core;

namespace APIIntegration.Workers
{
    public class ReplayWorker : BackgroundService
    {
        private readonly IConnectivityService _connectivity;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ReplayWorker> _logger;

        public ReplayWorker(IConnectivityService connectivity,IServiceScopeFactory scopeFactory,ILogger<ReplayWorker> logger)
        {
            _connectivity = connectivity;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[ReplayWorker] Starting replay engine...");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!_connectivity.IsOnline)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                    continue;
                }

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var replayService = scope.ServiceProvider.GetRequiredService<IReplayService>();

                    await replayService.ReplayPendingMessageAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[ReplayWorker] Replay failed with exception.");
                }

                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }
    }
}
