namespace Superscribe.Tests.Unit
{
    using Machine.Specifications;

    using Superscribe.Engine;

    public abstract class ActivationFunctionTests
    {
        protected static RouteData routeData;

        protected static IRouteEngine subject;

        protected static IRouteWalker walker;

        private Establish context = () =>
        {
            routeData = new RouteData();
            subject = RouteEngineFactory.Create();
            walker = subject.Walker();
        };

        protected static object HelloWorld(dynamic o)
        {
            return "Hello world";
        }
    }
    
    public class When_specifying_an_activation_function_that_resolves_to_true : ActivationFunctionTests
    {
        private Establish context = () => subject.Route(ʅ => ʅ / "Hello" / ((o, s) => true) * "World" * HelloWorld);

        private Because of = () => walker.WalkRoute("/Hello/World", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Hello world");
    }

    public class When_specifying_an_activation_function_that_resolves_to_false : ActivationFunctionTests
    {
        private Establish context = () => subject.Route(ʅ => ʅ / "Hello" / ((o, s) => false) * "World" * HelloWorld);

        private Because of = () => walker.WalkRoute("/Hello/World", "GET", routeData);

        private It should_set_the_incomplete_match_flag = () => walker.IncompleteMatch.ShouldBeTrue();
    }
}
