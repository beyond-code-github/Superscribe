namespace Superscribe.Owin.Engine
{
    using Superscribe.Engine;

    public class OwinRouteEngine : RouteEngine, IOwinRouteEngine
    {
        public OwinRouteEngine(SuperscribeOwinOptions options)
            : base(options.StringRouteParser, options.RouteWalkerFactory)
        {
            this.Config = options;
        }
        
        public SuperscribeOwinOptions Config { get; private set; }
    }
}
