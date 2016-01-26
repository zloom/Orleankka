open Orleans.Runtime.Configuration
open Orleans.Storage

open System
open System.Reflection
open System.Threading.Tasks

open Orleankka.Client
open Orleankka
open ReminderExample



let askReminder (system: IActorSystem) reminderId =
   
    if reminderId.Equals("reg") then
        system.ActorOf<Scheduler>("Any")
               .Tell({registr.Id = "some"})
               .ContinueWith(new Action<Task>(fun p-> printfn "reminder register %s"  (p.AsyncState.ToString()))) |> ignore
    else
        system.ActorOf<Scheduler>("Any")
              .Ask<bool>({check.Id = "some"})
              .ContinueWith(new Action<Task<bool>>(fun p-> printfn "reminder some is %b" p.Result)) |> ignore

   
    printfn "usageFact sent"
  


[<EntryPoint>]
let main argv =

    printfn "Init client"       

    let config = ClientConfiguration().LoadFromEmbeddedResource(Assembly.GetExecutingAssembly(), "Client.xml")
   
    use system = ActorSystem.Configure()
                            .Client()
                            .From(config)
                            .Register(typeof<Scheduler>.Assembly)                            
                            .Done()

    printfn "client ready"

    let sender = system |> askReminder

    Seq.initInfinite(fun _ -> Console.ReadLine()) |> Seq.iter sender 
    

    0 



