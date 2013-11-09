namespace Superscribe.Owin
{
    using System;
    using System.Linq;
    
    using Microsoft.Owin;

    using Superscribe.Models;

    public class OwinRouteData : IRouteData
    {
        public dynamic Parameters { get; set; }

        public object Response { get; set; }

        public int StatusCode { get; set; }

        public IOwinRequest OwinRequest { get; set; }

        public OwinResponse OwinResponse { get; set; }

        public SuperscribeOwinConfig Config { get; set; }
        
        public T Bind<T>() where T : class
        {
            string[] incomingMediaTypes;
            if (this.OwinRequest.Headers.TryGetValue("content-type", out incomingMediaTypes))
            {
                var mediaTypes = ConnegHelpers.GetWeightedValues(incomingMediaTypes);
                var mediaType = mediaTypes.FirstOrDefault(o => this.Config.MediaTypeHandlers.Keys.Contains(o) && this.Config.MediaTypeHandlers[o].Read != null);
                if (!string.IsNullOrEmpty(mediaType))
                {
                    var formatter = this.Config.MediaTypeHandlers[mediaType];
                    return formatter.Read(this.OwinRequest, typeof(T)) as T;
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
