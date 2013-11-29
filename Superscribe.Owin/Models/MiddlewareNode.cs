namespace Superscribe.Owin.Models
{
    using Superscribe.Models;

    public class MiddlewareNode : SuperscribeNode
    {
        public static SuperscribeNode operator *(SuperscribeNode node, MiddlewareNode middleware)
        {
            node.ActionFunction = middleware.ActionFunction;
            return node;
        }
    }
}
