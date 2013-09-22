namespace Superscribe.Demo.Owin
{
    using System.IO;

    using Newtonsoft.Json;

    using global::Owin;

    using Superscribe.Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinConfig();
            config.ContentHandlers.Add(
                "application/json",
                new ContentHandler
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

            config.ContentHandlers.Add(
                "text/html",
                new ContentHandler
                    {
                        Write = (res, o) => res.WriteAsync(o.ToString())
                    });

            app.UseSuperscribeModules(config);
        }
    }
}