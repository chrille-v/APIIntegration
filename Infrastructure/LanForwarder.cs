namespace APIIntegration.Infrastructure;

using APIIntegration.Core;
using APIIntegration.Core.Models;

public class LanForwarder : ILanForwarder
{
    public Task<bool> ForwardJobAsync(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}