using System;
using Superscribe.Models;

namespace Superscribe.Engine
{
    public interface IOwinRouteEngine : IRouteEngine
    {
        SuperscribeOwinOptions Config { get; }

        OwinNode Pipeline(Func<RouteGlue, GraphNode> config, Func<IAppBuilder, IAppBuilder> func, params object[] args);

        OwinNode Pipeline(Func<RouteGlue, GraphNode> config);

        OwinNode Pipeline(string routeTemplate);
    }
}
