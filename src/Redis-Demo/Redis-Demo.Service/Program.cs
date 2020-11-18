using Autofac;
using Redis_Demo.Service.Domain;
using Redis_Demo.Service.Infrastructure;
using Redis_Demo.Service.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Redis_Demo.Service
{
    class Program
    {
        private static readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        static void Main(string[] args)
        {
            Console.WriteLine("Service Started");
            var container = RegisterContainers();
            
            Console.WriteLine("Preparing Leads to Insert");
            var leads = new List<Lead>()
            {
                new Lead(Guid.NewGuid(), "111111111", "Customer1", 32, "11 5566-7788"),
                new Lead(Guid.NewGuid(), "222222222", "Customer1", 43, "11 4321-7766"),
                new Lead(Guid.NewGuid(), "333333333", "Customer1", 55, "11 98789-1243"),
            };

            // Inserting in Redis
            var leadContext = new LeadContext();
            leadContext.InsertLeads(leads);
            Console.WriteLine("Leads Inserted in Redis");

            // Get value by key in Redis
            var firstLeadInserted = leadContext.GetLeadByKey($"id-{leads.First().Id}");
            Console.WriteLine($"First lead Inserted: {firstLeadInserted}");

            // Get values by keys in Redis
            var allLeads = leadContext.GetLeadsByKeys(leads.Select(s => $"id-{s.Id}").ToList());
            Console.WriteLine("All Leads inserted:");
            allLeads.ForEach(s => Console.WriteLine(s));

            AppDomain.CurrentDomain.ProcessExit += (o, e) =>
            {                
                Console.WriteLine("Terminating...");
                autoResetEvent.Set();
            };

            autoResetEvent.WaitOne();
        }

        private static IContainer RegisterContainers()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<SeriveModule>();
            return builder.Build();

        }
    }
}
