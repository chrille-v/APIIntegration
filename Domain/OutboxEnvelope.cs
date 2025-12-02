using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIIntegration.Domain
{
    public class OutboxEnvelope<T>
    {
        public string Type { get; set; } = null!;
        public T Data { get; set; } = default!;
    }
}