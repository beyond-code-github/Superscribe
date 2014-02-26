namespace Superscribe.Tests.Unit
{
    using Machine.Specifications;

    using Superscribe.Engine;
    using Superscribe.Models;

    public abstract class ParameterTests
    {
        protected static IRouteEngine subject;

        protected static IRouteWalker walker;

        private Establish context = () =>
        {
            subject = RouteEngineFactory.Create();
            walker = subject.Walker();
        };

        protected static object Hello(dynamic o)
        {
            return string.Format("Hello {0}", o.Parameters.Name);
        }

        protected static object HelloUnknown(dynamic o)
        {
            return "Hello Unknown Person";
        }

        protected static object CommentOnMood(dynamic o)
        {
            return o.Parameters.Happy ? "Me too!" : "Why so sad?";
        }

        protected static object CheckAge(dynamic o)
        {
            return o.Parameters.Age >= 18 ? "Access granted" : "Too young";
        }
    }

    public class When_capturing_a_string_parameter : ParameterTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(r => r / "Hello" / (String)"Name", Hello);

        private Because of = () => result = walker.WalkRoute("/Hello/Pete", "GET", new RouteData());

        private It should_execute_the_final_function = () => result.Response.ShouldEqual("Hello Pete");
    }

    public class When_there_is_an_absent_optional_string_parameter_at_the_end_of_a_route : ParameterTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(r => r / "Hello" * HelloUnknown / (String)"Name", Hello);

        private Because of = () => result = walker.WalkRoute("/Hello", "GET", new RouteData());

        private It should_execute_the_final_function_not_the_optional_part = () => result.Response.ShouldEqual("Hello Unknown Person");
    }

    public class When_there_is_an_absent_optional_string_parameter_at_the_end_of_a_route_invoked_with_a_trailing_slash : ParameterTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(r => r / "Hello" * HelloUnknown / (String)"Name", Hello);

        private Because of = () => result = walker.WalkRoute("/Hello/", "GET", new RouteData());

        private It should_execute_the_final_function_not_the_optional_part = () => result.Response.ShouldEqual("Hello Unknown Person");
    }

    public class When_capturing_a_boolean_parameter_true : ParameterTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(r => r / "Happy" / (Bool)"Happy", CommentOnMood);

        private Because of = () => result = walker.WalkRoute("/Happy/true", "GET", new RouteData());

        private It should_execute_the_final_function = () => result.Response.ShouldEqual("Me too!");
    }

    public class When_capturing_a_boolean_parameter_false : ParameterTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(r => r / "Happy" / (Bool)"Happy", CommentOnMood);

        private Because of = () => result = walker.WalkRoute("/Happy/false", "GET", new RouteData());

        private It should_execute_the_final_function = () => result.Response.ShouldEqual("Why so sad?");
    }

    public class When_capturing_an_int_parameter : ParameterTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(r => r / "Confirm" / (Int)"Age", CheckAge);

        private Because of = () => result = walker.WalkRoute("/Confirm/18", "GET", new RouteData());

        private It should_execute_the_final_function = () => result.Response.ShouldEqual("Access granted");
    }

    //public class When_capturing_an_int_parameter_that_is_too_long : ParameterTests
    //{
    //    private Establish context = () => subject.Route(r => r / "Confirm" / (Int)"Age" * CheckAge);

    //    private Because of = () => result = walker.WalkRoute("/Confirm/18000000000", "GET", routeData);

    //    private It should_throw_an_error = () => result.ParamConversionError.ShouldEqual(true);
    //}

    public class When_capturing_a_long_parameter : ParameterTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(r => r / "Confirm" / (Long)"Age", CheckAge);

        private Because of = () => result = walker.WalkRoute("/Confirm/18000000000", "GET", new RouteData());

        private It should_execute_the_final_function = () => result.Response.ShouldEqual("Access granted");
    }
}
