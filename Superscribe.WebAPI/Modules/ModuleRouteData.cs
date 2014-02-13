namespace Superscribe.WebApi.Modules
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Security.Principal;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;
    using System.Web.Http.Routing;

    using Superscribe.Engine;
    using Superscribe.Utils;
    
    public class ModuleRouteData : IModuleRouteData
    {
        public ModuleRouteData()
        {
            this.Parameters = new DynamicDictionary();
        }

        public dynamic Parameters { get; set; }

        public IDictionary<string, object> Environment { get; set; }

        public object Response { get; set; }

        public HttpConfiguration Configuration { get; set; }

        public HttpControllerContext ControllerContext { get; set; }

        public ModelStateDictionary ModelState { get; set; }

        public HttpRequestMessage Request { get; set; }

        public UrlHelper Url { get; set; }

        public IPrincipal User { get; set; }

        public T Bind<T>() where T : class
        {
            var configuraton = this.ControllerContext.Configuration;
            var conneg = (IContentNegotiator)configuraton.Services.GetService(typeof(IContentNegotiator));
            var formatter = conneg.Negotiate(
                typeof(T),
                this.Request,
                configuraton.Formatters);

            var stream = this.Request.Content.ReadAsStreamAsync().Result;

            return formatter.Formatter.ReadFromStreamAsync(
                typeof(T),
                stream,
                this.Request.Content,
                null).Result as T;
        }

        public T Require<T>() where T : class
        {
            return this.ControllerContext.Configuration.DependencyResolver.GetService(typeof(T)) as T;
        }
    }
}
