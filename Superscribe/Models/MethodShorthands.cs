namespace Superscribe.Models
{
    using System;

    public class ʃGet
    {
        public static explicit operator ʃGet(Action<RouteData> action)
        {
            return new ʃGet();
        }
    }
}
