namespace Superscribe.WebApi.Engine
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using Superscribe.Engine;
    using Superscribe.Utils;

    public class WebApiRouteDataProviderAdapter : IWebApiRouteDataProvider
    {
        private readonly IRouteDataProvider routeDataProvider;

        public WebApiRouteDataProviderAdapter(IRouteDataProvider routeDataProvider)
        {
            this.routeDataProvider = routeDataProvider;
        }

        public IRouteData GetData(HttpRequestMessage request)
        {
            var environment = new Dictionary<string, object>();

            var route = request.RequestUri.PathAndQuery;
            var method = request.Method.ToString();

            var accepts = request.Headers.Accept.Select(o => o.MediaType).ToArray();
            var contentType = new string[] { };

            if (request.Content != null && request.Content.Headers.ContentType != null)
            {
                contentType = new[] { request.Content.Headers.ContentType.MediaType };
            }

            string querystring;
            string path;
            route.SplitPathAndQuery(out path, out querystring);

            environment[Superscribe.Constants.RequestPathEnvironmentKey] = path;
            environment[Superscribe.Constants.RequestQuerystringEnvironmentKey] = querystring;
            environment[Superscribe.Constants.RequestMethodEnvironmentKey] = method;
            environment[Superscribe.Constants.AcceptsEnvironmentKey] = accepts;
            environment[Superscribe.Constants.ContentTypeEnvironmentKey] = contentType;

            var routeData = this.routeDataProvider.GetData(
                environment,
                () => new WebApiRouteData { Request = request });

            return routeData;
        }
    }
}
