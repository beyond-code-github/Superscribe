namespace Superscribe.WebApi2.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    using Superscribe.WebApi2.Internals;

    public class SuperscribeModuleActionInvoker : IHttpActionInvoker
    {
        public Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            var actionDescriptor = actionContext.ActionDescriptor;
            var controllerContext = actionContext.ControllerContext;

            return TaskHelpers.RunSynchronously(() =>
            {
                return actionDescriptor.ExecuteAsync(controllerContext, new Dictionary<string, object>(), cancellationToken)
                                       .Then(value => actionDescriptor.ResultConverter.Convert(controllerContext, value), cancellationToken);
            }, cancellationToken)
            .Catch<HttpResponseMessage>(info =>
            {
                // Propagate anything which isn't HttpResponseException
                HttpResponseException httpResponseException = info.Exception as HttpResponseException;
                if (httpResponseException == null)
                {
                    return info.Throw();
                }

                HttpResponseMessage response = httpResponseException.Response;
                response.EnsureResponseHasRequest(actionContext.Request);

                return info.Handled(response);
            }, cancellationToken);
        }
    }
}
