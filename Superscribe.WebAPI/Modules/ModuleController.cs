namespace Superscribe.WebApi.Modules
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class ModuleController : ApiController
    {
        public HttpResponseMessage Process()
        {
            var webApiInfo = new ModuleRouteData
                                 {
                                     Configuration = this.Configuration,
                                     ControllerContext = this.ControllerContext,
                                     ModelState = this.ModelState,
                                     Request = this.Request,
                                     Url = this.Url,
                                     User = this.User
                                 };

            var walker = SuperscribeConfig.Walker<ModuleRouteData>();
            
            walker.WalkRoute(this.Request.RequestUri.PathAndQuery, this.Request.Method.ToString(), webApiInfo);

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

            var responseMessage = webApiInfo.Response as HttpResponseMessage;
            if (responseMessage != null)
            {
                return responseMessage;
            }

            var statusCode = webApiInfo.Response as HttpStatusCode?;
            if (statusCode != null)
            {
                return this.Request.CreateResponse(statusCode.Value);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, webApiInfo.Response);
        }
    }
}
