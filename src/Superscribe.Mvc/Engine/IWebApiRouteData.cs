namespace Superscribe.WebApi.Engine
{
    using System.Net.Http;

    public interface IWebApiRouteData
    {
        dynamic Parameters { get; }

        bool ActionNameSpecified { get; }

        string ActionName { get; }

        bool ControllerNameSpecified { get; }

        string ControllerName { get; }

        object Response { get; }

        bool ParamConversionError { get; }

        HttpRequestMessage Request { get; }
    }
}