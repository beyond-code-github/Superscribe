namespace Superscribe.Utils
{
    using System;
    using System.Linq;

    using Superscribe.Models;

    public interface IStringRouteParser
    {
        GraphNode MapToGraph(string routePattern);
    }

    public class StringRouteParser : IStringRouteParser
    {
        public GraphNode MapToGraph(string routePattern)
        {
            if (string.IsNullOrEmpty(routePattern))
            {
                throw new ArgumentException("Route pattern cannot be null", "routePattern");
            }

            if (routePattern == "/")
            {
                return null;
            }

            var parts = routePattern.Split('/').Where(o => !string.IsNullOrEmpty(o.Trim()));

            GraphNode node = null;
            foreach (var part in parts)
            {
                var thisNode = new ConstantNode(part);
                if (node != null)
                {
                    node.Slash(thisNode);
                }

                node = thisNode;
            }

            return node;
        }
    }
}
