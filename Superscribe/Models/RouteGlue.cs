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

        public static SuperscribeNode operator /(RouteGlue state, string other)
        {
            var node = new ConstantNode(other);
            if (!string.IsNullOrEmpty(state.Method))
            {
                node.AddAllowedMethod(state.Method);    
            }

            return node;
        }

        public static SuperscribeNode operator /(RouteGlue state, SuperscribeNode other)
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

        public static NonConsumingNode operator /(RouteGlue state, Action<dynamic> other)
        {
            var nonConsuming = new NonConsumingNode<double>();
            nonConsuming.ActivationFunction = (routedata, s) => true;
            nonConsuming.ActionFunction = (data, segment) => other(data);

            return nonConsuming;
        }

        public static NonConsumingNode<double> operator /(RouteGlue state, Func<dynamic, string, double> other)
        {
            var nonConsuming = new NonConsumingNode<double>();
            nonConsuming.ActivationFunction = (routedata, s) => true;
            nonConsuming.ActionFunction = (data, segment) => nonConsuming.Result = other(data, segment);
            return nonConsuming;
        }

        public static NonConsumingNode<double> operator /(RouteGlue state, Predicate<double> other)
        {
            var nonConsuming = new NonConsumingNode<double>();
            nonConsuming.SetMatchFromParentValue(other);
            return nonConsuming;
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