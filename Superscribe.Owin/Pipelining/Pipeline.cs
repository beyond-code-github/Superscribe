namespace Superscribe.Owin.Pipelining
{
    public class Pipeline
    {
        public static OwinNodeFuture Action<T>()
        {
            return new OwinNodeFuture(typeof(T));
        }
    }
}
