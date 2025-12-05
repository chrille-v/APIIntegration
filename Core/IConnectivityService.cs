using System;
using System.Collections.Generic;
using System.Text;

namespace APIIntegration.Core
{
    public interface IConnectivityService
    {
        bool IsOnline { get; }
        DateTime LastCheck { get; }

        void SetOnline();
        void SetOffline();
    }
}
