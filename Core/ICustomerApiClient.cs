using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIIntegration.Core.Models;

namespace APIIntegration.Core
{
    public interface ICustomerApiClient
    {
        Task<ApiResult> SendAsync(OutboxMessage message);
    }
}