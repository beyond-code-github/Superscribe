using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Superscribe
{
    public class MediaTypeHandler
    {
        public Func<IDictionary<string, object>, object, Task> Write { get; set; }

        public Func<IDictionary<string, object>, Type, object> Read { get; set; }
    }
}
