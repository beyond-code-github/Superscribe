namespace Superscribe.Tests.Owin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using global::Owin;

    using Superscribe.Owin;
    using Superscribe.Owin.Extensions;

    public class TestMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        private readonly string value;

        public TestMiddleware(Func<IDictionary<string, object>, Task> next, string value)
        {
            this.next = next;
            this.value = value;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            environment.TrySetHeaderValues(this.value, new[] { this.value });
            await this.next(environment);
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new SuperscribeOwinConfig();
            config.MediaTypeHandlers.Add(
                "text/html",
                new MediaTypeHandler
                {
                    Read = (env, o) =>
                        {
                            using (var reader = new StreamReader(env.GetRequestBody())) return reader.ReadToEnd();
                        },
                    Write = (env, o) => env.WriteResponse(o.ToString())
                });

            ʃ.Route(ʅ => o => "Hello World");

            app.Use(typeof(TestMiddleware), "before")
                .Use(typeof(SuperscribeMiddleware), config)
                .Use(typeof(TestMiddleware), "after");
        }
    }
}
