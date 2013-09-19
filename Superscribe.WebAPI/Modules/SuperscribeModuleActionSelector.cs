namespace Superscribe.WebApi.Modules
{
    using System;
    using System.Linq;
    using System.Web.Http.Controllers;

    public class SuperscribeModuleActionSelector : IHttpActionSelector
    {
        public HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            var member = typeof(ModuleController).GetMethod("Process");
            return new ReflectedHttpActionDescriptor(controllerContext.ControllerDescriptor, member);
        }

        public ILookup<string, HttpActionDescriptor> GetActionMapping(HttpControllerDescriptor controllerDescriptor)
        {
            throw new NotImplementedException();
        }
    }
}
