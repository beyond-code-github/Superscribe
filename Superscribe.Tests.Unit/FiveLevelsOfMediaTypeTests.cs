namespace Superscribe.Tests.Unit
{
    using System.Collections;
    using System.Collections.Generic;

    using Machine.Specifications;

    using Superscribe.Engine;

    public abstract class FiveLevelsOfMediaTypeTests
    {
        protected static IRouteEngine subject;

        protected static IRouteWalker walker;

        private Establish context = () =>
        {
            subject = RouteEngineFactory.Create();
            walker = subject.Walker();
        };
    }

    public class When_specifying_a_5LMT_domain_filter_that_matches : FiveLevelsOfMediaTypeTests
    {
        private static RouteData result;

        private static IDictionary<string, object> environment;

        private Establish context = () =>
            {
                subject.Route("Hello/Pete", o => "Hello Pete", When.DomainModel("Test"));
                environment = new Dictionary<string, object>
                                    {
                                          { Constants.RequestPathEnvironmentKey, "/Hello/Pete" },
                                          { Constants.RequestQuerystringEnvironmentKey, string.Empty },
                                          { Constants.RequestMethodEnvironmentKey, "GET" },
                                          { Constants.AcceptsEnvironmentKey, "application/json;domain-model=Test" },
                                    };
            };

        private Because of = () => result = walker.WalkRoute(environment, new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Hello Pete");
    }
}
