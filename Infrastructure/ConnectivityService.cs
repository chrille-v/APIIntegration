using APIIntegration.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIIntegration.Infrastructure
{
   public class ConnectivityService : IConnectivityService
{
    private int _online = 1;
    private long _lastCheckTicks = DateTime.UtcNow.Ticks;

    public bool IsOnline => _online == 1;
    public DateTime LastCheck => new DateTime(Interlocked.Read(ref _lastCheckTicks));

    public void SetOnline()
    {
        Interlocked.Exchange(ref _online, 1);
        Interlocked.Exchange(ref _lastCheckTicks, DateTime.UtcNow.Ticks);
    }

    public void SetOffline()
    {
        Interlocked.Exchange(ref _online, 0);
        Interlocked.Exchange(ref _lastCheckTicks, DateTime.UtcNow.Ticks);
    }
}
}
