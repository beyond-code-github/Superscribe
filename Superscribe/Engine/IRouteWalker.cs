namespace Superscribe.Engine
{
    using System.Collections.Generic;

    public interface IRouteWalker
    {
        bool ParamConversionError { get; }

        Queue<string> RemainingSegments { get; }

        RouteData WalkRoute(IDictionary<string, object> environment, RouteData info);
    }
}