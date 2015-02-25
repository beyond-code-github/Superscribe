using System;
using System.Collections.Generic;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Superscribe.Extensions
{
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<AppFunc, AppFunc>>>;

    public static class MiddlewareExtensions
    {
        public static AppDelegate AsTyped(this AppFunc appfunc)
        {
            return env => appfunc(env);
        }

        public static AppFunc AsUntyped(this AppDelegate typedAppfunc)
        {
            return env => typedAppfunc(env);
        }

        public static Func<AppFunc, AppFunc> AsUntyped(this MidDelegate midDelegate)
        {
            return next => midDelegate(next.AsTyped()).AsUntyped();
        }

        public static BuildFunc Use(
            this BuildFunc appBuilder,
            Func<IDictionary<string, object>, MidDelegate> middleware)
        {
            appBuilder(properties => middleware(properties).AsUntyped());
            return appBuilder;
        }
    }
}