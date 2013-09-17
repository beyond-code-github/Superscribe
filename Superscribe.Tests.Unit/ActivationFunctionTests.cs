namespace Superscribe.Tests.Unit
{
    using Machine.Specifications;

    using Superscribe.Models;
    using Superscribe.Utils;

    public abstract class ActivationFunctionTests
    {
        protected static RouteData routeData;

        protected static RouteWalker subject;

        private Establish context = () =>
        {
            routeData = new RouteData();
            ʃ.Reset();

            subject = new RouteWalker(ʃ.Base);
        };

        protected static void HelloWorld(RouteData o)
        {
            o.Response = "Hello world";
        }
    }
    
    public class When_specifying_an_activation_function_that_resolves_to_true : ActivationFunctionTests
    {
        private Establish context = () => ʃ.Route((root, ʅ) => root / "Hello" / ((o, s) => true) * "World" * HelloWorld);

        private Because of = () => subject.WalkRoute("/Hello/World", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Hello world");
    }

    public class When_specifying_an_activation_function_that_resolves_to_false : ActivationFunctionTests
    {
        private Establish context = () => ʃ.Route((root, ʅ) => root / ((o, s) => false) * "Hello" / "World" * HelloWorld);

        private Because of = () => subject.WalkRoute("/Hello/World", "GET", routeData);

        private It should_set_the_incomplete_match_flag = () => subject.IncompleteMatch.ShouldBeTrue();
    }
}
