namespace Superscribe.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    using Superscribe.Engine;
    using Superscribe.WebApi.Engine;
    using Superscribe.WebApi.Internals;

    public class SuperscribeActionInvokerAdapter : IHttpActionInvoker
    {
        private IHttpActionInvoker baseInvoker;

        public SuperscribeActionInvokerAdapter(IHttpActionInvoker baseInvoker)
        {
            this.baseInvoker = baseInvoker;
        }

        public Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            var actionDescriptor = actionContext.ActionDescriptor;
            var controllerContext = actionContext.ControllerContext;

            var provider = actionContext.Request.GetRouteDataProvider();
            var routedata = provider.GetData(actionContext.Request);

            foreach (var param in actionContext.ActionArguments)
            {
                if (!routedata.Parameters.ContainsKey(param.Key))
                {
                    routedata.Parameters.Add(param.Key, param.Value);
                }
            }

            var arguments = routedata.Parameters;

            return TaskHelpers.RunSynchronously(() =>
            {
                return actionDescriptor.ExecuteAsync(controllerContext, (IDictionary<string, object>)arguments, cancellationToken)
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
