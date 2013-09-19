namespace Superscribe.Tests.Unit
{
    using Machine.Specifications;

    using Superscribe.Models;
    using Superscribe.Utils;

    public abstract class FinalFunctionTests
    {
        protected static RouteData routeData;

        protected static RouteWalker<RouteData> subject;

        private Establish context = () =>
        {
            routeData = new RouteData();
            ʃ.Reset();

            subject = new RouteWalker<RouteData>(ʃ.Base);
        };

        protected static object HelloWorld(dynamic o)
        {
            return "Hello world";
        }
    }

    public class When_specifying_a_final_function_against_a_root_node : FinalFunctionTests
    {
        private Establish context = () => ʃ.Route(root => root * HelloWorld);
        
        private Because of = () => subject.WalkRoute("/", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Hello world");
    }

    public class When_specifying_a_final_function_against_a_leaf_node : FinalFunctionTests
    {
        private Establish context = () => ʃ.Route(root => root / "Hello" / "World" * HelloWorld);

        private Because of = () => subject.WalkRoute("/Hello/World", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Hello world");
    }

    public class When_specifying_a_final_function_against_a_node : FinalFunctionTests
    {
        private Establish context = () => ʃ.Route(root => root / "Hello" * HelloWorld / "World");

        private Because of = () => subject.WalkRoute("/Hello/World", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Hello world");
    }
}
