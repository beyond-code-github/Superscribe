namespace Superscribe
{
    using System;

    using Superscribe.Cache;
    using Superscribe.Engine;
    using Superscribe.Models;
    using Superscribe.Utils;

    public class SuperscribeOptions
    {
        public SuperscribeOptions()
        {
            this.StringRouteParser = new StringRouteParser();
            this.RouteCache = new RouteCache();
            this.RouteWalkerFactory = baseNode => new RouteWalker(baseNode, this.RouteCache);
        }

        public IRouteCache RouteCache { get; set; }

        public IStringRouteParser StringRouteParser { get; set; }

        public Func<GraphNode, IRouteWalker> RouteWalkerFactory { get; set; }
    }
}
