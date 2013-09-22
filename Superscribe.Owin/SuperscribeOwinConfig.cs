namespace Superscribe.Owin
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using global::Owin.Types;

    public class SuperscribeOwinConfig
    {
        public SuperscribeOwinConfig()
        {
            this.ContentHandlers = new Dictionary<string, Func<OwinResponse, object, Task>>();
        }

        public Dictionary<string, Func<OwinResponse, object, Task>> ContentHandlers { get; set; }
    }
}
