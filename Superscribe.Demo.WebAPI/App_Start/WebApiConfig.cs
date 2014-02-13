namespace Superscribe.Demo.WebApi.App_Start
{
    using System.Web.Http;

    using Superscribe.Engine;
    using Superscribe.Models;
    using Superscribe.WebApi;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            SuperscribeConfig.Register(config);
            var engine = RouteEngineFactory.Create();

            engine.Route(() => "api" / "values".Controller() / (
                  -(Int)"id"
                | ~"(first|last)".Action()
                | +("foruser" / (Int)"userId")));
        }
    }
}
