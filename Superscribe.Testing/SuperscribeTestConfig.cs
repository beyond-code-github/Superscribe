namespace Superscribe.Testing
{
    using System.Web.Http;

    using Superscribe.WebApi;

    public static class SuperscribeTestConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);

            ʃ.Route(s => s / "sites" / "siteId".Int() / (
                +("blog" / (
                      +("tags".Controller("blogtags"))
                    | +("posts".Controller("blogposts") / (
                          +(-"postId".Int() / -"media".Controller("blogpostmedia") / -"id".Int())
                        | +("archives".Controller("blogpostarchives") / -"year".Int() / "month".Int())))))
                | +("portfolio" / (
                      +("projects".Controller("portfolioprojects") / -"projectId".Int() / -"media".Controller("portfolioprojectmedia") / +"id".Int())
                    | +("tags".Controller("portfoliotags"))
                    | +("categories".Controller("portfoliocategories") / -"id".Int())))));
        }
    }
}