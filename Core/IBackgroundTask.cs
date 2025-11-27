namespace APIIntegration.Core;

public interface IBackgroundTask
{
    Task RunAsync(CancellationToken cancellationToken);
}