namespace Superscribe.Owin
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Owin;

    using Superscribe.Utils;

    public static class StartupExtensions
    {
        public static void UseSuperscribe(
            this IAppBuilder builder, SuperscribeOwinConfig config)
        {
            SuperscribeHandler(builder, config);
        }

        public static void UseSuperscribeModules(
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

            SuperscribeHandler(builder, config);
        }

        private static void SuperscribeHandler(IAppBuilder builder, SuperscribeOwinConfig config)
        {
            builder.Run(ctx =>
                {
                    var req = ctx.Request;
                    var res = ctx.Response;

                    var path = req.Path;
                    var routeData = new OwinRouteData { OwinRequest = req, Response = res, Config = config };

                    var walker = new RouteWalker<OwinRouteData>(ʃ.Base);
                    walker.WalkRoute(path.ToString(), req.Method, routeData);

                    if (walker.IncompleteMatch)
                    {
                        res.StatusCode = 404;
                        return res.WriteAsync("404 - Route was incomplete");
                    }

                    if (walker.ExtraneousMatch)
                    {
                        res.StatusCode = 404;
                        return res.WriteAsync("404 - Route match failed");
                    }

                    var responseTask = routeData.Response as Task;
                    if (responseTask != null)
                    {
                        return responseTask;
                    }

                    // Set status code
                    if (routeData.StatusCode > 0)
                    {
                        res.StatusCode = routeData.StatusCode;
                    }

                    string[] outgoingMediaTypes;
                    if (req.Headers.TryGetValue("accept", out outgoingMediaTypes))
                    {
                        var mediaTypes = ConnegHelpers.GetWeightedValues(outgoingMediaTypes);
                        var mediaType = mediaTypes.FirstOrDefault(o => config.MediaTypeHandlers.Keys.Contains(o) && config.MediaTypeHandlers[o].Write != null);
                        if (!string.IsNullOrEmpty(mediaType))
                        {
                            var formatter = config.MediaTypeHandlers[mediaType];
                            res.ContentType = mediaType;
                            return formatter.Write(res, routeData.Response);
                        }
                        
                        throw new NotSupportedException("Media type is not supported");
                    }
                    
                    throw new NotSupportedException("Response type is not supported");
                });
        }
    }
}
