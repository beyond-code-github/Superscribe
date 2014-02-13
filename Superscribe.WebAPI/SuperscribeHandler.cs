namespace Superscribe.WebApi
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Superscribe.Engine;
    using Superscribe.WebApi.Modules;

    public class SuperscribeHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var webApiInfo = new ModuleRouteData
            {
                Request = request
            };

            var walker = request.GetDependencyScope().GetService(typeof(IRouteWalker)) as IRouteWalker;
            webApiInfo = walker.WalkRoute(
                request.RequestUri.PathAndQuery,
                request.Method.ToString(),
                webApiInfo) as ModuleRouteData;

            if (walker.ExtraneousMatch || walker.IncompleteMatch || !walker.FinalFunctionExecuted)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var responseMessage = webApiInfo.Response as HttpResponseMessage;
            if (responseMessage != null)
            {
                return Task.FromResult(responseMessage);
            }

            var statusCode = webApiInfo.Response as HttpStatusCode?;
            if (statusCode != null)
            {
                return Task.FromResult(request.CreateResponse(statusCode.Value));
            }

            return Task.FromResult(request.CreateResponse(HttpStatusCode.OK, webApiInfo.Response));
        }
    }
}
