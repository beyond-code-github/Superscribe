namespace Superscribe.WebApi.Engine
{
    using System.Net.Http;

    using Superscribe.Engine;

    public class WebApiRouteDataAdapter : IWebApiRouteData
    {
        private readonly IRouteData routeData;

        public WebApiRouteDataAdapter(IRouteData routeData)
        {
            this.routeData = routeData;
        }

        public dynamic Parameters
        {
            get
            {
                return routeData.Parameters;
            }
        }

        public bool ActionNameSpecified { get; private set; }

        public string ActionName { get; set; }

        public bool ControllerNameSpecified { get; private set; }

        public string ControllerName { get; set; }

        public object Response { get; set; }

        public bool ParamConversionError { get; set; }

        public HttpRequestMessage Request { get; set; }
    }
}
