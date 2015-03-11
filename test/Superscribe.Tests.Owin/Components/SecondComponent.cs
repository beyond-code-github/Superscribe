using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Superscribe.Extensions;

namespace Superscribe.Tests.Owin.Components
{
    public class SecondComponent
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        public SecondComponent(Func<IDictionary<string, object>, Task> next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            await environment.WriteResponse("before#2");
            await this.next(environment);
            await environment.WriteResponse("after#2");
        }
    }
}