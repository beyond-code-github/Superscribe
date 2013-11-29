namespace Superscribe.Owin.Models
{
    using Superscribe.Models;

    public class MiddlewareNode : GraphNode
    {
        public static GraphNode operator *(GraphNode node, MiddlewareNode middleware)
        {
            node.ActionFunction = middleware.ActionFunction;
            return node;
        }
    }
}
