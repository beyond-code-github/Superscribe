namespace Superscribe.Testing
{
    using System;
    using System.Web.Http;

    using Superscribe.Models;
    using Superscribe.WebApi;

    public static class BenchmarkConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);

            var site = ʃ.Route(o => o / "sites" / (ʃInt)"siteId");

            var blog = site / "blog";
            var blogposts = blog / "posts".Controller("blogposts");

            for (var i = 0; i < 250; i++)
            {
                GenerateRoutes(site, Guid.NewGuid().ToString());
            }

            GenerateRoutes(site, "portfolio");

            // sites/{siteId}/blog/posts/
            // sites/{siteId}/blog/posts/{postId}
            // sites/{siteId}/blog/posts/{postId}/media
            // sites/{siteId}/blog/posts/{postId}/media/{id}
            ʃ.Route(o => blogposts / -(ʃInt)"postId" / -"media".Controller("blogpostmedia") / -(ʃInt)"id");

            // sites/{siteId}/blog/tags
            ʃ.Route(o => blog / "tags".Controller("blogtags"));

            // sites/{siteId}/blog/posts/archives
            // sites/{siteId}/blog/posts/archives/{year}/{month}
            ʃ.Route(o => blogposts / "archives".Controller("blogpostarchives") / -(ʃInt)"year" / (ʃInt)"month");

            //for (var i = 0; i < 50; i++)
            //{
            //    GenerateRoutes(site, Guid.NewGuid().ToString());
            //}
        }

        private static void GenerateRoutes(GraphNode site, string name)
        {
            var baseroute = site / name;

            var projectsroute = baseroute / "projects".Controller(name + "projects") / -(ʃInt)"projectId";

            // sites/{siteId}/portfolio/projects
            // sites/{siteId}/portfolio/projects/{projectId}
            // sites/{siteId}/portfolio/projects/{projectId}/media
            // sites/{siteId}/portfolio/projects/{projectId}/media/{id}
            ʃ.Route(o => projectsroute / -"media".Controller(name + "projectmedia") / -(ʃInt)"id");

            // sites/{siteId}/portfolio/tags
            ʃ.Route(o => baseroute / "tags".Controller(name + "tags"));

            // sites/{siteId}/portfolio/categories
            // sites/{siteId}/portfolio/categories/{id}
            ʃ.Route(o => baseroute / "categories".Controller(name + "categories") / -(ʃInt)"id");
        }
    }
}