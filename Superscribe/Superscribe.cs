namespace Superscribe
{
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    using global::Superscribe.Models;
    using global::Superscribe.Utils;
    using global::Superscribe.WebAPI;
    using global::Superscribe.WebAPI.Internals;

    public static class Superscribe
    {
        public static HttpConfiguration HttpConfiguration { get; private set; }

        public static HttpControllerTypeCache ControllerTypeCache { get; private set; }

        public static ʃ Base { get; set; }

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

            Base = new ʃ();
        }

        public static RouteWalker Walker()
        {
            return new RouteWalker(Base.Transitions);
        }
    }
}
