namespace Superscribe.WebApi.Engine
{
    using System.Net.Http;

    using Superscribe.Engine;

    public class WebApiRouteDataProviderAdapter : IWebApiRouteDataProvider
    {
        private readonly IRouteDataProvider routeDataProvider;

        public WebApiRouteDataProviderAdapter(IRouteDataProvider routeDataProvider)
        {
            this.routeDataProvider = routeDataProvider;
        }

        public IRouteData GetData(HttpRequestMessage request)
        {
            var routeData = this.routeDataProvider.GetData(
                request.RequestUri.PathAndQuery,
                request.Method.ToString(),
                () => new WebApiRouteData { Request = request });

            return routeData;
        }
    }
}
