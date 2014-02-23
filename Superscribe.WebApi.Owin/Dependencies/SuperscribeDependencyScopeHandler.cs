namespace Superscribe.WebApi.Owin.Dependencies
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Hosting;

    using Superscribe.WebApi.Owin.Extensions;

    public class SuperscribeDependencyScopeHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var existingScope = request.GetDependencyScope();
            var dependencyScope = request.GetSuperscribeDependencyScope(existingScope);

            request.Properties[HttpPropertyKeys.DependencyScope] = dependencyScope;
            request.RegisterForDispose(dependencyScope);

            return base.SendAsync(request, cancellationToken);
        }
    }
}