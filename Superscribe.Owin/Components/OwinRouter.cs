namespace Superscribe.Owin.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Owin;

    using Superscribe.Owin.Engine;
    using Superscribe.Owin.Extensions;
    using Superscribe.Owin.Pipelining;

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
            var path = environment["owin.RequestPath"].ToString();
            var method = environment["owin.RequestMethod"].ToString();
            
            var routeData = new OwinRouteData { Environment = environment, Config = engine.Config };
            var walker = this.engine.Walker();
            var data = walker.WalkRoute(path, method, routeData);

            environment["superscribe.RouteData"] = data;

            if (data.IncompleteMatch)
            {
                environment.SetResponseStatusCode(404);
                environment.WriteResponse("404 - Route was incomplete");
                return;
            }

            if (data.ExtraneousMatch)
            {
                environment.SetResponseStatusCode(404);
                environment.WriteResponse("404 - Route match failed");
                return;
            }

            environment["route.Parameters"] = routeData.Parameters;
            environment[Constants.SuperscribeRouteWalkerEnvironmentKey] = walker;
            environment[Constants.SuperscribeRouteDataProviderEnvironmentKey] = new OwinRouteDataProvider(data);

            if (routeData.Pipeline.Any())
            {
                IAppBuilder branch = this.builder.New();
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
