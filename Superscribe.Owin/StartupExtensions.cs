using System;
using System.Linq;
using Microsoft.AspNet.Builder;
using Superscribe.Components;
using Superscribe.Engine;

namespace Superscribe.Owin
{
    public static class StartupExtensions
    {
        public static IApplicationBuilder UseSuperscribeRouter(
            this IApplicationBuilder builder, IOwinRouteEngine engine)
        {
            return SuperscribeRouter(builder, engine);
        }

        public static IApplicationBuilder UseSuperscribeHandler(
            this IApplicationBuilder builder, IOwinRouteEngine engine)
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

        private static IApplicationBuilder SuperscribeRouter(IApplicationBuilder builder, IOwinRouteEngine engine)
        {
            return builder.Use(typeof(OwinRouter), builder, engine);
        }
    }
}
