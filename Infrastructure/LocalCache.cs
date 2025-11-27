namespace APIIntegration.Infrastructure;

using APIIntegration.Core;
using APIIntegration.Core.Models;

class LocalCache : ILocalCache
{
    public Task<Message?> GetMessageAsync(string messageId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Message>> GetPendingMessageAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task MarkAsAcknowledgedAsync(string messageId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task MarkAsSentAsync(string messageId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SaveMessageAsync(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}