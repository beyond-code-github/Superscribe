namespace Superscribe.WebApi
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    using Superscribe.Engine;
    using Superscribe.WebApi.Dependencies;
    using Superscribe.WebApi.Internals;

    /// <summary>
    /// Superscribe configuration for Web API
    /// </summary>
    public static class SuperscribeConfig
    {
        public static HttpConfiguration HttpConfiguration { get; private set; }

        public static HttpControllerTypeCache ControllerTypeCache { get; private set; }

        public static IRouteEngine RegisterModules(HttpConfiguration configuration, IRouteEngine engine = null, string qualifier = "")
        {
            if (engine == null)
            {
                engine = RouteEngineFactory.Create();
            }

            configuration.DependencyResolver = new SuperscribeDependencyAdapter(configuration.DependencyResolver, engine);

            var modules = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                           from type in assembly.GetTypes()
                           where typeof(SuperscribeModule).IsAssignableFrom(type) && type != typeof(SuperscribeModule)
                           select new { Type = type }).ToList();

            foreach (var module in modules)
            {
                var instance = (SuperscribeModule)Activator.CreateInstance(module.Type);
                instance.Initialise(engine);
            }

            configuration.MessageHandlers.Add(new SuperscribeHandler());
            
            var actionSelector = configuration.Services.GetService(typeof(IHttpActionSelector)) as IHttpActionSelector;
            var controllerSelector = configuration.Services.GetService(typeof(IHttpControllerSelector)) as IHttpControllerSelector;
            var actionInvoker = configuration.Services.GetService(typeof(IHttpActionInvoker)) as IHttpActionInvoker;

            configuration.Services.Replace(typeof(IHttpActionSelector), new SuperscribeActionSelectorAdapter(actionSelector));
            configuration.Services.Replace(typeof(IHttpControllerSelector), new SuperscribeControllerSelectorAdapter(controllerSelector));
            configuration.Services.Replace(typeof(IHttpActionInvoker), new SuperscribeActionInvokerAdapter(actionInvoker));
            
            ControllerTypeCache = new HttpControllerTypeCache(configuration);

            var template = "{*wildcard}";
            if (!string.IsNullOrEmpty(qualifier))
            {
                template = qualifier + "/" + template;
            }

            HttpConfiguration = configuration;

            // We need a single default route that will match everything
            // configuration.Routes.Clear();
            configuration.Routes.MapHttpRoute(
                name: "Superscribe",
                routeTemplate: template,
                defaults: new { });

            return engine;
        }

        public static IRouteEngine Register(HttpConfiguration configuration, IRouteEngine engine = null, string qualifier = "")
        {
            engine = RegisterCommon(configuration, qualifier, engine);

            var actionSelector = configuration.Services.GetService(typeof(IHttpActionSelector)) as IHttpActionSelector;
            var controllerSelector = configuration.Services.GetService(typeof(IHttpControllerSelector)) as IHttpControllerSelector;
            var actionInvoker = configuration.Services.GetService(typeof(IHttpActionInvoker)) as IHttpActionInvoker;

            configuration.Services.Replace(typeof(IHttpActionSelector), new SuperscribeActionSelectorAdapter(actionSelector));
            configuration.Services.Replace(typeof(IHttpControllerSelector), new SuperscribeControllerSelectorAdapter(controllerSelector));
            configuration.Services.Replace(typeof(IHttpActionInvoker), new SuperscribeActionInvokerAdapter(actionInvoker));

            return engine;
        }
        
        private static IRouteEngine RegisterCommon(HttpConfiguration configuration, string qualifier, IRouteEngine engine = null)
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

            if (engine == null)
            {
                engine = RouteEngineFactory.Create();
            }

            configuration.DependencyResolver = new SuperscribeDependencyAdapter(configuration.DependencyResolver, engine);

            return engine;
        }
    }
}
