namespace Superscribe.Owin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Superscribe.Engine;
    using Superscribe.Owin.Extensions;
    using Superscribe.Owin.Pipelining;
    using Superscribe.Utils;

    public class OwinRouteData : RouteData, IModuleRouteData
    {
        public OwinRouteData()
        {
            this.Parameters = new DynamicDictionary();
            this.Pipeline = new List<Middleware>();
        }
        
        public int StatusCode { get; set; }
        
        public SuperscribeOwinOptions Config { get; set; }

        public List<Middleware> Pipeline { get; set; }

        public T Bind<T>() where T : class
        {
            string[] incomingMediaTypes;
            if (this.Environment.TryGetHeaderValues("content-type", out incomingMediaTypes))
            {
                var mediaTypes = ConnegHelpers.GetWeightedValues(incomingMediaTypes);
                var mediaType = mediaTypes.FirstOrDefault(o => this.Config.MediaTypeHandlers.Keys.Contains(o) && this.Config.MediaTypeHandlers[o].Read != null);
                if (!string.IsNullOrEmpty(mediaType))
                {
                    var formatter = this.Config.MediaTypeHandlers[mediaType];
                    return formatter.Read(this.Environment, typeof(T)) as T;
                }
                        
                throw new NotSupportedException("Media type is not supported");
            }

            throw new NotSupportedException("No content-type was found on the request");
        }

        public T Require<T>() where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}
