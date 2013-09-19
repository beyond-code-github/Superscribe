namespace Superscribe.Demo.WebApiModules
{
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

            this.Get["Hello" / (ʃString)"Name"] = o => new { Message = string.Format("Hello {0}", o.Parameters.Name) };
            
            this.Post["Save"] = o =>
            {
                var message = o.Bind<MessageWrapper>();

                return new { Message = "You entered - " + message.Message };
            };
        }
    }
}