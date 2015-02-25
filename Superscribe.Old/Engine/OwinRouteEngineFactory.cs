namespace Superscribe.Owin.Engine
{
    public class OwinRouteEngineFactory
    {
        public static IOwinRouteEngine Create()
        {
            var engine = new OwinRouteEngine(new SuperscribeOwinOptions());
            return engine;
        }

        public static IOwinRouteEngine Create(SuperscribeOwinOptions options)
        {
            var engine = new OwinRouteEngine(options);
            return engine;
        }
    }
}
