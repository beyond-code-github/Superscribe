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
        private AppFunc _next;

        private readonly IOwinRouteEngine _engine;

        public OwinHandler(IOwinRouteEngine engine)
        {
            _engine = engine;
        }

        public AppFunc Compose(AppFunc nextApplication)
        {
            _next = nextApplication;
            return Invoke;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var routeData = environment["superscribe.RouteData"] as OwinRouteData;
            Debug.Assert(routeData != null, "routeData != null");

            if (routeData.ExtraneousMatch)
            {
                environment.SetResponseStatusCode(404);
                await environment.WriteResponse("404 - Route match failed");
                return;
            }

            if (routeData.NoMatchingFinalFunction)
            {
                environment.SetResponseStatusCode(405);
                await environment.WriteResponse("405 - No final function was configured for this method");
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
                var mediaType = mediaTypes.FirstOrDefault(o => _engine.Config.MediaTypeHandlers.Keys.Contains(o) && _engine.Config.MediaTypeHandlers[o].Write != null);
                if (!string.IsNullOrEmpty(mediaType))
                {
                    var formatter = _engine.Config.MediaTypeHandlers[mediaType];
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
