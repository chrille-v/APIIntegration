using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIIntegration.Core;
using APIIntegration.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace APIIntegration.Infrastructure
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly IntegrationDbContext db;
        public OutboxRepository(IntegrationDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(OutboxMessage message)
        {
            db.OutboxMessages.Add(message);
            await db.SaveChangesAsync();
        }

        public Task<List<OutboxMessage>> GetPendingAsync(int maxBatch)
        {
            return db.OutboxMessages
                .Where(x => x.Status == "Pending")
                .OrderBy(x => x.CreatedAt)
                .Take(maxBatch)
                .ToListAsync();
        }

        public async Task IncrementRetryAsync(Guid id)
        {
            var msg = await db.OutboxMessages.FindAsync(id);
            if (msg == null) return;

            msg.RetryCount++;
            msg.LastAttemptAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }

        public async Task MarkAsFailedAsync(Guid id, string error)
        {
            var msg = await db.OutboxMessages.FindAsync(id);

            if (msg == null) return;

            msg.Status = "Failed";
            msg.LastAttemptAt = DateTime.UtcNow;
            msg.RetryCount ++;
            await db.SaveChangesAsync();
        }

        public async Task MarkAsSentAsync(Guid id)
        {
            var msg = await db.OutboxMessages.FindAsync(id);

            if (msg == null) return;

            msg.Status = "Sent";
            msg.LastAttemptAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
    }
}