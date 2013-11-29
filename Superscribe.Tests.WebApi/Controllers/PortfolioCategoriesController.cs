namespace Superscribe.Testing.Http.Controllers
{
    using System.Web.Http;

    public class PortfolioCategoriesController : ApiController
    {
        public string Get(int siteId)
        {
            return string.Format("Get_PortfolioCategories_{0}", siteId);
        }

        public string GetById(int siteId, int id)
        {
            return string.Format("GetById_PortfolioCategories_{0}_{1}", siteId, id);
        }
    }
}
