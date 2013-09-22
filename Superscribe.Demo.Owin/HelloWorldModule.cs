namespace Superscribe.Demo.Owin
{
    using Superscribe.Owin;

    public class HelloWorldModule : SuperscribeOwinModule
    {
        public HelloWorldModule()
        {
            this.Get["/"] = _ => "Hello world";

            this.Get["Test"] = _ => new { Message = "Hello World" };
        }
    }
}