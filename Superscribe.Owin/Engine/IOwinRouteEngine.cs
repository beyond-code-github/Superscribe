namespace Superscribe.Owin.Engine
{
    using System;

    using global::Owin;

    using Superscribe.Engine;
    using Superscribe.Models;
    using Superscribe.Owin.Pipelining;

    public interface IOwinRouteEngine : IRouteEngine
    {
        SuperscribeOwinOptions Config { get; }

        OwinNode Pipeline(Func<RouteGlue, GraphNode> config, Func<IAppBuilder, IAppBuilder> func, params object[] args);

        OwinNode Pipeline(Func<RouteGlue, GraphNode> config);
    }
}
