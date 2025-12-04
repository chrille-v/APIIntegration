using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIIntegration.Config
{
    public class LanSettings
    {
        public string BaseUrl { get; set; } = null!;
        public string JobUpdateEndpoint { get; set; } = null!;
        public string JobStatusEndpoint { get; set; } = null!;
        public string SharedKey { get; set; } = null!;
    }
    
}