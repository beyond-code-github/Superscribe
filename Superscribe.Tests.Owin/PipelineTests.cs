namespace Superscribe.Tests.Owin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Machine.Specifications;

    using Microsoft.Owin.Testing;

    using Superscribe.Owin;
    using Superscribe.Owin.Extensions;
    using Superscribe.Owin.Pipelining;

    public class FirstComponent
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        public FirstComponent(Func<IDictionary<string, object>, Task> next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            await environment.WriteResponse("before#1");
            await this.next(environment);
            await environment.WriteResponse("after#1");
        }
    }

    public class SecondComponent
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        public SecondComponent(Func<IDictionary<string, object>, Task> next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            await environment.WriteResponse("before#2");
            await this.next(environment);
            await environment.WriteResponse("after#2");
        }
    }

    public class ThirdComponent
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        public ThirdComponent(Func<IDictionary<string, object>, Task> next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            await environment.WriteResponse("before#3");
            await this.next(environment);
            await environment.WriteResponse("after#3");
        }
    }

    public abstract class PipelineTests
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
                    
                    builder.Use(typeof(OwinRouter), builder, config);
                });

            client = owinTestServer.HttpClient;
            client.DefaultRequestHeaders.Add("accept", "text/html");

            ʃ.Reset();
        };
    }

    public class When_building_a_pipeline_with_a_single_element_given_one_middleware : PipelineTests
    {
        private Establish context = () => ʃ.Route(ʅ => ʅ / "Hello" * Pipeline.Action<FirstComponent>());

        private Because of = () => responseMessage = client.GetAsync("http://localhost/Hello").Result;

        private It should_execute_the_final_function = () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("before#1after#1");

        private It should_return_200 = () => responseMessage.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }
    
    public class When_building_a_pipeline_with_a_single_element_given_two_middleware : PipelineTests
    {
        private Establish context = () => ʃ.Route(ʅ => ʅ / "Hello" * Pipeline.Action<FirstComponent>()
                                                                    * Pipeline.Action<SecondComponent>());

        private Because of = () => responseMessage = client.GetAsync("http://localhost/Hello").Result;

        private It should_execute_the_final_function = () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("before#1before#2after#2after#1");

        private It should_return_200 = () => responseMessage.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class When_building_a_pipeline_with_a_two_elements_given_two_funcs : PipelineTests
    {
        private Establish context = () => ʃ.Route(ʅ => ʅ / "Hello" * Pipeline.Action<FirstComponent>()
                                                                / "World" * Pipeline.Action<SecondComponent>());

        private Because of = () => responseMessage = client.GetAsync("http://localhost/Hello/World").Result;

        private It should_execute_the_final_function = () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("before#1before#2after#2after#1");

        private It should_return_200 = () => responseMessage.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public abstract class When_building_a_pipeline_with_options : PipelineTests
    {
        private Establish context = () => ʃ.Route(ʅ => ʅ / "Hello" * Pipeline.Action<FirstComponent>() / (
                                                              ʅ / "World" * Pipeline.Action<SecondComponent>()
                                                            | ʅ / "Foobar" * Pipeline.Action<ThirdComponent>()));
    }

    public class When_building_a_pipeline_with_options_invoking_the_first : When_building_a_pipeline_with_options
    {
        private Because of = () => responseMessage = client.GetAsync("http://localhost/Hello/World").Result;

        private It should_execute_the_final_function = () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("before#1before#2after#2after#1");

        private It should_return_200 = () => responseMessage.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class When_building_a_pipeline_with_options_invoking_the_second : When_building_a_pipeline_with_options
    {
        private Because of = () => responseMessage = client.GetAsync("http://localhost/Hello/Foobar").Result;

        private It should_execute_the_final_function = () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("before#1before#3after#3after#1");

        private It should_return_200 = () => responseMessage.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }
}
