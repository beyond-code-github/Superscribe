namespace Superscribe.Testing.WebApi
{
    using System.Web.Http;

    using Superscribe.Models;
    using Superscribe.WebApi;

    public static class SuperscribeTestConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);
            Define.Reset();

            Define.Route(ʅ => ʅ / "api" / (Long)"parentId" /
                        "Forms".Controller() / (
                            ʅ / "VisibleFor".Action() / (String)"appDataId"
                          | ʅ / -(Long)"id"));

            Define.Route(ʅ => ʅ / "sites" / (Int)"siteId" / (
                ʅ / "blog" / (
                      ʅ / "tags".Controller("blogtags")
                    | ʅ / "posts".Controller("blogposts") / (
                          ʅ / -(Int)"postId" / -"media".Controller("blogpostmedia") / -(Int)"id"
                        | ʅ / "archives".Controller("blogpostarchives") / -(Int)"year" / (Int)"month"))
                | ʅ / "portfolio" / (
                      ʅ / "projects".Controller("portfolioprojects") / -(Int)"projectId" / -"media".Controller("portfolioprojectmedia") / -(Int)"id"
                    | ʅ / "tags".Controller("portfoliotags")
                    | ʅ / "categories".Controller("portfoliocategories") / -(Int)"id")));
        }
    }
}