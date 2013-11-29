namespace Superscribe.Owin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class Middleware
    {
        public static MiddlewareNode Use<T>(params object[] args)
        {
            var node = new MiddlewareNode();
            node.ActionFunction = async (o, s) =>
                {
                    var routeData = o as OwinRouteData;
                    
                    var branch = routeData.Builder.New();
                    branch.Use(typeof(T), args);

                    var next = (Func<IDictionary<string, object>, Task>)branch.Build(typeof(Func<IDictionary<string, object>, Task>));
                    await next(routeData.Environment);
                };

            return node;
        }
    }
}
