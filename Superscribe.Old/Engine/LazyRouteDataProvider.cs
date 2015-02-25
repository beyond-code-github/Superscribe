namespace Superscribe.Engine
{
    using System;

    public class LazyRouteDataProvider : IRouteDataProvider
    {
        private readonly IRouteWalker routeWalker;

        private IRouteData data;

        public string Uri { get; private set; }

        public string Method { get; private set; }

        public LazyRouteDataProvider(IRouteWalker routeWalker)
        {
            this.routeWalker = routeWalker;
        }

        public IRouteData GetData(string uri, string method, Func<RouteData> factory)
        {
            if (string.IsNullOrEmpty(this.Uri) && string.IsNullOrEmpty(this.Method))
            {
                this.Uri = uri;
                this.Method = method;
            }

            if (this.Method != method || this.Uri != uri)
            {
                throw new InvalidOperationException("Attempted to use a routedata provider that had already been evaluated for a different url or method");   
            }

            if (this.data == null)
            {
                this.data = this.routeWalker.WalkRoute(this.Uri, this.Method, factory());
            }

            return this.data;
        }
    }
}
