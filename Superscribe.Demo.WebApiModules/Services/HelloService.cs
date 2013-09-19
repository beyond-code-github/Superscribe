namespace Superscribe.Demo.WebApiModules.Services
{
    public interface IHelloService
    {
        string SayHello(string name);
    }

    public class HelloService : IHelloService
    {
        public string SayHello(string name)
        {
            return string.Format("Hello {0}", name);
        }
    }
}