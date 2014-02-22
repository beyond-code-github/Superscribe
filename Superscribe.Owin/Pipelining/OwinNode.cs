namespace Superscribe.Owin.Pipelining
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using global::Owin;

    using Superscribe.Models;
    using Superscribe.Utils;

    public class OwinNode : IAppBuilder
    {
        public GraphNode Node { get; set; }

        public List<Middleware> Middleware { get; set; }

        public IDictionary<string, object> Properties { get; private set; }

        /// <summary>
        /// Base constructor for superscribe states
        /// </summary>
        public OwinNode(GraphNode node)
        {
            this.Node = node;
            this.Middleware = new List<Middleware>();
            this.Properties = new Dictionary<string, object>();

            var existingAction = node.ActionFunction;
            node.ActionFunction = (o, s) =>
            {
                if (existingAction != null)
                {
                    existingAction(o, s);
                }

                var routeData = o as OwinRouteData;
                Debug.Assert(routeData != null, "routeData != null");

                foreach (var middleware in this.Middleware)
                {
                    routeData.Pipeline.Add(middleware);
                }
            };
        }

        public static implicit operator GraphNode(OwinNode owinNode)
        {
            return owinNode.Node;
        }

        /// <summary>
        /// Shorthand for .Slash between a state and a transition to a ConstantState
        /// </summary>
        /// <param name="owinNode">State</param>
        /// <param name="other">String used create constant state</param>
        public static GraphNode operator /(OwinNode owinNode, string other)
        {
            return owinNode.Node / other;
        }

        /// <summary>
        /// Shorthand for .Slash between a state and a transition
        /// </summary>
        /// <param name="owinNode">Any state</param>
        /// <param name="other">Any transition</param>
        public static GraphNode operator /(OwinNode owinNode, GraphNode other)
        {
            return owinNode.Node / other;
        }

        /// <summary>
        /// Shorthand for .Slash between a state and its possible transitions
        /// </summary>
        /// <param name="owinNode">State</param>
        /// <param name="others">IEnumerble of transitions</param>
        public static GraphNode operator /(OwinNode owinNode, IEnumerable<GraphNode> others)
        {
            foreach (var s in others)
            {
                owinNode.Node.Slash(s.Base());
            }

            return owinNode.Node;
        }

        /// <summary>
        /// Shorthand for creating a transition list from two alternatives
        /// </summary>
        /// <param name="node">First transition</param>
        /// <param name="other">Second transition</param>
        /// <returns>New list of transitions</returns>
        public static SuperList operator |(OwinNode node, OwinNode other)
        {
            return new SuperList { node, other };
        }

        /// <summary>
        /// Shorthand for adding a transition to an existing list 
        /// </summary>
        /// <param name="states">List of transitions</param>
        /// <param name="other">Alternative transition</param>
        /// <returns>Modified list of transitions</returns>
        public static SuperList operator |(SuperList states, OwinNode other)
        {
            states.Add(other);
            return states;
        }

        /// <summary>
        /// Shorthand for adding a transition to an existing list 
        /// </summary>
        /// <param name="node">First transition</param>
        /// <param name="others">Alternative transitions</param>
        /// <returns>Modified list of transitions</returns>
        public static SuperList operator |(OwinNode node, SuperList others)
        {
            var list = new SuperList { node };
            list.AddRange(others);
            return list;
        }

        public IAppBuilder Use(object middleware, params object[] args)
        {
            return this * new OwinNodeFuture(middleware, args);
        }

        public object Build(Type returnType)
        {
            throw new NotImplementedException();
        }

        public IAppBuilder New()
        {
            throw new NotImplementedException();
        }
    }
}
