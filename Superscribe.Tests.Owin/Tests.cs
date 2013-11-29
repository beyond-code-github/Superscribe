namespace Superscribe.Tests.Owin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Machine.Specifications;

    using Microsoft.Owin.Testing;

    using Superscribe.Owin;
    using Superscribe.Owin.Extensions;

    public class TestMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        private readonly string value;

        public TestMiddleware(Func<IDictionary<string, object>, Task> next, string value)
        {
            this.next = next;
            this.value = value;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            environment.TrySetHeaderValues(this.value, new[] { this.value });
            await this.next(environment);
        }
    }

    public class Tests
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

                        ʃ.Route(ʅ => o => "Hello World");

                        builder.Use(typeof(TestMiddleware), "before")
                            .Use(typeof(OwinRouter), builder, config)
                            .Use(typeof(TestMiddleware), "after")
                            .Use(typeof(OwinHandler), config);
                    });

                client = owinTestServer.HttpClient;
                client.DefaultRequestHeaders.Add("accept", "text/html");
            };

        private Because of = () =>
                responseMessage = client.GetAsync("http://localhost/").Result;

        private It should_add_the_before_header =
            () => responseMessage.Headers.FirstOrDefault(o => o.Key == "before").Value.ShouldContain("before");

        private It should_set_the_response =
           () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("Hello World");

        private It should_add_the_after_header =
            () => responseMessage.Headers.FirstOrDefault(o => o.Key == "after").Value.ShouldContain("after");
    }
}
