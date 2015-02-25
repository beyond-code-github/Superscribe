using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Superscribe.Engine;
using Superscribe.Extensions;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Superscribe.Components
{
    public class OwinHandler
    {
        private readonly IOwinRouteEngine engine;

        public OwinHandler(AppFunc next, IOwinRouteEngine engine)
        {
            this.engine = engine;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var routeData = environment["superscribe.RouteData"] as OwinRouteData;
            Debug.Assert(routeData != null, "routeData != null");

            if (routeData.ExtraneousMatch)
            {
                environment.SetResponseStatusCode(404);
                environment.WriteResponse("404 - Route match failed");
                return;
            }

            if (routeData.NoMatchingFinalFunction)
            {
                environment.SetResponseStatusCode(405);
                environment.WriteResponse("405 - No final function was configured for this method");
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
