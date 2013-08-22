namespace Superscribe.Owin
{
    using Superscribe.Models;

    using global::Owin.Types;

    public class OwinRouteData : RouteData
    {
        public OwinRequest Request { get; set; }

        public OwinResponse Response { get; set; }
    }
}
