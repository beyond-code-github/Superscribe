namespace Superscribe.Owin.Engine
{
    using Superscribe.Engine;

    public interface IOwinRouteEngine : IRouteEngine
    {
        SuperscribeOwinOptions Config { get; }
    }
}
