using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using APIIntegration.Core.Models;
using APIIntegration.Infrastructure;

namespace APIIntegration.Services
{
    public class OutboxService : BackgroundService
    {
        private readonly OutboxRepository repo;

        public OutboxService(OutboxRepository repo)
        {
            this.repo = repo;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var orderData = new OrderDto
            {
                Type = "Hello World!",
                Data = "42"
            };

            var message = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = "CreateOrder",
                Payload = JsonSerializer.Serialize(new OutboxEnvelope<OrderDto>
                {
                    Type = "CreateOrder",
                    Data = orderData
                })
            };

            await repo.AddAsync(message);
        }
    }

    class OrderDto
    {
        public string Type { get; set; } = null!;
        public string Data { get; set; } = null!;
    }
}