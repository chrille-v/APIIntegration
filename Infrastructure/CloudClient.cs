namespace APIIntegration.Infrastructure;

using System.Threading;
using System.Threading.Tasks;
using APIIntegration.Core;
using APIIntegration.Core.Models;

public class CloudClient : ICloudClient
{
    public Task<T?> PollForJobsAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendUpdateAsync<T>(Message message, T payload, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}