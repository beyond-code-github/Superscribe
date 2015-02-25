namespace Superscribe.Engine
{
    public interface IModuleRouteData : IRouteData
    {
        T Bind<T>() where T : class;

        T Require<T>() where T : class;
    }
}
