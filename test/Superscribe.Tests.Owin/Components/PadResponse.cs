using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Superscribe.Extensions;

namespace Superscribe.Tests.Owin.Components
{
    public class PadResponse
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        private readonly string tag;

        public PadResponse(Func<IDictionary<string, object>, Task> next, string tag)
        {
            this.next = next;
            this.tag = tag;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            await environment.WriteResponse("<" + this.tag + ">");
            await this.next(environment);
            await environment.WriteResponse("</" + this.tag + ">");
        }
    }
}