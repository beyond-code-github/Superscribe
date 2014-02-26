namespace Superscribe.Tests.WebApi.Owin
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;

    using global::Owin;

    using Machine.Specifications;

    using Microsoft.Owin.Testing;

    using Superscribe.Engine;
    using Superscribe.Models;
    using Superscribe.Owin;
    using Superscribe.Owin.Engine;
    using Superscribe.Owin.Extensions;
    using Superscribe.WebApi;
    using Superscribe.WebApi.Owin.Extensions;

    using Constants = Superscribe.WebApi.Constants;

    public class Tests
    {
        public static object SwitchActionForApp(dynamic routeData)
        {
            routeData.Environment[Constants.ActionNamePropertyKey] = "GetByIdentifier";
            return new FinalFunction.ExecuteAndContinue();
        }

        protected static TestServer owinTestServer;

        protected static HttpResponseMessage responseMessage;

        protected static HttpClient client;

        protected static IOwinRouteEngine engine;

        protected Establish context = () =>
        {
            owinTestServer = TestServer.Create(
                builder =>
                {
                    var config = new SuperscribeOwinOptions();
                    
                    engine = OwinRouteEngineFactory.Create(config);
                    engine.Route(r => r / "api" / "Apps".Controller() / (String)"appIdentifier", SwitchActionForApp);

                    var httpConfig = new HttpConfiguration();
                    SuperscribeConfig.Register(httpConfig, engine);

                    builder.UseSuperscribeRouter(engine);
                    builder.UseWebApi(httpConfig).WithSuperscribe(httpConfig, engine);
                });



            client = owinTestServer.HttpClient;
            client.DefaultRequestHeaders.Add("accept", "text/html");
        };

        private Because of = () =>
                responseMessage = client.GetAsync("http://localhost/api/apps/projectSPRINT").Await();
        
        private It should_set_the_response =
           () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("\"Get by identifier: projectSPRINT\"");
    }
}
