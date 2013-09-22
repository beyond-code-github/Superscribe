namespace Superscribe.Models
{
    public interface IRouteData
    {
        dynamic Parameters { get; set; }

        object Response { get; set; }

        T Bind<T>() where T : class;

        T Require<T>() where T : class;
    }
}