namespace Superscribe.WebApi
{
    using System.Net.Http;

    using Superscribe.Engine;
    using Superscribe.WebApi.Engine;
    using Superscribe.WebApi.Models;

    /// <summary>
    /// Extension methods to aid when defining superscribe states that match url segments
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Matches the given pattern and sets ControllerName
        /// </summary>
        public static ControllerNode Controller(this string pattern)
        {
            return new ControllerNode(pattern);
        }

        /// <summary>
        /// Matches the given pattern then sets controller name to another value
        /// </summary>
        public static ControllerNode Controller(this string pattern, string controllerName)
        {
            return new ControllerNode(pattern) { ControllerName = controllerName };
        }

        /// <summary>
        /// Matches the given pattern then sets ActionName
        /// </summary>
        public static ActionNode Action(this string pattern)
        {
            return new ActionNode { Template = pattern };
        }

        /// <summary>
        /// Matches the given pattern then sets ActionName to another value
        /// </summary>
        public static ActionNode Action(this string pattern, string actionName)
        {
            return new ActionNode { Template = pattern, ActionName = actionName };
        }

        public static IRouteWalker GetRouteWalker(this HttpRequestMessage request)
        {
            return request.GetDependencyScope().GetService(typeof(IRouteWalker)) as IRouteWalker;
        }

        public static IWebApiRouteDataProvider GetRouteDataProvider(this HttpRequestMessage request)
        {
            return request.GetDependencyScope().GetService(typeof(IWebApiRouteDataProvider)) as IWebApiRouteDataProvider;

        }
    }
}
