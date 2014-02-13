namespace Superscribe.Testing.WebApi
{
    using System.Web.Http;

    using Superscribe.Models;
    using Superscribe.WebApi;

    public static class CombinationConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var engine = SuperscribeConfig.Register(config);

            engine.Route("sites" / (Int)"siteId" / "blog" / "tags".Controller("blogtags"));
            engine.Route("sites" / (Int)"siteId" / "blog" / "posts".Controller("blogposts") / -(Int)"postId" / -"media".Controller("blogpostmedia") / -(Int)"id");
            engine.Route("sites" / (Int)"siteId" / "blog" / "posts".Controller("blogposts") / "archives".Controller("blogpostarchives") / -(Int)"year" / (Int)"month");
            engine.Route("sites" / (Int)"siteId" / "portfolio" / "projects".Controller("portfolioprojects") / -(Int)"projectId" / -"media".Controller("portfolioprojectmedia") / -(Int)"id");
            engine.Route("sites" / (Int)"siteId" / "portfolio" / "tags".Controller("portfoliotags"));
            engine.Route("sites" / (Int)"siteId" / "portfolio" / "categories".Controller("portfoliocategories") / -(Int)"id");    
        }
    }
}