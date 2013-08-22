namespace Superscribe.Testing.Controllers
{
    using System.Web.Http;

    public class BlogPostsController : ApiController
    {
        public string Get(int siteId)
        {
            return string.Format("Get_BlogPosts_{0}", siteId);
        }

        public string GetById(int siteId, int postId)
        {
            return string.Format("GetById_BlogPosts_{0}_{1}", siteId, postId);
        }
    }
}
