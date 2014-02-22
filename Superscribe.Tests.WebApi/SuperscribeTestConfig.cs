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

            var forms = engine.Route(r => r / "api" / (Long)"parentId" / "Forms".Controller());
            var blog = engine.Route(r => r / "sites" / (Int)"siteId" / "blog");
            var portfolio = engine.Route(r => r / "sites" / (Int)"siteId" / "portfolio");

            var blogposts = engine.Route(blog / "posts".Controller("blogposts"));

            engine.Route(forms / "VisibleFor".Action() / (String)"appDataId");
            engine.Route(forms / -(Long)"id");
            
            engine.Route(blog / "tags".Controller("blogtags"));
            
            engine.Route(blogposts / -(Int)"postId" / -"media".Controller("blogpostmedia") / -(Int)"id");
            engine.Route(blogposts / "archives".Controller("blogpostarchives") / -(Int)"year" / (Int)"month");

            engine.Route(portfolio / "projects".Controller("portfolioprojects") / -(Int)"projectId" / -"media".Controller("portfolioprojectmedia") / -(Int)"id");
            engine.Route(portfolio / "tags".Controller("portfoliotags"));
            engine.Route(portfolio / "categories".Controller("portfoliocategories") / -(Int)"id");
        }
    }
}