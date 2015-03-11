using System;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Superscribe.Pipelining
{
    using MidFunc = Func<AppFunc, AppFunc>;

    public class Middleware
    {
        public Func<MidFunc, MidFunc> Compose { get; set; }
    }
}
