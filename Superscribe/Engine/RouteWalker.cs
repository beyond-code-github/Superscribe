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

        private readonly IRouteCache routeCache;

        public RouteWalker(GraphNode baseNode, IRouteCache routeCache)
        {
            this.baseNode = baseNode;
            this.routeCache = routeCache;
        }

        public bool ParamConversionError { get; private set; }

        public IDictionary<string, object> Environment { get; set; }

        public Queue<string> RemainingSegments { get; private set; }

        private string Route
        {
            get
            {
                return this.Environment[Constants.RequestPathEnvironmentKey].ToString();
            }
        }

        private string Querystring
        {
            get
            {
                return this.Environment[Constants.RequestQuerystringEnvironmentKey].ToString();
            }
        }

        private string Method
        {
            get
            {
                return this.Environment[Constants.RequestMethodEnvironmentKey].ToString();
            }
        }

        public RouteData WalkRoute(IDictionary<string, object> environment, RouteData info)
        {
            this.Environment = environment;

            CacheEntry<RouteData> cacheEntry;
            if (this.routeCache.TryGet(this.Method + "-" + this.Route, out cacheEntry))
            {
                info = cacheEntry.Info;
                info.Response = cacheEntry.OnComplete(info);

                info.FinalFunctionExecuted = true;

                return info;
            }
            
            if (!string.IsNullOrEmpty(this.Querystring))
            {
                var queries = this.Querystring.Split('&');
                foreach (var query in queries)
                {
                    if (query.Contains("="))
                    {
                        var operands = query.Split('=');
                        info.Parameters[operands[0]] = Uri.UnescapeDataString(operands[1]);
                    }
                }
            }

            this.RemainingSegments = new Queue<string>(this.Route.Split('/'));
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
                foreach (var action in match.ActionFunctions.Values)
                {
                    action(info, this.PeekNextSegment());
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
                    var function = match.FinalFunctions.FirstOrDefault(o => o.MatchesFilter(this.Environment));
                    if (function != null)
                    {
                        onComplete = function;
                    }
                }

                var nextMatch = this.FindNextMatch(info, this.PeekNextSegment(), match.Edges);
                if (nextMatch == null)
                {
                    if (this.HasFinalsButNoneMatchTheCurrentMethod(match))
                    {
                        info.NoMatchingFinalFunction = true;
                        return;
                    }
                }

                match = nextMatch;
            }

            if (this.RemainingSegments.Any(o => !string.IsNullOrEmpty(o)))
            {
                info.ExtraneousMatch = true;
                return;
            }

            if (onComplete != null)
            {
                this.routeCache.Store(this.Method + "-" + this.Route, new CacheEntry<RouteData> { Info = info, OnComplete = onComplete.Function });
                info.Response = onComplete.Function(info);
                info.FinalFunctionExecuted = true;
            }
        }

        private GraphNode FindNextMatch(RouteData info, string segment, IEnumerable<GraphNode> states)
        {
            return !string.IsNullOrEmpty(segment) ?
                states.FirstOrDefault(o => o.ActivationFunction(info, segment))
                : null;
        }

        private bool HasFinalsButNoneMatchTheCurrentMethod(GraphNode match)
        {
            return match.FinalFunctions.Count > 0
                   && !match.FinalFunctions.Any(o => o.MatchesFilter(this.Environment));
        }
    }
}
