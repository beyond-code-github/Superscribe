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

            ʃ.Route((root, ʅ) => root / "sites" / (ʃInt)"siteId" / "blog" / "tags".Controller("blogtags"));
            ʃ.Route((root, ʅ) => root / "sites" / (ʃInt)"siteId" / "blog" / "posts".Controller("blogposts"));
            ʃ.Route((root, ʅ) => root / "sites" / (ʃInt)"siteId" / "portfolio" / "tags".Controller("portfoliotags"));
            ʃ.Route((root, ʅ) => root / "sites" / (ʃInt)"siteId" / "portfolio" / "categories".Controller("portfoliocategories") / -(ʃInt)"id");
        }
    }
}