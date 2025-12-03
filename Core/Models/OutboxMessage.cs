using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIIntegration.Core.Models
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = "";
        public string Payload { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime? LastAttemptAt { get; set; }
        public int RetryCount { get; set; }
        public string Status { get; set; } = null!;
    }
}