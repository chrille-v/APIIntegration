using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIIntegration.Core.Models;

namespace APIIntegration.Core
{
    public interface IOutboxRepository
    {
        Task AddAsync(OutboxMessage message);
        Task<List<OutboxMessage>> GetPendingAsync(int maxBatch);
        Task MarkAsSentAsync(Guid id);
        Task MarkAsFailedAsync(Guid id, string error);
        Task IncrementRetryAsync(Guid id);
    }
}