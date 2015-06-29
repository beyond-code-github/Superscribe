namespace Superscribe.Models
{
    using System;

    using Superscribe.Models.Filters;
    using Superscribe.Utils;

    public class RouteGlue
    {
        public RouteGlue()
        {
        }

        public RouteGlue(string method)
        {
            this.Method = method;
        }

        public string Method { get; set; }

        public RouteGlue this[string method]
        {
            get
            {
                return new RouteGlue(method);
            }
        }

        public static GraphNode operator /(RouteGlue state, string other)
        {
            var node = new ConstantNode(other);
            return node;
        }

        public static GraphNode operator /(RouteGlue state, GraphNode other)
        {
            var node = other.Base();
            return node;
        }

        public static SuperList operator /(RouteGlue state, SuperList others)
        {
            return others;
        }

        public static NodeFuture operator /(RouteGlue state, Func<dynamic, string, bool> activation)
        {
            return new NodeFuture { ActivationFunction = activation };
        }

        public static FinalFunction operator *(RouteGlue state, Func<dynamic, object> other)
        {
            var function = new FinalFunction(other, new MethodFilter(state.Method));
            return function;
        }
    }
}