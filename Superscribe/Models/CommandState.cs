namespace Superscribe.Models
{
    using System;

    public class CommandState : SuperscribeState
    {
        public CommandState(string pattern, Action<RouteData, string> command)
        {
            this.Command = command;
            this.Template = pattern;
        }
    }
}
