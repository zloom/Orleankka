using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleankka;
using Orleankka.Client;

using Orleans.Runtime.Configuration;

namespace ReminderExample.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            var config = new ClientConfiguration()
                .LoadFromEmbeddedResource(typeof(Program), "Client.xml");

            var system = ActorSystem.Configure()
                .Client()
                .From(config)
                .Register(typeof(ActorWithReminder).Assembly)
                .Done();


            Console.WriteLine("Started");

            while (true)
            {
                Console.WriteLine("Press c key to check reminder or r to register");
                var k = Console.ReadKey();
                if (k.KeyChar == 'c')
                {
                    system.ActorOf<ActorWithReminder>("Any").Ask<bool>(new CheckReminder())
                          .ContinueWith(p => Console.WriteLine("Reminder check result: {0}", p.Result));
                }

                if (k.KeyChar == 'r')
                {
                    system.ActorOf<ActorWithReminder>("Any").Tell(new InitReminder())
                         .ContinueWith(p => Console.WriteLine("Reminder init result: {0}", p.Status));
                }

            }
        }
    }
}
