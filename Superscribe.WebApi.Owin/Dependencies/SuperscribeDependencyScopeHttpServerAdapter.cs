namespace Superscribe.WebApi.Owin.Dependencies
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Hosting;

    using Superscribe.WebApi.Owin.Extensions;

    public class SuperscribeDependencyScopeHttpServerAdapter : HttpServer
    {
        public SuperscribeDependencyScopeHttpServerAdapter(HttpConfiguration configuration)
            : base(configuration)
        {
        }

        public SuperscribeDependencyScopeHttpServerAdapter(HttpConfiguration configuration, HttpMessageHandler dispatcher)
            : base(configuration, dispatcher)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Note: no need to call request.RegisterForDispose as AutofacMiddleware will dispose the ILifetimeScope instance.
            var existingScope = this.Configuration.DependencyResolver.BeginScope();
            var dependencyScope = request.GetSuperscribeDependencyScope(existingScope);

            request.Properties[HttpPropertyKeys.DependencyScope] = dependencyScope;
            request.RegisterForDispose(dependencyScope);

            return base.SendAsync(request, cancellationToken);
        }
    }
}