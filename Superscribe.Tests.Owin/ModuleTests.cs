namespace Superscribe.Tests.Owin
{
    using System.IO;
    using System.Net;
    using System.Net.Http;

    using Machine.Specifications;

    using Microsoft.Owin.Testing;

    using Superscribe.Owin;
    using Superscribe.Owin.Extensions;
    
    public class Module : SuperscribeOwinModule
    {
        public Module()
        {
            this.Get["Hello"] = o => "Hello World";
        }
    }

    public abstract class ModuleTests
    {
        protected static TestServer owinTestServer;

        protected static HttpResponseMessage responseMessage;

        protected static HttpClient client;

        protected Establish context = () =>
        {
            ʃ.Reset();
            owinTestServer = TestServer.Create(
                builder =>
                {
                    var config = new SuperscribeOwinConfig();
                    config.MediaTypeHandlers.Add(
                        "text/html",
                        new MediaTypeHandler
                        {
                            Read = (env, o) => { using (var reader = new StreamReader(env.GetRequestBody())) return reader.ReadToEnd(); },
                            Write = (env, o) => env.WriteResponse(o.ToString())
                        });

                    builder.UseSuperscribeRouter(config)
                        .UseSuperscribeHandler(config);
                });

            client = owinTestServer.HttpClient;
            client.DefaultRequestHeaders.Add("accept", "text/html");
        };
    }

    public class When_scanning_for_modules : ModuleTests
    {
        private Because of = () => responseMessage = client.GetAsync("http://localhost/Hello").Result;

        private It should_wire_up_get_handlers =
            () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("Hello World");
    }

    public class When_hitting_a_route_that_is_incomplete: ModuleTests
    {
        private Because of = () => responseMessage = client.GetAsync("http://localhost/NotDefined").Result;

        private It should_return_a_404 =
            () => responseMessage.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
    }

    public class When_hitting_an_extraneous_route : ModuleTests
    {
        private Because of = () => responseMessage = client.GetAsync("http://localhost/Hello/More").Result;

        private It should_return_a_404 =
            () => responseMessage.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
    }
}
