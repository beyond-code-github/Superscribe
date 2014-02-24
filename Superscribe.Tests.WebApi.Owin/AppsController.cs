namespace Superscribe.Tests.WebApi.Owin
{
    using System.Web.Http;

    public class AppsController : ApiController
    {
        public string GetByIdentifier(string appIdentifier)
        {
            return "Get by identifier: " + appIdentifier;
        }

        public string GetSettings(string appIdentifier)
        {
            return "Get settings: " + appIdentifier;
        }
    }
}
