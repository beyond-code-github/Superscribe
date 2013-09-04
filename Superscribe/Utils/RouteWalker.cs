namespace Superscribe.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Superscribe.Models;

    public class RouteWalker
    {
        private readonly SuperscribeNode baseNode;

        public RouteWalker(SuperscribeNode baseNode)
        {
            this.baseNode = baseNode;
        }

        public bool ExtraneousMatch { get; private set; }

        public bool IncompleteMatch { get; private set; }

        public bool ParamConversionError { get; private set; }

        public Queue<string> RemainingSegments { get; private set; }

        public void WalkRoute(string route, RouteData info)
        {
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

        public void WalkRoute(RouteData info, SuperscribeNode match)
        {
            Action<RouteData> onComplete = null;
            while (match != null)
            {
                if (match.ActionFunction != null)
                {
                    match.ActionFunction(info, this.PeekNextSegment());
                }

                if (!(match is NonConsumingNode))
                {
                    this.RemainingSegments.Dequeue();
                }

                if (match.FinalFunction != null)
                {
                    onComplete = match.FinalFunction;
                }

                var nextMatch = FindNextMatch(this.PeekNextSegment(), match.Edges);
                if (nextMatch == null
                    && match.FinalFunction == null
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
                onComplete(info);
            }
        }

        private static SuperscribeNode FindNextMatch(string segment, IEnumerable<SuperscribeNode> states)
        {
            return states.FirstOrDefault(o => o.ActivationFunction(segment));
        }
    }
}
