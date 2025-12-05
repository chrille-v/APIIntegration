using APIIntegration.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace APIIntegration.Workers
{
    public class OfflineDetectionWorker : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConnectivityService _connectivityService;
        private readonly ILogger<OfflineDetectionWorker> _logger;

        public OfflineDetectionWorker(IHttpClientFactory httpClientFactory,IConnectivityService connectivityService,ILogger<OfflineDetectionWorker> logger)
        {
            _httpClientFactory = httpClientFactory;
            _connectivityService = connectivityService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OfflineDetectionWorker started.");

            var client = _httpClientFactory.CreateClient("CloudApi");
            int consecutiveFailures = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                bool success;

                try
                {
                    // TODO: use real endpoint when we get API spec ("health")
                    var response = await client.GetAsync("health", stoppingToken);
                    success = response.IsSuccessStatusCode;
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    success = false;
                    _logger.LogWarning(ex, "Health check call failed.");
                }

                if (success)
                {
                    if (!_connectivityService.IsOnline)
                    {
                        _logger.LogInformation("Connectivity restored � switching to ONLINE.");
                    }

                    _connectivityService.SetOnline();
                    consecutiveFailures = 0;
                }
                else
                {
                    consecutiveFailures++;

                    if (consecutiveFailures >= 3 && _connectivityService.IsOnline)
                    {
                        _logger.LogWarning("Connectivity lost after {Failures} failed checks � switching to OFFLINE.",
                            consecutiveFailures);
                        _connectivityService.SetOffline();
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            _logger.LogInformation("OfflineDetectionWorker stopping.");
        }
    }
}
