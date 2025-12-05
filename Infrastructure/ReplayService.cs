using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using APIIntegration.Core;
using APIIntegration.Core.Models;

namespace APIIntegration.Infrastructure
{
    public class ReplayService : IReplayService
    {
        private readonly IOutboxRepository _outbox;
        private readonly ICustomerApiClient _api;
        private readonly ILogger<ReplayService> _logger;

        public ReplayService(IOutboxRepository outbox,ICustomerApiClient api,ILogger<ReplayService> logger)
        {
            _outbox = outbox;
            _api = api;
            _logger = logger;
        }

        public async Task ReplayPendingMessageAsync(CancellationToken cancellationToken)
        {
            var pending = await _outbox.GetPendingAsync(maxBatch: 20);

            if (pending.Count == 0)
                return;

            foreach (var msg in pending)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    _logger.LogInformation("[Replay] Sending message {Id} (Retry={Retry})",
                        msg.Id, msg.RetryCount);

                    var result = await _api.SendAsync(msg);

                    if (result.Success)
                    {
                        _logger.LogInformation("[Replay] Message {Id} sent successfully.", msg.Id);
                        await _outbox.MarkAsSentAsync(msg.Id);
                    }
                    else
                    {
                        _logger.LogWarning("[Replay] Message {Id} failed: {Error}",
                            msg.Id, result.Error);

                        await _outbox.MarkAsFailedAsync(msg.Id, result.Error ?? "Unknown error");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[Replay] Exception when sending message {Id}", msg.Id);
                    await _outbox.IncrementRetryAsync(msg.Id);
                }
            }
        }
    }
}
