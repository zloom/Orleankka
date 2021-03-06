### Prerequisites

* Azure SDK 2.4
* Azure Compute Emulator v2.4 

### How to run

* Start compute emulator as administrator.
* Make sure you've also started Visual Studio as administrator.
* Build solution and make sure there is no build errors due to missing packages
* Choose Deployment project to be a "Startup project"
* Hit F5 to deploy everything into emulator
* In compute emulator you should see 2 roles: Client (web role) running 1 instance, and Cluster (worker role) running 3 instances
* Client is an ASP.NET application and after the role is started up, the Visual Studio should open the entry page in your browser
* Enter the count of publisher grains you want to spawn and click "Spawn"
* After some delay you will be redirected to notifications page where you can observe all events generated by all grains in a cluster
* You can click "Back" in a browser and spawn more grains any time you wish

### Have fun!
