using APIIntegration.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIIntegration.Services
{
    public class LanWorkerService : BackgroundService
    {
        private readonly ILanForwarder _lanForwarder;
        private readonly ILocalCache _localCache;
        private readonly ILogger<LanWorkerService> _logger;

        public LanWorkerService(ILanForwarder lanForwarder, ILocalCache localCache, ILogger<LanWorkerService> logger)
        {
            _lanForwarder = lanForwarder;
            _localCache = localCache;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("LanWorkerService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //fetch next LAN -> cloud message from local DB
                    var message = await _localCache.GetNextLanMessageAsync(stoppingToken);

                    //if no pending messages, wait a bit and continue
                    if (message == null)
                    {
                        await Task.Delay(500, stoppingToken);
                        continue;
                    }

                    //try forwarding message through LAN
                    bool ok = await _lanForwarder.ForwardJobAsync(message, stoppingToken);

                    //update status based on result
                    if (ok)
                    {
                        await _localCache.MarkAsSentAsync(message.MessageId, stoppingToken);
                        _logger.LogInformation("Message forwarded and marked as Sent. ID={MessageId}", message.MessageId);
                    }
                    else
                    {
                        await _localCache.IncrementRetryAsync(message.MessageId, stoppingToken);
                        _logger.LogWarning("LAN forward failed. MessageId={MessageId}, retry count increased.", message.MessageId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in LAN worker loop.");
                }

                await Task.Delay(200, stoppingToken);
            }
        }



    }
}
