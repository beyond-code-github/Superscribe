namespace Superscribe.Models
{
    using System;

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

        public static GraphNode operator /(RouteGlue state, string other)
        {
            var node = new ConstantNode(other);
            if (!string.IsNullOrEmpty(state.Method))
            {
                node.AddAllowedMethod(state.Method);    
            }

            return node;
        }

        public static GraphNode operator /(RouteGlue state, GraphNode other)
        {
            var node = other.Base();
            if (!string.IsNullOrEmpty(state.Method))
            {
                node.AddAllowedMethod(state.Method);
            }

            return node;
        }

        public static SuperList operator /(RouteGlue state, SuperList others)
        {
            foreach (var other in others)
            {
                var node = other.Base();
                if (!string.IsNullOrEmpty(state.Method))
                {
                    node.AddAllowedMethod(state.Method);
                }
            }
            
            return others;
        }

        public static NodeFuture operator /(RouteGlue state, Func<dynamic, string, bool> activation)
        {
            return new NodeFuture { ActivationFunction = activation };
        }

        public static FinalFunction operator *(RouteGlue state, Func<dynamic, object> other)
        {
            var function = new FinalFunction { Method = state.Method, Function = other };
            return function;
        }

        public RouteGlue this[string method]
        {
            get
            {
                return new RouteGlue(method);
            }
        }
    }
}