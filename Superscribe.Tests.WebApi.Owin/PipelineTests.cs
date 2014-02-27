namespace Superscribe.Tests.WebApi.Owin
{
    using System.Net.Http;
    using System.Reflection;
    using System.Web.Http;

    using Autofac;
    using Autofac.Integration.WebApi;

    using DotNetDoodle.Owin;
    using DotNetDoodle.Owin.Dependencies.Autofac;

    using global::Owin;

    using Machine.Specifications;

    using Microsoft.Owin.Testing;

    using Superscribe.Owin;
    using Superscribe.Owin.Engine;
    using Superscribe.Owin.Extensions;
    using Superscribe.WebApi;
    using Superscribe.WebApi.Owin.Extensions;

    public abstract class PipelineTests
    {
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
                        engine.Route(r => r / "Values".Controller());
                        engine.Route(r => r / "Api" / "Values".Controller());

                        var httpConfig = new HttpConfiguration();
                        SuperscribeConfig.Register(httpConfig, engine);
                        
                        engine.Pipeline("Values").Use<ApiDependencies>();

                        builder.UseAutofacContainer(RegisterServices())
                            .UseSuperscribeRouter(engine)
                            .UseWebApiWithContainer(httpConfig)
                            .WithSuperscribe(httpConfig, engine);
                    });

                client = owinTestServer.HttpClient;
                client.DefaultRequestHeaders.Add("accept", "text/html");
            };

        public static IContainer RegisterServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterOwinApplicationContainer();

            builder.RegisterType<Repository>()
                   .As<IRepository>()
                   .InstancePerLifetimeScope();

            return builder.Build();
        }
    }

    public class When_going_through_the_normal_route : PipelineTests
    {
        private Because of = () =>
                responseMessage = client.GetAsync("http://localhost/Api/Values/").Await();

        private It should_set_the_response =
           () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("\"value1\"");
    }

    public class When_going_through_the_pipeline_route : PipelineTests
    {
        private Because of = () =>
                responseMessage = client.GetAsync("http://localhost/Values/").Await();

        private It should_set_the_response =
           () => responseMessage.Content.ReadAsStringAsync().Result.ShouldEqual("\"value3\"");
    }
}
