﻿namespace Superscribe.Engine
{
    using System;

    using Superscribe.Models;
    using Superscribe.Models.Filters;

    public interface IRouteEngine
    {
        GraphNode Base { get; }

        SuperscribeOptions Config { get; }

        IRouteWalker Walker();

        GraphNode Route(string routeTemplate);

        GraphNode Route(string routeTemplate, Func<dynamic, object> func);

        GraphNode Route(string routeTemplate, Func<dynamic, object> func, params Filter[] filters);

        GraphNode Route(GraphNode node);

        GraphNode Route(GraphNode config, Func<dynamic, object> func);

        GraphNode Route(GraphNode config, Func<dynamic, object> func, params Filter[] filters);

        GraphNode Route(Func<RouteGlue, GraphNode> config);

        GraphNode Route(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func);

        GraphNode Route(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func, params Filter[] filters);
        
        GraphNode Get(string routeTemplate, Func<dynamic, object> func);

        GraphNode Get(GraphNode config, Func<dynamic, object> func);
        
        GraphNode Get(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func);

        GraphNode Get(string routeTemplate, Func<dynamic, object> func, params Filter[] filters);

        GraphNode Get(GraphNode config, Func<dynamic, object> func, params Filter[] filters);

        GraphNode Get(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func, params Filter[] filters);
        
        GraphNode Post(string routeTemplate, Func<dynamic, object> func);
        
        GraphNode Post(GraphNode config, Func<dynamic, object> func);
        
        GraphNode Post(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func);

        GraphNode Post(string routeTemplate, Func<dynamic, object> func, params Filter[] filters);

        GraphNode Post(GraphNode config, Func<dynamic, object> func, params Filter[] filters);

        GraphNode Post(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func, params Filter[] filters);
        
        GraphNode Put(string routeTemplate, Func<dynamic, object> func);
        
        GraphNode Put(GraphNode config, Func<dynamic, object> func);

        GraphNode Put(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func);

        GraphNode Put(GraphNode config, Func<dynamic, object> func, params Filter[] filters);

        GraphNode Put(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func, params Filter[] filters);
        
        GraphNode Patch(string routeTemplate, Func<dynamic, object> func);
        
        GraphNode Patch(GraphNode config, Func<dynamic, object> func);
        
        GraphNode Patch(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func);

        GraphNode Patch(string routeTemplate, Func<dynamic, object> func, params Filter[] filters);

        GraphNode Patch(GraphNode config, Func<dynamic, object> func, params Filter[] filters);

        GraphNode Patch(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func, params Filter[] filters);
        
        GraphNode Delete(string routeTemplate, Func<dynamic, object> func);
        
        GraphNode Delete(GraphNode config, Func<dynamic, object> func);
        
        GraphNode Delete(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func);

        GraphNode Delete(string routeTemplate, Func<dynamic, object> func, params Filter[] filters);

        GraphNode Delete(GraphNode config, Func<dynamic, object> func, params Filter[] filters);

        GraphNode Delete(Func<RouteGlue, GraphNode> config, Func<dynamic, object> func, params Filter[] filters);
        
    }
}
