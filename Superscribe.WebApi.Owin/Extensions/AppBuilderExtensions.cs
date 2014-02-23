namespace Superscribe.WebApi.Owin.Extensions
{
    using System.Web.Http;

    using global::Owin;

    using Superscribe.Engine;
    using Superscribe.WebApi.Dependencies;
    using Superscribe.WebApi.Owin.Dependencies;

    public static class ContextBuilderExtensions
    {
        public static IAppBuilder WithSuperscribe(this IAppBuilder builder, HttpConfiguration configuration, IRouteEngine engine)
        {
            configuration.DependencyResolver = new SuperscribeDependencyAdapter(configuration.DependencyResolver, engine);
            configuration.MessageHandlers.Insert(0, new SuperscribeDependencyScopeHandler());

            return builder;
        }
    }
}
