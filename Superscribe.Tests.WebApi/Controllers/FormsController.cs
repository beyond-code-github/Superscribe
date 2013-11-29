namespace Superscribe.Testing.Http.Controllers
{
    using System.Web.Http;

    public class FormsController : ApiController
    {
        public string Get(long parentId)
        {
            return string.Format("Get_Forms_{0}", parentId);
        }

        public string GetById(long parentId, long id)
        {
            return string.Format("GetById_Forms_{0}_{1}", parentId, id);
        }

        [HttpGet]
        public string VisibleFor(long parentId, string appDataId)
        {
            return string.Format("VisibleFor_Forms_{0}_{1}", parentId, appDataId);
        }
    }
}
