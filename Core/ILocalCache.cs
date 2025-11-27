namespace APIIntegration.Core;

using APIIntegration.Core.Models;

public interface ILocalCache
{
    Task SaveMessageAsync(Message message, CancellationToken cancellationToken);
    Task<Message?> GetMessageAsync(string messageId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Message>> GetPendingMessageAsync(CancellationToken cancellationToken);
    Task MarkAsSentAsync(string messageId, CancellationToken cancellationToken);
    Task MarkAsAcknowledgedAsync(string messageId, CancellationToken cancellationToken);
}