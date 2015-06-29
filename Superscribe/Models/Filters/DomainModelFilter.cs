namespace Superscribe.Models.Filters
{
    using System.Collections.Generic;

    public class DomainModelFilter : Filter
    {
        private string domainModel;

        public DomainModelFilter(string domainModel)
        {
            this.domainModel = domainModel;
        }

        public override bool IsMatch(IDictionary<string, object> environment)
        {
            throw new System.NotImplementedException();
        }
    }
}
