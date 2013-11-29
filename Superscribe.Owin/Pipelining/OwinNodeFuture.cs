namespace Superscribe.Owin.Pipelining
{
    using System.Diagnostics;

    using Superscribe.Models;

    public class OwinNodeFuture
    {
        private readonly object middleware;

        public OwinNodeFuture(object middleware)
        {
            this.middleware = middleware;
        }

        public static OwinNode operator *(GraphNode node, OwinNodeFuture future)
        {
            var owinNode = new OwinNode(node);
            var existingAction = node.ActionFunction;

            owinNode.Middleware.Add(future.middleware);

            node.ActionFunction = (o, s) =>
                {
                    if (existingAction != null)
                    {
                        existingAction(o, s);
                    }

                    var routeData = o as OwinRouteData;
                    Debug.Assert(routeData != null, "routeData != null");

                    foreach (var middleware in owinNode.Middleware)
                    {
                        routeData.Pipeline.Add(middleware);
                    }
                };

            return owinNode;
        }

        public static OwinNode operator *(OwinNode owinNode, OwinNodeFuture future)
        {
            owinNode.Middleware.Add(future.middleware);
            return owinNode;
        }
    }
}
