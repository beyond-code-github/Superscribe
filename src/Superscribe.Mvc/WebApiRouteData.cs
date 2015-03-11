using System.IO;
using System.Net.Http.Formatting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Mvc.WebApiCompatShim;

namespace Superscribe.WebApi
{
    using System.Security.Principal;
    using Superscribe.Engine;
    using Superscribe.Utils;

    public class WebApiRouteData : RouteData, IModuleRouteData
    {
        public WebApiRouteData()
        {
            this.Parameters = new DynamicDictionary();
        }

        public HttpContext Context { get; set; }

        public UrlHelper Url { get; set; }

        public IPrincipal User { get; set; }

        public T Bind<T>() where T : class
        {
            this.Context.ApplicationServices
            var conneg = this.Context.RequestServices.GetRequiredService<IContentNegotiator>();
            var formatter = conneg.Negotiate(
                typeof(T),
                this.Context.Request,
                configuraton.Formatters);
            
            var stream = this.Context.Request.Body;

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
