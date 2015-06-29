//namespace Server
// 
//open Owin
//open Microsoft.Owin
//open Superscribe.Engine;
//open Superscribe.Models
//open Superscribe.Owin
//open Superscribe.Owin.Engine
//open Superscribe.Owin.Extensions
//open EkonBenefits.FSharp.Dynamic
//
//type NameBeginningWith(letter) as this = 
//    inherit GraphNode()
//        do
//            this.ActivationFunction <- fun data segment -> segment.StartsWith(letter)
//            this.ActionFunctions.Add(
//                "set_param_Name",
//                fun data segment -> data.Parameters?Add("Name", segment));
//                
//
//type Startup() =
//    member x.Configuration(app: Owin.IAppBuilder) =
//        let define = OwinRouteEngineFactory.Create();
//        app.UseSuperscribeRouter(define).UseSuperscribeHandler(define) |> ignore
//
//        let hello = ConstantNode("hello")
//
//        define.Route(
//            hello / NameBeginningWith "p", 
//            fun o -> 
//                "Hello " + o?Parameters?Name + ", great first letter!" :> obj) |> ignore
//
//        define.Route(
//            hello / String "Name", 
//            fun o -> 
//                "Hello " + o?Parameters?Name :> obj) |> ignore
// 
//[<assembly: OwinStartup(typeof<Startup>)>]
//do ()