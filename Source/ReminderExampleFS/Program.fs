open Orleans.Runtime.Configuration
open Orleans.Storage

open Orleankka
open Orleankka.Playground
open Orleankka.Core
open Orleankka.Cluster
open Orleankka.Client

open System
open System.Reflection
open System.Threading.Tasks

open ReminderExample
  


[<EntryPoint>]
let main argv =

    printfn "Init server"       

    let clusterConfig = ClusterConfiguration().LoadFromEmbeddedResource(Assembly.GetExecutingAssembly(), "Server.xml")   
   
    use system = ActorSystem.Configure()
                            .Cluster()
                            .From(clusterConfig)
                            .Register(typeof<Scheduler>.Assembly)                          
                            .Done()

    printfn "server ready"
    Console.ReadLine() |> ignore
    0 



