namespace Superscribe.Models.Filters
{
    using System.Collections.Generic;

    public abstract class Filter
    {
        public abstract bool IsMatch(IDictionary<string, object> environment);
    }
}
