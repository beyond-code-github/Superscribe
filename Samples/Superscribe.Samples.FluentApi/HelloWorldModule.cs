namespace Superscribe.Samples.FluentApi
{
    using Superscribe.Models;
    using Superscribe.Owin;

    public class HelloWorldModule : SuperscribeOwinModule
    {
        public HelloWorldModule()
        {
            ʃ.Base.FinalFunctions.Add(new FinalFunction("GET", _ => @"
                Welcome to Superscribe 
                <a href='/Hello/World'>Say hello...</a>
            "));

            var helloRoute = new ConstantNode("Hello").Slash(new ConstantNode("World"));
            helloRoute.FinalFunctions.Add(new FinalFunction("GET", _ => "Hello World"));

            var justHelloRoute = new ConstantNode("Hello").Optional();
            
            ʃ.Base.Zip(helloRoute.Base());
        }
    }
}