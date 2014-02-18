namespace Superscribe.Demo.Owin
{
    using System.IO;

    using Newtonsoft.Json;

    using global::Owin;

    using Superscribe.Owin;
    using Superscribe.Owin.Engine;
    using Superscribe.Owin.Extensions;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinOptions();

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
                                };

                                return obj;
                            }
                    });

            var engine = OwinRouteEngineFactory.Create(config);
            
            app.UseSuperscribeRouter(engine)
                .UseSuperscribeHandler(engine);
        }
    }
}