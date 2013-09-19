namespace Superscribe.Demo.WebApiModules.App_Start
{
    using System.Web.Http;

    using Superscribe.WebApi;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableSystemDiagnosticsTracing();

            SuperscribeConfig.RegisterModules(config);
        }
    }
}
