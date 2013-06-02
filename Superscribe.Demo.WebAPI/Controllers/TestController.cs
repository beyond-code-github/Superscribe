namespace Superscribe.Demo.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;

    public class TestController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new[] { "test1", "test2" };
        }

        [HttpGet]
        public IEnumerable<string> More()
        {
            return new[] { "test1", "test2", "test3", "test4" };
        }
    }
}