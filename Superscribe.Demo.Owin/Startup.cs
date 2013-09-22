namespace Superscribe.Demo.Owin
{
    using Newtonsoft.Json;

    using global::Owin;

    using Superscribe.Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinConfig();
            config.ContentHandlers.Add("application/json", (res, o) => res.WriteAsync(JsonConvert.SerializeObject(o)));
            config.ContentHandlers.Add("text/html", (res, o) => res.WriteAsync(o.ToString()));

            app.UseSuperscribeModules(config);
        }
    }
}