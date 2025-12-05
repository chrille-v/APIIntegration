using APIIntegration.Config;
using APIIntegration.Core;
using APIIntegration.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text;

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


    //depends on message type(update or status) to resolve endpoint
    private string ResolveEndpoint(Message message)
    {
        return message.Type switch
        {
            MessageType.JobUpdate => _lanSettings.JobUpdateEndpoint,
            MessageType.JobStatus => _lanSettings.JobStatusEndpoint,
            _ => throw new ArgumentOutOfRangeException(nameof(message.Type))
        };
    }

    public async Task<bool> ForwardJobAsync(Message message, CancellationToken cancellationToken)
    {
        var endpoint = ResolveEndpoint(message);
        var url = $"{_lanSettings.BaseUrl}{endpoint}";

        var content = new StringContent(message.Payload, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync(url, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("LAN forward failed: {Status} - MessageId={MessageId}",response.StatusCode, message.MessageId);
                return false;
            }

            _logger.LogInformation("Lan forward successful. MessageId={MessageId}", message.MessageId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error forwarding message to LAN. MessageId={MessageId}", message.MessageId);
            return false;
        }
    }
}
