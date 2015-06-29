namespace Server
 
open System
open System.Collections.Generic
open System.Threading.Tasks

open Owin
open Microsoft.Owin
open Superscribe.Engine;
open Superscribe.Models
open Superscribe.Owin
open Superscribe.Owin.Engine
open Superscribe.Owin.Extensions
open EkonBenefits.FSharp.Dynamic

type AppFunc = Func<IDictionary<string, obj>, Task>
type MidFunc = Func<AppFunc, AppFunc>

type RequireHttps(next: AppFunc) =
    member this.Invoke(environment: IDictionary<string, obj>) : Task =
        match environment.["owin.RequestScheme"].ToString() with
            | "https" -> (next.Invoke(environment))
            | other -> 
                environment.["owin.ResponseStatusCode"] <- 400 :> obj
                environment.["owin.ResponseReasonPhrase"] <- "Connection was not secure" :> obj
                Task.FromResult<obj>(null) :> Task

type RequireAuthentication(next: AppFunc) =
    member this.Invoke(environment: IDictionary<string, obj>) : Task =
        let requestHeaders = environment.["owin.RequestHeaders"] :?> Dictionary<string, string> 
        match requestHeaders.["Authentication"] with
            | "ABC123" -> (next.Invoke(environment))
            | other -> 
                environment.["owin.ResponseStatusCode"] <- 403 :> obj
                environment.["owin.ResponseReasonPhrase"] <- "Authentication required" :> obj
                Task.FromResult<obj>(null) :> Task
        
type Startup() =
    member x.Configuration(app: Owin.IAppBuilder) =
        let define = OwinRouteEngineFactory.Create();   
        app.UseSuperscribeRouter(define).UseSuperscribeHandler(define) |> ignore

        define.Route("admin/token", fun o -> "{ token: ABC123 }" :> obj) |> ignore
        define.Route("admin/users", fun o -> "List all users" :> obj) |> ignore

        let users = define.Route("users")
        define.Route(users / String "UserId", fun o -> "User details for " + o?Parameters?UserId :> obj) |> ignore

        define.Pipeline("admin").Use<RequireHttps>() |> ignore
        define.Pipeline("admin/users").Use<RequireAuthentication>() |> ignore
         
[<assembly: OwinStartup(typeof<Startup>)>]
do ()