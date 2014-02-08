namespace Superscribe.Tests.Unit
{
    using System;

    using Machine.Fakes;
    using Machine.Specifications;

    using Superscribe.Models;
    using Superscribe.Utils;

    public abstract class StringRouteParserTests : WithSubject<StringRouteParser>
    {
        private Establish context = () => { };
    }

    public class When_parsing_a_basic_single_segment : StringRouteParserTests
    {
        private static GraphNode result;

        private Because of = () => result = Subject.MapToGraph("Hello");

        private It should_create_a_constant_node = () => result.ShouldBeOfType<ConstantNode>();

        private It should_set_the_pattern = () => result.Template.ShouldEqual("Hello");

        private It should_be_a_single_node = () => result.Edges.ShouldBeEmpty();
    }

    public class When_parsing_two_basic_segments : StringRouteParserTests
    {
        private static GraphNode result;

        private Because of = () => result = Subject.MapToGraph("Hello/World");

        private It should_create_a_constant_node = () => result.Parent.ShouldBeOfType<ConstantNode>();

        private It should_set_the_pattern = () => result.Parent.Template.ShouldEqual("Hello");
        
        private It should_have_a_child_constant_node = () => result.ShouldBeOfType<ConstantNode>();

        private It should_set_the_child_node_pattern = () => result.Template.ShouldEqual("World");

        private It should_have_no_more_child_nodes = () => result.Edges.ShouldBeEmpty();
    }

    public class When_parsing_root : StringRouteParserTests
    {
        private static GraphNode result;
        
        private Because of = () => result = Subject.MapToGraph("/");

        private It should_return_null = () => result.ShouldBeNull();
    }

    public class When_parsing_empty_string : StringRouteParserTests
    {
        private static GraphNode result;

        private static Exception ex;

        private Because of = () => ex = Catch.Exception(() => result = Subject.MapToGraph(string.Empty));

        private It should_throw_an_invalid_argument_exception = () => ex.ShouldBeOfType<ArgumentException>();
    }

    public class When_parsing_null_string : StringRouteParserTests
    {
        private static GraphNode result;

        private static Exception ex;

        private Because of = () => ex = Catch.Exception(() => result = Subject.MapToGraph(null));

        private It should_throw_an_invalid_argument_exception = () => ex.ShouldBeOfType<ArgumentException>();
    }
}
