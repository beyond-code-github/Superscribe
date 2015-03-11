using System.Collections.Generic;
using System.Threading.Tasks;

namespace Superscribe
{
    public delegate Task AppDelegate(IDictionary<string, object> env);

    public delegate AppDelegate MidDelegate(AppDelegate next);
}