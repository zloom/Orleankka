﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Orleankka;
using Orleankka.Cluster;

using Orleans.Runtime.Configuration;

namespace ReminderExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running demo. Booting cluster might take some time ...\n");
            var assembly = Assembly.GetExecutingAssembly();

            var config = new ClusterConfiguration().LoadFromEmbeddedResource<Program>("Server.xml");

            var system = ActorSystem.Configure()
                .Cluster()
                .From(config)
                .Register(assembly)
             //   .Run<Bootstrapper>()
                .Done();

            Console.WriteLine("Finished booting cluster...");
            Console.ReadLine();

            system.Dispose();
        }
    }
}
