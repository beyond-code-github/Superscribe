namespace HelloWorldModule

open Superscribe.Owin

type HelloWorldModule() as this =
    inherit SuperscribeOwinModule()
        do 
            this.Get.["/"] <- fun _ -> "Hello world" :> obj