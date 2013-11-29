namespace Superscribe.Testing.Http
{
    using Superscribe.Models;
    using Superscribe.Utils;

    public class RouteData : IRouteData
    {
        public RouteData()
        {
            this.Parameters = new DynamicDictionary();
        }
        
        public dynamic Parameters { get; set; }
        
        public object Response { get; set; }

        public bool ParamConversionError { get; set; }
        
        public T Bind<T>() where T : class
        {
            throw new System.NotImplementedException();
        }

        public T Require<T>() where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}
