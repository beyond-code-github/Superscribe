namespace Superscribe.Testing.Controllers
{
    using System.Web.Http;

    public class PortfolioProjectMediaController : ApiController
    {
        public string Get(int siteId, int projectId)
        {
            return "Get";
        }

        public string GetById(int siteId, int projectId, int id)
        {
            return "GetById";
        }
    }
}
