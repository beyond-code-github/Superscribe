namespace Superscribe.WebAPI
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    using Superscribe.Models;
    using Superscribe.WebAPI.Internals;

    public class SuperscribeActionInvoker : IHttpActionInvoker
    {
        public Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            var actionDescriptor = actionContext.ActionDescriptor;
            var controllerContext = actionContext.ControllerContext;

            var webApiInfo = new RouteData();
            var walker = SuperscribeConfig.Walker();
            walker.WalkRoute(controllerContext.Request.RequestUri.AbsolutePath, webApiInfo);

            foreach (var param in actionContext.ActionArguments)
            {
                if (!webApiInfo.Parameters.ContainsKey(param.Key))
                {
                    webApiInfo.Parameters.Add(param.Key, param.Value);
                }
            }

            var arguments = webApiInfo.Parameters;

            return
                Extensions.RunSynchronously(
                    () =>
                    actionDescriptor.ExecuteAsync(controllerContext, arguments, cancellationToken)
                                    .Then(
                                        value => actionDescriptor.ResultConverter.Convert(controllerContext, value),
                                        cancellationToken),
                    cancellationToken).Catch(
                        info =>
                        {
                            // Propagate anything which isn't HttpResponseException
                            var httpResponseException = info.Exception as HttpResponseException;
                            if (httpResponseException == null)
                            {
                                return info.Throw();
                            }

                            var response = httpResponseException.Response;
                            response.EnsureResponseHasRequest(actionContext.Request);

                            return info.Handled(response);
                        },
                        cancellationToken);
        }
    }
}
