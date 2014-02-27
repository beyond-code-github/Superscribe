namespace Superscribe.Tests.WebApi.Owin
{
    using System.Web.Http;

    public class ValuesController : ApiController
    {
        private readonly IRepository repository;

        public ValuesController(IRepository repository)
        {
            this.repository = repository;
        }

        public string Get()
        {
            return this.repository.Values();
        }
    }
}
