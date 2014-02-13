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

            engine.Route(o => "Values".Controller());
            
            var httpconfig = new HttpConfiguration();
            SuperscribeConfig.Register(httpconfig);
            
            app.UseSuperscribeRouter(engine)
                .UseWebApiWithSuperscribe(httpconfig, engine);
        }
    }
}