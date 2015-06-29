namespace Superscribe.Owin.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Owin;

    using Superscribe.Owin.Engine;

    public class OwinRouter
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        private readonly IAppBuilder builder;

        private readonly IOwinRouteEngine engine;

        public OwinRouter(Func<IDictionary<string, object>, Task> next, IAppBuilder builder, IOwinRouteEngine engine)
        {
            this.next = next;
            this.builder = builder;
            this.engine = engine;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var headers = (IDictionary<string, string[]>)environment["owin.RequestHeaders"];
            
            var path = environment["owin.RequestPath"].ToString();
            var method = environment["owin.RequestMethod"].ToString();

            string[] accepts;
            string[] contentType;
            headers.TryGetValue("accepts", out accepts);
            headers.TryGetValue("content-type", out contentType);

            var parts = path.Split('?');
            if (parts.Length > 0)
            {
                path = parts[0];
            }

            var querystring = string.Empty;
            if (parts.Length > 1)
            {
                querystring = parts[1];
            }

            environment[Constants.RequestPathEnvironmentKey] = path;
            environment[Constants.RequestQuerystringEnvironmentKey] = querystring;
            environment[Constants.RequestMethodEnvironmentKey] = method;
            environment[Constants.AcceptsEnvironmentKey] = accepts;
            environment[Constants.ContentTypeEnvironmentKey] = contentType;

            var routeData = new OwinRouteData { Environment = environment, Config = this.engine.Config };
            var walker = this.engine.Walker();
            var data = walker.WalkRoute(environment, routeData);

            environment["superscribe.RouteData"] = data;
            environment["route.Parameters"] = routeData.Parameters;
            environment[Constants.SuperscribeRouteWalkerEnvironmentKey] = walker;
            environment[Constants.SuperscribeRouteDataProviderEnvironmentKey] = new OwinRouteDataProvider(data);

            if (routeData.Pipeline.Any())
            {
                var branch = this.builder.New();
                foreach (var middleware in routeData.Pipeline)
                {
                    var func = middleware.Obj as Func<IAppBuilder, IAppBuilder>;
                    if (func != null)
                    {
                        branch = func(branch);
                    }
                    else
                    {
                        branch.Use(middleware.Obj, middleware.Args);    
                    }
                }

                branch.Use(typeof(RedirectMiddleware), this.next);

                var continuation = (Func<IDictionary<string, object>, Task>)branch.Build(typeof(Func<IDictionary<string, object>, Task>));
                await continuation(environment);
            }
            else
            {
                await this.next(environment);
            }
        }
    }
}
