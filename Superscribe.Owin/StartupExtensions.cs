namespace Superscribe.Owin
{
    using System;
    using System.Linq;

    using global::Owin;

    public static class StartupExtensions
    {
        public static IAppBuilder UseSuperscribe(
            this IAppBuilder builder, SuperscribeOwinConfig config)
        {
            return SuperscribeHandler(builder, config);
        }

        public static IAppBuilder UseSuperscribeModules(
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

            return SuperscribeHandler(builder, config);
        }

        private static IAppBuilder SuperscribeHandler(IAppBuilder builder, SuperscribeOwinConfig config)
        {
            return builder.Use(typeof(SuperscribeMiddleware), config);
        }
    }
}
