namespace Superscribe.Testing.WebApi
{
    using System.Web.Http;

    using Superscribe.Engine;
    using Superscribe.Models;
    using Superscribe.WebApi;
    using Superscribe.WebApi.Dependencies;

    public static class SuperscribeTestConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);
            var engine = RouteEngineFactory.Create();

            config.DependencyResolver = new SuperscribeDependencyAdapter(config.DependencyResolver, engine);

            engine.Route(ʅ => ʅ / "api" / (Long)"parentId" /
                        "Forms".Controller() / (
                            ʅ / "VisibleFor".Action() / (String)"appDataId"
                          | ʅ / -(Long)"id"));

            engine.Route(ʅ => ʅ / "sites" / (Int)"siteId" / (
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