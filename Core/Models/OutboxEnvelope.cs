using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIIntegration.Core.Models
{
    public class OutboxEnvelope<T>
    {
        public string Type { get; set; } = "";
        public T Data { get; set; } = default!;
    }
}