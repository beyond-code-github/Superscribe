namespace Superscribe.WebApi.Owin.Extensions
{
    using System.Net.Http;
    using System.Web.Http.Dependencies;

    using Superscribe.Engine;
    using Superscribe.Owin;
    using Superscribe.WebApi.Dependencies;

    public static class RequestExtensions
    {
        public static IDependencyScope GetSuperscribeDependencyScope(this HttpRequestMessage request, IDependencyScope existingScope)
        {
            var routeWalker = request.GetOwinContext().Environment[Constants.SuperscribeRouteWalkerEnvironmentKey] as IRouteWalker;
            var routeDataProvider = request.GetOwinContext().Environment[Constants.SuperscribeRouteDataProviderEnvironmentKey] as IRouteDataProvider;

            return new SuperscribeDependencyScopeAdapter(existingScope, routeWalker, routeDataProvider);
        }
    }
}
