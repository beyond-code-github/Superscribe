namespace Superscribe.WebApi
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Web.Http.Controllers;

    using Superscribe.Models;
    using Superscribe.WebApi.Internals;

    public class SuperscribeActionSelector : IHttpActionSelector
    {
        private ActionSelectorCacheItem fastCache;

        private readonly object cacheKey = new object();

        public HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            var info = new RouteData();

            var walker = SuperscribeConfig.Walker();
            walker.WalkRoute(controllerContext.Request.RequestUri.AbsolutePath, info);

            var internalSelector = GetInternalSelector(controllerContext.ControllerDescriptor);

            if (info.ActionNameSpecified)
            {
                return internalSelector.SelectAction(controllerContext, info.Parameters.Select(o => o.Key), info.ActionName);
            }

            return internalSelector.SelectAction(controllerContext, info.Parameters.Select(o => o.Key));
        }

        public ILookup<string, HttpActionDescriptor> GetActionMapping(HttpControllerDescriptor controllerDescriptor)
        {
            if (controllerDescriptor == null)
            {
                throw new ArgumentNullException("controllerDescriptor");
            }

            var internalSelector = GetInternalSelector(controllerDescriptor);
            return internalSelector.GetActionMapping();
        }

        private ActionSelectorCacheItem GetInternalSelector(HttpControllerDescriptor controllerDescriptor)
        {
            // First check in the local fast cache and if not a match then look in the broader 
            // HttpControllerDescriptor.Properties cache
            if (this.fastCache == null)
            {
                var selector = new ActionSelectorCacheItem(controllerDescriptor);
                Interlocked.CompareExchange(ref this.fastCache, selector, null);
                return selector;
            }

            if (this.fastCache.HttpControllerDescriptor == controllerDescriptor)
            {
                // If the key matches and we already have the delegate for creating an instance then just execute it
                return this.fastCache;
            }

            // If the key doesn't match then lookup/create delegate in the HttpControllerDescriptor.Properties for
            // that HttpControllerDescriptor instance
            return (ActionSelectorCacheItem)controllerDescriptor.Properties.GetOrAdd(
               this.cacheKey,
               _ => new ActionSelectorCacheItem(controllerDescriptor));
        }
    }
}
