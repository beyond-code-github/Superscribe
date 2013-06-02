namespace Superscribe.WebApi
{
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    using Superscribe.Utils;
    using Superscribe.WebApi.Internals;

    /// <summary>
    /// Superscribe configuration for Web API
    /// </summary>
    public static class SuperscribeConfig
    {
        public static HttpConfiguration HttpConfiguration { get; private set; }

        public static HttpControllerTypeCache ControllerTypeCache { get; private set; }

        public static void Register(HttpConfiguration configuration)
        {
            HttpConfiguration = configuration;

            // We need a single default route that will match everything
            configuration.Routes.Clear();
            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{*wildcard}",
                defaults: new { controller = "values", id = RouteParameter.Optional }
            );

            ControllerTypeCache = new HttpControllerTypeCache(configuration);

            configuration.Services.Replace(typeof(IHttpActionSelector), new SuperscribeActionSelector());
            configuration.Services.Replace(typeof(IHttpControllerSelector), new SuperscribeControllerSelector());
            configuration.Services.Replace(typeof(IHttpActionInvoker), new SuperscribeActionInvoker());
        }

        public static RouteWalker Walker()
        {
            return new RouteWalker(ʃ.Base.Transitions);
        }
    }
}
