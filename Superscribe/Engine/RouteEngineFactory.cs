namespace Superscribe.Engine
{
    public class RouteEngineFactory
    {
        public static IRouteEngine Create(SuperscribeOptions options)
        {
            var engine = new RouteEngine(options);
            return engine;
        }

        public static IRouteEngine Create()
        {
            return Create(new SuperscribeOptions());
        }
    }
}
