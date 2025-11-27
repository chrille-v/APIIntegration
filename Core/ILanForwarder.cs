namespace APIIntegration.Core;

using APIIntegration.Core.Models;

public interface ILanForwarder
{
    Task<bool> ForwardJobAsync(Message message, CancellationToken cancellationToken);
}