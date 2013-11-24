namespace Superscribe.Tests.Owin
{
    using System.Linq;
    using System.Net.Http;

    using Machine.Specifications;

    using Microsoft.Owin.Testing;

    public class Tests
    {
        protected static TestServer owinTestServer;

        protected static HttpResponseMessage responseMessage;

        protected static HttpClient client;

        protected Establish context = () =>
            {
                owinTestServer = TestServer.Create(builder => new Startup().Configuration(builder));

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
