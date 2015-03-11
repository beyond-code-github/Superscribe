using System.Net;
using System.Net.Http;
using Microsoft.AspNet.TestHost;
using Superscribe.Engine;
using Superscribe.Owin;
using Superscribe.Tests.Owin.Components;
using Xunit;

namespace Superscribe.Tests.Owin
{
    public class PipelineTestsFixture
    {
        public TestServer OwinTestServer { get; }

        private readonly IOwinRouteEngine _engine;

        public PipelineTestsFixture()
        {
            _engine = OwinRouteEngineFactory.Create();
            OwinTestServer = TestServer.Create(
                   builder =>

                   {
                       builder.UseSuperscribeRouter(_engine);
                   });

            _engine.Pipeline("Single").Use(next => new FirstComponent(next).Invoke);
            _engine.Pipeline("Double")
                 .Use(next => new FirstComponent(next).Invoke)
                 .Use(next => new SecondComponent(next).Invoke);

            _engine.Pipeline("One").Use(next => new FirstComponent(next).Invoke);
            _engine.Pipeline("One/Two").Use(next => new SecondComponent(next).Invoke);

            _engine.Pipeline("Option").Use(next => new FirstComponent(next).Invoke);
            _engine.Pipeline("Option/One").Use(next => new SecondComponent(next).Invoke);
            _engine.Pipeline("Option/Two").Use(next => new ThirdComponent(next).Invoke);

            _engine.Pipeline("Pad").Use(next => new PadResponse(next, "h1").Invoke);
            _engine.Pipeline("Pad/Response").Use(next => new FirstComponent(next).Invoke);
        }
    }

    public class PipelineTests : IClassFixture<PipelineTestsFixture>
    {
        private readonly HttpClient _client;

        private readonly PipelineTestsFixture _fixture;

        public PipelineTests(PipelineTestsFixture fixture)
        {
            _fixture = fixture;
            _client = _fixture.OwinTestServer.CreateClient();
            _client.DefaultRequestHeaders.Add("accept", "text/html");
        }

        [Fact]
        public async void When_building_a_pipeline_with_a_single_element_given_one_middleware()
        {
            var responseMessage = await _client.GetAsync("http://localhost/Single");

            Assert.Equal("before#1after#1", await responseMessage.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Fact]
        public async void When_building_a_pipeline_with_a_single_element_given_two_middleware()
        {
            var responseMessage = await _client.GetAsync("http://localhost/Double");

            Assert.Equal("before#1before#2after#2after#1", await responseMessage.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }
        
        [Fact]
        public async void When_building_a_pipeline_with_a_two_elements_given_two_funcs()
        {
            var responseMessage = await _client.GetAsync("http://localhost/One/Two");

            Assert.Equal("before#1before#2after#2after#1", await responseMessage.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Fact]
        public async void When_building_a_pipeline_with_options_invoking_the_first()
        {
            var responseMessage = await _client.GetAsync("http://localhost/Option/One");

            Assert.Equal("before#1before#2after#2after#1", await responseMessage.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Fact]
        public async void When_building_a_pipeline_with_options_invoking_the_second()
        {
            var responseMessage = await _client.GetAsync("http://localhost/Option/Two");

            Assert.Equal("before#1before#3after#3after#1", await responseMessage.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Fact]
        public async void When_building_a_pipeline_with_middleware_that_takes_parameters()
        {
            var responseMessage = await _client.GetAsync("http://localhost/Pad/Response");

            Assert.Equal("<h1>before#1after#1</h1>", await responseMessage.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }
    }
}
