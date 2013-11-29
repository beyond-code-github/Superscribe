namespace Superscribe.Testing.Http.Controllers
{
    using System.Web.Http;

    public class PortfolioTagsController : ApiController
    {
        public string Get(int siteId)
        {
            return string.Format("Get_PortfolioTags_{0}", siteId);
        }
    }
}
