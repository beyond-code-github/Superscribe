namespace Superscribe.Demo.WebApiModules
{
    using Superscribe.Demo.WebApiModules.Services;
    using Superscribe.Models;
    using Superscribe.WebApi.Modules;

    public class MessageWrapper
    {
        public string Message { get; set; }
    }

    public class HelloWorldModule : SuperscribeModule
    {
        public HelloWorldModule()
        {
            this.Get["/"] = o => new { Message = "Hello World" };
            
            this.Get["Hello" / (String)"Name"] = o =>
                {
                    var helloService = o.Require<IHelloService>();
                    return new { Message = helloService.SayHello(o.Parameters.Name) };
                };

            this.Post["Save"] = o =>
                {
                    var wrapper = o.Bind<MessageWrapper>();
                    return new { Message = "You entered - " + wrapper.Message };
                };
        }
    }
}