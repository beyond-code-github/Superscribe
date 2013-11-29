namespace Superscribe.Testing.WebApi.Controllers
{
    using System.Web.Http;

    public class BlogTagsController : ApiController
    {
        public string Get(int siteId)
        {
            return string.Format("Get_BlogTags_{0}", siteId);
        }
    }
}
