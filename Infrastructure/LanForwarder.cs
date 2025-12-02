using APIIntegration.Config;
using APIIntegration.Core;
using APIIntegration.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

public class LanForwarder : ILanForwarder
{
    private readonly HttpClient _httpClient;
    private readonly LanSettings _lanSettings;
    private readonly ILogger<LanForwarder> _logger;

    public LanForwarder(HttpClient httpClient,IOptions<LanSettings> lanOptions,ILogger<LanForwarder> logger)
    {
        _httpClient = httpClient;
        _lanSettings = lanOptions.Value;
        _logger = logger;
    }

    public Task<bool> ForwardJobAsync(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}




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