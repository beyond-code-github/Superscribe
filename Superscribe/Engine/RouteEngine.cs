namespace Superscribe.Engine
{
    using System;

    using Superscribe.Models;
    using Superscribe.Utils;

    public class RouteEngine : IRouteEngine
    {
        private readonly GraphNode @base = new GraphNode();

        private readonly IStringRouteParser stringRouteParser;

        private readonly Func<GraphNode, IRouteWalker> routeWalkerFactory;

        public RouteEngine(IStringRouteParser stringRouteParser, Func<GraphNode, IRouteWalker> routeWalkerFactory)
        {
            this.stringRouteParser = stringRouteParser;
            this.routeWalkerFactory = routeWalkerFactory;
        }

        public GraphNode Base
        {
            get
            {
                return this.@base;
            }
        }

        public IRouteWalker Walker()
        {
            return routeWalkerFactory(this.Base);
        }

        /// <summary>
        /// Define a partial route or attach a route to Superscribe's Base State
        /// </summary>
        /// <param name="config">Route configuration function</param>
        /// <returns>The last state in the chain</returns>
        public GraphNode Route(Func<GraphNode> config)
        {
            var leaf = config();
            this.Base.Zip(leaf.Base());
            return leaf;
        }

        /// <summary>
        /// Define a partial route or attach a route to Superscribe's Base State
        /// </summary>
        /// <param name="config">Route configuration function</param>
        /// <returns>The last state in the chain</returns>
        public GraphNode Route(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue());
            this.Base.Zip(leaf.Base());
            return leaf;
        }

        public GraphNode Route(Func<RouteGlue, SuperList> config)
        {
            var list = config(new RouteGlue());
            foreach (var node in list)
            {
                this.Base.Zip(node.Base());    
            }
            
            return this.Base;
        }

        public GraphNode Route(Func<RouteGlue, Func<dynamic, object>> config)
        {
            var final = config(new RouteGlue());
            this.Base.FinalFunctions.Add(new FinalFunction { Function = final });
            return this.Base;
        }

        public void Route(GraphNode config)
        {
            if (config != null)
            {
                this.Base.Zip(config.Base());
            }
        }

        public void Route(string config, Func<dynamic, object> func)
        {
            var finalNode = this.Base;
            var node = this.stringRouteParser.MapToGraph(config);

            if (node != null)
            {
                this.Base.Zip(node.Base());
                finalNode = node;
            }

            finalNode.FinalFunctions.Add(new FinalFunction { Function = func });
        }
        
        public void Route(GraphNode config, Func<dynamic, object> func)
        {
            config.FinalFunctions.Add(new FinalFunction { Function = func });
            this.Base.Zip(config.Base());
        }

        public GraphNode Get(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue("GET"));
            leaf.AddAllowedMethod("GET");
            this.Base.Zip(leaf.Base());

            return leaf;
        }

        public GraphNode Post(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue("POST"));
            leaf.AddAllowedMethod("POST");
            this.Base.Zip(leaf.Base());

            return leaf;
        }

        public GraphNode Put(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue("PUT"));
            leaf.AddAllowedMethod("PUT");
            this.Base.Zip(leaf.Base());

            return leaf;
        }

        public GraphNode Patch(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue("PATCH"));
            leaf.AddAllowedMethod("PATCH");
            this.Base.Zip(leaf.Base());

            return leaf;
        }

        public GraphNode Delete(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue("DELETE"));
            leaf.AddAllowedMethod("DELETE");
            this.Base.Zip(leaf.Base());

            return leaf;
        }
    }
}
