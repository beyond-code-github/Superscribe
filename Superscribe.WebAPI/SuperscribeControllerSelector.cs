namespace Superscribe.WebApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    using Superscribe.Engine;

    public class SuperscribeControllerSelector : IHttpControllerSelector
    {
        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var walker = request.GetDependencyScope().GetService(typeof(IRouteWalker)) as IRouteWalker;

            var info = walker.WalkRoute(
                request.RequestUri.PathAndQuery,
                request.Method.ToString(),
                new RouteData());

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

            if (info.Environment.ContainsKey(Constants.ControllerNamePropertyKey))
            {
                var controllerName = info.Environment[Constants.ControllerNamePropertyKey].ToString();
                var controllerType = SuperscribeConfig.ControllerTypeCache.GetControllerTypes(controllerName).FirstOrDefault();
                if (controllerType != null)
                {
                    return new HttpControllerDescriptor(
                        SuperscribeConfig.HttpConfiguration, controllerName, controllerType);
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
