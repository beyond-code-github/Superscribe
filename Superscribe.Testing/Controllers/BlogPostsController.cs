namespace Superscribe.Testing.Controllers
{
    using System.Web.Http;

    public class BlogPostsController : ApiController
    {
        public string Get(int siteId)
        {
            return "Get";
        }

        public string GetById(int siteId, int postId)
        {
            return "GetById";
        }
    }
}
