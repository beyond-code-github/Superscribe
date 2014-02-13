namespace Superscribe.Testing.WebApi
{
    using System;
    using System.Web.Http;

    using Superscribe.Engine;
    using Superscribe.Models;
    using Superscribe.WebApi;

    public static class BenchmarkConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var engine = SuperscribeConfig.Register(config);

            var site = engine.Route(o => o / "sites" / (Int)"siteId");

            var blog = site / "blog";
            var blogposts = blog / "posts".Controller("blogposts");

            for (var i = 0; i < 250; i++)
            {
                GenerateRoutes(engine, site, Guid.NewGuid().ToString());
            }

            GenerateRoutes(engine, site, "portfolio");

            // sites/{siteId}/blog/posts/
            // sites/{siteId}/blog/posts/{postId}
            // sites/{siteId}/blog/posts/{postId}/media
            // sites/{siteId}/blog/posts/{postId}/media/{id}
            engine.Route(o => blogposts / -(Int)"postId" / -"media".Controller("blogpostmedia") / -(Int)"id");

            // sites/{siteId}/blog/tags
            engine.Route(o => blog / "tags".Controller("blogtags"));

            // sites/{siteId}/blog/posts/archives
            // sites/{siteId}/blog/posts/archives/{year}/{month}
            engine.Route(o => blogposts / "archives".Controller("blogpostarchives") / -(Int)"year" / (Int)"month");

            //for (var i = 0; i < 50; i++)
            //{
            //    GenerateRoutes(site, Guid.NewGuid().ToString());
            //}
        }

        private static void GenerateRoutes(IRouteEngine engine, GraphNode site, string name)
        {
            var baseroute = site / name;

            var projectsroute = baseroute / "projects".Controller(name + "projects") / -(Int)"projectId";

            // sites/{siteId}/portfolio/projects
            // sites/{siteId}/portfolio/projects/{projectId}
            // sites/{siteId}/portfolio/projects/{projectId}/media
            // sites/{siteId}/portfolio/projects/{projectId}/media/{id}
            engine.Route(o => projectsroute / -"media".Controller(name + "projectmedia") / -(Int)"id");

            // sites/{siteId}/portfolio/tags
            engine.Route(o => baseroute / "tags".Controller(name + "tags"));

            // sites/{siteId}/portfolio/categories
            // sites/{siteId}/portfolio/categories/{id}
            engine.Route(o => baseroute / "categories".Controller(name + "categories") / -(Int)"id");
        }
    }
}