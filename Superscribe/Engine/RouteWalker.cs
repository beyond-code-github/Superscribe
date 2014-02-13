namespace Superscribe.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Superscribe.Cache;
    using Superscribe.Models;

    public class RouteWalker : IRouteWalker
    {
        private readonly GraphNode baseNode;

        public RouteWalker(GraphNode baseNode)
        {
            this.baseNode = baseNode;
        }

        public bool ExtraneousMatch { get; private set; }

        public bool IncompleteMatch { get; private set; }

        public bool ParamConversionError { get; private set; }

        public bool FinalFunctionExecuted { get; private set; }

        public string Route { get; set; }

        public string Method { get; set; }

        public Queue<string> RemainingSegments { get; private set; }

        public RouteData WalkRoute(string route, string method, RouteData info)
        {
            string querystring = null;
            this.Method = method;
            this.Route = route;

            CacheEntry<RouteData> cacheEntry;
            if (RouteCache.TryGet(method + "-" + route, out cacheEntry))
            {
                info = cacheEntry.Info;
                info.Response = cacheEntry.OnComplete(info);

                FinalFunctionExecuted = true;

                return info;
            }

            var parts = route.Split('?');
            if (parts.Length > 0)
            {
                route = parts[0];
            }

            if (parts.Length > 1)
            {
                querystring = parts[1];
            }

            if (!string.IsNullOrEmpty(querystring))
            {
                var queries = querystring.Split('&');
                foreach (var query in queries)
                {
                    if (query.Contains("="))
                    {
                        var operands = query.Split('=');
                        info.Parameters[operands[0]] = Uri.UnescapeDataString(operands[1]);
                    }
                }
            }

            this.RemainingSegments = new Queue<string>(route.Split('/'));
            this.WalkRoute(info, this.baseNode);

            return info;
        }

        public string PeekNextSegment()
        {
            if (this.RemainingSegments.Any())
            {
                return this.RemainingSegments.Peek();
            }

            return string.Empty;
        }

        public void WalkRoute(RouteData info, GraphNode match)
        {
            FinalFunction onComplete = null;
            while (match != null)
            {
                if (match.ActionFunction != null)
                {
                    match.ActionFunction(info, this.PeekNextSegment());
                }

                if (this.RemainingSegments.Any())
                {
                    this.RemainingSegments.Dequeue();
                }
                
                if (onComplete != null)
                {
                    if (onComplete.IsExclusive)
                    {
                        onComplete = null;
                    }
                }

                if (match.FinalFunctions.Count > 0)
                {
                    var function = match.FinalFunctions.FirstOrDefault(o => o.Method == this.Method)
                                   ?? match.FinalFunctions.FirstOrDefault(o => string.IsNullOrEmpty(o.Method));

                    if (function != null)
                    {
                        onComplete = function;
                    }
                }

                var nextMatch = this.FindNextMatch(info, this.PeekNextSegment(), match.Edges);
                if (nextMatch == null
                    && !match.FinalFunctions.Any(o => string.IsNullOrEmpty(o.Method) || o.Method == this.Method)
                    && match.Edges.Any()
                    && match.Edges.All(o => !(o.IsOptional)))
                {
                    this.IncompleteMatch = true;
                    return;
                }

                match = nextMatch;
            }

            if (this.RemainingSegments.Any(o => !string.IsNullOrEmpty(o)))
            {
                this.ExtraneousMatch = true;
                return;
            }

            if (onComplete != null)
            {
                RouteCache.Store(this.Method + "-" + this.Route, new CacheEntry<RouteData> { Info = info, OnComplete = onComplete.Function });
                info.Response = onComplete.Function(info);
                FinalFunctionExecuted = true;
            }
        }

        private GraphNode FindNextMatch(RouteData info, string segment, IEnumerable<GraphNode> states)
        {
            return !string.IsNullOrEmpty(segment) ?
                states.FirstOrDefault(o => o.ActivationFunction(info, segment) && (!o.AllowedMethods.Any() || o.AllowedMethods.Contains(this.Method)))
                : null;
        }
    }
}
