namespace APIIntegration.Core.Models;

using System.Threading.Tasks.Dataflow;

public class Message
{
    public string MessageId { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTime LastUpdate { get; set; }
    public MessageStatus Status { get; set; }
}