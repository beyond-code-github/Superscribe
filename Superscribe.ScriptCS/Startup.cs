namespace Superscribe.ScriptCS
{
    using System.IO;

    using global::Owin;

    using global::Superscribe.Owin;
    using global::Superscribe.Owin.Extensions;

    using Newtonsoft.Json;

    using Superscribe.Owin.Engine;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinOptions();
            SetupMediaTypes(config);

            var engine = OwinRouteEngineFactory.Create(config);

            app.UseSuperscribeRouter(engine).UseSuperscribeHandler(engine);
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
