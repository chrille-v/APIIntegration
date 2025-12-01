using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIIntegration.Core;
using APIIntegration.Data;

namespace APIIntegration.Infrastructure
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly IntegrationDbContext db;
        public OutboxRepository(IntegrationDbContext db)
        {
            this.db = db;
        }

        public Task AddAsync(OutboxMessage message)
        {
            throw new NotImplementedException();
        }

        public Task<List<OutboxMessage>> GetPendingAsync(int maxBatch)
        {
            throw new NotImplementedException();
        }

        public Task IncrementRetryAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task MarkAsFailedAsync(Guid id, string error)
        {
            throw new NotImplementedException();
        }

        public Task MarkAsSentAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}