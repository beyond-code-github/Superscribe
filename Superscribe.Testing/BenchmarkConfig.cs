namespace Superscribe.Testing
{
    using System;
    using System.Web.Http;

    using Superscribe.Models;
    using Superscribe.Utils;

    public static class BenchmarkConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);

            var site = ʃ.Route(o => o / "sites" / "siteId".Int());

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
            ʃ.Route(o => blogposts / -"postId".Int() / -"media".Controller("blogpostmedia") / -"id".Int());

            // sites/{siteId}/blog/tags
            ʃ.Route(o => blog / "tags".ʃ(i => i.ControllerName = "blogtags"));

            // sites/{siteId}/blog/posts/archives
            // sites/{siteId}/blog/posts/archives/{year}/{month}
            ʃ.Route(o => blogposts / "archives".Controller("blogpostarchives") / -"year".Int() / "month".Int());

            //for (var i = 0; i < 50; i++)
            //{
            //    GenerateRoutes(site, Guid.NewGuid().ToString());
            //}
        }

        private static void GenerateRoutes(SuperscribeState site, string name)
        {
            var baseroute = site / name;

            var projectsroute = baseroute / "projects".Controller(name + "projects") / -"projectId".Int();

            // sites/{siteId}/portfolio/projects
            // sites/{siteId}/portfolio/projects/{projectId}
            // sites/{siteId}/portfolio/projects/{projectId}/media
            // sites/{siteId}/portfolio/projects/{projectId}/media/{id}
            ʃ.Route(o => projectsroute / -"media".Controller(name + "projectmedia") / -"id".Int());

            // sites/{siteId}/portfolio/tags
            ʃ.Route(o => baseroute / "tags".Controller(name + "tags"));

            // sites/{siteId}/portfolio/categories
            // sites/{siteId}/portfolio/categories/{id}
            ʃ.Route(o => baseroute / "categories".Controller(name + "categories") / -"id".Int());
        }
    }
}