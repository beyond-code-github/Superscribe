namespace Superscribe.Owin.Components
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Superscribe.Owin.Engine;
    using Superscribe.Owin.Extensions;

    public class OwinHandler
    {
        private readonly IOwinRouteEngine engine;

        public OwinHandler(Func<IDictionary<string, object>, Task> next, IOwinRouteEngine engine)
        {
            this.engine = engine;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var routeData = environment["superscribe.RouteData"] as OwinRouteData;
            Debug.Assert(routeData != null, "routeData != null");

            if (routeData.IncompleteMatch)
            {
                environment.SetResponseStatusCode(404);
                environment.WriteResponse("404 - Route was incomplete");
                return;
            }

            if (routeData.ExtraneousMatch)
            {
                environment.SetResponseStatusCode(404);
                environment.WriteResponse("404 - Route match failed");
                return;
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
                var mediaType = mediaTypes.FirstOrDefault(o => this.engine.Config.MediaTypeHandlers.Keys.Contains(o) && this.engine.Config.MediaTypeHandlers[o].Write != null);
                if (!string.IsNullOrEmpty(mediaType))
                {
                    var formatter = this.engine.Config.MediaTypeHandlers[mediaType];
                    environment.SetResponseContentType(mediaType);

                    await formatter.Write(environment, routeData.Response);

                    return;
                }

                throw new NotSupportedException("Media type is not supported");
            }

            throw new NotSupportedException("Response type is not supported");
        }
    }
}
