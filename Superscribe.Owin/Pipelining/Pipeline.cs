namespace Superscribe.Owin.Pipelining
{
    using System;

    using global::Owin;

    public class Pipeline
    {
        public static OwinNodeFuture Action<T>(params object[] args)
        {
            return new OwinNodeFuture(typeof(T), args);
        }

        public static OwinNodeFuture Action(Func<IAppBuilder, IAppBuilder> func, params object[] args)
        {
            return new OwinNodeFuture(func, args);
        }
    }
}
