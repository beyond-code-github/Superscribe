namespace Superscribe.Owin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Owin;

    using Superscribe.Owin.Extensions;
    using Superscribe.Utils;

    public class OwinRouter
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        private readonly IAppBuilder builder;

        private readonly SuperscribeOwinConfig config;

        public OwinRouter(Func<IDictionary<string, object>, Task> next, IAppBuilder builder, SuperscribeOwinConfig config)
        {
            this.next = next;
            this.builder = builder;
            this.config = config;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var path = environment["owin.RequestPath"].ToString();
            var method = environment["owin.RequestMethod"].ToString();

            var routeData = new OwinRouteData { Builder = builder, Environment = environment, Config = config };

            var walker = new RouteWalker<OwinRouteData>(ʃ.Base);
            walker.WalkRoute(path, method, routeData);
            
            // Default status code to 200
            environment.SetResponseStatusCode(200);

            if (walker.IncompleteMatch)
            {
                environment.SetResponseStatusCode(404);
                environment.WriteResponse("404 - Route was incomplete").RunSynchronously();
                return;
            }

            if (walker.ExtraneousMatch)
            {
                environment.SetResponseStatusCode(404);
                environment.WriteResponse("404 - Route match failed").RunSynchronously(); ;
                return;
            }

            var responseTask = routeData.Response as Task;
            if (responseTask != null)
            {
                await responseTask;
                await next.Invoke(environment);
            }
            
            // Set status code
            if (routeData.StatusCode > 0)
            {
                environment.SetResponseStatusCode(routeData.StatusCode);
            }

            string[] outgoingMediaTypes;
            if (environment.TryGetHeaderValues("accept", out outgoingMediaTypes))
            {
                var mediaTypes = ConnegHelpers.GetWeightedValues(outgoingMediaTypes);
                var mediaType = mediaTypes.FirstOrDefault(o => config.MediaTypeHandlers.Keys.Contains(o) && config.MediaTypeHandlers[o].Write != null);
                if (!string.IsNullOrEmpty(mediaType))
                {
                    var formatter = config.MediaTypeHandlers[mediaType];
                    environment.SetResponseContentType(mediaType);

                    await formatter.Write(environment, routeData.Response);
                    await next.Invoke(environment);

                    return;
                }

                throw new NotSupportedException("Media type is not supported");
            }

            throw new NotSupportedException("Response type is not supported");

        }
    }
}
