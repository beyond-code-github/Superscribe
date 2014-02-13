namespace Superscribe.Engine
{
    using System.Collections.Generic;

    public interface IRouteWalker
    {
        bool ExtraneousMatch { get; }

        bool IncompleteMatch { get; }

        bool ParamConversionError { get; }

        string Route { get; set; }

        string Method { get; set; }

        Queue<string> RemainingSegments { get; }

        RouteData WalkRoute(string route, string method, RouteData info);
    }
}