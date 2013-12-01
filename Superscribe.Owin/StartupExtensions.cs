namespace Superscribe.Owin
{
    using System;
    using System.Linq;

    using global::Owin;

    public static class StartupExtensions
    {
        public static IAppBuilder UseSuperscribeRouter(
            this IAppBuilder builder, SuperscribeOwinConfig config)
        {
            return SuperscribeRouter(builder, config);
        }

        public static IAppBuilder UseSuperscribeHandler(
            this IAppBuilder builder, SuperscribeOwinConfig config)
        {
            var modules = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                           from type in assembly.GetTypes()
                           where typeof(SuperscribeOwinModule).IsAssignableFrom(type) && type != typeof(SuperscribeOwinModule)
                           select new { Type = type }).ToList();

            foreach (var module in modules)
            {
                Activator.CreateInstance(module.Type);
            }

            return builder.Use(typeof(OwinHandler), config);
        }

        private static IAppBuilder SuperscribeRouter(IAppBuilder builder, SuperscribeOwinConfig config)
        {
            return builder.Use(typeof(OwinRouter), builder, config);
        }
    }
}
