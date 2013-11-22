namespace Superscribe.WebApi2.Modules
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    public class SuperscribeModuleControllerSelector : IHttpControllerSelector
    {
        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var controllerType = SuperscribeConfig.ControllerTypeCache.GetControllerTypes("Module").FirstOrDefault();
            return new HttpControllerDescriptor(
                    SuperscribeConfig.HttpConfiguration,
                    "ModuleController",
                    controllerType);
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            throw new System.NotImplementedException();
        }
    }
}

