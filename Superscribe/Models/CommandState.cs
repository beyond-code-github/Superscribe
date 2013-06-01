namespace Superscribe.Models
{
    using System;
    using System.Text.RegularExpressions;

    public class CommandState : ʃ
    {
        public Action<WebApiInfo> Command { get; set; }

        public CommandState(string pattern, Action<WebApiInfo> command)
        {
            Command = command;
            this.Template = pattern;
        }
    }
}
