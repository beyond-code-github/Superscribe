namespace Superscribe
{
    using System;
    using System.Text.RegularExpressions;

    using Superscribe.Models;

    /// <summary>
    /// Superscribe helper class
    /// </summary>
    public class ʃ
    {
        public static SuperscribeState Base = new SuperscribeState();

        #region Static Methods

        /// <summary>
        /// Matches any valid identifier and sets ControllerName
        /// </summary>
        public static ControllerState Controller
        {
            get
            {
                return new ControllerState { Pattern = new Regex("([a-z]|[A-Z]|[0-9])+", RegexOptions.Compiled) };
            }
        }

        /// <summary>
        /// Matches any valid identifier and sets ActionName
        /// </summary>
        public static ActionState Action
        {
            get
            {
                return new ActionState { Pattern = new Regex("([a-z]|[A-Z]|[0-9])+", RegexOptions.Compiled) };
            }
        }

        /// <summary>
        /// Matches a string constant and performs no action
        /// </summary>
        /// <param name="value">Constant value to match</param>
        public static ConstantState Constant(string value)
        {
            return new ConstantState(value);
        }

        /// <summary>
        /// Matches a string constant and performs a custom action
        /// </summary>
        /// <param name="value">Constant value to match</param>
        /// <param name="configure">Action to execute</param>
        public static ConstantState Constant(string value, Action<ConstantState> configure)
        {
            var result = new ConstantState(value);
            configure(result);
            return result;
        }

        /// <summary>
        /// Matches an integer and adds name and value to the parameters dictionary
        /// </summary>
        /// <param name="name">Parameter name</param>
        public static ParamState<int> Int(string name)
        {
            return new ParamState<int>(name) { Pattern = new Regex("[0-9]+", RegexOptions.Compiled) };
        }

        /// <summary>
        /// Matches a boolean and adds the name and value to the parameters dictionary
        /// </summary>
        /// <param name="name">Parameter name</param>
        public static ParamState<bool> Bool(string name)
        {
            return new ParamState<bool>(name) { Pattern = new Regex("(true|false)", RegexOptions.Compiled) };
        }

        /// <summary>
        /// Matches a string and adds the name and value to the parameters dictionary
        /// </summary>
        /// <param name="name">Parameter name</param>
        public static ParamState<string> String(string name)
        {
            return new ParamState<string>(name) { Pattern = new Regex("", RegexOptions.Compiled) };
        }

        /// <summary>
        /// Define a partial route or attach a route to Superscribe's Base State
        /// </summary>
        /// <param name="config">Route configuration function</param>
        /// <returns>The last state in the chain</returns>
        public static SuperscribeState Route(Func<SuperscribeState, SuperscribeState> config)
        {
            return config(Base);
        }

        #endregion
    }
}
