namespace Superscribe.Utils
{
    using System;
    using System.Text.RegularExpressions;

    using global::Superscribe.Models;

    public static class Extensions
    {
        public static ControllerState Controller(this string pattern)
        {
            return new ControllerState { Template = pattern };
        }

        public static ControllerState Controller(this string pattern, string controllerName)
        {
            return new ControllerState { Template = pattern, ControllerName = controllerName };
        }

        public static ActionState Action(this string pattern)
        {
            return new ActionState { Template = pattern };
        }

        public static ParamState<int> Int(this string name)
        {
            return new ParamState<int>(name) { Pattern = new Regex("[0-9]+") };
        }

        public static CommandState ʃ(this string pattern, Action<WebApiInfo> command)
        {
            return new CommandState(pattern, command);
        }
    }
}
