namespace Superscribe.Owin.Extensions
{
    using System;
    using System.Linq;

    using global::Owin;

    using Superscribe.Owin.Components;
    using Superscribe.Owin.Engine;

    public static class StartupExtensions
    {
        public static IAppBuilder UseSuperscribeRouter(
            this IAppBuilder builder, IOwinRouteEngine engine)
        {
            return SuperscribeRouter(builder, engine);
        }

        public static IAppBuilder UseSuperscribeHandler(
            this IAppBuilder builder, IOwinRouteEngine engine)
        {
            if (engine.Config.ScanForModules)
            {
                var modules = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                               from type in assembly.GetTypes()
                               where typeof(SuperscribeOwinModule).IsAssignableFrom(type) && type != typeof(SuperscribeOwinModule)
                               select new { Type = type }).ToList();

                foreach (var module in modules)
                {
                    var owinModule = (SuperscribeOwinModule)Activator.CreateInstance(module.Type);
                    owinModule.Initialise(engine);
                }
            }

            return builder.Use(typeof(OwinHandler), engine);
        }

        private static IAppBuilder SuperscribeRouter(IAppBuilder builder, IOwinRouteEngine engine)
        {
            return builder.Use(typeof(OwinRouter), builder, engine);
        }
    }
}
