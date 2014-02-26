namespace Superscribe.Engine
{
    using System.Collections.Generic;

    using Superscribe.Utils;

    public class RouteData : IRouteData
    {
        public dynamic Parameters { get; set; }

        public IDictionary<string, object> Environment { get; set; }
        
        public object Response { get; set; }

        public bool IncompleteMatch { get; set; }

        public bool ExtraneousMatch { get; set; }
        
        public bool NoMatchingFinalFunction { get; set; }

        public bool FinalFunctionExecuted { get; set; }

        public string Method { get; set; }

        public string Url { get; set; }
        
        public RouteData()
        {
            this.Environment = new Dictionary<string, object>();
            this.Parameters = new DynamicDictionary();
        }
    }
}
