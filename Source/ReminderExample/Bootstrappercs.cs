using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleankka.Cluster;

namespace ReminderExample
{
    public class Bootstrapper: IBootstrapper
    {
        public Task Run(object properties)
        {
            return ClusterActorSystem.Current.ActorOf(typeof(ActorWithReminder), "Any").Tell(new InitReminder());
        }
    }
}
