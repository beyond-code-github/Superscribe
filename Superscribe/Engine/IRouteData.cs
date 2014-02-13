namespace Superscribe.Engine
{
    using System.Collections.Generic;

    public interface IRouteData
    {
        dynamic Parameters { get; set; }

        IDictionary<string, object> Environment { get; set; }
        
        object Response { get; set; }
    }
}
