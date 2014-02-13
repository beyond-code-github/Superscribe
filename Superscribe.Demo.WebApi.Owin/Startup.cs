namespace Superscribe.Demo.WebApi.Owin
{
    using System.Web.Http;

    using global::Owin;

    using Superscribe.Owin;
    using Superscribe.Owin.Engine;
    using Superscribe.Owin.Extensions;
    using Superscribe.WebApi;
    using Superscribe.WebApi.Owin.Extensions;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinOptions();
            var engine = OwinRouteEngineFactory.Create(config);

            var httpconfig = new HttpConfiguration();
            SuperscribeConfig.Register(httpconfig, engine);

            engine.Route(o => "Values".Controller());      

            app.UseSuperscribeRouter(engine)
                .UseWebApiWithSuperscribe(httpconfig, engine);
        }
    }
}