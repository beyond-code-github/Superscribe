namespace Superscribe.WebApi
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Superscribe.Models;

    public class SuperscribeHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var provider = request.GetRouteDataProvider();
            var info = provider.GetData(request);

            if (info.ExtraneousMatch || !info.FinalFunctionExecuted || info.Response is FinalFunction.ExecuteAndContinue)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var responseMessage = info.Response as HttpResponseMessage;
            if (responseMessage != null)
            {
                return Task.FromResult(responseMessage);
            }

            var statusCode = info.Response as HttpStatusCode?;
            if (statusCode != null)
            {
                return Task.FromResult(request.CreateResponse(statusCode.Value));
            }

            return Task.FromResult(request.CreateResponse(HttpStatusCode.OK, info.Response));
        }
    }
}
