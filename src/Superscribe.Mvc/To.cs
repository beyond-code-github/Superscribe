namespace Superscribe.WebApi
{
    using System;

    using Superscribe.Engine;
    using Superscribe.Models;

    public static class To
    {
        public static Func<dynamic, object> Action(string name)
        {
            return o =>
                {
                    var routeData = o as IRouteData;
                    routeData.Environment[Constants.ActionNamePropertyKey] = name;
                    return new FinalFunction.ExecuteAndContinue();
                };
        }
    }
}
