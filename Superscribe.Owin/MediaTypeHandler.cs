namespace Superscribe.Owin
{
    using System;
    using System.Threading.Tasks;
    
    using Microsoft.Owin;

    public class MediaTypeHandler
    {
        public Func<IOwinResponse, object, Task> Write { get; set; }

        public Func<IOwinRequest, Type, object> Read { get; set; }
    }
}
