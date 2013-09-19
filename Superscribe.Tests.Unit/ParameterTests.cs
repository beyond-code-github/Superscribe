namespace Superscribe.Tests.Unit
{
    using Machine.Specifications;

    using Superscribe.Models;
    using Superscribe.Utils;

    public abstract class ParameterTests
    {
        protected static RouteData routeData;

        protected static RouteWalker subject;

        private Establish context = () =>
        {
            routeData = new RouteData();
            ʃ.Reset();

            subject = new RouteWalker(ʃ.Base);
        };

        protected static void Hello(RouteData o)
        {
            o.Response = string.Format("Hello {0}", o.Parameters.Name);
        }

        protected static void HelloUnknown(RouteData o)
        {
            o.Response = "Hello Unknown Person";
        }

        protected static void CommentOnMood(RouteData o)
        {
            o.Response = o.Parameters.Happy ? "Me too!" : "Why so sad?";
        }

        protected static void CheckAge(RouteData o)
        {
            o.Response = o.Parameters.Age >= 18 ? "Access granted" : "Too young";
        }
    }

    public class When_capturing_a_string_parameter : ParameterTests
    {
        private Establish context = () => ʃ.Route(root => root / "Hello" / (ʃString)"Name" * Hello);

        private Because of = () => subject.WalkRoute("/Hello/Pete", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Hello Pete");
    }

    public class When_there_is_an_absent_optional_string_parameter_at_the_end_of_a_route : ParameterTests
    {
        private Establish context = () => ʃ.Route(root => root / "Hello" * HelloUnknown / -(ʃString)"Name" * Hello);

        private Because of = () => subject.WalkRoute("/Hello", "GET", routeData);

        private It should_execute_the_final_function_not_the_optional_part = () => routeData.Response.ShouldEqual("Hello Unknown Person");
    }

    public class When_there_is_an_absent_optional_string_parameter_at_the_end_of_a_route_invoked_with_a_trailing_slash : ParameterTests
    {
        private Establish context = () => ʃ.Route(root => root / "Hello" * HelloUnknown / -(ʃString)"Name" * Hello);

        private Because of = () => subject.WalkRoute("/Hello/", "GET", routeData);

        private It should_execute_the_final_function_not_the_optional_part = () => routeData.Response.ShouldEqual("Hello Unknown Person");
    }

    public class When_capturing_a_boolean_parameter_true : ParameterTests
    {
        private Establish context = () => ʃ.Route(root => root / "Happy" / (ʃBool)"Happy" * CommentOnMood);

        private Because of = () => subject.WalkRoute("/Happy/true", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Me too!");
    }

    public class When_capturing_a_boolean_parameter_false : ParameterTests
    {
        private Establish context = () => ʃ.Route(root => root / "Happy" / (ʃBool)"Happy" * CommentOnMood);

        private Because of = () => subject.WalkRoute("/Happy/false", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Why so sad?");
    }

    public class When_capturing_an_int_parameter : ParameterTests
    {
        private Establish context = () => ʃ.Route(root => root / "Confirm" / (ʃInt)"Age" * CheckAge);

        private Because of = () => subject.WalkRoute("/Confirm/18", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Access granted");
    }

    public class When_capturing_an_int_parameter_that_is_too_long : ParameterTests
    {
        private Establish context = () => ʃ.Route(root => root / "Confirm" / (ʃInt)"Age" * CheckAge);

        private Because of = () => subject.WalkRoute("/Confirm/18000000000", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.ParamConversionError.ShouldEqual(true);
    }

    public class When_capturing_a_long_parameter : ParameterTests
    {
        private Establish context = () => ʃ.Route(root => root / "Confirm" / (ʃLong)"Age" * CheckAge);

        private Because of = () => subject.WalkRoute("/Confirm/18000000000", "GET", routeData);

        private It should_execute_the_final_function = () => routeData.Response.ShouldEqual("Access granted");
    }
}
