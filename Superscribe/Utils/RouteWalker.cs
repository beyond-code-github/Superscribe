namespace Superscribe.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Superscribe.Models;

    public class RouteWalker<T> where T : IRouteData
    {
        private readonly SuperscribeNode baseNode;

        public RouteWalker(SuperscribeNode baseNode)
        {
            this.baseNode = baseNode;
        }

        public bool ExtraneousMatch { get; private set; }

        public bool IncompleteMatch { get; private set; }

        public bool ParamConversionError { get; private set; }

        public string Method { get; set; }

        public Queue<string> RemainingSegments { get; private set; }

        public void WalkRoute(string route, string method, T info)
        {
            string querystring = null;
            this.Method = method;

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
                        info.Parameters[operands[0]] = operands[1];
                    }
                }
            }

            this.RemainingSegments = new Queue<string>(route.Split('/'));
            this.WalkRoute(info, this.baseNode);
        }

        public string PeekNextSegment()
        {
            if (this.RemainingSegments.Any())
            {
                return this.RemainingSegments.Peek();
            }

            return string.Empty;
        }

        public void WalkRoute(T info, SuperscribeNode match)
        {
            Func<T, object> onComplete = null;
            while (match != null)
            {
                if (match.ActionFunction != null)
                {
                    match.ActionFunction(info, this.PeekNextSegment());
                }

                if (!(match is NonConsumingNode))
                {
                    if (this.RemainingSegments.Any())
                    {
                        this.RemainingSegments.Dequeue();
                    }
                }

                if (match.FinalFunctions.Count > 0)
                {
                    var function = match.FinalFunctions.FirstOrDefault(o => o.Method == this.Method)
                                   ?? match.FinalFunctions.FirstOrDefault(o => string.IsNullOrEmpty(o.Method));

                    if (function != null)
                    {
                        onComplete = o => function.Function(o);
                    }
                }

                var nextMatch = this.FindNextMatch(info, this.PeekNextSegment(), match.Edges);
                if (nextMatch == null
                    && !match.FinalFunctions.Any(o => string.IsNullOrEmpty(o.Method) || o.Method == this.Method)
                    && match.Edges.Any()
                    && match.Edges.All(o => !(o.IsOptional || o is NonConsumingNode)))
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
                info.Response = onComplete(info);
            }
        }

        private SuperscribeNode FindNextMatch(T info, string segment, IEnumerable<SuperscribeNode> states)
        {
            return !string.IsNullOrEmpty(segment) ?
                states.FirstOrDefault(o => o.ActivationFunction(info, segment) && (!o.AllowedMethods.Any() || o.AllowedMethods.Contains(this.Method)))
                : null;
        }
    }
}
