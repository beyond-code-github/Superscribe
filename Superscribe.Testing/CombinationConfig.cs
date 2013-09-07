namespace Superscribe.Testing
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

            ʃ.Route((root, ʅ) => root / "sites" / (ʃInt)"siteId" / "blog" / "tags".Controller("blogtags"));
            ʃ.Route((root, ʅ) => root / "sites" / (ʃInt)"siteId" / "blog" / "posts".Controller("blogposts") / -(ʃInt)"postId" / -"media".Controller("blogpostmedia") / -(ʃInt)"id");
            ʃ.Route((root, ʅ) => root / "sites" / (ʃInt)"siteId" / "blog" / "posts".Controller("blogposts") / "archives".Controller("blogpostarchives") / -(ʃInt)"year" / (ʃInt)"month");
            ʃ.Route((root, ʅ) => root / "sites" / (ʃInt)"siteId" / "portfolio" / "projects".Controller("portfolioprojects") / -(ʃInt)"projectId" / -"media".Controller("portfolioprojectmedia") / -(ʃInt)"id");
            ʃ.Route((root, ʅ) => root / "sites" / (ʃInt)"siteId" / "portfolio" / "tags".Controller("portfoliotags"));
            ʃ.Route((root, ʅ) => root / "sites" / (ʃInt)"siteId" / "portfolio" / "categories".Controller("portfoliocategories") / -(ʃInt)"id");    
        }
    }
}