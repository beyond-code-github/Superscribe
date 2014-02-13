namespace Superscribe.Demo.OData
{
    using System.IO;

    using global::Owin;

    using Newtonsoft.Json;

    using Owin;

    using Superscribe.Owin.Engine;
    using Superscribe.Owin.Extensions;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new SuperscribeOwinOptions();
            SetupMediaTypes(options);

            var define = OwinRouteEngineFactory.Create(options);
            define.Route("/", o => "Hello World");

            app.UseSuperscribeRouter(define).UseSuperscribeHandler(define);
        }

        private static void SetupMediaTypes(SuperscribeOwinOptions config)
        {
            config.MediaTypeHandlers.Add(
                "application/json",
                new MediaTypeHandler
                    {
                        Write = (env, o) => env.WriteResponse(JsonConvert.SerializeObject(o)),
                        Read = (env, type) =>
                            {
                                object obj;
                                using (var reader = new StreamReader(env.GetRequestBody()))
                                {
                                    obj = JsonConvert.DeserializeObject(reader.ReadToEnd(), type);
                                }
                                ;

                                return obj;
                            }
                    });

            config.MediaTypeHandlers.Add(
                "text/html",
                new MediaTypeHandler { Write = (env, o) => env.WriteResponse(o.ToString()) });
        }
    }
}