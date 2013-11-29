namespace Superscribe.Testing.Http.Controllers
{
    using System.Web.Http;

    public class BlogPostMediaController : ApiController
    {
        public string Get(int siteId, int postId)
        {
            return string.Format("Get_BlogPostMedia_{0}_{1}", siteId, postId);
        }

        public string GetById(int siteId, int postId, int id)
        {
            return string.Format("GetById_BlogPostMedia_{0}_{1}_{2}", siteId, postId, id);
        }
    }
}
