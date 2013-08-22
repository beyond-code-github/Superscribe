namespace Superscribe.WebApi.Internals
{
    using System.Net.Http;

    public static class HttpResponseMessageExtensions
    {
        internal static void EnsureResponseHasRequest(this HttpResponseMessage response, HttpRequestMessage request)
        {
            if (response != null && response.RequestMessage == null)
            {
                response.RequestMessage = request;
            }
        }
    }
}
