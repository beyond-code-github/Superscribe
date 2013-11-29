namespace Superscribe.Testing.WebApi
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Machine.Specifications;

    using WebAPI.Testing;

    public abstract class RouteCombiningTestsBase
    {
        protected static Browser browser;

        protected static HttpResponseMessage response;

        protected Establish context = () =>
        {
            var config = new HttpConfiguration();
            CombinationConfig.Register(config);

            browser = new Browser(config);
        };
    }

    public class Route_Combination_When_routing_any_url : RouteCombiningTestsBase
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
    
    public class Route_Combination_PortfolioProjectMediaTest : RouteCombiningTestsBase
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

    public class Route_Combination_PortfolioTagsTest : RouteCombiningTestsBase
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

    public class Route_Combination_PortfolioProjectsTest : RouteCombiningTestsBase
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

    public class Route_Combination_PortfolioProjectsIdTest : RouteCombiningTestsBase
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

    public class Route_Combination_PortfolioCategoriesTest : RouteCombiningTestsBase
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

    public class Route_Combination_PortfolioCategoriesIdTest : RouteCombiningTestsBase
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

    public class Route_Combination_BlogPostsTest : RouteCombiningTestsBase
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

    public class Route_Combination_BlogPostsIdTest : RouteCombiningTestsBase
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

    public class Route_Combination_BlogPostMediaTest : RouteCombiningTestsBase
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

    public class Route_Combination_BlogPostMediaIdTest : RouteCombiningTestsBase
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

    public class Route_Combination_BlogTagsTest : RouteCombiningTestsBase
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

    public class Route_Combination_BlogPostArchivesTest : RouteCombiningTestsBase
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

    public class Route_Combination_BlogPostArchiveDateTest : RouteCombiningTestsBase
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
