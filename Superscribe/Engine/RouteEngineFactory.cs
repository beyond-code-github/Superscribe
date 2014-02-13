namespace Superscribe.Engine
{
    public class RouteEngineFactory
    {
        public static IRouteEngine Create(SuperscribeOptions options)
        {
            var engine = new RouteEngine(options.StringRouteParser, options.RouteWalkerFactory);
            return engine;
        }

        public static IRouteEngine Create()
        {
            return Create(new SuperscribeOptions());
        }
    }
}
