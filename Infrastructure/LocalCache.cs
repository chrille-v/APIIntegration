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


    public Task<Message?> GetNextLanMessageAsync(CancellationToken cancellationToken)
    {
        // TODO: SELECT * FROM MessagesWHERE Status = 0  ORDER BY LastUpdate LIMIT 1
        throw new NotImplementedException();
    }

    public Task IncrementRetryAsync(string messageId, CancellationToken cancellationToken)
    {
        // TODO: UPDATE Messages SET RetryCount = RetryCount + 1 WHERE MessageId = @id
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