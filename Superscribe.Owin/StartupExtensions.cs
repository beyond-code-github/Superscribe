namespace Superscribe.Owin
{
    using global::Owin;

    using Superscribe.Models;
    using Superscribe.Utils;

    public static class StartupExtensions
    {
        public static IAppBuilder UseSuperscribe(
            this IAppBuilder builder)
        {
            return builder.UseHandlerAsync((req, res) =>
                {
                    var path = req.Path;
                    var routeData = new RouteData();

                    var walker = new RouteWalker<RouteData>(ʃ.Base);
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

                    res.ContentType = "text/html";

                    return res.WriteAsync(routeData.Response.ToString());
                });
        }
    }
}
