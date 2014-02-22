namespace Superscribe.Owin
{
    using System.Collections.Generic;
    using System.IO;

    using Superscribe.Owin.Extensions;

    public class SuperscribeOwinOptions : SuperscribeOptions
    {
        public SuperscribeOwinOptions()
        {
            this.MediaTypeHandlers = new Dictionary<string, MediaTypeHandler> {
                {
                    "text/html", 
                    new MediaTypeHandler
                    {
                        Read = (env, o) =>
                        {
                            using (var reader = new StreamReader(env.GetRequestBody())) return reader.ReadToEnd();
                        },
                        Write = (env, o) => env.WriteResponse(o.ToString())
                    } 
                } 
            };

            this.ScanForModules = true;
        }

        public Dictionary<string, MediaTypeHandler> MediaTypeHandlers { get; set; }

        public bool ScanForModules { get; set; }
    }
}
