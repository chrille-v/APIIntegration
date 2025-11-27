namespace APIIntegration.Core;

public interface IReplayService
{
    Task ReplayPendingMessageAsync(CancellationToken cancellationToken);
}