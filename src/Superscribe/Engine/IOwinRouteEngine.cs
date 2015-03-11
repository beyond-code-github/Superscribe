using System;
using Superscribe.Models;
using Superscribe.Pipelining;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Superscribe.Engine
{
    using MidFunc = System.Func<AppFunc, AppFunc>;

    public interface IOwinRouteEngine : IRouteEngine
    {
        SuperscribeOwinOptions Config { get; }

        OwinNode Pipeline(Func<RouteGlue, GraphNode> config, MidFunc func);

        OwinNode Pipeline(Func<RouteGlue, GraphNode> config);

        OwinNode Pipeline(string routeTemplate);
    }
}
