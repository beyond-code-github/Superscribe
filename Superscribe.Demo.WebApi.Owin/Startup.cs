namespace Superscribe.Demo.WebApi.Owin
{
    using System.Web.Http;

    using global::Owin;

    using Superscribe.Owin.Engine;
    using Superscribe.Owin.Extensions;
    using Superscribe.WebApi;
    using Superscribe.WebApi.Owin.Extensions;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var engine = OwinRouteEngineFactory.Create();

            var httpconfig = new HttpConfiguration();
            SuperscribeConfig.Register(httpconfig, engine);

            engine.Route(o => "Values".Controller());      
            
            app.UseSuperscribeRouter(engine)
                .UseWebApi(httpconfig)
                .WithSuperscribe(httpconfig, engine);
        }
    }
}