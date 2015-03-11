using System;
using System.Collections.Generic;
using Superscribe.Engine;

namespace Superscribe.WebApi.Dependency
{
    public class SuperscribeDependencyAdapter : IDependencyResolver
    {
        private readonly IDependencyResolver baseDependencyResolver;

        private readonly IRouteEngine routeEngine;

        public SuperscribeDependencyAdapter(IDependencyResolver baseDependencyResolver, IRouteEngine routeEngine)
        {
            this.baseDependencyResolver = baseDependencyResolver;
            this.routeEngine = routeEngine;
        }

        public void Dispose()
        {
            this.baseDependencyResolver.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return this.baseDependencyResolver.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.baseDependencyResolver.GetServices(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            var originalScope = this.baseDependencyResolver.BeginScope();
            var walker = this.routeEngine.Walker();
            var scope = new SuperscribeDependencyScopeAdapter(originalScope, walker, new LazyRouteDataProvider(walker));
            return scope;
        }
    }
}
