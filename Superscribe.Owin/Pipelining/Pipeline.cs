namespace Superscribe.Owin.Pipelining
{
    using System;

    using global::Owin;

    public class Pipeline
    {
        public static OwinNodeFuture Action<T>()
        {
            return new OwinNodeFuture(typeof(T));
        }

        public static OwinNodeFuture Use(Func<IAppBuilder, IAppBuilder> func)
        {
            return new OwinNodeFuture(func);
        }
    }
}
