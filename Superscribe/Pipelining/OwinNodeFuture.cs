using Superscribe.Models;

namespace Superscribe.Pipelining
{
    public class OwinNodeFuture
    {
        private readonly Middleware middleware;

        public OwinNodeFuture(object middleware, params object[] args)
        {
            this.middleware = new Middleware { Obj = middleware, Args = args };
        }

        public static OwinNode operator *(GraphNode node, OwinNodeFuture future)
        {
            var owinNode = new OwinNode(node);
            owinNode.Middleware.Add(future.middleware);
            
            return owinNode;
        }

        public static OwinNode operator *(OwinNode owinNode, OwinNodeFuture future)
        {
            owinNode.Middleware.Add(future.middleware);
            return owinNode;
        }
    }
}
