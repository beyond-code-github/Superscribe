namespace Superscribe.WebApi.Engine
{
    using System.Net.Http;

    using Superscribe.Engine;

    public interface IWebApiRouteDataProvider
    {
        IRouteData GetData(HttpRequestMessage request);
    }
}
