namespace Superscribe
{
    using System;

    using Superscribe.Engine;
    using Superscribe.Models;
    using Superscribe.Utils;

    public class SuperscribeOptions
    {
        public SuperscribeOptions()
        {
            this.StringRouteParser = new StringRouteParser();
            this.RouteWalkerFactory = baseNode => new RouteWalker(baseNode);
        }

        public IStringRouteParser StringRouteParser { get; set; }

        public Func<GraphNode, IRouteWalker> RouteWalkerFactory { get; set; }
    }
}
