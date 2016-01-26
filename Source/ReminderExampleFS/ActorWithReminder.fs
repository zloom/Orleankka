namespace ReminderExample

    open Orleankka
    open System


    type check ={
        Id: string
    }

    type registr ={
        Id:string
    }

    type Scheduler() =
        inherit Actor()   
              
        override this.OnReminder (id: string) =
            base.OnReminder(id)      
       

        member this.On (check: check) = 
            this.Reminders.IsRegistered(check.Id)              
           

        member this.On (register: registr) =
            this.Reminders.Register(register.Id, TimeSpan.Zero, TimeSpan.FromSeconds(65.0))              
           

