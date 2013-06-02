namespace Superscribe.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    using Superscribe.Models;

    public class RouteWalker
    {
        private readonly IEnumerable<SuperscribeState> baseStates;

        public RouteWalker(IEnumerable<SuperscribeState> baseStates)
        {
            this.baseStates = baseStates;
        }

        public bool ExtraneousMatch { get; private set; }

        public bool IncompleteMatch { get; private set; }

        public bool ParamConversionError { get; private set; }

        public string RemainingRoute { get; private set; }

        public void WalkRoute(string route, RouteData info)
        {
            this.RemainingRoute = route;
            this.WalkRoute(info, baseStates);
        }

        public void WalkRoute(RouteData info, IEnumerable<SuperscribeState> states)
        {
            var segment = string.Empty;

            while (string.IsNullOrEmpty(segment))
            {
                var slashIndex = this.RemainingRoute.IndexOf("/", System.StringComparison.Ordinal);

                if (slashIndex >= 0)
                {
                    segment = this.RemainingRoute.Substring(0, slashIndex);
                    this.RemainingRoute = this.RemainingRoute.Substring(slashIndex + 1);
                }
                else
                {
                    segment = this.RemainingRoute;
                    this.RemainingRoute = null;
                    break;
                }
            }

            var match = MatchState(segment, states);
            var controllerState = match as ControllerState;
            if (controllerState != null)
            {
                info.ControllerName = !string.IsNullOrEmpty(controllerState.ControllerName) ? controllerState.ControllerName : segment;
            }

            var actionState = match as ActionState;
            if (actionState != null)
            {
                info.ActionName = segment;
            }

            var paramState = match as ParamState;
            if (paramState != null)
            {
                object value;
                var success = paramState.TryParse(segment, out value);

                if (success)
                {
                    info.Parameters.Add(paramState.Name, value);
                }
                else
                {
                    this.ParamConversionError = true;
                }
            }

            var commandState = match as CommandState;
            if (commandState != null)
            {
                commandState.Command(info);
            }

            if (string.IsNullOrEmpty(this.RemainingRoute) && match != null && match.Transitions.Any()
                && match.Transitions.All(o => !o.IsOptional))
            {
                IncompleteMatch = true;
            }

            if (match == null)
            {
                ExtraneousMatch = true;
            }

            if (!string.IsNullOrEmpty(this.RemainingRoute) && match != null)
            {
                WalkRoute(info, match.Transitions);
            }
        }

        private static SuperscribeState MatchState(string segment, IEnumerable<SuperscribeState> states)
        {
            return states.FirstOrDefault(o => o.IsMatch(segment));
        }
    }
}
