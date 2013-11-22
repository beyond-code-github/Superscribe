namespace Superscribe.WebApi2.MultipleCollectionsPerController
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Superscribe.WebApi2.MultipleCollectionsPerController.App_Start;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
