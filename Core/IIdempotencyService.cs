namespace APIIntegration.Core;

public interface IIdempotencyService
{
    /// <summary>
    /// Checks whether a message with this ID has alreaady been fully processed.
    /// </summary>
    /// <param name="messageId"></param>
    /// <returns></returns>
    bool IsProcessed(string messageId);

    /// <summary>
    /// Marks the message as processed.
    /// </summary>
    /// <param name="messageId"></param>
    void MarkProcessed(string messageId);
}