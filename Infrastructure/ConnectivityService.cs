using APIIntegration.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIIntegration.Infrastructure
{
    public class ConnectivityService : IConnectivityService
    {
        private int _online = 1;

        public bool IsOnline => _online == 1;
        public DateTime LastCheck { get; private set; } = DateTime.UtcNow;
        public void SetOnline()
        {
            Interlocked.Exchange(ref _online, 1);
            LastCheck = DateTime.UtcNow;
        }

        public void SetOffline()
        {
            Interlocked.Exchange(ref _online, 0);
            LastCheck = DateTime.UtcNow;
        }

    }
}
