namespace Superscribe.Engine
{
    using System.Collections.Generic;

    using Superscribe.Utils;

    public class RouteData : IRouteData
    {
        public RouteData()
        {
            this.Environment = new Dictionary<string, object>();
            this.Parameters = new DynamicDictionary();
        }

        public dynamic Parameters { get; set; }

        public IDictionary<string, object> Environment { get; set; }

        public object Response { get; set; }

        public bool ExtraneousMatch { get; set; }

        public bool NoMatchingFinalFunction { get; set; }

        public bool FinalFunctionExecuted { get; set; }
    }
}
