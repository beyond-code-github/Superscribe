namespace Superscribe.Engine
{
    using System.Collections.Generic;

    public interface IRouteData
    {
        string Method { get; set; }

        string Url { get; set; }

        dynamic Parameters { get; set; }

        IDictionary<string, object> Environment { get; set; }
        
        object Response { get; set; }
        
        bool ExtraneousMatch { get; set; }

        bool FinalFunctionExecuted { get; set; }

        bool NoMatchingFinalFunction { get; set; }
    }
}
