namespace Superscribe.Models
{
    using System;

    public class CommandNode : SuperscribeNode
    {
        public CommandNode(string pattern, Action<RouteData, string> actionFunction)
        {
            this.ActionFunction = actionFunction;
            this.Template = pattern;
        }
    }
}
