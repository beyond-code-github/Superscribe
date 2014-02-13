namespace Superscribe.Owin.Components
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class RedirectMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        public RedirectMiddleware(Func<IDictionary<string, object>, Task> ignored, Func<IDictionary<string, object>, Task> next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            await this.next(environment);
        }
    }
}
