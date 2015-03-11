using Superscribe.Owin;
using Xunit;

namespace Superscribe.Tests.Owin
{
    using System.IO;
    using System.Net;
    using System.Net.Http;

    using Microsoft.AspNet.TestHost;
    using Newtonsoft.Json;

    using Superscribe;
    using Superscribe.Engine;
    using Superscribe.Extensions;

    public class ModuleTestsFixture
    {
        public TestServer OwinTestServer;
        
        public ModuleTestsFixture()
        {
            OwinTestServer = TestServer.Create(
            builder =>
            {
                var config = new SuperscribeOwinOptions();
                config.MediaTypeHandlers.Add("application/json", new MediaTypeHandler
                {
                    Write = (env, o) => env.WriteResponse(JsonConvert.SerializeObject(o)),
                    Read = (env, type) =>
                    {
                        object obj;
                        using (var reader = new StreamReader(env.GetRequestBody()))
                        {
                            obj = JsonConvert.DeserializeObject(reader.ReadToEnd(), type);
                        };

                        return obj;
                    }
                });

                var engine = OwinRouteEngineFactory.Create(config);

                builder.UseSuperscribeRouter(engine)
                    .UseSuperscribeHandler(engine);
            });
        }
    }

    public class ModuleTests : IClassFixture<ModuleTestsFixture>
    {
        private readonly ModuleTestsFixture _moduleTestsFixture;

        private readonly HttpClient _client;

        public ModuleTests(ModuleTestsFixture moduleTestsFixture)
        {
            _moduleTestsFixture = moduleTestsFixture;
            _client = _moduleTestsFixture.OwinTestServer.CreateClient();
            _client.DefaultRequestHeaders.Add("accept", "text/html");
        }

        [Fact]
        public async void When_scanning_for_modules_it_should_wire_up_get_handlers()
        {
            var responseMessage = await _client.GetAsync("http://localhost/Hello");
            var content = await responseMessage.Content.ReadAsStringAsync();
            Assert.Equal("Hello World", content);
        }

        [Fact]
        public async void When_hitting_a_route_that_is_incomplete_it_should_return_a_404()
        {
            var responseMessage = await _client.GetAsync("http://localhost/NotDefined");
            Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
        }

        [Fact]
        public async void When_hitting_an_extraneous_route_it_should_return_a_404()
        {
            var responseMessage = await _client.GetAsync("http://localhost/Hello/More");
            Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
        }
    }

    public class JsonTests : IClassFixture<ModuleTestsFixture>
    {
        private readonly ModuleTestsFixture _moduleTestsFixture;

        private readonly HttpClient _client;

        public JsonTests(ModuleTestsFixture moduleTestsFixture)
        {
            _moduleTestsFixture = moduleTestsFixture;
            _client = _moduleTestsFixture.OwinTestServer.CreateClient();
            _client.DefaultRequestHeaders.Add("accept", "application / json");
        }

        [Fact]
        public async void When_getting_products_it_should_return_a_200()
        {
            var responseMessage = await _client.GetAsync("http://localhost/Products");
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Fact]
        public async void When_getting_products_by_id_it_should_return_a_200()
        {
            var responseMessage = await _client.GetAsync("http://localhost/Products/1");
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Fact]
        public async void When_getting_products_by_category_it_should_return_a_200()
        {
            var responseMessage = await _client.GetAsync("http://localhost/Products/Fashion");
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Fact]
        public async void When_posting_a_product_it_should_return_a_201()
        {
            var product = new Product { Id = 6, Category = "Books", Description = "Dune Messiah" };
            var content = new StringContent(JsonConvert.SerializeObject(product));
            content.Headers.ContentType.MediaType = "application/json";

            var responseMessage = await _client.PostAsync("http://localhost/Products", content);
            var response = await responseMessage.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);
            Assert.Equal("{\"Message\":\"Received product id: 6, description: Dune Messiah\"}", response);
        }
    }
}