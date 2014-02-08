namespace Superscribe.Testing.WebApi
{
    using System.Web.Http;

    using Superscribe.Models;
    using Superscribe.WebApi;

    public static class CombinationConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);
            ʃ.Reset();

            ʃ.Route(ʅ => ʅ / "sites" / (Int)"siteId" / "blog" / "tags".Controller("blogtags"));
            ʃ.Route(ʅ => ʅ / "sites" / (Int)"siteId" / "blog" / "posts".Controller("blogposts") / -(Int)"postId" / -"media".Controller("blogpostmedia") / -(Int)"id");
            ʃ.Route(ʅ => ʅ / "sites" / (Int)"siteId" / "blog" / "posts".Controller("blogposts") / "archives".Controller("blogpostarchives") / -(Int)"year" / (Int)"month");
            ʃ.Route(ʅ => ʅ / "sites" / (Int)"siteId" / "portfolio" / "projects".Controller("portfolioprojects") / -(Int)"projectId" / -"media".Controller("portfolioprojectmedia") / -(Int)"id");
            ʃ.Route(ʅ => ʅ / "sites" / (Int)"siteId" / "portfolio" / "tags".Controller("portfoliotags"));
            ʃ.Route(ʅ => ʅ / "sites" / (Int)"siteId" / "portfolio" / "categories".Controller("portfoliocategories") / -(Int)"id");    
        }
    }
}