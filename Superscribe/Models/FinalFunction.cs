
namespace Superscribe.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Superscribe.Models.Filters;

    public class ExclusiveFinalFunction : FinalFunction
    {
        public ExclusiveFinalFunction()
        {
        }

        public ExclusiveFinalFunction(Func<dynamic, object> func, params Filter[] filters)
            : base(func, filters)
        {
        }

        public override bool IsExclusive
        {
            get
            {
                return true;
            }
        }
    }

    public class FinalFunction
    {
        public FinalFunction()
        {
            this.Filters = new List<Filter>();
        }

        public FinalFunction(Func<dynamic, object> func, params Filter[] filters)
        {
            this.Filters = new List<Filter>();
            this.Filters.AddRange(filters);
            this.Function = func;
        }

        public virtual bool IsExclusive
        {
            get
            {
                return false;
            }
        }

        public List<Filter> Filters { get; set; }

        public Func<dynamic, object> Function { get; set; }

        public static FinalFunctionList operator |(FinalFunction function, FinalFunction other)
        {
            return new FinalFunctionList { function, other };
        }

        public static FinalFunctionList operator |(FinalFunctionList functions, FinalFunction other)
        {
            functions.Add(other);
            return functions;
        }

        public bool MatchesFilter(IDictionary<string, object> environment)
        {
            return this.Filters.All(o => o.IsMatch(environment));
        }

        public class ExecuteAndContinue
        {
        }
    }
}
