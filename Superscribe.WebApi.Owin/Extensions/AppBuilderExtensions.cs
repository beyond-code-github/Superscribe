namespace Superscribe.WebApi.Owin.Extensions
{
    using System.Web.Http;

    using global::Owin;

    using Superscribe.Engine;
    using Superscribe.WebApi.Dependencies;
    using Superscribe.WebApi.Owin.Dependencies;

    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseWebApiWithSuperscribe(this IAppBuilder app, HttpConfiguration configuration, IRouteEngine engine)
        {
            configuration.DependencyResolver = new SuperscribeDependencyAdapter(configuration.DependencyResolver, engine);
            HttpServer httpServer = new SuperscribeDependencyScopeHttpServerAdapter(configuration);
            return app.UseWebApi(httpServer);
        }
    }
}
