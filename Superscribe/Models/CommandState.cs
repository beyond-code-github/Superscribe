namespace Superscribe.Models
{
    using System;

    public class CommandState : SuperscribeState
    {
        public Action<RouteData> Command { get; set; }

        public CommandState(string pattern, Action<RouteData> command)
        {
            Command = command;
            this.Template = pattern;
        }
    }
}
