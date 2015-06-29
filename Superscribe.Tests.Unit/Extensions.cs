namespace Superscribe.Tests.Unit
{
    using System.Collections.Generic;

    using Superscribe.Engine;
    using Superscribe.Utils;

    public static class Extensions
    {
        public static RouteData WalkRoute(
            this IRouteWalker routeWalker,
            string route,
            string method,
            RouteData routeData)
        {
            string path;
            string querystring;
            route.SplitPathAndQuery(out path, out querystring);

            var environment = new Dictionary<string, object>();
            environment[Constants.RequestPathEnvironmentKey] = path;
            environment[Constants.RequestQuerystringEnvironmentKey] = querystring;
            environment[Constants.RequestMethodEnvironmentKey] = method;

            return routeWalker.WalkRoute(environment, routeData);
        }
    }
}
