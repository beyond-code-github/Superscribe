namespace Superscribe.WebApi.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Security;
    using System.Web.Http.Dependencies;

    using Superscribe.Engine;
    using Superscribe.WebApi.Engine;

    public class SuperscribeDependencyScopeAdapter : IDependencyScope
    {
        private bool _disposed;

        private readonly IDependencyScope _requestContainer;

        private readonly IRouteWalker routeWalker;

        private readonly IWebApiRouteDataProvider routeDataProvider;

        [SecuritySafeCritical]
        ~SuperscribeDependencyScopeAdapter()
        {
            this.Dispose(false);
        }

        public SuperscribeDependencyScopeAdapter(
            IDependencyScope requestContainer,
            IRouteWalker routeWalker,
            IRouteDataProvider routeDataProvider)
        {
            this._requestContainer = requestContainer;
            this.routeWalker = routeWalker;
            this.routeDataProvider = new WebApiRouteDataProviderAdapter(routeDataProvider);
        }

        [SecuritySafeCritical]
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IRouteWalker))
            {
                return this.routeWalker;
            }

            if (serviceType == typeof(IWebApiRouteDataProvider))
            {
                return this.routeDataProvider;
            }

            return this._requestContainer.GetService(serviceType);
        }

        [SecuritySafeCritical]
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return
                this._requestContainer.GetService(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;
        }

        [SecuritySafeCritical]
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    if (this._requestContainer != null && this._requestContainer is IDisposable)
                    {
                        (this._requestContainer as IDisposable).Dispose();
                    }
                }
                this._disposed = true;
            }
        }
    }
}
