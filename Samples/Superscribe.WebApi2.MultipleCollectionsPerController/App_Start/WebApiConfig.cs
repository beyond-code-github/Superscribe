namespace Superscribe.WebApi2.MultipleCollectionsPerController.App_Start
{
    using System.Web.Http;

    using Superscribe.Models;
    using Superscribe.WebApi2;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            SuperscribeConfig.Register(config);

            ʃ.Route(ʅ => "api" / "Blogs".Controller() / (
                  ʅ / -(ʃInt)"id"
                | ʅ["GET"] / (
                      ʅ / "Posts".Action("GetBlogPosts")
                    | ʅ / "Tags".Action("GetBlogTags"))
                | ʅ["POST"] / (
                      ʅ / "Posts".Action("PostBlogPost") / (ʃInt)"id" 
                    | ʅ / "Tags".Action("PostBlogTag") / (ʃInt)"id")));

            //config.Routes.MapHttpRoute(
            //    name: "Mail by Location",
            //    routeTemplate: "api/blog/{location}",
            //    defaults: new { controller = "Mail" },
            //    constraints: new { location = @"[a-zA-Z]+" }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "Mail",
            //    routeTemplate: "api/mail/{id}",
            //    defaults: new { controller = "Mail", id = RouteParameter.Optional }
            //);

        }
    }
}
