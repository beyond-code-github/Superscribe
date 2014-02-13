namespace Superscribe.Tests.OwinFrameworkHandoff
{
    using System.IO;
    using System.Net.Http;
    using System.Web.Http;

    using global::Owin;

    using Machine.Specifications;

    using Microsoft.Owin.Testing;

    using Superscribe.Engine;
    using Superscribe.Owin;
    using Superscribe.Owin.Engine;
    using Superscribe.Owin.Extensions;
    using Superscribe.Owin.Pipelining;

    public abstract class FrameworkTests
    {
        protected static TestServer owinTestServer;

        protected static HttpResponseMessage responseMessage;

        protected static HttpClient client;

        protected Establish context = () =>
        {
            owinTestServer = TestServer.Create(
                builder =>
                {
                    var httpconfig = new HttpConfiguration();
                    httpconfig.Routes.MapHttpRoute(
                        name: "DefaultApi",
                        routeTemplate: "api/webapi/",
                        defaults: new { controller = "Hello" }
                    );

                    var engine = OwinRouteEngineFactory.Create(new SuperscribeOwinOptions());
                    builder.UseSuperscribeRouter(engine);

                    // Set up a route that will respond only to even numbers using the fluent api
                    engine.Route(ʅ => ʅ / "api" / (
                          ʅ / "webapi" * Pipeline.Action(o => o.UseWebApi(httpconfig))
                        | ʅ / "nancy" * Pipeline.Action(o => o.UseNancy())));
                });

            client = owinTestServer.HttpClient;
            client.DefaultRequestHeaders.Add("accept", "text/html");
        };
    }

    public class When_hitting_the_webapi_route : FrameworkTests
    {
        private Because of = () => responseMessage = client.GetAsync("http://localhost/api/webapi").Result;

        private It should_wire_up_get_handlers =
            () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("\"Hello from Web API\"");
    }

    public class When_hitting_the_nancy_route : FrameworkTests
    {
        private Because of = () => responseMessage = client.GetAsync("http://localhost/api/nancy").Result;

        private It should_wire_up_get_handlers =
            () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("Hello from Nancy");
    }
}