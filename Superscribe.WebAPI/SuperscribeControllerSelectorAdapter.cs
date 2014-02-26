namespace Superscribe.WebApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    public class SuperscribeControllerSelectorAdapter : IHttpControllerSelector
    {
        private readonly IHttpControllerSelector baseSelector;

        public SuperscribeControllerSelectorAdapter(IHttpControllerSelector baseSelector)
        {
            this.baseSelector = baseSelector;
        }

        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var provider = request.GetRouteDataProvider();
            var info = provider.GetData(request);

            // We should have consumed all of the route by now, if we haven't then throw a 404
            if (!info.ExtraneousMatch)
            {
                if (info.Environment.ContainsKey(Constants.ControllerNamePropertyKey))
                {
                    var controllerName = info.Environment[Constants.ControllerNamePropertyKey].ToString();
                    var controllerType = SuperscribeConfig.ControllerTypeCache.GetControllerTypes(controllerName).FirstOrDefault();
                    if (controllerType != null)
                    {
                        return new HttpControllerDescriptor(
                            SuperscribeConfig.HttpConfiguration,
                            controllerName,
                            controllerType);
                    }
                }
            }

            return this.baseSelector.SelectController(request);
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return baseSelector.GetControllerMapping();
        }
    }
}
