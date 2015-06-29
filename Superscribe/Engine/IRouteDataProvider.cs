namespace Superscribe.Engine
{
    using System;
    using System.Collections.Generic;

    public interface IRouteDataProvider
    {
        IRouteData GetData(IDictionary<string, object> environment, Func<RouteData> factory);
    }
}