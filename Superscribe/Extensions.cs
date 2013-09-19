namespace Superscribe
{
    using System;

    using Superscribe.Models;

    /// <summary>
    /// Extension methods to aid when defining superscribe states that match url segments
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Matches the given pattern and sets ControllerName
        /// </summary>
        public static ControllerNode Controller(this string pattern)
        {
            return new ControllerNode { Template = pattern };
        }

        /// <summary>
        /// Matches the given pattern then sets controller name to another value
        /// </summary>
        public static ControllerNode Controller(this string pattern, string controllerName)
        {
            return new ControllerNode { Template = pattern, ControllerName = controllerName };
        }

        /// <summary>
        /// Matches the given pattern then sets ActionName
        /// </summary>
        public static ActionNode Action(this string pattern)
        {
            return new ActionNode { Template = pattern };
        }

        /// <summary>
        /// Matches the given pattern then sets ActionName to another value
        /// </summary>
        public static ActionNode Action(this string pattern, string actionName)
        {
            return new ActionNode { Template = pattern, ActionName = actionName };
        }

        /// <summary>
        /// Matches an integer parameter and adds the name and value to the Parameters dictionary
        /// </summary>
        public static ParamNode<int> Int(this string name)
        {
            return Superscribe.ʃ.Int(name);
        }

        /// <summary>
        /// Matches a boolean parameter and adds the name and value to the Parameters dictionary
        /// </summary>
        public static ParamNode<bool> Bool(this string name)
        {
            return Superscribe.ʃ.Bool(name);
        }

        /// <summary>
        /// Matches a string parameter and adds the name and value to the Parameters dictionary
        /// </summary>
        public static ParamNode<string> String(this string name)
        {
            return Superscribe.ʃ.String(name);
        }

        /// <summary>
        /// Matches the given pattern and then executes the given command
        /// </summary>
        public static CommandNode ʃ(this string pattern, Action<dynamic, string> command)
        {
            return new CommandNode(pattern, command);
        }

        public static SuperscribeNode ʃ(this string value)
        {
            return Superscribe.ʃ.Constant(value);
        }
    }
}
