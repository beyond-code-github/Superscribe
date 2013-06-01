namespace Superscribe.Testing.Console
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Machine.Specifications;

    using global::WebAPI.Testing;

    public abstract class RouteTestsBase
    {
        protected static Browser browser;

        protected static HttpResponseMessage response;

        protected Establish context = () =>
        {
            var config = new HttpConfiguration();
            SuperscribeConfig.Register(config);

            browser = new Browser(config);
        };
    }

    public class PortfolioProjectMediaTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/sites/123/portfolio/projects/123/media/123",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class PortfolioTagsTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/sites/123/portfolio/tags",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class PortfolioProjectsTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/sites/123/portfolio/projects",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class PortfolioProjectsIdTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/sites/123/portfolio/projects/123",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class PortfolioCategoriesTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/sites/123/portfolio/categories",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class PortfolioCategoriesIdTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/sites/123/portfolio/categories/123",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class BlogPostsTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/sites/123/blog/posts",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class BlogPostsIdTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/sites/123/blog/posts/123",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class BlogPostMediaTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/sites/123/blog/posts/123/media",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class BlogPostMediaIdTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/sites/123/blog/posts/123/media/123",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class BlogTagsTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/sites/123/blog/tags",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class BlogPostArchivesTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/sites/123/blog/posts/archives",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class BlogPostArchiveDateTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/sites/123/blog/posts/archives/2013/12",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }
}
