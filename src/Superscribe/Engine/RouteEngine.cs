namespace Superscribe.Engine
{
    using System;

    using Superscribe.Models;
    using Superscribe.Utils;

    public class RouteEngine : IRouteEngine
    {
        private readonly GraphNode @base = new GraphNode();

        protected readonly IStringRouteParser stringRouteParser;

        protected readonly Func<GraphNode, IRouteWalker> routeWalkerFactory;

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

        public GraphNode Route(string routeTemplate)
        {
            var node = this.stringRouteParser.MapToGraph(routeTemplate);
            if (node != null)
            {
                this.Base.Zip(node.Base());
                return node;
            }

            return null;
        }

        public GraphNode Route(string config, Func<dynamic, object> func)
        {
            var finalNode = this.Base;
            var node = this.stringRouteParser.MapToGraph(config);

            if (node != null)
            {
                this.Base.Zip(node.Base());
                finalNode = node;
            }

            finalNode.FinalFunctions.Add(new ExclusiveFinalFunction { Function = func });

            return node;
        }

        public GraphNode Route(GraphNode config)
        {
            this.Base.Zip(config.Base());
            return config;
        }

        public GraphNode Route(GraphNode config, Func<dynamic, object> func)
        {
            config.FinalFunctions.Add(new ExclusiveFinalFunction { Function = func });
            this.Base.Zip(config.Base());

            return config;
        }

        public GraphNode Route(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue());
            this.Base.Zip(leaf.Base());
            return leaf;
        }

        public GraphNode Route(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func)
        {
            var leaf = config(new RouteGlue());
            leaf.FinalFunctions.Add(new ExclusiveFinalFunction { Function = func });
            this.Base.Zip(leaf.Base());

            return leaf;
        }
        
        public GraphNode Get(string routeTemplate, Func<dynamic, object> func)
        {
            return this.MethodNode(routeTemplate, func, "GET");
        }
        
        public GraphNode Get(GraphNode leaf, Func<dynamic, object> func)
        {
            return this.MethodNode(leaf, func, "GET");
        }
        
        public GraphNode Get(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func)
        {
            return this.MethodNode(config, func, "GET");
        }
        
        public GraphNode Post(string routeTemplate, Func<dynamic, object> func)
        {
            return this.MethodNode(routeTemplate, func, "POST");
        }
        
        public GraphNode Post(GraphNode leaf, Func<dynamic, object> func)
        {
            return this.MethodNode(leaf, func, "POST");
        }
        
        public GraphNode Post(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func)
        {
            return this.MethodNode(config, func, "POST");
        }
        
        public GraphNode Patch(string routeTemplate, Func<dynamic, object> func)
        {
            return this.MethodNode(routeTemplate, func, "PATCH");
        }
        
        public GraphNode Patch(GraphNode leaf, Func<dynamic, object> func)
        {
            return this.MethodNode(leaf, func, "PATCH");
        }
        
        public GraphNode Patch(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func)
        {
            return this.MethodNode(config, func, "PATCH");
        }

        public GraphNode Put(string routeTemplate, Func<dynamic, object> func)
        {
            return this.MethodNode(routeTemplate, func, "PUT");
        }
        
        public GraphNode Put(GraphNode leaf, Func<dynamic, object> func)
        {
            return this.MethodNode(leaf, func, "PUT");
        }
        
        public GraphNode Put(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func)
        {
            return this.MethodNode(config, func, "PUT");
        }
        
        public GraphNode Delete(string routeTemplate, Func<dynamic, object> func)
        {
            return this.MethodNode(routeTemplate, func, "DELETE");
        }
        
        public GraphNode Delete(GraphNode leaf, Func<dynamic, object> func)
        {
            return this.MethodNode(leaf, func, "DELETE");
        }
        
        public GraphNode Delete(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func)
        {
            return this.MethodNode(config, func, "DELETE");
        }

        private GraphNode MethodNode(string routeTemplate, Func<dynamic, object> func, string method)
        {
            var finalNode = this.Base;
            var node = this.stringRouteParser.MapToGraph(routeTemplate);

            if (node != null)
            {
                this.Base.Zip(node.Base());
                finalNode = node;
            }

            finalNode.FinalFunctions.Add(new ExclusiveFinalFunction { Method = method, Function = func });
            return node;
        }
        
        private GraphNode MethodNode(GraphNode leaf, Func<dynamic, object> func, string method)
        {
            leaf.FinalFunctions.Add(new ExclusiveFinalFunction { Method = method, Function = func });

            this.Base.Zip(leaf.Base());
            return leaf;
        }
        
        private GraphNode MethodNode(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func, string method)
        {
            var leaf = config(new RouteGlue());
            leaf.FinalFunctions.Add(new ExclusiveFinalFunction { Method = method, Function = func });

            this.Base.Zip(leaf.Base());
            return leaf;
        }
    }
}
