namespace Superscribe.Samples.FluentApi
{
    using System.IO;

    using global::Owin;

    using Newtonsoft.Json;

    using Superscribe.Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinConfig();
            config.MediaTypeHandlers.Add(
                "application/json",
                new MediaTypeHandler
                    {
                        Write = (res, o) => res.WriteAsync(JsonConvert.SerializeObject(o)),
                        Read = (req, type) =>
                            {
                                object obj;
                                using (var reader = new StreamReader(req.Body))
                                {
                                    obj = JsonConvert.DeserializeObject(reader.ReadToEnd(), type);
                                };

                                return obj;
                            }
                    });

            config.MediaTypeHandlers.Add(
                "text/html",
                new MediaTypeHandler
                    {
                        Write = (res, o) => res.WriteAsync(o.ToString())
                    });

            app.UseSuperscribeModules(config);
        }
    }
}