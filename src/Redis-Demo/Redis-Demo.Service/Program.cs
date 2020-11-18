using Autofac;
using Redis_Demo.Service.Domain;
using Redis_Demo.Service.Infrastructure;
using Redis_Demo.Service.Modules;
using System;
using System.Collections.Generic;
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
            var leadContext = new LeadContext();
            leadContext.InsertLeads(leads);

            Console.WriteLine("Leads Inserted in Redis");


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
