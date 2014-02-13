using System.Collections.Generic;
using System.Web.Http;

namespace Superscribe.Demo.WebApi.Owin
{
    public class ValuesController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }
    }
}