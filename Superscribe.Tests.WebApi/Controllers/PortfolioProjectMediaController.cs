namespace Superscribe.Testing.Http.Controllers
{
    using System.Web.Http;

    public class PortfolioProjectMediaController : ApiController
    {
        public string Get(int siteId, int projectId)
        {
            return string.Format("Get_PortfolioProjectMedia_{0}_{1}", siteId, projectId);
        }

        public string GetById(int siteId, int projectId, int id)
        {
            return string.Format("GetById_PortfolioProjectMedia_{0}_{1}_{2}", siteId, projectId, id);
        }
    }
}
