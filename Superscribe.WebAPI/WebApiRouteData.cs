namespace Superscribe.WebApi
{
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Security.Principal;
    using System.Web.Http.Routing;

    using Superscribe.Engine;
    using Superscribe.Utils;

    public class WebApiRouteData : RouteData, IModuleRouteData
    {
        public WebApiRouteData()
        {
            this.Parameters = new DynamicDictionary();
        }

        public HttpRequestMessage Request { get; set; }

        public UrlHelper Url { get; set; }

        public IPrincipal User { get; set; }

        public T Bind<T>() where T : class
        {
            var configuraton = this.Request.GetConfiguration();
            var conneg = (IContentNegotiator)configuraton.Services.GetService(typeof(IContentNegotiator));
            var formatter = conneg.Negotiate(
                typeof(T),
                this.Request,
                configuraton.Formatters);

            var stream = this.Request.Content.ReadAsStreamAsync().Result;

            return formatter.Formatter.ReadFromStreamAsync(
                typeof(T),
                stream,
                this.Request.Content,
                null).Result as T;
        }

        public T Require<T>() where T : class
        {
            return this.Request.GetConfiguration().DependencyResolver.GetService(typeof(T)) as T;
        }
    }
}
