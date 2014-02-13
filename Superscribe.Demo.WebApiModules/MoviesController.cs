namespace Superscribe.Demo.WebApiModules
{
    using System.Collections.Generic;
    using System.Web.Http;

    public class MoviesController : ApiController
    {
        [Route("movies")]
        public IEnumerable<string> Get()
        {
            return new[] { "The Matrix", "Lord of the Rings" };
        }
    }
}