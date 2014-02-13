namespace Superscribe.WebApi.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;

    using Superscribe.Engine;
    using Superscribe.WebApi.Owin.Dependencies;

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
            var scope = new SuperscribeDependencyScopeAdapter(originalScope, this.routeEngine.Walker());
            return scope;
        }
    }
}
