namespace Superscribe.Testing
{
    using System.Web.Http;

    using global::Superscribe.Utils;

    public static class SuperscribeConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Superscribe.Register(config);

            ʃ.Route(s => s / "sites" / "siteId".Int() / (
                -("blog" / (
                      -("tags".Controller("blogtags"))
                    | -("posts".Controller("blogposts") / (
                          -(~"postId".Int() / ~"media".Controller("blogpostmedia") / ~"id".Int())
                        | -("archives".Controller("blogpostarchives") / ~"year".Int() / "month".Int())))))
                | -("portfolio" / (
                      -("projects".Controller("portfolioprojects") / ~"projectId".Int() / ~"media".Controller("portfolioprojectmedia") / ~"id".Int())
                    | -("tags".Controller("portfoliotags"))
                    | -("categories".Controller("portfoliocategories") / ~"id".Int())))));
        }

        public static void Register2(HttpConfiguration config)
        {
            Superscribe.Register(config);

            var site = ʃ.Route(o => o / "sites" / "siteId".Int());

            var blog = site / "blog";
            var blogposts = blog / "posts".Controller("blogposts");
            var baseroute = site / "portfolio";

            var projectsroute = baseroute / "projects".Controller("portfolioprojects") / "projectId".Int();

            // sites/{siteId}/portfolio/projects
            // sites/{siteId}/portfolio/projects/{projectId}
            // sites/{siteId}/portfolio/projects/{projectId}/media
            // sites/{siteId}/portfolio/projects/{projectId}/media/{id}
            ʃ.Route(o => projectsroute / ~"media".Controller("portfolioprojectmedia") / ~"id".Int());

            // sites/{siteId}/portfolio/tags
            ʃ.Route(o => baseroute / "tags".Controller("portfoliotags"));

            // sites/{siteId}/portfolio/categories
            // sites/{siteId}/portfolio/categories/{id}
            ʃ.Route(o => baseroute / "categories".Controller("portfoliocategories") / ~"id".Int());

            // sites/{siteId}/blog/posts/
            // sites/{siteId}/blog/posts/{postId}
            // sites/{siteId}/blog/posts/{postId}/media
            // sites/{siteId}/blog/posts/{postId}/media/{id}
            ʃ.Route(o => blogposts / ~"postId".Int() / ~"media".Controller("blogpostmedia") / ~"id".Int());

            // sites/{siteId}/blog/tags
            ʃ.Route(o => blog / "tags".ʃ(i => i.ControllerName = "blogtags"));

            // sites/{siteId}/blog/posts/archives
            // sites/{siteId}/blog/posts/archives/{year}/{month}
            ʃ.Route(o => blogposts / "archives".Controller("blogpostarchives") / ~"year".Int() / "month".Int());
        }
    }
}