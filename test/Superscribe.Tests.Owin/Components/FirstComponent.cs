using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Superscribe.Extensions;

namespace Superscribe.Tests.Owin.Components
{
    public class FirstComponent
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        public FirstComponent(Func<IDictionary<string, object>, Task> next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            await environment.WriteResponse("before#1");
            await this.next(environment);
            await environment.WriteResponse("after#1");
        }
    }
}