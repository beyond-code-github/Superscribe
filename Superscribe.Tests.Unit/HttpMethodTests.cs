namespace Superscribe.Tests.Unit
{
    using Machine.Specifications;

    using Superscribe.Models;
    using Superscribe.Testing;
    using Superscribe.Utils;

    public abstract class HttpMethodTests
    {
        protected static RouteData routeData;

        protected static RouteWalker<RouteData> subject;

        private Establish context = () =>
        {
            routeData = new RouteData();
            ʃ.Reset();

            subject = new RouteWalker<RouteData>(ʃ.Base);
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
    }

    public class When_defining_a_route_with_no_modifiers_and_issuing_a_get : HttpMethodTests
    {
        private Establish context = () => ʃ.Route(root => root / "Hello" / (ʃString)"Name" * Hello);

        private Because of = () => subject.WalkRoute("/Hello/Pete", "GET", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Hello Pete");
    }

    public class When_defining_a_route_with_no_modifiers_and_issuing_a_post : HttpMethodTests
    {
        private Establish context = () => ʃ.Route(root => root / "Hello" / (ʃString)"Name" * Hello);

        private Because of = () => subject.WalkRoute("/Hello/Pete", "POST", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Hello Pete");
    }

    public class When_defining_a_route_with_options_and_issuing_a_get_to_the_first : HttpMethodTests
    {
        private Establish context = () => ʃ.Route(ʅ => ʅ / (
              ʅ / "Hello" / (ʃString)"Name" * Hello
            | ʅ / "Confirm" / (ʃLong)"Age" * CheckAge));

        private Because of = () => subject.WalkRoute("/Hello/Pete", "GET", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Hello Pete");
    }

    public class When_defining_a_route_with_options_and_issuing_a_get_to_the_second : HttpMethodTests
    {
        private Establish context = () => ʃ.Route(ʅ => ʅ / (
              ʅ / "Hello" / (ʃString)"Name" * Hello
            | ʅ / "Confirm" / (ʃLong)"Age" * CheckAge));

        private Because of = () => subject.WalkRoute("/Confirm/18", "GET", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Access granted");
    }

    public class When_defining_a_route_with_options_and_issuing_a_post_to_the_first : HttpMethodTests
    {
        private Establish context = () => ʃ.Route((ʅ) => ʅ / (
              ʅ / "Hello" / (ʃString)"Name" * Hello
            | ʅ / "Confirm" / (ʃLong)"Age" * CheckAge));

        private Because of = () => subject.WalkRoute("/Hello/Pete", "POST", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Hello Pete");
    }

    public class When_defining_a_route_with_options_and_issuing_a_post_to_the_second : HttpMethodTests
    {
        private Establish context = () => ʃ.Route((ʅ) => ʅ / (
              ʅ / "Hello" / (ʃString)"Name" * Hello
            | ʅ / "Confirm" / (ʃLong)"Age" * CheckAge));

        private Because of = () => subject.WalkRoute("/Confirm/18", "POST", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Access granted");
    }

    public class When_defining_a_route_with_options_and_explicit_methods_and_issuing_a_get_to_the_first : HttpMethodTests
    {
        private Establish context = () => ʃ.Route((ʅ) => ʅ / (
              ʅ["GET"] / "Hello" / (ʃString)"Name" * Hello
            | ʅ["POST"] / "Confirm" / (ʃLong)"Age" * CheckAge));

        private Because of = () => subject.WalkRoute("/Hello/Pete", "GET", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Hello Pete");
    }

    public class When_defining_a_route_with_options_and_explicit_methods_and_issuing_a_post_to_the_first : HttpMethodTests
    {
        private Establish context = () => ʃ.Route((ʅ) => ʅ / (
              ʅ["GET"] / "Hello" / (ʃString)"Name" * Hello
            | ʅ["POST"] / "Confirm" / (ʃLong)"Age" * CheckAge));

        private Because of = () => subject.WalkRoute("/Hello/Pete", "POST", routeData);

        private It should_set_the_incomplete_match_flag = () => subject.IncompleteMatch.ShouldBeTrue();
    }

    public class When_defining_a_route_with_options_and_explicit_methods_and_issuing_a_post_to_the_second : HttpMethodTests
    {
        private Establish context = () => ʃ.Route((ʅ) => ʅ / (
              ʅ["GET"] / "Hello" / (ʃString)"Name" * Hello
            | ʅ["POST"] / "Confirm" / (ʃLong)"Age" * CheckAge));

        private Because of = () => subject.WalkRoute("/Confirm/18", "POST", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Access granted");
    }

    public class When_defining_a_route_with_options_and_explicit_methods_and_issuing_a_get_to_the_second : HttpMethodTests
    {
        private Establish context = () => ʃ.Route((ʅ) => ʅ / (
              ʅ["GET"] / "Hello" / (ʃString)"Name" * Hello
            | ʅ["POST"] / "Confirm" / (ʃLong)"Age" * CheckAge));

        private Because of = () => subject.WalkRoute("/Confirm/18", "GET", routeData);

        private It should_set_the_incomplete_match_flag = () => subject.IncompleteMatch.ShouldBeTrue();
    }

    public class When_defining_a_route_under_a_subnode_with_options_and_explicit_methods_and_issuing_a_get_to_the_first : HttpMethodTests
    {
        private Establish context = () => ʃ.Route(ʅ => ʅ /"Api" / (
              ʅ["GET"] / "Hello" / (ʃString)"Name" * Hello
            | ʅ["POST"] / "Confirm" / (ʃLong)"Age" * CheckAge));

        private Because of = () => subject.WalkRoute("/Api/Hello/Pete", "GET", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Hello Pete");
    }

    public class When_defining_a_route_under_a_subnode_with_options_and_explicit_methods_and_issuing_a_post_to_the_second : HttpMethodTests
    {
        private Establish context = () => ʃ.Route((ʅ) => ʅ /"Api" / (
              ʅ["GET"] / "Hello" / (ʃString)"Name" * Hello
            | ʅ["POST"] / "Confirm" / (ʃLong)"Age" * CheckAge));

        private Because of = () => 
            subject.WalkRoute("/Api/Confirm/18", "POST", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Access granted");
    }

    public class When_defining_a_route_under_a_subnode_where_both_explcit_method_definitons_contain_the_same_matcher_and_issuing_a_get : HttpMethodTests
    {
        private Establish context = () => ʃ.Route((ʅ) => ʅ /"Api" / (
              ʅ["GET"] / "Product" * GetProduct
            | ʅ["POST"] / "Product" * UpdateProduct));

        private Because of = () =>
            subject.WalkRoute("/Api/Product", "GET", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Product Details");
    }

    public class When_defining_a_route_under_a_subnode_where_both_explcit_method_definitons_contain_the_same_matcher_and_issuing_a_post : HttpMethodTests
    {
        private Establish context = () => ʃ.Route((ʅ) => ʅ /"Api" / (
              ʅ["GET"] / "Product" * GetProduct
            | ʅ["POST"] / "Product" * UpdateProduct));

        private Because of = () =>
            subject.WalkRoute("/Api/Product", "POST", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Update Product");
    }

    public class When_defining_a_route_with_explicit_method_final_function_options_and_issuing_a_get : HttpMethodTests
    {
        private Establish context = () => ʃ.Route((ʅ) => ʅ /"Products" / (ʃLong)"Id" * (
              ʅ["GET"] * GetProduct
            | ʅ["POST"] * UpdateProduct));

        private Because of = () => subject.WalkRoute("/Products/1", "GET", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Product Details");
    }

    public class When_defining_a_route_with_explicit_method_final_function_options_and_issuing_a_post : HttpMethodTests
    {
        private Establish context = () => ʃ.Route((ʅ) => ʅ /"Products" / (ʃLong)"Id" * (
              ʅ["GET"] * GetProduct
            | ʅ["POST"] * UpdateProduct));

        private Because of = () => subject.WalkRoute("/Products/1", "POST", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Update Product");
    }

    public class When_defining_combinational_routes_with_dedicated_method_calls_and_issuing_a_get : HttpMethodTests
    {
        private Establish context = () =>
            {
                ʃ.Get((ʅ) => "Products" / (ʃLong)"Id" * GetProduct);
                ʃ.Post((ʅ) => "Products" / (ʃLong)"Id" * UpdateProduct);
        };

        private Because of = () => subject.WalkRoute("/Products/1", "GET", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Product Details");
    }

    public class When_defining_combinational_routes_with_dedicated_method_calls_and_issuing_a_post : HttpMethodTests
    {
        private Establish context = () =>
        {
            ʃ.Get(ʅ => "Products" / (ʃLong)"Id" * GetProduct);
            ʃ.Post(ʅ => "Products" / (ʃLong)"Id" * UpdateProduct);
        };

        private Because of = () => subject.WalkRoute("/Products/1", "POST", routeData);

        private It should_respond_correctly = () => routeData.Response.ShouldEqual("Update Product");
    }
}
