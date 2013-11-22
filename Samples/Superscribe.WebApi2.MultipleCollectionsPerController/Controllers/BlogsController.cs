namespace Superscribe.WebApi2.MultipleCollectionsPerController.Controllers
{
    using System.Web.Http;

    public class BlogsController : ApiController
    {
        public string Get()
        {
            return "Get";
        }
        
        public string GetById(int id)
        {
            return "GetById";
        }

        public string GetBlogPosts()
        {
            return "Blog Posts";
        }

        public string GetBlogTags()
        {
            return "Blog Tags";
        }

        public string GetBlogTags(string query)
        {
            return "Blog Tags - " + query;
        }

        public string PostBlogPosts()
        {
            return "Post Blog Posts";
        }

        public string PostBlogTags()
        {
            return "Post Blog Tags";
        }
    }
}
