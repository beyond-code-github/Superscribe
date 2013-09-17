namespace Superscribe.WebApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    using Superscribe.Models;

    public class SuperscribeControllerSelector : IHttpControllerSelector
    {
        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var info = new RouteData();

            var walker = SuperscribeConfig.Walker();
            walker.WalkRoute(request.RequestUri.AbsolutePath, request.Method.ToString(), info);

            // We should have consumed all of the route by now, if we haven't then throw a 404
            if (walker.ExtraneousMatch)
            {
                throw new HttpResponseException(
                    request.CreateErrorResponse(
                        HttpStatusCode.NotFound,
                        string.Format(
                            "Superscribe encountered extraneous segments for the route or failed on the first match: '{0}'",
                            request.RequestUri)));
            }

            if (walker.IncompleteMatch)
            {
                throw new HttpResponseException(
                    request.CreateErrorResponse(
                        HttpStatusCode.NotFound,
                        string.Format(
                            "Superscribe was expecting further route segments '{0}'",
                            request.RequestUri)));
            }

            if (info.ControllerNameSpecified)
            {
                var controllerType = SuperscribeConfig.ControllerTypeCache.GetControllerTypes(info.ControllerName).FirstOrDefault();
                if (controllerType != null)
                {
                    return new HttpControllerDescriptor(
                        SuperscribeConfig.HttpConfiguration, info.ControllerName, controllerType);
                }
            }

            throw new HttpResponseException(
                request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    string.Format(
                        "Superscribe was not able to match a controller for the route '{0}'", request.RequestUri)));
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            throw new System.NotImplementedException();
        }
    }
}
