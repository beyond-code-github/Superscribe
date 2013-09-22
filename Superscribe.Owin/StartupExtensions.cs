namespace Superscribe.Owin
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Owin;

    using Superscribe.Utils;

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
            return builder.UseHandlerAsync((req, res) =>
                {
                    var path = req.Path;
                    var routeData = new OwinRouteData { OwinRequest = req, Response = res, Config = config };

                    var walker = new RouteWalker<OwinRouteData>(ʃ.Base);
                    walker.WalkRoute(path, req.Method, routeData);

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
                        var mediaType = mediaTypes.FirstOrDefault(o => config.ContentHandlers.Keys.Contains(o) && config.ContentHandlers[o].Write != null);
                        if (!string.IsNullOrEmpty(mediaType))
                        {
                            var formatter = config.ContentHandlers[mediaType];
                            res.SetHeader("content-type", mediaType);
                            return formatter.Write(res, routeData.Response);
                        }
                        
                        throw new NotSupportedException("Media type is not supported");
                    }
                    
                    throw new NotSupportedException("Response type is not supported");
                });
        }
    }
}
