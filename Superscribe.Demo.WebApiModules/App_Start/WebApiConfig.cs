namespace Superscribe.Demo.WebApiModules.App_Start
{
    using System.Web.Http;

    using Superscribe.WebApi;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableSystemDiagnosticsTracing();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { controller = "values", id = RouteParameter.Optional });

            var engine = SuperscribeConfig.RegisterModules(config);

            engine.Route("values".Controller());
        }
    }
}
