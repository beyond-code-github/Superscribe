namespace Superscribe.Owin
{
    using System;
    using System.Threading.Tasks;

    using global::Owin.Types;

    public class ContentHandler
    {
        public Func<OwinResponse, object, Task> Write { get; set; }

        public Func<OwinRequest, Type, object> Read { get; set; }
    }
}
