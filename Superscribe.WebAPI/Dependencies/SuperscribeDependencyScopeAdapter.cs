
namespace Superscribe.WebApi.Owin.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Security;
    using System.Web.Http.Dependencies;

    using Superscribe.Engine;

    public class SuperscribeDependencyScopeAdapter : IDependencyScope
    {
        private bool _disposed;

        private readonly IDependencyScope _requestContainer;

        private readonly IRouteWalker routeWalker;

        [SecuritySafeCritical]
        ~SuperscribeDependencyScopeAdapter()
        {
            Dispose(false);
        }
        
        public SuperscribeDependencyScopeAdapter(
            IDependencyScope requestContainer,
            IRouteWalker routeWalker)
        {
            _requestContainer = requestContainer;
            this.routeWalker = routeWalker;
        }

        [SecuritySafeCritical]
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IRouteWalker))
            {
                return this.routeWalker;
            }

            return _requestContainer.GetService(serviceType);
        }

        [SecuritySafeCritical]
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return
                _requestContainer.GetService(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;
        }

        [SecuritySafeCritical]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_requestContainer != null && _requestContainer is IDisposable)
                    {
                        (_requestContainer as IDisposable).Dispose();
                    }
                }
                _disposed = true;
            }
        }
    }
}
