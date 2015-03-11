namespace Superscribe.Models
{
    using System;

    public class NodeFuture
    {
        public GraphNode Parent { get; set; }

        public Func<dynamic, string, bool> ActivationFunction { get; set; }

        public static GraphNode operator *(NodeFuture future, string constant)
        {
            var node = new ConstantNode(constant);
            node.ActivationFunction = future.ActivationFunction;

            return future.Parent.Slash(node);
        }

        public static GraphNode operator *(NodeFuture future, GraphNode node)
        {
            node.ActivationFunction = future.ActivationFunction;

            return future.Parent.Slash(node);
        }
    }
}
