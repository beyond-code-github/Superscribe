namespace Superscribe.Testing.WebApi.Controllers
{
    using System.Web.Http;

    public class FormsController : ApiController
    {
        [HttpGet]
        public string Get(long parentId)
        {
            return string.Format("Get_Forms_{0}", parentId);
        }

        [HttpGet]
        public string GetById(long parentId, long id)
        {
            return string.Format("GetById_Forms_{0}_{1}", parentId, id);
        }

        [HttpPatch]
        public string Patch(long parentId, long id)
        {
            return string.Format("Patch_Forms_{0}_{1}", parentId, id);
        }

        [HttpDelete]
        public string Delete(long parentId, long id)
        {
            return string.Format("Delete_Forms_{0}_{1}", parentId, id);
        }

        [HttpGet]
        public string VisibleFor(long parentId, string appDataId)
        {
            return string.Format("VisibleFor_Forms_{0}_{1}", parentId, appDataId);
        } 
        
        [HttpGet]
        public string RenderForm(long parentId, long id)
        {
            return string.Format("RenderForm_Forms_{0}_{1}", parentId, id);
        }
    }
}
