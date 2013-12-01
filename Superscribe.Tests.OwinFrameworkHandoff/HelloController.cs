namespace Superscribe.Tests.OwinFrameworkHandoff
{
    using System.Web.Http;

    public class HelloController : ApiController
    {
        public string Get()
        {
            return "Hello from Web API";
        }
    }
}