namespace Superscribe.Owin.Engine
{
    using System;

    using Superscribe.Engine;

    public class OwinRouteDataProvider : IRouteDataProvider
    {
        private readonly IRouteData data;

        public OwinRouteDataProvider(IRouteData data)
        {
            this.data = data;
        }

        public IRouteData GetData(string uri, string method, Func<RouteData> factory)
        {
            return this.data;
        }
    }
}
