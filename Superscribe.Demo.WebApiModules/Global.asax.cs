namespace Superscribe.Demo.WebApiModules
{
    using System.Web.Http;
    using System.Web.Mvc;

    using Superscribe.Demo.WebApiModules.App_Start;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters); 
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}