using Superscribe.Models;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Superscribe.Pipelining
{
    using MidFunc = System.Func<AppFunc, AppFunc>;

    public class OwinNodeFuture
    {
        private readonly MidFunc middleware;

        public OwinNodeFuture(MidFunc middleware)
        {
            this.middleware = middleware;
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
