namespace Superscribe.Engine
{
    using System;

    using Superscribe.Models;
    using Superscribe.Utils;

    public interface IRouteEngine
    {
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

        GraphNode Route(Func<RouteGlue, SuperList> config);

        GraphNode Route(Func<RouteGlue, Func<object, object>> config);

        void Route(GraphNode config);

        void Route(string config, Func<object, object> func);

        void Route(GraphNode config, Func<object, object> func);

        GraphNode Get(Func<RouteGlue, GraphNode> config);

        void Get(GraphNode config);

        GraphNode Post(Func<RouteGlue, GraphNode> config);

        void Post(GraphNode config);

        GraphNode Put(Func<RouteGlue, GraphNode> config);

        void Put(GraphNode config);
        
        GraphNode Patch(Func<RouteGlue, GraphNode> config);

        void Patch(GraphNode config);

        GraphNode Delete(Func<RouteGlue, GraphNode> config);

        void Delete(GraphNode config);

        IRouteWalker Walker();
        
        GraphNode Base { get; }
    }
}
