namespace APIIntegration.Core;

using APIIntegration.Core.Models;

public interface ILocalCache
{
    Task SaveMessageAsync(Message message, CancellationToken cancellationToken);
    Task<Message?> GetMessageAsync(string messageId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Message>> GetPendingMessageAsync(CancellationToken cancellationToken);

    // Fetch next LAN message waiting to be sent (Status = Pending)
    Task<Message?> GetNextLanMessageAsync(CancellationToken cancellationToken);

    // Increase retry counter
    Task IncrementRetryAsync(string messageId, CancellationToken cancellationToken);

    Task MarkAsSentAsync(string messageId, CancellationToken cancellationToken);
    Task MarkAsAcknowledgedAsync(string messageId, CancellationToken cancellationToken);

}