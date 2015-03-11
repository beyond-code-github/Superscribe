using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Superscribe.Engine;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Superscribe.Components
{ 
    public class OwinRouter
    {
        private AppFunc _next;

        private readonly IOwinRouteEngine _engine;
        
        public OwinRouter(IOwinRouteEngine engine)
        {
            _engine = engine;
        }

        public AppFunc Compose(AppFunc nextApplication)
        {
            _next = nextApplication;
            return Invoke;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var path = environment["owin.RequestPath"].ToString();
            var method = environment["owin.RequestMethod"].ToString();
            
            var routeData = new OwinRouteData { Environment = environment, Config = _engine.Config };
            var walker = _engine.Walker();
            var data = walker.WalkRoute(path, method, routeData);

            environment["superscribe.RouteData"] = data;
            environment["route.Parameters"] = routeData.Parameters;
            environment[Constants.SuperscribeRouteWalkerEnvironmentKey] = walker;
            environment[Constants.SuperscribeRouteDataProviderEnvironmentKey] = new OwinRouteDataProvider(data);

            if (routeData.Pipeline.Any())
            {
                var iterator = routeData.Pipeline.AsEnumerable().Reverse();
                var chain = iterator.Aggregate(_next, (appFunc, next) => next(appFunc));
                await chain(environment);
            }
            else
            {
                await _next(environment);
            }
        }
    }
}
