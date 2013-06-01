namespace Superscribe.Testing.Controllers
{
    using System.Web.Http;

    public class PortfolioCategoriesController : ApiController
    {
        public string Get(int siteId)
        {
            return "Get";
        }

        public string GetById(int siteId, int id)
        {
            return "GetById";
        }
    }
}
