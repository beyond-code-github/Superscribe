open System
open Microsoft.Owin.Hosting
open Microsoft.Owin.Builder
open Nowin
open System.Security.Cryptography.X509Certificates

[<EntryPoint>]
let main argv =  
    let options = new StartOptions (ServerFactory = "Nowin", Port = Nullable<int> 8888)
    
    let owinbuilder = new AppBuilder();
    OwinServerFactory.Initialize(owinbuilder.Properties);
    
    let builder = ServerBuilder.New().SetPort(8888).SetOwinApp(owinbuilder.Build())
    builder.SetCertificate(new X509Certificate2("../../localhost.pfx", "nowin"));

    use a =  WebApp.Start<Server.Startup>(options)
    Console.WriteLine("Server running on port {0}", options.Port)
    Console.ReadLine() |> ignore    
    0