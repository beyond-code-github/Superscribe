namespace Superscribe.Testing.Controllers
{
    using System.Web.Http;

    public class BlogPostMediaController : ApiController
    {
        public string Get(int siteId, int postId)
        {
            return "Get";
        }

        public string GetById(int siteId, int postId, int id)
        {
            return "GetById";
        }
    }
}
