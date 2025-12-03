using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIIntegration.Core.Models;

namespace APIIntegration.Infrastructure.Data
{
    public class IntegrationDbContext : DbContext
    {
        public IntegrationDbContext(DbContextOptions<IntegrationDbContext> options) : base(options)
        {
            
        }

        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    }
}