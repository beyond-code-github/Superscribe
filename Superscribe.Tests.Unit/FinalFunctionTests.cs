namespace Superscribe.Tests.Unit
{
    using Machine.Specifications;

    using Superscribe.Engine;
    using Superscribe.Models;

    public abstract class FinalFunctionTests
    {
        protected static RouteData routeData;

        protected static IRouteEngine subject;

        protected static IRouteWalker walker;

        private Establish context = () =>
            {
                subject = RouteEngineFactory.Create();
                walker = subject.Walker();

                routeData = new RouteData();
            };

        protected static object HelloWorld(dynamic o)
        {
            return "Hello world";
        }
    }

    public class When_specifying_a_final_function_against_a_root_node : FinalFunctionTests
    {
        private Establish context = () => subject.Route(ʅ => HelloWorld);
        
        private Because of = () => walker.WalkRoute("/", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Hello world");
    }

    public class When_specifying_a_final_function_against_a_leaf_node : FinalFunctionTests
    {
        private Establish context = () => subject.Route(ʅ => ʅ / "Hello" / "World" * HelloWorld);

        private Because of = () => walker.WalkRoute("/Hello/World", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Hello world");
    }

    public class When_specifying_a_final_function_against_a_node : FinalFunctionTests
    {
        private Establish context = () => subject.Route(ʅ => ʅ / "Hello" * HelloWorld / "World");

        private Because of = () => walker.WalkRoute("/Hello/World", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Hello world");
    }

    public class When_specifying_an_exclusive_final_function_against_a_leaf_node : FinalFunctionTests
    {
        private Establish context = () => subject.Route(ʅ => ʅ / "Hello" / "World" * Final.Exclusive * HelloWorld);

        private Because of = () => walker.WalkRoute("/Hello/World", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Hello world");
    }

    public class When_specifying_an_exclusive_final_function_against_a_node_mid_graph : FinalFunctionTests
    {
        private Establish context = () => subject.Route(ʅ => ʅ / "Hello" * Final.Exclusive * HelloWorld / "World");

        private Because of = () => walker.WalkRoute("/Hello/World", "GET", routeData);

        private It should_not_execute_the_final_function = () => routeData.Response.ShouldNotEqual("Hello world");
    }
}
