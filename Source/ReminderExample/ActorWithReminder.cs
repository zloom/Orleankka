using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleankka;

namespace ReminderExample
{   
    [Serializable]
    public class InitReminder
    {}

    [Serializable]
    public class CheckReminder
    {}

    public class ActorWithReminder : Actor
    {
        Task On(InitReminder x) => this.Reminders.Register("someReminder", TimeSpan.Zero, TimeSpan.FromSeconds(65.0));

        Task<bool> On(CheckReminder x) => this.Reminders.IsRegistered("someReminder");

        public override Task OnReminder(string id) => Task.FromResult("Done!");
    }
}
