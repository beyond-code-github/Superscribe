namespace Superscribe
{
    using Superscribe.Models.Filters;
    
    public static class When
    {
        public static Filter DomainModel(string value)
        {
            return new DomainModelFilter(value);
        }
    }
}
