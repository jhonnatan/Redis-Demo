using Microsoft.Extensions.Configuration;
using Redis_Demo.Service.Domain;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Redis_Demo.Service.Infrastructure
{
    public class LeadContext
    {
        private readonly string redisConnectionStr;

        public LeadContext()
        {
            redisConnectionStr = Environment.GetEnvironmentVariable("REDIS_CONN");
        }

        public void InsertLeads(List<Lead> leads)
        {
            var connection = ConnectionMultiplexer.Connect(redisConnectionStr);
            var dbRedis = connection.GetDatabase();

            foreach (var lead in leads)
            {
                dbRedis.StringSet($"id-{lead.Id}", JsonSerializer.Serialize(lead), expiry: null);
            }
        }
    }
}
