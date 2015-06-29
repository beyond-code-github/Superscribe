namespace Superscribe.Engine
{
    using System;
    using System.Collections.Generic;

    public class LazyRouteDataProvider : IRouteDataProvider
    {
        private readonly IRouteWalker routeWalker;

        private IRouteData data;

        public LazyRouteDataProvider(IRouteWalker routeWalker)
        {
            this.routeWalker = routeWalker;
        }

        public IRouteData GetData(IDictionary<string, object> environment, Func<RouteData> factory)
        {
            return this.data ?? (this.data = this.routeWalker.WalkRoute(environment, factory()));
        }
    }
}
