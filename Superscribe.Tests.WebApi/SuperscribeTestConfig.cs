namespace Superscribe.Testing.Http
{
    using System.Web.Http;

    using Superscribe.Models;
    using Superscribe.WebApi;

    public static class SuperscribeTestConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);
            ʃ.Reset();

            ʃ.Route(ʅ => ʅ / "api" / (ʃLong)"parentId" /
                        "Forms".Controller() / (
                            ʅ / "VisibleFor".Action() / (ʃString)"appDataId"
                          | ʅ / -(ʃLong)"id"));

            ʃ.Route(ʅ => ʅ / "sites" / (ʃInt)"siteId" / (
                ʅ / "blog" / (
                      ʅ / "tags".Controller("blogtags")
                    | ʅ / "posts".Controller("blogposts") / (
                          ʅ / -(ʃInt)"postId" / -"media".Controller("blogpostmedia") / -(ʃInt)"id"
                        | ʅ / "archives".Controller("blogpostarchives") / -(ʃInt)"year" / (ʃInt)"month"))
                | ʅ / "portfolio" / (
                      ʅ / "projects".Controller("portfolioprojects") / -(ʃInt)"projectId" / -"media".Controller("portfolioprojectmedia") / -(ʃInt)"id"
                    | ʅ / "tags".Controller("portfoliotags")
                    | ʅ / "categories".Controller("portfoliocategories") / -(ʃInt)"id")));
        }
    }
}