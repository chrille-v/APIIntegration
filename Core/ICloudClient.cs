namespace APIIntegration.Core;

using APIIntegration.Core.Models;

public interface ICloudClient
{
    Task<T?> PollForJobsAsync<T>(string path, CancellationToken cancellationToken = default);
    Task<bool> SendUpdateAsync<T>(Message message, T payload, CancellationToken cancellationToken = default);
}