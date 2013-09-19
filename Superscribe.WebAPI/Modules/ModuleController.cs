namespace Superscribe.WebApi.Modules
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Superscribe.Models;

    public class ModuleController : ApiController
    {
        public HttpResponseMessage Process()
        {
            var webApiInfo = new RouteData();
            var walker = SuperscribeConfig.Walker();
            walker.WalkRoute(this.Request.RequestUri.AbsolutePath, this.Request.Method.ToString(), webApiInfo);

            if (walker.ExtraneousMatch)
            {
                throw new HttpResponseException(
                    this.Request.CreateErrorResponse(
                        HttpStatusCode.NotFound,
                        string.Format(
                            "Superscribe encountered extraneous segments for the route or failed on the first match: '{0}'",
                            this.Request.RequestUri)));
            }

            if (walker.IncompleteMatch)
            {
                throw new HttpResponseException(
                    this.Request.CreateErrorResponse(
                        HttpStatusCode.NotFound,
                        string.Format(
                            "Superscribe was expecting further route segments '{0}'",
                            this.Request.RequestUri)));
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, webApiInfo.Response);
        }
    }
}
