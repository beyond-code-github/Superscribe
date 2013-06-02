namespace Superscribe.Demo.WebApi.App_Start
{
    using System.Web.Http;
    using Superscribe.WebApi;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);

            ʃ.Route(o => o / "api" / "values".Controller() / (
                  -"id".Int()
                | ~"(first|last)".Action()
                | +("foruser" / "userId".Int())));
        }
    }
}
