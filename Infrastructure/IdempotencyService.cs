namespace APIIntegration.Infrastructure;

using APIIntegration.Core;

public class IdempotencyService : IIdempotencyService
{
    public bool IsProcessed(string messageId)
    {
        throw new NotImplementedException();
    }

    public void MarkProcessed(string messageId)
    {
        throw new NotImplementedException();
    }
}