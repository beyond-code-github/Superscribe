namespace Superscribe.Testing.Http.Controllers
{
    using System.Web.Http;

    public class PortfolioProjectsController : ApiController
    {
        public string Get(int siteId)
        {
            return string.Format("Get_PortfolioProjects_{0}", siteId);
        }

        public string GetById(int siteId, int projectId)
        {
            return string.Format("GetById_PortfolioProjects_{0}_{1}", siteId, projectId);
        }
    }
}
