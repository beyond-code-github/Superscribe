namespace Superscribe.Tests.WebApi.Owin
{
    using System.Collections.Generic;

    public interface IRepository
    {
        string Values();
    }

    public class Repository : IRepository
    {
        public string Values()
        {
            return "value1";
        }
    }

    public class ApiRepository : IRepository
    {
        public string Values()
        {
            return "value3";
        }
    }
}
