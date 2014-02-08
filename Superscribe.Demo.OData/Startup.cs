namespace Superscribe.Demo.OData
{
    using System.IO;

    using global::Owin;

    using Newtonsoft.Json;

    using Owin;

    using Superscribe.Owin.Extensions;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinConfig();
            SetupMediaTypes(config);

            Define.Route("/", o => "Hello World");

            app.UseSuperscribeRouter(config).UseSuperscribeHandler(config);
        }

        private static void SetupMediaTypes(SuperscribeOwinConfig config)
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