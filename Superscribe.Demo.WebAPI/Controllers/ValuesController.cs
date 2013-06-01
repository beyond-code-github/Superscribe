namespace Superscribe.Demo.WebAPI.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class ComplexValue
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }

    public class ValuesController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2", "value3", "value4" };
        }

        public string GetById(int id)
        {
            return new[] { "value1", "value2", "value3", "value4" }.ElementAt(id - 1);
        }

        public string GetByUserId(int userId)
        {
            return new[] { "uservalue1", "uservalue2", "uservalue3" }.ElementAt(userId - 1);
        }

        [HttpGet]
        public string First()
        {
            return new[] { "value1", "value2", "value3", "value4" }.First();
        }

        [HttpGet]
        public string Last()
        {
            return new[] { "value1", "value2", "value3", "value4" }.Last();
        }

        public HttpResponseMessage Post([FromBody] ComplexValue value, int id)
        {
            return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }
    }
}