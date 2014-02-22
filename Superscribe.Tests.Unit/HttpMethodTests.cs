namespace Superscribe.Tests.Unit
{
    using Machine.Specifications;

    using Superscribe.Engine;
    using Superscribe.Models;

    public abstract class HttpMethodTests
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

        protected static object CheckAge(dynamic o)
        {
            return o.Parameters.Age >= 18 ? "Access granted" : "Too young";
        }

        protected static object GetProduct(dynamic o)
        {
            return "Product Details";
        }

        protected static object UpdateProduct(dynamic o)
        {
            return "Update Product";
        }

        public void AfterContextCleanup()
        {
        }
    }

    public class When_defining_a_route_with_no_modifiers_and_issuing_a_get : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(root => root / "Hello" / (String)"Name" * Hello);

        private Because of = () => result = walker.WalkRoute("/Hello/Pete", "GET", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Hello Pete");
    }

    public class When_defining_a_route_with_simple_string_syntax_and_issuing_a_get : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route("Hello/Pete", o => "Hello Pete");

        private Because of = () => result = walker.WalkRoute("/Hello/Pete", "GET", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Hello Pete");
    }
    
    public class When_defining_a_route_with_simple_syntax_and_issuing_a_get : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route("Hello" / (String)"Name", Hello);

        private Because of = () => result = walker.WalkRoute("/Hello/Pete", "GET", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Hello Pete");
    }

    public class When_defining_a_route_with_no_modifiers_and_issuing_a_post : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(root => root / "Hello" / (String)"Name" * Hello);

        private Because of = () => result = walker.WalkRoute("/Hello/Pete", "POST", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Hello Pete");
    }

    public class When_defining_a_route_with_options_and_issuing_a_get_to_the_first : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(ʅ => ʅ / (
              ʅ / "Hello" / (String)"Name" * Hello
            | ʅ / "Confirm" / (Long)"Age" * CheckAge));

        private Because of = () => result = walker.WalkRoute("/Hello/Pete", "GET", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Hello Pete");
    }

    public class When_defining_a_route_with_options_and_issuing_a_get_to_the_second : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(ʅ => ʅ / (
              ʅ / "Hello" / (String)"Name" * Hello
            | ʅ / "Confirm" / (Long)"Age" * CheckAge));

        private Because of = () => result = walker.WalkRoute("/Confirm/18", "GET", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Access granted");
    }

    public class When_defining_a_route_with_options_and_issuing_a_post_to_the_first : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route((ʅ) => ʅ / (
              ʅ / "Hello" / (String)"Name" * Hello
            | ʅ / "Confirm" / (Long)"Age" * CheckAge));

        private Because of = () => result = walker.WalkRoute("/Hello/Pete", "POST", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Hello Pete");
    }

    public class When_defining_a_route_with_options_and_issuing_a_post_to_the_second : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route((ʅ) => ʅ / (
              ʅ / "Hello" / (String)"Name" * Hello
            | ʅ / "Confirm" / (Long)"Age" * CheckAge));

        private Because of = () => result = walker.WalkRoute("/Confirm/18", "POST", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Access granted");
    }

    public class When_defining_a_route_with_options_and_explicit_methods_and_issuing_a_get_to_the_first : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route((ʅ) => ʅ / (
              ʅ["GET"] / "Hello" / (String)"Name" * Hello
            | ʅ["POST"] / "Confirm" / (Long)"Age" * CheckAge));

        private Because of = () => result = walker.WalkRoute("/Hello/Pete", "GET", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Hello Pete");
    }

    public class When_defining_a_route_with_options_and_explicit_methods_and_issuing_a_post_to_the_first : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () =>
            {
                subject.Get("Hello" / (String)"Name" * Hello);
                subject.Post("Confirm" / (Long)"Age" * CheckAge);
            };

        private Because of = () => result = walker.WalkRoute("/Hello/Pete", "POST", new RouteData());

        private It should_set_the_incomplete_match_flag = () => result.IncompleteMatch.ShouldBeTrue();
    }

    public class When_defining_a_route_with_options_and_explicit_methods_and_issuing_a_post_to_the_second : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route((ʅ) => ʅ / (
              ʅ["GET"] / "Hello" / (String)"Name" * Hello
            | ʅ["POST"] / "Confirm" / (Long)"Age" * CheckAge));

        private Because of = () => result = walker.WalkRoute("/Confirm/18", "POST", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Access granted");
    }

    public class When_defining_a_route_with_options_and_explicit_methods_and_issuing_a_get_to_the_second : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route((ʅ) => ʅ / (
              ʅ["GET"] / "Hello" / (String)"Name" * Hello
            | ʅ["POST"] / "Confirm" / (Long)"Age" * CheckAge));

        private Because of = () => result = walker.WalkRoute("/Confirm/18", "GET", new RouteData());

        private It should_set_the_incomplete_match_flag = () => result.IncompleteMatch.ShouldBeTrue();
    }

    public class When_defining_a_route_under_a_subnode_with_options_and_explicit_methods_and_issuing_a_get_to_the_first : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route(ʅ => ʅ / "Api" / (
              ʅ["GET"] / "Hello" / (String)"Name" * Hello
            | ʅ["POST"] / "Confirm" / (Long)"Age" * CheckAge));

        private Because of = () => result = walker.WalkRoute("/Api/Hello/Pete", "GET", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Hello Pete");
    }

    public class When_defining_a_route_under_a_subnode_with_options_and_explicit_methods_and_issuing_a_post_to_the_second : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route((ʅ) => ʅ / "Api" / (
              ʅ["GET"] / "Hello" / (String)"Name" * Hello
            | ʅ["POST"] / "Confirm" / (Long)"Age" * CheckAge));

        private Because of = () =>
            result = walker.WalkRoute("/Api/Confirm/18", "POST", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Access granted");
    }

    public class When_defining_a_route_under_a_subnode_where_both_explcit_method_definitons_contain_the_same_matcher_and_issuing_a_get : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route((ʅ) => ʅ / "Api" / (
              ʅ["GET"] / "Product" * GetProduct
            | ʅ["POST"] / "Product" * UpdateProduct));

        private Because of = () =>
            result = walker.WalkRoute("/Api/Product", "GET", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Product Details");
    }

    public class When_defining_a_route_under_a_subnode_where_both_explcit_method_definitons_contain_the_same_matcher_and_issuing_a_post : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route((ʅ) => ʅ / "Api" / (
              ʅ["GET"] / "Product" * GetProduct
            | ʅ["POST"] / "Product" * UpdateProduct));

        private Because of = () =>
            result = walker.WalkRoute("/Api/Product", "POST", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Update Product");
    }

    public class When_defining_a_route_with_explicit_method_final_function_options_and_issuing_a_get : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route((ʅ) => ʅ / "Products" / (Long)"Id" * (
              ʅ["GET"] * GetProduct
            | ʅ["POST"] * UpdateProduct));

        private Because of = () => result = walker.WalkRoute("/Products/1", "GET", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Product Details");
    }

    public class When_defining_a_route_with_explicit_method_final_function_options_and_issuing_a_post : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () => subject.Route((ʅ) => ʅ / "Products" / (Long)"Id" * (
              ʅ["GET"] * GetProduct
            | ʅ["POST"] * UpdateProduct));

        private Because of = () => result = walker.WalkRoute("/Products/1", "POST", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Update Product");
    }

    public class When_defining_combinational_routes_with_dedicated_method_calls_and_issuing_a_get : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () =>
            {
                subject.Get((ʅ) => "Products" / (Long)"Id" * GetProduct);
                subject.Post((ʅ) => "Products" / (Long)"Id" * UpdateProduct);
        };

        private Because of = () => result = walker.WalkRoute("/Products/1", "GET", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Product Details");
    }

    public class When_defining_combinational_routes_with_dedicated_method_calls_and_issuing_a_post : HttpMethodTests
    {
        protected static RouteData result;

        private Establish context = () =>
        {
            subject.Get(ʅ => "Products" / (Long)"Id" * GetProduct);
            subject.Post(ʅ => "Products" / (Long)"Id" * UpdateProduct);
        };

        private Because of = () => result = walker.WalkRoute("/Products/1", "POST", new RouteData());

        private It should_respond_correctly = () => result.Response.ShouldEqual("Update Product");
    }
}
