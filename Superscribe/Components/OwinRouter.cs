using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Superscribe.Engine;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Superscribe.Components
{
    using MidFunc = System.Func<AppFunc, AppFunc>;
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<AppFunc, AppFunc>>>;

    public class OwinRouter
    {
        private readonly AppFunc next;

        private readonly IOwinRouteEngine engine;
        
        public OwinRouter(AppFunc next, IOwinRouteEngine engine)
        {
            this.next = next;
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
            environment["route.Parameters"] = routeData.Parameters;
            environment[Constants.SuperscribeRouteWalkerEnvironmentKey] = walker;
            environment[Constants.SuperscribeRouteDataProviderEnvironmentKey] = new OwinRouteDataProvider(data);

            if (routeData.Pipeline.Any())
            {
                foreach (var middleware in routeData.Pipeline)
                {
                    BuildFunc build = middleware =>
                    {
                        branch(middleware)
                    };

                    var func = middleware.Obj as Func<AppFunc, AppFunc>;
                    if (func != null)
                    {
                        branch = func(branch);
                    }
                    else
                    {
                        
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
