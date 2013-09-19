namespace Superscribe.Demo.WebApiModules
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Superscribe.Demo.WebApiModules.App_Start;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}