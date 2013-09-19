namespace Superscribe.Models
{
    using System;

    public class CommandNode : SuperscribeNode
    {
        public CommandNode(string pattern, Action<dynamic, string> actionFunction)
        {
            this.ActionFunction = actionFunction;
            this.Template = pattern;
        }
    }
}
