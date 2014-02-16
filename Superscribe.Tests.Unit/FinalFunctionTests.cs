namespace Superscribe.Tests.Unit
{
    using Machine.Specifications;

    using Superscribe.Engine;
    using Superscribe.Models;

    public abstract class FinalFunctionTests
    {
        protected static IRouteEngine subject;

        protected static IRouteWalker walker;

        private Establish context = () =>
            {
                subject = RouteEngineFactory.Create();
                walker = subject.Walker();
            };

        protected static object HelloWorld(dynamic o)
        {
            return "Hello world";
        }
    }

    public class When_specifying_a_final_function_against_a_root_node : FinalFunctionTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(ʅ => HelloWorld);
        
        private Because of = () => result = walker.WalkRoute("/", "GET", new RouteData());

        private It should_execute_the_final_function = () => result.Response.ShouldEqual("Hello world");
    }

    public class When_specifying_a_final_function_against_a_leaf_node : FinalFunctionTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(ʅ => ʅ / "Hello" / "World" * HelloWorld);

        private Because of = () => result = walker.WalkRoute("/Hello/World", "GET", new RouteData());

        private It should_execute_the_final_function = () => result.Response.ShouldEqual("Hello world");
    }

    public class When_specifying_a_final_function_against_a_node : FinalFunctionTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(ʅ => ʅ / "Hello" * HelloWorld / "World");

        private Because of = () => result = walker.WalkRoute("/Hello/World", "GET", new RouteData());

        private It should_execute_the_final_function = () => result.Response.ShouldEqual("Hello world");
    }

    public class When_specifying_an_exclusive_final_function_against_a_leaf_node : FinalFunctionTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(ʅ => ʅ / "Hello" / "World" * Final.Exclusive * HelloWorld);

        private Because of = () => result = walker.WalkRoute("/Hello/World", "GET", new RouteData());

        private It should_execute_the_final_function = () => result.Response.ShouldEqual("Hello world");
    }

    public class When_specifying_an_exclusive_final_function_against_a_node_mid_graph : FinalFunctionTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(ʅ => ʅ / "Hello" * Final.Exclusive * HelloWorld / "World");

        private Because of = () => result = walker.WalkRoute("/Hello/World", "GET", new RouteData());

        private It should_not_execute_the_final_function = () => result.Response.ShouldNotEqual("Hello world");
    }
}
