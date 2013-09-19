namespace Superscribe.Demo.WebApiModules
{
    using Superscribe.Models;

    public class HelloWorldModule : SuperscribeModule
    {
        public HelloWorldModule()
        {
            this.Get["/"] = o => 
                o.Response = "Hello World";

            this.Get["Hello" / (ʃString)"Name"] = o => 
                o.Response = string.Format("Hello {0}", o.Parameters.Name);
        }
    }
}