namespace Superscribe.Engine
{
    using System;

    public interface IRouteDataProvider
    {
        IRouteData GetData(string url, string method, Func<RouteData> factory);
    }
}