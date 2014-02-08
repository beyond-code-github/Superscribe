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
            Define.Reset();

            Define.Route(ʅ => ʅ / "sites" / (Int)"siteId" / "blog" / "tags".Controller("blogtags"));
            Define.Route(ʅ => ʅ / "sites" / (Int)"siteId" / "blog" / "posts".Controller("blogposts") / -(Int)"postId" / -"media".Controller("blogpostmedia") / -(Int)"id");
            Define.Route(ʅ => ʅ / "sites" / (Int)"siteId" / "blog" / "posts".Controller("blogposts") / "archives".Controller("blogpostarchives") / -(Int)"year" / (Int)"month");
            Define.Route(ʅ => ʅ / "sites" / (Int)"siteId" / "portfolio" / "projects".Controller("portfolioprojects") / -(Int)"projectId" / -"media".Controller("portfolioprojectmedia") / -(Int)"id");
            Define.Route(ʅ => ʅ / "sites" / (Int)"siteId" / "portfolio" / "tags".Controller("portfoliotags"));
            Define.Route(ʅ => ʅ / "sites" / (Int)"siteId" / "portfolio" / "categories".Controller("portfoliocategories") / -(Int)"id");    
        }
    }
}