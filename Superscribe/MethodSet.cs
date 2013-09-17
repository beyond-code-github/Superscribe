namespace Superscribe
{
    using System;

    using Superscribe.Models;

    public class MethodSet
    {
        public Action<RouteData> Get { get; set; }

        public Action<RouteData> Post { get; set; }

        public Action<RouteData> Put { get; set; }

        public Action<RouteData> Patch { get; set; }

        public Action<RouteData> Delete { get; set; }
    }
}
