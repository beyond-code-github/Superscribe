namespace Superscribe.Engine
{
    using System;

    using Superscribe.Models;
    using Superscribe.Utils;

    public interface IRouteEngine
    {
        GraphNode Base { get; }

        GraphNode Delete(GraphNode config);

        IRouteWalker Walker();

        /// <summary>
        /// Define a partial route or attach a route to Superscribe's Base State
        /// </summary>
        /// <param name="config">Route configuration function</param>
        /// <returns>The last state in the chain</returns>
        GraphNode Route(Func<GraphNode> config);

        /// <summary>
        /// Define a partial route or attach a route to Superscribe's Base State
        /// </summary>
        /// <param name="config">Route configuration function</param>
        /// <returns>The last state in the chain</returns>
        GraphNode Route(Func<RouteGlue, GraphNode> config);

        GraphNode Route(Func<RouteGlue, GraphNode> config, Func<object, object> func);

        GraphNode Route(Func<RouteGlue, SuperList> config);

        GraphNode Route(Func<RouteGlue, Func<object, object>> config);

        GraphNode Route(GraphNode config);

        GraphNode Route(string routeTemplate);

        GraphNode Route(string routeTemplate, Func<object, object> func);

        GraphNode Route(GraphNode config, Func<object, object> func);

        GraphNode Get(Func<RouteGlue, GraphNode> config);

        GraphNode Get(GraphNode leaf);

        GraphNode Post(Func<RouteGlue, GraphNode> config);

        GraphNode Post(GraphNode leaf);

        GraphNode Put(Func<RouteGlue, GraphNode> config);

        GraphNode Put(GraphNode leaf);

        GraphNode Patch(Func<RouteGlue, GraphNode> config);

        GraphNode Patch(GraphNode leaf);

        GraphNode Delete(Func<RouteGlue, GraphNode> config);
    }
}
