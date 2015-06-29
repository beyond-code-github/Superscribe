namespace Superscribe.Owin.Engine
{
    using System;
    using System.Collections.Generic;

    using Superscribe.Engine;

    public class OwinRouteDataProvider : IRouteDataProvider
    {
        private readonly IRouteData data;

        public OwinRouteDataProvider(IRouteData data)
        {
            this.data = data;
        }

        public IRouteData GetData(IDictionary<string, object> environment, Func<RouteData> factory)
        {
            return this.data;
        }
    }
}
