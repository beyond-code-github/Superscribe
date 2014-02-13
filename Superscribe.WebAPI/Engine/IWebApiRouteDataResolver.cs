namespace Superscribe.WebApi.Engine
{
    public interface IWebApiRouteDataResolver
    {
        IWebApiRouteData GetData(string url, string method);
    }
}