using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIIntegration.Core;
using APIIntegration.Core.Models;

namespace APIIntegration.Infrastructure
{
    public class MockCustomerApiClient : ICustomerApiClient
    {
        public Task<ApiResult> SendAsync(OutboxMessage message)
        {
            Console.WriteLine($"[MOCK API] Sends: {message.Type} (Id: {message.Id})");

            return Task.FromResult(ApiResult.Ok());
        }
    }
}