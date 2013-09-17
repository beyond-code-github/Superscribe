namespace Superscribe
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Superscribe.Models;

    /// <summary>
    /// Superscribe helper class
    /// </summary>
    public class ʃ
    {
        public static SuperscribeNode Base = new SuperscribeNode();

        #region Static Methods

        /// <summary>
        /// Matches any valid identifier and sets ControllerName
        /// </summary>
        public static ControllerNode Controller
        {
            get
            {
                return new ControllerNode { Pattern = new Regex("([a-z]|[A-Z]|[0-9])+", RegexOptions.Compiled) };
            }
        }

        /// <summary>
        /// Matches any valid identifier and sets ActionName
        /// </summary>
        public static ActionNode Action
        {
            get
            {
                return new ActionNode { Pattern = new Regex("([a-z]|[A-Z]|[0-9])+", RegexOptions.Compiled) };
            }
        }
        
        public static object ʅ
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Matches a string constant and performs no action
        /// </summary>
        /// <param name="value">Constant value to match</param>
        public static ConstantNode Constant(string value)
        {
            return new ConstantNode(value);
        }

        /// <summary>
        /// Matches a string constant and performs a custom action
        /// </summary>
        /// <param name="value">Constant value to match</param>
        /// <param name="configure">Action to execute</param>
        public static ConstantNode Constant(string value, Action<ConstantNode> configure)
        {
            var result = new ConstantNode(value);
            configure(result);
            return result;
        }

        /// <summary>
        /// Matches an integer and adds name and value to the parameters dictionary
        /// </summary>
        /// <param name="name">Parameter name</param>
        public static ʃInt Int(string name)
        {
            return new ʃInt(name) { Pattern = new Regex("[0-9]+", RegexOptions.Compiled) };
        }

        /// <summary>
        /// Matches an integer and adds name and value to the parameters dictionary
        /// </summary>
        /// <param name="name">Parameter name</param>
        public static ʃLong Long(string name)
        {
            return new ʃLong(name) { Pattern = new Regex("[0-9]+", RegexOptions.Compiled) };
        }

        /// <summary>
        /// Matches a boolean and adds the name and value to the parameters dictionary
        /// </summary>
        /// <param name="name">Parameter name</param>
        public static ʃBool Bool(string name)
        {
            return new ʃBool(name) { Pattern = new Regex("(true|false)", RegexOptions.Compiled) };
        }

        /// <summary>
        /// Matches a string and adds the name and value to the parameters dictionary
        /// </summary>
        /// <param name="name">Parameter name</param>
        public static ʃString String(string name)
        {
            return new ʃString(name) { Pattern = new Regex("", RegexOptions.Compiled) };
        }

        /// <summary>
        /// Define a partial route or attach a route to Superscribe's Base State
        /// </summary>
        /// <param name="config">Route configuration function</param>
        /// <returns>The last state in the chain</returns>
        public static SuperscribeNode Route(Func<SuperscribeNode, SuperscribeNode> config)
        {
            var leaf = config(Base);
            leaf.AddAllowedMethod("GET");

            return leaf;
        }

        /// <summary>
        /// Define a partial route or attach a route to Superscribe's Base State
        /// </summary>
        /// <param name="config">Route configuration function</param>
        /// <returns>The last state in the chain</returns>
        public static SuperscribeNode Route(Func<SuperscribeNode, RouteGlue, SuperscribeNode> config)
        {
            var leaf = config(Base, new RouteGlue("GET"));
            leaf.AddAllowedMethod("GET");

            return leaf;
        }

        public static SuperscribeNode Get(Func<SuperscribeNode, SuperscribeNode> config)
        {
            var leaf = config(Base);
            leaf.AddAllowedMethod("GET");
            return leaf;
        }

        public static SuperscribeNode Get(Func<SuperscribeNode, RouteGlue, SuperscribeNode> config)
        {
            var leaf = config(Base, new RouteGlue("GET"));
            leaf.AddAllowedMethod("GET");
            return leaf;
        }

        public static SuperscribeNode Post(Func<SuperscribeNode, SuperscribeNode> config)
        {
            var leaf = config(Base);
            leaf.AddAllowedMethod("POST");
            return leaf;
        }

        public static SuperscribeNode Post(Func<SuperscribeNode, RouteGlue, SuperscribeNode> config)
        {
            var leaf = config(Base, new RouteGlue("POST"));
            leaf.AddAllowedMethod("POST");
            return leaf;
        }

        public static SuperscribeNode Put(Func<SuperscribeNode, SuperscribeNode> config)
        {
            var leaf = config(Base);
            leaf.AddAllowedMethod("PUT");
            return leaf;
        }

        public static SuperscribeNode Put(Func<SuperscribeNode, RouteGlue, SuperscribeNode> config)
        {
            var leaf = config(Base, new RouteGlue("PUT"));
            leaf.AddAllowedMethod("PUT");
            return leaf;
        }

        public static SuperscribeNode Patch(Func<SuperscribeNode, SuperscribeNode> config)
        {
            var leaf = config(Base);
            leaf.AddAllowedMethod("PATCH");
            return leaf;
        }

        public static SuperscribeNode Patch(Func<SuperscribeNode, RouteGlue, SuperscribeNode> config)
        {
            var leaf = config(Base, new RouteGlue("PATCH"));
            leaf.AddAllowedMethod("PATCH");
            return leaf;
        }

        public static SuperscribeNode Delete(Func<SuperscribeNode, SuperscribeNode> config)
        {
            var leaf = config(Base);
            leaf.AddAllowedMethod("DELETE");
            return leaf;
        }

        public static SuperscribeNode Delete(Func<SuperscribeNode, RouteGlue, SuperscribeNode> config)
        {
            var leaf = config(Base, new RouteGlue("DELETE"));
            leaf.AddAllowedMethod("DELETE");
            return leaf;
        }

        #endregion

        public static void Reset()
        {
            Base = new SuperscribeNode();
        }
    }
}
