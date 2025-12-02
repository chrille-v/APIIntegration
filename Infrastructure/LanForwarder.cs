using APIIntegration.Core;
using APIIntegration.Core.Models;

public class LanForwarder : ILanForwarder
{
    public Task<bool> ForwardJobAsync(Message message, CancellationToken cancellationToken)
    {
        // TODO:
        // 1. Read LAN endpoint base URL from configuration (ApiSettings / LanSettings)
        // 2. Build final LAN endpoint URL based on message type (e.g., /lan/job/update or /lan/job/status)
        // 3. Serialize the message.Payload to JSON
        // 4. Send the HTTP POST request using HttpClient (registered via DI)
        // 5. Handle non-successful responses:
        //    - Log error details
        //    - Return false so the retry logic can handle it
        // 6. On success:
        //    - Return true to signal completion
        // 7. Cancellation:
        //    - Respect the CancellationToken
        // 8. Replace placeholder implementation once real LAN API spec arrives

        throw new NotImplementedException();
    }
}
