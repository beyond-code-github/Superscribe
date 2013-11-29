namespace Superscribe.Owin
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Superscribe.Owin.Extensions;

    public class OwinHandler
    { 
        private readonly SuperscribeOwinConfig config;

        public OwinHandler(Func<IDictionary<string, object>, Task> next, SuperscribeOwinConfig config)
        {
            this.config = config;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var routeData = environment["superscribe.RouteData"] as OwinRouteData;
            Debug.Assert(routeData != null, "routeData != null");

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

                    return;
                }

                throw new NotSupportedException("Media type is not supported");
            }

            throw new NotSupportedException("Response type is not supported");
        }
    }
}
