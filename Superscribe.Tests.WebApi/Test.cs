namespace Superscribe.Testing.WebApi
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Machine.Specifications;

    using WebAPI.Testing;

    public abstract class RouteTestsBase
    {
        protected static Browser browser;

        protected static HttpResponseMessage response;

        protected Establish context = () =>
        {
            var config = new HttpConfiguration();
            SuperscribeTestConfig.Register(config);
            
            browser = new Browser(config);
        };
    }

    public class When_routing_using_the_any_matchers : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/Any/BlogPosts/Get/1",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_be_case_insensitive = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class When_routing_any_url : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/Sites/123/Portfolio/Projects/123/Media/123",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_be_case_insensitive = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public class FormsTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/api/2/forms",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"Get_Forms_2\"");
    }

    public class FormsIdTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/api/2/forms/123",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"GetById_Forms_2_123\"");
    }

    public class FormsVisibleForTest : RouteTestsBase
    {
        private Because of = () => response = browser.Get(
            "/api/2/forms/visiblefor/abcde12345",
            (with) =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

        private It should_return_200_ok = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"VisibleFor_Forms_2_abcde12345\"");
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

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"GetById_PortfolioProjectMedia_123_123_123\"");
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

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"Get_PortfolioTags_123\"");
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

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"Get_PortfolioProjects_123\"");
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

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"GetById_PortfolioProjects_123_123\"");
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

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"Get_PortfolioCategories_123\"");
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

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"GetById_PortfolioCategories_123_123\"");
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

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"Get_BlogPosts_123\"");

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

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"GetById_BlogPosts_123_123\"");

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

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"Get_BlogPostMedia_123_123\"");

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

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"GetById_BlogPostMedia_123_123_123\"");

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

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"Get_BlogTags_123\"");

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

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"Get_BlogPostArchives_123\"");

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

        private It should_hit_the_right_controller =
            () => response.Content.ReadAsStringAsync().Result.ShouldEqual("\"Get_BlogPostArchives_123\"");

    }
}
