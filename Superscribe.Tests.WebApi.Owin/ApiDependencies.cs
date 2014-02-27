namespace Superscribe.Tests.WebApi.Owin
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Autofac;

    using DotNetDoodle.Owin.Dependencies;

    public class ApiDependencies
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        public ApiDependencies(Func<IDictionary<string, object>, Task> next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var container = environment.GetRequestContainer() as ILifetimeScope;
            var newBuilder = new ContainerBuilder();
            newBuilder.RegisterType<ApiRepository>()
                   .As<IRepository>()
                   .InstancePerLifetimeScope();

            newBuilder.Update(container.ComponentRegistry);

            await this.next(environment);
        }
    }
}
