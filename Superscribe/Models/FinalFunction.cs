
namespace Superscribe.Models
{
    using System;

    public class FinalFunction
    {
        public string Method { get; set; }

        public Action<RouteData> Function { get; set; }

        public static FinalFunctionList operator |(FinalFunction function, FinalFunction other)
        {
            return new FinalFunctionList { function, other };
        }

        public static FinalFunctionList operator |(FinalFunctionList functions, FinalFunction other)
        {
            functions.Add(other);
            return functions;
        }
    }
}
