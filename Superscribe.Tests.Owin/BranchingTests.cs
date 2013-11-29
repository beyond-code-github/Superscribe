namespace Superscribe.Tests.Owin
{
    using System.IO;
    using System.Linq;
    using System.Net.Http;

    using Machine.Specifications;

    using Microsoft.Owin.Testing;

    using Superscribe.Owin;
    using Superscribe.Owin.Extensions;
    using Superscribe.Owin.Models;

    public class BranchingTests
    {
        protected static TestServer owinTestServer;

        protected static HttpResponseMessage responseMessage;

        protected static HttpClient client;

        protected Establish context = () =>
        {
            owinTestServer = TestServer.Create(
                builder =>
                    {
                        var config = new SuperscribeOwinConfig();
                        config.MediaTypeHandlers.Add(
                            "text/html",
                            new MediaTypeHandler
                            {
                                Read = (env, o) =>
                                {
                                    using (var reader = new StreamReader(env.GetRequestBody())) return reader.ReadToEnd();
                                },
                                Write = (env, o) => env.WriteResponse(o.ToString())
                            });

                        ʃ.Route(ʅ => ʅ / "Hello"
                            * Middleware.Use<TestMiddleware>("before") 
                            * (o => "Hello World"));

                        builder.Use(typeof(OwinRouter), builder, config);
                    });

            client = owinTestServer.HttpClient;
            client.DefaultRequestHeaders.Add("accept", "text/html");
        };

        private Because of = () =>
                responseMessage = client.GetAsync("http://localhost/Hello").Result;

        private It should_add_the_before_header =
            () => responseMessage.Headers.FirstOrDefault(o => o.Key == "before").Value.ShouldContain("before");

        private It should_set_the_response =
           () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("Hello World");
    }
}
