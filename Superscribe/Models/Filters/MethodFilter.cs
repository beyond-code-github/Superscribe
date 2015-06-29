namespace Superscribe.Models.Filters
{
    using System.Collections.Generic;

    public class MethodFilter : Filter
    {
        private readonly string method;

        public MethodFilter(string method)
        {
            this.method = method;
        }

        public override bool IsMatch(IDictionary<string, object> environment)
        {
            return this.method == environment[Constants.RequestMethodEnvironmentKey].ToString();
        }
    }
}
