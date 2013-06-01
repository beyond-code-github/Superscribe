namespace Superscribe.Testing
{
    using System;
    using System.Web.Http;

    public static class RouteConfig
    {
        public static void Register(HttpConfiguration config)
        {
            for (var i = 0; i < 250; i++)
            {
                GenerateRoutes(config, Guid.NewGuid().ToString());
            }

            GenerateRoutes(config, "portfolio");

            // Blog

            config.Routes.MapHttpRoute(
            name: "BlogPostMediaRoute",
            routeTemplate: "sites/{siteId}/blog/posts/{postId}/media/{id}",
            defaults: new { controller = "blogpostmedia", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
            name: "BlogTagsRoute",
            routeTemplate: "sites/{siteId}/blog/tags",
            defaults: new { controller = "blogtags" }
            );

            config.Routes.MapHttpRoute(
            name: "BlogPostArchiveRoute",
            routeTemplate: "sites/{siteId}/blog/posts/archives/{year}/{month}",
            defaults: new { controller = "blogpostarchives" }
            );

            config.Routes.MapHttpRoute(
            name: "BlogPostArchivesRoute",
            routeTemplate: "sites/{siteId}/blog/posts/archives",
            defaults: new { controller = "blogpostarchives" }
            );

            config.Routes.MapHttpRoute(
            name: "BlogPostsRoute",
            routeTemplate: "sites/{siteId}/blog/posts/{id}",
            defaults: new { controller = "blogposts", id = RouteParameter.Optional }
            );

            //for (var i = 0; i < 50; i++)
            //{
            //    GenerateRoutes(config, Guid.NewGuid().ToString());
            //}
        }

        private static void GenerateRoutes(HttpConfiguration config, string name)
        {
            config.Routes.MapHttpRoute(
                name: name + "ProjectMediaRoute",
                routeTemplate: "sites/{siteId}/" + name + "/projects/{projectId}/media/{id}",
                defaults: new { controller = name + "projectmedia", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: name + "TagsRoute",
                routeTemplate: "sites/{siteId}/" + name + "/tags",
                defaults: new { controller = name + "tags" });

            config.Routes.MapHttpRoute(
                name: name + "ProjectsRoute",
                routeTemplate: "sites/{siteId}/" + name + "/projects/{id}",
                defaults: new { controller = name + "projects", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: name + "CategoriesRoute",
                routeTemplate: "sites/{siteId}/" + name + "/categories/{id}",
                defaults: new { controller = name + "categories", id = RouteParameter.Optional });
        }
    }
}
