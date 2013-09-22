namespace Superscribe.Owin
{
    using global::Owin.Types;

    using Superscribe.Models;

    public class OwinRouteData : IRouteData
    {
        public dynamic Parameters { get; set; }
        
        public object Response { get; set; }

        public OwinRequest OwinRequest { get; set; }
        
        public OwinResponse OwinRespose { get; set; }
    }
}
