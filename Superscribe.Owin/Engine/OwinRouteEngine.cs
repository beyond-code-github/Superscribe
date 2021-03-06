﻿namespace Superscribe.Owin.Engine
{
    using System;

    using global::Owin;

    using Superscribe.Engine;
    using Superscribe.Models;
    using Superscribe.Owin.Pipelining;

    public class OwinRouteEngine : RouteEngine, IOwinRouteEngine
    {
        public OwinRouteEngine(SuperscribeOwinOptions options)
            : base(options)
        {
            this.Config = options;
        }

        public new SuperscribeOwinOptions Config { get; private set; }

        public OwinNode Pipeline(Func<RouteGlue, GraphNode> config, Func<IAppBuilder, IAppBuilder> func, params object[] args)
        {
            var leaf = config(new RouteGlue()) * new OwinNodeFuture(func, args);
            this.Base.Zip(leaf.Node.Base());

            return leaf;
        }

        public OwinNode Pipeline(Func<RouteGlue, GraphNode> config)
        {
            var leaf = config(new RouteGlue());
            this.Base.Zip(leaf.Base());

            return new OwinNode(leaf);
        }

        public OwinNode Pipeline(string routeTemplate)
        {
            var node = this.Config.StringRouteParser.MapToGraph(routeTemplate);

            if (node != null)
            {
                var owinNode = new OwinNode(node);
                this.Base.Zip(owinNode.Node.Base());

                return owinNode;
            }
            
            return null;
        }
    }
}
