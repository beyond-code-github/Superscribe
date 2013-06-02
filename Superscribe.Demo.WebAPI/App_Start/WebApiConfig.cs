namespace Superscribe.Demo.WebAPI.App_Start
{
    using System.Web.Http;
    using Superscribe.WebAPI;

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
