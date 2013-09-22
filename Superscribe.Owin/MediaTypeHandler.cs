namespace Superscribe.Owin
{
    using System;
    using System.Threading.Tasks;

    using global::Owin.Types;

    public class MediaTypeHandler
    {
        public Func<OwinResponse, object, Task> Write { get; set; }

        public Func<OwinRequest, Type, object> Read { get; set; }
    }
}
