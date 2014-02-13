namespace Superscribe.Owin.Engine
{
    public class OwinRouteEngineFactory
    {
        public static IOwinRouteEngine Create(SuperscribeOwinOptions options)
        {
            var engine = new OwinRouteEngine(options);
            return engine;
        }

    }
}
