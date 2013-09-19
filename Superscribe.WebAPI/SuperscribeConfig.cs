namespace Superscribe.WebApi
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    using Superscribe.Utils;
    using Superscribe.WebApi.Internals;
    using Superscribe.WebApi.Modules;

    /// <summary>
    /// Superscribe configuration for Web API
    /// </summary>
    public static class SuperscribeConfig
    {
        public static HttpConfiguration HttpConfiguration { get; private set; }

        public static HttpControllerTypeCache ControllerTypeCache { get; private set; }

        public static void RegisterModules(HttpConfiguration configuration, string qualifier = "")
        {
            RegisterCommon(configuration, qualifier);

            configuration.Services.Replace(typeof(IHttpActionSelector), new SuperscribeModuleActionSelector());
            configuration.Services.Replace(typeof(IHttpControllerSelector), new SuperscribeModuleControllerSelector());
            configuration.Services.Replace(typeof(IHttpActionInvoker), new SuperscribeModuleActionInvoker());

            var modules = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                           from type in assembly.GetTypes()
                           where typeof(SuperscribeModule).IsAssignableFrom(type) && type != typeof(SuperscribeModule)
                           select new { Type = type }).ToList();

            foreach (var module in modules)
            {
                Activator.CreateInstance(module.Type);
            }
        }

        public static void Register(HttpConfiguration configuration, string qualifier = "")
        {
            RegisterCommon(configuration, qualifier);

            configuration.Services.Replace(typeof(IHttpActionSelector), new SuperscribeActionSelector());
            configuration.Services.Replace(typeof(IHttpControllerSelector), new SuperscribeControllerSelector());
            configuration.Services.Replace(typeof(IHttpActionInvoker), new SuperscribeActionInvoker());
        }

        private static void RegisterCommon(HttpConfiguration configuration, string qualifier)
        {
            var template = "{*wildcard}";
            if (!string.IsNullOrEmpty(qualifier))
            {
                template = qualifier + "/" + template;
            }

            HttpConfiguration = configuration;

            // We need a single default route that will match everything
            // configuration.Routes.Clear();
            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: template,
                defaults: new { controller = "values", id = RouteParameter.Optional });

            ControllerTypeCache = new HttpControllerTypeCache(configuration);
        }

        public static RouteWalker Walker()
        {
            return new RouteWalker(ʃ.Base);
        }
    }
}
