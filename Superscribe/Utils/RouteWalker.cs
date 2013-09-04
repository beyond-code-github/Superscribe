namespace Superscribe.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Superscribe.Models;

    public class RouteWalker
    {
        private readonly SuperscribeState baseState;

        public RouteWalker(SuperscribeState baseState)
        {
            this.baseState = baseState;
        }

        public bool ExtraneousMatch { get; private set; }

        public bool IncompleteMatch { get; private set; }

        public bool ParamConversionError { get; private set; }

        public Queue<string> RemainingSegments { get; private set; }

        public void WalkRoute(string route, RouteData info)
        {
            this.RemainingSegments = new Queue<string>(route.Split('/'));
            this.WalkRoute(info, baseState);
        }

        public string PeekNextSegment()
        {
            if (this.RemainingSegments.Any())
            {
                return this.RemainingSegments.Peek();
            }

            return string.Empty;
        }

        public void WalkRoute(RouteData info, SuperscribeState match)
        {
            Action<RouteData> onComplete = null;
            while (match != null)
            {
                if (match.Command != null)
                {
                    match.Command(info, this.PeekNextSegment());
                }

                if (!(match is NonConsumingState))
                {
                    this.RemainingSegments.Dequeue();
                }

                if (match.OnComplete != null)
                {
                    onComplete = match.OnComplete;
                }

                var nextMatch = MatchState(this.PeekNextSegment(), match.Transitions);
                if (nextMatch == null
                    && match.OnComplete == null
                    && match.Transitions.Any()
                    && match.Transitions.All(o => !(o.IsOptional || o is NonConsumingState)))
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

        private static SuperscribeState MatchState(string segment, IEnumerable<SuperscribeState> states)
        {
            return states.FirstOrDefault(o => o.IsMatch(segment));
        }
    }
}
