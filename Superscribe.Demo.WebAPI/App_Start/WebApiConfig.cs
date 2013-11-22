namespace Superscribe.Demo.WebApi.App_Start
{
    using System.Web.Http;

    using Superscribe.Models;
    using Superscribe.WebApi;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);

            ʃ.Route(() => "api" / "values".Controller() / (
                  -(ʃInt)"id"
                | ~"(first|last)".Action()
                | +("foruser" / (ʃInt)"userId")));
        }
    }
}
